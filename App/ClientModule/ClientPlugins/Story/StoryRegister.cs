using UnityEngine;
using System.Collections.Generic;
using System.Text;
using ScriptableFramework;
using ClientPlugins;

public static class StoryRegister
{
    public static void Register()
    {
        Plugin.Proxy.RegisterSimpleStoryApi("ai_do_normal", "ai_do_normal command", "AiDoNormal");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_do_member", "ai_do_member command", "AiDoMember");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_cast_skill", "ai_cast_skill command", "AiCastSkill");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_chase", "ai_chase command", "AiChase");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_keep_away", "ai_keep_away command", "AiKeepAway");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_go_home", "ai_go_home command", "AiGohome");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_rand_move", "ai_rand_move command", "AiRandMove");

        Plugin.Proxy.RegisterSimpleStoryApi("ai_select_skill_by_distance", "ai_select_skill_by_distance function", "AiSelectSkillByDistance");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_need_chase", "ai_need_chase function", "AiNeedChase");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_need_keep_away", "ai_need_keep_away function", "AiNeedKeepAway");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_select_skill", "ai_select_skill function", "AiSelectSkill");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_select_target", "ai_select_target function", "AiSelectTarget");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_get_skill", "ai_get_skill function", "AiGetSkill");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_get_target", "ai_get_target function", "AiGetTarget");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_get_skills", "ai_get_skills function", "AiGetSkills");
        Plugin.Proxy.RegisterSimpleStoryApi("ai_get_entities", "ai_get_entities function", "AiGetEntities");

        Plugin.Proxy.RegisterStoryApi("select", "select function", "AiQuery");
    }
}
