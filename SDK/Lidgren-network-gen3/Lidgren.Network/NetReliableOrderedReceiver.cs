using System;

namespace Lidgren.Network
{
	internal sealed class NetReliableOrderedReceiver : NetReceiverChannelBase
	{
		private int m_windowStart;
		private int m_windowSize;
		private NetBitVector m_earlyReceived;
		internal NetIncomingMessage[] m_withheldMessages;

		public NetReliableOrderedReceiver(NetConnection connection, int windowSize)
			: base(connection)
		{
			m_windowSize = windowSize;
			m_withheldMessages = new NetIncomingMessage[windowSize];
			m_earlyReceived = new NetBitVector(windowSize);
		}

		private void AdvanceWindow()
		{
			m_earlyReceived.Set(m_windowStart % m_windowSize, false);
			m_windowStart = (m_windowStart + 1) % NetConstants.NumSequenceNumbers;
		}

		internal override void ReceiveMessage(NetIncomingMessage message)
		{
			int relate = NetUtility.RelativeSequenceNumber(message.m_sequenceNumber, m_windowStart);

			// ack no matter what
			m_connection.QueueAck(message.m_receivedMessageType, message.m_sequenceNumber);

			if (relate == 0) {

        //
        // excellent, right on time
        //
        if (m_connection.m_logMessageReceive) {
          m_peer.LogWarning("Received message #" + message.m_sequenceNumber + " right on time " + m_connection.RemoteEndPoint.Address.ToString() + ":" + m_connection.RemoteEndPoint.Port);
        }
        
				AdvanceWindow();
				m_peer.ReleaseMessage(message);

				// release withheld messages
				int nextSeqNr = (message.m_sequenceNumber + 1) % NetConstants.NumSequenceNumbers;

				while (m_earlyReceived[nextSeqNr % m_windowSize])
				{
					message = m_withheldMessages[nextSeqNr % m_windowSize];
					NetException.Assert(message != null);

					// remove it from withheld messages
					m_withheldMessages[nextSeqNr % m_windowSize] = null;

          //m_peer.LogDebug("Releasing withheld message #" + message + " " + m_connection.RemoteEndPoint.Address.ToString() + ":" + m_connection.RemoteEndPoint.Port);

					m_peer.ReleaseMessage(message);

					AdvanceWindow();
					nextSeqNr++;
				}

				return;
			}

			if (relate < 0)
			{
        if (m_connection.m_logMessageReceive) {
          m_peer.LogWarning("Received message #" + message.m_sequenceNumber + " DROPPING DUPLICATE " + m_connection.RemoteEndPoint.Address.ToString() + ":" + m_connection.RemoteEndPoint.Port);
        }
				// duplicate
				return;
			}

			// relate > 0 = early message
			if (relate > m_windowSize)
			{
				// too early message!
        if (m_connection.m_logMessageReceive) {
          m_peer.LogWarning("Received " + message + " TOO EARLY! Expected " + m_windowStart + " " + m_connection.RemoteEndPoint.Address.ToString() + ":" + m_connection.RemoteEndPoint.Port);
        }
				return;
			}

			m_earlyReceived.Set(message.m_sequenceNumber % m_windowSize, true);
      if (m_connection.m_logMessageReceive) {
        m_peer.LogWarning("Received " + message + " WITHHOLDING, waiting for " + m_windowStart + " " + m_connection.RemoteEndPoint.Address.ToString() + ":" + m_connection.RemoteEndPoint.Port);
      }
			m_withheldMessages[message.m_sequenceNumber % m_windowSize] = message;
		}
	}
}
