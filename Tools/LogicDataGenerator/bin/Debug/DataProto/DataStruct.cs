//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: DataProto/Data.proto
namespace GameFrameworkData
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GeneralRecordData")]
  public partial class GeneralRecordData : global::ProtoBuf.IExtensible
  {
    public GeneralRecordData() {}
    
    private readonly global::System.Collections.Generic.List<string> _PrimaryKeys = new global::System.Collections.Generic.List<string>();
    [global::ProtoBuf.ProtoMember(1, Name=@"PrimaryKeys", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<string> PrimaryKeys
    {
      get { return _PrimaryKeys; }
    }
  
    private readonly global::System.Collections.Generic.List<string> _ForeignKeys = new global::System.Collections.Generic.List<string>();
    [global::ProtoBuf.ProtoMember(2, Name=@"ForeignKeys", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<string> ForeignKeys
    {
      get { return _ForeignKeys; }
    }
  
    private int _DataVersion;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"DataVersion", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int DataVersion
    {
      get { return _DataVersion; }
      set { _DataVersion = value; }
    }
    private byte[] _Data = null;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"Data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public byte[] Data
    {
      get { return _Data; }
      set { _Data = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableAccount")]
  public partial class TableAccount : global::ProtoBuf.IExtensible
  {
    public TableAccount() {}
    
    private string _AccountId;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"AccountId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string AccountId
    {
      get { return _AccountId; }
      set { _AccountId = value; }
    }
    private bool _IsBanned;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"IsBanned", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool IsBanned
    {
      get { return _IsBanned; }
      set { _IsBanned = value; }
    }
    private ulong _UserGuid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"UserGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong UserGuid
    {
      get { return _UserGuid; }
      set { _UserGuid = value; }
    }
    private bool _IsValid;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"IsValid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool IsValid
    {
      get { return _IsValid; }
      set { _IsValid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableActivationCode")]
  public partial class TableActivationCode : global::ProtoBuf.IExtensible
  {
    public TableActivationCode() {}
    
    private string _ActivationCode;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"ActivationCode", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string ActivationCode
    {
      get { return _ActivationCode; }
      set { _ActivationCode = value; }
    }
    private bool _IsActivated;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"IsActivated", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool IsActivated
    {
      get { return _IsActivated; }
      set { _IsActivated = value; }
    }
    private string _AccountId;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"AccountId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string AccountId
    {
      get { return _AccountId; }
      set { _AccountId = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableFriendInfo")]
  public partial class TableFriendInfo : global::ProtoBuf.IExtensible
  {
    public TableFriendInfo() {}
    
    private ulong _Guid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Guid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong Guid
    {
      get { return _Guid; }
      set { _Guid = value; }
    }
    private ulong _UserGuid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"UserGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong UserGuid
    {
      get { return _UserGuid; }
      set { _UserGuid = value; }
    }
    private ulong _FriendGuid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"FriendGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong FriendGuid
    {
      get { return _FriendGuid; }
      set { _FriendGuid = value; }
    }
    private string _FriendNickname;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"FriendNickname", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string FriendNickname
    {
      get { return _FriendNickname; }
      set { _FriendNickname = value; }
    }
    private long _QQ;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"QQ", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long QQ
    {
      get { return _QQ; }
      set { _QQ = value; }
    }
    private long _weixin;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"weixin", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long weixin
    {
      get { return _weixin; }
      set { _weixin = value; }
    }
    private bool _IsBlack;
    [global::ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"IsBlack", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool IsBlack
    {
      get { return _IsBlack; }
      set { _IsBlack = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableGlobalData")]
  public partial class TableGlobalData : global::ProtoBuf.IExtensible
  {
    public TableGlobalData() {}
    
    private string _Key;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Key", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string Key
    {
      get { return _Key; }
      set { _Key = value; }
    }
    private int _IntValue;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"IntValue", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int IntValue
    {
      get { return _IntValue; }
      set { _IntValue = value; }
    }
    private float _FloatValue;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"FloatValue", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float FloatValue
    {
      get { return _FloatValue; }
      set { _FloatValue = value; }
    }
    private string _StrValue;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"StrValue", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string StrValue
    {
      get { return _StrValue; }
      set { _StrValue = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableGlobalParam")]
  public partial class TableGlobalParam : global::ProtoBuf.IExtensible
  {
    public TableGlobalParam() {}
    
    private string _ParamType;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"ParamType", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string ParamType
    {
      get { return _ParamType; }
      set { _ParamType = value; }
    }
    private string _ParamValue;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"ParamValue", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string ParamValue
    {
      get { return _ParamValue; }
      set { _ParamValue = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableGuid")]
  public partial class TableGuid : global::ProtoBuf.IExtensible
  {
    public TableGuid() {}
    
    private string _GuidType;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"GuidType", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string GuidType
    {
      get { return _GuidType; }
      set { _GuidType = value; }
    }
    private ulong _GuidValue;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"GuidValue", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong GuidValue
    {
      get { return _GuidValue; }
      set { _GuidValue = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableItemInfo")]
  public partial class TableItemInfo : global::ProtoBuf.IExtensible
  {
    public TableItemInfo() {}
    
    private ulong _ItemGuid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"ItemGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong ItemGuid
    {
      get { return _ItemGuid; }
      set { _ItemGuid = value; }
    }
    private ulong _UserGuid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"UserGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong UserGuid
    {
      get { return _UserGuid; }
      set { _UserGuid = value; }
    }
    private int _ItemId;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"ItemId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int ItemId
    {
      get { return _ItemId; }
      set { _ItemId = value; }
    }
    private int _ItemNum;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"ItemNum", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int ItemNum
    {
      get { return _ItemNum; }
      set { _ItemNum = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableMailInfo")]
  public partial class TableMailInfo : global::ProtoBuf.IExtensible
  {
    public TableMailInfo() {}
    
    private ulong _Guid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Guid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong Guid
    {
      get { return _Guid; }
      set { _Guid = value; }
    }
    private string _Sender;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Sender", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string Sender
    {
      get { return _Sender; }
      set { _Sender = value; }
    }
    private long _Receiver;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"Receiver", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long Receiver
    {
      get { return _Receiver; }
      set { _Receiver = value; }
    }
    private string _SendDate;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"SendDate", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string SendDate
    {
      get { return _SendDate; }
      set { _SendDate = value; }
    }
    private string _ExpiryDate;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"ExpiryDate", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string ExpiryDate
    {
      get { return _ExpiryDate; }
      set { _ExpiryDate = value; }
    }
    private string _Title;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"Title", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string Title
    {
      get { return _Title; }
      set { _Title = value; }
    }
    private string _Text;
    [global::ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"Text", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string Text
    {
      get { return _Text; }
      set { _Text = value; }
    }
    private int _Money;
    [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name=@"Money", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Money
    {
      get { return _Money; }
      set { _Money = value; }
    }
    private int _Gold;
    [global::ProtoBuf.ProtoMember(9, IsRequired = true, Name=@"Gold", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Gold
    {
      get { return _Gold; }
      set { _Gold = value; }
    }
    private string _ItemIds;
    [global::ProtoBuf.ProtoMember(10, IsRequired = true, Name=@"ItemIds", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string ItemIds
    {
      get { return _ItemIds; }
      set { _ItemIds = value; }
    }
    private string _ItemNumbers;
    [global::ProtoBuf.ProtoMember(11, IsRequired = true, Name=@"ItemNumbers", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string ItemNumbers
    {
      get { return _ItemNumbers; }
      set { _ItemNumbers = value; }
    }
    private int _LevelDemand;
    [global::ProtoBuf.ProtoMember(12, IsRequired = true, Name=@"LevelDemand", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int LevelDemand
    {
      get { return _LevelDemand; }
      set { _LevelDemand = value; }
    }
    private bool _IsRead;
    [global::ProtoBuf.ProtoMember(13, IsRequired = true, Name=@"IsRead", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool IsRead
    {
      get { return _IsRead; }
      set { _IsRead = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableMailStateInfo")]
  public partial class TableMailStateInfo : global::ProtoBuf.IExtensible
  {
    public TableMailStateInfo() {}
    
    private ulong _MailGuid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"MailGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong MailGuid
    {
      get { return _MailGuid; }
      set { _MailGuid = value; }
    }
    private ulong _UserGuid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"UserGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong UserGuid
    {
      get { return _UserGuid; }
      set { _UserGuid = value; }
    }
    private bool _IsRead;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"IsRead", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool IsRead
    {
      get { return _IsRead; }
      set { _IsRead = value; }
    }
    private bool _IsReceived;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"IsReceived", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool IsReceived
    {
      get { return _IsReceived; }
      set { _IsReceived = value; }
    }
    private bool _IsDeleted;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"IsDeleted", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool IsDeleted
    {
      get { return _IsDeleted; }
      set { _IsDeleted = value; }
    }
    private string _ExpiryDate;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"ExpiryDate", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string ExpiryDate
    {
      get { return _ExpiryDate; }
      set { _ExpiryDate = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableMemberInfo")]
  public partial class TableMemberInfo : global::ProtoBuf.IExtensible
  {
    public TableMemberInfo() {}
    
    private ulong _MemberGuid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"MemberGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong MemberGuid
    {
      get { return _MemberGuid; }
      set { _MemberGuid = value; }
    }
    private ulong _UserGuid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"UserGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong UserGuid
    {
      get { return _UserGuid; }
      set { _UserGuid = value; }
    }
    private int _HeroId;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"HeroId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int HeroId
    {
      get { return _HeroId; }
      set { _HeroId = value; }
    }
    private int _Level;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"Level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Level
    {
      get { return _Level; }
      set { _Level = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableNicknameInfo")]
  public partial class TableNicknameInfo : global::ProtoBuf.IExtensible
  {
    public TableNicknameInfo() {}
    
    private string _Nickname;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Nickname", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string Nickname
    {
      get { return _Nickname; }
      set { _Nickname = value; }
    }
    private ulong _UserGuid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"UserGuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong UserGuid
    {
      get { return _UserGuid; }
      set { _UserGuid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableUserInfo")]
  public partial class TableUserInfo : global::ProtoBuf.IExtensible
  {
    public TableUserInfo() {}
    
    private ulong _Guid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Guid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong Guid
    {
      get { return _Guid; }
      set { _Guid = value; }
    }
    private string _AccountId;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"AccountId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string AccountId
    {
      get { return _AccountId; }
      set { _AccountId = value; }
    }
    private string _Nickname;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"Nickname", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string Nickname
    {
      get { return _Nickname; }
      set { _Nickname = value; }
    }
    private int _HeroId;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"HeroId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int HeroId
    {
      get { return _HeroId; }
      set { _HeroId = value; }
    }
    private string _CreateTime;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"CreateTime", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string CreateTime
    {
      get { return _CreateTime; }
      set { _CreateTime = value; }
    }
    private string _LastLogoutTime;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"LastLogoutTime", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string LastLogoutTime
    {
      get { return _LastLogoutTime; }
      set { _LastLogoutTime = value; }
    }
    private int _Level;
    [global::ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"Level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Level
    {
      get { return _Level; }
      set { _Level = value; }
    }
    private int _ExpPoints;
    [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name=@"ExpPoints", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int ExpPoints
    {
      get { return _ExpPoints; }
      set { _ExpPoints = value; }
    }
    private int _SceneId;
    [global::ProtoBuf.ProtoMember(9, IsRequired = true, Name=@"SceneId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int SceneId
    {
      get { return _SceneId; }
      set { _SceneId = value; }
    }
    private float _PositionX;
    [global::ProtoBuf.ProtoMember(10, IsRequired = true, Name=@"PositionX", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float PositionX
    {
      get { return _PositionX; }
      set { _PositionX = value; }
    }
    private float _PositionZ;
    [global::ProtoBuf.ProtoMember(11, IsRequired = true, Name=@"PositionZ", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float PositionZ
    {
      get { return _PositionZ; }
      set { _PositionZ = value; }
    }
    private float _FaceDir;
    [global::ProtoBuf.ProtoMember(12, IsRequired = true, Name=@"FaceDir", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float FaceDir
    {
      get { return _FaceDir; }
      set { _FaceDir = value; }
    }
    private int _Money;
    [global::ProtoBuf.ProtoMember(13, IsRequired = true, Name=@"Money", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Money
    {
      get { return _Money; }
      set { _Money = value; }
    }
    private int _Gold;
    [global::ProtoBuf.ProtoMember(14, IsRequired = true, Name=@"Gold", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Gold
    {
      get { return _Gold; }
      set { _Gold = value; }
    }
    private int _SummonerSkillId;
    [global::ProtoBuf.ProtoMember(15, IsRequired = true, Name=@"SummonerSkillId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int SummonerSkillId
    {
      get { return _SummonerSkillId; }
      set { _SummonerSkillId = value; }
    }
    private string _IntDatas;
    [global::ProtoBuf.ProtoMember(16, IsRequired = true, Name=@"IntDatas", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string IntDatas
    {
      get { return _IntDatas; }
      set { _IntDatas = value; }
    }
    private string _FloatDatas;
    [global::ProtoBuf.ProtoMember(17, IsRequired = true, Name=@"FloatDatas", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string FloatDatas
    {
      get { return _FloatDatas; }
      set { _FloatDatas = value; }
    }
    private string _StringDatas;
    [global::ProtoBuf.ProtoMember(18, IsRequired = true, Name=@"StringDatas", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string StringDatas
    {
      get { return _StringDatas; }
      set { _StringDatas = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}