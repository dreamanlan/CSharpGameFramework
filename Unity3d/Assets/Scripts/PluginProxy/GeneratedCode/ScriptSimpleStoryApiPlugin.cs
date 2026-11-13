using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

public class ScriptSimpleStoryApiPlugin : ScriptPluginProxyBase
{
	public void Init(DslCalculatorCodeStructure code)
	{
	}
	public bool OnCalc(DslCalculator calculator, IList<BoxedValue> operands, AsyncCalcResult result)
	{
		return false;
	}

	protected override void PrepareMembers()
	{
	}
}
