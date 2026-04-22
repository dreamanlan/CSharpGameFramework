using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

public class AiQuery : IStoryApiPlugin
{
    public void Init(DslCalculator calculator)
    {
        m_Calculator = calculator;
    }

    public bool Calc(AsyncCalcResult result)
    {
        if (null != m_Select && null != m_From) {
            var fromVal = m_From.Calc();
            ArrayList coll = new ArrayList();

            //filter
            IEnumerable enumer = fromVal.ObjectVal as IEnumerable;
            if (null != enumer) {
                var enumerator = enumer.GetEnumerator();
                while (enumerator.MoveNext()) {
                    var v = enumerator.Current;
                    var bv = BoxedValue.FromObject(v);
                    if (null != m_Where) {
                        var whereVal = m_Where.Calc();
                        object wvObj = whereVal.ObjectVal;
                        int wv = (int)System.Convert.ChangeType(wvObj, typeof(int));
                        if (wv != 0) {
                            AddRow(coll, bv);
                        }
                    } else {
                        AddRow(coll, bv);
                    }
                }
            }

            //sort
            int ct = m_OrderBy.Count;
            if (ct > 0) {
                coll.Sort(new AiQueryComparer(m_Desc, ct));
            }

            //get the results
            ArrayList res = new ArrayList();
            for (int i = 0; i < coll.Count; ++i) {
                var ao = coll[i] as ArrayList;
                res.Add(ao[0]);
            }
            result.Value = BoxedValue.FromObject(result);
        }
        return false;
    }

    public void LoadCallData(Dsl.FunctionData callData)
    {
        string id = callData.GetId();
        if (id == "select") {
            m_Select = m_Calculator.Load(callData.GetParam(0));
        } else if (id == "from") {
            m_From = m_Calculator.Load(callData.GetParam(0));
        } else if (id == "where") {
            m_Where = m_Calculator.Load(callData.GetParam(0));
        } else if (id == "orderby") {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                var v = m_Calculator.Load(callData.GetParam(i));
                m_OrderBy.Add(v);
            }
        } else if (id == "asc") {
            m_Desc = false;
        } else if (id == "desc") {
            m_Desc = true;
        }
    }

    public void LoadFuncData(Dsl.FunctionData funcData)
    {
        LoadCallData(funcData);
    }

    public void LoadStatementData(Dsl.StatementData statementData)
    {
        for (int i = 0; i < statementData.Functions.Count; ++i) {
            var funcData = statementData.Functions[i].AsFunction;
            LoadFuncData(funcData);
        }
    }

    private void AddRow(ArrayList coll, BoxedValue v)
    {
        ArrayList row = new ArrayList();
        coll.Add(row);

        var selectVal = m_Select.Calc();
        row.Add(selectVal.GetObject());

        for (int i = 0; i < m_OrderBy.Count; ++i) {
            var val = m_OrderBy[i];
            var orderVal = val.Calc();
            row.Add(orderVal.GetObject());
        }
    }

    private DslCalculator m_Calculator = null;
    private IExpression m_Select = null;
    private IExpression m_From = null;
    private IExpression m_Where = null;
    private List<IExpression> m_OrderBy = new List<IExpression>();
    private bool m_Desc = false;
}
