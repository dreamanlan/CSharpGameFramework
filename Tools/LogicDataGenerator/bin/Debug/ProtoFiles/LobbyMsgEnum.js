//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/LobbyMsg.dsl生成！！！
//----------------------------------------------------------------------------
var LobbyMessageDefine = {
	NodeRegister : 1,
	NodeRegisterResult : 2,
	VersionVerify : 3,
	VersionVerifyResult : 4,
	DirectLogin : 5,
	AccountLogout : 6,
	Logout : 7,
	GetQueueingCount : 8,
	UserHeartbeat : 9,
	KickUser : 10,
	TooManyOperations : 11,
	RequestNickname : 12,
	RequestSceneRoomInfo : 13,
	RequestSceneRoomList : 14,
	ServerShutdown : 15,
	GmCode : 16,
	AccountLogin : 17,
	AccountLoginResult : 18,
	QueueingCountResult : 19,
	RequestNicknameResult : 20,
	ActivateAccount : 21,
	ActivateAccountResult : 22,
	ChangeName : 23,
	ChangeNameResult : 24,
	RoleEnter : 25,
	RoleEnterResult : 26,
	EnterScene : 27,
	ChangeSceneRoom : 28,
	EnterSceneResult : 29,
	QuitRoom : 30,
	Msg_CLC_StoryMessage : 31,
	Msg_LC_PublishEvent : 32,
	Msg_LC_SendGfxMessage : 33,
	Msg_LC_HighlightPrompt : 34,
};

exports.GameFrameworkMessage = {

	LobbyMessageDefine : LobbyMessageDefine,

};
