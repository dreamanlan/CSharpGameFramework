using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Story;

public sealed class SkillBarManager
{
    public void Init(GameObject cameraObj)
    {
        GameObject obj = ResourceSystem.Instance.NewObject("UI/SkillBar") as GameObject;
        Canvas canvas = obj.GetComponent<Canvas>();
        if (null != canvas && null != cameraObj) {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cameraObj.GetComponent<Camera>();
            uiCamera = canvas.worldCamera;
        }
        skillBar = obj.GetComponent<SkillBar>();
        skillBar.Clear();

        //subscribe
        subscribes.Add(Utility.EventSystem.Subscribe<int, int, int>("ui_add_skill_button", "ui", AddSkillButton));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_remove_skill_buttons", "ui", RemoveAllSkillButtons));
        subscribes.Add(Utility.EventSystem.Subscribe<int, float, int>("ui_actor_mp", "ui", ChangeMp));
        subscribes.Add(Utility.EventSystem.Subscribe<int, float>("ui_actor_hp", "ui", ChangeHp));
        subscribes.Add(Utility.EventSystem.Subscribe<int, int, float>("ui_skill_cooldown", "ui", OnSkillCooldown));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_show", "ui", Show));
        subscribes.Add(Utility.EventSystem.Subscribe("ui_hide", "ui", Hide));

        //publish
        //member skill
        skillBar.onSelectSkill = (int objID,int skillID) => {
            Utility.SendMessage("GameRoot", "OnCastSkill", new object[] { objID, skillID });
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

    private void AddSkillButton(int actorID, int objID, int skillId)
    {
        try {
            TableConfig.Actor actCfg = TableConfig.ActorProvider.Instance.GetActor(actorID);
            if (null != actCfg) {
                TableConfig.Skill skillCfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                if (null == skillCfg) {
                    skillCfg = TableConfig.SkillProvider.Instance.GetSkill(actCfg.skill4);
                }
                if (null != skillCfg) {
                    SkillBar.SkillInfo skill = skillBar.AddSkill(objID, skillCfg.id, skillCfg.icon, 0);
                    skill.SetMp(PluginFramework.Instance.GetNpcMp(objID));
                }
            }
        } catch (System.Exception ex) {
            Debug.LogErrorFormat("exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void RemoveAllSkillButtons()
    {
        for (int i = 0; i < skillBar.GetSkillCount(); i++) {
            SkillBar.SkillInfo skill = skillBar.GetSkillByIndex(i);
            if (skill != null) {
                skill.StopEffect();
                skill.SetSkillEmpty();
                skill.SetMp(0);
                skill.StopCooldown(0);
            }
        }
        skillBar.RemoveAllSkills();
    }

    private void OnSkillCooldown(int objID,int skillID,float cooldownTime)
    {
        try {
            SkillBar.SkillInfo skill = skillBar.GetSkillByID(objID);
            if (skill != null) {
                if (skill.objID != PluginFramework.Instance.LeaderId) {
                    if (!skill.isEmpty && skill.skillID == skillID) {
                        skill.StartCooldown(cooldownTime);
                    }
                } else {
                    if (skill.skillID == skillID) {
                        skill.StartCooldown(cooldownTime);
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
            RectTransform rect = skillBar.panelSkill.transform as RectTransform;
            if (null != rect) {
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, new Vector2(pos.x, pos.y), uiCamera))
                    return true;
            }
        }
        return false;
    }

    private SkillBar skillBar;
    private List<object> subscribes = new List<object>();
    private Camera uiCamera = null;

    public static SkillBarManager Instance
    {
        get
        {
            return s_Instance;
        }
    }
    private static SkillBarManager s_Instance = new SkillBarManager();
}
