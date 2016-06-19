using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;
using ScriptRuntime;
using GameFramework;
using GameFramework.Skill;
using GameFramework.Skill.Trigers;

namespace GameFramework.Story.Commands
{
    /// <summary>
    /// blackboardclear();
    /// </summary>
    internal class BlackboardClearCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            BlackboardClearCommand cmd = new BlackboardClearCommand();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
        }

        protected override void Evaluate(StoryInstance instance)
        {
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            ClientModule.Instance.BlackBoard.ClearVariables();
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
        }
    }
    /// <summary>
    /// blackboardset(name,value);
    /// </summary>
    internal class BlackboardSetCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            BlackboardSetCommand cmd = new BlackboardSetCommand();
            cmd.m_AttrName = m_AttrName.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_AttrName.Substitute(iterator, args);
            m_Value.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_AttrName.Evaluate(instance);
            m_Value.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            string name = m_AttrName.Value;
            object value = m_Value.Value;
            ClientModule.Instance.BlackBoard.SetVariable(name, value);
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_AttrName.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue<object> m_Value = new StoryValue();
    }
    /// <summary>
    /// camerafollow(npc_unit_id1,npc_unit_id2,...);
    /// </summary>
    internal class CameraFollowCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CameraFollowCommand cmd = new CameraFollowCommand();
            for (int i = 0; i < m_UnitIds.Count; i++) {
                cmd.m_UnitIds.Add(m_UnitIds[i].Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_UnitIds.Count; i++) {
                m_UnitIds[i].Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_UnitIds.Count; i++) {
                m_UnitIds[i].Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            for (int i = 0; i < m_UnitIds.Count; i++) {
                int unitId = m_UnitIds[i].Value;
                EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
                if (null != entity && (!entity.IsDead() || entity.IsBorning)) {
                    Utility.SendMessage("GameRoot", "CameraFollow", entity.GetId());
                    break;
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<int> val = new StoryValue<int>();
                val.InitFromDsl(callData.GetParam(i));
                m_UnitIds.Add(val);
            }
        }

        private List<IStoryValue<int>> m_UnitIds = new List<IStoryValue<int>>();
    }
    /// <summary>
    /// camerafollowrange(npc_unit_id_begin,npc_unit_id_end);
    /// </summary>
    internal class CameraFollowRangeCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CameraFollowRangeCommand cmd = new CameraFollowRangeCommand();
            cmd.m_BeginUnitId = m_BeginUnitId.Clone();
            cmd.m_EndUnitId = m_EndUnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_BeginUnitId.Substitute(iterator, args);
            m_EndUnitId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_BeginUnitId.Evaluate(instance);
            m_EndUnitId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int beginUnitId = m_BeginUnitId.Value;
            int endUnitId = m_EndUnitId.Value;
            for (int unitId = beginUnitId; unitId <= endUnitId; ++unitId) {
                EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
                if (null != entity && (!entity.IsDead() || entity.IsBorning)) {
                    Utility.SendMessage("GameRoot", "CameraFollow", entity.GetId());
                    break;
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_BeginUnitId.InitFromDsl(callData.GetParam(0));
                m_EndUnitId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_BeginUnitId = new StoryValue<int>();
        private IStoryValue<int> m_EndUnitId = new StoryValue<int>();
    }
    /// <summary>
    /// cameralookat(npc_unit_id);
    /// or
    /// cameralookat(vector3(x,y,z));
    /// </summary>
    internal class CameraLookCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CameraLookCommand cmd = new CameraLookCommand();
            cmd.m_Arg = m_Arg.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_Arg.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_Arg.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            object obj = m_Arg.Value;
            if (obj is int) {
                int unitId = (int)obj;
                EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
                if (null != entity) {
                    Vector3 pos = entity.GetMovementStateInfo().GetPosition3D();
                    Utility.SendMessage("GameRoot", "CameraLook", new float[] { pos.X, pos.Y + entity.GetRadius(), pos.Z });
                }
            } else {
                Vector3 pos = (Vector3)obj;
                Utility.SendMessage("GameRoot", "CameraLook", new float[] { pos.X, pos.Y, pos.Z });
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Arg.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<object> m_Arg = new StoryValue();
    }
    /// <summary>
    /// camerafollowpath();
    /// </summary>
    internal class CameraFollowPathCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CameraFollowPathCommand cmd = new CameraFollowPathCommand();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
        }

        protected override void Evaluate(StoryInstance instance)
        {
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Utility.SendMessage("GameRoot", "CameraFollowPath", null);
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
        }
    }
    /// <summary>
    /// lockframe(scale);
    /// </summary>
    internal class LockFrameCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            LockFrameCommand cmd = new LockFrameCommand();
            cmd.m_Scale = m_Scale.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_Scale.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_Scale.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            float scale = m_Scale.Value;
            UnityEngine.Time.timeScale = scale;
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Scale.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<float> m_Scale = new StoryValue<float>();
    }
    /// <summary>
    /// setleaderid([objid,]leaderid);
    /// </summary>
    internal class SetLeaderIdCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SetLeaderIdCommand cmd = new SetLeaderIdCommand();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_LeaderId = m_LeaderId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_ParamNum > 1) {
                m_ObjId.Substitute(iterator, args);
            }
            m_LeaderId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_ParamNum > 1) {
                m_ObjId.Evaluate(instance);
            }
            m_LeaderId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int leaderId = m_LeaderId.Value;
            if (m_ParamNum > 1) {
                int objId = m_ObjId.Value;
                EntityInfo npc = ClientModule.Instance.GetEntityById(objId);
                if (null != npc) {
                    npc.GetAiStateInfo().LeaderID = leaderId;
                }
            } else {
                ClientModule.Instance.LeaderID = leaderId;
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_LeaderId.InitFromDsl(callData.GetParam(1));
            } else if (m_ParamNum > 0) {
                m_LeaderId.InitFromDsl(callData.GetParam(0));
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_LeaderId = new StoryValue<int>();
    }
    /// <summary>
    /// showdlg(storyDlgId);
    /// </summary>
    internal class ShowDlgCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ShowDlgCommand cmd = new ShowDlgCommand();
            cmd.m_StoryDlgId = m_StoryDlgId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_StoryDlgId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_StoryDlgId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            GfxStorySystem.Instance.SendMessage("show_dlg", m_StoryDlgId.Value);
            LogSystem.Info("showdlg {0}", m_StoryDlgId.Value);
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryDlgId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_StoryDlgId = new StoryValue<int>();
    }
    /// <summary>
    /// areadetect(name,radius,type,callback)[set(var,val)];
    /// </summary>
    internal class AreaDetectCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            AreaDetectCommand cmd = new AreaDetectCommand();
            cmd.m_Name = m_Name.Clone();
            cmd.m_Radius = m_Radius.Clone();
            cmd.m_Type = m_Type.Clone();
            cmd.m_EventName = m_EventName.Clone();
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_ElseSetVal = m_ElseSetVal.Clone();
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_Name.Substitute(iterator, args);
            m_Radius.Substitute(iterator, args);
            m_Type.Substitute(iterator, args);
            m_EventName.Substitute(iterator, args);
            if (m_HaveSet) {
                m_SetVar.Substitute(iterator, args);
                m_SetVal.Substitute(iterator, args);
                m_ElseSetVal.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_Name.Evaluate(instance);
            m_Radius.Evaluate(instance);
            m_Type.Evaluate(instance);
            m_EventName.Evaluate(instance);
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance);
                m_SetVal.Evaluate(instance);
                m_ElseSetVal.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            bool triggered = false;
            string name = m_Name.Value;
            float radius = m_Radius.Value;
            string type = m_Type.Value;
            string eventName = m_EventName.Value;
            UnityEngine.GameObject obj = UnityEngine.GameObject.Find(name);
            if (null != obj) {
                UnityEngine.Vector3 pos = obj.transform.position;
                if (type == "myself") {
                    EntityViewModel view = EntityController.Instance.GetEntityViewById(ClientModule.Instance.LeaderID);
                    if (null != view && null!=view.Actor) {
                        if ((view.Actor.transform.position - pos).sqrMagnitude < radius * radius) {
                            GfxStorySystem.Instance.SendMessage(eventName, name, radius, type);
                            triggered = true;
                        }
                    }
                } else if (type == "anyenemy" || type == "anyfriend") {
                    EntityInfo myself = ClientModule.Instance.GetEntityById(ClientModule.Instance.LeaderID);
                    ClientModule.Instance.KdTree.Query(pos.x, pos.y, pos.z, radius, (float distSqr, KdTreeObject kdObj) => {
                        if (type == "anyenemy" && EntityInfo.GetRelation(myself, kdObj.Object) == CharacterRelation.RELATION_ENEMY ||
                            type == "anyfriend" && EntityInfo.GetRelation(myself, kdObj.Object) == CharacterRelation.RELATION_FRIEND) {
                            GfxStorySystem.Instance.SendMessage(eventName, name, radius, type);
                            triggered = true;
                            return false;
                        }
                        return true;
                    });
                }
            }
            string varName = m_SetVar.Value;
            object varVal = m_SetVal.Value;
            object elseVal = m_ElseSetVal.Value;
            if (triggered) {
                instance.SetVariable(varName, varVal);
            } else {
                instance.SetVariable(varName, elseVal);
            }
            return false;         
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 3) {
                m_Name.InitFromDsl(callData.GetParam(0));
                m_Radius.InitFromDsl(callData.GetParam(1));
                m_Type.InitFromDsl(callData.GetParam(2));
                m_EventName.InitFromDsl(callData.GetParam(3));
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 2) {
                Dsl.CallData first = statementData.Functions[0].Call;
                Dsl.CallData second = statementData.Functions[1].Call;
                if (null != first && null != second) {
                    m_HaveSet = true;

                    Load(first);
                    LoadSet(second);
                }
            }
        }

        private void LoadSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
                m_ElseSetVal.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<string> m_Name = new StoryValue<string>();
        private IStoryValue<float> m_Radius = new StoryValue<float>();
        private IStoryValue<string> m_Type = new StoryValue<string>();
        private IStoryValue<string> m_EventName = new StoryValue<string>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<object> m_ElseSetVal = new StoryValue();
        private bool m_HaveSet = false;
    }
    /// <summary>
    /// setstorystate(0_or_1);
    /// </summary>
    internal class SetStoryStateCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SetStoryStateCommand cmd = new SetStoryStateCommand();
            cmd.m_StoryState = m_StoryState.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_StoryState.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_StoryState.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int state = m_StoryState.Value;
            ClientModule.Instance.IsStoryState = state != 0;
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryState.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_StoryState = new StoryValue<int>();
    }
    /// <summary>
    /// bindui(gameobject){
    ///     var("@varname","Panel/Input");
    ///     inputs("",...);
    ///     toggles("",...);
    ///     sliders("",...);
    ///     dropdowns("",...);
    ///     onevent("button","eventtag","Panel/Button");
    /// };
    /// </summary>
    internal class BindUiCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            BindUiCommand cmd = new BindUiCommand();
            cmd.m_Obj = m_Obj.Clone();
            for (int i = 0; i < m_VarInfos.Count; ++i) {
                cmd.m_VarInfos.Add(new VarInfo(m_VarInfos[i]));
            }
            for (int i = 0; i < m_EventInfos.Count; ++i) {
                cmd.m_EventInfos.Add(new EventInfo(m_EventInfos[i]));
            }
            for (int i = 0; i < m_Inputs.Count; ++i) {
                cmd.m_Inputs.Add(m_Inputs[i].Clone());
            }
            for (int i = 0; i < m_Toggles.Count; ++i) {
                cmd.m_Toggles.Add(m_Toggles[i].Clone());
            }
            for (int i = 0; i < m_Sliders.Count; ++i) {
                cmd.m_Sliders.Add(m_Sliders[i].Clone());
            }
            for (int i = 0; i < m_DropDowns.Count; ++i) {
                cmd.m_DropDowns.Add(m_DropDowns[i].Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_Obj.Substitute(iterator, args);
            for (int i = 0; i < m_VarInfos.Count; ++i) {
                m_VarInfos[i].m_VarName.Substitute(iterator, args);
                m_VarInfos[i].m_ControlPath.Substitute(iterator, args);
            }
            for (int i = 0; i < m_EventInfos.Count; ++i) {
                m_EventInfos[i].m_Tag.Substitute(iterator, args);
                m_EventInfos[i].m_Path.Substitute(iterator, args);
            }
            var list = m_Inputs;
            for (int k = 0; k < list.Count; ++k) {
                list[k].Substitute(iterator, args);
            }
            list = m_Toggles;
            for (int k = 0; k < list.Count; ++k) {
                list[k].Substitute(iterator, args);
            }
            list = m_Sliders;
            for (int k = 0; k < list.Count; ++k) {
                list[k].Substitute(iterator, args);
            }
            list = m_DropDowns;
            for (int k = 0; k < list.Count; ++k) {
                list[k].Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_Obj.Evaluate(instance);
            for (int i = 0; i < m_VarInfos.Count; ++i) {
                m_VarInfos[i].m_VarName.Evaluate(instance);
                m_VarInfos[i].m_ControlPath.Evaluate(instance);
            }
            for (int i = 0; i < m_EventInfos.Count; ++i) {
                m_EventInfos[i].m_Tag.Evaluate(instance);
                m_EventInfos[i].m_Path.Evaluate(instance);
            }
            var list = m_Inputs;
            for (int k = 0; k < list.Count; ++k) {
                list[k].Evaluate(instance);
            }
            list = m_Toggles;
            for (int k = 0; k < list.Count; ++k) {
                list[k].Evaluate(instance);
            }
            list = m_Sliders;
            for (int k = 0; k < list.Count; ++k) {
                list[k].Evaluate(instance);
            }
            list = m_DropDowns;
            for (int k = 0; k < list.Count; ++k) {
                list[k].Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            UnityEngine.GameObject obj = m_Obj.Value as UnityEngine.GameObject;
            if (null != obj) {
                UiStoryInitializer initer = obj.GetComponent<UiStoryInitializer>();
                if (null != initer) {
                    UiStoryEventHandler handler = obj.GetComponent<UiStoryEventHandler>();
                    if (null == handler) {
                        handler = obj.AddComponent<UiStoryEventHandler>();
                    }
                    if (null != handler) {
                        int ct = m_VarInfos.Count;
                        for (int ix = 0; ix < ct; ++ix) {
                            string name = m_VarInfos[ix].m_VarName.Value;
                            string path = m_VarInfos[ix].m_ControlPath.Value;
                            UnityEngine.GameObject ctrl = Utility.FindChildObjectByPath(obj, path);
                            AddVariable(instance, name, ctrl);
                        }

                        handler.WindowName = initer.WindowName;
                        var list = m_Inputs;
                        for (int k = 0; k < list.Count; ++k) {
                            string path = list[k].Value;
                            var comp = Utility.FindComponentInChildren<UnityEngine.UI.InputField>(obj, path);
                            handler.InputLabels.Add(comp);
                        }
                        list = m_Toggles;
                        for (int k = 0; k < list.Count; ++k) {
                            string path = list[k].Value;
                            var comp = Utility.FindComponentInChildren<UnityEngine.UI.Toggle>(obj, path);
                            handler.InputToggles.Add(comp);
                        }
                        list = m_Sliders;
                        for (int k = 0; k < list.Count; ++k) {
                            string path = list[k].Value;
                            var comp = Utility.FindComponentInChildren<UnityEngine.UI.Slider>(obj, path);
                            handler.InputSliders.Add(comp);
                        }
                        list = m_DropDowns;
                        for (int k = 0; k < list.Count; ++k) {
                            string path = list[k].Value;
                            var comp = Utility.FindComponentInChildren<UnityEngine.UI.Dropdown>(obj, path);
                            handler.InputDropdowns.Add(comp);
                        }

                        ct = m_EventInfos.Count;
                        for (int ix = 0; ix < ct; ++ix) {
                            string evt = m_EventInfos[ix].m_Event.Value;
                            string tag = m_EventInfos[ix].m_Tag.Value;
                            string path = m_EventInfos[ix].m_Path.Value;
                            if (evt == "button") {
                                UnityEngine.UI.Button button = Utility.FindComponentInChildren<UnityEngine.UI.Button>(obj, path);
                                button.onClick.AddListener(() => { handler.OnClickHandler(tag); });
                            } else if (evt == "toggle") {
                                UnityEngine.UI.Toggle toggle = Utility.FindComponentInChildren<UnityEngine.UI.Toggle>(obj, path);
                                toggle.onValueChanged.AddListener((bool val) => { handler.OnToggleHandler(tag, val); });
                            } else if (evt == "dropdown") {
                                UnityEngine.UI.Dropdown dropdown = Utility.FindComponentInChildren<UnityEngine.UI.Dropdown>(obj, path);
                                dropdown.onValueChanged.AddListener((int val) => { handler.OnDropdownHandler(tag, val); });
                            } else if (evt == "slider") {
                                UnityEngine.UI.Slider slider = Utility.FindComponentInChildren<UnityEngine.UI.Slider>(obj, path);
                                slider.onValueChanged.AddListener((float val) => { handler.OnSliderHandler(tag, val); });
                            } else if (evt == "input") {
                                UnityEngine.UI.InputField input = Utility.FindComponentInChildren<UnityEngine.UI.InputField>(obj, path);
                                input.onEndEdit.AddListener((string val) => { handler.OnInputHandler(tag, val); });
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Obj.InitFromDsl(callData.GetParam(0));
            }
        }

        protected override void Load(Dsl.FunctionData funcData)
        {
            Load(funcData.Call);
            for (int i = 0; i < funcData.Statements.Count; ++i) {
                Dsl.CallData callData = funcData.Statements[i] as Dsl.CallData;
                if (null != callData) {
                    string id = callData.GetId();
                    if (id == "var") {
                        LoadVar(callData);
                    } else if (id == "onevent") {
                        LoadEvent(callData);
                    } else if (id == "inputs") {
                        LoadPaths(m_Inputs, callData);
                    } else if (id == "toggles") {
                        LoadPaths(m_Toggles, callData);
                    } else if (id == "sliders") {
                        LoadPaths(m_Sliders, callData);
                    } else if (id == "dropdowns") {
                        LoadPaths(m_DropDowns, callData);
                    }
                }
            }
        }

        private void LoadVar(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                VarInfo info = new VarInfo();
                info.m_VarName.InitFromDsl(callData.GetParam(0));
                info.m_ControlPath.InitFromDsl(callData.GetParam(1));
                m_VarInfos.Add(info);
            }
        }

        private void LoadEvent(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                EventInfo info = new EventInfo();
                info.m_Event.InitFromDsl(callData.GetParam(0));
                info.m_Tag.InitFromDsl(callData.GetParam(1));
                info.m_Path.InitFromDsl(callData.GetParam(2));
                m_EventInfos.Add(info);
            }
        }

        private class VarInfo
        {
            internal IStoryValue<string> m_VarName = null;
            internal IStoryValue<string> m_ControlPath = null;

            internal VarInfo() 
            {
                m_VarName = new StoryValue<string>();
                m_ControlPath = new StoryValue<string>();
            }
            internal VarInfo(VarInfo other)
            {
                m_VarName = other.m_VarName.Clone();
                m_ControlPath = other.m_ControlPath.Clone();
            }
        }
        private class EventInfo
        {
            internal IStoryValue<string> m_Event = null;
            internal IStoryValue<string> m_Tag = null;
            internal IStoryValue<string> m_Path = null;

            internal EventInfo()
            {
                m_Event = new StoryValue<string>();
                m_Tag = new StoryValue<string>();
                m_Path = new StoryValue<string>();
            }
            internal EventInfo(EventInfo other)
            {
                m_Event = other.m_Event.Clone();
                m_Tag = other.m_Tag.Clone();
                m_Path = other.m_Path.Clone();
            }
        }

        private IStoryValue<object> m_Obj = new StoryValue();
        private List<VarInfo> m_VarInfos = new List<VarInfo>();
        internal List<IStoryValue<string>> m_Inputs = new List<IStoryValue<string>>();
        internal List<IStoryValue<string>> m_Toggles = new List<IStoryValue<string>>();
        internal List<IStoryValue<string>> m_Sliders = new List<IStoryValue<string>>();
        internal List<IStoryValue<string>> m_DropDowns = new List<IStoryValue<string>>();
        private List<EventInfo> m_EventInfos = new List<EventInfo>();
        
        private static void LoadPaths(List<IStoryValue<string>> List, Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> path = new StoryValue<string>();
                path.InitFromDsl(callData.GetParam(i));
                List.Add(path);
            }
        }
        private static void AddVariable(StoryInstance instance, string name, UnityEngine.GameObject control)
        {
            instance.SetVariable(name, control);
            UnityEngine.UI.Text text = control.GetComponent<UnityEngine.UI.Text>();
            if (null != text) {
                instance.SetVariable(string.Format("{0}_Text", name), text);
            }
            UnityEngine.UI.Image image = control.GetComponent<UnityEngine.UI.Image>();
            if (null != image) {
                instance.SetVariable(string.Format("{0}_Image", name), image);
            }
            UnityEngine.UI.RawImage rawImage = control.GetComponent<UnityEngine.UI.RawImage>();
            if (null != rawImage) {
                instance.SetVariable(string.Format("{0}_RawImage", name), rawImage);
            }
            UnityEngine.UI.Button button = control.GetComponent<UnityEngine.UI.Button>();
            if (null != button) {
                instance.SetVariable(string.Format("{0}_Button", name), button);
            }
            UnityEngine.UI.Dropdown dropdown = control.GetComponent<UnityEngine.UI.Dropdown>();
            if (null != dropdown) {
                instance.SetVariable(string.Format("{0}_Dropdown", name), dropdown);
            }
            UnityEngine.UI.InputField inputField = control.GetComponent<UnityEngine.UI.InputField>();
            if (null != inputField) {
                instance.SetVariable(string.Format("{0}_Input", name), inputField);
            }
            UnityEngine.UI.Slider slider = control.GetComponent<UnityEngine.UI.Slider>();
            if (null != inputField) {
                instance.SetVariable(string.Format("{0}_Slider", name), slider);
            }
            UnityEngine.UI.Toggle toggle = control.GetComponent<UnityEngine.UI.Toggle>();
            if (null != toggle) {
                instance.SetVariable(string.Format("{0}_Toggle", name), toggle);
            }
            UnityEngine.UI.ToggleGroup toggleGroup = control.GetComponent<UnityEngine.UI.ToggleGroup>();
            if (null != toggleGroup) {
                instance.SetVariable(string.Format("{0}_ToggleGroup", name), toggleGroup);
            }
            UnityEngine.UI.Scrollbar scrollbar = control.GetComponent<UnityEngine.UI.Scrollbar>();
            if (null != scrollbar) {
                instance.SetVariable(string.Format("{0}_Scrollbar", name), scrollbar);
            }
        }
    }
}
