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
            registry.Register("enablecalculatorlog", "enablecalculatorlog(val1, val2, val3) - enable calculator log",
                new ExpressionFactoryHelper<EnableCalculatorLogCommand>());
            registry.Register("resetdsl", "resetdsl(val) - reset dsl",
                new ExpressionFactoryHelper<DoResetDslCommand>());
            registry.Register("scp", "scp(val) - scp command",
                new ExpressionFactoryHelper<DoScpCommand>());
            registry.Register("gm", "gm(val) - gm command",
                new ExpressionFactoryHelper<DoGmCommand>());
            registry.Register("setdebug", "setdebug(flag) - set debug mode",
                new ExpressionFactoryHelper<SetDebugCommand>());
            registry.Register("allocmemory", "allocmemory(key, size) - allocate memory and store in context variable",
                new ExpressionFactoryHelper<AllocMemoryCommand>());
            registry.Register("freememory", "freememory(key) - free memory from context variable",
                new ExpressionFactoryHelper<FreeMemoryCommand>());
            registry.Register("consumecpu", "consumecpu(time_us) - consume CPU for testing",
                new ExpressionFactoryHelper<ConsumeCpuCommand>());
        }
    }
}
