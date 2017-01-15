using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using SLua;

public class Cs2LuaEnvironment
{
    public bool LuaInited
    {
        get { return m_LuaInited; }
    }
    public Assembly Assembly
    {
        get { return Cs2LuaAssembly.Instance.Assembly; }
    }
    public LuaSvr LuaSvr
    {
        get { return m_LuaSvr; }
    }
    public void Init()
    {
#if !CS2LUA_DEBUG
        m_LuaSvr.init(null, () => {
            var entry = (LuaTable)m_LuaSvr.start("Cs2LuaScript__Program");
            entry.invoke("Init");
            m_LuaInited = true;
        });
#endif
    }

#if CS2LUA_DEBUG
    private LuaSvr m_LuaSvr = null;    
#else
    private LuaSvr m_LuaSvr = new LuaSvr();
#endif
    private bool m_LuaInited = false;
}
