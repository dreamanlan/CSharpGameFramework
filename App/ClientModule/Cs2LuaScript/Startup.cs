using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

class Startup : IStartupPlugin
{
    public void Start(GameObject obj, MonoBehaviour behaviour)
    {
        HandlerRegister.Register();
        SkillTriggerRegister.Register();
        StoryRegister.Register();
        behaviour.StartCoroutine(this.Tick());
    }
    public void Call(string name, params object[] args)
    {
        HandlerRegister.Call(name, args);
    }
    private IEnumerator Tick()
    {
        yield return new WaitForSeconds(10.0f);
        UnityEngine.Debug.Log("tick after start 10 seconds.");
        yield return null;
    }
}
