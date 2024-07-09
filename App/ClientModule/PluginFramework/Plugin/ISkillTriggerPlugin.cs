using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotnetSkillScript;

namespace ScriptableFramework.Plugin
{
    public interface ISkillTriggerPlugin
    {
        void SetProxy(SkillTriggerProxy proxy);
        ISkillTriggerPlugin Clone();
        void Reset();
        bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime);        
        void LoadCallData(Dsl.FunctionData callData, SkillInstance instance);
        void LoadFuncData(Dsl.FunctionData funcData, SkillInstance instance);
        void LoadStatementData(Dsl.StatementData statementData, SkillInstance instance);
        void OnInitProperties();
    }
}
