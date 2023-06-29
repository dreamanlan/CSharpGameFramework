using UnityEngine;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;
using ClientPlugins;

public static class StoryRegister
{
    public static void Register()
    {
        Plugin.Proxy.RegisterSimpleStoryCommand("ai_do_normal", "AiDoNormal");
        Plugin.Proxy.RegisterSimpleStoryCommand("ai_do_member", "AiDoMember");
        Plugin.Proxy.RegisterSimpleStoryCommand("ai_cast_skill", "AiCastSkill");
        Plugin.Proxy.RegisterSimpleStoryCommand("ai_chase", "AiChase");
        Plugin.Proxy.RegisterSimpleStoryCommand("ai_keep_away", "AiKeepAway");
        Plugin.Proxy.RegisterSimpleStoryCommand("ai_go_home", "AiGohome");
        Plugin.Proxy.RegisterSimpleStoryCommand("ai_rand_move", "AiRandMove");

        Plugin.Proxy.RegisterSimpleStoryValue("ai_select_skill_by_distance", "AiSelectSkillByDistance");
        Plugin.Proxy.RegisterSimpleStoryValue("ai_need_chase", "AiNeedChase");
        Plugin.Proxy.RegisterSimpleStoryValue("ai_need_keep_away", "AiNeedKeepAway");
        Plugin.Proxy.RegisterSimpleStoryValue("ai_select_skill", "AiSelectSkill");
        Plugin.Proxy.RegisterSimpleStoryValue("ai_select_target", "AiSelectTarget");
        Plugin.Proxy.RegisterSimpleStoryValue("ai_get_skill", "AiGetSkill");
        Plugin.Proxy.RegisterSimpleStoryValue("ai_get_target", "AiGetTarget");
        Plugin.Proxy.RegisterSimpleStoryValue("ai_get_skills", "AiGetSkills");
        Plugin.Proxy.RegisterSimpleStoryValue("ai_get_entities", "AiGetEntities");
        Plugin.Proxy.RegisterStoryValue("select", "AiQuery");
    }
}
