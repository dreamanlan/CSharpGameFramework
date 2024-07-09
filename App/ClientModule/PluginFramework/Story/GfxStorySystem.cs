using System;
using System.Collections.Generic;
using DotnetStoryScript;
using ScriptableFramework;

namespace ScriptableFramework.Story
{
    public sealed class GfxStorySystem
    {
        public int SceneId
        {
            get { return m_SceneId; }
            set { m_SceneId = value; }
        }
        public void Init()
        {
            //register story commands
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "preload", "preload command", new StoryCommandFactoryHelper<Story.Commands.PreloadCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "startstory", "startstory command", new StoryCommandFactoryHelper<Story.Commands.StartStoryCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "stopstory", "stopstory command", new StoryCommandFactoryHelper<Story.Commands.StopStoryCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "waitstory", "waitstory command", new StoryCommandFactoryHelper<Story.Commands.WaitStoryCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "pausestory", "pausestory command", new StoryCommandFactoryHelper<Story.Commands.PauseStoryCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "resumestory", "resumestory command", new StoryCommandFactoryHelper<Story.Commands.ResumeStoryCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "firemessage", "firemessage command", new Story.Commands.FireMessageCommandFactory());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "fireconcurrentmessage", "fireconcurrentmessage command", new Story.Commands.FireConcurrentMessageCommandFactory());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "waitallmessage", "waitallmessage command", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "waitallmessagehandler", "waitallmessagehandler command", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "suspendallmessagehandler", "suspendallmessagehandler command", new StoryCommandFactoryHelper<Story.Commands.SuspendAllMessageHandlerCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "resumeallmessagehandler", "resumeallmessagehandler command", new StoryCommandFactoryHelper<Story.Commands.ResumeAllMessageHandlerCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendroomstorymessage", "sendroomstorymessage command", new StoryCommandFactoryHelper<Story.Commands.SendRoomStoryMessageCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendserverstorymessage", "sendserverstorymessage command", new StoryCommandFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendclientstorymessage", "sendclientstorymessage command", new StoryCommandFactoryHelper<DotnetStoryScript.CommonCommands.DummyCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "publishgfxevent", "publishgfxevent command", new StoryCommandFactoryHelper<Story.Commands.PublishGfxEventCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendgfxmessage", "sendgfxmessage command", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendgfxmessagewithtag", "sendgfxmessagewithtag command", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendgfxmessagewithgameobject", "sendgfxmessagewithgameobject command", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageWithGameObjectCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendskillmessage", "sendskillmessage command", new StoryCommandFactoryHelper<Story.Commands.SendSkillMessageCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "creategameobject", "creategameobject command", new StoryCommandFactoryHelper<Story.Commands.CreateGameObjectCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "settransform", "settransform command", new StoryCommandFactoryHelper<Story.Commands.SetTransformCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "addtransform", "addtransform command", new StoryCommandFactoryHelper<Story.Commands.AddTransformCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroygameobject", "destroygameobject command", new StoryCommandFactoryHelper<Story.Commands.DestroyGameObjectCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setparent", "setparent command", new StoryCommandFactoryHelper<Story.Commands.SetParentCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setactive", "setactive command", new StoryCommandFactoryHelper<Story.Commands.SetActiveCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setvisible", "setvisible command", new StoryCommandFactoryHelper<Story.Commands.SetVisibleCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "putonground", "putonground command", new StoryCommandFactoryHelper<Story.Commands.PutOnGroundCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setnavmeshagentenable", "setnavmeshagentenable command", new StoryCommandFactoryHelper<Story.Commands.SetNavmeshAgentEnableCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "addcomponent", "addcomponent command", new StoryCommandFactoryHelper<Story.Commands.AddComponentCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "removecomponent", "removecomponent command", new StoryCommandFactoryHelper<Story.Commands.RemoveComponentCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "installplugin", "installplugin command", new StoryCommandFactoryHelper<Story.Commands.InstallPluginCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "removeplugin", "removeplugin command", new StoryCommandFactoryHelper<Story.Commands.RemovePluginCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "openurl", "openurl command", new StoryCommandFactoryHelper<Story.Commands.OpenUrlCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "quit", "quit command", new StoryCommandFactoryHelper<Story.Commands.QuitCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "loadui", "loadui command", new StoryCommandFactoryHelper<Story.Commands.LoadUiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "bindui", "bindui command", new StoryCommandFactoryHelper<Story.Commands.BindUiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setactorscale", "setactorscale command", new StoryCommandFactoryHelper<Story.Commands.SetActorScaleCommand>());

            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "changescene", "changescene command", new StoryCommandFactoryHelper<Story.Commands.ChangeSceneCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "openbattle", "openbattle command", new StoryCommandFactoryHelper<Story.Commands.OpenBattleCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "closebattle", "closebattle command", new StoryCommandFactoryHelper<Story.Commands.CloseBattleCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "createscenelogic", "createscenelogic command", new StoryCommandFactoryHelper<Story.Commands.CreateSceneLogicCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroyscenelogic", "destroyscenelogic command", new StoryCommandFactoryHelper<Story.Commands.DestroySceneLogicCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "pausescenelogic", "pausescenelogic command", new StoryCommandFactoryHelper<Story.Commands.PauseSceneLogicCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "restarttimeout", "restarttimeout command", new StoryCommandFactoryHelper<Story.Commands.RestartTimeoutCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "highlightpromptwithdict", "highlightpromptwithdict command", new StoryCommandFactoryHelper<Story.Commands.HighlightPromptWithDictCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "highlightprompt", "highlightprompt command", new StoryCommandFactoryHelper<Story.Commands.HighlightPromptCommand>());

            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "blackboardset", "blackboardset command", new StoryCommandFactoryHelper<Story.Commands.BlackboardSetCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "blackboardclear", "blackboardclear command", new StoryCommandFactoryHelper<Story.Commands.BlackboardClearCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerafollow", "camerafollow command", new StoryCommandFactoryHelper<Story.Commands.CameraFollowCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralook", "cameralook command", new StoryCommandFactoryHelper<Story.Commands.CameraLookCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralookimmediately", "cameralookimmediately command", new StoryCommandFactoryHelper<Story.Commands.CameraLookImmediatelyCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralooktoward", "cameralooktoward command", new StoryCommandFactoryHelper<Story.Commands.CameraLookTowardCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerafixedyaw", "camerafixedyaw command", new StoryCommandFactoryHelper<Story.Commands.CameraFixedYawCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerayaw", "camerayaw command", new StoryCommandFactoryHelper<Story.Commands.CameraYawCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameraheight", "cameraheight command", new StoryCommandFactoryHelper<Story.Commands.CameraHeightCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameradistance", "cameradistance command", new StoryCommandFactoryHelper<Story.Commands.CameraDistanceCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerasetdistanceheight", "camerasetdistanceheight command", new StoryCommandFactoryHelper<Story.Commands.CameraSetDistanceHeightCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameraresetdistanceheight", "cameraresetdistanceheight command", new StoryCommandFactoryHelper<Story.Commands.CameraResetDistanceHeightCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerasetfollowspeed", "camerasetfollowspeed command", new StoryCommandFactoryHelper<Story.Commands.CameraSetFollowSpeedCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameraresetfollowspeed", "cameraresetfollowspeed command", new StoryCommandFactoryHelper<Story.Commands.CameraResetFollowSpeedCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerafollowobj", "camerafollowobj command", new StoryCommandFactoryHelper<Story.Commands.CameraFollowObjCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralookobj", "cameralookobj command", new StoryCommandFactoryHelper<Story.Commands.CameraLookObjCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralookobjimmediately", "cameralookobjimmediately command", new StoryCommandFactoryHelper<Story.Commands.CameraLookObjImmediatelyCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralooktowardobj", "cameralooktowardobj command", new StoryCommandFactoryHelper<Story.Commands.CameraLookTowardObjCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralookcopy", "cameralookcopy command", new StoryCommandFactoryHelper<Story.Commands.CameraLookCopyCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralookobjcopy", "cameralookobjcopy command", new StoryCommandFactoryHelper<Story.Commands.CameraLookObjCopyCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameraenable", "cameraenable command", new StoryCommandFactoryHelper<Story.Commands.CameraEnableCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "lockframe", "lockframe command", new StoryCommandFactoryHelper<Story.Commands.LockFrameCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "showdlg", "showdlg command", new StoryCommandFactoryHelper<Story.Commands.ShowDlgCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "areadetect", "areadetect command", new StoryCommandFactoryHelper<Story.Commands.AreaDetectCommand>());

            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "gameobjectanimation", "gameobjectanimation command", new StoryCommandFactoryHelper<Story.Commands.GameObjectAnimationCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "gameobjectanimationparam", "gameobjectanimationparam command", new StoryCommandFactoryHelper<Story.Commands.GameObjectAnimationParamCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "gameobjectcastskill", "gameobjectcastskill command", new StoryCommandFactoryHelper<Story.Commands.GameObjectCastSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "gameobjectstopskill", "gameobjectstopskill command", new StoryCommandFactoryHelper<Story.Commands.GameObjectStopSkillCommand>());

            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "createnpc", "createnpc command", new StoryCommandFactoryHelper<Story.Commands.CreateNpcCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroynpc", "destroynpc command", new StoryCommandFactoryHelper<Story.Commands.DestroyNpcCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroynpcwithobjid", "destroynpcwithobjid command", new StoryCommandFactoryHelper<Story.Commands.DestroyNpcWithObjIdCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcface", "npcface command", new StoryCommandFactoryHelper<Story.Commands.NpcFaceCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcmove", "npcmove command", new StoryCommandFactoryHelper<Story.Commands.NpcMoveCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcmovewithwaypoints", "npcmovewithwaypoints command", new StoryCommandFactoryHelper<Story.Commands.NpcMoveWithWaypointsCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcstop", "npcstop command", new StoryCommandFactoryHelper<Story.Commands.NpcStopCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcattack", "npcattack command", new StoryCommandFactoryHelper<Story.Commands.NpcAttackCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetformation", "npcsetformation command", new StoryCommandFactoryHelper<Story.Commands.NpcSetFormationCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcenableai", "npcenableai command", new StoryCommandFactoryHelper<Story.Commands.NpcEnableAiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetai", "npcsetai command", new StoryCommandFactoryHelper<Story.Commands.NpcSetAiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetaitarget", "npcsetaitarget command", new StoryCommandFactoryHelper<Story.Commands.NpcSetAiTargetCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcanimation", "npcanimation command", new StoryCommandFactoryHelper<Story.Commands.NpcAnimationCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcanimationparam", "npcanimationparam command", new StoryCommandFactoryHelper<Story.Commands.NpcAnimationParamCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcaddimpact", "npcaddimpact command", new StoryCommandFactoryHelper<Story.Commands.NpcAddImpactCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcremoveimpact", "npcremoveimpact command", new StoryCommandFactoryHelper<Story.Commands.NpcRemoveImpactCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npccastskill", "npccastskill command", new StoryCommandFactoryHelper<Story.Commands.NpcCastSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcstopskill", "npcstopskill command", new StoryCommandFactoryHelper<Story.Commands.NpcStopSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npclisten", "npclisten command", new StoryCommandFactoryHelper<Story.Commands.NpcListenCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetcamp", "npcsetcamp command", new StoryCommandFactoryHelper<Story.Commands.NpcSetCampCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetsummonerid", "npcsetsummonerid command", new StoryCommandFactoryHelper<Story.Commands.NpcSetSummonerIdCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetsummonskillid", "npcsetsummonskillid command", new StoryCommandFactoryHelper<Story.Commands.NpcSetSummonSkillIdCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objface", "objface command", new StoryCommandFactoryHelper<Story.Commands.ObjFaceCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objmove", "objmove command", new StoryCommandFactoryHelper<Story.Commands.ObjMoveCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objmovewithwaypoints", "objmovewithwaypoints command", new StoryCommandFactoryHelper<Story.Commands.ObjMoveWithWaypointsCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objstop", "objstop command", new StoryCommandFactoryHelper<Story.Commands.ObjStopCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objattack", "objattack command", new StoryCommandFactoryHelper<Story.Commands.ObjAttackCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetformation", "objsetformation command", new StoryCommandFactoryHelper<Story.Commands.ObjSetFormationCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objenableai", "objenableai command", new StoryCommandFactoryHelper<Story.Commands.ObjEnableAiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetai", "objsetai command", new StoryCommandFactoryHelper<Story.Commands.ObjSetAiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetaitarget", "objsetaitarget command", new StoryCommandFactoryHelper<Story.Commands.ObjSetAiTargetCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objanimation", "objanimation command", new StoryCommandFactoryHelper<Story.Commands.ObjAnimationCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objanimationparam", "objanimationparam command", new StoryCommandFactoryHelper<Story.Commands.ObjAnimationParamCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objaddimpact", "objaddimpact command", new StoryCommandFactoryHelper<Story.Commands.ObjAddImpactCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objremoveimpact", "objremoveimpact command", new StoryCommandFactoryHelper<Story.Commands.ObjRemoveImpactCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objcastskill", "objcastskill command", new StoryCommandFactoryHelper<Story.Commands.ObjCastSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objstopskill", "objstopskill command", new StoryCommandFactoryHelper<Story.Commands.ObjStopSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objlisten", "objlisten command", new StoryCommandFactoryHelper<Story.Commands.ObjListenCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetcamp", "objsetcamp command", new StoryCommandFactoryHelper<Story.Commands.ObjSetCampCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetsummonerid", "objsetsummonerid command", new StoryCommandFactoryHelper<Story.Commands.ObjSetSummonerIdCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetsummonskillid", "objsetsummonskillid command", new StoryCommandFactoryHelper<Story.Commands.ObjSetSummonSkillIdCommand>());

            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sethp", "sethp command", new StoryCommandFactoryHelper<Story.Commands.SetHpCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setenergy", "setenergy command", new StoryCommandFactoryHelper<Story.Commands.SetEnergyCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objset", "objset command", new StoryCommandFactoryHelper<Story.Commands.ObjSetCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setlevel", "setlevel command", new StoryCommandFactoryHelper<Story.Commands.SetLevelCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setattr", "setattr command", new StoryCommandFactoryHelper<Story.Commands.SetAttrCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setunitid", "setunitid command", new StoryCommandFactoryHelper<Story.Commands.SetUnitIdCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setleaderid", "setleaderid command", new StoryCommandFactoryHelper<Story.Commands.SetLeaderIdCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "markcontrolbystory", "markcontrolbystory command", new StoryCommandFactoryHelper<Story.Commands.MarkControlByStoryCommand>());

            //register value or functions
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "gettime", "gettime function", new StoryFunctionFactoryHelper<Story.Functions.GetTimeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "gettimescale", "gettimescale function", new StoryFunctionFactoryHelper<Story.Functions.GetTimeScaleFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isactive", "isactive function", new StoryFunctionFactoryHelper<Story.Functions.IsActiveFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isreallyactive", "isreallyactive function", new StoryFunctionFactoryHelper<Story.Functions.IsReallyActiveFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isvisible", "isvisible function", new StoryFunctionFactoryHelper<Story.Functions.IsVisibleFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isnavmeshagentenabled", "isnavmeshagentenabled function", new StoryFunctionFactoryHelper<Story.Functions.IsNavmeshAgentEnabledFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getcomponent", "getcomponent function", new StoryFunctionFactoryHelper<Story.Functions.GetComponentFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getcomponentinparent", "getcomponentinparent function", new StoryFunctionFactoryHelper<Story.Functions.GetComponentInParentFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getcomponentinchildren", "getcomponentinchildren function", new StoryFunctionFactoryHelper<Story.Functions.GetComponentInChildrenFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getcomponents", "getcomponents function", new StoryFunctionFactoryHelper<Story.Functions.GetComponentsFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getcomponentsinparent", "getcomponentsinparent function", new StoryFunctionFactoryHelper<Story.Functions.GetComponentsInParentFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getcomponentsinchildren", "getcomponentsinchildren function", new StoryFunctionFactoryHelper<Story.Functions.GetComponentsInChildrenFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getgameobject", "getgameobject function", new StoryFunctionFactoryHelper<Story.Functions.GetGameObjectFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getparent", "getparent function", new StoryFunctionFactoryHelper<Story.Functions.GetParentFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "findchild", "findchild function", new StoryFunctionFactoryHelper<Story.Functions.FindChildFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getchildcount", "getchildcount function", new StoryFunctionFactoryHelper<Story.Functions.GetChildCountFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getchild", "getchild function", new StoryFunctionFactoryHelper<Story.Functions.GetChildFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getunitytype", "getunitytype function", new StoryFunctionFactoryHelper<Story.Functions.GetUnityTypeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getunityuitype", "getunityuitype function", new StoryFunctionFactoryHelper<Story.Functions.GetUnityUiTypeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getusertype", "getusertype function", new StoryFunctionFactoryHelper<Story.Functions.GetUserTypeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getactor", "getactor function", new StoryFunctionFactoryHelper<Story.Functions.GetActorFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getentityinfo", "getentityinfo function", new StoryFunctionFactoryHelper<Story.Functions.GetEntityInfoFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getglobal", "getglobal function", new StoryFunctionFactoryHelper<Story.Functions.GetGlobalFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "pluginFramework", "pluginFramework function", new StoryFunctionFactoryHelper<Story.Functions.PluginFrameworkFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getentityview", "getentityview function", new StoryFunctionFactoryHelper<Story.Functions.GetEntityViewFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "dictget", "dictget function", new StoryFunctionFactoryHelper<Story.Functions.DictGetFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "dictformat", "dictformat function", new StoryFunctionFactoryHelper<Story.Functions.DictFormatFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isclient", "isclient function", new StoryFunctionFactoryHelper<Story.Functions.IsClientFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getsceneid", "getsceneid function", new StoryFunctionFactoryHelper<Story.Functions.GetSceneIdFunction>());

            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "blackboardget", "blackboardget function", new StoryFunctionFactoryHelper<Story.Functions.BlackboardGetFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getdialogitem", "getdialogitem function", new StoryFunctionFactoryHelper<Story.Functions.GetDialogItemFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "offsetspline", "offsetspline function", new StoryFunctionFactoryHelper<Story.Functions.OffsetSplineFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "offsetvector3", "offsetvector3 function", new StoryFunctionFactoryHelper<Story.Functions.OffsetVector3Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getactoricon", "getactoricon function", new StoryFunctionFactoryHelper<Story.Functions.GetActorIconFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getmonsterinfo", "getmonsterinfo function", new StoryFunctionFactoryHelper<Story.Functions.GetMonsterInfoFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getaidata", "getaidata function", new StoryFunctionFactoryHelper<Story.Functions.GetAiDataFunction>());
            
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcidlist", "npcidlist function", new StoryFunctionFactoryHelper<Story.Functions.NpcIdListFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "combatnpccount", "combatnpccount function", new StoryFunctionFactoryHelper<Story.Functions.CombatNpcCountFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npccount", "npccount function", new StoryFunctionFactoryHelper<Story.Functions.NpcCountFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "unitid2objid", "unitid2objid function", new StoryFunctionFactoryHelper<Story.Functions.UnitId2ObjIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objid2unitid", "objid2unitid function", new StoryFunctionFactoryHelper<Story.Functions.ObjId2UnitIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "unitid2uniqueid", "unitid2uniqueid function", new StoryFunctionFactoryHelper<Story.Functions.UnitId2UniqueIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objid2uniqueid", "objid2uniqueid function", new StoryFunctionFactoryHelper<Story.Functions.ObjId2UniqueIdFunction>());
            
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcgetformation", "npcgetformation function", new StoryFunctionFactoryHelper<Story.Functions.NpcGetFormationFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcgetnpctype", "npcgetnpctype function", new StoryFunctionFactoryHelper<Story.Functions.NpcGetNpcTypeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcgetsummonerid", "npcgetsummonerid function", new StoryFunctionFactoryHelper<Story.Functions.NpcGetSummonerIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcgetsummonskillid", "npcgetsummonskillid function", new StoryFunctionFactoryHelper<Story.Functions.NpcGetSummonSkillIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcfindimpactseqbyid", "npcfindimpactseqbyid function", new StoryFunctionFactoryHelper<Story.Functions.NpcFindImpactSeqByIdFunction>());

            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objgetformation", "objgetformation function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetFormationFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objgetnpctype", "objgetnpctype function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetNpcTypeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objgetsummonerid", "objgetsummonerid function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetSummonerIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objgetsummonskillid", "objgetsummonskillid function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetSummonSkillIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objfindimpactseqbyid", "objfindimpactseqbyid function", new StoryFunctionFactoryHelper<Story.Functions.ObjFindImpactSeqByIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isenemy", "isenemy function", new StoryFunctionFactoryHelper<Story.Functions.IsEnemyFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isfriend", "isfriend function", new StoryFunctionFactoryHelper<Story.Functions.IsFriendFunction>());

            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getposition", "getposition function", new StoryFunctionFactoryHelper<Story.Functions.GetPositionFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getpositionx", "getpositionx function", new StoryFunctionFactoryHelper<Story.Functions.GetPositionXFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getpositiony", "getpositiony function", new StoryFunctionFactoryHelper<Story.Functions.GetPositionYFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getpositionz", "getpositionz function", new StoryFunctionFactoryHelper<Story.Functions.GetPositionZFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getrotation", "getrotation function", new StoryFunctionFactoryHelper<Story.Functions.GetRotationFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getrotationx", "getrotationx function", new StoryFunctionFactoryHelper<Story.Functions.GetRotationXFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getrotationy", "getrotationy function", new StoryFunctionFactoryHelper<Story.Functions.GetRotationYFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getrotationz", "getrotationz function", new StoryFunctionFactoryHelper<Story.Functions.GetRotationZFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getscale", "getscale function", new StoryFunctionFactoryHelper<Story.Functions.GetScaleFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getscalex", "getscalex function", new StoryFunctionFactoryHelper<Story.Functions.GetScaleXFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getscaley", "getscaley function", new StoryFunctionFactoryHelper<Story.Functions.GetScaleYFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getscalez", "getscalez function", new StoryFunctionFactoryHelper<Story.Functions.GetScaleZFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "position", "position function", new StoryFunctionFactoryHelper<DotnetStoryScript.CommonFunctions.Vector3Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "rotation", "rotation function", new StoryFunctionFactoryHelper<DotnetStoryScript.CommonFunctions.Vector3Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "scale", "scale function", new StoryFunctionFactoryHelper<DotnetStoryScript.CommonFunctions.Vector3Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getcamp", "getcamp function", new StoryFunctionFactoryHelper<Story.Functions.GetCampFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "iscombatnpc", "iscombatnpc function", new StoryFunctionFactoryHelper<Story.Functions.IsCombatNpcFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "gethp", "gethp function", new StoryFunctionFactoryHelper<Story.Functions.GetHpFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getenergy", "getenergy function", new StoryFunctionFactoryHelper<Story.Functions.GetEnergyFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getmaxhp", "getmaxhp function", new StoryFunctionFactoryHelper<Story.Functions.GetMaxHpFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getmaxenergy", "getmaxenergy function", new StoryFunctionFactoryHelper<Story.Functions.GetMaxEnergyFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "calcoffset", "calcoffset function", new StoryFunctionFactoryHelper<Story.Functions.CalcOffsetFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "calcdir", "calcdir function", new StoryFunctionFactoryHelper<Story.Functions.CalcDirFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objget", "objget function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "gettableid", "gettableid function", new StoryFunctionFactoryHelper<Story.Functions.GetTableIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getlevel", "getlevel function", new StoryFunctionFactoryHelper<Story.Functions.GetLevelFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getattr", "getattr function", new StoryFunctionFactoryHelper<Story.Functions.GetAttrFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "iscontrolbystory", "iscontrolbystory function", new StoryFunctionFactoryHelper<Story.Functions.IsControlByStoryFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "cancastskill", "cancastskill function", new StoryFunctionFactoryHelper<Story.Functions.CanCastSkillFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isundercontrol", "isundercontrol function", new StoryFunctionFactoryHelper<Story.Functions.IsUnderControlFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getplayerid", "getplayerid function", new StoryFunctionFactoryHelper<Story.Functions.GetPlayerIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getplayerunitid", "getplayerunitid function", new StoryFunctionFactoryHelper<Story.Functions.GetPlayerUnitIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getleaderid", "getleaderid function", new StoryFunctionFactoryHelper<Story.Functions.GetLeaderIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getleadertableid", "getleadertableid function", new StoryFunctionFactoryHelper<Story.Functions.GetLeaderTableIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getmembercount", "getmembercount function", new StoryFunctionFactoryHelper<Story.Functions.GetMemberCountFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getmembertableid", "getmembertableid function", new StoryFunctionFactoryHelper<Story.Functions.GetMemberTableIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getmemberlevel", "getmemberlevel function", new StoryFunctionFactoryHelper<Story.Functions.GetMemberLevelFunction>());

            LoadCustomCommandsAndFunctions();
        }
        public void Reset()
        {
            Reset(true);
        }
        public void Reset(bool logIfTriggered)
        {
            LoadCustomCommandsAndFunctions();

            m_GlobalVariables.Clear();
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info) {
                    info.Reset(logIfTriggered);
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
            m_StoryLogicInfos.Clear();
        }
        public void LoadSceneStories()
        {
            TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(m_SceneId);
            if (null != cfg) {
                string[] filePath;
                int ct1 = cfg.SceneDslFile.Count;
                int ct2 = cfg.ClientDslFile.Count;
                filePath = new string[ct1 + ct2];
                for (int i = 0; i < ct1; i++) {
                    filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + cfg.SceneDslFile[i]);
                }
                for (int i = 0; i < ct2; i++) {
                    filePath[ct1 + i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + cfg.ClientDslFile[i]);
                }
                StoryConfigManager.Instance.LoadStories(0, string.Empty, filePath);
                Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(0);
                if (null != stories) {
                    foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                        AddStoryInstance(pair.Key, pair.Value.Clone());
                    }
                }
            }
        }
        public void LoadBattleStories(int sceneId)
        {
            TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(sceneId);
            if (null != cfg) {
                string[] filePath;
                int ct1 = cfg.SceneDslFile.Count;
                int ct2 = cfg.ClientDslFile.Count;
                filePath = new string[ct1 + ct2];
                for (int i = 0; i < ct1; i++) {
                    filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + cfg.SceneDslFile[i]);
                }
                for (int i = 0; i < ct2; i++) {
                    filePath[ct1 + i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + cfg.ClientDslFile[i]);
                }
                StoryConfigManager.Instance.LoadStories(0, string.Empty, filePath);
                Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(0);
                if (null != stories) {
                    foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                        AddStoryInstance(pair.Key, pair.Value.Clone());
                    }
                }
            }
        }
        public void LoadStory(string _namespace, string file)
        {
            string filePath = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + file);
            StoryConfigManager.Instance.LoadStories(0, _namespace, filePath);
            Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath);
            if (null != stories) {
                foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                    AddStoryInstance(pair.Key, pair.Value.Clone());
                }
            }
        }
        public void LoadAiStory(string _namespace, string file)
        {
            string filePath = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + file);
            StoryConfigManager.Instance.LoadStories(0, _namespace, filePath);
            Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath);
            if (null != stories) {
                foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                    AiStoryInstanceInfo info = new AiStoryInstanceInfo();
                    info.m_StoryInstance = pair.Value.Clone();
                    info.m_IsUsed = false;
                    AddAiStoryInstanceInfoToPool(pair.Key, info);
                }
            }
        }
        public void ClearStoryInstancePool()
        {
            m_StoryInstancePool.Clear();
            m_AiStoryInstancePool.Clear();
        }

        public AiStoryInstanceInfo NewAiStoryInstance(string storyId, string _namespace, params string[] aiFiles)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            AiStoryInstanceInfo instInfo = GetUnusedAiStoryInstanceInfoFromPool(storyId);
            if (null == instInfo) {
                int ct;
                string[] filePath;
                ct = aiFiles.Length;
                filePath = new string[ct];
                for (int i = 0; i < ct; i++) {
                    filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + aiFiles[i]);
                }
                StoryConfigManager.Instance.LoadStories(0, _namespace, filePath);
                StoryInstance instance = StoryConfigManager.Instance.NewStoryInstance(storyId, 0);
                if (instance == null) {
                    LogSystem.Error("Can't load story config, story:{0} scene:{1} !", storyId, m_SceneId);
                    return null;
                }
                for (int ix = 0; ix < filePath.Length; ++ix) {
                    Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath[ix]);
                    if (null != stories) {
                        foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                            if (pair.Key != storyId) {
                                AiStoryInstanceInfo info = new AiStoryInstanceInfo();
                                info.m_StoryInstance = pair.Value.Clone();
                                info.m_IsUsed = false;
                                AddAiStoryInstanceInfoToPool(pair.Key, info);
                            }
                        }
                    }
                }
                AiStoryInstanceInfo res = new AiStoryInstanceInfo();
                res.m_StoryInstance = instance;
                res.m_IsUsed = true;

                AddAiStoryInstanceInfoToPool(storyId, res);
                return res;
            } else {
                instInfo.m_IsUsed = true;
                return instInfo;
            }
        }
        public void RecycleAiStoryInstance(AiStoryInstanceInfo info)
        {
            info.m_StoryInstance.Reset();
            info.m_IsUsed = false;
        }

        public int ActiveStoryCount
        {
            get
            {
                return m_StoryLogicInfos.Count;
            }
        }
        public IList<StoryInstance> ActiveStories
        {
            get { return m_StoryLogicInfos; }
        }
        public StrBoxedValueDict GlobalVariables
        {
            get { return m_GlobalVariables; }
        }
        public StoryInstance GetStory(string storyId)
        {
            return GetStory(storyId, string.Empty);
        }
        public StoryInstance GetStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            return GetStoryInstance(storyId);
        }
        public void StartStories(string storyId)
        {
            StartStories(storyId, string.Empty);
        }
        public void StartStories(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            foreach(var pair in m_StoryInstancePool) {
                var info = pair.Value;
                if (IsMatch(info.StoryId, storyId)) {
                    StopStory(info.StoryId);
                    m_StoryLogicInfos.Add(info);
                    info.Context = null;
                    info.GlobalVariables = m_GlobalVariables;
                    info.Start();

                    LogSystem.Info("StartStory {0} scene {1}", info.StoryId, m_SceneId);
                }
            }
        }
        public void PauseStories(string storyId, bool pause)
        {
            PauseStories(storyId, string.Empty, pause);
        }
        public void PauseStories(string storyId, string _namespace, bool pause)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    info.IsPaused = pause;
                }
            }
        }
        public void StopStories(string storyId)
        {
            StopStories(storyId, string.Empty);
        }
        public void StopStories(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
        }
        public int CountStories(string storyId)
        {
            return CountStories(storyId, string.Empty);
        }
        public int CountStories(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int ct = 0;
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info && IsMatch(info.StoryId, storyId) && !info.IsInTick) {
                    ++ct;
                }
            }
            return ct;
        }
        public void MarkStoriesTerminated(string storyId)
        {
            MarkStoriesTerminated(storyId, string.Empty);
        }
        public void MarkStoriesTerminated(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    info.IsTerminated = true;
                }
            }
        }
        public void SkipCurMessageHandlers(string endMsg, string storyId)
        {
            SkipCurMessageHandlers(endMsg, storyId, string.Empty);
        }
        public void SkipCurMessageHandlers(string endMsg, string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    info.ClearMessage();
                    var enumer = info.GetMessageHandlerEnumerator();
                    while (enumer.MoveNext()) {
                        var handler = enumer.Current;
                        if (handler.IsTriggered && handler.MessageId != endMsg) {
                            handler.CanSkip = true;
                        }
                    }
                    var cenumer = info.GetConcurrentMessageHandlerEnumerator();
                    while (cenumer.MoveNext()) {
                        var handler = cenumer.Current;
                        if (handler.IsTriggered && handler.MessageId != endMsg) {
                            handler.CanSkip = true;
                        }
                    }
                }
            }
        }
        public void StartStory(string storyId)
        {
            StartStory(storyId, string.Empty);
        }
        public void StartStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            StoryInstance inst = GetStoryInstance(storyId);
            if (null != inst) {
                StopStory(storyId);
                m_StoryLogicInfos.Add(inst);
                inst.Context = null;
                inst.GlobalVariables = m_GlobalVariables;
                inst.Start();

                LogSystem.Info("StartStory {0}", storyId);
            } else {
                LogSystem.Error("Can't load story, story:{0} scene:{1} !", storyId, m_SceneId);
            }
        }
        public void PauseStory(string storyId, bool pause)
        {
            PauseStory(storyId, string.Empty, pause);
        }
        public void PauseStory(string storyId, string _namespace, bool pause)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    info.IsPaused = pause;
                }
            }
        }
        public void StopStory(string storyId)
        {
            StopStory(storyId, string.Empty);
        }
        public void StopStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
        }
        public int CountStory(string storyId)
        {
            return CountStory(storyId, string.Empty);
        }
        public int CountStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int ct = 0;
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info && info.StoryId == storyId && !info.IsInTick) {
                    ++ct;
                }
            }
            return ct;
        }
        public void MarkStoryTerminated(string storyId)
        {
            MarkStoryTerminated(storyId, string.Empty);
        }
        public void MarkStoryTerminated(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    info.IsTerminated = true;
                }
            }
        }
        public void Tick()
        {
            long time = TimeUtility.GetLocalMilliseconds();
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.Tick(time);
                if (info.IsTerminated) {
                    m_StoryLogicInfos.RemoveAt(ix);
                }
            }
        }
        public void AddBindedStory(UnityEngine.Object obj, StoryInstance inst)
        {
            m_BindedStoryInstances.Add(new BindedStoryInfo { Object = obj, Instance = inst });
        }
        public BoxedValueList NewBoxedValueList()
        {
            var args = m_BoxedValueListPool.Alloc();
            args.Clear();
            return args;
        }
        public void SendMessage(string msgId)
        {
            var args = NewBoxedValueList();
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValue arg1)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            args.Add(arg3);
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValueList args)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                var newArgs = info.NewBoxedValueList();
                newArgs.AddRange(args);
                info.SendMessage(msgId, newArgs);
            }
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        var newArgs = infos[ix].m_StoryInstance.NewBoxedValueList();
                        newArgs.AddRange(args);
                        infos[ix].m_StoryInstance.SendMessage(msgId, newArgs);
                    }
                }
            }
            m_BoxedValueListPool.Recycle(args);
        }
        public void SendConcurrentMessage(string msgId)
        {
            var args = NewBoxedValueList();
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValue arg1)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            args.Add(arg3);
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValueList args)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                var newArgs = info.NewBoxedValueList();
                newArgs.AddRange(args);
                info.SendConcurrentMessage(msgId, newArgs);
            }
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        var newArgs = infos[ix].m_StoryInstance.NewBoxedValueList();
                        newArgs.AddRange(args);
                        infos[ix].m_StoryInstance.SendConcurrentMessage(msgId, newArgs);
                    }
                }
            }
            m_BoxedValueListPool.Recycle(args);
        }
        public int CountMessage(string msgId)
        {
            int sum = 0;
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                sum += info.CountMessage(msgId);
            }
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        sum += infos[ix].m_StoryInstance.CountMessage(msgId);
                    }
                }
            }
            return sum;
        }
        public void SuspendMessageHandler(string msgId, bool suspend)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.SuspendMessageHandler(msgId, suspend);
            }
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        infos[ix].m_StoryInstance.SuspendMessageHandler(msgId, suspend);
                    }
                }
            }
        }

        private void AddStoryInstance(string storyId, StoryInstance info)
        {
            if (!m_StoryInstancePool.ContainsKey(storyId)) {
                m_StoryInstancePool.Add(storyId, info);
            } else {
                m_StoryInstancePool[storyId] = info;
            }
        }
        private StoryInstance GetStoryInstance(string storyId)
        {
            StoryInstance info;
            m_StoryInstancePool.TryGetValue(storyId, out info);
            return info;
        }
        private void LoadCustomCommandsAndFunctions()
        {
            string valFile = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + "Story/Common/CustomFunctions.dsl");
            string cmdFile = HomePath.GetAbsolutePath(FilePathDefine_Client.C_DslPath + "Story/Common/CustomCommands.dsl");
            Dsl.DslFile file1 = CustomCommandFunctionParser.LoadStory(valFile);
            Dsl.DslFile file2 = CustomCommandFunctionParser.LoadStory(cmdFile);
            CustomCommandFunctionParser.FirstParse(file1, file2);
            CustomCommandFunctionParser.FinalParse(file1, file2);
        }
        private void AddAiStoryInstanceInfoToPool(string storyId, AiStoryInstanceInfo info)
        {
            List<AiStoryInstanceInfo> infos;
            if (m_AiStoryInstancePool.TryGetValue(storyId, out infos)) {
                infos.Add(info);
            } else {
                infos = new List<AiStoryInstanceInfo>();
                infos.Add(info);
                m_AiStoryInstancePool.Add(storyId, infos);
            }
        }
        private AiStoryInstanceInfo GetUnusedAiStoryInstanceInfoFromPool(string storyId)
        {
            AiStoryInstanceInfo info = null;
            List<AiStoryInstanceInfo> infos;
            if (m_AiStoryInstancePool.TryGetValue(storyId, out infos)) {
                int ct = infos.Count;
                for (int ix = 0; ix < ct; ++ix) {
                    if (!infos[ix].m_IsUsed) {
                        info = infos[ix];
                        break;
                    }
                }
            }
            return info;
        }
        private bool IsMatch(string realId, string prefixId)
        {
            if (realId == prefixId || realId.Length > prefixId.Length && realId.StartsWith(prefixId) && realId[prefixId.Length] == ':')
                return true;
            return false;
        }

        private GfxStorySystem() { }

        private class BindedStoryInfo
        {
            internal UnityEngine.Object Object;
            internal StoryInstance Instance;
        }
        private List<BindedStoryInfo> m_BindedStoryInstances = new List<BindedStoryInfo>();
        private SimpleObjectPool<BoxedValueList> m_BoxedValueListPool = new SimpleObjectPool<BoxedValueList>();

        private int m_SceneId = 0;
        private StrBoxedValueDict m_GlobalVariables = new StrBoxedValueDict();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private Dictionary<string, List<AiStoryInstanceInfo>> m_AiStoryInstancePool = new Dictionary<string, List<AiStoryInstanceInfo>>();

        public static GfxStorySystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static GfxStorySystem s_Instance = new GfxStorySystem();

        public static void ThreadInitMask()
        {
            StoryCommandManager.ThreadCommandGroupsMask = (ulong)((1 << (int)StoryCommandGroupDefine.GM) + (1 << (int)StoryCommandGroupDefine.GFX));
            StoryFunctionManager.ThreadFunctionGroupsMask = (ulong)((1 << (int)StoryFunctionGroupDefine.GM) + (1 << (int)StoryFunctionGroupDefine.GFX));
        }
    }
}
