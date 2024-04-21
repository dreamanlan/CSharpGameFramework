using System;
using System.Collections.Generic;

namespace GameFramework
{
    public enum CampIdEnum : int
    {
        Unkown = 0,
        Friendly,
        Hostile,
        Blue,
        Red,
        FreedomCamp_Begin,
        FreedomCamp_End = 0x7fffffff,
    }

    // relationship
    public enum CharacterRelation : int
    {
        RELATION_INVALID = -1,
        RELATION_ENEMY,				// hostile
        RELATION_FRIEND,			// friendly
        RELATION_NUMBERS
    }

    public enum CharacterPropertyEnum : int
    {
        x1001_基础物理攻击 = 1001,
        x1002_物理攻击成长 = 1002,
        x1003_物理攻击加成 = 1003,
        x1004_额外物理攻击 = 1004,
        x1005_最终物理攻击 = 1005,
        x1006_基础法术攻击 = 1006,
        x1007_法术攻击成长 = 1007,
        x1008_法术攻击加成 = 1008,
        x1009_额外法术攻击 = 1009,
        x1010_最终法术攻击 = 1010,
        x1011_基础物理防御 = 1011,
        x1012_物理防御成长 = 1012,
        x1013_物理防御加成 = 1013,
        x1014_额外物理防御 = 1014,
        x1015_最终物理防御 = 1015,
        x1016_基础法术防御 = 1016,
        x1017_法术防御成长 = 1017,
        x1018_法术防御加成 = 1018,
        x1019_额外法术防御 = 1019,
        x1020_最终法术防御 = 1020,
        x1021_暴击等级 = 1021,
        x1022_抗暴击等级 = 1022,
        x1023_额外暴击率 = 1023,
        x1024_命中等级 = 1024,
        x1025_额外命中率 = 1025,
        x1026_闪避等级 = 1026,
        x1027_额外闪避率 = 1027,
        x1028_破击等级 = 1028,
        x1029_额外破击概率 = 1029,
        x1030_格挡等级 = 1030,
        x1031_额外格挡概率 = 1031,
        x1032_格挡强度 = 1032,
        x1033_暴击伤害 = 1033,
        x1034_暴击伤害减免 = 1034,
        x1035_伤害加成_百分比 = 1035,
        x1036_伤害加成_定值 = 1036,
        x1037_伤害减免_百分比 = 1037,
        x1038_伤害减免_定值 = 1038,
        x1039_忽略物理防御 = 1039,
        x1040_忽略法术防御 = 1040,
        x1041_属性攻击预留 = 1041,
        x1042_属性攻击预留 = 1042,
        x1043_属性攻击预留 = 1043,

        x2001_基础生命 = 2001,
        x2002_生命成长 = 2002,
        x2003_生命加成 = 2003,
        x2004_额外生命 = 2004,
        x2005_最大生命值 = 2005,
        x2006_当前生命值 = 2006,
        x2007_基础速度 = 2007,
        x2008_速度成长 = 2008,
        x2009_速度加成 = 2009,
        x2010_额外速度 = 2010,
        x2011_最终速度 = 2011,
        x2012_护盾值 = 2012,

        x3001_眩晕 = 3001,
        x3002_昏睡 = 3002,
        x3003_倒地 = 3003,
        x3004_减速 = 3004,
        x3005_浮空 = 3005,
        x3006_灼烧 = 3006,
        x3007_感电 = 3007,
        x3008_霸体 = 3008,
        x3009_无敌 = 3009,
        x3010_复活 = 3010,
        x3011_隐身 = 3011,

        x4001_职业 = 4001,
        x4002_阵营 = 4002,
        x4003_品质 = 4003,
        x4004_性别 = 4004,

        x5001_等级 = 5001,
        x5002_经验 = 5002,
        x5003_SC最大值 = 5003,
        x5004_SC当前值 = 5004,
        x5005_UC最大值 = 5005,
        x5006_UC当前值 = 5006,
    }

    public static class CharacterStateUtility
    {
        public static int NameToState(string name)
        {
            if (name == "stun") {
                return (int)CharacterPropertyEnum.x3001_眩晕;
            } else if (name == "sleep") {
                return (int)CharacterPropertyEnum.x3002_昏睡;
            } else if (name == "knockdown") {
                return (int)CharacterPropertyEnum.x3003_倒地;
            } else if (name == "slow") {
                return (int)CharacterPropertyEnum.x3004_减速;
            } else if (name == "float") {
                return (int)CharacterPropertyEnum.x3005_浮空;
            } else if (name == "fire") {
                return (int)CharacterPropertyEnum.x3006_灼烧;
            } else if (name == "shock") {
                return (int)CharacterPropertyEnum.x3007_感电;
            } else if (name == "armor") {
                return (int)CharacterPropertyEnum.x3008_霸体;
            } else if (name == "invincible") {
                return (int)CharacterPropertyEnum.x3009_无敌;
            } else if (name == "revive") {
                return (int)CharacterPropertyEnum.x3010_复活;
            } else {
                return int.Parse(name);
            }
        }
        public static string StateToName(int state)
        {
            switch (state) {
                case (int)CharacterPropertyEnum.x3001_眩晕:
                    return "stun";
                case (int)CharacterPropertyEnum.x3002_昏睡:
                    return "sleep";
                case (int)CharacterPropertyEnum.x3003_倒地:
                    return "knockdown";
                case (int)CharacterPropertyEnum.x3004_减速:
                    return "slow";
                case (int)CharacterPropertyEnum.x3005_浮空:
                    return "float";
                case (int)CharacterPropertyEnum.x3006_灼烧:
                    return "fire";
                case (int)CharacterPropertyEnum.x3007_感电:
                    return "shock";
                case (int)CharacterPropertyEnum.x3008_霸体:
                    return "armor";
                case (int)CharacterPropertyEnum.x3009_无敌:
                    return "invincible";
                case (int)CharacterPropertyEnum.x3010_复活:
                    return "revive";
                default:
                    return state.ToString();
            }
        }
    }
}

