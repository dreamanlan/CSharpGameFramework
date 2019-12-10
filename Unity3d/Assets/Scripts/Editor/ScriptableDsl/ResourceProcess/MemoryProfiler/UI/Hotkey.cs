using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal class HotKey
    {
        public string name;
        public HotKey() {}
        public HotKey(string name)
        {
            this.name = name;
        }
    }
    internal class HotKeyTrigger
    {
        public virtual bool Test(Event evt)
        {
            return false;
        }
    }
    internal class HotKeyTriggerKey : HotKeyTrigger
    {
        public KeyCode m_KeyCode;
        public bool m_Shift;
        public bool m_Ctrl;
        public bool m_Alt;
        public HotKeyTriggerKey(KeyCode keyCode, bool shift = false, bool ctrl = false, bool alt = false)
        {
            m_KeyCode = keyCode;
            m_Shift = shift;
            m_Ctrl = ctrl;
            m_Alt = alt;
        }

        public override bool Test(Event evt)
        {
            if (evt.type == EventType.KeyDown
                && evt.keyCode == m_KeyCode
                && evt.shift == m_Shift
                && evt.alt == m_Alt
                && evt.control == m_Ctrl)
            {
                evt.Use();
                return true;
            }
            return false;
        }
    }
    internal class HotKeyTriggerCommand : HotKeyTrigger
    {
        public string m_CommandName;
        public HotKeyTriggerCommand() {}
        public HotKeyTriggerCommand(string commandName)
        {
            m_CommandName = commandName;
        }

        public override bool Test(Event evt)
        {
            switch (evt.type)
            {
                case EventType.ExecuteCommand:
                    if (evt.commandName == m_CommandName)
                    {
                        evt.Use();
                        return true;
                    }
                    break;
                case EventType.ValidateCommand:
                    if (evt.commandName == m_CommandName)
                    {
                        evt.Use();
                    }
                    break;
            }
            return false;
        }
    }
    internal class HotKeyBind
    {
        public HotKey m_HotKey;
        public HotKeyTrigger m_Trigger;
        public HotKeyBind() {}
        public HotKeyBind(HotKey hotKey, HotKeyTrigger trigger)
        {
            m_HotKey = hotKey;
            m_Trigger = trigger;
        }

        public bool IsTriggered()
        {
            return m_Trigger.Test(Event.current);
        }
    }

    internal class DefaultHotKey
    {
        public HotKeyBind m_CameraFocus;
        public HotKeyBind m_CameraShowAll;
        public DefaultHotKey()
        {
            m_CameraFocus = new HotKeyBind(new HotKey("Camera Focus"), new HotKeyTriggerCommand("FrameSelected"));
            m_CameraShowAll = new HotKeyBind(new HotKey("Camera Show All"), new HotKeyTriggerKey(KeyCode.A));
        }
    }
}
