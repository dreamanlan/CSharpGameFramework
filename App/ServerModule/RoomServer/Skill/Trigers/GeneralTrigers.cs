using System;
using System.Collections.Generic;
using ScriptRuntime;
using SkillSystem;
using GameFramework;
using GameFramework.Story;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// bornfinish(start_time);
    /// </summary>
    internal class BornFinishTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            BornFinishTriger triger = new BornFinishTriger();
            
                        return triger;
        }

        public override void Reset()
        {
            
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime >= StartTime) {
                scene.EntityController.BornFinish(senderObj.ActorId);
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
            
        }

        
    }
    /// <summary>
    /// deadfinish(start_time);
    /// </summary>
    internal class DeadFinishTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            DeadFinishTriger triger = new DeadFinishTriger();
            
                        return triger;
        }

        public override void Reset()
        {
            
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime >= StartTime) {
                scene.EntityController.DeadFinish(senderObj.ActorId);
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
            
        }

        
    }
    /// <summary>
    /// sendstorymessage(start_time,msg,arg1,arg2,arg3,...);
    /// </summary>
    public class SendStoryMessageTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            SendStoryMessageTrigger copy = new SendStoryMessageTrigger();
            
            copy.m_Msg = m_Msg;
            copy.m_Args.AddRange(m_Args);
                        return copy;
        }

        public override void Reset()
        {
            
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            List<object> args = new List<object>();
            args.Add(senderObj);
            for (int i = 0; i < m_Args.Count; ++i) {
                args.Add(m_Args[i]);
            }
            scene.StorySystem.SendMessage(m_Msg, args.ToArray());
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                StartTime = int.Parse(callData.GetParamId(0));
                m_Msg = callData.GetParamId(1);
            }
            for (int i = 2; i < num; ++i) {
                m_Args.Add(callData.GetParamId(i));
            }
            
        }

        private string m_Msg = string.Empty;
        private List<string> m_Args = new List<string>();

        
    }
}
