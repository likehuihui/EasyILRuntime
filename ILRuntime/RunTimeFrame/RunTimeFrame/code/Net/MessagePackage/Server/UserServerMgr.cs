using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;
using System.Runtime.InteropServices;

namespace MessagePackage
{
    public partial class LogonMessageMgr
    {
        public const ushort LEN_WEEK = 7;
        /// <summary>
        /// 购买道具成功
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandlePropertySuccess(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_PropertySuccess msg = new CMD_GP_PropertySuccess();
            msg.wPropertyID = readBuffer.ReadUshort();
            msg.wPropertyCount = readBuffer.ReadUshort();
            msg.wMemberOrder = readBuffer.ReadUshort();
            msg.MemberOverDate = readBuffer.ReadDateTime();
            msg.lConsumeGold = readBuffer.ReadLong();
            //readBuffer.ReadObj(msg);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }

        /// <summary>
        /// 个人资料
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserIndividual(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserIndividual msg = new CMD_GP_UserIndividual();

            msg.dwUserID = readBuffer.ReadUint();
            // msg.bGender = readBuffer.ReadByte();

            if (readBuffer.CurrentPos < readBuffer.Length)
            {
                msg.dictStr = new Dictionary<int, object>();
                CReadPacketHelper helper = new CReadPacketHelper(readBuffer);
                while (readBuffer.CurrentPos < readBuffer.Length)
                {
                    KeyValuePair<int, object> val = helper.ReadValue();
                    msg.dictStr.Add(val.Key, val.Value);
                }
            }

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.ModifyIndiviaualReturn(msg);
            }
            //if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }


        /// <summary>
        /// 购买道具失败
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandlePropertyFailure(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_PropertyFailure msg = new CMD_GP_PropertyFailure();
            msg.lResultCode = readBuffer.ReadInt();
            msg.szDescribestring = readBuffer.ReadString(256);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }

        /// <summary>
        /// 魅力查询失败
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleQueryCharmFailure(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_QueryCharmFailure msg = new CMD_GP_QueryCharmFailure();
            msg.lResultCode = readBuffer.ReadInt();
            msg.szDescribestring = readBuffer.ReadString(256);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }

        /// <summary>
        /// 是否挂机用户查询结果
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleQueryUserIsOnlineAWard(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_QueryUserIsOnlineAWard msg = new CMD_GP_QueryUserIsOnlineAWard();
            msg.lResultCode = readBuffer.ReadInt();

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }

        /// <summary>
        /// 查询银行信息
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleInsureRecord(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GPA_InsureRecord msg = new CMD_GPA_InsureRecord();
            msg.cbPageCount = readBuffer.ReadByte();
            msg.dwTotalCount = readBuffer.ReadUint();
            msg.bankRecord = new tagBankRecord();
            msg.bankRecord.dwSrcGameID = readBuffer.ReadUint();
            msg.bankRecord.dwDstGameID = readBuffer.ReadUint();
            msg.bankRecord.szSrcNickName = readBuffer.ReadString(64);
            msg.bankRecord.szDstNickName = readBuffer.ReadString(64);
            msg.bankRecord.lOperateScore = readBuffer.ReadLong();
            msg.bankRecord.lRevenue = readBuffer.ReadLong();
            msg.bankRecord.nTradeType = readBuffer.ReadInt();
            msg.bankRecord.stOperateTime = readBuffer.ReadDateTime();

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }

        /// <summary>
        /// 登录银行
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleLogonBank(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GPA_LogonBank msg = new CMD_GPA_LogonBank();
            msg.cbLogonResult = readBuffer.ReadByte();

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserTransferUserInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserTransferUserInfo msg = new CMD_GP_UserTransferUserInfo();
            msg.dwTargetGameID = readBuffer.ReadUint();
            msg.szNickName = readBuffer.ReadString(64);
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 转账记录查询结果
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserTransferRecordResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            const int LEN_NICKNAME = 32;
            const int LEN_TRANS_REMARK = 32;

            CMD_GP_TransferRecordResult msg = new CMD_GP_TransferRecordResult();
            msg.iStartIndex = rb.ReadInt();
            msg.iEndIndex = rb.ReadInt();
            msg.iQueryType = rb.ReadInt();
            msg.iQueryKind = rb.ReadInt();
            msg.iMaxIndex = rb.ReadInt();
            msg.iThisTurnLength = rb.ReadInt();
            msg.trRecord = new TransferRecordVO[msg.iThisTurnLength];
            for (int i = 0; i < msg.iThisTurnLength; i++)
            {
                TransferRecordVO vo = new TransferRecordVO();
                vo.szSourceNickName = rb.ReadString(LEN_NICKNAME * 2);
                vo.dwSourceUserID = rb.ReadUint();
                vo.dwSourceGameID = rb.ReadUint();
                vo.lOperationScore = rb.ReadLong();
                vo.lTaxScore = rb.ReadLong();
                vo.sOperationTime = rb.ReadDateTime();
                vo.szTransRemark = rb.ReadString(LEN_TRANS_REMARK * 2);
                msg.trRecord[i] = vo;
            }

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.TransferRecordResult(msg);
            }
            return true;
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleOperateFailure(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_OperateFailure msg = new CMD_GP_OperateFailure();
            msg.lResultCode = readBuffer.ReadInt();
            msg.szDescribestring = readBuffer.ReadString();

            //  if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.ChangeuserInfoFaile(msg);
            }
            return true;
        }

        //低保查询返回的结果
        private bool HandleUserBaseensure(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {

            CMD_GP_BaseEnsureParamter msg = new CMD_GP_BaseEnsureParamter();

            msg.lScoreCondition = readBuffer.ReadLong();
            msg.lScoreAmount = readBuffer.ReadLong();
            msg.cbTakeTimes = readBuffer.ReadByte();
            msg.cbAlreadyTakeTimes = readBuffer.ReadByte();
            msg.bindingState = readBuffer.ReadByte();

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.UserBaseensure(msg);
            }
            return true;
        }

        //领取低保的结果
        private bool HandleUserBaseensureResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_BaseEnsureResult result = new CMD_GP_BaseEnsureResult();
            result.bSuccessed = readBuffer.ReadBoolean();
            result.lGameScore = readBuffer.ReadLong();
            result.szNotifyContent = readBuffer.ReadString();
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.BaseensureResult(result);
            }
            return true;
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleOperateSuccess(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_OperateSuccess msg = new CMD_GP_OperateSuccess();

            msg.lResultCode = readBuffer.ReadInt();
            msg.szDescribestring = readBuffer.ReadString();
            // if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.ChangeuserInfoSuccess(msg);
            }

            return true;
        }

        /// <summary>
        /// 修改密码返回
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleModifyPasswdResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GPA_ModifyPasswdResult msg = new CMD_GPA_ModifyPasswdResult();
            msg.lResultCode = readBuffer.ReadInt();
            msg.szDescribeString = readBuffer.ReadString(256);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 魅力兑换成功
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleChangedCharmSuccess(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_ChangedCharmSuccess msg = new CMD_GP_ChangedCharmSuccess();
            msg.dwUserID = readBuffer.ReadUint();
            msg.lLessCharm = readBuffer.ReadLong();
            msg.lAllChangedCharm = readBuffer.ReadLong();
            msg.lUserInsure = readBuffer.ReadLong();

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleQueryUserInfoRequest(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_QueryUserInfoRequest msg = new CMD_GP_QueryUserInfoRequest();
            msg.dwDstGameID = readBuffer.ReadUint();

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 银行成功
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserInsureSuccess(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserInsureSuccess msg = new CMD_GP_UserInsureSuccess();
            msg.dwUserID = readBuffer.ReadUint();
            msg.lUserScore = readBuffer.ReadLong();
            msg.lUserInsure = readBuffer.ReadLong();
            msg.szDescribestring = readBuffer.ReadString();

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.InsureSuccess(msg);
            }
            return true;
        }

        /// <summary>
        /// 银行失败
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserInsureFailure(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserInsureFailure msg = new CMD_GP_UserInsureFailure();
            msg.lResultCode = readBuffer.ReadInt();
            msg.szDescribestring = readBuffer.ReadString();

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.InsureFailure(msg.lResultCode, msg.szDescribestring);
            }

            return true;
        }

        /// <summary>
        /// 查询银行
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleQueryInsureInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_QueryInsureInfo msg = new CMD_GP_QueryInsureInfo();
            msg.dwUserID = readBuffer.ReadUint();
            msg.szPassword = readBuffer.ReadString(66);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }


        /// <summary>
        /// 银行资料
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserInsureInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_CheckBankPassword msg = new CMD_GP_CheckBankPassword();
            msg.wResult = readBuffer.ReadUshort();
            msg.szDescribeString = readBuffer.ReadString(256);
            //CMD_GP_UserInsureInfo msg = new CMD_GP_UserInsureInfo();
            //msg.wRevenueTake = readBuffer.ReadUshort();
            //msg.wRevenueTransfer = readBuffer.ReadUshort();
            //msg.wServerID = readBuffer.ReadUshort();
            //msg.lUserScore = readBuffer.ReadLong();
            //msg.lUserInsure = readBuffer.ReadLong();
            //msg.lTransferPrerequisite = readBuffer.ReadLong();

            //Console.WriteLine(String.Format("UserServerMgr.HandleUserInsureInfo {0} {1} {2} {3} {4} {5}", msg.wRevenueTake, msg.wRevenueTransfer,
            //    msg.wServerID, msg.lUserScore, msg.lUserInsure, msg.lTransferPrerequisite));
            // if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            Loom.QueueOnMainThread(() =>
            {
              //  NoticeManager.SendNoticeQuick(HallNotice.CheckBankPassword, msg);
            });

            return true;
        }


        /// <summary>
        /// 转账金币
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserTransferScore(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserTransferScore msg = new CMD_GP_UserTransferScore();
            msg.dwUserId = readBuffer.ReadUint();
            msg.lTransferScore = readBuffer.ReadLong();
            msg.szPassword = readBuffer.ReadString(66);
            msg.szAccounts = readBuffer.ReadString(64);
            msg.szMachineID = readBuffer.ReadString(66);
            msg.szTransRemark = readBuffer.ReadString(64);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }


        /// <summary>
        /// 提取金币
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserTakeScore(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserTakeScore msg = new CMD_GP_UserTakeScore();
            msg.dwUserID = readBuffer.ReadUint();
            msg.lTakeScore = readBuffer.ReadUshort();
            msg.szPassword = readBuffer.ReadString(66);
            msg.szMachineID = readBuffer.ReadString(66);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }


        /// <summary>
        /// 存入金币
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserSaveScore(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserSaveScore msg = new CMD_GP_UserSaveScore();
            msg.dwUserID = readBuffer.ReadUint();
            msg.lSaveScore = readBuffer.ReadLong();
            msg.szMachineID = readBuffer.ReadString(66);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;

        }

        /// <summary>
        /// 挂机用户状态
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleQueryUserOnlineAWard(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_QueryUserOnlineAWard msg = new CMD_GP_QueryUserOnlineAWard();
            msg.lResultCode = readBuffer.ReadInt();
            msg.szDescribestring = readBuffer.ReadString(256);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;

        }

        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleQueryIndividual(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_QueryIndividual msg = new CMD_GP_QueryIndividual();
            msg.dwUserID = readBuffer.ReadUint();
            msg.szPassword = readBuffer.ReadString(66);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleChangePassword(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_ModifyLogonPassBack msg = new CMD_GP_ModifyLogonPassBack();
            msg.szNewLogonPassword = readBuffer.ReadString(66);
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }

        /// <summary>
        /// 金币转账成功
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserTransferSuccess(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserTransferSuccess msg = new CMD_GP_UserTransferSuccess();
            msg.dwUserID = readBuffer.ReadUint();
            msg.lRevenue = readBuffer.ReadLong();
            msg.lUserScore = readBuffer.ReadLong();
            msg.lUserInsure = readBuffer.ReadLong();

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 系统头像
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleSystemFaceInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_SystemFaceInfo msg = new CMD_GP_SystemFaceInfo();
            msg.wFaceID = readBuffer.ReadUint();
            msg.dwUserID = readBuffer.ReadUint();
            msg.szPassword = readBuffer.ReadString(66);
            msg.szMachineID = readBuffer.ReadString(66);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        private bool HandleChangeNameResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_ChangeNickNameResult msg = new CMD_GP_ChangeNickNameResult(rb);

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.ChangeNickNameResult(msg);
            }
            return true;
        }


        /// <summary>
        /// 用户头像
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleUserFaceInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserFaceInfo msg = new CMD_GP_UserFaceInfo();
            msg.wFaceID = readBuffer.ReadUint();
            msg.dwCustomID = readBuffer.ReadUint();

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 魅力查询成功
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleQueryCharmSuccess(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_QueryCharmSuccess msg = new CMD_GP_QueryCharmSuccess();
            msg.dwUserID = readBuffer.ReadByte();
            msg.lLessCharm = readBuffer.ReadLong();
            msg.lAllChangedCharm = readBuffer.ReadLong();
            msg.lUserInsure = readBuffer.ReadLong();

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleModifyUnderWrite(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_ModifyUnderWrite msg = new CMD_GP_ModifyUnderWrite();
            msg.dwUserID = readBuffer.ReadByte();
            msg.szPassword = readBuffer.ReadString(66);
            msg.szUnderWrite = readBuffer.ReadString(64);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 绑定机器
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleModifyMachine(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_ModifyMachine msg = new CMD_GP_ModifyMachine();
            msg.cbBind = readBuffer.ReadByte();
            msg.dwUserID = readBuffer.ReadUint();
            msg.szPassword = readBuffer.ReadString(66);
            msg.szMachineID = readBuffer.ReadString(66);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleModifyLogonPass(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_ModifyLogonPass msg = new CMD_GP_ModifyLogonPass();
            msg.dwUserID = readBuffer.ReadUint();
            msg.szDesPassword = readBuffer.ReadString(66);
            msg.szScrPassword = readBuffer.ReadString(64);
            // msg.szSafeAnswer = readBuffer.ReadString(66);

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }



        /// <summary>
        /// 签到信息
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleCheckInInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_CheckInInfo msg = new CMD_GP_CheckInInfo();
            msg.wSeriesDate = readBuffer.ReadUshort();
            msg.bTodayChecked = readBuffer.ReadBoolean();

            msg.lRewardGold = new long[LEN_WEEK];
            msg.dwPropImageID = new uint[LEN_WEEK];
            msg.szPropName = new string[LEN_WEEK];
            for (int i = 0; i < LEN_WEEK; i++)
            {
                msg.lRewardGold[i] = readBuffer.ReadLong();
            }
            for (int i = 0; i < LEN_WEEK; i++)
            {
                msg.dwPropImageID[i] = readBuffer.ReadUint();
            }
            for (int j = 0; j < LEN_WEEK; j++)
            {
                msg.szPropName[j] = readBuffer.ReadString(64);
            }
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.CheckInfo(msg);
            }
            // if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }
        private bool HandleCheckInInfoNew(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_CheckInInfo_NEW msg = new CMD_GP_CheckInInfo_NEW();
            msg.szPropDescribe = new string[LEN_WEEK];
            for (int i = 0; i < LEN_WEEK; i++)
            {
                msg.szPropDescribe[i] = readBuffer.ReadString(256);
            }
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.CheckInfoNew(msg);
            }
            // if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }


        /// <summary>
        /// 签到结果
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleCheckInResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_CheckInResult msg = new CMD_GP_CheckInResult();
            msg.bSuccessed = readBuffer.ReadBoolean();
            msg.lScore = readBuffer.ReadLong();
            msg.szNotifyContent = readBuffer.ReadString();

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.CheckInResult(msg);
            }
            // readBuffer.ReadMsg(msg);
            //if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 道具信息
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandlePropInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_PROPINFO msg = new CMD_GP_PROPINFO();
            msg.dwPropID = readBuffer.ReadUint();
            msg.dwUseType = readBuffer.ReadUint();
            msg.dwPropType = readBuffer.ReadUint();
            msg.dwPropValue = readBuffer.ReadUint();
            msg.dwLevelLimit = readBuffer.ReadUint();
            msg.dwImageID = readBuffer.ReadUint();
            msg.dwShopID = readBuffer.ReadUint();
            msg.dwPropPrize = readBuffer.ReadUint();
            msg.dwPropRebate = readBuffer.ReadUint();
            msg.dwPropBeyond = readBuffer.ReadUint();
            msg.bCanBeAdd = readBuffer.ReadByte();
            msg.dwFlID = readBuffer.ReadUint();
            msg.strPropName = readBuffer.ReadString(64);
            msg.strDescribe = readBuffer.ReadString(256);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.PropInfo(msg);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }
        /// <summary>
        /// 道具商城请求完成
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandlePropFinish(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.PropInfoFinish();
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 道具商城购买成功
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleBuyPropSuccess(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.BuyPropSuccess();
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }

        /// <summary>
        /// 道具商城购买失败
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleBuyPropFailed(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.BuyPropFailed();
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }

        /// <summary>
        /// 背包信息
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleBagInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_BagPropInfoMsg msg = new CMD_GP_BagPropInfoMsg();
            msg.cbStatus = readBuffer.ReadByte();
            msg.wBind = readBuffer.ReadUshort();
            msg.dwIndexID = readBuffer.ReadUint();
            msg.dwPropID = readBuffer.ReadUint();
            msg.dwUseType = readBuffer.ReadUint();
            msg.dwPropType = readBuffer.ReadUint();
            msg.dwPropValue = readBuffer.ReadUint();
            msg.dwImageID = readBuffer.ReadUint();
            msg.dwUseTimes = readBuffer.ReadUint();
            msg.dwLevelLimit = readBuffer.ReadUint();
            msg.dwRemainDay = readBuffer.ReadUint();
            msg.dwSpecialValue = readBuffer.ReadUint();
            msg.strSpecialInfo = readBuffer.ReadString(256);
            msg.EndTime = readBuffer.ReadDateTime();
            msg.dwFlID = readBuffer.ReadUint();
            msg.bCanBeAdd = readBuffer.ReadByte();
            msg.strPropName = readBuffer.ReadString(64);
            msg.strDescribe = readBuffer.ReadString(256);

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.BagInfo(msg);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }
        /// <summary>
        /// 背包请求完成
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleBagFinish(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.BagInfoFinish();
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        private bool UseBagGiftResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            int count = readBuffer.Length / 80;
            CMD_GP_SimpleGiftResultProp[] all = new CMD_GP_SimpleGiftResultProp[count];
            for (int i = 0; i < count; i++)
            {
                CMD_GP_SimpleGiftResultProp msg = new CMD_GP_SimpleGiftResultProp();
                msg.dwPropID = readBuffer.ReadUint();
                msg.iPropNum = readBuffer.ReadLong();
                msg.dwImageID = readBuffer.ReadUint();
                msg.strPropName = readBuffer.ReadString(64);
                all[i] = msg;
            }


            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.RequestBagResult(all);
            }
            return true;
        }
        /// <summary>
        /// 使用背包道具结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleUseBagPropResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UsePropResult msg = new CMD_GP_UsePropResult();
            msg.wUseResult = readBuffer.ReadUshort();
            msg.dwPropType = readBuffer.ReadUint();
            msg.dwValue = readBuffer.ReadUint();
            msg.strValue = readBuffer.ReadString(256);
            msg.bUpdate = readBuffer.ReadBoolean();

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.UseBagPorpResult(msg);
            }

            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msg);
            return true;
        }

        /// <summary>
        /// 删除背包道具结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleDeleteBagPropResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            byte Bscu = readBuffer.ReadByte();
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.DeleteBagPorpResult(Bscu);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 邮件数据
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleMailInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_MailData msg = new CMD_GP_MailData();
            msg.dwMailID = readBuffer.ReadUint();
            msg.dwSenderID = readBuffer.ReadUint();
            msg.dwReceiverID = readBuffer.ReadUint();
            msg.dwPropID = readBuffer.ReadUint();
            msg.wPropNum = readBuffer.ReadUshort();
            msg.dwImageID = readBuffer.ReadUint();
            msg.strInformation = readBuffer.ReadString(512);
            msg.strName = readBuffer.ReadString(64);
            msg.wIsRead = readBuffer.ReadUshort();
            msg.BeginTime = readBuffer.ReadDateTime();
            msg.wLimitDays = readBuffer.ReadUshort();
            msg.wMailType = readBuffer.ReadUshort();
            msg.SenderNickname = readBuffer.ReadString(64);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.MailInfo(msg);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 邮件数据完成
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleMailInfoFinish(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.MailInfoFinish();
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 发送邮件失败
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleSendMailFailure(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_SendMailFailure msgInfo = new CMD_GP_SendMailFailure();
            msgInfo.lResultCode = readBuffer.ReadInt();
            msgInfo.szDescribeString = readBuffer.ReadString();
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.SendMailFailure(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleSendMailUseInfoResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_UserMailUserInfo msgInfo = new CMD_GP_UserMailUserInfo();
            msgInfo.dwTargetGameID = readBuffer.ReadUint();
            msgInfo.szAccounts = readBuffer.ReadString(64);
            msgInfo.dwTargetRealGameID = readBuffer.ReadUint();
            msgInfo.dwVipLevel = readBuffer.ReadUint();
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.SendMailUseInfoResult(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 发送邮件结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleSendMailResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_SendMailResult msgInfo = new CMD_GP_SendMailResult();
            msgInfo.lResultCode = readBuffer.ReadInt();
            msgInfo.szDescribeString = readBuffer.ReadString();
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.SendMailResult(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 发送邮件结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleSendOperationResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_MailOperation_Result msgInfo = new CMD_GP_MailOperation_Result();
            msgInfo.lOperCode = readBuffer.ReadInt();
            msgInfo.dwMailID = readBuffer.ReadUint();
            msgInfo.dwoperatorID = readBuffer.ReadUint();
            msgInfo.lResultCode = readBuffer.ReadInt();
            msgInfo.szDescribeString = readBuffer.ReadString(256);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.OperationResult(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }

        /// <summary>
        /// CDKey结果
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleRequestCDKeyResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_RequestCdkeyResult msgInfo = new CMD_GP_RequestCdkeyResult(rb);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.RequestCDKeyResult(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, rb);
            return true;
        }

        /// <summary>
        /// 跑马灯数据
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HanldeMarqueeInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_MarqueeMsg msgInfo = new CMD_GP_MarqueeMsg();
            msgInfo.strInfomation = readBuffer.ReadString(256);
            msgInfo.strInfomation = StringUtil.TrimUnusefulChar(msgInfo.strInfomation);
            msgInfo.wCycleNum = readBuffer.ReadUshort();
            msgInfo.cbType = readBuffer.ReadByte();
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.MarqueeInfo(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 跑马灯数据完成
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HanldeMarqueeInfoFinish(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.MarqueeInfoFinish();
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }


        /// <summary>
        /// 等级参数
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleGrowLevelParameter(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_GrowLevelParameter msgInfo = new CMD_GP_GrowLevelParameter();
            msgInfo.wCurrLevelID = readBuffer.ReadUshort();
            msgInfo.dwExperience = readBuffer.ReadUint();
            msgInfo.dwCurgradeExperience = readBuffer.ReadUint();
            msgInfo.dwUpgradeExperience = readBuffer.ReadUint();
            msgInfo.lUpgradeRewardGold = readBuffer.ReadLong();
            msgInfo.lUpgradeRewardIngot = readBuffer.ReadLong();
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.GrowLevelParameter(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 等级升级
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleGrowlevelUpgrade(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_GrowLevelUpgrade msgInfo = new CMD_GP_GrowLevelUpgrade();
            //msgInfo.lCurrScore = readBuffer.ReadLong();
            //msgInfo.lCurrIngot = readBuffer.ReadLong();
            //msgInfo.szNotifyContent = readBuffer.ReadString(256);
            //if (MessageMgr.DT_Interface != null)
            //{
            //    MessageMgr.DT_Interface.GrowlevelUpgrade(msgInfo);
            //}
            //if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 刷新金币
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleRefreshMoneyResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_RefreshMoneyResult msgInfo = new CMD_GP_RefreshMoneyResult();
            msgInfo.lScore = readBuffer.ReadLong();
            msgInfo.lInsure = readBuffer.ReadLong();
            msgInfo.dwUserMedal = readBuffer.ReadUint();
            msgInfo.dBeans = readBuffer.ReadDouble();
          //  if (DC.channel == EChannel.DA_HENG_BengBeng || DC.channel == EChannel.DA_HENG_xianwan)
                msgInfo.isRechargeFirst = readBuffer.ReadUshort();
            msgInfo.szNotifyContent = readBuffer.ReadString(256);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.RefreshMoneyResult(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 查询抽奖信息
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleDayrewardInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_PROPINFO msgInfo = new CMD_GP_PROPINFO();
            msgInfo.dwPropID = readBuffer.ReadUint();
            msgInfo.dwUseType = readBuffer.ReadUint();
            msgInfo.dwPropType = readBuffer.ReadUint();
            msgInfo.dwPropValue = readBuffer.ReadUint();
            msgInfo.dwLevelLimit = readBuffer.ReadUint();
            msgInfo.dwImageID = readBuffer.ReadUint();
            msgInfo.dwShopID = readBuffer.ReadUint();
            msgInfo.dwPropPrize = readBuffer.ReadUint();
            msgInfo.dwPropRebate = readBuffer.ReadUint();
            msgInfo.dwPropBeyond = readBuffer.ReadUint();
            msgInfo.bCanBeAdd = readBuffer.ReadByte();
            msgInfo.dwFlID = readBuffer.ReadUint();
            msgInfo.strPropName = readBuffer.ReadString(64);
            msgInfo.strDescribe = readBuffer.ReadString(256);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.DayrewardInfo(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 抽奖次数返回
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleDayrewardCount(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_DayRewardInfo msgInfo = new CMD_GP_DayRewardInfo();
            msgInfo.wSeriesTodayReward = readBuffer.ReadUshort();
            msgInfo.wFreeCount = readBuffer.ReadUshort();
            msgInfo.dwUseTimes = readBuffer.ReadUshort();
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.DayrewardCount(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }
        /// <summary>
        /// 抽奖结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleDayrewardResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_DayRewardResult msgInfo = new CMD_GP_DayRewardResult();
            msgInfo.bSuccessed = readBuffer.ReadBoolean();
            msgInfo.wSeriesTodayReward = readBuffer.ReadUshort();
            msgInfo.dwPropID = readBuffer.ReadUint();
            msgInfo.DayRewardTypeID = readBuffer.ReadUint();
            msgInfo.szNotifyContent = readBuffer.ReadString();
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.DayrewardResult(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }


        /// <summary>
        /// 开通银行结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleInsureEnableResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GR_S_UserInsureEnableResult msgInfo = new CMD_GR_S_UserInsureEnableResult();
            msgInfo.cbActivityGame = readBuffer.ReadByte();
            msgInfo.cbInsureEnabled = readBuffer.ReadByte();
            msgInfo.szDescribeString = readBuffer.ReadString();

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.InsureEnableResult(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, readBuffer);
            return true;
        }



        /// <summary>
        /// 排行榜
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleWealthRankTopResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            const int SIZE = 4 + 64 + 2 + 8 + 4 + 64;
            int count = rb.Length / SIZE;
            CMD_GP_WealthRankTopResult[] cmdList = new CMD_GP_WealthRankTopResult[count];
            for (int i = 0; i < count; i++)
            {
                CMD_GP_WealthRankTopResult cmd = new CMD_GP_WealthRankTopResult();
                cmd.dwRank = rb.ReadUint();
                cmd.szNickName = rb.ReadString(64);
                cmd.wFaceID = rb.ReadUshort();
                cmd.lScore = rb.ReadLong();
                cmd.iVipLvl = rb.ReadInt();
                cmd.szDeclaration = rb.ReadString(64);
                cmdList[i] = cmd;
            }

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.WealthRankTopResult(cmdList);
            }
            return true;
        }


        /// <summary>
        /// 税收比例查询结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleQueryTaxResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_QueryTaxResult cmd = new CMD_GP_QueryTaxResult();
            cmd.iqueryType = rb.ReadInt();
           // if (DC.nowPlatform == EPlatform.GameHuaErJie)
                cmd.iGoldMult = rb.ReadInt();

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.TaxInfo(cmd);
            }
            return true;
        }

        /// <summary>
        /// 税收详情查询结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleQueryTaxDetailResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {

            CMD_GP_QueryTaxDetailResult cmd = new CMD_GP_QueryTaxDetailResult();
            cmd.lTotalScore = rb.ReadLong();
            cmd.lEnableScore = rb.ReadLong();
            cmd.iStartIndex = rb.ReadInt();
            cmd.iEndIndex = rb.ReadInt();
            cmd.iMaxIndex = rb.ReadInt();
            cmd.iThisTurnLength = rb.ReadInt();
            cmd.trRecord = new TaxDetailRecordVO[cmd.iThisTurnLength];
            for (var i = 0; i < cmd.trRecord.Length; i++)
            {
                TaxDetailRecordVO vo = new TaxDetailRecordVO();
                vo.iGameID = rb.ReadInt();
                vo.lTaxScore = rb.ReadLong();
                vo.tRecordTime = rb.ReadDateTime();
                vo.iStatus = rb.ReadInt();
                cmd.trRecord[i] = vo;
            }

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.QueryTaxDetailResult(cmd);
            }
            return true;
        }

        /// <summary>
        /// 领取税收结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleGetTaxResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_GetTaxResult cmd = new CMD_GP_GetTaxResult();
            cmd.iErrorCode = rb.ReadInt();
            cmd.lGetScore = rb.ReadLong();

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.GetTaxResult(cmd);
            }
            return true;
        }

        /// <summary>
        /// 银行快捷操作结果
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="rb"></param>
        /// <returns></returns>
        private bool BankQuickOperaResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_BankQuickOperateResult cmd = new CMD_GP_BankQuickOperateResult();
            cmd.iOperaResult = rb.ReadInt();
            cmd.iOperaUserID = rb.ReadUint();

            cmd.iOperaData = new long[ConstDefine.LEN_BANK_QUICK_MAX];
            for (var i = 0; i < ConstDefine.LEN_BANK_QUICK_MAX; i++)
            {
                cmd.iOperaData[i] = rb.ReadLong();
            }

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.BankQuickOperateResult(cmd);
            }
            return true;
        }

        /// <summary>
        /// 代理查询结果
        /// </summary>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleAgentResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_AgentReply cmd = new CMD_GP_AgentReply();
            cmd.iShowNum = rb.ReadInt();
            cmd.asShowContent = new AgentStructVO[cmd.iShowNum];
            for (var i = 0; i < cmd.iShowNum; i++)
            {
                AgentStructVO vo = new AgentStructVO();
                vo.dwUserID = rb.ReadUint();
                vo.dwGameID = rb.ReadUint();
                vo.szNickName = rb.ReadString(64);
                vo.szNotice = rb.ReadString(64);
                cmd.asShowContent[i] = vo;
            }

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.AgentResult(cmd);
            }
            return true;
        }

        /// <summary>
        /// 充值返回订单
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="rb"></param>
        /// <returns></returns>
        private bool HandleRequestRechargeOrderResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
           // if (DC.nowPlatform == EPlatform.Game3322)
            //{
                CMD_GP_RequestRechargeOrderResult_3322 msg = new CMD_GP_RequestRechargeOrderResult_3322(rb);
                if (MessageMgr.DT_Interface != null)
                {
                    MessageMgr.DT_Interface.RechargeOrderResult(msg);
                }
           // }
           // else
          //  {
              //  CMD_GP_RequestRechargeOrderResult msg = new CMD_GP_RequestRechargeOrderResult(rb);
               // if (MessageMgr.DT_Interface != null)
              //  {
                  //  MessageMgr.DT_Interface.RechargeOrderResult(msg);
               // }
           // }
            return true;
        }

        bool HandleRequestRechargeAlipayResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            var msg = new CMD_GP_RequestRechargeOrderAlipayResult(rb);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.RequestRechargeAlipayResult(msg);
            }
            return true;
        }

        bool HandleRequestRechargeWeixinResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            var msg = new CMD_GP_RequestRechargeOrderWeiXinResult(rb);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.RequestRechargeWeixinResult(msg);
            }
            return true;
        }

        /// <summary>
        /// 心跳返回
        /// </summary>
        /// <returns></returns>
        private bool HandleRequestHeartBeatResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_HeartBeatResult msg = new CMD_GP_HeartBeatResult();
            msg.iIsOnline = rb.ReadInt();

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.HeartBeatBack(msg);
            }
            return true;
        }

        /// <summary>
        /// 收到跑马灯
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="rb"></param>
        /// <returns></returns>
        private bool HandleMarqueeNewBack(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_MarqueeMsgNew msg = new CMD_GP_MarqueeMsgNew(rb);
            Loom.QueueOnMainThread(() =>
            {
              //  NoticeManager.SendNoticeQuick(HallNotice.NEW_MARQUREE, new MarqueeDataVO(msg));
            });
            return true;
        }

        private bool HandleMarqueeNewFinsh(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            return true;
        }
    }
}
