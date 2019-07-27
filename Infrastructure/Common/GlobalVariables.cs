using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    /// <summary>
    /// 这里放客户端与服务器存在差异的变量值，供各公共模块使用（如果是各模块所需的逻辑数据，则不要放在这里，独立写读表器）。
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
        public bool IsDevice
        {
            get { return m_IsDevice; }
            set { m_IsDevice = value; }
        }
        public bool IsPublish
        {
            get { return m_IsPublish; }
            set { m_IsPublish = value; }
        }
        public bool IsIphone4S
        {
            get { return m_IsIphone4S; }
            set { m_IsIphone4S = value; }
        }
        public bool StoryEditorOpen
        {
            get { return m_StoryEditorOpen; }
            set { m_StoryEditorOpen = value; }
        }
        public bool StoryEditorContinue
        {
            get { return m_StoryEditorContinue; }
            set { m_StoryEditorContinue = value; }
        }

        public bool IsStorySkipped
        {
            get { return m_IsStorySkipped; }
            set { m_IsStorySkipped = value; }
        }
        public bool IsStorySpeedup
        {
            get { return m_IsStorySpeedup; }
            set { m_IsStorySpeedup = value; }
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
        private bool m_IsDevice = false;
        private bool m_IsPublish = false;
        private bool m_IsIphone4S = false;

        private bool m_StoryEditorOpen = false;
        private bool m_StoryEditorContinue = false;

        private bool m_IsStorySkipped = false;
        private bool m_IsStorySpeedup = false;
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
