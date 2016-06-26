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
		Msg_CL_GetMailList,
		Msg_LC_NotifyNewMail,
		GmCode,
		AccountLogin,
		AccountLoginResult,
		QueueingCountResult,
		RequestNicknameResult,
		ChangeName,
		ChangeNameResult,
		RoleEnter,
		FriendInfoForMessage,
		MemberInfoForMessage,
		ItemInfoForMessage,
		RoleEnterResult,
		EnterScene,
		ChangeSceneRoom,
		EnterSceneResult,
		QuitRoom,
		MailItemForMessage,
		MailInfoForMessage,
		Msg_LC_SyncMailList,
		Msg_CL_ReadMail,
		Msg_CL_ReceiveMail,
		Msg_CL_DeleteMail,
		Msg_LC_LackOfSpace,
		Msg_LC_SyncFriendList,
		Msg_CL_AddFriend,
		Msg_LC_AddFriend,
		Msg_CL_RemoveFriend,
		Msg_LC_RemoveFriend,
		Msg_CL_MarkBlack,
		Msg_LC_MarkBlack,
		Msg_LC_SyncRoleInfo,
		Msg_LC_SyncMemberList,
		Msg_LC_SyncItemList,
		Msg_CL_UseItem,
		Msg_CL_DiscardItem,
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
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.FriendInfoForMessage, typeof(FriendInfoForMessage));
			s_Type2LobbyMessageDefine.Add(typeof(FriendInfoForMessage), (int)LobbyMessageDefine.FriendInfoForMessage);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.GetQueueingCount, typeof(GetQueueingCount));
			s_Type2LobbyMessageDefine.Add(typeof(GetQueueingCount), (int)LobbyMessageDefine.GetQueueingCount);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.GmCode, typeof(GmCode));
			s_Type2LobbyMessageDefine.Add(typeof(GmCode), (int)LobbyMessageDefine.GmCode);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.ItemInfoForMessage, typeof(ItemInfoForMessage));
			s_Type2LobbyMessageDefine.Add(typeof(ItemInfoForMessage), (int)LobbyMessageDefine.ItemInfoForMessage);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.KickUser, typeof(KickUser));
			s_Type2LobbyMessageDefine.Add(typeof(KickUser), (int)LobbyMessageDefine.KickUser);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Logout, typeof(Logout));
			s_Type2LobbyMessageDefine.Add(typeof(Logout), (int)LobbyMessageDefine.Logout);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.MailInfoForMessage, typeof(MailInfoForMessage));
			s_Type2LobbyMessageDefine.Add(typeof(MailInfoForMessage), (int)LobbyMessageDefine.MailInfoForMessage);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.MailItemForMessage, typeof(MailItemForMessage));
			s_Type2LobbyMessageDefine.Add(typeof(MailItemForMessage), (int)LobbyMessageDefine.MailItemForMessage);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.MemberInfoForMessage, typeof(MemberInfoForMessage));
			s_Type2LobbyMessageDefine.Add(typeof(MemberInfoForMessage), (int)LobbyMessageDefine.MemberInfoForMessage);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CL_AddFriend, typeof(Msg_CL_AddFriend));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CL_AddFriend), (int)LobbyMessageDefine.Msg_CL_AddFriend);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CL_DeleteMail, typeof(Msg_CL_DeleteMail));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CL_DeleteMail), (int)LobbyMessageDefine.Msg_CL_DeleteMail);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CL_DiscardItem, typeof(Msg_CL_DiscardItem));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CL_DiscardItem), (int)LobbyMessageDefine.Msg_CL_DiscardItem);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CL_GetMailList, typeof(Msg_CL_GetMailList));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CL_GetMailList), (int)LobbyMessageDefine.Msg_CL_GetMailList);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CL_MarkBlack, typeof(Msg_CL_MarkBlack));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CL_MarkBlack), (int)LobbyMessageDefine.Msg_CL_MarkBlack);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CL_ReadMail, typeof(Msg_CL_ReadMail));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CL_ReadMail), (int)LobbyMessageDefine.Msg_CL_ReadMail);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CL_ReceiveMail, typeof(Msg_CL_ReceiveMail));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CL_ReceiveMail), (int)LobbyMessageDefine.Msg_CL_ReceiveMail);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CL_RemoveFriend, typeof(Msg_CL_RemoveFriend));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CL_RemoveFriend), (int)LobbyMessageDefine.Msg_CL_RemoveFriend);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CL_UseItem, typeof(Msg_CL_UseItem));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CL_UseItem), (int)LobbyMessageDefine.Msg_CL_UseItem);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_CLC_StoryMessage, typeof(Msg_CLC_StoryMessage));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_CLC_StoryMessage), (int)LobbyMessageDefine.Msg_CLC_StoryMessage);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_AddFriend, typeof(Msg_LC_AddFriend));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_AddFriend), (int)LobbyMessageDefine.Msg_LC_AddFriend);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_HighlightPrompt, typeof(Msg_LC_HighlightPrompt));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_HighlightPrompt), (int)LobbyMessageDefine.Msg_LC_HighlightPrompt);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_LackOfSpace, typeof(Msg_LC_LackOfSpace));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_LackOfSpace), (int)LobbyMessageDefine.Msg_LC_LackOfSpace);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_MarkBlack, typeof(Msg_LC_MarkBlack));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_MarkBlack), (int)LobbyMessageDefine.Msg_LC_MarkBlack);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_NotifyNewMail, typeof(Msg_LC_NotifyNewMail));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_NotifyNewMail), (int)LobbyMessageDefine.Msg_LC_NotifyNewMail);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_PublishEvent, typeof(Msg_LC_PublishEvent));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_PublishEvent), (int)LobbyMessageDefine.Msg_LC_PublishEvent);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_RemoveFriend, typeof(Msg_LC_RemoveFriend));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_RemoveFriend), (int)LobbyMessageDefine.Msg_LC_RemoveFriend);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_SendGfxMessage, typeof(Msg_LC_SendGfxMessage));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_SendGfxMessage), (int)LobbyMessageDefine.Msg_LC_SendGfxMessage);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_SyncFriendList, typeof(Msg_LC_SyncFriendList));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_SyncFriendList), (int)LobbyMessageDefine.Msg_LC_SyncFriendList);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_SyncItemList, typeof(Msg_LC_SyncItemList));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_SyncItemList), (int)LobbyMessageDefine.Msg_LC_SyncItemList);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_SyncMailList, typeof(Msg_LC_SyncMailList));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_SyncMailList), (int)LobbyMessageDefine.Msg_LC_SyncMailList);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_SyncMemberList, typeof(Msg_LC_SyncMemberList));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_SyncMemberList), (int)LobbyMessageDefine.Msg_LC_SyncMemberList);
			s_LobbyMessageDefine2Type.Add((int)LobbyMessageDefine.Msg_LC_SyncRoleInfo, typeof(Msg_LC_SyncRoleInfo));
			s_Type2LobbyMessageDefine.Add(typeof(Msg_LC_SyncRoleInfo), (int)LobbyMessageDefine.Msg_LC_SyncRoleInfo);
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

