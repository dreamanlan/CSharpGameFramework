using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GameFramework;

class Startup : IScenePlugin
{
    public void Init(Scene scene)
    {
        HandlerRegister.Register();
    }
    public void ChangeScene(Scene scene)
    {

    }
    public void Tick(Scene scene)
    {
    }
    public void Call(string name, params object[] args)
    {
        HandlerRegister.Call(name, args);
    }
}
