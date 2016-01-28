package(GameFrameworkMessage);

message(LB_VerifyAccount)
{
  member(MsgCode, int32, required);
  member(Account, string, required);
  member(OpCode, int32, required);
  member(ChannelId, int32, required);
  member(Data, string, required);
};

message(BL_VerifyAccountResult)
{
  member(MsgCode, int32, required);
  member(Account, string, required);
  member(Result, bool, required);
  member(AccountId, string, required);
  member(Oid, string, required);
  member(OpCode, int32, required);
  member(ChannelId, int32, required);
  member(Token, string, required);
};

message(LB_VerifyCode)
{
  member(MsgCode, int32, required);
  member(AccountId, string, required);
  member(UserGuid, uint64, required);
  member(OpCode, int32, required);
  member(ChannelId, int32, required);
  member(Code, string, required);
};

message(BL_VerifyCodeResult)
{
  member(MsgCode, int32, required);
  member(AccountId, string, required);
  member(UserGuid, uint64, required);
  member(Code, string, required);
  member(Result, int32, required);
  member(Content, string, required);
};

message(LB_UseCode)
{
  member(MsgCode, int32, required);
  member(AccountId, string, required);
  member(UserGuid, uint64, required);
  member(OpCode, int32, required);
  member(ChannelId, int32, required);
  member(Code, string, required);
};

message(BL_UseCodeResult)
{
  member(MsgCode, int32, required);
  member(AccountId, string, required);
  member(UserGuid, uint64, required);
  member(Code, string, required);
  member(Result, int32, required);
};

message(BL_PayOrder)
{
  member(OrderId, string, required);
  member(GoodsRegisterId, string, required);
  member(GoodsNum, int32, required);
  member(GoodsPrice, float, required);
  member(Guid, uint64, required);
  member(ChannelId, string, required);
};

message(LB_PayOrderResult)
{
  member(OrderId, string, required);
  member(ChannelId, string, required);
};
