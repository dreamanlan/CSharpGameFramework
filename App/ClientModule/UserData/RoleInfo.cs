using System;
using System.Collections.Generic;

namespace GameFramework
{
    public enum GuideActionEnum : int
    {
        Gow = 1,//战神赛
        Arena = 2,//名人战
        Expedition = 3,//远征
        Attempt = 4,//试炼
        Gold = 5,//刷金
        Moba = 7,//攻城
        Platform = 8,//平台防御
        Partner = 9,//同伴争取
        TalentDungeon = 10,//天赋猎区
        SecretArea = 11,//秘境、挂机
        Pair = 12,//双人本
        Loot = 13
    }
    public class Teammate
    {
        public string Nick { get; set; }
        public int ResId { get; set; }
        public int Money { get; set; }
        public float TotalDamage { get; set; }
        public int ReliveTime { get; set; }
        public int HitCount { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public bool IsTimeFit { get; set; }
        public bool IsHitCountFit { get; set; }
    }
    public enum BattleResultEnum : int
    {
        Win,      //胜利
        Lost,     //失败
        Unfinish, //未完成
    }
    public enum Difficulty : int
    {
        None = -1,      //秘境无
        Normal = 0,     //秘境普通难度
        Hard = 1,      //秘境困难难度
        Hell = 2,      //秘境地狱难度
    }
    public class RoleInfo
    {
        public RoleInfo()
        {
        }
        public ulong Guid
        {
            get { return m_Guid; }
            set { m_Guid = value; }
        }
        public string Nickname
        {
            get { return m_Nickname; }
            set { m_Nickname = value; }
        }
        public int HeroId
        {
            get { return m_HeroId; }
            set { m_HeroId = value; }
        }
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }
        public int SceneId
        {
            get { return m_SceneId; }
            set { m_SceneId = value; }
        }
        // 角色GUID
        private ulong m_Guid = 0;
        // 角色昵称               
        private string m_Nickname;
        // 角色职业               
        private int m_HeroId = 0;
        // 角色等级      
        private int m_Level = 0;
        // 所在的场景ID
        private int m_SceneId = 0;
    }
}
