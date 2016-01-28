package(GameFrameworkMessage);

message(Msg_LD_Connect)
{
  member(ClientName,string,required);
};

message(Msg_DL_Connect)
{
  member(Result,bool,required);
  member(Error,string,optional);
};

message(Msg_LD_SingleLoadRequest)
{
	option(dontgenenum);
	enum(LoadTypeEnum){
	  LoadAll;
	  LoadSingle;
	  LoadMulti;
	};
  member(MsgId, int32, required);
  member(LoadType, LoadTypeEnum, required);
  member(Keys, string, repeated);
  member(Start, int32, optional);
  member(Count, int32, optional) {
	default(-1);
  };
};

message(Msg_DL_SingleRowResult)
{
	option(dontgenenum);
  member(MsgId, int32, required);
  member(PrimaryKeys, string, repeated);
  member(ForeignKeys, string, repeated);
  member(DataVersion, int32, required);
  member(Data, bytes, optional);
};

message(Msg_LD_Load)
{
  member(MsgId, int32, required);
  member(PrimaryKeys, string, repeated);
  member(SerialNo, int64, required);
  member(LoadRequests, Msg_LD_SingleLoadRequest, repeated);
};

message(Msg_DL_LoadResult)
{
  enum(ErrorNoEnum){
    Exception(-1);
    Success(0);
    NotFound;
    PrepError;
    PostError;
    TimeoutError;
    Undone;
  };
  member(MsgId, int32, required);
  member(PrimaryKeys, string, repeated);
  member(SerialNo, int64, required);
  member(ErrorNo, ErrorNoEnum, required);
  member(ErrorInfo, string, optional);
  member(Results, Msg_DL_SingleRowResult, repeated);
};

message(Msg_LD_Save)
{
  member(MsgId, int32, required);
  member(PrimaryKeys, string, repeated);
  member(ForeignKeys, string, repeated);
  member(SerialNo, int64, required);
  member(Data, bytes, optional);
};

message(Msg_DL_SaveResult)
{
  enum(ErrorNoEnum){
    Exception(-1);
    Success(0);
    NotFound;
    PrepError;
    PostError;
    TimeoutError;
  };
  member(MsgId, int32, required);
  member(PrimaryKeys, string, repeated);
  member(SerialNo, int64, required);
  member(ErrorNo, ErrorNoEnum, required);
  member(ErrorInfo, string, optional);
};