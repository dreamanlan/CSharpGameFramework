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
            registry.Register("real_time_wait", "real_time_wait(milliseconds)[condition(exp)] - wait for specified real time",
                new ExpressionFactoryHelper<RealtimeWaitExp>());
            registry.Register("story_wait", "story_wait(milliseconds)[condition(exp)] - wait, can be skipped if story is skipped",
                new ExpressionFactoryHelper<StoryWaitExp>());
            registry.Register("story_real_time_wait", "story_real_time_wait(milliseconds)[condition(exp)] - wait for specified real time, can be skipped if story is skipped",
                new ExpressionFactoryHelper<StoryRealtimeWaitExp>());
            // Register message expressions
            registry.Register("local_message", "local_message(msgId, args...) - send local sequential message",
                new ExpressionFactoryHelper<LocalMessageExp>());
            registry.Register("local_concurrent_message", "local_concurrent_message(msgId, args...) - send local concurrent message",
                new ExpressionFactoryHelper<LocalConcurrentMessageExp>());
            registry.Register("story_local_message", "story_local_message(msgId, args...) - send message, skipped if story is skipped",
                new ExpressionFactoryHelper<StoryLocalMessageExp>());
            registry.Register("story_local_concurrent_message", "story_local_concurrent_message(msgId, args...) - send concurrent message, skipped if story is skipped",
                new ExpressionFactoryHelper<StoryLocalConcurrentMessageExp>());
            registry.Register("clear_message", "clear_message(msgId1, msgId2, ...) - clear message queues",
                new ExpressionFactoryHelper<ClearMessageExp>());
            // Register wait message expressions
            registry.Register("wait_local_message", "wait_local_message(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for message trigger",
                new ExpressionFactoryHelper<WaitLocalMessageExp>());
            registry.Register("wait_local_message_handler", "wait_local_message_handler(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for message handler",
                new ExpressionFactoryHelper<WaitLocalMessageHandlerExp>());
            registry.Register("story_wait_local_message", "story_wait_local_message(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for message, can be skipped",
                new ExpressionFactoryHelper<StoryWaitLocalMessageExp>());
            registry.Register("story_wait_local_message_handler", "story_wait_local_message_handler(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for handler, can be skipped",
                new ExpressionFactoryHelper<StoryWaitLocalMessageHandlerExp>());
            registry.Register("suspend_local_message_handler", "suspend_local_message_handler(msgId1, ...) - suspend message handlers",
                new ExpressionFactoryHelper<SuspendLocalMessageHandlerExp>());
            registry.Register("resume_local_message_handler", "resume_local_message_handler(msgId1, ...) - resume message handlers",
                new ExpressionFactoryHelper<ResumeLocalMessageHandlerExp>());
            // Register namespaced message expressions
            registry.Register("local_namespaced_message", "local_namespaced_message(msgId, args...) - send namespaced message",
                new ExpressionFactoryHelper<LocalNamespacedMessageExp>());
            registry.Register("local_concurrent_namespaced_message", "local_concurrent_namespaced_message(msgId, args...) - send concurrent namespaced message",
                new ExpressionFactoryHelper<LocalConcurrentNamespacedMessageExp>());
            registry.Register("story_local_namespaced_message", "story_local_namespaced_message(msgId, args...) - send namespaced message, can be skipped",
                new ExpressionFactoryHelper<StoryLocalNamespacedMessageExp>());
            registry.Register("story_local_concurrent_namespaced_message", "story_local_concurrent_namespaced_message(msgId, args...) - send concurrent namespaced message, can be skipped",
                new ExpressionFactoryHelper<StoryLocalConcurrentNamespacedMessageExp>());
            registry.Register("clear_namespaced_message", "clear_namespaced_message(msgId1, ...) - clear namespaced message queues",
                new ExpressionFactoryHelper<ClearNamespacedMessageExp>());
            registry.Register("wait_local_namespaced_message", "wait_local_namespaced_message(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for namespaced message",
                new ExpressionFactoryHelper<WaitLocalNamespacedMessageExp>());
            registry.Register("wait_local_namespaced_message_handler", "wait_local_namespaced_message_handler(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for namespaced handler",
                new ExpressionFactoryHelper<WaitLocalNamespacedMessageHandlerExp>());
            registry.Register("story_wait_local_namespaced_message", "story_wait_local_namespaced_message(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for namespaced message, can be skipped",
                new ExpressionFactoryHelper<StoryWaitLocalNamespacedMessageExp>());
            registry.Register("story_wait_local_namespaced_message_handler", "story_wait_local_namespaced_message_handler(msgId1, ...)[set(var,val) timeoutset(timeout,var,val)] - wait for namespaced handler, can be skipped",
                new ExpressionFactoryHelper<StoryWaitLocalNamespacedMessageHandlerExp>());
            registry.Register("suspend_local_namespaced_message_handler", "suspend_local_namespaced_message_handler(msgId1, ...) - suspend namespaced handlers",
                new ExpressionFactoryHelper<SuspendLocalNamespacedMessageHandlerExp>());
            registry.Register("resume_local_namespaced_message_handler", "resume_local_namespaced_message_handler(msgId1, ...) - resume namespaced handlers",
                new ExpressionFactoryHelper<ResumeLocalNamespacedMessageHandlerExp>());
            // Register control expressions
            registry.Register("print", "print(msg...) - print a log message",
                new ExpressionFactoryHelper<PrintExp>());
            registry.Register("printf", "printf(fmt...) - print a log message",
                new ExpressionFactoryHelper<PrintfExp>());
            registry.Register("suspend", "suspend() - suspend the current coroutine",
                new ExpressionFactoryHelper<SuspendExp>());
            registry.Register("terminate", "terminate() - terminate the current story instance",
                new ExpressionFactoryHelper<TerminateExp>());
            registry.Register("pause", "pause() - pause the current story instance",
                new ExpressionFactoryHelper<PauseExp>());
            registry.Register("time", "time() - get local milliseconds",
                new ExpressionFactoryHelper<TimeExp>());
            registry.Register("real_time", "real_time() - get local real milliseconds (not affected by time scale)",
                new ExpressionFactoryHelper<RealTimeExp>());
            registry.Register("elapsed_time_us", "elapsed_time_us() - get elapsed time in microseconds",
                new ExpressionFactoryHelper<ElapsedTimeUsExp>());
            registry.Register("story_break", "story_break([condition]) - break until condition is met or story is skipped/speedup",
                new ExpressionFactoryHelper<StoryBreakExp>());

            // Register context info expressions
            registry.Register("eval", "eval(exp1, exp2, ...) - evaluate all expressions and return the last value",
                new ExpressionFactoryHelper<EvalExp>());
            registry.Register("namespace", "namespace() - get current story namespace",
                new ExpressionFactoryHelper<NamespaceExp>());
            registry.Register("story_id", "story_id() - get current story ID",
                new ExpressionFactoryHelper<StoryIdExp>());
            registry.Register("message_id", "message_id() - get current message ID",
                new ExpressionFactoryHelper<MessageIdExp>());

            // Register list utility expressions
            registry.Register("string_list", "string_list(str_split_by_sep) - parse comma-separated string into list of strings",
                new ExpressionFactoryHelper<StringListExp>());
            registry.Register("int_list", "int_list(str_split_by_sep) - parse comma-separated string into list of ints",
                new ExpressionFactoryHelper<IntListExp>());
            registry.Register("float_list", "float_list(str_split_by_sep) - parse comma-separated string into list of floats",
                new ExpressionFactoryHelper<FloatListExp>());
            registry.Register("rnd_from_list", "rnd_from_list(list[, defval]) - return a random element from list",
                new ExpressionFactoryHelper<RndFromListExp>());

            // Register JSON expressions
            registry.Register("to_json", "to_json(obj) - serialize object to JSON string",
                new ExpressionFactoryHelper<ToJsonExp>());
            registry.Register("from_json", "from_json(json_str) - deserialize JSON string to object",
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
            registry.Register("vector2_int", "vector2_int(x, y) - create Vector2Int",
                new ExpressionFactoryHelper<Vector2IntExp>());
            registry.Register("vector3_int", "vector3_int(x, y, z) - create Vector3Int",
                new ExpressionFactoryHelper<Vector3IntExp>());

            // Register Vector list and math expressions
            registry.Register("vector2_list", "vector2_list(str_split_by_sep) - parse string into list of Vector2",
                new ExpressionFactoryHelper<Vector2ListExp>());
            registry.Register("vector3_list", "vector3_list(str_split_by_sep) - parse string into list of Vector3",
                new ExpressionFactoryHelper<Vector3ListExp>());
            registry.Register("vector2_dist", "vector2_dist(pt1, pt2) - distance between two Vector2 points",
                new ExpressionFactoryHelper<Vector2DistExp>());
            registry.Register("vector3_dist", "vector3_dist(pt1, pt2) - distance between two Vector3 points",
                new ExpressionFactoryHelper<Vector3DistExp>());
            registry.Register("vector2_to_3", "vector2_to_3(pt) - convert Vector2(x,y) to Vector3(x, 0, y)",
                new ExpressionFactoryHelper<Vector2To3Exp>());
            registry.Register("vector3_to_2", "vector3_to_2(pt) - convert Vector3(x,y,z) to Vector2(x, y)",
                new ExpressionFactoryHelper<Vector3To2Exp>());
            registry.Register("rnd_vector3", "rnd_vector3(pt, radius) - random Vector3 offset within radius",
                new ExpressionFactoryHelper<RndVector3Exp>());
            registry.Register("rnd_vector2", "rnd_vector2(pt, radius) - random Vector2 offset within radius",
                new ExpressionFactoryHelper<RndVector2Exp>());
        }
    }
}
