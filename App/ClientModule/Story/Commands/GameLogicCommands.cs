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
                EntityInfo leader = ClientModule.Instance.GetEntityById(ClientModule.Instance.LeaderID);
                if (leader != null) {
                    ClientModule.Instance.LeaderLinkID = leader.GetLinkId();
                }
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
}
