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

        public Dictionary<string, string> EncodeTable
        {
            get { return m_EncodeTable; }
        }
        public Dictionary<string, string> DecodeTable
        {
            get { return m_DecodeTable; }
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
            AddCrypto("skill", "_1_", m_EncodeTable, m_DecodeTable);
            AddCrypto("section", "_2_", m_EncodeTable, m_DecodeTable);
            AddCrypto("initialization", "_3_", m_EncodeTable, m_DecodeTable);
            AddCrypto("onstop", "_4_", m_EncodeTable, m_DecodeTable);
            AddCrypto("oninterrupt", "_5_", m_EncodeTable, m_DecodeTable);
            AddCrypto("story", "_6_", m_EncodeTable, m_DecodeTable);
            AddCrypto("local", "_7_", m_EncodeTable, m_DecodeTable);
            AddCrypto("onmessage", "_8_", m_EncodeTable, m_DecodeTable);
            AddCrypto("foreach", "_9_", m_EncodeTable, m_DecodeTable);
            AddCrypto("loop", "_10_", m_EncodeTable, m_DecodeTable);
            AddCrypto("looplist", "_11_", m_EncodeTable, m_DecodeTable);
            AddCrypto("while", "_12_", m_EncodeTable, m_DecodeTable);
            AddCrypto("if", "_13_", m_EncodeTable, m_DecodeTable);
            AddCrypto("else", "_14_", m_EncodeTable, m_DecodeTable);
            AddCrypto("inc", "_15_", m_EncodeTable, m_DecodeTable);
            AddCrypto("dec", "_16_", m_EncodeTable, m_DecodeTable);
            AddCrypto("assign", "_17_", m_EncodeTable, m_DecodeTable);
            AddCrypto("propset", "_18_", m_EncodeTable, m_DecodeTable);
            AddCrypto("propget", "_19_", m_EncodeTable, m_DecodeTable);
            AddCrypto("terminate", "_20_", m_EncodeTable, m_DecodeTable);
            AddCrypto("localmessage", "_21_", m_EncodeTable, m_DecodeTable);
            AddCrypto("wait", "_22_", m_EncodeTable, m_DecodeTable);
            AddCrypto("sleep", "_23_", m_EncodeTable, m_DecodeTable);
            AddCrypto("log", "_24_", m_EncodeTable, m_DecodeTable);
            AddCrypto("format", "_25_", m_EncodeTable, m_DecodeTable);
            AddCrypto("substring", "_26_", m_EncodeTable, m_DecodeTable);
            AddCrypto("startstory", "_27_", m_EncodeTable, m_DecodeTable);
            AddCrypto("stopstory", "_28_", m_EncodeTable, m_DecodeTable);
            AddCrypto("firemessage", "_29_", m_EncodeTable, m_DecodeTable);
            AddCrypto("list", "_30_", m_EncodeTable, m_DecodeTable);
            AddCrypto("createnpc", "_31_", m_EncodeTable, m_DecodeTable);
            AddCrypto("destroynpc", "_32_", m_EncodeTable, m_DecodeTable);
            AddCrypto("npcface", "_33_", m_EncodeTable, m_DecodeTable);
            AddCrypto("npcmove", "_34_", m_EncodeTable, m_DecodeTable);
            AddCrypto("npcpatrol", "_35_", m_EncodeTable, m_DecodeTable);
            AddCrypto("npcstop", "_36_", m_EncodeTable, m_DecodeTable);
            AddCrypto("npcattack", "_37_", m_EncodeTable, m_DecodeTable);
            AddCrypto("enableai", "_38_", m_EncodeTable, m_DecodeTable);
            AddCrypto("setcamp", "_39_", m_EncodeTable, m_DecodeTable);
            AddCrypto("getcamp", "_40_", m_EncodeTable, m_DecodeTable);
            AddCrypto("isenemy", "_41_", m_EncodeTable, m_DecodeTable);
            AddCrypto("isfriend", "_42_", m_EncodeTable, m_DecodeTable);
            AddCrypto("objface", "_43_", m_EncodeTable, m_DecodeTable);
            AddCrypto("objmove", "_44_", m_EncodeTable, m_DecodeTable);
            AddCrypto("time", "_45_", m_EncodeTable, m_DecodeTable);
        }

        private bool m_IsClient = true;
        private bool m_IsDebug = false;
        private bool m_IsDevice = false;
        private bool m_IsPublish = false;
        private bool m_IsIphone4S = false;

        private bool m_StoryEditorOpen = false;
        private bool m_StoryEditorContinue = false;
        private Dictionary<string, string> m_EncodeTable = new Dictionary<string, string>();
        private Dictionary<string, string> m_DecodeTable = new Dictionary<string, string>();

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
