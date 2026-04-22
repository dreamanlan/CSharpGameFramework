using System;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

namespace DotnetStoryScript
{
    public static class DslCalculatorHost
    {
        private static DslCalculatorApiRegistry s_SharedApiRegistry;
        private static readonly object s_SharedApiRegistryLock = new object();

        public static DslCalculatorApiRegistry GetSharedApiRegistry()
        {
            if (s_SharedApiRegistry == null) {
                lock (s_SharedApiRegistryLock) {
                    if (s_SharedApiRegistry == null) {
                        var registry = new DslCalculatorApiRegistry();
                        registry.Init();
                        RegisterStoryExpressions(registry);
                        s_SharedApiRegistry = registry;
                    }
                }
            }
            return s_SharedApiRegistry;
        }

        public static DslCalculator NewCalculator()
        {
            var calc = new DslCalculator();
            calc.ApiRegistry = GetSharedApiRegistry();
            return calc;
        }

        private static void RegisterStoryExpressions(DslCalculatorApiRegistry registry)
        {
            // Register wait expressions
            registry.Register("wait", "wait(milliseconds)[condition(exp)] - wait for specified game time",
                new ExpressionFactoryHelper<WaitExp>());
            registry.Register("realtimewait", "realtimewait(milliseconds)[condition(exp)] - wait for specified real time",
                new ExpressionFactoryHelper<RealtimeWaitExp>());
            registry.Register("storywait", "storywait(milliseconds)[condition(exp)] - wait, can be skipped if story is skipped",
                new ExpressionFactoryHelper<StoryWaitExp>());
            registry.Register("storyrealtimewait", "storyrealtimewait(milliseconds)[condition(exp)] - wait for specified real time, can be skipped if story is skipped",
                new ExpressionFactoryHelper<StoryRealtimeWaitExp>());
            // Register message expressions
            registry.Register("localmessage", "localmessage(msgId, args...) - send local sequential message",
                new ExpressionFactoryHelper<LocalMessageExp>());
            registry.Register("localconcurrentmessage", "localconcurrentmessage(msgId, args...) - send local concurrent message",
                new ExpressionFactoryHelper<LocalConcurrentMessageExp>());
            registry.Register("storylocalmessage", "storylocalmessage(msgId, args...) - send message, skipped if story is skipped",
                new ExpressionFactoryHelper<StoryLocalMessageExp>());
            registry.Register("storylocalconcurrentmessage", "storylocalconcurrentmessage(msgId, args...) - send concurrent message, skipped if story is skipped",
                new ExpressionFactoryHelper<StoryLocalConcurrentMessageExp>());
            registry.Register("clearmessage", "clearmessage(msgId1, msgId2, ...) - clear message queues",
                new ExpressionFactoryHelper<ClearMessageExp>());
            // Register wait message expressions
            registry.Register("waitlocalmessage", "waitlocalmessage(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for message trigger",
                new ExpressionFactoryHelper<WaitLocalMessageExp>());
            registry.Register("waitlocalmessagehandler", "waitlocalmessagehandler(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for message handler",
                new ExpressionFactoryHelper<WaitLocalMessageHandlerExp>());
            registry.Register("storywaitlocalmessage", "storywaitlocalmessage(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for message, can be skipped",
                new ExpressionFactoryHelper<StoryWaitLocalMessageExp>());
            registry.Register("storywaitlocalmessagehandler", "storywaitlocalmessagehandler(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for handler, can be skipped",
                new ExpressionFactoryHelper<StoryWaitLocalMessageHandlerExp>());
            registry.Register("suspendlocalmessagehandler", "suspendlocalmessagehandler(msgId1, ...) - suspend message handlers",
                new ExpressionFactoryHelper<SuspendLocalMessageHandlerExp>());
            registry.Register("resumelocalmessagehandler", "resumelocalmessagehandler(msgId1, ...) - resume message handlers",
                new ExpressionFactoryHelper<ResumeLocalMessageHandlerExp>());
            // Register namespaced message expressions
            registry.Register("localnamespacedmessage", "localnamespacedmessage(msgId, args...) - send namespaced message",
                new ExpressionFactoryHelper<LocalNamespacedMessageExp>());
            registry.Register("localconcurrentnamespacedmessage", "localconcurrentnamespacedmessage(msgId, args...) - send concurrent namespaced message",
                new ExpressionFactoryHelper<LocalConcurrentNamespacedMessageExp>());
            registry.Register("storylocalnamespacedmessage", "storylocalnamespacedmessage(msgId, args...) - send namespaced message, can be skipped",
                new ExpressionFactoryHelper<StoryLocalNamespacedMessageExp>());
            registry.Register("storylocalconcurrentnamespacedmessage", "storylocalconcurrentnamespacedmessage(msgId, args...) - send concurrent namespaced message, can be skipped",
                new ExpressionFactoryHelper<StoryLocalConcurrentNamespacedMessageExp>());
            registry.Register("clearnamespacedmessage", "clearnamespacedmessage(msgId1, ...) - clear namespaced message queues",
                new ExpressionFactoryHelper<ClearNamespacedMessageExp>());
            registry.Register("waitlocalnamespacedmessage", "waitlocalnamespacedmessage(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for namespaced message",
                new ExpressionFactoryHelper<WaitLocalNamespacedMessageExp>());
            registry.Register("waitlocalnamespacedmessagehandler", "waitlocalnamespacedmessagehandler(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for namespaced handler",
                new ExpressionFactoryHelper<WaitLocalNamespacedMessageHandlerExp>());
            registry.Register("storywaitlocalnamespacedmessage", "storywaitlocalnamespacedmessage(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for namespaced message, can be skipped",
                new ExpressionFactoryHelper<StoryWaitLocalNamespacedMessageExp>());
            registry.Register("storywaitlocalnamespacedmessagehandler", "storywaitlocalnamespacedmessagehandler(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for namespaced handler, can be skipped",
                new ExpressionFactoryHelper<StoryWaitLocalNamespacedMessageHandlerExp>());
            registry.Register("suspendlocalnamespacedmessagehandler", "suspendlocalnamespacedmessagehandler(msgId1, ...) - suspend namespaced handlers",
                new ExpressionFactoryHelper<SuspendLocalNamespacedMessageHandlerExp>());
            registry.Register("resumelocalnamespacedmessagehandler", "resumelocalnamespacedmessagehandler(msgId1, ...) - resume namespaced handlers",
                new ExpressionFactoryHelper<ResumeLocalNamespacedMessageHandlerExp>());
            // Register control expressions
            registry.Register("print", "print(msg...) - print a log message",
                new ExpressionFactoryHelper<PrintExp>());
            registry.Register("printf", "printf(fmt...) - print a log message",
                new ExpressionFactoryHelper<PrintfExp>());
            registry.Register("propset", "propset(var,val) - set variable",
                new ExpressionFactoryHelper<PropSetExp>());
            registry.Register("propget", "propget(var[,defval]) - get variable",
                new ExpressionFactoryHelper<PropGetExp>());
            registry.Register("suspend", "suspend() - suspend the current coroutine",
                new ExpressionFactoryHelper<SuspendExp>());
            registry.Register("terminate", "terminate() - terminate the current story instance",
                new ExpressionFactoryHelper<TerminateExp>());
            registry.Register("pause", "pause() - pause the current story instance",
                new ExpressionFactoryHelper<PauseExp>());
            registry.Register("time", "time() - get local milliseconds",
                new ExpressionFactoryHelper<TimeExp>());
            registry.Register("realtime", "realtime() - get local real milliseconds (not affected by time scale)",
                new ExpressionFactoryHelper<RealTimeExp>());
            registry.Register("elapsedtimeus", "elapsedtimeus() - get elapsed time in microseconds",
                new ExpressionFactoryHelper<ElapsedTimeUsExp>());
            registry.Register("storybreak", "storybreak([condition]) - break until condition is met or story is skipped/speedup",
                new ExpressionFactoryHelper<StoryBreakExp>());

            // Register context info expressions
            registry.Register("eval", "eval(exp1, exp2, ...) - evaluate all expressions and return the last value",
                new ExpressionFactoryHelper<EvalExp>());
            registry.Register("namespace", "namespace() - get current story namespace",
                new ExpressionFactoryHelper<NamespaceExp>());
            registry.Register("storyid", "storyid() - get current story ID",
                new ExpressionFactoryHelper<StoryIdExp>());
            registry.Register("messageid", "messageid() - get current message ID",
                new ExpressionFactoryHelper<MessageIdExp>());

            // Register list utility expressions
            registry.Register("stringlist", "stringlist(str_split_by_sep) - parse comma-separated string into list of strings",
                new ExpressionFactoryHelper<StringListExp>());
            registry.Register("intlist", "intlist(str_split_by_sep) - parse comma-separated string into list of ints",
                new ExpressionFactoryHelper<IntListExp>());
            registry.Register("floatlist", "floatlist(str_split_by_sep) - parse comma-separated string into list of floats",
                new ExpressionFactoryHelper<FloatListExp>());
            registry.Register("rndfromlist", "rndfromlist(list[, defval]) - return a random element from list",
                new ExpressionFactoryHelper<RndFromListExp>());

            // Register JSON expressions
            registry.Register("tojson", "tojson(obj) - serialize object to JSON string",
                new ExpressionFactoryHelper<ToJsonExp>());
            registry.Register("fromjson", "fromjson(json_str) - deserialize JSON string to object",
                new ExpressionFactoryHelper<FromJsonExp>());

            // Register Vector constructor expressions
            registry.Register("vector2", "vector2(x, y) - create Vector2",
                new ExpressionFactoryHelper<Vector2Exp>());
            registry.Register("vector3", "vector3(x, y, z) - create Vector3",
                new ExpressionFactoryHelper<Vector3Exp>());
            registry.Register("vector4", "vector4(x, y, z, w) - create Vector4",
                new ExpressionFactoryHelper<Vector4Exp>());
            registry.Register("quaternion", "quaternion(x, y, z, w) - create Quaternion",
                new ExpressionFactoryHelper<QuaternionExp>());
            registry.Register("eular", "eular(x, y, z) - create Quaternion from Euler angles",
                new ExpressionFactoryHelper<EularExp>());
            registry.Register("color", "color(r, g, b, a) - create ColorF",
                new ExpressionFactoryHelper<ColorExp>());
            registry.Register("color32", "color32(r, g, b, a) - create Color32",
                new ExpressionFactoryHelper<Color32Exp>());
            registry.Register("vector2int", "vector2int(x, y) - create Vector2Int",
                new ExpressionFactoryHelper<Vector2IntExp>());
            registry.Register("vector3int", "vector3int(x, y, z) - create Vector3Int",
                new ExpressionFactoryHelper<Vector3IntExp>());

            // Register Vector list and math expressions
            registry.Register("vector2list", "vector2list(str_split_by_sep) - parse string into list of Vector2",
                new ExpressionFactoryHelper<Vector2ListExp>());
            registry.Register("vector3list", "vector3list(str_split_by_sep) - parse string into list of Vector3",
                new ExpressionFactoryHelper<Vector3ListExp>());
            registry.Register("vector2dist", "vector2dist(pt1, pt2) - distance between two Vector2 points",
                new ExpressionFactoryHelper<Vector2DistExp>());
            registry.Register("vector3dist", "vector3dist(pt1, pt2) - distance between two Vector3 points",
                new ExpressionFactoryHelper<Vector3DistExp>());
            registry.Register("vector2to3", "vector2to3(pt) - convert Vector2(x,y) to Vector3(x, 0, y)",
                new ExpressionFactoryHelper<Vector2To3Exp>());
            registry.Register("vector3to2", "vector3to2(pt) - convert Vector3(x,y,z) to Vector2(x, y)",
                new ExpressionFactoryHelper<Vector3To2Exp>());
            registry.Register("rndvector3", "rndvector3(pt, radius) - random Vector3 offset within radius",
                new ExpressionFactoryHelper<RndVector3Exp>());
            registry.Register("rndvector2", "rndvector2(pt, radius) - random Vector2 offset within radius",
                new ExpressionFactoryHelper<RndVector2Exp>());
        }
    }
}
