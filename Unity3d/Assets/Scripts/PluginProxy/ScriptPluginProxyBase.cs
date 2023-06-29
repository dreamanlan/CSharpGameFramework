using System;

public abstract class ScriptPluginProxyBase
{
    public void LoadScript(string file)
    {
    }

    protected abstract void PrepareMembers();
}