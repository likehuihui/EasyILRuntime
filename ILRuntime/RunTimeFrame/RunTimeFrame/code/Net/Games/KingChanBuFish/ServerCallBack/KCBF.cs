using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;

namespace KCBF
{
    public partial class KCBF_ : Interface_Game
    {
        private KCBF_()
        {

        }
        private static KCBF_ KCBF__ = null;
        /// <summary>
        /// 森林舞会U3D回调函数
        /// </summary>
        public KCBF.U3D_KCBF_Interface KCBFInterFace = null;
        public static KCBF_ KCBFInstance
        {
            get
            {
                if (KCBF__ == null)
                {
                    KCBF__ = new KCBF_();
                }
                return KCBF__;
            }
        }
        public void Error(string errStr)
        {
            if (KCBFInterFace != null)
            {
                KCBFInterFace.Error(errStr);
            }
        }
        public bool HandleReadNetData(ushort mnCmd, ushort sbCmd, CyNetReadBuffer readBuffer)
        {
            switch (mnCmd)
            {
                case GameOperations.MDM_GF_FRAME://框架命令
                    return HandleFrameMsg(mnCmd, sbCmd, readBuffer);
                case GameOperations.MDM_GF_GAME://游戏命令
                    return HandleGameMsg(mnCmd, sbCmd, readBuffer);
                default:
                    Error("未能解析的主命令：" + mnCmd);
                    break;
            }
            return true;
        }

        /// <summary>
        /// 框架命令
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleFrameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case GameOperations.SUB_GF_GAME_STATUS:
                    return HandleGameStatusMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GF_SYSTEM_MESSAGE:
                    return HandleFrameSystemMessageMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GF_GAME_SCENE:
                    return HandleSceneMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_INFO:
                //return HandleMatchInfoMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_WAIT_TIP:
                //return HandleMatchWaitTip(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_RESULT:     //淘汰 优胜 奖状
                                                             //return HandleMatchResult(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_USER_WAIT_DISTRIBUTE:
                    return true;
                case GameOperations.SUB_GR_MATCH_PROMOTION: //晋级
                                                            //return HandleMatchPromotion(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_ELIMINATE:
                    return true;
            }
            Error("未能解析的错误 ZML:" + maincmd + "  ZIML:" + subCmd);
            return true;
        }

        /// <summary>
        /// 桌子状态
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleGameStatusMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GF_GameStatus msg = new CMD_GF_GameStatus();
            msg.cbGameStatus = readbuffer.ReadByte();
            msg.cbAllowLookon = readbuffer.ReadByte();
            Console.WriteLine("游戏状态{0} 是否旁观{1}", msg.cbGameStatus, msg.cbAllowLookon);
            msg_Status = msg;
            if (KCBFInterFace != null)
            {
                KCBFInterFace.GAME_GameStatus(msg);
            }
            return true;
        }
        /// <summary>
        /// 当收到游戏状态时 此值保存给断线重连使用
        /// </summary>
        public CMD_GF_GameStatus msg_Status;
        /// <summary>
        /// 发送框架系统消息
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleFrameSystemMessageMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_CM_SystemMessage msg = new CMD_CM_SystemMessage();
            msg.wType = readbuffer.ReadUshort();
            msg.wLength = readbuffer.ReadUshort();
            msg.szString = readbuffer.ReadString(msg.wLength*2);
            Console.WriteLine("森林舞会:" + msg.szString);
            if (KCBFInterFace != null)
            {
                KCBFInterFace.GAME_SystemMessage(msg.szString);
            }
            return true;
        }
        /// <summary>
        /// 收到场景消息
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleSceneMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            const byte GAME_STATUS_FREE = 0;					    //空闲状态
            const byte GAME_STATUS_PLAY = 100;						//游戏状态
            const byte GAME_SCENE_END = 101;

            switch (msg_Status.cbGameStatus)
            {
                case GAME_STATUS_FREE:
                    {

                    }
                    break;
                case GAME_STATUS_PLAY:
                case GAME_SCENE_END:
                    {

                    }
                    break;
                default:
                    // Error("HandleSceneMsg 未知状态：" + GAME_STATUS_WAIT);
                    break;
            }
            if (KCBFInterFace != null)
            {
                KCBFInterFace.GAME_SCENE_OK();
            }
            return true;
        }
        /*

                /// <summary>
                /// 收到比赛信息消息
                /// </summary>
                /// <param name="mainCmd"></param>
                /// <param name="subCmd"></param>
                /// <param name="readbuffer"></param>
                /// <returns></returns>
                /// 
                private bool HandleMatchInfoMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
                {
                    CMD_GR_Match_Info msg = new CMD_GR_Match_Info();
                    msg.szTitle = new string[4];
                    for (int i = 0; i < 4; i++ )
                    {
                        msg.szTitle[i] = readbuffer.ReadString(128);
                    }
                    msg.wGameCount = readbuffer.ReadUshort();
                    msg.wKindID = readbuffer.ReadUshort();
                    msg.cbMatchType = readbuffer.ReadByte();
                    msg.cbMatchScope = readbuffer.ReadByte();

                    if (KCBFInterFace != null)
                    {
                        KCBFInterFace.GAME_MATCH_INFO(msg);
                    }
                    return true;
                }

                private bool HandleMatchPromotion(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
                {
                    CMD_GR_MatchPromotion msg = new CMD_GR_MatchPromotion();
                    msg.dwMatchID = readbuffer.ReadUint();
                    msg.dwMatchNO = readbuffer.ReadUint();
                    msg.wRankID = readbuffer.ReadUshort();
                    msg.wCurGameCount = readbuffer.ReadUshort();
                    msg.wGameCount = readbuffer.ReadUshort();
                    msg.szMatchName = readbuffer.ReadString(64);

                    if (KCBFInterFace != null)
                    {
                        KCBFInterFace.GAME_MATCH_PROMOTION(msg);
                    }
                    return true;
                }

                private bool HandleMatchWaitTip(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
                {
                    CMD_GR_Match_Wait_Tip msg = new CMD_GR_Match_Wait_Tip();
                    msg.lScore = readbuffer.ReadLong();
                    msg.wRank = readbuffer.ReadUshort();
                    msg.wCurTableRank = readbuffer.ReadUshort();
                    msg.wUserCount = readbuffer.ReadUshort();
                    msg.wCurGameCount = readbuffer.ReadUshort();
                    msg.wGameCount = readbuffer.ReadUshort();
                    msg.wPlayingTable = readbuffer.ReadUshort();
                    msg.szMatchName = readbuffer.ReadString(64);

                    if (KCBFInterFace != null)
                    {
                        KCBFInterFace.GAME_MATCH_WATI_TIP(msg);
                    }
                    return true;
                }

                private bool HandleMatchResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
                {
                    CMD_GR_MatchResult msg = new CMD_GR_MatchResult();
                    msg.lGold = new long[6];
                    msg.dwIngot = new uint[6];
                    msg.dwExperience = new uint[6];
                    msg.szDescribe = new string[6];
                    msg.lMatchScore = new long[6];
                    msg.wRankID = new ushort[6];
                    msg.szPropName = new string[6];

                    for (int i = 0; i < 6; i++)
                    {
                        msg.lGold[i] = readbuffer.ReadLong();
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        msg.dwIngot[i] = readbuffer.ReadUint();
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        msg.dwExperience[i] = readbuffer.ReadUint();
                    }

                    for (int i = 0; i < 6; i++)
                    {
                         msg.szDescribe[i] = readbuffer.ReadString(64);
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        msg.lMatchScore[i] = readbuffer.ReadLong();
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        msg.wRankID[i] = readbuffer.ReadUshort();
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        msg.szPropName[i] = readbuffer.ReadString(64);
                    }

                    msg.szMatchName = readbuffer.ReadString(64);

                    if (KCBFInterFace != null)
                    {
                        KCBFInterFace.GAME_MATCH_RESULT(msg);
                    }

                    return true;
                }
        */

    }
   
}
