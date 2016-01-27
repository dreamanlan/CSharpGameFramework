using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace GameFramework
{
    public class ImpactInfo
    {
        public int Seq = 0;                        // 顺序号（添加到Character上时生成，在Character实例上唯一）
        public int ImpactId = -1;                  // 效果ID
        public int SkillId = -1;                   // 技能ID
        public int ImpactSenderId = -1;            // 效果触发者的Id
        public Vector3 SenderPosition;
        public int TargetType = 0;

        public long StartTime = 0;                 // 效果开始生效起始时间
        public int DurationTime = 0;               // 持续时间
        public float Damage = 0;
        public int HpRecover = 0;
        public int MpRecover = 0;
        public int AddShield = 0;
        public int AddAttack = 0;
        public int AddDefence = 0;
        public float AddRps = 0;
        public float AddCritical = 0;
        public float AddCriticalPow = 0;
        public float AddSpeed = 0;

        public int ImpactToTarget = 0;        
        public TableConfig.Skill ConfigData;

        public ImpactInfo(int impactId)
        {
            ImpactId = impactId;
            ConfigData = TableConfig.SkillProvider.Instance.GetSkill(ImpactId);
        }
        public ImpactInfo(TableConfig.Skill cfg)
        {
            if (null != cfg) {
                ImpactId = cfg.id;
            }
            ConfigData = cfg;
        }

        public void RefixCharacterProperty(EntityInfo entity)
        {
            if (AddAttack != 0) {
                entity.GetActualProperty().SetAttackBase(Operate_Type.OT_Relative, AddAttack);
            }
            if (AddDefence != 0) {
                entity.GetActualProperty().SetDefenceBase(Operate_Type.OT_Relative, AddDefence);
            }
            if (AddRps != 0) {
                entity.GetActualProperty().SetRps(Operate_Type.OT_Relative, AddRps);
            }
            if (AddCritical != 0) {
                entity.GetActualProperty().SetCritical(Operate_Type.OT_Relative, AddCritical);
            }
            if (AddCriticalPow != 0) {
                entity.GetActualProperty().SetCriticalPow(Operate_Type.OT_Relative, AddCriticalPow);
            }
            if (Math.Abs(AddSpeed) > Geometry.c_FloatPrecision) {
                entity.GetActualProperty().SetMoveSpeed(Operate_Type.OT_Relative, AddSpeed);
            }
        }
    }
}
