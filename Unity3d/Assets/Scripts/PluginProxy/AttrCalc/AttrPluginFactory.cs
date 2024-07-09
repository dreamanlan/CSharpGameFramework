using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptableFramework.Plugin;
using ScriptableFramework.AttrCalc;

public class NativeAttrPluginFactory : IAttrExpressionFactory
{
    public IAttrExpression Create()
    {
        return new NativeAttrExpressionProxy(m_ClassName);
    }
    public NativeAttrPluginFactory(string className)
    {
        m_ClassName = className;
    }

    private string m_ClassName;
}

public class ScriptAttrPluginFactory : IAttrExpressionFactory
{
    public IAttrExpression Create()
    {
        return new ScriptAttrExpressionProxy(m_ClassName);
    }
    public ScriptAttrPluginFactory(string className)
    {
        m_ClassName = className;
    }

    private string m_ClassName;
}

public class NativeAttrExpressionProxy : AbstractAttrExpression
{
    public override long Calc(ScriptableFramework.SceneContextInfo context, ScriptableFramework.CharacterProperty source, ScriptableFramework.CharacterProperty target, long[] args)
    {
        if (null != m_Plugin) {
            return m_Plugin.Calc(context, source, target, args);
        }
        return 0;
    }
    protected override void SetCalculator(DslCalculator calculator)
    {
        if (null != m_Plugin) {
            m_Plugin.SetCalculator(calculator);
        }
    }
    protected override bool Load(Dsl.ValueData valData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadValue(valData);
        }
        return base.Load(valData);
    }
    protected override bool Load(AttrExpressionList exps)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadExpressions(exps);
        }
        return base.Load(exps);
    }
    protected override bool Load(Dsl.FunctionData funcData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadFuncData(funcData);
        }
        return base.Load(funcData);
    }
    protected override bool Load(Dsl.StatementData statementData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadStatementData(statementData);
        }
        return base.Load(statementData);
    }

    public NativeAttrExpressionProxy(string className)
    {
        m_ClassName = className;

        var module = PluginManager.Instance.CreateObject(m_ClassName);
        m_Plugin = module as IAttrExpressionPlugin;
    }
    
    private string m_ClassName;
    private IAttrExpressionPlugin m_Plugin;
}

public class ScriptAttrExpressionProxy : AbstractAttrExpression
{
    public override long Calc(ScriptableFramework.SceneContextInfo context, ScriptableFramework.CharacterProperty source, ScriptableFramework.CharacterProperty target, long[] args)
    {
        if (null != m_Plugin) {
            return m_Plugin.Calc(context, source, target, args);
        }
        return 0;
    }
    protected override void SetCalculator(DslCalculator calculator)
    {
        if (null != m_Plugin) {
            m_Plugin.SetCalculator(calculator);
        }
    }
    protected override bool Load(Dsl.ValueData valData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadValue(valData);
        }
        return base.Load(valData);
    }
    protected override bool Load(AttrExpressionList exps)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadExpressions(exps);
        }
        return base.Load(exps);
    }
    protected override bool Load(Dsl.FunctionData funcData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadFuncData(funcData);
        }
        return base.Load(funcData);
    }
    protected override bool Load(Dsl.StatementData statementData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadStatementData(statementData);
        }
        return base.Load(statementData);
    }

    public ScriptAttrExpressionProxy(string className)
    {
        m_ClassName = className;
        m_FileName = m_ClassName.Replace(".", "__");

        //m_Plugin = new ScriptAttrExpressionPlugin();
        //m_Plugin.LoadScript(m_FileName);
    }
    
    private string m_ClassName;
    private string m_FileName;
    private IAttrExpressionPlugin m_Plugin;
}
