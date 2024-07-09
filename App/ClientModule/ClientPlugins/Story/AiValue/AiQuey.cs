using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework.Plugin;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript;

public class AiQuery : IStoryFunctionPlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public IStoryFunctionPlugin Clone()
    {
        var newObj = new AiQuery();
        if (null != m_Select) {
            newObj.m_Select = m_Select.Clone() as IStoryFunction;
        }
        if (null != m_From) {
            newObj.m_From = m_From.Clone() as IStoryFunction;
        }
        if (null != m_Where) {
            newObj.m_Where = m_Where.Clone() as IStoryFunction;
        }
        for (int i = 0; i < m_OrderBy.Count; ++i) {
            newObj.m_OrderBy.Add(m_OrderBy[i].Clone() as IStoryFunction);
        }
        newObj.m_Desc = m_Desc;
        return newObj;
    }

    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
    {
        if (null != m_Select && null != m_From) {
            m_From.Evaluate(instance, handler, iterator, args);
            ArrayList coll = new ArrayList();

            //filter
            IEnumerable enumer = m_From.Value.ObjectVal as IEnumerable;
            if (null != enumer) {
                var enumerator = enumer.GetEnumerator();
                while (enumerator.MoveNext()) {
                    var v = enumerator.Current;
                    var bv = BoxedValue.FromObject(v);
                    if (null != m_Where) {
                        m_Where.Evaluate(instance, handler, bv, args);
                        object wvObj = m_Where.Value;
                        int wv = (int)System.Convert.ChangeType(wvObj, typeof(int));
                        if (wv != 0) {
                            AddRow(coll, bv, instance, handler, args);
                        }
                    } else {
                        AddRow(coll, bv, instance, handler, args);
                    }
                }
            }

            //sort
            int ct = m_OrderBy.Count;
            if (ct > 0) {
                coll.Sort(new AiQueryComparer(m_Desc, ct));
            }

            //get the results
            ArrayList result = new ArrayList();
            for (int i = 0; i < coll.Count; ++i) {
                var ao = coll[i] as ArrayList;
                result.Add(ao[0]);
            }
            m_Proxy.Value = result;
        }
    }

    public void LoadCallData(Dsl.FunctionData callData)
    {
        string id = callData.GetId();
        if (id == "select") {
            m_Select = new StoryValue();
            m_Select.InitFromDsl(callData.GetParam(0));
        } else if (id == "from") {
            m_From = new StoryValue();
            m_From.InitFromDsl(callData.GetParam(0));
        } else if (id == "where") {
            m_Where = new StoryValue();
            m_Where.InitFromDsl(callData.GetParam(0));
        } else if (id == "orderby") {
            for (int i = 0; i < callData.GetParamNum(); ++i) {
                StoryValue v = new StoryValue();
                v.InitFromDsl(callData.GetParam(i));
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

    private void AddRow(ArrayList coll, BoxedValue v, StoryInstance instance, StoryMessageHandler handler, BoxedValueList args)
    {
        ArrayList row = new ArrayList();
        coll.Add(row);

        m_Select.Evaluate(instance, handler, v, args);
        row.Add(m_Select.Value.GetObject());

        for (int i = 0; i < m_OrderBy.Count; ++i) {
            var val = m_OrderBy[i];
            val.Evaluate(instance, handler, v, args);
            row.Add(val.Value.GetObject());
        }
    }

    private StoryValueResult m_Proxy = null;
    private IStoryFunction m_Select = null;
    private IStoryFunction m_From = null;
    private IStoryFunction m_Where = null;
    private List<IStoryFunction> m_OrderBy = new List<IStoryFunction>();
    private bool m_Desc = false;
}
