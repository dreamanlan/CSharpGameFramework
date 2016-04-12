using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// replaceshaderandfadecolor(starttime,remaintime,"shadername",startcolor,changecolor[,changetime]);
    /// </summary>
    class ReplaceShaderAndFadeColorTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            ReplaceShaderAndFadeColorTrigger copy = new ReplaceShaderAndFadeColorTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_RemainTime = m_RemainTime;
            copy.m_ShaderName = m_ShaderName;
            copy.m_StartColor = m_StartColor;
            copy.m_ChangeColor = m_ChangeColor;
            copy.m_ChangeTime = m_ChangeTime;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }
        public override void Reset()
        {
            RestoreMaterials();
            m_RealStartTime = m_StartTime;
            m_IsFinalColor = false;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) {
                RestoreMaterials();
                return false;
            }
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                RestoreMaterials();
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (curSectionTime > m_RealStartTime + m_RemainTime) {
                RestoreMaterials();
                return false;
            }
            if (m_Materials.Count <= 0) {
                GameObject shadow = Utility.FindChildObject(obj, "shadow");
                GameObject[] objs = new GameObject[] { obj, shadow };
                for (int ix = 0; ix < objs.Length; ++ix) {
                    GameObject _obj = objs[ix];
                    if (null != _obj) {
                        Renderer[] renderers = _obj.GetComponentsInChildren<Renderer>();
                        if (null != renderers) {
                            for (int i = 0; i < renderers.Length; ++i) {
                                Material mat = renderers[i].material;
                                m_Shaders.Add(mat.shader);
                                if (mat.HasProperty("_Color")) {
                                    m_Colors.Add(mat.color);
                                } else {
                                    m_Colors.Add(Color.white);
                                }
                                m_Materials.Add(mat);

                                if (mat.shader.name == "Tut/Shader/Toon/toon" || mat.shader.name == "Standard" || mat.shader.name == "Mobile/Diffuse" || mat.shader.name == "Unlit/Transparent" || mat.shader.name == "LOLL/Toon/toonwithStencil") {
                                } else {
                                    continue;
                                }
                                mat.shader = Shader.Find(m_ShaderName);
                                mat.color = m_StartColor;
                            }
                        }
                    }
                }
            } else {
                if (curSectionTime > m_RealStartTime + m_ChangeTime) {
                    if (!m_IsFinalColor) {
                        m_IsFinalColor = true;
                        for (int i = 0; i < m_Materials.Count; ++i) {
                            m_Materials[i].color = m_StartColor + m_ChangeColor;
                        }
                    }
                } else {
                    for (int i = 0; i < m_Materials.Count; ++i) {
                        m_Materials[i].color = m_StartColor + m_ChangeColor * ((curSectionTime - m_RealStartTime) / (m_ChangeTime * 1.0f));
                    }
                }
            }
            return true;
        }
        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 5) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RemainTime = long.Parse(callData.GetParamId(1));
                m_ShaderName = callData.GetParamId(2);
                m_StartColor = DslUtility.CalcColor(callData.GetParam(3) as Dsl.CallData);
                m_ChangeColor = DslUtility.CalcColor(callData.GetParam(4) as Dsl.CallData);
            }
            if (num >= 6) {
                m_ChangeTime = long.Parse(callData.GetParamId(5));
            }
            m_RealStartTime = m_StartTime;
        }
        
        private void RestoreMaterials()
        {
            int ct = m_Materials.Count;
            if (m_Shaders.Count != ct || m_Colors.Count != ct) {
                GameFramework.LogSystem.Error("replaceshaderandfadecolor materials num is not equal shaders num !");
                return;
            }
            for (int i = 0; i < ct; ++i) {
                m_Materials[i].shader = m_Shaders[i];
                if (m_Materials[i].HasProperty("_Color")) {
                    m_Materials[i].color = m_Colors[i];
                }
            }
            m_Materials.Clear();
            m_Shaders.Clear();
            m_Colors.Clear();
        }

        private long m_RemainTime = 0;
        private string m_ShaderName = "";
        private Color m_StartColor = Color.white;
        private Color m_ChangeColor = Color.white;
        private long m_ChangeTime = 1000;

        private long m_RealStartTime = 0;

        private bool m_IsFinalColor = false;
        private List<Material> m_Materials = new List<Material>();
        private List<Shader> m_Shaders = new List<Shader>();
        private List<Color> m_Colors = new List<Color>();
    }
    /// <summary>
    /// fadecolor(starttime,remaintime,"path","shadername",startcolor,changecolor[,changetime]);
    /// </summary>
    class FadeColorTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            FadeColorTrigger copy = new FadeColorTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_RemainTime = m_RemainTime;
            copy.m_GoPath = m_GoPath;
            copy.m_ShaderName = m_ShaderName;
            copy.m_StartColor = m_StartColor;
            copy.m_ChangeColor = m_ChangeColor;
            copy.m_ChangeTime = m_ChangeTime;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }
        public override void Reset()
        {
            m_material = null;
            m_RealStartTime = m_StartTime;
            m_IsFinalColor = false;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (curSectionTime > m_RealStartTime + m_RemainTime) {
                return false;
            }
            if (m_material == null) {
                Transform tf = obj.transform.Find(m_GoPath);
                if (tf != null) {
                    SkinnedMeshRenderer smr = tf.GetComponent<SkinnedMeshRenderer>();
                    if (smr != null) {
                        int count = smr.materials.Length;
                        Material material = null;
                        for (int i = 0; i < count; ++i) {
                            material = smr.materials[i];
                            if (material != null && material.shader != null) {
                                if (material.shader.name.CompareTo(m_ShaderName) == 0) {
                                    m_material = material;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (m_material != null) {
                if (curSectionTime > m_RealStartTime + m_ChangeTime) {
                    if (!m_IsFinalColor) {
                        m_IsFinalColor = true;
                        m_material.color = m_StartColor + m_ChangeColor;
                    }
                } else {
                    m_material.color = m_StartColor + m_ChangeColor * ((curSectionTime - m_RealStartTime) / (m_ChangeTime * 1.0f));
                }
            }
            return true;
        }
        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 6) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RemainTime = long.Parse(callData.GetParamId(1));
                m_GoPath = callData.GetParamId(2);
                m_ShaderName = callData.GetParamId(3);
                m_StartColor = DslUtility.CalcColor(callData.GetParam(4) as Dsl.CallData);
                m_ChangeColor = DslUtility.CalcColor(callData.GetParam(5) as Dsl.CallData);
            }
            if (num >= 7) {
                m_ChangeTime = long.Parse(callData.GetParamId(6));
            }
            m_RealStartTime = m_StartTime;
        }

        private long m_RemainTime = 0;
        private Material m_material = null;
        private string m_GoPath = "";
        private string m_ShaderName = "";
        private Color m_StartColor = Color.white;
        private Color m_ChangeColor = Color.white;
        private long m_ChangeTime = 1000;

        private bool m_IsFinalColor = false;
        private long m_RealStartTime = 0;
    }
}