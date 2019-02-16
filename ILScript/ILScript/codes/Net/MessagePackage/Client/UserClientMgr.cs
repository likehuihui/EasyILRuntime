using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Struct;
using MessagePackage.Operation;
using UnityEngine;
using System.Collections;

namespace MessagePackage
{
    public partial class LogonMessageMgr
    {
        /// <summary>
        /// 帐号登陆请求
        /// </summary>
        /// <param name="lgnAcc"></param>
        public void AccountLogon(ref CMD_GP_LogonAccounts lgnAcc)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            lgnAcc.cbValidateFlags = (byte)0x0L;
            lgnAcc.dwPlazaVersion = MessageMgr.CurMsgMgr.PlazaVersion;
            lgnAcc.szMachineID = lgnAcc.szMachineID;
            lgnAcc.szPassPortID = "110";


            //dwPlazaVersion;				    //广场版本
            //szMachineID[LEN_MACHINE_ID];		//机器序列
            //cbValidateFlags;			        //校验标识
            //szPassword[LEN_MD5];				//登录密码
            //szAccounts[LEN_ACCOUNTS];			//登录帐号
            //szPassPortID[LEN_PASS_PORT_ID];	//身份证号

            buffer.Write(lgnAcc.dwPlazaVersion);
            buffer.Write(lgnAcc.szMachineID, 66);
            buffer.Write(lgnAcc.cbValidateFlags);
            buffer.Write(lgnAcc.szPassword, 66);
            buffer.Write(lgnAcc.szAccounts, 64);
            buffer.Write(lgnAcc.szPassPortID, 38);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_LOGON, ConstDefine.SUB_GP_LOGON_ACCOUNTS, buffer);
        }

        /// <summary>
        /// 快速登陆
        /// </summary>
        public void QuickLogin(CMD_GP_QuickLogon cmd)
        {
            cmd.dwPlazaVersion = MessageMgr.CurMsgMgr.PlazaVersion;

            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(cmd.dwPlazaVersion);
            buf.Write(cmd.iLoginType);
            buf.Write(cmd.cLocalSign, 66);
            buf.Write(cmd.cQuickSign, 66);
            buf.Write(cmd.cWinXinSign, 66);
            buf.Write(cmd.cStrSpreader, 64);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_LOGON, ConstDefine.SUB_GP_QUICK_LOGON, buf);
        }
        /// <summary>
        /// 手机号注册
        /// </summary>
        public void RegistAccountPhone(CMD_GP_RegisterAccountsPhone regAcc)
        {
            regAcc.cbValidateFlags = (byte)0x0L;
            regAcc.dwPlazaVersion = MessageMgr.CurMsgMgr.PlazaVersion;
            regAcc.szMachineID = "PH-";

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(regAcc.dwPlazaVersion);
            buffer.Write(regAcc.szMachineID, 66);
            buffer.Write(regAcc.szLogonPass, 66);
            buffer.Write(regAcc.wFaceID);
            buffer.Write(regAcc.cbGender);
            buffer.Write(regAcc.szAccounts, 64);
            buffer.Write(regAcc.szNickName, 64);
            buffer.Write(regAcc.szSpreader, 64);
            buffer.Write(regAcc.szPassPortID, 38);
            buffer.Write(regAcc.szCompellation, 32);
            buffer.Write(regAcc.cbValidateFlags);
            buffer.Write(regAcc.vCodeMsg);
            buffer.Write(regAcc.szMobilePhone, 24);
          //  if (DC.channel == EChannel.DA_HENG_xianwan|| DC.channel == EChannel.DA_HENG_BengBeng)
                buffer.Write(regAcc.szExtraShuntMechineID, 80);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_LOGON, ConstDefine.SUB_GP_REGISTER_ACCOUNTS, buffer);
        }
        /// <summary>
        /// 帐号注册
        /// </summary>
        /// <param name="regAcc"></param>
        public void RegistAccount(CMD_GP_RegisterAccounts regAcc)
        {
            regAcc.cbValidateFlags = (byte)0x0L;
            regAcc.dwPlazaVersion = MessageMgr.CurMsgMgr.PlazaVersion;
            regAcc.szMachineID = "PH-";

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(regAcc.dwPlazaVersion);
            buffer.Write(regAcc.szMachineID, 66);
            buffer.Write(regAcc.szLogonPass, 66);
            buffer.Write(regAcc.wFaceID);
            buffer.Write(regAcc.cbGender);
            buffer.Write(regAcc.szAccounts, 64);
            buffer.Write(regAcc.szNickName, 64);
            buffer.Write(regAcc.szSpreader, 64);
            buffer.Write(regAcc.szPassPortID, 38);
            buffer.Write(regAcc.szCompellation, 32);
            buffer.Write(regAcc.cbValidateFlags);

            buffer.Write(regAcc.flagCodeKey, 96);
            buffer.Write(regAcc.vCodeMsg, 16);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_LOGON, ConstDefine.SUB_GP_REGISTER_ACCOUNTS, buffer);
        }
        /// <summary>
        /// 请求苹果订单二次验证
        /// </summary>
        public void RequestApplePay(CMD_GP_RequestRechargeOrderByIos msg)
        {

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            buffer.Write(msg.wChargeSign);
            buffer.Write(msg.dwUserID);
            buffer.Write(msg.szReceiptData, 19998);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_REQUEST_IOS_RECHARGE, buffer);
        }
        /// <summary>
        /// 请求验证码
        /// </summary>
        /// <param name="cmd"></param>
        public void RequestVerificationCode(CMD_GP_RequestVerificationCode cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.requestType);
            char[] chars = cmd.flagCodeKey.ToCharArray();
            for (var i = 0; i < 96; i++)
            {
                if (i < chars.Length)
                {
                    buffer.Write(chars[i]);
                }
                else
                {
                    buffer.Write('\0');
                }
            }

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_LOGON, ConstDefine.SUB_GP_REQUEST_VCCODE, buffer);
        }

        /// <summary>
        /// 绑定机器
        /// </summary>
        /// <param name="update"></param>
        public void ModifyMachine(ref CMD_GP_ModifyMachine modify)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(modify.cbBind);
            buffer.Write(modify.dwUserID);
            buffer.Write(modify.szPassword, 66);
            buffer.Write(modify.szMachineID, 66);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_MODIFY_MACHINE, buffer);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="update"></param>
        public void ModifyLogonPass(ref CMD_GP_ModifyLogonPass modify)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(modify.dwUserID);
            buffer.Write(modify.szDesPassword, 66);
            buffer.Write(modify.szScrPassword, 66);
            ///buffer.Write(modify.szSafeAnswer, 64);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_MODIFY_LOGON_PASS, buffer);
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="update"></param>
        public void ModifyInsurePass(ref CMD_GP_ModifyInsurePass modify)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(modify.dwUserID);
            buffer.Write(modify.szDesPassword, 66);
            buffer.Write(modify.szScrPassword, 66);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_MODIFY_INSURE_PASS, buffer);
        }

        /// <summary>
        /// 个人资料
        /// </summary>
        /// <param name="user"></param>
        public void UserIndividual(ref CMD_GP_UserIndividual user)
        {

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            CSendPacketHelper helper = new CSendPacketHelper(buffer);
            foreach (var vl in user.dictStr)
            {
                helper.WriteValue(vl.Value as string, vl.Key);
            }

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_MODIFY_INSURE_PASS, buffer);
        }


        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="modify"></param>
        public void ModifyUnderWrite(ref CMD_GP_ModifyUnderWrite modify)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write<CMD_GP_ModifyUnderWrite>(ref modify);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_MODIFY_UNDER_WRITE, buffer);
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        /// <param name="info"></param>
        public void UserFaceInfo(ref CMD_GP_UserFaceInfo info)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(info.wFaceID);
            buffer.Write(info.dwCustomID);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_USER_FACE_INFO, buffer);
        }


        /// <summary>
        /// 修改头像（系统头像）
        /// </summary>
        /// <param name="info"></param>
        public void SystemFaceInfo(ref CMD_GP_SystemFaceInfo info)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            //buffer.Write(info.wFaceID);
            //buffer.Write(info.dwUserID);
            //buffer.Write(info.szPassword,66);
            //buffer.Write(info.szMachineID,66);
            buffer.Write<CMD_GP_SystemFaceInfo>(ref info);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_SYSTEM_FACE_INFO, buffer);
        }

        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="query"></param>
        public void QueryIndividual(ref CMD_GP_QueryIndividual query)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(query.dwUserID);
            buffer.Write(query.szPassword, 66);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_QUERY_INDIVIDUAL, buffer);
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <param name="query"></param>
        public void ModifyIndividual(ref CMD_GP_ModifyIndividual query)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(2014);
            buffer.Write(query.cbGender);
            buffer.Write(query.dwUserID);
            buffer.Write(query.szPassword, 66);
            CSendPacketHelper helper = new CSendPacketHelper(buffer);
            foreach (var vl in query.dictStr)
            {
                helper.WriteValue(vl.Value as string, vl.Key);
            }

            //buffer.Write<CMD_GP_ModifyIndividual>(ref query);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_MODIFY_INDIVIDUAL, buffer);
        }

        /// <summary>
        /// 开通银行
        /// </summary>
        public void UserEnableInsure(ref CMD_GP_UserEnableInsure EnableInsure)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(EnableInsure.dwUserID);
            buffer.Write(EnableInsure.szLogonPassWord, 66);
            buffer.Write(EnableInsure.szInsurePassword, 66);
            buffer.Write(EnableInsure.szMachineID, 66);
           // Mgr_Game.Game_Mgr.closeLoading();
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_USER_ENABLE_INSURE, buffer);
        }

        /// <summary>
        /// 查询转账记录
        /// </summary>
        /// <param name="cmd"></param>
        public void TransferRecord(ref CMD_GP_TransferRecord cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.dwUserID);
            buffer.Write(cmd.iStartIndex);
            buffer.Write(cmd.iEndIndex);
            buffer.Write(cmd.iQueryType);
            buffer.Write(cmd.iQueryKind);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_TRANSFER_RECORD, buffer);
        }

        /// <summary>
        /// 查询税收比例
        /// </summary>
        /// <param name="cmd"></param>
        public void RequestTax(uint userId)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(userId);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_QUERY_TAX, buffer);
        }

        /// <summary>
        /// 查询税收记录
        /// </summary>
        /// <param name="cmd"></param>
        public void QueryTaxDetail(ref CMD_GP_QueryTaxDetail cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.dwUserID);
            buffer.Write(cmd.iStartIndex);
            buffer.Write(cmd.iEndIndex);
            buffer.Write(cmd.iQueryType);
            buffer.Write(cmd.iQueryKind);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_QUERY_TAX_DETAIL, buffer);
        }

        /// <summary>
        /// 领取税收
        /// </summary>
        public void GetTax(uint userId, int iGetKind)
        {
            CMD_GP_GetTax cmd = new CMD_GP_GetTax();
            cmd.dwUserID = userId;
            cmd.iGetKind = iGetKind;

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.dwUserID);
            buffer.Write(cmd.iGetKind);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_GET_TAX, buffer);
        }

        /// <summary>
        /// 存入金币
        /// </summary>
        public void UserSaveScore(ref CMD_GP_UserSaveScore score)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(score.dwUserID);
            buffer.Write(score.lSaveScore);
            buffer.Write(score.szMachineID, 66);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_USER_SAVE_SCORE, buffer);
        }

        /// <summary>
        /// 获取金币
        /// </summary>
        /// <param name="score"></param>
        public void UserTakeScore(ref CMD_GP_UserTakeScore score)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(score.dwUserID);
            buffer.Write(score.lTakeScore);
            buffer.Write(score.szPassword, 66);
            buffer.Write(score.szMachineID, 66);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_USER_TAKE_SCORE, buffer);
        }

        /// <summary>
        /// 转账金币
        /// </summary>
        /// <param name="score"></param>
        public void UserTransferScore(CMD_GP_UserTransferScore score)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            //用户ID
            buffer.Write(score.dwUserId);
            //转账金币
            buffer.Write(score.lTransferScore);
            //银行密码
            buffer.Write(score.szPassword, 66);
            //目标用户
            buffer.Write(score.szAccounts, 64);
            //机械序列
            buffer.Write(score.szMachineID, 66);
            //转账备注            
            buffer.Write(score.szTransRemark, 64);
            //校验MD5
            buffer.Write(score.tCheckSign, 66);

            string log = string.Format("银行转账：[{0}] 用户[{1}]转账给[{2}]金额[{3}]", DateTime.Now.ToString(), score.dwUserId, score.szAccounts, score.lTransferScore);

            Hashtable table = new Hashtable();            
            table.Add("bank", log);
           // ExceptionCacher.ins.UploadError(table);

            Debug.Log(log);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_USER_TRANSFER_SCORE, buffer);
        }

        /// <summary>
        /// 银行资料
        /// </summary>
        /// <param name="info"></param>
        public void UserInsureInfo(CMD_GP_UserInsureInfo info)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(info.wRevenueTake);
            buffer.Write(info.wRevenueTransfer);
            buffer.Write(info.wServerID);
            buffer.Write(info.lUserScore);
            buffer.Write(info.lUserInsure);
            buffer.Write(info.lTransferPrerequisite);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_USER_INSURE_INFO, buffer);
        }

        /// <summary>
        /// 查询银行
        /// </summary>
        /// <param name="info"></param>
        public void QueryInsureInfo(uint dwUserID, string passwd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(dwUserID);
            buffer.Write(passwd, 66);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_QUERY_INSURE_INFO, buffer);
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="info"></param>
        public void QueryUserInfoRequest(CMD_GP_QueryUserInfoRequest info)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(info.dwDstGameID);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_QUERY_USER_INFO_REQUEST, buffer);
        }

        //查询低保信息
        public void QueryBaseEnsureInfo(CMD_GP_BaseEnsureQuery cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.dwUserID);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_BASEENSURE_LOAD, buffer);
        }
        //发送领取低保
        public void PerformTakeBaseEnsure(CMD_GP_BaseEnsureTake TakeInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(TakeInfo.dwUserID);
            buffer.Write(TakeInfo.szPassword, 66);
            buffer.Write(TakeInfo.szMachineID, 66);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_BASEENSURE_TAKE, buffer);
        }

        public void CheckInQueryInfo(CMD_GP_CheckInQueryInfo checkInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(checkInfo.dwUserID);
            buffer.Write(checkInfo.szPassword, 66);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_CHECKIN_QUERY, buffer);
        }

        public void CheckInDone(CMD_GP_CheckInDone checkInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(checkInfo.dwUserID);
            buffer.Write(checkInfo.szPassword, 66);
            buffer.Write(checkInfo.szMachineID, 66);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_CHECKIN_DONE, buffer);
        }
        //发送道具信息请求
        public void SendPorpRequest()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_PROP, UserOperations.SUB_GP_PROP_DATA_REQUEST, buffer);
        }
        //发送购买道具
        public void SendBuyPorp(CMD_GP_BuyPropMsg msgInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msgInfo.dwUserID);
            buffer.Write(msgInfo.dwShopID);
            buffer.Write(msgInfo.dwNum);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_PROP, UserOperations.SUB_GP_PROP_BUY, buffer);
        }
        //发送赠送道具
        public void SendPresentProp(CMD_GP_SendMail msgInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msgInfo.dwSenderUserID);
            buffer.Write(msgInfo.dwRecverUserTD);
            buffer.Write(msgInfo.szSenderNickName, 64);
            buffer.Write(msgInfo.szRecverNickName, 64);
            buffer.Write(msgInfo.szMailName, 64);
            buffer.Write(msgInfo.szMailInfo, 512);
            buffer.Write(msgInfo.dwPropIndexID);
            buffer.Write(msgInfo.wPropNum);
            buffer.Write(msgInfo.dwSpecialValue);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_PROP, UserOperations.SUB_GP_PROP_GIFT, buffer);
        }
        //发送背包信息请求
        public void SendBagRequest(CMD_GP_BagMsg BagInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(BagInfo.dwUserID);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_BAG, UserOperations.SUB_GP_BAG_REQUEST, buffer);
        }
        //使用背包道具
        public void SendUseBagProp(CMD_GP_UsePropMsg msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msg.dwIndexID);
            buffer.Write(msg.dwUserID);
            buffer.Write(msg.dwTargetUserID);
            buffer.Write(msg.dwSpecialValue);
            buffer.Write(msg.tCheckSign, 66);
            buffer.Write(msg.strSpecialString, 256);

            string log = string.Format("红包：[{0}] 用户[{1}]转账给[{2}]金额[{3}]  物品索引[{4}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss "), msg.dwUserID, msg.dwTargetUserID, msg.dwSpecialValue, msg.dwIndexID);

            Hashtable table = new Hashtable();            
            table.Add("redbag", log);            
         //   ExceptionCacher.ins.UploadError(table);

            Debug.Log(log);

            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_BAG, UserOperations.SUB_GP_BAG_PROP_USE, buffer);
        }
        //删除背包道具
        public void SendDeleteBagProp(CMD_GP_UsePropMsg DeletePropMsg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(DeletePropMsg.dwIndexID);
            buffer.Write(DeletePropMsg.dwUserID);
            buffer.Write(DeletePropMsg.dwSpecialValue);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_BAG, UserOperations.SUB_GP_USE_PROP_THREW, buffer);
        }
        //获取邮件数据
        public void GetMailData(uint dwUserID)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(dwUserID);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_PLATFORM, UserOperations.SUB_GP_GET_MAIL_DATA, buffer);
        }
        //发送邮件请求
        public void SendMailRequest(CMD_GP_SendMail_UserInfoRequest msgInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msgInfo.cbByNickName);
            buffer.Write(msgInfo.szAccounts, 64);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_PLATFORM, UserOperations.SUB_GP_SENDMAIL_USERINFO_REQUEST, buffer);
        }
        //发送邮件
        public void SendMail(CMD_GP_SendMail msgInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msgInfo.dwSenderUserID);
            buffer.Write(msgInfo.dwRecverUserTD);
            buffer.Write(msgInfo.szSenderNickName, 64);
            buffer.Write(msgInfo.szRecverNickName, 64);
            buffer.Write(msgInfo.szMailName, 64);
            buffer.Write(msgInfo.szMailInfo, 512);
            buffer.Write(msgInfo.dwPropIndexID);
            buffer.Write(msgInfo.wPropNum);
            buffer.Write(msgInfo.dwSpecialValue);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_PLATFORM, UserOperations.SUB_GP_SEND_MAIL, buffer);
        }
        //邮件操作
        public void MailOperaton(CMD_GP_MailOperation msgInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msgInfo.lResultCode);
            buffer.Write(msgInfo.dwMailID);
            buffer.Write(msgInfo.dwoperatorID);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_PLATFORM, UserOperations.SUB_GP_MAIL_OPERATION, buffer);
        }
        /// <summary>
        /// 同步jackpot
        /// </summary>
        public void GetJackpot()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(1);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_SERVER_LIST, UserOperations.SUB_GP_GET_GAME_JACKPOT, buffer);
        }
        /// <summary>
        /// CDKEY兑换
        /// </summary>
        public static void RequestCDKey(uint userId, string cdkey)
        {
            CMD_GP_Request_CDKEY msg = new CMD_GP_Request_CDKEY();
            msg.dwUserID = userId;
            msg.szCDKEY = cdkey;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_PLATFORM, UserOperations.SUB_GP_REQUEST_CDKEY, msg.Serialize());
        }

        public void SendMarquee()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_MARQUEE, UserOperations.SUB_GP_GET_MARQUEE_DATA, buffer);
        }
        public void UpdateLevelInfo(CMD_GP_GrowLevelQueryInfo msgInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msgInfo.dwUserID);
            buffer.Write(msgInfo.szPassword, 66);
            buffer.Write(msgInfo.szMachineID, 66);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_GROWLEVEL_QUERY, buffer);
        }
        public void Refresh_Money(uint userId)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(userId);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_REFRESH_MONEY_QUERY, buffer);

        }
        public void ShowMeTheMoney(CMD_GP_AddRechargeMoney msg)
        {

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msg.dwUserID);
            buffer.Write(msg.dwGameRMB);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_ADD_RECHARGE_MONEY, buffer);
        }
        public void DayrewardQuery(CMD_GP_DayRewardQueryInfo msgInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msgInfo.dwUserID);
            buffer.Write(msgInfo.szPassword, 66);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_DAYREWARD_QUERY, buffer);
        }
        public void DayrewardDone(CMD_GP_DayRewardDone msgInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msgInfo.dwUserID);
            buffer.Write(msgInfo.szPassword, 66);
            buffer.Write(msgInfo.cbMode);
            buffer.Write(msgInfo.szMachineID, 66);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_DAYREWARD_DONE, buffer);
        }

        /// <summary>
        /// 请求排行榜
        /// </summary>
        public void WealthRankTop()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_WEALTH_RANK_TOP, buffer);
        }

        /// <summary>
        /// 请求代理列表
        /// </summary>
        public void RequestAgent()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_REQUEST_AGENT, buffer);
        }

        /// <summary>
        /// 银行快捷操作
        /// </summary>
        public void BankQuickOperate(CMD_GP_BankQuickOperate cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.iOperaType);
            buffer.Write(cmd.iOperaUserID);
            for (var i = 0; i < ConstDefine.LEN_BANK_QUICK_MAX; i++)
            {
                buffer.Write(cmd.iOperaData[i]);
            }
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_BANK_QUICK_OPERA, buffer);
        }

        /// <summary>
        /// 请求充值订单
        /// </summary>
        /// <param name="cmd"></param>
        public void RequestRechargeOrder(CMD_GP_RequestRechargeOrder cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.dwUserID);
            buffer.Write(cmd.dwRechargeValue);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_REQUEST_RECHARGE, buffer);
        }

        /// <summary>
        /// 请求充值订单3322
        /// </summary>
        /// <param name="cmd"></param>
        public void RequestRechargeOrder(CMD_GP_RequestRechargeOrder_3322 cmd)
        {
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_REQUEST_RECHARGE, cmd.Serialize());
        }

        public void RequestRechargeOrderAlipay(CMD_GP_RequestRechargeOrder_3322 cmd)
        {
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_REQUEST_RECHARGE, cmd.Serialize());
        }

        /// <summary>
        /// 请求支付宝支付
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="value"></param>
        public void RequestRechargeOrderAlipay(uint userId, int value)
        {
            var cmd = new CMD_GP_RequestRechargeOrderAlipay();
            cmd.dwUserID = userId;
            cmd.dwRechargeValue = value;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_REQUEST_RECHARGE_ALIPAY, cmd.Serialize());
        }

        /// <summary>
        /// 请求微信支付
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="value"></param>
        public void RequestRechargeOrderWeiXin(uint userId, int value)
        {
            var cmd = new CMD_GP_RequestRechargeOrderWeiXin();
            cmd.dwUserID = userId;
            cmd.dwRechargeValue = value;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_REQUEST_RECHARGE_WEIXIN, cmd.Serialize());
        }


        /// <summary>
        /// 心跳协议
        /// </summary>
        /// <param name="cmd"></param>
        public void HeartBeat(CMD_GP_HeartBeat cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.dwUserID);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_REQUEST_HEARTBEAT, buffer);
        }

        /// <summary>
        /// 请求新的跑马灯
        /// </summary>
        public void GetMarqueeNew()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_MARQUEE, UserOperations.SUB_GP_GET_MARQUEE_NEW, buffer);
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        public void ChangeNickName(uint userId, ushort type, string nickName)
        {
            CMD_GP_ChangeNickName cmd = new CMD_GP_ChangeNickName();
            cmd.dwUserID = userId;
            cmd.wType = type;
            cmd.szNewNickName = nickName;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_CHANGE_NAME, cmd.Serialize());
        }

        /// <summary>
        /// 请求手机验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="sign">是否检查 0不检查 1检查</param>
        static public void RequestPhoneVCCode(string phone, ushort sign)
        {
            CMD_GP_RequestPhoneVerificationCode cmd = new CMD_GP_RequestPhoneVerificationCode();
            cmd.phone = phone;
            cmd.checkSign = sign;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_LOGON, ConstDefine.SUB_GP_REQUEST_PHONE_VCCODE, cmd.Serialize());
        }

        /// <summary>
        /// 绑定手机
        /// </summary>
        /// <param name="vc">验证码</param>
        /// <param name="userId">用户ID</param>
        /// <param name="phone">手机号</param>
        static public void BindIphone(int vc, uint userId, string phone, ushort wIsFormalAC, string tNewAccount, string tNewNickName, string tNewPassword)
        {

            CMD_GP_RequestBindPhone cmd = new CMD_GP_RequestBindPhone();
            cmd.iVeriCode = vc;
            cmd.dwUserID = userId;
            cmd.phone = phone;
            cmd.wIsFormalAC = wIsFormalAC;
            cmd.tNewAccount = tNewAccount;
            cmd.tNewNickName = tNewNickName;
            cmd.tNewPassword = tNewPassword;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_LOGON, ConstDefine.SUB_GP_BIND_PHONE, cmd.Serialize());

        }

        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="vc">验证码</param>
        /// <param name="type">0重置登录密码  1重置银行密码</param>
        /// <param name="phone">手机号</param>
        static public void ResetPassword(int vc, ushort type, string phone, string pasd)
        {
            CMD_GP_RequestResetPasswordByPhone cmd = new CMD_GP_RequestResetPasswordByPhone();
            cmd.iVeriCode = vc;
            cmd.wType = type;
            cmd.phone = phone;
            cmd.newPassword = pasd;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_LOGON, ConstDefine.SUB_GP_RESET_PASSWORD, cmd.Serialize());

        }
        /// <summary>
        /// 获取房间
        /// </summary>
        public void GetRoomInfo(ushort kindID)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(0);
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_SERVER_LIST, ConstDefine.SUB_GP_GET_MATCH, buffer);
        }
        /// <summary>
        /// 请求抽奖物品
        ///1 请求抽奖剩余次数
        ///2 请求转盘列表物品信息
        ///3 请求抽奖
        ///4 请求自己的抽奖记录信息
        ///5 请求全服的抽奖记录信息
        /// </summary>
        public static void GetLotteryInfo(ushort sign, uint userID)
        {
            CMD_GP_LotteryInfo cmd = new CMD_GP_LotteryInfo();
            cmd.sign = sign;
            cmd.dwUserID = userID;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_LOTTERY_BASE_REQUEST, cmd.Serialize());
        }
        /// <summary>
        /// 手动领取奖励
        /// </summary>
        public static void GetAward(int recordID,uint userID,string name="",string qq="",string phone="",string address="")
        {
            CMD_GP_ReceiveLottery cmd = new CMD_GP_ReceiveLottery();
            cmd.iRecordID = recordID;
            cmd.dwUserID = userID;
            cmd.tPlayerName = name;
            cmd.tQQ = qq;
            cmd.tPhone = phone;
            cmd.tAdress = address;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_RECEIVE_LOTTERY, cmd.Serialize());
        }

        /// <summary>
        /// 每日分享完成
        /// </summary>
        public static void ShareEveryDay(uint userId)
        {
            CMD_GP_DailyShareWeiXin cmd = new CMD_GP_DailyShareWeiXin();
            cmd.dwUserID = userId;
            MessageMgr.CurMsgMgr.SendMsg2LgnSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_DAILY_SHARE_WEIXIN, cmd.Serialize());
        }
    }
}
