package(GameFrameworkMessage);

enum(TestEnum, lobby)
{
  ONE(1);
  TWO;
};

message(TestData, lobby)
{
	enumvalue("NODE_START+1");
  member(Name,string,optional);  
  member(Value,int,optional){
    default(1);
  };
  member(Other,TestEnum,required);
  member(Other1,TestEnum,required);
  member(Other2,TestEnum,required);
};

message(UserInfo, client)
{
  member(Guid,int,required);
  member(Nick,int,required);
  member(Key,int,required);
};
