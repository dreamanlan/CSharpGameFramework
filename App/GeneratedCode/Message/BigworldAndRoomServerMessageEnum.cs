//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/BigworldAndRoomServer.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFrameworkMessage
{
	public enum BigworldAndRoomServerMessageEnum
	{
		Msg_RL_RegisterRoomServer = 1,
		Msg_LR_ReplyRegisterRoomServer,
		Msg_LR_RoomUserInfo,
		Msg_LR_ReconnectUser,
		Msg_RL_ReplyReconnectUser,
		Msg_RL_RoomServerUpdateInfo,
		Msg_RL_UserLobbyItemInfo,
		Msg_RL_PickMoney,
		Msg_RL_PickItem,
		Msg_LR_ReclaimItem,
		Msg_RL_UserDrop,
		Msg_LR_UserQuit,
		Msg_RL_UserQuit,
		Msg_LR_UserReLive,
		Msg_LR_EnterScene,
		Msg_RL_EnterSceneResult,
		Msg_LR_ChangeScene,
		Msg_RL_ChangeSceneResult,
		Msg_RL_ChangeScene,
		Msg_LRL_StoryMessage,
		Msg_LBL_Message,
		Msg_LB_UpdateUserServerInfo,
		Msg_LB_QueryUserState,
		Msg_BL_QueryUserStateResult,
		Msg_BL_BroadcastText,
		Msg_LB_UserOffline,
		Msg_BL_UserOffline,
		Msg_LB_UserRelogin,
		Msg_LB_CancelMatch,
		Msg_LB_BigworldUserBaseInfo,
		Msg_LB_RequestEnterScene,
		MaxNum
	}

	public static class BigworldAndRoomServerMessageEnum2Type
	{
		public static Type Query(int id)
		{
			Type t;
			s_BigworldAndRoomServerMessageEnum2Type.TryGetValue(id, out t);
			return t;
		}

		public static int Query(Type t)
		{
			int id;
			s_Type2BigworldAndRoomServerMessageEnum.TryGetValue(t, out id);
			return id;
		}

		static BigworldAndRoomServerMessageEnum2Type()
		{
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_BL_BroadcastText, typeof(Msg_BL_BroadcastText));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_BL_BroadcastText), (int)BigworldAndRoomServerMessageEnum.Msg_BL_BroadcastText);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_BL_QueryUserStateResult, typeof(Msg_BL_QueryUserStateResult));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_BL_QueryUserStateResult), (int)BigworldAndRoomServerMessageEnum.Msg_BL_QueryUserStateResult);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_BL_UserOffline, typeof(Msg_BL_UserOffline));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_BL_UserOffline), (int)BigworldAndRoomServerMessageEnum.Msg_BL_UserOffline);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LB_BigworldUserBaseInfo, typeof(Msg_LB_BigworldUserBaseInfo));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LB_BigworldUserBaseInfo), (int)BigworldAndRoomServerMessageEnum.Msg_LB_BigworldUserBaseInfo);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LB_CancelMatch, typeof(Msg_LB_CancelMatch));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LB_CancelMatch), (int)BigworldAndRoomServerMessageEnum.Msg_LB_CancelMatch);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LB_QueryUserState, typeof(Msg_LB_QueryUserState));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LB_QueryUserState), (int)BigworldAndRoomServerMessageEnum.Msg_LB_QueryUserState);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LB_RequestEnterScene, typeof(Msg_LB_RequestEnterScene));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LB_RequestEnterScene), (int)BigworldAndRoomServerMessageEnum.Msg_LB_RequestEnterScene);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LB_UpdateUserServerInfo, typeof(Msg_LB_UpdateUserServerInfo));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LB_UpdateUserServerInfo), (int)BigworldAndRoomServerMessageEnum.Msg_LB_UpdateUserServerInfo);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LB_UserOffline, typeof(Msg_LB_UserOffline));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LB_UserOffline), (int)BigworldAndRoomServerMessageEnum.Msg_LB_UserOffline);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LB_UserRelogin, typeof(Msg_LB_UserRelogin));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LB_UserRelogin), (int)BigworldAndRoomServerMessageEnum.Msg_LB_UserRelogin);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LBL_Message, typeof(Msg_LBL_Message));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LBL_Message), (int)BigworldAndRoomServerMessageEnum.Msg_LBL_Message);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LR_ChangeScene, typeof(Msg_LR_ChangeScene));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LR_ChangeScene), (int)BigworldAndRoomServerMessageEnum.Msg_LR_ChangeScene);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LR_EnterScene, typeof(Msg_LR_EnterScene));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LR_EnterScene), (int)BigworldAndRoomServerMessageEnum.Msg_LR_EnterScene);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LR_ReclaimItem, typeof(Msg_LR_ReclaimItem));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LR_ReclaimItem), (int)BigworldAndRoomServerMessageEnum.Msg_LR_ReclaimItem);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LR_ReconnectUser, typeof(Msg_LR_ReconnectUser));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LR_ReconnectUser), (int)BigworldAndRoomServerMessageEnum.Msg_LR_ReconnectUser);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LR_ReplyRegisterRoomServer, typeof(Msg_LR_ReplyRegisterRoomServer));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LR_ReplyRegisterRoomServer), (int)BigworldAndRoomServerMessageEnum.Msg_LR_ReplyRegisterRoomServer);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LR_RoomUserInfo, typeof(Msg_LR_RoomUserInfo));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LR_RoomUserInfo), (int)BigworldAndRoomServerMessageEnum.Msg_LR_RoomUserInfo);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LR_UserQuit, typeof(Msg_LR_UserQuit));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LR_UserQuit), (int)BigworldAndRoomServerMessageEnum.Msg_LR_UserQuit);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LR_UserReLive, typeof(Msg_LR_UserReLive));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LR_UserReLive), (int)BigworldAndRoomServerMessageEnum.Msg_LR_UserReLive);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_LRL_StoryMessage, typeof(Msg_LRL_StoryMessage));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_LRL_StoryMessage), (int)BigworldAndRoomServerMessageEnum.Msg_LRL_StoryMessage);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_ChangeScene, typeof(Msg_RL_ChangeScene));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_ChangeScene), (int)BigworldAndRoomServerMessageEnum.Msg_RL_ChangeScene);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_ChangeSceneResult, typeof(Msg_RL_ChangeSceneResult));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_ChangeSceneResult), (int)BigworldAndRoomServerMessageEnum.Msg_RL_ChangeSceneResult);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_EnterSceneResult, typeof(Msg_RL_EnterSceneResult));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_EnterSceneResult), (int)BigworldAndRoomServerMessageEnum.Msg_RL_EnterSceneResult);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_PickItem, typeof(Msg_RL_PickItem));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_PickItem), (int)BigworldAndRoomServerMessageEnum.Msg_RL_PickItem);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_PickMoney, typeof(Msg_RL_PickMoney));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_PickMoney), (int)BigworldAndRoomServerMessageEnum.Msg_RL_PickMoney);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_RegisterRoomServer, typeof(Msg_RL_RegisterRoomServer));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_RegisterRoomServer), (int)BigworldAndRoomServerMessageEnum.Msg_RL_RegisterRoomServer);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_ReplyReconnectUser, typeof(Msg_RL_ReplyReconnectUser));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_ReplyReconnectUser), (int)BigworldAndRoomServerMessageEnum.Msg_RL_ReplyReconnectUser);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_RoomServerUpdateInfo, typeof(Msg_RL_RoomServerUpdateInfo));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_RoomServerUpdateInfo), (int)BigworldAndRoomServerMessageEnum.Msg_RL_RoomServerUpdateInfo);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_UserDrop, typeof(Msg_RL_UserDrop));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_UserDrop), (int)BigworldAndRoomServerMessageEnum.Msg_RL_UserDrop);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_UserLobbyItemInfo, typeof(Msg_RL_UserLobbyItemInfo));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_UserLobbyItemInfo), (int)BigworldAndRoomServerMessageEnum.Msg_RL_UserLobbyItemInfo);
			s_BigworldAndRoomServerMessageEnum2Type.Add((int)BigworldAndRoomServerMessageEnum.Msg_RL_UserQuit, typeof(Msg_RL_UserQuit));
			s_Type2BigworldAndRoomServerMessageEnum.Add(typeof(Msg_RL_UserQuit), (int)BigworldAndRoomServerMessageEnum.Msg_RL_UserQuit);
		}

		private static Dictionary<int, Type> s_BigworldAndRoomServerMessageEnum2Type = new Dictionary<int, Type>();
		private static Dictionary<Type, int> s_Type2BigworldAndRoomServerMessageEnum = new Dictionary<Type, int>();
	}
}

