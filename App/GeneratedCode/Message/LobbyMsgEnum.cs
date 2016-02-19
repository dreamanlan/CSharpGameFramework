//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/LobbyMsg.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFrameworkMessage
{
	public enum LobbyMessageDefine
	{
		NodeRegister = 1,
		NodeRegisterResult,
		VersionVerify,
		VersionVerifyResult,
		DirectLogin,
		AccountLogout,
		Logout,
		GetQueueingCount,
		UserHeartbeat,
		KickUser,
		TooManyOperations,
		RequestNickname,
		RequestSceneRoomInfo,
		RequestSceneRoomList,
		ServerShutdown,
		GmCode,
		AccountLogin,
		AccountLoginResult,
		QueueingCountResult,
		RequestNicknameResult,
		ActivateAccount,
		ActivateAccountResult,
		ChangeName,
		ChangeNameResult,
		RoleEnter,
		RoleEnterResult,
		EnterScene,
		ChangeSceneRoom,
		EnterSceneResult,
		QuitRoom,
		Msg_CLC_StoryMessage,
		Msg_LC_PublishEvent,
		Msg_LC_SendGfxMessage,
		Msg_LC_HighlightPrompt,
		MaxNum
	}

	public static class LobbyMessageDefine2Type
	{
		public static Type Query(int id)
		{
			Type t;
			s_LobbyMessageDefine2Type.TryGetValue(id, out t);
			return t;
		}

		public static int Query(Type t)
		{
			int id;
			s_Type2LobbyMessageDefine.TryGetValue(t, out id);
			return id;
		}

		static LobbyMessageDefine2Type()
		{
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.AccountLogin, typeof(AccountLogin));
			s_Type2LobbyMessageDefine.Add(typeof(AccountLogin), (int)LobbyMessageDefine.AccountLogin);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.AccountLoginResult, typeof(AccountLoginResult));
			s_Type2LobbyMessageDefine.Add(typeof(AccountLoginResult), (int)LobbyMessageDefine.AccountLoginResult);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.AccountLogout, typeof(AccountLogout));
			s_Type2LobbyMessageDefine.Add(typeof(AccountLogout), (int)LobbyMessageDefine.AccountLogout);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.ActivateAccount, typeof(ActivateAccount));
			s_Type2LobbyMessageDefine.Add(typeof(ActivateAccount), (int)LobbyMessageDefine.ActivateAccount);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.ActivateAccountResult, typeof(ActivateAccountResult));
			s_Type2LobbyMessageDefine.Add(typeof(ActivateAccountResult), (int)LobbyMessageDefine.ActivateAccountResult);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.ChangeName, typeof(ChangeName));
			s_Type2LobbyMessageDefine.Add(typeof(ChangeName), (int)LobbyMessageDefine.ChangeName);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.ChangeNameResult, typeof(ChangeNameResult));
			s_Type2LobbyMessageDefine.Add(typeof(ChangeNameResult), (int)LobbyMessageDefine.ChangeNameResult);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.ChangeSceneRoom, typeof(ChangeSceneRoom));
			s_Type2LobbyMessageDefine.Add(typeof(ChangeSceneRoom), (int)LobbyMessageDefine.ChangeSceneRoom);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.DirectLogin, typeof(DirectLogin));
			s_Type2LobbyMessageDefine.Add(typeof(DirectLogin), (int)LobbyMessageDefine.DirectLogin);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.EnterScene, typeof(EnterScene));
			s_Type2LobbyMessageDefine.Add(typeof(EnterScene), (int)LobbyMessageDefine.EnterScene);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.EnterSceneResult, typeof(EnterSceneResult));
			s_Type2LobbyMessageDefine.Add(typeof(EnterSceneResult), (int)LobbyMessageDefine.EnterSceneResult);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.GetQueueingCount, typeof(GetQueueingCount));
			s_Type2LobbyMessageDefine.Add(typeof(GetQueueingCount), (int)LobbyMessageDefine.GetQueueingCount);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.GmCode, typeof(GmCode));
			s_Type2LobbyMessageDefine.Add(typeof(GmCode), (int)LobbyMessageDefine.GmCode);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.KickUser, typeof(KickUser));
			s_Type2LobbyMessageDefine.Add(typeof(KickUser), (int)LobbyMessageDefine.KickUser);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Logout, typeof(Logout));
			s_Type2LobbyMessageDefine.Add(typeof(Logout), (int)LobbyMessageDefine.Logout);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CLC_StoryMessage, typeof(Msg_CLC_StoryMessage));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CLC_StoryMessage), (int)LobbyMessageDefine.Msg_CLC_StoryMessage);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_HighlightPrompt, typeof(Msg_LC_HighlightPrompt));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_HighlightPrompt), (int)LobbyMessageDefine.Msg_LC_HighlightPrompt);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_PublishEvent, typeof(Msg_LC_PublishEvent));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_PublishEvent), (int)LobbyMessageDefine.Msg_LC_PublishEvent);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_SendGfxMessage, typeof(Msg_LC_SendGfxMessage));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_SendGfxMessage), (int)LobbyMessageDefine.Msg_LC_SendGfxMessage);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.NodeRegister, typeof(NodeRegister));
			s_Type2LobbyMessageDefine.Add(typeof(NodeRegister), (int)LobbyMessageDefine.NodeRegister);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.NodeRegisterResult, typeof(NodeRegisterResult));
			s_Type2LobbyMessageDefine.Add(typeof(NodeRegisterResult), (int)LobbyMessageDefine.NodeRegisterResult);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.QueueingCountResult, typeof(QueueingCountResult));
			s_Type2LobbyMessageDefine.Add(typeof(QueueingCountResult), (int)LobbyMessageDefine.QueueingCountResult);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.QuitRoom, typeof(QuitRoom));
			s_Type2LobbyMessageDefine.Add(typeof(QuitRoom), (int)LobbyMessageDefine.QuitRoom);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.RequestNickname, typeof(RequestNickname));
			s_Type2LobbyMessageDefine.Add(typeof(RequestNickname), (int)LobbyMessageDefine.RequestNickname);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.RequestNicknameResult, typeof(RequestNicknameResult));
			s_Type2LobbyMessageDefine.Add(typeof(RequestNicknameResult), (int)LobbyMessageDefine.RequestNicknameResult);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.RequestSceneRoomInfo, typeof(RequestSceneRoomInfo));
			s_Type2LobbyMessageDefine.Add(typeof(RequestSceneRoomInfo), (int)LobbyMessageDefine.RequestSceneRoomInfo);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.RequestSceneRoomList, typeof(RequestSceneRoomList));
			s_Type2LobbyMessageDefine.Add(typeof(RequestSceneRoomList), (int)LobbyMessageDefine.RequestSceneRoomList);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.RoleEnter, typeof(RoleEnter));
			s_Type2LobbyMessageDefine.Add(typeof(RoleEnter), (int)LobbyMessageDefine.RoleEnter);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.RoleEnterResult, typeof(RoleEnterResult));
			s_Type2LobbyMessageDefine.Add(typeof(RoleEnterResult), (int)LobbyMessageDefine.RoleEnterResult);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.ServerShutdown, typeof(ServerShutdown));
			s_Type2LobbyMessageDefine.Add(typeof(ServerShutdown), (int)LobbyMessageDefine.ServerShutdown);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.TooManyOperations, typeof(TooManyOperations));
			s_Type2LobbyMessageDefine.Add(typeof(TooManyOperations), (int)LobbyMessageDefine.TooManyOperations);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.UserHeartbeat, typeof(UserHeartbeat));
			s_Type2LobbyMessageDefine.Add(typeof(UserHeartbeat), (int)LobbyMessageDefine.UserHeartbeat);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.VersionVerify, typeof(VersionVerify));
			s_Type2LobbyMessageDefine.Add(typeof(VersionVerify), (int)LobbyMessageDefine.VersionVerify);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.VersionVerifyResult, typeof(VersionVerifyResult));
			s_Type2LobbyMessageDefine.Add(typeof(VersionVerifyResult), (int)LobbyMessageDefine.VersionVerifyResult);
		}

		private static Dictionary<int, Type> s_LobbyMessageDefine2Type = new Dictionary<int, Type>();
		private static Dictionary<Type, int> s_Type2LobbyMessageDefine = new Dictionary<Type, int>();
	}
}

