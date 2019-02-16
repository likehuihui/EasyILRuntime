using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;
using UnityEngine;

namespace SLWH
{
    public partial class SLWH_ : Interface_Game
    {
        private SLWH_()
        {

        }
        private static SLWH_ SLWH__ = null;
        /// <summary>
        /// 森林舞会U3D回调函数
        /// </summary>
        public SLWH.U3D_SLWH_Interface SLWHInterFace = null;
        public static SLWH_ SLWHInstance
        {
            get
            {
                if (SLWH__ == null)
                {
                    SLWH__ = new SLWH_();
                }
                return SLWH__;
            }
        }
        public void Error(string errStr)
        {
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Error(errStr);
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
                case GameOperations.SUB_GF_SYSTEM_MESSAGE_NEW:
                    return HandleFrameSystemMessageMsgNew(maincmd, subCmd, readbuffer);
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
            if (SLWHInterFace != null)
            {
                SLWHInterFace.GAME_GameStatus(msg);
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
            Console.WriteLine("森林舞会:"+msg.szString);
            if (SLWHInterFace != null)
            {
                SLWHInterFace.GAME_SystemMessage(msg.szString);
            }
            return true;
        }
        private bool HandleFrameSystemMessageMsgNew(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_CM_SystemMessageNew msg = new CMD_CM_SystemMessageNew(readbuffer);
            Debug.Log("深林舞會"+msg);
            Loom.QueueOnMainThread(() =>
            {
                //NoticeManager.SendNoticeQuick(HallNotice.NEW_MARQUREE, new MarqueeDataVO(msg));
            });
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
                        CMD_S_StatusFree free = new CMD_S_StatusFree();
                        free.iUserScore = readbuffer.ReadLong();
                        free.cbTimeLeave = readbuffer.ReadByte();
                        free.MinChip = readbuffer.ReadLong();
                        free.MinChip = readbuffer.ReadLong();
                        int iAnimalType_Max = (int)eAnimalType.eAnimalType_Max;
                        int iColorType_Max = (int)eColorType.eColorType_Max;
                        int iEnjoyGameType = (int)eEnjoyGameType.eEnjoyGameType_Max;
                        free.arrSTAnimalJettonLimit = new tagSTAnimalAtt[iAnimalType_Max][];
                        for (int i = 0; i < iAnimalType_Max; i++)
                        {
                            free.arrSTAnimalJettonLimit[i] = new tagSTAnimalAtt[iColorType_Max];
                            for (int j = 0; j < iColorType_Max; j++)
                            {                              
                                free.arrSTAnimalJettonLimit[i][j].stAnimal.eAnimal = (eAnimalType)readbuffer.ReadInt();
                                free.arrSTAnimalJettonLimit[i][j].stAnimal.eColor = (eColorType)readbuffer.ReadInt();
                                free.arrSTAnimalJettonLimit[i][j].dwMul = readbuffer.ReadUint();
                                free.arrSTAnimalJettonLimit[i][j].qwJettonLimit = readbuffer.ReadUlong();
                            }
                        }
                        free.arrColorRate = new uint[iColorType_Max];
                        for (int i = 0; i < iColorType_Max; i++)
                        {
                            free.arrColorRate[i] =readbuffer.ReadUint();
                        }
                        free.arrSTEnjoyGameJettonLimit = new tagSTEnjoyGameInfo[iEnjoyGameType];
                        for (int i = 0; i < iEnjoyGameType; i++)
                        {
                            free.arrSTEnjoyGameJettonLimit[i].eEnjoyGame = (eEnjoyGameType)readbuffer.ReadInt();
                            free.arrSTEnjoyGameJettonLimit[i].dwMul = readbuffer.ReadUint();
                            free.arrSTEnjoyGameJettonLimit[i].qwJettonLimit = readbuffer.ReadUlong();
                        }

                        //free.stBankerInfo = readbuffer.ReadT<CMD_BANKER_INFO>();
                        free.stBankerInfo = new CMD_BANKER_INFO();
                        free.stBankerInfo.szBankerName = readbuffer.ReadString(64);
                        free.stBankerInfo.szBankerAccounts = readbuffer.ReadString(64);
                        free.stBankerInfo.dwUserID = readbuffer.ReadUint();
                        free.stBankerInfo.iBankerScore = readbuffer.ReadLong();
                        free.stBankerInfo.wBankCount = readbuffer.ReadUint();

                        free.stColor = new int[24];
                        for (int i = 0; i < free.stColor.Length; i++)
                        {
                            free.stColor[i] = readbuffer.ReadInt();
                        }

                        /*free.stBankerInfo = new CMD_BANKER_INFO();
                        free.stBankerInfo.szBankerName = readbuffer.ReadString(64);
                        free.stBankerInfo.szBankerAccounts = readbuffer.ReadString(64);
                        free.stBankerInfo.dwUserID = readbuffer.ReadUint();
                        free.stBankerInfo.iBankerScore = readbuffer.ReadLong();
                        free.stBankerInfo.wBankCount = readbuffer.ReadUint();*/

                        if (SLWHInterFace != null)
                        {
                            SLWHInterFace.GAME_SCENE_FREE(free);
                        }
                    }
                    break;
                case GAME_STATUS_PLAY:
                case GAME_SCENE_END:
                    {
                        CMD_S_StatusPlay Play_ = new CMD_S_StatusPlay();
                        Play_.iUserScore = readbuffer.ReadLong();
                        Play_.cbTimeLeave = readbuffer.ReadByte();
                        Play_.cbGameStatus = readbuffer.ReadByte();

                        Play_.MinChip = readbuffer.ReadLong();
                        Play_.MaxChip = readbuffer.ReadLong();

                        int iAnimalType_Max = (int)eAnimalType.eAnimalType_Max;
                        int iColorType_Max = (int)eColorType.eColorType_Max;
                        int iEnjoyGameType = (int)eEnjoyGameType.eEnjoyGameType_Max;
                        Play_.arrSTAnimalAtt = new tagSTAnimalAtt[iAnimalType_Max][];
                        for (byte i = 0; i < iAnimalType_Max; i++)
                        {
                            Play_.arrSTAnimalAtt[i] = new tagSTAnimalAtt[iColorType_Max];
                            for (byte j = 0; j < iColorType_Max; j++)
                            {
                                Play_.arrSTAnimalAtt[i][j].stAnimal.eAnimal = (eAnimalType)readbuffer.ReadInt();
                                Play_.arrSTAnimalAtt[i][j].stAnimal.eColor = (eColorType)readbuffer.ReadInt();
                                Play_.arrSTAnimalAtt[i][j].dwMul = readbuffer.ReadUint();
                                Play_.arrSTAnimalAtt[i][j].qwJettonLimit = readbuffer.ReadUlong();
                            }
                        }
                        Play_.arrColorRate = new uint[iColorType_Max];
                        for (byte i = 0; i < iColorType_Max; i++)
                        {
                            Play_.arrColorRate[i] = readbuffer.ReadUint();
                        }
                        Play_.arrSTEnjoyGameAtt = new tagSTEnjoyGameInfo[iEnjoyGameType];
                        for (byte i = 0; i < iEnjoyGameType; i++)
                        {
                            Play_.arrSTEnjoyGameAtt[i].eEnjoyGame = (eEnjoyGameType)readbuffer.ReadInt();
                            Play_.arrSTEnjoyGameAtt[i].dwMul = readbuffer.ReadUint();
                            Play_.arrSTEnjoyGameAtt[i].qwJettonLimit = readbuffer.ReadUlong();
                        }

                        Play_.stWinAnimal.stAnimalInfo.eAnimal = (eAnimalType)readbuffer.ReadInt();
                        Play_.stWinAnimal.stAnimalInfo.eColor = (eColorType)readbuffer.ReadInt();
                        Play_.stWinAnimal.ePrizeMode = (eAnimalPrizeMode)readbuffer.ReadInt();
                        Play_.stWinAnimal.qwFlag = readbuffer.ReadUlong();
                        Play_.stWinAnimal.arrstRepeatModePrize = new tagSTAnimalInfo[20];
                        for (int i = 0; i < 20; i++)
                        {
                            Play_.stWinAnimal.arrstRepeatModePrize[i].eAnimal = (eAnimalType)readbuffer.ReadInt();
                            Play_.stWinAnimal.arrstRepeatModePrize[i].eColor = (eColorType)readbuffer.ReadInt();
                        }
                        Play_.stWinAnimal.PrizeIndex = new int[2][];
                        for (int i = 0; i < 2; i++)
                        {
                            Play_.stWinAnimal.PrizeIndex[i] = new int[12];
                            for (int j = 0; j < 12; j++)
                            {
                                Play_.stWinAnimal.PrizeIndex[i][j] = readbuffer.ReadInt();
                            }
                        }

                        Play_.stWinEnjoyGameType.ePrizeGameType = (eEnjoyGameType)readbuffer.ReadInt();
                        Play_.stWinEnjoyGameType.qwFlag = readbuffer.ReadUlong();

                        //Play_.stBankerInfo = readbuffer.ReadT<CMD_BANKER_INFO>();
                        Play_.stBankerInfo = new CMD_BANKER_INFO();
                        Play_.stBankerInfo.szBankerName = readbuffer.ReadString(64);
                        Play_.stBankerInfo.szBankerAccounts = readbuffer.ReadString(64);
                        Play_.stBankerInfo.dwUserID = readbuffer.ReadUint();
                        Play_.stBankerInfo.iBankerScore = readbuffer.ReadLong();
                        Play_.stBankerInfo.wBankCount = readbuffer.ReadUint();

                        Play_.stColor = new int[24];
                        for (int i = 0; i < Play_.stColor.Length; i++)
                        {
                            Play_.stColor[i] = readbuffer.ReadInt();
                        }

                        /*Play_.stBankerInfo = new CMD_BANKER_INFO();
                        Play_.stBankerInfo.szBankerName = readbuffer.ReadString(64);
                        Play_.stBankerInfo.szBankerAccounts = readbuffer.ReadString(64);
                        Play_.stBankerInfo.dwUserID = readbuffer.ReadUint();
                        Play_.stBankerInfo.iBankerScore = readbuffer.ReadLong();
                        Play_.stBankerInfo.wBankCount = readbuffer.ReadUint();*/

                        if (SLWHInterFace != null)
                        {
                            SLWHInterFace.GAME_SCENE_PLAYING(Play_);
                        }
                    }
                    break;
                default:
                   // Error("HandleSceneMsg 未知状态：" + GAME_STATUS_WAIT);
                    break;
            }
            if (SLWHInterFace != null)
            {
                SLWHInterFace.GAME_SCENE_OK();
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

            if (SLWHInterFace != null)
            {
                SLWHInterFace.GAME_MATCH_INFO(msg);
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

            if (SLWHInterFace != null)
            {
                SLWHInterFace.GAME_MATCH_PROMOTION(msg);
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

            if (SLWHInterFace != null)
            {
                SLWHInterFace.GAME_MATCH_WATI_TIP(msg);
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

            if (SLWHInterFace != null)
            {
                SLWHInterFace.GAME_MATCH_RESULT(msg);
            }
            
            return true;
        }
*/

        //空闲状态
        public struct CMD_S_StatusFree
        {
            public long							iUserScore;						//我的金币

	        //全局信息
	        public byte							cbTimeLeave;						//剩余时间
            public long MinChip;                            //最小筹码
            public long MaxChip;                            //最大筹码
	        // 下注限制信息
	        public tagSTAnimalAtt[][]		    arrSTAnimalJettonLimit; //动物属性4 3 [eAnimalType_Max][eColorType_Max]  
	        public uint[]						arrColorRate;//颜色分布概率3  [eColorType_Max]
	        public tagSTEnjoyGameInfo[]			arrSTEnjoyGameJettonLimit;//庄闲和属性 [eEnjoyGameType_Max]

	        public CMD_BANKER_INFO				stBankerInfo;						//庄家信息
            public int[] stColor;                                     //转盘颜色分布[24]
        };

        //游戏状态
        public struct CMD_S_StatusPlay
        {
	        public long							iUserScore;						//我的金币

	        public byte							cbTimeLeave;						//剩余时间
	        public byte							cbGameStatus;					//游戏状态

            public long MinChip;                            //最小筹码
            public long MaxChip;                            //最大筹码
	        // 倍率信息
            public tagSTAnimalAtt[][] arrSTAnimalAtt; //动物属性4 3 [eAnimalType_Max][eColorType_Max]
            public uint[] arrColorRate;//颜色分布概率3 [eColorType_Max]
            public tagSTEnjoyGameInfo[] arrSTEnjoyGameAtt;//庄闲和属性[eEnjoyGameType_Max]

	        //开奖信息
	        public tagSTAnimalPrize				    stWinAnimal;						//开奖动物
	        public tagSTEnjoyGamePrizeInfo			stWinEnjoyGameType;					//开奖庄闲和

            public CMD_BANKER_INFO stBankerInfo;						//庄家信息
            public int[] stColor;                                     //转盘颜色分布[24]
        };
    }

   
}
