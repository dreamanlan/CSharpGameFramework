using System;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

namespace ScriptableFramework.GmCommands
{
    /// <summary>
    /// Registers GM-specific story expressions with DslCalculator
    /// </summary>
    public static class GmExpressionRegistrar
    {
        public static void RegisterGmExpressions(DslCalculatorApiRegistry registry)
        {
            if (registry == null)
                return;

            // Register GM expressions
            registry.Register("enable_calculator_log", "enable_calculator_log(val1, val2, val3) - enable calculator log",
                new ExpressionFactoryHelper<EnableCalculatorLogCommand>());
            registry.Register("reset_dsl", "reset_dsl(val) - reset dsl",
                new ExpressionFactoryHelper<DoResetDslCommand>());
            registry.Register("scp", "scp(val) - scp command",
                new ExpressionFactoryHelper<DoScpCommand>());
            registry.Register("gm", "gm(val) - gm command",
                new ExpressionFactoryHelper<DoGmCommand>());
            registry.Register("set_debug", "set_debug(flag) - set debug mode",
                new ExpressionFactoryHelper<SetDebugCommand>());
            registry.Register("alloc_memory", "alloc_memory(key, size) - allocate memory and store in context variable",
                new ExpressionFactoryHelper<AllocMemoryCommand>());
            registry.Register("free_memory", "free_memory(key) - free memory from context variable",
                new ExpressionFactoryHelper<FreeMemoryCommand>());
            registry.Register("consume_cpu", "consume_cpu(time_us) - consume CPU for testing",
                new ExpressionFactoryHelper<ConsumeCpuCommand>());
        }
    }
}
