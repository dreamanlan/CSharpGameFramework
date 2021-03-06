package(GameFrameworkMessage);

//=============================================
//GM消息
enum(GmResultEnum)
{
  Success(1);		//操作成功
  Failed;		//操作失败
};
enum(GmStateEnum)
{
  Online(1);	//在线
  Offline;	//离线
  Banned;	//封停
};
message(GmUserBasic)
{
  member(m_HeroId, int32, required);
  member(m_Level, int32, required);
  member(m_Vip, int32, required);
  member(m_Money, int32, required);
  member(m_Gold, int32, required);
  member(m_CreateTime, string, required);
  member(m_LastLogoutTime, string, required);
  member(m_GoldCash, int32, required);
};
message(GmUserInfo) 
{
  member(m_Guid, uint64, required);
  member(m_AccountId, string, required);
  member(m_Nickname, string, required);
  member(m_UserState, GmStateEnum, required);
  member(m_UserBasic, GmUserBasic, optional);
  member(m_UserEquips, ItemDataMsg, repeated);
  member(m_UserBagItems, ItemDataMsg, repeated);
};

message(Msg_CL_GmQueryUserByGuid) {
  member(m_QueryGuid, uint64, required);
};
message(Msg_CL_GmQueryUserByNickname) {
  member(m_QueryNickname, string, required);
};
message(Msg_LC_GmQueryUser) {
  member(m_Result, GmResultEnum, required);
  member(m_Info, GmUserInfo, optional);
};
message(Msg_CL_GmQueryUserByFuzzyNickname) {
  member(m_QueryNickname, string, required);
};
message(Msg_LC_GmQueryUserByFuzzyNickname) {
  member(m_Result, GmResultEnum, required);
  member(m_QueryNickname, string, required);
  member(m_Infos, GmUserInfo, repeated);
};
message(Msg_CL_GmQueryAccount) {
  member(m_QueryAccount, string, required);
};
message(Msg_LC_GmQueryAccount) {
  member(m_Result, GmResultEnum, required);
  member(m_QueryAccount, string, required);
  member(m_AccountState, GmStateEnum, optional);
  member(m_Infos, GmUserInfo, repeated);
};
message(Msg_CL_GmKickUser) {
  member(m_Nickname, string, required);
  member(m_LockMinutes, int32, required);
};
message(Msg_LC_GmKickUser) {
  member(m_Result, GmResultEnum, required);
  member(m_Nickname, string, required);
  member(m_KickedGuid, int64, optional);
  member(m_KickedAccountId, string, optional);
};
message(Msg_CL_GmLockUser) {
  member(m_AccountId, string, required);
};
message(Msg_LC_GmLockUser) {
  member(m_Result, GmResultEnum, required);
  member(m_AccountId, string, required);
};
message(Msg_CL_GmUnlockUser) {
  member(m_AccountId, string, required);
};
message(Msg_LC_GmUnlockUser) {
  member(m_Result, GmResultEnum, required);
  member(m_AccountId, string, required);
};
message(Msg_CL_GmAddExp) {
  member(m_Nick, string, required);
  member(m_Exp, int32, required);
};
message(Msg_LC_GmAddExp) {
  member(m_Result, int32, required);
};
message(Msg_CL_GmUpdateMaxUserCount) {
  member(m_MaxUserCount, int32, required);
  member(m_MaxQueueingCount, int32, required);
};
message(Msg_CL_PublishNotice) {
  member(m_Content, string, required);
  member(m_RollNum, int32, required);
};
message(Msg_LC_PublishNotice) {
  member(m_Result, GmResultEnum, required);
};
message(Msg_CL_SendMail) {
  member(m_Receiver, string, required);
  member(m_Title, string, required);
  member(m_Text, string, required);
  member(m_ExpiryDate, int32, required);
  member(m_LevelDemand, int32, required);
  member(m_ItemId, int32, required);
  member(m_ItemNum, int32, required);
  member(m_Money, int32, required);
  member(m_Gold, int32, required);
  member(m_Stamina, int32, required);
};
message(Msg_LC_SendMail) {
  member(m_Result, GmResultEnum, required);
};
message(Msg_CL_GmHomeNotice) {
  member(m_Content, string, required);
};
message(Msg_LC_GmHomeNotice) {
  member(m_Result, GmResultEnum, required);
};
message(Msg_CL_GmForbidChat) {
  member(m_Nickname, string, required);
  member(m_IsForbid, bool, required);
};
message(Msg_LC_GmForbidChat) {
  member(m_Result, GmResultEnum, required);
  member(m_Nickname, string, required);
};
message(Msg_CL_GmCode) {
  member(m_Nick, string, required);
  member(m_Content, string, required);
};

message(Msg_CL_GmGeneralOperation)
{
  member(m_OperationType, int32, required);
  member(m_Params, string, repeated);
};
message(Msg_LC_GmGeneralOperation)
{
  member(m_Result, GmResultEnum, required);
  member(m_OperationType, int32, required);
  member(m_Params, string, repeated);
};
//=================================================
message(ItemDataMsg) {
  member(Guid, uint64, optional);
  member(ItemId, int32, required);
  member(Level, int32, optional);
  member(Experience, int32, optional);
  member(Num, int32, optional);
  member(AppendProperty, int32, optional);
  member(EnhanceStarLevel, int32, optional);
  member(StrengthLevel, int32, optional);
  member(StrengthFailCount, int32, optional);
  member(IsCanTrade, bool, optional);
};
//=================================================
