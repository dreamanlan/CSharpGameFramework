using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using DotnetStoryScript.DslExpression;

public class ScriptSimpleStoryApiPlugin : ScriptPluginProxyBase
{
	public void Init(DslCalculator calculator)
	{
	}
	public bool OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
	{
		return false;
	}

	protected override void PrepareMembers()
	{
	}
}
