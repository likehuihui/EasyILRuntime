using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;

namespace GOLDS
{
    public partial class GOLDS_ : Interface_Game
    {
        private GOLDS_()
        {

        }
        private static GOLDS_ GOLDS__ = null;
        /// <summary>
        /// 金鲨银鲨U3D回调函数
        /// </summary>
        public GOLDS.U3D_GOLDS_Interface GOLDSInterFace = null;
        public static GOLDS_ GOLDSInstance
        {
            get
            {
                if (GOLDS__ == null)
                {
                    GOLDS__ = new GOLDS_();
                }
                return GOLDS__;
            }
        }
        public void Error(string errStr)
        {
            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Error(errStr);
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
            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.GAME_GameStatus(msg);
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
            msg.szString = readbuffer.ReadString(msg.wLength);
            Console.WriteLine("金鲨银鲨:"+msg.szString);
            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.GAME_SystemMessage(msg.szString);
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
                        CMD_S_StatusFree free = new CMD_S_StatusFree();
                        free.cbTimeLeave = readbuffer.ReadByte();
                        free.lUserMaxScore = readbuffer.ReadLong();
                        free.lStorageScore = readbuffer.ReadLong();
                        free.nAnimalPercent =new  int[9];
                        for (int i = 0; i < free.nAnimalPercent.Length; i++)
			            {
                            free.nAnimalPercent[i] = readbuffer.ReadInt();
			            }
                        free.wBankerUser = readbuffer.ReadUshort();
                        free.cbBankerTime = readbuffer.ReadUshort();
                        free.lBankerWinScore = readbuffer.ReadLong();
                        free.lBankerScore = readbuffer.ReadLong();
                        free.bEnableSysBanker = readbuffer.ReadBoolean();
                        free.lApplyBankerCondition = readbuffer.ReadLong();
                        free.lAreaLimitScore = readbuffer.ReadLong();
                        free.szGameRoomName = readbuffer.ReadString(64);

                        if (GOLDSInterFace != null)
                        {
                            GOLDSInterFace.GAME_SCENE_FREE(free);
                        }
                    }
                    break;
                case GAME_STATUS_PLAY:
                case GAME_SCENE_END:
                    {
                        CMD_S_StatusPlay Play_ = new CMD_S_StatusPlay();
                        Play_.lAllJettonScore = new long[12];
                        for (int i = 0; i < Play_.lAllJettonScore.Length; i++)
                        {
                            Play_.lAllJettonScore[i]=readbuffer.ReadLong();
                        }
                        Play_.lStorageScore = readbuffer.ReadLong();
                        Play_.lUserJettonScore = new long[12];
                        for (int i = 0; i < Play_.lUserJettonScore.Length; i++)
                        {
                            Play_.lUserJettonScore[i] = readbuffer.ReadLong();
                        }
                        Play_.lUserMaxScore = readbuffer.ReadLong();
                        Play_.nAnimalPercent = new int[9];
                        for (int i = 0; i < Play_.nAnimalPercent.Length; i++)
                        {
                            Play_.nAnimalPercent[i] = readbuffer.ReadInt();
                        }

                        Play_.lApplyBankerCondition = readbuffer.ReadLong();

                        Play_.lAreaLimitScore = readbuffer.ReadLong();
                        Play_.cbTableCardArray = readbuffer.ReadBytes(2);

                        Play_.wBankerUser = readbuffer.ReadUshort();

                        Play_.cbBankerTime = readbuffer.ReadUshort();

                        Play_.lBankerWinScore = readbuffer.ReadLong();
                        Play_.lBankerScore = readbuffer.ReadLong();
                        Play_.bEnableSysBanker = readbuffer.ReadBoolean();
                        Play_.lEndBankerScore = readbuffer.ReadLong();
                        Play_.lEndUserScore = readbuffer.ReadLong();
                        Play_.lEndUserReturnScore = readbuffer.ReadLong();
                        Play_.lEndRevenue = readbuffer.ReadLong();
                        Play_.cbTimeLeave = readbuffer.ReadByte();
                        Play_.cbGameStatus = readbuffer.ReadByte();
                        Play_.szGameRoomName = readbuffer.ReadString(64);
                        if (GOLDSInterFace != null)
                        {
                            GOLDSInterFace.GAME_SCENE_PLAYING(Play_);
                        }
                    }
                    break;
                default:
                   // Error("HandleSceneMsg 未知状态：" + GAME_STATUS_WAIT);
                    break;
            }
            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.GAME_SCENE_OK();
            }
            return true;
        }
    }

   
}
