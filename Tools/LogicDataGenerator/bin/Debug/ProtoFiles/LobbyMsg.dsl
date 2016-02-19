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

message(ActivateAccount)
{
  member(m_ActivationCode, string, required);
};

message(ActivateAccountResult)
{
	enum(ActivateAccountResultEnum)
	{
	  Success(0);    	//激活成功
	  InvalidCode;    //失效的激活码（该激活码已经被使用）
	  MistakenCode;   //错误的激活码（该激活码不存在）
	  Error;          //激活失败(系统问题)
	};
  member(m_Result, ActivateAccountResultEnum, required);
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

message(RoleEnterResult)
{	
	enum(RoleEnterResultEnum)
	{
	  Success(0);
	  Wait;
	  UnknownError;
	};
  message(MemberInfo)
  {
    member(Hero, int32, required);
    member(Level, int32, required);
  };
  member(m_Result, RoleEnterResultEnum, required);
  member(m_Nickname, string, required);
  member(m_HeroId, int, required);
	member(m_Money, int, optional);
  member(m_Gold, int, optional);
	member(m_Level, int, optional);
	member(m_SceneId, int, optional);
  member(m_WorldId, int, optional);
  member(Members, MemberInfo, repeated);
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
  member(server_ip, string, optional);
  member(server_port, uint, optional);
  member(key, uint, optional);
  member(camp_id, int, optional);
  member(scene_type, int, optional);
  member(result, int, required);
  member(prime, int, required);
};

message(QuitRoom) {
	member(m_IsQuitRoom, bool, required);
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