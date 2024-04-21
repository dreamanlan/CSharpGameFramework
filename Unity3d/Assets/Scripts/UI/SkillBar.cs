using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using GameFramework;

public class SkillBar : MonoBehaviour
{
    public class SkillInfo
    {
        public int objID;
        public int skillID;
        public SkillIconInfo icon;

        public float cooldownTime;
        public bool isCoolingDown;
        public float timeToStartCooldown;
        public float timeToEndCooldown;

        public bool isEmpty;
        public bool forcePlayParticle = false;

        private float timeToHideMpChange;
        private bool isPlayingMpChange;

        public void SetSkillEmpty()
        {
            isEmpty = true;            
            icon.deadMask.gameObject.SetActive(true);
        }
        public void SetMp(float mp)
        {
            if (icon.rage != null) {
                icon.rage.value = mp;
            }
        }
        public float GetMp()
        {
            if (icon.rage != null) {
                return icon.rage.value;
            }
            return 1;
        }
        public void SetHp(float hp)
        {
            if (icon.health != null) {
                icon.health.value = hp;
            }
        }
        public void StartCooldown()
        {
            isCoolingDown = true;
            timeToStartCooldown = Time.time;
            timeToEndCooldown = Time.time + cooldownTime;
        }
        public void StartCooldown(float cooldownTime)
        {
            this.cooldownTime = cooldownTime;
            isCoolingDown = true;
            timeToStartCooldown = Time.time;
            timeToEndCooldown = Time.time + cooldownTime;
        } 

        public void StopCooldown(float value)
        {
            isCoolingDown = false;
            timeToStartCooldown = 0;
            timeToEndCooldown = 0;
            icon.coolDown.fillAmount = 0;
        }
        public void PlayEffect()
        {
            icon.particle.gameObject.SetActive(true);
        }
        public bool IsPlayingEffect()
        {
            return icon.particle.gameObject.activeInHierarchy;
        }
        public void StopEffect()
        {
            icon.particle.gameObject.SetActive(false);
        }
        public void Update()
        {
            icon.coolDown.fillAmount = 1 - (Time.time - timeToStartCooldown) / (timeToEndCooldown - timeToStartCooldown);
            if (Time.time > timeToEndCooldown) {
                isCoolingDown = false;
            }

            bool bCanFindSkillTarget = true;
            if (icon.disableMask) {
                bCanFindSkillTarget = PluginFramework.Instance.SkillCanFindTarget(objID, skillID);
            }
            bool bSkillIsReady = GetMp() > 0.999f && isCoolingDown == false && !isEmpty;
            bool bNeedPlayerEffect = (bSkillIsReady && bCanFindSkillTarget) || forcePlayParticle;
            if (!IsPlayingEffect()) {
                if (bNeedPlayerEffect) {
                    PlayEffect();
                }
            }
            else {
                if (!bNeedPlayerEffect) {
                    StopEffect();
                }
            }

            if (isPlayingMpChange && Time.time > timeToHideMpChange) {
                isPlayingMpChange = false;
                icon.mpChange.gameObject.SetActive(false);
            }

            // Can release the ultimate move, but when there is no target, it will be grayed out.
            if (icon.disableMask)
            {
                icon.disableMask.gameObject.SetActive(bSkillIsReady && !bCanFindSkillTarget && !forcePlayParticle);
            }

        }

        public void PlayMpAnimation(int mp)
        {
            Animation anim = icon.mpChange.gameObject.GetComponent<Animation>();
            if (null != anim) {
                icon.mpChange.text = mp.ToString();
                icon.mpChange.gameObject.SetActive(true);
                anim.Play();
                isPlayingMpChange = true;
                timeToHideMpChange = Time.time + anim.clip.length;
            }
        }
    }

    //Bind controls
    public GameObject panelSkill;
    public GameObject prefabSkillIcon;

    //Callback outer layer
    public delegate void SelectSkillFun(int objID,int skillID);
    public SelectSkillFun onSelectSkill;
    public delegate void FunOnDestroy();
    public FunOnDestroy onDestroy;
    
    private List<SkillInfo> skills = new List<SkillInfo>();

    public void Start() {
        
    }
    public void OnDestroy(){if (onDestroy != null) onDestroy();}

    public void Clear() {
        foreach (var skill in skills) {
            GameObject.Destroy(skill.icon.gameObject);
        }
        skills.Clear();
    }
    public SkillInfo AddSkill(int objID,int skillID, int icon,float cooldownTime,bool canSelect = true)
    {
        GameObject panelObj = Utility.AttachUIAsset(panelSkill, prefabSkillIcon, "SkillIcon");

        SkillInfo skill = new SkillInfo();
        skill.icon = panelObj.GetComponent<SkillIconInfo>();
        skill.icon.gameObject.SetActive(true);

        skill.icon.head.sprite = SpriteManager.GetSkillIcon(icon);
        skill.icon.deadMask.gameObject.SetActive(false);
        skill.icon.button.enabled = canSelect;
        skill.objID = objID;
        skill.cooldownTime = cooldownTime;
        skill.skillID = skillID;
        skill.icon.skillIdTxt.text = skillID.ToString();

        skills.Add(skill);
        UIEventTrigger.Get(skill.icon.button).onClick = () => {
            if (!skill.isCoolingDown && !skill.isEmpty) {
                onSelectSkill(objID,skillID);
            }
        };
        RefreshSkillPosition();
        return skill;
    }
    private void RefreshSkillPosition() {
        int ct = skills.Count;
        for (int i = 0; i < ct; i++) {
            Vector3 pos = skills[i].icon.transform.localPosition;
            pos.x = (ct / 2 - i) * 86 - 40;
            skills[i].icon.transform.localPosition = pos;
        }
    }
    public void RemoveSkill(int objID) {
        SkillInfo skill = GetSkillByID(objID) ;
        if (skill != null) {
            GameObject.Destroy(skill.icon.gameObject);
            skills.Remove(skill);
        }
    }
    public void RemoveAllSkills()
    {
        for (int i = 0; i < skills.Count; i++) {
            GameObject.Destroy(skills[i].icon.gameObject);
        }
        skills.Clear();
    }

    public int GetSkillCount()
    {
        return skills.Count;
    }
    public SkillInfo GetSkillByIndex(int index)
    {
        if (index >= 0 && index < skills.Count) {
            return skills[index];
        }
        return null;
    }
    public SkillInfo GetSkillByID(int objID) {
        for (int i = 0; i < skills.Count; i++) {
            if (skills[i].objID == objID) {
                return skills[i];
            }
        }
        return null;
    }

    public void Update() {
        foreach (SkillInfo skill in skills) {
            skill.Update();
        }
    }
}

