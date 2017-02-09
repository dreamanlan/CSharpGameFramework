explanationfile("Data.version.notes");
///////////////////////////////////////////////////////////////////////////////
// data definitions
version("0.0.1");
///////////////////////////////////////////////////////////////////////////////
package(GameFrameworkData);

typeconverter(DateTime)
{
	messagetype(string);
	message2logic
	{:m_{1} = DateTime.ParseExact(m_{0}.{1},"yyyyMMddHHmmss",null);:};
	logic2message
	{:m_{0}.{1} = m_{1}.ToString("yyyyMMddHHmmss");:};
};

typeconverter("List<int>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitGeneralList<int>(new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinGeneralList(",",m_{1});:};
	crudcode
	{:
public int Get{0}Count()
{{
  return m_{0}.Count;
}}
public void Set{0}(int ix, int val)
{{
  m_{0}[ix]=val;
  {1};
}}
public int Get{0}(int ix)
{{
  return m_{0}[ix];
}}
public void Add{0}(int val)
{{
  m_{0}.Add(val);
  {1};
}}
public void Del{0}(int ix)
{{
  m_{0}.RemoveAt(ix);
  {1};
}}
public void Visit{0}(MyAction<int> visit)
{{
  foreach(int v in m_{0}){{
    visit(v);
  }}
}}
public int[] {0}ToArray()
{{
  return m_{0}.ToArray();
}}:};
};

typeconverter("List<DateTime>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitDateTimeList<int>(new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinDateTimeList(",",m_{1});:};
	crudcode
	{:
public int Get{0}Count()
{{
  return m_{0}.Count;
}}
public void Set{0}(int ix, DateTime val)
{{
  m_{0}[ix]=val;
  {1};
}}
public DateTime Get{0}(int ix)
{{
  return m_{0}[ix];
}}
public void Add{0}(DateTime val)
{{
  m_{0}.Add(val);
  {1};
}}
public void Del{0}(int ix)
{{
  m_{0}.RemoveAt(ix);
  {1};
}}
public void Visit{0}(MyAction<DateTime> visit)
{{
  foreach(DateTime v in m_{0}){{
    visit(v);
  }}
}}
public DateTime[] {0}ToArray()
{{
  return m_{0}.ToArray();
}}:};
};

typeconverter("Dictionary<DateTime,DateTime>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitDateTimeDictionary(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinDateTimeDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<DateTime,int>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitDateTimeKeyDictionary<int>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinDateTimeKeyDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<int,DateTime>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitDateTimeValueDictionary<int>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinDateTimeValueDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<int,string>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitGeneralDictionary<int,string>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinGeneralDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<string,int>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitGeneralDictionary<string,int>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinGeneralDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<string,float>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitGeneralDictionary<string,float>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinGeneralDictionary(";",",",m_{1});:};
};

typeconverter("Dictionary<string,string>")
{
	messagetype(string);
	message2logic
	{:m_{1} = DataProtoUtility.SplitGeneralDictionary<string,string>(new char[]{{';'}},new char[]{{','}}, m_{0}.{1});:};
	logic2message
	{:m_{0}.{1} = DataProtoUtility.JoinGeneralDictionary(";",",",m_{1});:};
};

message(GeneralRecordData)
{
	option(dontgenenum);
	option(dontgendb);
  member(PrimaryKeys, string, repeated);
  member(ForeignKeys, string, repeated);
  member(DataVersion, int32, required);
  member(Data, bytes, optional);
};

message(TableGuid) 
{
	member(GuidType, string, required){
		maxsize(24);
		primarykey;
	};
	member(GuidValue, ulong, required);
};

message(TableNicknameInfo)
{
	wrapname(NicknameInfo);
	member(Nickname, string, required){
		maxsize(32);
		primarykey;
	};
	member(UserGuid, ulong, required);   
};

message(TableMailInfo)
{
	member(Guid, ulong, required){
		primarykey;
	};
	member(Sender, string, required){
		maxsize(32);
	};
	member(Receiver, long, required);  
	member(SendDate, DateTime, required){
		maxsize(24);
	};  
	member(ExpiryDate, DateTime, required){
		maxsize(24);
	};   
	member(Title, string, required){
		maxsize(64);
	};  
	member(Text, string, required){
		maxsize(1024);
	};
	member(Money, int, required);  
	member(Gold, int, required);
	member(ItemIds, "List<int>", required);  
	member(ItemNumbers, "List<int>", required);  
	member(LevelDemand, int, required);
	member(IsRead, bool, required);
};

message(TableActivationCode)
{
	member(ActivationCode, string, required){
		maxsize(32);
		primarykey;
	};
	member(IsActivated, bool, required);
	member(AccountId, string, required){
		maxsize(64);
	};
};

message(TableGlobalData)
{
	member(Key, string, required){
		maxsize(32);
		primarykey;
	};
	member(IntValue, int, required);
	member(FloatValue, float, required);
	member(StrValue, string, required){
		maxsize(1024);
	};
};

//////////////////////////////////////////////////////////////////////
message(TableAccount) 
{
	member(AccountId, string, required){
		maxsize(64);
		primarykey;
	};	
	member(Password, string, required){
	  maxsize(256);
	};
	member(IsBanned, bool, required); 
	member(UserGuid, ulong, required);
};

message(TableUserInfo) 
{
  wrapname(UserInfo);
	member(Guid, ulong, required){
		primarykey;
	};
	member(AccountId, string, required){
		maxsize(64);
		foreignkey;
	};  
	member(Nickname, string, required){
		maxsize(32);
	};
	member(HeroId, int, required);	
	member(CreateTime, DateTime, required){
		maxsize(24);
	};
	member(LastLogoutTime, DateTime, required){
		maxsize(24);
	};
	member(Level, int, required);
	member(ExpPoints, int, required);
	member(SceneId, int, required);	
	member(PositionX, float, required);
	member(PositionZ, float, required);
	member(FaceDir, float, required);
	member(Money, int, required);
	member(Gold, int, required);
	member(SummonerSkillId, int, required);
	member(IntDatas, "Dictionary<string,int>", required);
	member(FloatDatas, "Dictionary<string,float>", required);
	member(StringDatas, "Dictionary<string,string>", required);
};

message(TableMemberInfo)
{
  wrapname(MemberInfo);
	member(MemberGuid, ulong, required){
		primarykey;
	};
	member(UserGuid, ulong, required){
	  foreignkey;
	};
	member(HeroId, int, required);
	member(Level, int, required);
};

message(TableItemInfo)
{
  wrapname(ItemInfo);
	member(ItemGuid, ulong, required){
		primarykey;
	};	
	member(UserGuid, ulong, required){
	  foreignkey;
	};	
	member(ItemId, int, required);
	member(ItemNum, int, required);
};

message(TableMailStateInfo)
{
  wrapname(MailState);
	member(MailGuid, ulong, required){
		primarykey;
	};
	member(UserGuid, ulong, required){
	  foreignkey;
	};
	member(IsRead, bool, required);
	member(IsReceived, bool, required);
	member(IsDeleted, bool, required);
	member(ExpiryDate, DateTime, required){
		maxsize(24);
	};
};

message(TableFriendInfo)
{
  wrapname(FriendInfo);
	member(Guid, ulong, required){
		primarykey;
	};
	member(UserGuid, ulong, required){
	  foreignkey;
	};
	member(FriendGuid, ulong, required);
	member(FriendNickname, string, required){
		maxsize(32);
	};
	member(QQ, long, required);
	member(weixin, long, required);
	member(IsBlack, bool, required);
};
