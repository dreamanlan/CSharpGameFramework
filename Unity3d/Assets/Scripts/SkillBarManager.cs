using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Story;

public sealed class SkillBarManager
{
    public void Init()
    {
        GameObject obj = ResourceSystem.Instance.NewObject("UI/SkillBar") as GameObject;
        skillBar = obj.GetComponent<SkillBar>();
        skillBar.Clear();

        //订阅
        subscribes.Add(Utility.EventSystem.Subscribe<int,int>("ui_add_actor_button", "ui", AddSummonButton));//头像
        subscribes.Add(Utility.EventSystem.Subscribe<int>("ui_remove_actor_button", "ui", RemoveSummonButton));
        subscribes.Add(Utility.EventSystem.Subscribe<int, float, int>("ui_actor_mp", "ui", ChangeMp));
        subscribes.Add(Utility.EventSystem.Subscribe<int, float>("ui_actor_hp", "ui", ChangeHp));
        subscribes.Add(Utility.EventSystem.Subscribe<int, int, float>("ui_skill_cooldown", "ui", OnSkillCooldown));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_show", "ui", Show));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_hide", "ui", Hide));

        //发布
        //队员技能
        skillBar.onSelectSkill = (int objID,int skillID) => {
            bool bSuccess = ClientModule.Instance.CastSkill(objID, skillID);
        };
        //召唤师技能
        skillBar.onSelectSummonerSkill = (int skillID) => {
            bool bSuccess = ClientModule.Instance.CastSkill(ClientModule.Instance.LeaderID, skillID);
        };

    }
    public void Release()
    {
        if (null != skillBar) {
            ResourceSystem.Instance.RecycleObject(skillBar.gameObject);
        }
        for (int i = 0; i < subscribes.Count; i++) {
            Utility.EventSystem.Unsubscribe(subscribes[i]);
        }
        subscribes.Clear();
    }

    private void AddSummonButton(int actorID,int objID)
    {
        try {
            TableConfig.Actor actCfg = TableConfig.ActorProvider.Instance.GetActor(actorID);
            if (null != actCfg) {
                TableConfig.Skill skillCfg0 = TableConfig.SkillProvider.Instance.GetSkill(actCfg.skill4);
                if (null != skillCfg0) {
                    SkillBar.SkillInfo skill = skillBar.AddSkill(objID, skillCfg0.id, actCfg.icon, skillCfg0.cooldown);
                    skill.SetMp(ClientModule.Instance.GetNpcMp(objID));
                }

                if (ClientModule.Instance.LeaderID == objID) {
                    TableConfig.Skill skillCfg1 = TableConfig.SkillProvider.Instance.GetSkill(ClientModule.Instance.SummonerSkillId);
                    if (null != skillCfg1) {
                        skillBar.SetSummonerSkill(objID, skillCfg1.id, skillCfg1.icon, 0);
                    }
                }
            }
        } catch (Exception ex) {
            LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void RemoveSummonButton(int objID)
    {
        Debug.Log("RemoveSummonButton,id=" + objID.ToString());
        try {
            SkillBar.SkillInfo skill = skillBar.GetSkillByID(objID);
            if (skill != null) {
                skill.StopEffect();
                skill.SetSkillEmpty();
                skill.SetMp(0);
                skill.StopCooldown(0);
            }
        } catch (Exception ex) {
            LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void OnSkillCooldown(int objID,int skillID,float cooldownTime)
    {
        try {
            SkillBar.SkillInfo skill = skillBar.GetSkillByID(objID);
            if (skill != null) {
                if (skill.objID != ClientModule.Instance.LeaderID) {
                    if (!skill.isEmpty && skill.skillID == skillID) {
                        skill.StartCooldown(cooldownTime);
                    }
                } else {
                    if (skill.skillID == skillID) {
                        skill.StartCooldown(cooldownTime);
                    } else if (skillID == skillBar.summonerSkill.skillID) {
                        skillBar.summonerSkill.StartCooldown(cooldownTime);
                    }
                }
            }
        } catch (Exception ex) {
            LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void ChangeMp(int objID, float mp, int mpChange)
    {
        try {
            SkillBar.SkillInfo skill = skillBar.GetSkillByID(objID);
            if (skill != null) {
                skill.SetMp(mp);
                if (mpChange != 0) {
                    skill.PlayMpAnimation(mpChange);
                }
            }
        } catch (Exception ex) {
            LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void ChangeHp(int objID, float hp)
    {
        try {
            SkillBar.SkillInfo skill = skillBar.GetSkillByID(objID);
            if (skill != null) {
                skill.SetHp(hp);
            }
        } catch (Exception ex) {
            LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    public void Show()
    {
        skillBar.gameObject.SetActive(true);
    }
    public void Hide()
    {
        skillBar.gameObject.SetActive(false);
    }
    public bool IsOn(Vector3 pos)
    {
        if (null != skillBar) {
            int ct = skillBar.GetSkillCount();
            for (int i = 0; i < ct; ++i) {
                SkillBar.SkillInfo skillInfo = skillBar.GetSkillByIndex(i);
                if (null != skillInfo && null!=skillInfo.icon) {
                    SkillIconInfo iconInfo = skillInfo.icon;
                    RectTransform rect = iconInfo.button.transform as RectTransform;
                    if (null != rect) {
                        if(RectTransformUtility.RectangleContainsScreenPoint(rect, new Vector2(pos.x, pos.y)))
                            return true;
                    }
                }
            }
            SkillIconInfo iconInfo2 = skillBar.summonerSkillIcon;
            if (null != iconInfo2) {
                RectTransform rect = iconInfo2.button.transform as RectTransform;
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, new Vector2(pos.x, pos.y)))
                    return true;
            }
        }
        return false;
    }

    private SkillBar skillBar;
    private List<object> subscribes = new List<object>();

    public static SkillBarManager Instance
    {
        get
        {
            return s_Instance;
        }
    }
    private static SkillBarManager s_Instance = new SkillBarManager();
}
