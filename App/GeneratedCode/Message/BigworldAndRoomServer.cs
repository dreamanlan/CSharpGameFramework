// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: BigworldAndRoomServer.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace GameFrameworkMessage
{

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_BL_BroadcastText : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public int BroadcastType { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string Content { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int RollCount { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_BL_QueryUserStateResult : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong Guid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int State { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_BL_UserChangeScene : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong Guid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int SceneId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_BL_UserOffline : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong Guid { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LB_BigworldUserBaseInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public string NodeName { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int WorldId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public string AccountId { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public string ClientInfo { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public string StartServerTime { get; set; }

        [global::ProtoBuf.ProtoMember(6, IsRequired = true)]
        public int SceneId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LB_QueryUserState : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong Guid { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LB_RequestEnterScene : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public Msg_LB_BigworldUserBaseInfo BaseInfo { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public Msg_LR_RoomUserInfo User { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int SceneId { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public int WantRoomId { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public int FromSceneId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LB_UpdateUserServerInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public int WorldId { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int UserCount { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LB_UserOffline : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong Guid { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LB_UserRelogin : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong Guid { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LBL_Message : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public MsgTypeEnum MsgType { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue("")]
        public string TargetName
        {
            get => __pbn__TargetName ?? "";
            set => __pbn__TargetName = value;
        }
        public bool ShouldSerializeTargetName() => __pbn__TargetName != null;
        public void ResetTargetName() => __pbn__TargetName = null;
        private string __pbn__TargetName;

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public byte[] Data { get; set; }

        [global::ProtoBuf.ProtoContract()]
        public enum MsgTypeEnum
        {
            Node = 0,
            Room = 1,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LR_ActiveScene : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong[] UserGuids { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int SceneId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LR_ChangeScene : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int TargetRoomId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LR_EnterScene : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public Msg_LR_RoomUserInfo UserInfo { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        public int HP
        {
            get => __pbn__HP.GetValueOrDefault();
            set => __pbn__HP = value;
        }
        public bool ShouldSerializeHP() => __pbn__HP != null;
        public void ResetHP() => __pbn__HP = null;
        private int? __pbn__HP;

        [global::ProtoBuf.ProtoMember(5)]
        public int MP
        {
            get => __pbn__MP.GetValueOrDefault();
            set => __pbn__MP = value;
        }
        public bool ShouldSerializeMP() => __pbn__MP != null;
        public void ResetMP() => __pbn__MP = null;
        private int? __pbn__MP;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LR_ReclaimItem : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int ItemId { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public string Model { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public string Particle { get; set; }

        [global::ProtoBuf.ProtoMember(6, IsRequired = true)]
        public int TipDict { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LR_ReconnectUser : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LR_ReplyRegisterRoomServer : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public bool IsOk { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LR_RoomUserInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong Guid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string Nick { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public uint Key { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public int Hero { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public int Camp { get; set; }

        [global::ProtoBuf.ProtoMember(6, IsRequired = true)]
        public bool IsMachine { get; set; }

        [global::ProtoBuf.ProtoMember(7, IsRequired = true)]
        public int Level { get; set; }

        [global::ProtoBuf.ProtoMember(8)]
        public float EnterX
        {
            get => __pbn__EnterX.GetValueOrDefault();
            set => __pbn__EnterX = value;
        }
        public bool ShouldSerializeEnterX() => __pbn__EnterX != null;
        public void ResetEnterX() => __pbn__EnterX = null;
        private float? __pbn__EnterX;

        [global::ProtoBuf.ProtoMember(9)]
        public float EnterY
        {
            get => __pbn__EnterY.GetValueOrDefault();
            set => __pbn__EnterY = value;
        }
        public bool ShouldSerializeEnterY() => __pbn__EnterY != null;
        public void ResetEnterY() => __pbn__EnterY = null;
        private float? __pbn__EnterY;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LR_UserQuit : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LR_UserReLive : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_LRL_StoryMessage : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public int RoomId
        {
            get => __pbn__RoomId.GetValueOrDefault();
            set => __pbn__RoomId = value;
        }
        public bool ShouldSerializeRoomId() => __pbn__RoomId != null;
        public void ResetRoomId() => __pbn__RoomId = null;
        private int? __pbn__RoomId;

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public string MsgId { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        public global::System.Collections.Generic.List<MessageArg> Args { get; } = new global::System.Collections.Generic.List<MessageArg>();

        [global::ProtoBuf.ProtoContract()]
        public partial class MessageArg : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
            public Msg_LRL_StoryMessage.ArgType val_type { get; set; }

            [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
            public string str_val { get; set; }

        }

        [global::ProtoBuf.ProtoContract()]
        public enum ArgType
        {
            NULL = 0,
            INT = 1,
            FLOAT = 2,
            STRING = 3,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_ActiveScene : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong[] UserGuids { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int SceneId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_ActiveSceneResult : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong[] UserGuids { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int Result { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_ChangeScene : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong[] UserGuids { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int SceneId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_ChangeSceneResult : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int TargetRoomId { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public int Result { get; set; }

        [global::ProtoBuf.ProtoMember(5)]
        public int HP
        {
            get => __pbn__HP.GetValueOrDefault();
            set => __pbn__HP = value;
        }
        public bool ShouldSerializeHP() => __pbn__HP != null;
        public void ResetHP() => __pbn__HP = null;
        private int? __pbn__HP;

        [global::ProtoBuf.ProtoMember(6)]
        public int MP
        {
            get => __pbn__MP.GetValueOrDefault();
            set => __pbn__MP = value;
        }
        public bool ShouldSerializeMP() => __pbn__MP != null;
        public void ResetMP() => __pbn__MP = null;
        private int? __pbn__MP;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_EnterSceneResult : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int Result { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_PickItem : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int ItemId { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public string Model { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public string Particle { get; set; }

        [global::ProtoBuf.ProtoMember(6)]
        [global::System.ComponentModel.DefaultValue("")]
        public string RoomSvrName
        {
            get => __pbn__RoomSvrName ?? "";
            set => __pbn__RoomSvrName = value;
        }
        public bool ShouldSerializeRoomSvrName() => __pbn__RoomSvrName != null;
        public void ResetRoomSvrName() => __pbn__RoomSvrName = null;
        private string __pbn__RoomSvrName;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_PickMoney : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int Num { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_RegisterRoomServer : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public string ServerName { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int MaxRoomNum { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public string ServerIp { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public uint ServerPort { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_ReplyReconnectUser : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int Result { get; set; }

        [global::ProtoBuf.ProtoContract()]
        public enum ReconnectResultEnum
        {
            Drop = 0,
            NotExist = 1,
            Online = 2,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_RoomServerUpdateInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public string ServerName { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int IdleRoomNum { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int UserNum { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_UserDrop : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public bool IsBattleEnd { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_UserLobbyItemInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public int ItemId { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int ItemNum { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Msg_RL_UserQuit : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong UserGuid { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int RoomId { get; set; }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
