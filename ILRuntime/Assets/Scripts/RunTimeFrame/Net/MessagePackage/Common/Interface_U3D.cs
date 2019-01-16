using System;
using System.Collections.Generic;
using System.Text;
using MessagePackage.Struct;
using CYNetwork;
/// <summary>
/// 子游戏需继承的接口
/// </summary>
public interface Interface_Game
{
    /// <summary>
    /// 主子命令回调
    /// </summary>
    /// <param name="mnCmd"></param>
    /// <param name="sbCmd"></param>
    /// <param name="readBuffer"></param>
    /// <returns></returns>
    bool HandleReadNetData(ushort mnCmd, ushort sbCmd, CyNetReadBuffer readBuffer);
   // bool CheckGame();
    void Error(string errStr);
}
/// <summary>
/// U3D大厅接口
/// </summary>
public interface Interface_U3D_DT
{
    void CMD_(ushort mainCMD, ushort subCmd);
    void Error(string str, int stata);
    void LogonFinish(CMD_GP_LogonFinish msg);
    void LogonFailed(CMD_GP_LogonFailure cmd);
    void QuickLoginBack(CMD_GP_QuickLogonBack cmd);
    void LogonSucc(CMD_GP_LogonSuccess succMsg);
    void LogonListType(Dictionary<ushort, tagGameType> dic_GameType);
    void LogonListKind(Dictionary<ushort, tagGameKind> dic_GameKind);
    void LogonListRoom(Dictionary<ushort, tagGameServer> dic_GameRoom);
    void LogonListMatch(Dictionary<ushort, tagGameMatch> dic_GameMatch);
    void LogonList();
    /// <summary>
    /// 银行操作失败
    /// </summary>
    /// <param name="lResultCode"></param>
    /// <param name="szDescribestring"></param>
    void InsureFailure(long lResultCode, string szDescribestring);
    /// <summary>
    /// 银行操作成功
    /// </summary>
    /// <param name="msg"></param>
    void InsureSuccess(CMD_GP_UserInsureSuccess msg);

    //修改资料成功
    void ChangeuserInfoSuccess(CMD_GP_OperateSuccess msg);

    //修改资料失败
    void ChangeuserInfoFaile(CMD_GP_OperateFailure msg);

    //用户信息返回
    void ModifyIndiviaualReturn(CMD_GP_UserIndividual msg);

    //低保信息查询
    void UserBaseensure(CMD_GP_BaseEnsureParamter msg);

    //领取低保结果
    void BaseensureResult(CMD_GP_BaseEnsureResult result);

    void CheckInfo(CMD_GP_CheckInInfo CheckInfo);

    void CheckInfoNew(CMD_GP_CheckInInfo_NEW msg);

    void CheckInResult(CMD_GP_CheckInResult msg);

    void PropInfo(CMD_GP_PROPINFO PrpoMsgInfo);

    void PropInfoFinish();
    void BuyPropSuccess();
    void BuyPropFailed();

    void BagInfo(CMD_GP_BagPropInfoMsg BagMsgInfo);

    void BagInfoFinish();

    void UseBagPorpResult(CMD_GP_UsePropResult msg);

    void DeleteBagPorpResult(byte msg);

    void MailInfo(CMD_GP_MailData msg);
    void MailInfoFinish();

    void SendMailFailure(CMD_GP_SendMailFailure msgInfo);

    void SendMailUseInfoResult(CMD_GP_UserMailUserInfo msgInfo);

    void SendMailResult(CMD_GP_SendMailResult msgInfo);

    void OperationResult(CMD_GP_MailOperation_Result msgInfo);

    void MarqueeInfo(CMD_GP_MarqueeMsg msgInfo);

    void MarqueeInfoFinish();
    void GrowLevelConfig(CMD_GP_GrowLevelConfig msgInfo);
    void GrowLevelParameter(CMD_GP_GrowLevelParameter msgInfo);
    void GrowlevelUpgrade(CMD_GP_GrowLevelUpgrade msgInfo);
    void RefreshMoneyResult(CMD_GP_RefreshMoneyResult msgInfo);
    void DayrewardInfo(CMD_GP_PROPINFO msgInfo);
    void DayrewardCount(CMD_GP_DayRewardInfo msgInfo);
    void DayrewardResult(CMD_GP_DayRewardResult msgInfo);

    void InsureEnableResult(CMD_GR_S_UserInsureEnableResult msgInfo);
    void WealthRankTopResult(CMD_GP_WealthRankTopResult[] cmdList);
    bool CheckNet();

    /// <summary>
    /// 验证码返回
    /// </summary>
    /// <param name="cmd"></param>
    void VerificationCodeResult(CMD_GP_RequestVerificationCodeResult cmd);

    /// <summary>
    /// 转账记录结果
    /// </summary>
    /// <param name="cmd"></param>
    void TransferRecordResult(CMD_GP_TransferRecordResult cmd);

    /// <summary>
    /// 税收比例查询结果
    /// </summary>
    /// <param name="cmd"></param>
    void TaxInfo(CMD_GP_QueryTaxResult cmd);

    /// <summary>
    /// 税收详情查询结果
    /// </summary>
    /// <param name="cmd"></param>
    void QueryTaxDetailResult(CMD_GP_QueryTaxDetailResult cmd);

    /// <summary>
    /// 领取税收结果
    /// </summary>
    /// <param name="cmd"></param>
    void GetTaxResult(CMD_GP_GetTaxResult cmd);

    /// <summary>
    /// 银行快捷操作结果
    /// </summary>
    /// <param name="cmd"></param>
    void BankQuickOperateResult(CMD_GP_BankQuickOperateResult cmd);

    /// <summary>
    /// 代理列表
    /// </summary>
    /// <param name="cmd"></param>
    void AgentResult(CMD_GP_AgentReply cmd);

    /// <summary>
    /// 充值订单结果
    /// </summary>
    void RechargeOrderResult(CMD_GP_RequestRechargeOrderResult cmd);

    void RechargeOrderResult(CMD_GP_RequestRechargeOrderResult_3322 cmd);

    void RequestRechargeAlipayResult(CMD_GP_RequestRechargeOrderAlipayResult cmd);
    void RequestRechargeWeixinResult(CMD_GP_RequestRechargeOrderWeiXinResult cmd);

    /// <summary>
    /// 心跳协议返回
    /// </summary>
    void HeartBeatBack(CMD_GP_HeartBeatResult msg);

    /// <summary>
    /// 改变昵称结果
    /// </summary>
    /// <param name="msg"></param>
    void ChangeNickNameResult(CMD_GP_ChangeNickNameResult msg);

    /// <summary>
    /// 请求手机验证码结果
    /// </summary>
    /// <param name="msg"></param>
    void PhoneVCCodeResult(CMD_GP_RequestPhoneVerificationCodeResult msg);

    /// <summary>
    /// 请求绑定手机结果
    /// </summary>
    /// <param name="msg"></param>
    void BindPhoneBack(CMD_GP_RequestBindPhoneBack msg);

    /// <summary>
    /// 修改密码结果
    /// </summary>
    void ChangePwdBack(CMD_GP_ModifyLogonPassBack msg);
    /// <summary>
    /// 请求重置密码结果
    /// </summary>
    /// <param name="msg"></param>
    void ResetPasswordBack(CMD_GP_RequestResetPasswordByPhoneBack msg);

    /// <summary>
    /// CDKEY结果
    /// </summary>
    /// <param name="msg"></param>
    void RequestCDKeyResult(CMD_GP_RequestCdkeyResult msg);
    /// <summary>
    /// 礼包使用结果
    /// </summary>
    void RequestBagResult(CMD_GP_SimpleGiftResultProp[] msg);
}
/// <summary>
/// U3D房间接口
/// </summary>
public interface Interface_U3D_ROOM
{
    void CMD_(ushort mainCMD, ushort subCmd,int len);
    void Error(string str);
    /// <summary>
    /// 登陆完毕
    /// </summary>
    void LogonFinish();
    /// <summary>
    /// 登陆失败
    /// </summary>
    /// <param name="str"></param>
    void LogonFailed(string str);
    /// <summary>
    /// 登陆成功
    /// </summary>
    /// <param name="succMsg"></param>
    void LogonSucc(CMD_GR_LogonSuccess succMsg);
    void ROOM_Config(CMD_GR_ConfigServer msg);
    void User_EnterROOM(tagUserInfoHead userInfo);
    void Config_OK();
    void SystemMessage(string szString);
    void RequestShiBai(CMD_GR_RequestFailure msg);
    void Table_Status(CMD_GR_TableStatus msg);
    void Table_Info(CMD_GR_TableInfo msg);
    void User_Status(CMD_GR_UserStatus msg);
    void User_FenShu(CMD_GR_UserScore msg);

    void CloseRoom();
    void CloseApp();



    void MatchSignupResult(CMD_GP_MatchSignupResult msg);
    void MatchNum(CMD_GR_Match_Num msg);
    void MatchFee(CMD_GR_Match_Fee msg);
    void MatchStatus(byte cbMatchStatus);
    void MatchGoldUpdate(CMD_GR_MatchGoldUpdate msg);
    void CreateRoomCallBack(CMD_RC_S_RoomCardData msg);
    void JionRoomCallBack(CMD_RC_S_RoomCardData msg);

}

/// <summary>
/// U3D比赛接口
/// </summary>
public interface Interface_U3D_MATCH
{
    void MatchSignupResult(CMD_GP_MatchSignupResult msg);

    void MatchNum(CMD_GR_Match_Num msg);
}