using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Struct;
using MessagePackage.Operation;
using UnityEngine;
using System.IO;

namespace MessagePackage
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LogonMessageMgr : DelegateMsgBase
    {
        public override void Error(int status, string errStr)
        {
            Loom.QueueOnMainThread(() =>
            {
                Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&& " + errStr + "  " + MessageMgr.CurMsgMgr.NetHallErrorNotify);
                if (MessageMgr.DT_Interface != null)
                {
                    MessageMgr.DT_Interface.Error(errStr, status);
                }
            }
            );
        }

        public override bool HandleReadNetData(ushort mnCmd, ushort sbCmd, CyNetReadBuffer readbuffer)
        {
            Loom.QueueOnMainThread(() =>
            {
                //Debug.LogFormat("收到登录服务器协议： {0}   {1}", mnCmd, sbCmd);
                CyNetReadBuffer TP = new CyNetReadBuffer(readbuffer.Length - 8);
                //             for (int i = 0; i < readbuffer.Length - 8; i++)
                //             {
                //                 TP.NetBuffer[i] = readbuffer.NetBuffer[i + 8];
                //             }
                TP.Length = readbuffer.Length - 8;
                Array.Copy(readbuffer.NetBuffer, 8, TP.NetBuffer, 0, TP.Length);
                if (MessageMgr.DT_Interface != null)
                {
                    MessageMgr.DT_Interface.CMD_(mnCmd, sbCmd);
                }

                switch (mnCmd)
                {
                    case ConstDefine.MDM_GP_LOGON:
                        HandleLogonMsg(mnCmd, sbCmd, TP);
                        break;
                    case ConstDefine.MDM_GP_SERVER_LIST:
                        HandleListMsg(mnCmd, sbCmd, TP);
                        break;
                    case ConstDefine.MDM_GP_USER_SERVICE:
                        HandleUserServerMsg(mnCmd, sbCmd, TP);
                        break;
                    case ConstDefine.MDM_GP_MARQUEE:
                        HanldeMarqueeMsg(mnCmd, sbCmd, TP);
                        break;
                    case ConstDefine.MDM_GP_PROP:
                        HandlePropMsg(mnCmd, sbCmd, TP);
                        break;
                    case ConstDefine.MDM_GP_PLATFORM:
                        HandlePlatformMsg(mnCmd, sbCmd, TP);
                        break;
                    case ConstDefine.MDM_GP_BAG:
                        HandleBagMsg(mnCmd, sbCmd, TP);
                        break;
                    default:
                        Error(0, "错误HandleReadNetData收到了没有进行解析的主命令" + sbCmd);
                        break;
                }
            });
            return false;
        }

        /// <summary>
        /// 列表命令解析
        /// </summary>
        /// <param name="mnCmd"></param>
        /// <param name="sbCmd"></param>
        /// <param name="tP"></param>
        /// <returns></returns>
        private bool HandleListMsg(ushort mnCmd, ushort sbCmd, CyNetReadBuffer tP)
        {
            Console.WriteLine("收到列表命令");
            switch (sbCmd)
            {
                case ConstDefine.SUB_GP_LIST_TYPE:
                    return HandleGameType(tP);
                case ConstDefine.SUB_GP_LIST_KIND:
                    return HandleGameKind(tP);
                case ConstDefine.SUB_GP_LIST_NODE:
                    return true;
                case ConstDefine.SUB_GP_LIST_PAGE:
                    return true;
                case ConstDefine.SUB_GP_LIST_SERVER:
                    return HandleGameRoomType(tP);
                case ConstDefine.SUB_GP_VIDEO_OPTION:
                    return true;
                case ConstDefine.SUB_GP_LIST_MATCH:
                    return HandleGameMatch(tP);
                case UserOperations.SUB_GP_BACK_GAME_JACKPOT:
                    return JackpotResult(mnCmd, sbCmd, tP);
                case ConstDefine.SUB_GP_LIST_FINISH:
                    return HandleListFinish();
                case ConstDefine.SUB_GP_SERVER_FINISH:
                    return true;
                default:
                    Error(0, "错误HandleListMsg收到了子命令" + sbCmd);
                    return true;
            }
        }
        public bool HandleListFinish()
        {
            Console.WriteLine("列表接受完毕");
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.LogonList();
            }
            return true;
        }

        public bool HandleGameRoomType(CyNetReadBuffer buffer)
        {
            // int size = buffer.Length / 174;
            int size = buffer.Length / 176;
            for (int i = 0; i < size; ++i)
            {
                tagGameServer room = new tagGameServer();

                room.wKindID = buffer.ReadUshort();
                room.wNodeID = buffer.ReadUshort();
                room.wSortID = buffer.ReadUshort();
                room.wServerID = buffer.ReadUshort();
                room.wServerKind = buffer.ReadUshort();
                room.wServerType = buffer.ReadUshort();
                room.wServerPort = buffer.ReadUshort();
                room.lCellScore = buffer.ReadLong();
                room.lEnterScore = buffer.ReadLong();
                room.dwServerRule = buffer.ReadUint();
                room.dwOnLineCount = buffer.ReadUint();
                room.dwAndroidCount = buffer.ReadUint();
                room.dwFullCount = buffer.ReadUint();
                room.szServerAddr = buffer.ReadString(64);
                room.szServerName = buffer.ReadString(64);
                room.wNowState = buffer.ReadUshort();
                MessageMgr.CurMsgMgr.Dic_GameRoom[room.wServerID] = room;
                Console.WriteLine("游戏IP:{0} 房间名称:{1}  ###{2}", room.szServerAddr, room.szServerName, room.wServerPort);
            }
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.LogonListRoom(MessageMgr.CurMsgMgr.Dic_GameRoom);
            }
            return true;
        }

        public bool HandleGameType(CyNetReadBuffer buffer)
        {
            int size = buffer.Length / 70;
            for (int i = 0; i < size; ++i)
            {
                tagGameType type = new tagGameType();
                type.wJoinID = buffer.ReadUshort();
                type.wSortID = buffer.ReadUshort();
                type.wTypeID = buffer.ReadUshort();
                type.wHeatNum = buffer.ReadUshort();
                type.szTypeName = buffer.ReadString(64);
                MessageMgr.CurMsgMgr.Dic_GameType[type.wTypeID] = type;
                Console.WriteLine("游戏类型:{0} 类型名称:{1}", type.wTypeID, type.szTypeName);
            }
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.LogonListType(MessageMgr.CurMsgMgr.Dic_GameType);
            }
            return true;
        }
        public bool HandleGameKind(CyNetReadBuffer buffer)
        {
            int size = buffer.Length / 150;
            for (int i = 0; i < size; ++i)
            {
                tagGameKind kind = new tagGameKind();
                kind.wTypeID = buffer.ReadUshort();
                kind.wJoinID = buffer.ReadUshort();
                kind.wSortID = buffer.ReadUshort();
                kind.wKindID = buffer.ReadUshort();
                kind.wGameID = buffer.ReadUshort();
                kind.wGameTypeID = buffer.ReadUshort();
                kind.wHeatNum = buffer.ReadUshort();
                kind.dwOnLineCount = buffer.ReadUint();
                kind.dwAndroidCount = buffer.ReadUint();
                kind.dwFullCount = buffer.ReadUint();
                kind.szKindName = buffer.ReadString(64);
                kind.szProcessName = buffer.ReadString(64);
                //网狐的MFC进程名替换为uinty3d的场景名称
                kind.szProcessName = Path.GetFileNameWithoutExtension(kind.szProcessName);
                MessageMgr.CurMsgMgr.Dic_GameKind[kind.wKindID] = kind;
                Debug.LogFormat("收到游戏数据 [id:{0} name:{1} key:{2}] ", kind.wKindID, kind.szKindName, kind.szProcessName);
            }
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.LogonListKind(MessageMgr.CurMsgMgr.Dic_GameKind);
            }
            return true;
        }
        public bool HandleGameMatch(CyNetReadBuffer buffer)
        {
            int size = buffer.Length / 191;
            for (int i = 0; i < size; i++)
            {
                tagGameMatch Match = new tagGameMatch();
                Match.wServerID = buffer.ReadUshort();
                Match.dwMatchID = buffer.ReadUint();
                Match.dwMatchNO = buffer.ReadUint();
                Match.cbMatchType = buffer.ReadByte();
                Match.szMatchName = buffer.ReadString(64);
                Match.cbMemberOrder = buffer.ReadByte();
                Match.cbMatchFeeType = buffer.ReadByte();
                Match.lMatchFee = buffer.ReadLong();
                Match.szPropName = buffer.ReadString(64);
                Match.dwImageID = buffer.ReadUint();
                Match.wStartUserCount = buffer.ReadUshort();
                Match.wMatchPlayCount = buffer.ReadUshort();
                Match.wRewardCount = buffer.ReadUshort();
                Match.MatchStartTime = buffer.ReadDateTime();
                Match.MatchEndTime = buffer.ReadDateTime();
                MessageMgr.CurMsgMgr.Dic_GameMatch[Match.wServerID] = Match;
                Console.WriteLine("比赛名称:", Match.szMatchName);
            }
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.LogonListMatch(MessageMgr.CurMsgMgr.Dic_GameMatch);
            }

            return true;
        }

        /// <summary>
        /// 登陆响应（返回消息）
        /// </summary>
        /// <param name="maincmd">主命令 MDM_</param>
        /// <param name="subCmd">子命令 SUB_ </param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleLogonMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case ConstDefine.SUB_GP_REQUEST_VCCODE_RESULT:
                    return HandleVerificationCodeResult(readbuffer);

                case ConstDefine.SUB_GP_LOGON_FAILURE:
                    return HandleActionFailed(readbuffer);

                case ConstDefine.SUB_GP_LOGON_FINISH:
                    return HandleLogonFinish(maincmd, subCmd, readbuffer);

                case ConstDefine.SUB_GP_QUICK_LOGON_BACK:
                    return QuickLoginBackHandler(maincmd, subCmd, readbuffer);

                case ConstDefine.SUB_GP_REQUEST_PHONE_VCCODE_RESULT:
                    return RequestPhoneVCCodeResultHandler(maincmd, subCmd, readbuffer);

                case ConstDefine.SUB_GP_BIND_PHONE_BACK:
                    return BindPhoneBackHandler(maincmd, subCmd, readbuffer);

                case ConstDefine.SUB_GP_RESET_PASSWORD_BACK:
                    return ResetPasswordBackHandler(maincmd, subCmd, readbuffer);

                case ConstDefine.SUB_GP_LOGON_SUCCESS:
                    return HandleLogonSucc(maincmd, subCmd, readbuffer);

                case ConstDefine.SUB_GP_GROWLEVEL_CONFIG:
                    return HandleGrowLevelConfig(maincmd, subCmd, readbuffer);

                case ConstDefine.SUB_GP_VALIDATE_MBCARD:
                    {
                        Console.WriteLine("登录失败 请查看框架层");
                    }
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 用户服务（响应）
        /// </summary>
        /// <param name="maincmd">主命令 MDM_</param>
        /// <param name="subCmd">子命令 SUB_ </param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleUserServerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case UserOperations.SUB_GP_OPERATE_SUCCESS:
                    return HandleOperateSuccess(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_OPERATE_FAILURE:
                    return HandleOperateFailure(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_REQUEST_HEARTBEAT_RESULT:
                    return HandleRequestHeartBeatResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_USER_INDIVIDUAL:
                    return HandleUserIndividual(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_MODIFY_MACHINE:
                    return HandleModifyMachine(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_MODIFY_LOGON_PASS:
                    return HandleModifyLogonPass(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_MODIFY_UNDER_WRITE:
                    return HandleModifyUnderWrite(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_USER_FACE_INFO:
                    return HandleUserFaceInfo(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_SYSTEM_FACE_INFO:
                    return HandleSystemFaceInfo(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_CHANGE_NAME_RESULT:
                    return HandleChangeNameResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_QUERY_INDIVIDUAL:
                    return HandleQueryIndividual(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_USER_SAVE_SCORE:
                    return HandleUserSaveScore(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_USER_TAKE_SCORE:
                    return HandleUserTakeScore(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_USER_TRANSFER_SCORE:
                    return HandleUserTransferScore(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_USER_INSURE_INFO:
                    return HandleUserInsureInfo(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_QUERY_INSURE_INFO:
                    return HandleQueryInsureInfo(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_USER_INSURE_SUCCESS:
                    return HandleUserInsureSuccess(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_USER_INSURE_FAILURE:
                    return HandleUserInsureFailure(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_QUERY_USER_INFO_REQUEST:
                    return HandleQueryUserInfoRequest(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_QUERY_USER_INFO_RESULT:
                    return HandleUserTransferUserInfo(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_TRANSFER_RECORD_RESULT:
                    return HandleUserTransferRecordResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_BASEENSURE_PARAMETER:
                    return HandleUserBaseensure(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_BASEENSURE_RESULT:
                    return HandleUserBaseensureResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_CHECKIN_INFO:
                    return HandleCheckInInfo(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_CHECKIN_RESULT:
                    return HandleCheckInResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_CHECKIN_INFO_NEW:
                    return HandleCheckInInfoNew(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_GROWLEVEL_PARAMETER:
                    return HandleGrowLevelParameter(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_GROWLEVEL_UPGRADE:
                    return HandleGrowlevelUpgrade(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_REFRESH_MONEY_RESULT:
                    return HandleRefreshMoneyResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_DAYREWARD_INFO:
                    return HandleDayrewardInfo(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_DAYREWARD_COUNT:
                    return HandleDayrewardCount(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_DAYREWARD_RESULT:
                    return HandleDayrewardResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_USER_INSURE_ENABLE_RESULT:
                    return HandleInsureEnableResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_WEALTH_RANK_TOP_RESULT:
                    return HandleWealthRankTopResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_QUERY_TAX_RESULT:
                    return HandleQueryTaxResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_QUERY_TAX_DETAIL_RESULT:
                    return HandleQueryTaxDetailResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_GET_TAX_RESULT:
                    return HandleGetTaxResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_BANK_QUICK_OPERA_RESULT:
                    return BankQuickOperaResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_REQUEST_RECHARGE_RESULT:
                    return HandleRequestRechargeOrderResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_REQUEST_RECHARGE_ALIPAY_RESULT:
                    return HandleRequestRechargeAlipayResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_REQUEST_RECHARGE_WEIXIN_RESULT:
                    return HandleRequestRechargeWeixinResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_REQUEST_AGENT_RESULT:
                    return HandleAgentResult(maincmd, subCmd, readbuffer);

                case UserOperations.SUB_GP_MODIFY_LOGON_PASS_BACK:
                    return ChangePwdBackHandler(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_LOTTERY_CHANCE_BACK:
                    return ResidueTimes(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_LOTTERY_ITEM_BACK:
                    return LotteryItemInfo(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_LOTTERY_RESULT_BACK:
                    return LotteryResult(maincmd, subCmd, readbuffer);


                case UserOperations.SUB_GP_SELF_LOTTERY_RECORD_BACK:
                    return SelfLotteryRecord(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_WHOLE_LOTTERY_RECORD_BACK:
                    return AllLotteryRecord(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_RECEIVE_LOTTERY_BACK:
                    return ReceiveLotteryBack(maincmd, subCmd, readbuffer);
            }
            return false;
        }
        /// <summary>
        /// 剩余次数
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool ResidueTimes(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GP_LotteryChanceBack msg = new CMD_GP_LotteryChanceBack();
            msg.iLeftLotteryTimes = readbuffer.ReadInt();
           // NoticeManager.SendNoticeQuick(HallNotice.RESIDUETIMES, msg);
            return true;
        }
        /// <summary>
        /// 抽奖物品信息
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool LotteryItemInfo(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GP_LotteryItemBack msg = new CMD_GP_LotteryItemBack();
            msg.lItemData = new Struct.LotteryItemInfo[8];
            for (int i = 0; i < 8; i++)
            {
                LotteryItemInfo lii = new Struct.LotteryItemInfo(readbuffer);
                msg.lItemData[i] = lii;
            }
         
            //NoticeManager.SendNoticeQuick(HallNotice.LOTTERYITEMINFO, msg);
            return true;
        }
        /// <summary>
        /// 抽奖结果返回
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool LotteryResult(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GP_RequestLotteryBack msg = new CMD_GP_RequestLotteryBack(readbuffer);
           // NoticeManager.SendNoticeQuick(HallNotice.LOTTERYRESULT, msg);
            return true;
        }
        /// <summary>
        /// 自身抽奖记录
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool SelfLotteryRecord(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GP_SelfLotteryRecordBack msg = new CMD_GP_SelfLotteryRecordBack(readbuffer);
           // NoticeManager.SendNoticeQuick(HallNotice.SELFRECORD, msg);

            return true;
        }
        /// <summary>
        /// 所有抽奖记录
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool AllLotteryRecord(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GP_WholeLotteryRecordBack msg = new CMD_GP_WholeLotteryRecordBack(readbuffer);
          //  NoticeManager.SendNoticeQuick(HallNotice.ALLRECORD, msg);
            return true;
        }
        /// <summary>
        /// 领取奖励返回
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool ReceiveLotteryBack(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GP_ReceiveLotteryBack msg = new CMD_GP_ReceiveLotteryBack(readbuffer);
          //  NoticeManager.SendNoticeQuick(HallNotice.RECEIVEBACK, msg);
            return true;
        }
        /// <summary>
        /// 道具商城消息
        /// </summary>
        /// <param name="maincmd">主命令 MDM_</param>
        /// <param name="subCmd">子命令 SUB_ </param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandlePropMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case UserOperations.SUB_GP_PROP_DATA_BACK:
                    return HandlePropInfo(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_PROP_DATA_FINISH:
                    return HandlePropFinish(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_PROP_BUY_SUCCESS:
                    return HandleBuyPropSuccess(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_PROP_BUY_FAILED:
                    return HandleBuyPropFailed(maincmd, subCmd, readbuffer);
            }
            return false;
        }

        /// <summary>
        /// 大厅消息
        /// </summary>
        /// <param name="maincmd">主命令 MDM_</param>
        /// <param name="subCmd">子命令 SUB_ </param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandlePlatformMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case UserOperations.SUB_GP_MAIL_DATA_BACK:
                    return HandleMailInfo(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_MAIL_DATA_BACK_FINISH:
                    return HandleMailInfoFinish(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_SENDMAIL_FAILURE:
                    return HandleSendMailFailure(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_SENDMAIL_USERINFO_RESULT:
                    return HandleSendMailUseInfoResult(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_SEND_MAIL_RESULT:
                    return HandleSendMailResult(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP__MAIL_OPERATION_RESULT:
                    return HandleSendOperationResult(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_REQUEST_CDKEY_BACK:
                    return HandleRequestCDKeyResult(maincmd, subCmd, readbuffer);

            }
            return true;
        }
        private bool JackpotResult(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            int count = readbuffer.Length / 20;
            CMD_GP_ServerUpdateJackpot[] msgall = new CMD_GP_ServerUpdateJackpot[count];
            for (int i = 0; i < count; i++)
            {
                CMD_GP_ServerUpdateJackpot msg = new CMD_GP_ServerUpdateJackpot();
                msg.GameID = readbuffer.ReadInt();
                msg.RecordJack = readbuffer.ReadLong();
                msg.AddSpeed = readbuffer.ReadLong();
                // msg.RecordTime = readbuffer.ReadUint();
                //  msg.NowTime = readbuffer.ReadUint();
                msgall[i] = msg;
            }
            Loom.QueueOnMainThread(() =>
            {
               // NoticeManager.SendNoticeQuick(HallNotice.JACKPOT, msgall);
            });

            return true;
        }
        /// <summary>
        /// 跑马灯消息
        /// </summary>
        /// <param name="maincmd">主命令 MDM_</param>
        /// <param name="subCmd">子命令 SUB_ </param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HanldeMarqueeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case UserOperations.SUB_GP_MARQUEE_DATA_BACK:
                    return HanldeMarqueeInfo(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_MARQUEE_DATA_FINISH:
                    return HanldeMarqueeInfoFinish(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_MARQUEE_NEW_BACK:
                    return HandleMarqueeNewBack(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_MARQUEE_NEW_FINSH:
                    return HandleMarqueeNewFinsh(maincmd, subCmd, readbuffer);
            }
            return true;
        }

        /// <summary>
        /// 背包消息
        /// </summary>
        /// <param name="maincmd">主命令 MDM_</param>
        /// <param name="subCmd">子命令 SUB_ </param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleBagMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case UserOperations.SUB_GP_BAG_DATA_BACK:
                    return HandleBagInfo(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_BAG_DATA_FINISH:
                    return HandleBagFinish(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_USE_PROP_BACK:
                    return HandleUseBagPropResult(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_THREW_PROP_BACK:
                    return HandleDeleteBagPropResult(maincmd, subCmd, readbuffer);
                case UserOperations.SUB_GP_USE_GROUP_PROP_BACK:
                    return UseBagGiftResult(maincmd, subCmd, readbuffer);
            }
            return false;
        }

        /// <summary>
        /// 收到验证码
        /// </summary>        
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleVerificationCodeResult(CyNetReadBuffer rb)
        {
            CMD_GP_RequestVerificationCodeResult cmd = new CMD_GP_RequestVerificationCodeResult();
            cmd.flagCodeKey = rb.ReadString(96);
            cmd.codeIndex = rb.ReadInt();
            Console.WriteLine(cmd.flagCodeKey);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.VerificationCodeResult(cmd);
            }
            return true;
        }

        /// <summary>
        /// 接收消息 登陆失败
        /// </summary>
        /// writeBuffer.ReadStruct 没有数组和字符串可以直接读取结构体
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        private bool HandleActionFailed(CyNetReadBuffer writeBuffer)
        {
            CMD_GP_LogonFailure msg = new CMD_GP_LogonFailure();
            msg.lResultCode = writeBuffer.ReadInt();
            msg.szDescribeString = writeBuffer.ReadString();
            Console.WriteLine(msg.szDescribeString);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.LogonFailed(msg);
            }
            return true;
        }

        /// <summary>
        /// 登陆完成
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleLogonFinish(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_LogonFinish msg = new CMD_GP_LogonFinish();
            msg.wIntermitTime = readBuffer.ReadUshort();
            msg.wOnLineCountTime = readBuffer.ReadUshort();
            Console.WriteLine("中断时间：" + msg.wIntermitTime);
            Console.WriteLine("更新时间：" + msg.wOnLineCountTime);
            msg.uTimeMarqueeData = readBuffer.ReadUint();
            msg.bIsCountDataTimer = readBuffer.ReadBoolean();
            msg.tCheckSign = readBuffer.ReadString(66);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.LogonFinish(msg);
            }
            return true;
        }

        /// <summary>
        /// 快速注册返回缓存数据
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="rb"></param>
        /// <returns></returns>
        private bool QuickLoginBackHandler(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_QuickLogonBack cmd = new CMD_GP_QuickLogonBack();

            cmd.cLocalSign = rb.ReadString(66);
            cmd.cLocalPassword = rb.ReadString(66);

            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.QuickLoginBack(cmd);
            }
            return true;
        }

        /// <summary>
        /// 请求手机验证码结果
        /// </summary>
        /// <returns></returns>
        public bool RequestPhoneVCCodeResultHandler(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_RequestPhoneVerificationCodeResult cmd = new CMD_GP_RequestPhoneVerificationCodeResult(rb);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.PhoneVCCodeResult(cmd);
            }
            return true;
        }

        /// <summary>
        /// 请求绑定手机结果
        /// </summary>
        /// <returns></returns>
        public bool BindPhoneBackHandler(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_RequestBindPhoneBack cmd = new CMD_GP_RequestBindPhoneBack(rb);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.BindPhoneBack(cmd);
            }
            return true;
        }

        public bool ChangePwdBackHandler(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_ModifyLogonPassBack cmd = new CMD_GP_ModifyLogonPassBack();
            cmd.szNewLogonPassword = rb.ReadString(66);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.ChangePwdBack(cmd);
            }
            return true;
        }



        /// <summary>
        /// 请求重置密码结果
        /// </summary>
        /// <returns></returns>
        public bool ResetPasswordBackHandler(ushort mainCmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GP_RequestResetPasswordByPhoneBack cmd = new CMD_GP_RequestResetPasswordByPhoneBack(rb);
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.ResetPasswordBack(cmd);
            }
            return true;
        }

        public static CMD_GP_LogonSuccess succMsg_ = null;
        /// <summary>
        /// 登陆返回(成功)
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleLogonSucc(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_LogonSuccess succMsg = new CMD_GP_LogonSuccess(readBuffer);
            //if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, succMsg);
            //Console.WriteLine(succMsg.szAccounts);
            //Console.WriteLine(succMsg.szNickName);
            //Console.WriteLine(succMsg.dwGameID);
            //Console.WriteLine(succMsg.dwUserID);
            //Console.WriteLine(succMsg.lUserScore);
            //Console.WriteLine(succMsg.lUserInsure);
            //Console.WriteLine(succMsg.wFaceID);

            //Console.WriteLine(succMsg.cbMemberOrder);
            //Console.WriteLine(succMsg.MemberOverDate);
            //Console.WriteLine("----------------------------------------------------------------------&&&&&&");
            //if (readBuffer.CurrentPos < readBuffer.Length)
            //{
            //    succMsg.attachInfo = new Dictionary<int, object>();
            //    CReadPacketHelper helper = new CReadPacketHelper(readBuffer);
            //    while (readBuffer.CurrentPos < readBuffer.Length)
            //    {
            //        KeyValuePair<int, object> val = helper.ReadValue();
            //        succMsg.attachInfo.Add(val.Key, val.Value);
            //    }
            //}
            succMsg_ = succMsg;
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.LogonSucc(succMsg);
            }
            return true;
        }
        /// <summary>
        /// 等级配置
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        private bool HandleGrowLevelConfig(ushort mainCmd, ushort subCmd, CyNetReadBuffer readBuffer)
        {
            CMD_GP_GrowLevelConfig msgInfo = new CMD_GP_GrowLevelConfig();
            msgInfo.wLevelCount = readBuffer.ReadUshort();
            msgInfo.GrowLevelItem = new tagGrowLevelConfig[msgInfo.wLevelCount];
            for (int i = 0; i < msgInfo.wLevelCount; i++)
            {
                msgInfo.GrowLevelItem[i].wLevelID = readBuffer.ReadUshort();
                msgInfo.GrowLevelItem[i].dwExperience = readBuffer.ReadUint();
            }
            if (MessageMgr.DT_Interface != null)
            {
                MessageMgr.DT_Interface.GrowLevelConfig(msgInfo);
            }
            if (MessageMgr.CurMsgMgr.MsgCallFunc != null) MessageMgr.CurMsgMgr.MsgCallFunc(mainCmd, subCmd, msgInfo);
            return true;
        }
    }

}
