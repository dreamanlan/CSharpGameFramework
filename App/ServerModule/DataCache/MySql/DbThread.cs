using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GameFramework
{
    internal class DbThread : MyServerThread
    {
        internal DbThread(ServerAsyncActionProcessor actionQueue)
            : base(actionQueue)
        {
        }

        protected override void OnTick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastElapsedTickTime != 0) {
                long elapsedTickTime = curTime - m_LastElapsedTickTime;
                if (elapsedTickTime > c_WarningTickTime) {
                    LogSys.Log(LOG_TYPE.MONITOR, "DbThread Tick:{0} {1}", this.Thread.ManagedThreadId, elapsedTickTime);
                }
            }
            m_LastElapsedTickTime = curTime;

            if (m_LastTickTime + c_TickInterval < curTime) {
                m_LastTickTime = curTime;

                DBConn.KeepConnection();
                try {
                    MySqlConnection conn = DBConn.MySqlConn;
                    using (MySqlCommand cmd = new MySqlCommand("select * from DSNodeInfo where 1=2", conn)) {
                        cmd.ExecuteNonQuery();
                    }
                } catch (Exception ex) {
                    LogSys.Log(LOG_TYPE.ERROR, "DbThread.Tick keep connection exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }

            if (m_LastLogTime + 60000 < curTime) {
                m_LastLogTime = curTime;

                DebugPoolCount((string msg) => {
                    LogSys.Log(LOG_TYPE.INFO, "DbThread.ActionQueue {0} {1}", this.Thread.ManagedThreadId, msg);
                });
                LogSys.Log(LOG_TYPE.MONITOR, "DbThread.ActionQueue Current Action {0} {1}", this.Thread.ManagedThreadId, this.CurActionNum);
            }
        }
        protected override void OnQuit()
        {
            DBConn.Close();
        }

        private long m_LastTickTime = 0;
        private const long c_TickInterval = 60000;
        private const long c_WarningTickTime = 1000;
        private long m_LastElapsedTickTime = 0;

        private long m_LastLogTime = 0;
    }
}
