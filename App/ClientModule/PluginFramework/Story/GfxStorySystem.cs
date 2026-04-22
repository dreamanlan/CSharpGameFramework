using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
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
            var registry = DslCalculatorHost.GetSharedApiRegistry();

            //register story commands
            registry.Register("preload", "preload command", new ExpressionFactoryHelper<Story.Commands.PreloadCommand>());
            registry.Register("startstory", "startstory command", new ExpressionFactoryHelper<Story.Commands.StartStoryCommand>());
            registry.Register("stopstory", "stopstory command", new ExpressionFactoryHelper<Story.Commands.StopStoryCommand>());
            registry.Register("waitstory", "waitstory command", new ExpressionFactoryHelper<Story.Commands.WaitStoryCommand>());
            registry.Register("pausestory", "pausestory command", new ExpressionFactoryHelper<Story.Commands.PauseStoryCommand>());
            registry.Register("resumestory", "resumestory command", new ExpressionFactoryHelper<Story.Commands.ResumeStoryCommand>());
            registry.Register("firemessage", "firemessage command", new ExpressionFactoryHelper<Story.Commands.FireMessageCommand>());
            registry.Register("fireconcurrentmessage", "fireconcurrentmessage command", new ExpressionFactoryHelper<Story.Commands.FireConcurrentMessageCommand>());
            registry.Register("waitallmessage", "waitallmessage command", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageCommand>());
            registry.Register("waitallmessagehandler", "waitallmessagehandler command", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
            registry.Register("suspendallmessagehandler", "suspendallmessagehandler command", new ExpressionFactoryHelper<Story.Commands.SuspendAllMessageHandlerCommand>());
            registry.Register("resumeallmessagehandler", "resumeallmessagehandler command", new ExpressionFactoryHelper<Story.Commands.ResumeAllMessageHandlerCommand>());
            registry.Register("sendroomstorymessage", "sendroomstorymessage command", new ExpressionFactoryHelper<Story.Commands.SendRoomStoryMessageCommand>());
            registry.Register("sendserverstorymessage", "sendserverstorymessage command", new ExpressionFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
            registry.Register("sendclientstorymessage", "sendclientstorymessage command", new ExpressionFactoryHelper<Story.Commands.DummyCommand>());
            registry.Register("publishgfxevent", "publishgfxevent command", new ExpressionFactoryHelper<Story.Commands.PublishGfxEventCommand>());
            registry.Register("sendgfxmessage", "sendgfxmessage command", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageCommand>());
            registry.Register("sendgfxmessagewithtag", "sendgfxmessagewithtag command", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());
            registry.Register("sendgfxmessagewithgameobject", "sendgfxmessagewithgameobject command", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageWithGameObjectCommand>());
            registry.Register("sendskillmessage", "sendskillmessage command", new ExpressionFactoryHelper<Story.Commands.SendSkillMessageCommand>());
            registry.Register("creategameobject", "creategameobject command", new ExpressionFactoryHelper<Story.Commands.CreateGameObjectCommand>());
            registry.Register("settransform", "settransform command", new ExpressionFactoryHelper<Story.Commands.SetTransformCommand>());
            registry.Register("addtransform", "addtransform command", new ExpressionFactoryHelper<Story.Commands.AddTransformCommand>());
            registry.Register("destroygameobject", "destroygameobject command", new ExpressionFactoryHelper<Story.Commands.DestroyGameObjectCommand>());
            registry.Register("setparent", "setparent command", new ExpressionFactoryHelper<Story.Commands.SetParentCommand>());
            registry.Register("setactive", "setactive command", new ExpressionFactoryHelper<Story.Commands.SetActiveCommand>());
            registry.Register("setvisible", "setvisible command", new ExpressionFactoryHelper<Story.Commands.SetVisibleCommand>());
            registry.Register("putonground", "putonground command", new ExpressionFactoryHelper<Story.Commands.PutOnGroundCommand>());
            registry.Register("setnavmeshagentenable", "setnavmeshagentenable command", new ExpressionFactoryHelper<Story.Commands.SetNavMeshAgentEnabledCommand>());
            registry.Register("setnavmeshagentenabled", "setnavmeshagentenabled command", new ExpressionFactoryHelper<Story.Commands.SetNavMeshAgentEnabledCommand>());
            registry.Register("addcomponent", "addcomponent command", new ExpressionFactoryHelper<Story.Commands.AddComponentCommand>());
            registry.Register("removecomponent", "removecomponent command", new ExpressionFactoryHelper<Story.Commands.RemoveComponentCommand>());
            registry.Register("installplugin", "installplugin command", new ExpressionFactoryHelper<Story.Commands.InstallPluginCommand>());
            registry.Register("removeplugin", "removeplugin command", new ExpressionFactoryHelper<Story.Commands.RemovePluginCommand>());
            registry.Register("openurl", "openurl command", new ExpressionFactoryHelper<Story.Commands.OpenUrlCommand>());
            registry.Register("quit", "quit command", new ExpressionFactoryHelper<Story.Commands.QuitCommand>());
            registry.Register("loadui", "loadui command", new ExpressionFactoryHelper<Story.Commands.LoadUiCommand>());
            registry.Register("bindui", "bindui command", new ExpressionFactoryHelper<Story.Commands.BindUiCommand>());
            registry.Register("setactorscale", "setactorscale command", new ExpressionFactoryHelper<Story.Commands.SetActorScaleCommand>());

            registry.Register("changescene", "changescene command", new ExpressionFactoryHelper<Story.Commands.ChangeSceneCommand>());
            registry.Register("openbattle", "openbattle command", new ExpressionFactoryHelper<Story.Commands.OpenBattleCommand>());
            registry.Register("closebattle", "closebattle command", new ExpressionFactoryHelper<Story.Commands.CloseBattleCommand>());
            registry.Register("createscenelogic", "createscenelogic command", new ExpressionFactoryHelper<Story.Commands.CreateSceneLogicCommand>());
            registry.Register("destroyscenelogic", "destroyscenelogic command", new ExpressionFactoryHelper<Story.Commands.DestroySceneLogicCommand>());
            registry.Register("pausescenelogic", "pausescenelogic command", new ExpressionFactoryHelper<Story.Commands.PauseSceneLogicCommand>());
            registry.Register("restarttimeout", "restarttimeout command", new ExpressionFactoryHelper<Story.Commands.RestartTimeoutCommand>());
            registry.Register("highlightpromptwithdict", "highlightpromptwithdict command", new ExpressionFactoryHelper<Story.Commands.HighlightPromptWithDictCommand>());
            registry.Register("highlightprompt", "highlightprompt command", new ExpressionFactoryHelper<Story.Commands.HighlightPromptCommand>());

            registry.Register("blackboardset", "blackboardset command", new ExpressionFactoryHelper<Story.Commands.BlackboardSetCommand>());
            registry.Register("blackboardclear", "blackboardclear command", new ExpressionFactoryHelper<Story.Commands.BlackboardClearCommand>());
            registry.Register("camerafollow", "camerafollow command", new ExpressionFactoryHelper<Story.Commands.CameraFollowCommand>());
            registry.Register("cameralook", "cameralook command", new ExpressionFactoryHelper<Story.Commands.CameraLookCommand>());
            registry.Register("cameralookimmediately", "cameralookimmediately command", new ExpressionFactoryHelper<Story.Commands.CameraLookImmediatelyCommand>());
            registry.Register("cameralooktoward", "cameralooktoward command", new ExpressionFactoryHelper<Story.Commands.CameraLookTowardCommand>());
            registry.Register("camerafixedyaw", "camerafixedyaw command", new ExpressionFactoryHelper<Story.Commands.CameraFixedYawCommand>());
            registry.Register("camerayaw", "camerayaw command", new ExpressionFactoryHelper<Story.Commands.CameraYawCommand>());
            registry.Register("cameraheight", "cameraheight command", new ExpressionFactoryHelper<Story.Commands.CameraHeightCommand>());
            registry.Register("cameradistance", "cameradistance command", new ExpressionFactoryHelper<Story.Commands.CameraDistanceCommand>());
            registry.Register("camerasetdistanceheight", "camerasetdistanceheight command", new ExpressionFactoryHelper<Story.Commands.CameraSetDistanceHeightCommand>());
            registry.Register("cameraresetdistanceheight", "cameraresetdistanceheight command", new ExpressionFactoryHelper<Story.Commands.CameraResetDistanceHeightCommand>());
            registry.Register("camerasetfollowspeed", "camerasetfollowspeed command", new ExpressionFactoryHelper<Story.Commands.CameraSetFollowSpeedCommand>());
            registry.Register("cameraresetfollowspeed", "cameraresetfollowspeed command", new ExpressionFactoryHelper<Story.Commands.CameraResetFollowSpeedCommand>());
            registry.Register("camerafollowobj", "camerafollowobj command", new ExpressionFactoryHelper<Story.Commands.CameraFollowObjCommand>());
            registry.Register("cameralookobj", "cameralookobj command", new ExpressionFactoryHelper<Story.Commands.CameraLookObjCommand>());
            registry.Register("cameralookobjimmediately", "cameralookobjimmediately command", new ExpressionFactoryHelper<Story.Commands.CameraLookObjImmediatelyCommand>());
            registry.Register("cameralooktowardobj", "cameralooktowardobj command", new ExpressionFactoryHelper<Story.Commands.CameraLookTowardObjCommand>());
            registry.Register("cameralookcopy", "cameralookcopy command", new ExpressionFactoryHelper<Story.Commands.CameraLookCopyCommand>());
            registry.Register("cameralookobjcopy", "cameralookobjcopy command", new ExpressionFactoryHelper<Story.Commands.CameraLookObjCopyCommand>());
            registry.Register("cameraenable", "cameraenable command", new ExpressionFactoryHelper<Story.Commands.CameraEnableCommand>());
            registry.Register("lockframe", "lockframe command", new ExpressionFactoryHelper<Story.Commands.LockFrameCommand>());
            registry.Register("showdlg", "showdlg command", new ExpressionFactoryHelper<Story.Commands.ShowDlgCommand>());
            registry.Register("areadetect", "areadetect command", new ExpressionFactoryHelper<Story.Commands.AreaDetectCommand>());

            registry.Register("gameobjectanimation", "gameobjectanimation command", new ExpressionFactoryHelper<Story.Commands.GameObjectAnimationCommand>());
            registry.Register("gameobjectanimationparam", "gameobjectanimationparam command", new ExpressionFactoryHelper<Story.Commands.GameObjectAnimationParamCommand>());
            registry.Register("gameobjectcastskill", "gameobjectcastskill command", new ExpressionFactoryHelper<Story.Commands.GameObjectCastSkillCommand>());
            registry.Register("gameobjectstopskill", "gameobjectstopskill command", new ExpressionFactoryHelper<Story.Commands.GameObjectStopSkillCommand>());

            registry.Register("createnpc", "createnpc command", new ExpressionFactoryHelper<Story.Commands.CreateNpcCommand>());
            registry.Register("destroynpc", "destroynpc command", new ExpressionFactoryHelper<Story.Commands.DestroyNpcCommand>());
            registry.Register("destroynpcwithobjid", "destroynpcwithobjid command", new ExpressionFactoryHelper<Story.Commands.DestroyNpcWithObjIdCommand>());
            registry.Register("npcface", "npcface command", new ExpressionFactoryHelper<Story.Commands.NpcFaceCommand>());
            registry.Register("npcmove", "npcmove command", new ExpressionFactoryHelper<Story.Commands.NpcMoveCommand>());
            registry.Register("npcmovewithwaypoints", "npcmovewithwaypoints command", new ExpressionFactoryHelper<Story.Commands.NpcMoveWithWaypointsCommand>());
            registry.Register("npcstop", "npcstop command", new ExpressionFactoryHelper<Story.Commands.NpcStopCommand>());
            registry.Register("npcattack", "npcattack command", new ExpressionFactoryHelper<Story.Commands.NpcAttackCommand>());
            registry.Register("npcsetformation", "npcsetformation command", new ExpressionFactoryHelper<Story.Commands.NpcSetFormationCommand>());
            registry.Register("npcenableai", "npcenableai command", new ExpressionFactoryHelper<Story.Commands.NpcEnableAiCommand>());
            registry.Register("npcsetai", "npcsetai command", new ExpressionFactoryHelper<Story.Commands.NpcSetAiCommand>());
            registry.Register("npcsetaitarget", "npcsetaitarget command", new ExpressionFactoryHelper<Story.Commands.NpcSetAiTargetCommand>());
            registry.Register("npcanimation", "npcanimation command", new ExpressionFactoryHelper<Story.Commands.NpcAnimationCommand>());
            registry.Register("npcanimationparam", "npcanimationparam command", new ExpressionFactoryHelper<Story.Commands.NpcAnimationParamCommand>());
            registry.Register("npcaddimpact", "npcaddimpact command", new ExpressionFactoryHelper<Story.Commands.NpcAddImpactCommand>());
            registry.Register("npcremoveimpact", "npcremoveimpact command", new ExpressionFactoryHelper<Story.Commands.NpcRemoveImpactCommand>());
            registry.Register("npccastskill", "npccastskill command", new ExpressionFactoryHelper<Story.Commands.NpcCastSkillCommand>());
            registry.Register("npcstopskill", "npcstopskill command", new ExpressionFactoryHelper<Story.Commands.NpcStopSkillCommand>());
            registry.Register("npclisten", "npclisten command", new ExpressionFactoryHelper<Story.Commands.NpcListenCommand>());
            registry.Register("npcsetcamp", "npcsetcamp command", new ExpressionFactoryHelper<Story.Commands.NpcSetCampCommand>());
            registry.Register("npcsetsummonerid", "npcsetsummonerid command", new ExpressionFactoryHelper<Story.Commands.NpcSetSummonerIdCommand>());
            registry.Register("npcsetsummonskillid", "npcsetsummonskillid command", new ExpressionFactoryHelper<Story.Commands.NpcSetSummonSkillIdCommand>());
            registry.Register("objface", "objface command", new ExpressionFactoryHelper<Story.Commands.ObjFaceCommand>());
            registry.Register("objmove", "objmove command", new ExpressionFactoryHelper<Story.Commands.ObjMoveCommand>());
            registry.Register("objmovewithwaypoints", "objmovewithwaypoints command", new ExpressionFactoryHelper<Story.Commands.ObjMoveWithWaypointsCommand>());
            registry.Register("objstop", "objstop command", new ExpressionFactoryHelper<Story.Commands.ObjStopCommand>());
            registry.Register("objattack", "objattack command", new ExpressionFactoryHelper<Story.Commands.ObjAttackCommand>());
            registry.Register("objsetformation", "objsetformation command", new ExpressionFactoryHelper<Story.Commands.ObjSetFormationCommand>());
            registry.Register("objenableai", "objenableai command", new ExpressionFactoryHelper<Story.Commands.ObjEnableAiCommand>());
            registry.Register("objsetai", "objsetai command", new ExpressionFactoryHelper<Story.Commands.ObjSetAiCommand>());
            registry.Register("objsetaitarget", "objsetaitarget command", new ExpressionFactoryHelper<Story.Commands.ObjSetAiTargetCommand>());
            registry.Register("objanimation", "objanimation command", new ExpressionFactoryHelper<Story.Commands.ObjAnimationCommand>());
            registry.Register("objanimationparam", "objanimationparam command", new ExpressionFactoryHelper<Story.Commands.ObjAnimationParamCommand>());
            registry.Register("objaddimpact", "objaddimpact command", new ExpressionFactoryHelper<Story.Commands.ObjAddImpactCommand>());
            registry.Register("objremoveimpact", "objremoveimpact command", new ExpressionFactoryHelper<Story.Commands.ObjRemoveImpactCommand>());
            registry.Register("objcastskill", "objcastskill command", new ExpressionFactoryHelper<Story.Commands.ObjCastSkillCommand>());
            registry.Register("objstopskill", "objstopskill command", new ExpressionFactoryHelper<Story.Commands.ObjStopSkillCommand>());
            registry.Register("objlisten", "objlisten command", new ExpressionFactoryHelper<Story.Commands.ObjListenCommand>());
            registry.Register("objsetcamp", "objsetcamp command", new ExpressionFactoryHelper<Story.Commands.ObjSetCampCommand>());
            registry.Register("objsetsummonerid", "objsetsummonerid command", new ExpressionFactoryHelper<Story.Commands.ObjSetSummonerIdCommand>());
            registry.Register("objsetsummonskillid", "objsetsummonskillid command", new ExpressionFactoryHelper<Story.Commands.ObjSetSummonSkillIdCommand>());

            registry.Register("sethp", "sethp command", new ExpressionFactoryHelper<Story.Commands.SetHpCommand>());
            registry.Register("setenergy", "setenergy command", new ExpressionFactoryHelper<Story.Commands.SetEnergyCommand>());
            registry.Register("objset", "objset command", new ExpressionFactoryHelper<Story.Commands.ObjSetCommand>());
            registry.Register("setlevel", "setlevel command", new ExpressionFactoryHelper<Story.Commands.SetLevelCommand>());
            registry.Register("setattr", "setattr command", new ExpressionFactoryHelper<Story.Commands.SetAttrCommand>());
            registry.Register("setunitid", "setunitid command", new ExpressionFactoryHelper<Story.Commands.SetUnitIdCommand>());
            registry.Register("setleaderid", "setleaderid command", new ExpressionFactoryHelper<Story.Commands.SetLeaderIdCommand>());
            registry.Register("markcontrolbystory", "markcontrolbystory command", new ExpressionFactoryHelper<Story.Commands.MarkControlByStoryCommand>());

            //register value or functions
            registry.Register("gettime", "gettime function", new ExpressionFactoryHelper<Story.Functions.GetTimeFunction>());
            registry.Register("gettimescale", "gettimescale function", new ExpressionFactoryHelper<Story.Functions.GetTimeScaleFunction>());
            registry.Register("isactive", "isactive function", new ExpressionFactoryHelper<Story.Functions.IsActiveFunction>());
            registry.Register("isreallyactive", "isreallyactive function", new ExpressionFactoryHelper<Story.Functions.IsReallyActiveFunction>());
            registry.Register("isvisible", "isvisible function", new ExpressionFactoryHelper<Story.Functions.IsVisibleFunction>());
            registry.Register("isnavmeshagentenabled", "isnavmeshagentenabled function", new ExpressionFactoryHelper<Story.Functions.IsNavmeshAgentEnabledFunction>());
            registry.Register("getcomponent", "getcomponent function", new ExpressionFactoryHelper<Story.Functions.GetComponentFunction>());
            registry.Register("getcomponentinparent", "getcomponentinparent function", new ExpressionFactoryHelper<Story.Functions.GetComponentInParentFunction>());
            registry.Register("getcomponentinchildren", "getcomponentinchildren function", new ExpressionFactoryHelper<Story.Functions.GetComponentInChildrenFunction>());
            registry.Register("getcomponents", "getcomponents function", new ExpressionFactoryHelper<Story.Functions.GetComponentsFunction>());
            registry.Register("getcomponentsinparent", "getcomponentsinparent function", new ExpressionFactoryHelper<Story.Functions.GetComponentsInParentFunction>());
            registry.Register("getcomponentsinchildren", "getcomponentsinchildren function", new ExpressionFactoryHelper<Story.Functions.GetComponentsInChildrenFunction>());
            registry.Register("getgameobject", "getgameobject function", new ExpressionFactoryHelper<Story.Functions.GetGameObjectFunction>());
            registry.Register("getparent", "getparent function", new ExpressionFactoryHelper<Story.Functions.GetParentFunction>());
            registry.Register("findchild", "findchild function", new ExpressionFactoryHelper<Story.Functions.FindChildFunction>());
            registry.Register("getchildcount", "getchildcount function", new ExpressionFactoryHelper<Story.Functions.GetChildCountFunction>());
            registry.Register("getchild", "getchild function", new ExpressionFactoryHelper<Story.Functions.GetChildFunction>());
            registry.Register("getunitytype", "getunitytype function", new ExpressionFactoryHelper<Story.Functions.GetUnityTypeFunction>());
            registry.Register("getunityuitype", "getunityuitype function", new ExpressionFactoryHelper<Story.Functions.GetUnityUiTypeFunction>());
            registry.Register("getusertype", "getusertype function", new ExpressionFactoryHelper<Story.Functions.GetUserTypeFunction>());
            registry.Register("getactor", "getactor function", new ExpressionFactoryHelper<Story.Functions.GetActorFunction>());
            registry.Register("getentityinfo", "getentityinfo function", new ExpressionFactoryHelper<Story.Functions.GetEntityInfoFunction>());
            registry.Register("getglobal", "getglobal function", new ExpressionFactoryHelper<Story.Functions.GetGlobalFunction>());
            registry.Register("pluginFramework", "pluginFramework function", new ExpressionFactoryHelper<Story.Functions.PluginFrameworkFunction>());
            registry.Register("getentityview", "getentityview function", new ExpressionFactoryHelper<Story.Functions.GetEntityViewFunction>());
            registry.Register("dictget", "dictget function", new ExpressionFactoryHelper<Story.Functions.DictGetFunction>());
            registry.Register("dictformat", "dictformat function", new ExpressionFactoryHelper<Story.Functions.DictFormatFunction>());
            registry.Register("isclient", "isclient function", new ExpressionFactoryHelper<Story.Functions.IsClientFunction>());
            registry.Register("getsceneid", "getsceneid function", new ExpressionFactoryHelper<Story.Functions.GetSceneIdFunction>());

            registry.Register("blackboardget", "blackboardget function", new ExpressionFactoryHelper<Story.Functions.BlackboardGetFunction>());
            registry.Register("getdialogitem", "getdialogitem function", new ExpressionFactoryHelper<Story.Functions.GetDialogItemFunction>());
            registry.Register("offsetspline", "offsetspline function", new ExpressionFactoryHelper<Story.Functions.OffsetSplineFunction>());
            registry.Register("offsetvector3", "offsetvector3 function", new ExpressionFactoryHelper<Story.Functions.OffsetVector3Function>());
            registry.Register("getactoricon", "getactoricon function", new ExpressionFactoryHelper<Story.Functions.GetActorIconFunction>());
            registry.Register("getmonsterinfo", "getmonsterinfo function", new ExpressionFactoryHelper<Story.Functions.GetMonsterInfoFunction>());
            registry.Register("getaidata", "getaidata function", new ExpressionFactoryHelper<Story.Functions.GetAiDataFunction>());

            registry.Register("npcidlist", "npcidlist function", new ExpressionFactoryHelper<Story.Functions.NpcIdListFunction>());
            registry.Register("combatnpccount", "combatnpccount function", new ExpressionFactoryHelper<Story.Functions.CombatNpcCountFunction>());
            registry.Register("npccount", "npccount function", new ExpressionFactoryHelper<Story.Functions.NpcCountFunction>());
            registry.Register("unitid2objid", "unitid2objid function", new ExpressionFactoryHelper<Story.Functions.UnitId2ObjIdFunction>());
            registry.Register("objid2unitid", "objid2unitid function", new ExpressionFactoryHelper<Story.Functions.ObjId2UnitIdFunction>());
            registry.Register("unitid2uniqueid", "unitid2uniqueid function", new ExpressionFactoryHelper<Story.Functions.UnitId2UniqueIdFunction>());
            registry.Register("objid2uniqueid", "objid2uniqueid function", new ExpressionFactoryHelper<Story.Functions.ObjId2UniqueIdFunction>());

            registry.Register("npcgetformation", "npcgetformation function", new ExpressionFactoryHelper<Story.Functions.NpcGetFormationFunction>());
            registry.Register("npcgetnpctype", "npcgetnpctype function", new ExpressionFactoryHelper<Story.Functions.NpcGetNpcTypeFunction>());
            registry.Register("npcgetsummonerid", "npcgetsummonerid function", new ExpressionFactoryHelper<Story.Functions.NpcGetSummonerIdFunction>());
            registry.Register("npcgetsummonskillid", "npcgetsummonskillid function", new ExpressionFactoryHelper<Story.Functions.NpcGetSummonSkillIdFunction>());
            registry.Register("npcfindimpactseqbyid", "npcfindimpactseqbyid function", new ExpressionFactoryHelper<Story.Functions.NpcFindImpactSeqByIdFunction>());

            registry.Register("objgetformation", "objgetformation function", new ExpressionFactoryHelper<Story.Functions.ObjGetFormationFunction>());
            registry.Register("objgetnpctype", "objgetnpctype function", new ExpressionFactoryHelper<Story.Functions.ObjGetNpcTypeFunction>());
            registry.Register("objgetsummonerid", "objgetsummonerid function", new ExpressionFactoryHelper<Story.Functions.ObjGetSummonerIdFunction>());
            registry.Register("objgetsummonskillid", "objgetsummonskillid function", new ExpressionFactoryHelper<Story.Functions.ObjGetSummonSkillIdFunction>());
            registry.Register("objfindimpactseqbyid", "objfindimpactseqbyid function", new ExpressionFactoryHelper<Story.Functions.ObjFindImpactSeqByIdFunction>());
            registry.Register("isenemy", "isenemy function", new ExpressionFactoryHelper<Story.Functions.IsEnemyFunction>());
            registry.Register("isfriend", "isfriend function", new ExpressionFactoryHelper<Story.Functions.IsFriendFunction>());

            registry.Register("getposition", "getposition function", new ExpressionFactoryHelper<Story.Functions.GetPositionFunction>());
            registry.Register("getpositionx", "getpositionx function", new ExpressionFactoryHelper<Story.Functions.GetPositionXFunction>());
            registry.Register("getpositiony", "getpositiony function", new ExpressionFactoryHelper<Story.Functions.GetPositionYFunction>());
            registry.Register("getpositionz", "getpositionz function", new ExpressionFactoryHelper<Story.Functions.GetPositionZFunction>());
            registry.Register("getrotation", "getrotation function", new ExpressionFactoryHelper<Story.Functions.GetRotationFunction>());
            registry.Register("getrotationx", "getrotationx function", new ExpressionFactoryHelper<Story.Functions.GetRotationXFunction>());
            registry.Register("getrotationy", "getrotationy function", new ExpressionFactoryHelper<Story.Functions.GetRotationYFunction>());
            registry.Register("getrotationz", "getrotationz function", new ExpressionFactoryHelper<Story.Functions.GetRotationZFunction>());
            registry.Register("getscale", "getscale function", new ExpressionFactoryHelper<Story.Functions.GetScaleFunction>());
            registry.Register("getscalex", "getscalex function", new ExpressionFactoryHelper<Story.Functions.GetScaleXFunction>());
            registry.Register("getscaley", "getscaley function", new ExpressionFactoryHelper<Story.Functions.GetScaleYFunction>());
            registry.Register("getscalez", "getscalez function", new ExpressionFactoryHelper<Story.Functions.GetScaleZFunction>());
            registry.Register("position", "position function", new ExpressionFactoryHelper<Story.Functions.Vector3Exp>());
            registry.Register("rotation", "rotation function", new ExpressionFactoryHelper<Story.Functions.Vector3Exp>());
            registry.Register("scale", "scale function", new ExpressionFactoryHelper<Story.Functions.Vector3Exp>());
            registry.Register("getcamp", "getcamp function", new ExpressionFactoryHelper<Story.Functions.GetCampFunction>());
            registry.Register("iscombatnpc", "iscombatnpc function", new ExpressionFactoryHelper<Story.Functions.IsCombatNpcFunction>());
            registry.Register("gethp", "gethp function", new ExpressionFactoryHelper<Story.Functions.GetHpFunction>());
            registry.Register("getenergy", "getenergy function", new ExpressionFactoryHelper<Story.Functions.GetEnergyFunction>());
            registry.Register("getmaxhp", "getmaxhp function", new ExpressionFactoryHelper<Story.Functions.GetMaxHpFunction>());
            registry.Register("getmaxenergy", "getmaxenergy function", new ExpressionFactoryHelper<Story.Functions.GetMaxEnergyFunction>());
            registry.Register("calcoffset", "calcoffset function", new ExpressionFactoryHelper<Story.Functions.CalcOffsetFunction>());
            registry.Register("calcdir", "calcdir function", new ExpressionFactoryHelper<Story.Functions.CalcDirFunction>());
            registry.Register("objget", "objget function", new ExpressionFactoryHelper<Story.Functions.ObjGetFunction>());
            registry.Register("gettableid", "gettableid function", new ExpressionFactoryHelper<Story.Functions.GetTableIdFunction>());
            registry.Register("getlevel", "getlevel function", new ExpressionFactoryHelper<Story.Functions.GetLevelFunction>());
            registry.Register("getattr", "getattr function", new ExpressionFactoryHelper<Story.Functions.GetAttrFunction>());
            registry.Register("iscontrolbystory", "iscontrolbystory function", new ExpressionFactoryHelper<Story.Functions.IsControlByStoryFunction>());
            registry.Register("cancastskill", "cancastskill function", new ExpressionFactoryHelper<Story.Functions.CanCastSkillFunction>());
            registry.Register("isundercontrol", "isundercontrol function", new ExpressionFactoryHelper<Story.Functions.IsUnderControlFunction>());
            registry.Register("getplayerid", "getplayerid function", new ExpressionFactoryHelper<Story.Functions.GetPlayerIdFunction>());
            registry.Register("getplayerunitid", "getplayerunitid function", new ExpressionFactoryHelper<Story.Functions.GetPlayerUnitIdFunction>());
            registry.Register("getleaderid", "getleaderid function", new ExpressionFactoryHelper<Story.Functions.GetLeaderIdFunction>());
            registry.Register("getleadertableid", "getleadertableid function", new ExpressionFactoryHelper<Story.Functions.GetLeaderTableIdFunction>());
            registry.Register("getmembercount", "getmembercount function", new ExpressionFactoryHelper<Story.Functions.GetMemberCountFunction>());
            registry.Register("getmembertableid", "getmembertableid function", new ExpressionFactoryHelper<Story.Functions.GetMemberTableIdFunction>());
            registry.Register("getmemberlevel", "getmemberlevel function", new ExpressionFactoryHelper<Story.Functions.GetMemberLevelFunction>());

            GmCommands.GmExpressionRegistrar.RegisterGmExpressions(registry);
        }
        public void Reset()
        {
            Reset(true);
        }
        public void Reset(bool logIfTriggered)
        {
            m_ContextVariables.Clear();
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
        public StrBoxedValueDict ContextVariables
        {
            get { return m_ContextVariables; }
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
                    info.ContextVariables = m_ContextVariables;
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
                inst.ContextVariables = m_ContextVariables;
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
        private StrBoxedValueDict m_ContextVariables = new StrBoxedValueDict();

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
            //DslCalculator does not use thread masks
        }
    }
}
