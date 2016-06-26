package(GameFrameworkMessage);

//=================================================
//node消息头
message(NodeMessageWithAccount)
{
  option(dontgenenum);
  member(m_Account, string, required);
};
message(NodeMessageWithGuid)
{
  option(dontgenenum);
  member(m_Guid, ulong, required);
};
message(NodeMessageWithAccountAndGuid)
{
  option(dontgenenum);
  member(m_Account, string, required);
  member(m_Guid, ulong, required);  
};
message(NodeRegister)
{
  member(m_Name, string, required);
};
message(NodeRegisterResult)
{
  member(m_IsOk, bool, required);
};


//=============================================
enum(LobbyArgType) {
  NULL;
  INT;
  FLOAT;
  STRING;
};

//=============================================
//仅由node处理的消息
message(VersionVerify){};
message(VersionVerifyResult)
{
	member(m_Result, int, required);
	member(m_EnableLog, int, required);
	member(m_ShopMask, uint, required);
};

//=================================================
message(DirectLogin){};
message(AccountLogout){};
message(Logout){};
message(GetQueueingCount){};
message(UserHeartbeat){};
message(KickUser){};
message(TooManyOperations){};
message(RequestNickname){};
message(RequestSceneRoomInfo){};
message(RequestSceneRoomList){};
message(ServerShutdown){};
message(Msg_CL_GetMailList){};
message(Msg_LC_NotifyNewMail){};

//=================================================
message(GmCode) {
  member(m_Content, string, required);
};

//=================================================

message(AccountLogin)
{  
  member(m_AccountId, string, required);
  member(m_Password, string, required);
  member(m_ClientInfo, string, required);
};

message(AccountLoginResult)
{	
	enum(AccountLoginResultEnum)
	{
	  Success(0);    	//登录成功
	  FirstLogin;     //账号首次登录
	  Error;          //登录失败
	  Wait;          	//登录人太多，等待
	  Banned;         //账号已被封停    
	  AlreadyOnline;  //账号在别处已登录
	  Queueing;       //排队
	  QueueFull;      //排队满
	  Failed;         //账号验证失败
	  Nonactivated;   //账号未激活
	};
  member(m_AccountId, string, required);
  member(m_Result, AccountLoginResultEnum, required);
	member(m_UserGuid, ulong, required);
};

message(QueueingCountResult) {
  member(m_QueueingCount, int, required);
};

message(RequestNicknameResult)
{
  member(m_Nicknames, string, repeated);
};

message(ChangeName)
{
  member(m_Nickname, string, required);
};

message(ChangeNameResult)
{
	enum(ChangeNameResultEnum)
	{
	  Success(0);
	  NicknameError;
	  UnknownError;
	};
  member(m_Result, ChangeNameResultEnum, required);
  member(m_Nickname, string, required);
};

message(RoleEnter) {
  member(m_Nickname, string, optional);
};

message(FriendInfoForMessage)
{
	member(FriendGuid, ulong, required);
	member(FriendNickname, string, required);
	member(QQ, long, required);
	member(weixin, long, required);
	member(IsBlack, bool, required);
};
message(MemberInfoForMessage)
{
  member(Hero, int32, required);
  member(Level, int32, required);
};
message(ItemInfoForMessage)
{
	member(ItemGuid, ulong, required);
	member(ItemId, int, required);
	member(ItemNum, int, required);
};
message(RoleEnterResult)
{	
	enum(RoleEnterResultEnum)
	{
	  Success(0);
	  Wait;
	  Reconnect;
	  UnknownError;
	};
  member(Result, RoleEnterResultEnum, required);
  member(Nickname, string, required);
  member(HeroId, int, required);
	member(Money, int, required);
  member(Gold, int, required);
	member(Level, int, required);
	member(SceneId, int, required);
  member(WorldId, int, required);
  member(Members, MemberInfoForMessage, repeated);
  member(Items, ItemInfoForMessage, repeated);
  member(Friends, FriendInfoForMessage, repeated);
  member(SummonerSkillId, int32, required);
};

message(EnterScene)
{
	member(m_SceneId, int, required);
	member(m_RoomId, int, optional);
};

message(ChangeSceneRoom)
{
	member(m_SceneId, int, optional);
	member(m_RoomId, int, optional);
};

message(EnterSceneResult) {
  member(result, int, required);
  member(server_ip, string, optional);
  member(server_port, uint, optional);
  member(key, uint, optional);
  member(camp_id, int, optional);
  member(scene_type, int, optional);
  member(prime, int, optional);
};

message(QuitRoom) {
	member(m_IsQuitRoom, bool, required);
};

message(MailItemForMessage)
{
  member(m_ItemId, int32, required);
  member(m_ItemNum, int32, required);
};
message(MailInfoForMessage)
{
  member(m_AlreadyRead, bool, required);
  member(m_MailGuid, uint64, required);
  member(m_Module, int32, required);
  member(m_Title, string, required);
  member(m_Sender, string, required);
  member(m_SendTime, string, required);
  member(m_Text, string, required);
  member(m_Items, MailItemForMessage, repeated);
  member(m_Money, int32, required);
  member(m_Gold, int32, required);
};
message(Msg_LC_SyncMailList)
{
  member(m_Mails, MailInfoForMessage, repeated);
};
message(Msg_CL_ReadMail) {
  member(m_MailGuid, uint64, required);
};
message(Msg_CL_ReceiveMail) {
	member(m_MailGuid, uint64, required);
};
message(Msg_CL_DeleteMail) {
  member(m_MailGuid, uint64, required);
};
message(Msg_LC_LackOfSpace)
{
	member(m_Succeed, bool, required);
	member(m_ReceiveNum, int32, required);
	member(m_FreeNum, int32, required);
	member(m_MailGuid, uint64, required);
};

message(Msg_LC_SyncFriendList)
{
  member(m_Friends, FriendInfoForMessage, repeated);
};
message(Msg_CL_AddFriend) {
  member(m_FriendNickname, string, required);
};
message(Msg_LC_AddFriend) {
  member(m_FriendInfo, FriendInfoForMessage, required);
};
message(Msg_CL_RemoveFriend) {
	member(m_FriendGuid, uint64, required);
};
message(Msg_LC_RemoveFriend) {
	member(m_FriendGuid, uint64, required);
};
message(Msg_CL_MarkBlack) {
	member(m_FriendGuid, uint64, required);
};
message(Msg_LC_MarkBlack) {
	member(m_FriendGuid, uint64, required);
};

message(Msg_LC_SyncRoleInfo)
{
  member(HeroId, int, required);
	member(Money, int, required);
  member(Gold, int, required);
	member(Level, int, required);
  member(SummonerSkillId, int32, required);
};

message(Msg_LC_SyncMemberList)
{
	member(m_Members, MemberInfoForMessage, repeated);
};

message(Msg_LC_SyncItemList)
{
	member(m_Items, ItemInfoForMessage, repeated);
};
message(Msg_CL_UseItem) {
	member(ItemId, int, required);
	member(ItemNum, int, required);
};
message(Msg_CL_DiscardItem) {
	member(ItemGuid, ulong, required);
};

message(Msg_CLC_StoryMessage)
{
  message(MessageArg) {
    member(val_type, LobbyArgType, required);
    member(str_val, string, required);
  };
  member(m_MsgId, string, required);
  member(m_Args, MessageArg, repeated);
};

message(Msg_LC_PublishEvent) {
  message(EventArg) {
    member(val_type, LobbyArgType, required);
    member(str_val, string, required);
  };
  member(is_logic_event, bool, required);
  member(ev_name, string, required);
  member(group, string, required);
  member(args, EventArg, repeated);
};

message(Msg_LC_SendGfxMessage) {
  message(EventArg) {
    member(val_type, LobbyArgType, required);
    member(str_val, string, required);
  };
  member(is_with_tag, bool, required);
  member(name, string, required);
  member(msg, string, required);
  member(args, EventArg, repeated);
};

message(Msg_LC_HighlightPrompt) {
	member(dict_id, string, required);
	member(argument, string, repeated);
};