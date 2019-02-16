using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using UnityEngine;
using System.Threading;

namespace MessagePackage
{
    /// <summary>
    /// 游戏服务
    /// </summary>
    public partial class GameMessageMgr : DelegateMsgBase
    {


        public override void Error(int status, string errStr)
        {
            if (MessageMgr.ROOM_Interface != null)
            {
                if (Thread.CurrentThread == Loom.unityThread)
                {
                    MessageMgr.ROOM_Interface.Error(errStr);
                }
                else
                {
                    Loom.QueueOnMainThread(() =>
                    {
                        MessageMgr.ROOM_Interface.Error(errStr);
                    });
                }
            }

        }
        
        /// <summary>
        /// 主命令
        /// </summary>
        /// <param name="mnCmd"></param>
        /// <param name="sbCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        public override bool HandleReadNetData(ushort mnCmd, ushort sbCmd, CyNetReadBuffer readbuffer)
        {
            Loom.QueueOnMainThread(() =>
            {
                // if (MessageMgr.CurMsgMgr.GameUserHandleMsg != null && MessageMgr.CurMsgMgr.GameUserHandleMsg(mnCmd, sbCmd, readbuffer)) return true;
                CyNetReadBuffer TP = new CyNetReadBuffer(readbuffer.Length - 8);
                //             for (int i = 0; i < readbuffer.Length - 8; i++)
                //             {
                //                 TP.NetBuffer[i] = readbuffer.NetBuffer[i + 8];
                //             }
                TP.Length = readbuffer.Length - 8;
                Array.Copy(readbuffer.NetBuffer, 8, TP.NetBuffer, 0, TP.Length);

                //Console.WriteLine("收到游戏房间子命令~~~~~~~~~~~~~~~~~~~~~~~~  " + sbCmd);

                Console.WriteLine("收到游戏房间主命令" + mnCmd);
                if (MessageMgr.ROOM_Interface != null)
                {
                    MessageMgr.ROOM_Interface.CMD_(mnCmd, sbCmd, TP.Length);
                }
                switch (mnCmd)
                {
                    case GameOperations.MDM_GR_LOGON:
                        HandleGameServerMsg(mnCmd, sbCmd, TP);
                        break;
                    case GameOperations.MDM_GR_CONFIG:
                        HandleRoomServerMsg(mnCmd, sbCmd, TP);
                        break;
                    case GameOperations.MDM_GR_USER:
                        HandleUserServerMsg(mnCmd, sbCmd, TP);
                        break;
                    case GameOperations.MDM_GR_STATUS:
                        HandleStatusMsg(mnCmd, sbCmd, TP);
                        break;
                    case GameOperations.MDM_CM_SYSTEM:
                        HandleSystemMsg(mnCmd, sbCmd, TP);
                        break;
                    case GameOperations.MDM_GF_FRAME:
                    case GameOperations.MDM_GF_GAME:
                    case GameOperations.MDM_GR_INSURE:
                        {
                            if (MessageMgr.Game_Interface == null)
                            {
                                if (mnCmd != GameOperations.MDM_GF_FRAME || sbCmd != GameOperations.SUB_GF_GAME_STATUS)
                                {
                                    Error(0, string.Format("协议[{0} -> {1}] 找不到对应的游戏接口对象", mnCmd, sbCmd));
                                }
                            }
                            else
                            {
                                MessageMgr.Game_Interface.HandleReadNetData(mnCmd, sbCmd, TP);
                            }
                        }
                        break;
                    case GameOperations.MDM_GR_MATCH:
                        HandleMatchMsg(mnCmd, sbCmd, TP);
                        break;
                    case GameOperations.MDM_GR_ROOM_CARD:
                        HandleRoomCardMsg(mnCmd, sbCmd, TP);
                        break;
                    default:
                        break;
                }
            });
            
            return true;
        }

        /// <summary>
        /// 登录信息
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleGameServerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            
            switch (subCmd)
            {
                case GameOperations.SUB_GR_LOGON_SUCCESS:
                    return HandleLogonSuccessMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_LOGON_FAILURE:
                    return HandleLogonFailureMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_LOGON_FINISH:
                    {
                        Console.WriteLine("登录完毕");
                        if (MessageMgr.ROOM_Interface != null)
                        {
                            MessageMgr.ROOM_Interface.LogonFinish();
                        }
                        //if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(maincmd, subCmd, null);
                    }
                    return true;

            }

            return false;
        }

        /// <summary>
        /// (登陆成功后发送)配置信息
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleRoomServerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case GameOperations.SUB_GR_CONFIG_SERVER:
                    return HandleConfigServerMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_CONFIG_COLUMN:
                     return HandleConfigColumnMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_CONFIG_PROPERTY:
                    return HandleConfigPropertyMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_CONFIG_FINISH:
                    return HandleConfigFinishMsg(maincmd, subCmd, readbuffer);
            }

            return true;
        }

        /// <summary>
        /// 用户命令 MDM_GR_USER
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleUserServerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case GameOperations.SUB_GR_USER_ENTER:
                    return HandleUserInfoEnterMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_USER_SCORE:
                    return HandleUserFenShu(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_USER_STATUS:
                    return HandleUserStatus(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_REQUEST_FAILURE:
                    return HandleRequestFailureMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_USER_CHAT:
                    return HandleUserChatMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_USER_EXPRESSION:
                    return HandleUserExpressionMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_WISPER_CHAT:
                    return HandleWisperChatMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_WISPER_EXPRESSION:
                    return HandleWisperExpressionMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_PROPERTY_FAILURE:
                    return HandlePropertyFailureMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_PROPERTY_SUCCESS:
                    return HandlePropertySuccessMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_PROPERTY_TRUMPET:
                    return HandleSendTrumpetMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_USER_INVITE:
                    return HandleUserInviteMsg(maincmd, subCmd, readbuffer);
            }

            return false;
        }

        /// <summary>
        /// 系统命令
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleSystemMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case GameOperations.SUB_CM_SYSTEM_MESSAGE:
                    return HandleSystemMessageMsg(maincmd, subCmd, readbuffer); 
            }
            return false;
        }

        private bool HandleMatchMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd) 
            {
                case GameOperations.SUB_GR_MATCH_FEE:           //报名费用
                    return HandleMatchFee(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_NUM:           //等待人数
                    return HandleMatchNum(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_LEAVE_MATCH:         //退出比赛
                    return HandleMatchStatus(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_STATUS:        //比赛状态
                    return HandleMatchStatus(maincmd, subCmd, readbuffer);
                 case GameOperations.SUB_GR_MATCH_INFO:         //比赛信息
                    return true;
                 case GameOperations.SUB_GR_MATCH_WAIT_TIP:     //等待提示
                    return true;
                 case GameOperations.SUB_GR_MATCH_RESULT:       //比赛结果
                    return true;
                 case GameOperations.SUB_GR_MATCH_USER_COUNT:   //参赛人数
                    return true;
                 case GameOperations.SUB_GR_MATCH_DESC:         //比赛描述
                    return true;
                case GameOperations.SUB_GR_MATCH_GOLDUPDATE:
                    return HandleMatchGoldUpdate(maincmd, subCmd, readbuffer);
            }
            return false;
        }

        /// <summary>
        /// 桌子状态
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleStatusMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case GameOperations.SUB_GR_TABLE_STATUS:
                    return HandleTableStatusMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_TABLE_INFO:
                    return HandleTableInfo(maincmd, subCmd, readbuffer);
            }
            return false;
        }
        /// <summary>
        /// 房卡
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleRoomCardMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case GameOperations.SUB_RC_S_ROOM_CREATE:
                    return HandleCreateRoomMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_RC_S_ROOM_JOIN:
                    return HandleJoinRoomInfo(maincmd, subCmd, readbuffer);
            }
            return false;
        }
    }
}