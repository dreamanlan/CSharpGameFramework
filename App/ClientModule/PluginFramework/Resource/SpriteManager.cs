using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameFramework
{
    public class SpriteManager
    {
        public static void Init()
        {
            m_AllActorIcons = Resources.LoadAll<Sprite>("UITexture/ActorIcons");
            m_AllActorBigIcons = Resources.LoadAll<Sprite>("UITexture/ActorBigIcon");
            m_AllSkillIcons = Resources.LoadAll<Sprite>("UITexture/SkillIcons");
        }

        public static Sprite GetActorIcon(int id)
        {
            if (id >= 0 && id < m_AllActorIcons.Length) {
                return m_AllActorIcons[id];
            }
            return null;
        }
        public static Sprite GetActorBigIcon(int id)
        {
            if (id >= 0 && id < m_AllActorBigIcons.Length) {
                return m_AllActorBigIcons[id];
            }
            return null;
        }
        public static Sprite GetSkillIcon(int id)
        {
            if (id >= 0 && id < m_AllSkillIcons.Length) {
                return m_AllSkillIcons[id];
            }
            return null;
        }

        public static int GetActorIconIndex(Sprite obj)
        {
            return System.Array.IndexOf(m_AllActorIcons, obj);
        }
        public static int GetActorBigIconIndex(Sprite obj)
        {
            return System.Array.IndexOf(m_AllActorBigIcons, obj);
        }
        public static int GetSkillIconIndex(Sprite obj)
        {
            return System.Array.IndexOf(m_AllSkillIcons, obj);
        }

        private static Sprite[] m_AllActorIcons;
        private static Sprite[] m_AllActorBigIcons;
        private static Sprite[] m_AllSkillIcons;
    }
}
