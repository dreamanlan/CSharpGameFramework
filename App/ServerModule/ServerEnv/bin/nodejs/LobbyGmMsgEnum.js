//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/LobbyGmMsg.dsl生成！！！
//----------------------------------------------------------------------------
var LobbyGmMessageDefine = {
	GmUserBasic : 1,
	GmUserInfo : 2,
	Msg_CL_GmQueryUserByGuid : 3,
	Msg_CL_GmQueryUserByNickname : 4,
	Msg_LC_GmQueryUser : 5,
	Msg_CL_GmQueryUserByFuzzyNickname : 6,
	Msg_LC_GmQueryUserByFuzzyNickname : 7,
	Msg_CL_GmQueryAccount : 8,
	Msg_LC_GmQueryAccount : 9,
	Msg_CL_GmKickUser : 10,
	Msg_LC_GmKickUser : 11,
	Msg_CL_GmLockUser : 12,
	Msg_LC_GmLockUser : 13,
	Msg_CL_GmUnlockUser : 14,
	Msg_LC_GmUnlockUser : 15,
	Msg_CL_GmAddExp : 16,
	Msg_LC_GmAddExp : 17,
	Msg_CL_GmUpdateMaxUserCount : 18,
	Msg_CL_PublishNotice : 19,
	Msg_LC_PublishNotice : 20,
	Msg_CL_SendMail : 21,
	Msg_LC_SendMail : 22,
	Msg_CL_GmHomeNotice : 23,
	Msg_LC_GmHomeNotice : 24,
	Msg_CL_GmForbidChat : 25,
	Msg_LC_GmForbidChat : 26,
	Msg_CL_GmCode : 27,
	Msg_CL_GmGeneralOperation : 28,
	Msg_LC_GmGeneralOperation : 29,
	ItemDataMsg : 30,
};

exports.GameFrameworkMessage = {

	LobbyGmMessageDefine : LobbyGmMessageDefine,

};
