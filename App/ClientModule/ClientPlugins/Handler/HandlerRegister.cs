﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void HandlerDelegation(params object[] args);
public static class HandlerRegister
{
    public static void Register()
    {
        //在这里注册所有基于名字的消息处理
    }
    public static void Call(string name, params object[] args)
    {
        HandlerDelegation handler;
        if (s_Handlers.TryGetValue(name, out handler)) {
            handler(args);
        }
    }
    private static void Register(string name, HandlerDelegation handler)
    {
        if (s_Handlers.ContainsKey(name)) {
            s_Handlers.Add(name, handler);
        } else {
            s_Handlers[name] = handler;
        }
    }

    private static Dictionary<string, HandlerDelegation> s_Handlers = new Dictionary<string, HandlerDelegation>();
}
