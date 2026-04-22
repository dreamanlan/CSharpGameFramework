using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptableFramework
{
    /// <summary>
    /// Put the variable values that are different between the client and the server here
    /// for use by each public module (if it is the logical data required by each module,
    /// do not put it here and write the meter reader independently).
    /// </summary>
    public class GlobalVariables
    {
        public const int c_TotalPreservedRoomCount = 64;
        public const int c_PreservedRoomCountPerThread = 4;

        public bool IsClient
        {
            get { return m_IsClient; }
            set { m_IsClient = value; }
        }
        public bool IsDebug
        {
            get { return m_IsDebug; }
            set { m_IsDebug = value; }
        }
        public bool IsDevelopment
        {
            get { return m_IsDevelopment; }
            set { m_IsDevelopment = value; }
        }
        public bool IsDevice
        {
            get { return m_IsDevice; }
            set { m_IsDevice = value; }
        }
        public bool LoggerEnabled
        {
            get { return m_LoggerEnabled; }
            set { m_LoggerEnabled = value; }
        }

        private static void AddCrypto(string s, string d, Dictionary<string, string> encodeTable, Dictionary<string, string> decodeTable)
        {
            encodeTable.Add(s, d);
            decodeTable.Add(d, s);
        }

        private GlobalVariables()
        {
        }

        private bool m_IsClient = true;
        private bool m_IsDebug = false;
        private bool m_IsDevelopment = false;
        private bool m_IsDevice = false;
        private bool m_LoggerEnabled = true;

        public static bool s_EnableCalculatorLog = false;
        public static bool s_EnableCalculatorDetailLog = false;
        public static bool s_EnableCalculatorOperatorLog = false;
		
        public static GlobalVariables Instance
        {
            get { return s_Instance; }
        }
        private static GlobalVariables s_Instance = new GlobalVariables();
    }
}
