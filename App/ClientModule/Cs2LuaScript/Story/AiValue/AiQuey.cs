using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using SkillSystem;
using StorySystem;

public class AiQuery : IStoryValuePlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public IStoryValuePlugin Clone()
    {
        var newObj = new AiQuery();
        if (null != m_Select) {
            newObj.m_Select = m_Select.Clone() as IStoryValue;
        }
        if (null != m_From) {
            newObj.m_From = m_From.Clone() as IStoryValue;
        }
        if (null != m_Where) {
            newObj.m_Where = m_Where.Clone() as IStoryValue;
        }
        for (int i = 0; i < m_OrderBy.Count; ++i) {
            newObj.m_OrderBy.Add(m_OrderBy[i].Clone() as IStoryValue);
        }
        newObj.m_Desc = m_Desc;
        return newObj;
    }

    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
    {
        if (null != m_Select && null != m_From) {
            m_From.Evaluate(instance, handler, iterator, args);
            ArrayList coll = new ArrayList();

            //筛选
            IEnumerable enumer = m_From.Value as IEnumerable;
            if (null != enumer) {
                var enumerator = enumer.GetEnumerator();
                while (enumerator.MoveNext()) {
                    var v = enumerator.Current;
                    if (null != m_Where) {
                        m_Where.Evaluate(instance, handler, v, args);
                        object wvObj = m_Where.Value;
                        int wv = (int)System.Convert.ChangeType(wvObj, typeof(int));
                        if (wv != 0) {
                            AddRow(coll, v, instance, handler, args);
                        }
                    } else {
                        AddRow(coll, v, instance, handler, args);
                    }
                }
            }

            //排序
            int ct = m_OrderBy.Count;
            if (ct > 0) {
                coll.Sort(new AiQueryComparer(m_Desc, ct));
            }

            //收集结果
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
            var funcData = statementData.Functions[i];
            LoadFuncData(funcData);
        }
    }

    private void AddRow(ArrayList coll, object v, StoryInstance instance, StoryMessageHandler handler, object[] args)
    {
        ArrayList row = new ArrayList();
        coll.Add(row);

        m_Select.Evaluate(instance, handler, v, args);
        row.Add(m_Select.Value);

        for (int i = 0; i < m_OrderBy.Count; ++i) {
            var val = m_OrderBy[i];
            val.Evaluate(instance, handler, v, args);
            row.Add(val.Value);
        }
    }

    private StoryValueResult m_Proxy = null;
    private IStoryValue m_Select = null;
    private IStoryValue m_From = null;
    private IStoryValue m_Where = null;
    private List<IStoryValue> m_OrderBy = new List<IStoryValue>();
    private bool m_Desc = false;
}
