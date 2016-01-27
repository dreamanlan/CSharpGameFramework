using System;
using System.Collections.Generic;

namespace GameFramework
{
  /**
   * @brief 角色属性类
   */
  public sealed class CharacterProperty
  {
    /**
     * @brief 构造函数
     *
     * @param owner
     *
     * @return 
     */
    public CharacterProperty()
    {}

    /**
     * @brief 移动速度
     */
    public float MoveSpeed
    {
      get { return m_MoveSpeed / s_Key; }
    }
    /**
     * @brief 行走速度
     */
    public float WalkSpeed
    {
      get { return m_WalkSpeed / s_Key; }
    }
    /**
     * @brief 跑路速度
     */
    public float RunSpeed
    {
      get { return m_RunSpeed / s_Key; }
    }
    /**
     * @brief 最大血量
     */
    public int HpMax
    {
      get { return m_HpMax / s_Key; }
    }

    /**
     * @brief 最大血量
     */
    public int RageMax
    {
      get { return m_RageMax / s_Key; }
    }

    /**
     * @brief 最大能量
     */
    public int EnergyMax
    {
      get { return m_EnergyMax / s_Key; }
    }

    /**
     * @brief 生命值回复速度
     */
    public float HpRecover
    {
      get { return m_HpRecover / s_Key; }
    }

    /**
     * @brief 能量值回复速度
     */
    public float EnergyRecover
    {
      get { return m_EnergyRecover / s_Key; }
    }

    /**
     * @brief 物理攻击力
     */
    public int AttackBase
    {
        get { return m_AttackBase / s_Key; }
    }

    /**
     * @brief 物理防御力
     */
    public int DefenceBase
    {
      get { return m_DefenceBase / s_Key; }
    }
    
    /**
     * @brief 暴击率
     */
    public float Critical
    {
      get { return m_Critical / s_Key; }
    }

    /**
     * @brief 暴击额外伤害比率
     */
    public float CriticalPow
    {
      get { return m_CriticalPow / s_Key; }
    }

    /**
     * @brief 攻击速度
     */
    public float Rps
    {
      get { return m_Rps / s_Key; }
    }

    /**
     * @brief 攻击距离
     */
    public float AttackRange
    {
      get { return m_AttackRange / s_Key; }
    }

    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetMoveSpeed(Operate_Type opType, float tVal)
    {
      m_MoveSpeed = UpdateAttr(m_MoveSpeed, m_MoveSpeed, opType, tVal);
    }
    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetWalkSpeed(Operate_Type opType, float tVal)
    {
      m_WalkSpeed = UpdateAttr(m_WalkSpeed, m_WalkSpeed, opType, tVal);
    }
    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetRunSpeed(Operate_Type opType, float tVal)
    {
      m_RunSpeed = UpdateAttr(m_RunSpeed, m_RunSpeed, opType, tVal);
    }
    
    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetHpMax(Operate_Type opType, int tVal)
    {
      m_HpMax = (int)UpdateAttr(m_HpMax, m_HpMax, opType, tVal);
    }

    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetRageMax(Operate_Type opType, int tVal)
    {
      m_RageMax = (int)UpdateAttr(m_RageMax, m_RageMax, opType, tVal);
    }

    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetEnergyMax(Operate_Type opType, int tVal)
    {
      m_EnergyMax = (int)UpdateAttr(m_EnergyMax, m_EnergyMax, opType, tVal);
    }

    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetHpRecover(Operate_Type opType, float tVal)
    {
      m_HpRecover = UpdateAttr(m_HpRecover, opType, tVal);
    }
    
    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetEnergyRecover(Operate_Type opType, float tVal)
    {
      m_EnergyRecover = UpdateAttr(m_EnergyRecover, opType, tVal);
    }

    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetAttackBase(Operate_Type opType, int tVal)
    {
      m_AttackBase = (int)UpdateAttr(m_AttackBase, opType, tVal);
    }

    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetDefenceBase(Operate_Type opType, int tVal)
    {
      m_DefenceBase = (int)UpdateAttr(m_DefenceBase, opType, tVal);
    }
        
    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetCritical(Operate_Type opType, float tVal)
    {
      m_Critical = UpdateAttr(m_Critical, opType, tVal);
    }

    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetCriticalPow(Operate_Type opType, float tVal)
    {
      m_CriticalPow = UpdateAttr(m_CriticalPow, opType, tVal);
    }

    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetRps(Operate_Type opType, float tVal)
    {
      m_Rps = UpdateAttr(m_Rps, opType, tVal);
    }

    /**
     * @brief 角色属性修改
     *
     * @param optype 操作类型
     * @param val 值
     *
     */
    public void SetAttackRange(Operate_Type opType, float tVal)
    {
      m_AttackRange = UpdateAttr(m_AttackRange, opType, tVal);
    }

    public static float UpdateAttr(float val, float maxVal, Operate_Type opType, float tVal)
    {
      float ret = val;
      if (opType == Operate_Type.OT_PercentMax) {
        float t = maxVal * (tVal / 100.0f);
        ret = t;
      } else {
        ret = UpdateAttr(val, opType, tVal);
      }
      return ret;
    }

    public static float UpdateAttr(float val, Operate_Type opType, float tVal)
    {
      float ret = val;
      if (opType == Operate_Type.OT_Absolute) {
        ret = tVal * s_Key;
      } else if (opType == Operate_Type.OT_Relative) {
        float t = (ret + tVal * s_Key);
        if (t < 0) {
          t = 0;
        }
        ret = t;
      } else if (opType == Operate_Type.OT_PercentCurrent) {
        float t = (ret * (tVal / 100.0f));
        ret = t;
      }
      return ret;
    }

    /**
     * @brief 奔跑速度
     */
    private float m_MoveSpeed;
    /**
     * @brief 走路速度
     */
    private float m_WalkSpeed;
    /**
     * @brief 跑路速度
     */
    private float m_RunSpeed;
    /**
     * @brief 最大生命值
     */
    private int m_HpMax;
    /**
     * @brief 最大怒气值
     */
    private int m_RageMax;
    /**
     * @brief 最大能量值
     */
    private int m_EnergyMax;
    /**
     * @brief 生命值回复速度
     */
    private float m_HpRecover;
    /**
     * @brief 能量值回复速度
     */
    private float m_EnergyRecover;
    /**
     * @brief 物理攻击力
     */
    private int m_AttackBase;
    /**
     * @brief 物理防御力
     */
    private int m_DefenceBase;
    /**
     * @brief 暴击率
     */
    private float m_Critical;
    /**
     * @brief 暴击额外伤害比率
     */
    private float m_CriticalPow;
    /**
     * @brief 攻击速度
     */
    private float m_Rps;
    /**
     * @brief 攻击距离
     */
    private float m_AttackRange;

    //------------------------------------------------------------------------
    //注意：Key的修改应该在所有对象创建前执行，否则属性会乱！！！
    //------------------------------------------------------------------------
    public static int Key
    {
      get { return s_Key; }
      set { s_Key = value; }
    }

    private static int s_Key = 1;
  }
}
