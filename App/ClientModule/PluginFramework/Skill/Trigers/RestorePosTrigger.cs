using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// storepos([start_time[, isForRoundMove]]);
    /// </summary>
    public class StorePosTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            StorePosTrigger copy = new StorePosTrigger();
            
            
            copy.m_IsForRoundMove = m_IsForRoundMove;
            return copy;
        }
        public override void Reset()
        {
            
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num >= 2) {
                m_IsForRoundMove = callData.GetParamId(1)=="true";
            }
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            GameObject target = senderObj.TargetGfxObj;
            if (null != senderObj.TrackEffectObj) {
                obj = senderObj.TrackEffectObj;
                target = senderObj.GfxObj;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            if (m_IsForRoundMove) {
                Vector3 srcPos = obj.transform.position;
                Vector3 targetPos = Vector3.zero;
                if (null != target) {
                    targetPos = target.transform.position;
                }           
                TriggerUtil.GetSkillStartPosition(srcPos, senderObj.ConfigData, instance, senderObj.ObjId, senderObj.TargetObjId, ref targetPos);
                if (targetPos.sqrMagnitude > Geometry.c_FloatPrecision) {
                    instance.CustomDatas.AddData<Vector3>(targetPos);
                }
            } else {
                Vector3 pos = obj.transform.position;
                instance.CustomDatas.AddData<Vector3>(pos);
            }
            return false;
        }
        
        private bool m_IsForRoundMove = false;
    }
    /// <summary>
    /// restorepos([start_time]);
    /// </summary>
    public class RestorePosTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            RestorePosTrigger copy = new RestorePosTrigger();
            
            
            return copy;
        }
        public override void Reset()
        {
            
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            if (callData.GetParamNum() >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (null != senderObj.TrackEffectObj) {
                obj = senderObj.TrackEffectObj;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            Vector3 old_pos = instance.CustomDatas.GetData<Vector3>();
            if (old_pos.sqrMagnitude > Geometry.c_FloatPrecision) {
                obj.transform.position = old_pos;
            }
            return false;
        }
        
    }
}
