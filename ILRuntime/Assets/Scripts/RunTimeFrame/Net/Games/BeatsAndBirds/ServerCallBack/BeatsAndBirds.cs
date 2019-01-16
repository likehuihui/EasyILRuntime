using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;

namespace BeatsAndBirds
{
    public partial class BeatsAndBirds_ : Interface_Game
    {
        private BeatsAndBirds_()
        {

        }

        private static BeatsAndBirds_ ins_ = null;

        public U3D_BeatsAndBirds_Interface BeatsAndBirdsInterface = null;
        public static BeatsAndBirds_ BeatsAndBirdsInstance
        {
            get
            {
                if (ins_ == null)
                {
                    ins_ = new BeatsAndBirds_();
                }
                return ins_;
            }
        }

        void Log(string content)
        {
            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Log(content);
            }
        }

        public void Error(string errStr)
        {
            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Error(errStr);
            }
        }

        public bool HandleReadNetData(ushort mnCmd, ushort sbCmd, CyNetReadBuffer readBuffer)
        {
            Log(string.Format("飞禽走兽协议：主协议：{0}  子协议：{1}  长度:{2}", mnCmd, sbCmd, readBuffer.Length));
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
            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.GAME_GameStatus(msg);
            }
            return true;
        }

        /// <summary>
        /// 当收到游戏状态时 此值保存给断线重连使用
        /// </summary>
        private CMD_GF_GameStatus msg_Status;
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
            msg.szString = readbuffer.ReadString(msg.wLength * 2);
            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.GAME_SystemMessage(msg.szString);
            }
            return true;
        }
        private bool HandleFrameSystemMessageMsgNew(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_CM_SystemMessageNew msg = new CMD_CM_SystemMessageNew(readbuffer);
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
            const byte GAME_STATUS_WAIT = 200;						//等待状态
            //结束原因
            //#define GER_NO_PLAYER					    0x10								//没有玩家
            ////游戏状态
            const byte GS_TK_FREE = GAME_STATUS_FREE;	            //等待开始
            const byte GS_TK_BET = GAME_STATUS_PLAY;	            //下注状态
            const byte GS_TK_END = GAME_STATUS_PLAY + 1;            //开奖状态

            switch (msg_Status.cbGameStatus)
            {
                case GS_TK_FREE:
                    {
                        CMD_S_StatusFree free = new CMD_S_StatusFree();

                        free.cbTimeLeave = readbuffer.ReadByte();
                        free.lUserMaxScore = readbuffer.ReadLong();
                        free.nAnimalPercent = new int[AREA_ALL];
                        for (var i = 0; i < free.nAnimalPercent.Length; i++)
                        {
                            free.nAnimalPercent[i] = readbuffer.ReadInt();
                        }

                        free.wBankerUser = readbuffer.ReadShort();
                        free.cbBankerTime = readbuffer.ReadShort();
                        free.lBankerWinScore = readbuffer.ReadLong();
                        free.lBankerScore = readbuffer.ReadLong();
                        free.bEnableSysBanker = readbuffer.ReadBoolean();
                        free.lApplyBankerCondition = readbuffer.ReadLong();
                        free.lAreaLimitScore = readbuffer.ReadLong();
                        free.szGameRoomName = new char[32];
                        for (var i = 0; i < free.szGameRoomName.Length; i++)
                        {
                            free.szGameRoomName[i] = readbuffer.ReadChar();
                        }

                        free.iBetArrayNum = readbuffer.ReadInt();
                        free.iBetArray = new long[free.iBetArrayNum];
                        for (var i = 0; i < free.iBetArrayNum; i++)
                        {
                            free.iBetArray[i] = readbuffer.ReadLong();
                        }

                        if (BeatsAndBirdsInterface != null)
                        {
                            BeatsAndBirdsInterface.GAME_SCENE_FREE(free);
                        }
                    }
                    break;
                case GS_TK_BET:
                    {
                        CMD_S_StatusPlay cmd = readStatusPlay(readbuffer);

                        if (BeatsAndBirdsInterface != null)
                        {
                            BeatsAndBirdsInterface.GAME_SCENE_BET(cmd);
                        }
                    }
                    break;
                case GS_TK_END:
                    {
                        CMD_S_StatusPlay cmd = readStatusPlay(readbuffer);

                        if (BeatsAndBirdsInterface != null)
                        {
                            BeatsAndBirdsInterface.GAME_SCENE_END(cmd);
                        }
                    }
                    break;
                default:
                    Error("HandleSceneMsg 未知状态：" + GAME_STATUS_WAIT);
                    break;
            }
            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.GAME_SCENE_OK();
            }
            return true;
        }

        CMD_S_StatusPlay readStatusPlay(CyNetReadBuffer rb)
        {
            CMD_S_StatusPlay cmd = new CMD_S_StatusPlay();
            cmd.lAllJettonScore = new long[AREA_COUNT + 1];
            for(var i = 0; i < cmd.lAllJettonScore.Length; i++)
            {
                cmd.lAllJettonScore[i] = rb.ReadLong();
            }
            cmd.lUserJettonScore = new long[AREA_COUNT + 1];
            for (var i = 0; i < cmd.lUserJettonScore.Length; i++)
            {
                cmd.lUserJettonScore[i] = rb.ReadLong();
            }
            cmd.lUserMaxScore = rb.ReadLong();
            cmd.nAnimalPercent = new int[AREA_ALL];
            for (var i = 0; i < cmd.nAnimalPercent.Length; i++)
            {
                cmd.nAnimalPercent[i] = rb.ReadInt();
            }

            cmd.lApplyBankerCondition = rb.ReadLong();
            cmd.lAreaLimitScore = rb.ReadLong();

            cmd.cbTableCardArray = new byte[3];
            for (var i = 0; i < cmd.cbTableCardArray.Length; i++)
            {
                cmd.cbTableCardArray[i] = rb.ReadByte();
            }
            cmd.cbShaYuAddMulti = new byte[2];
            for (int i = 0; i < cmd.cbShaYuAddMulti.Length; i++)
            {
                cmd.cbShaYuAddMulti[i] = rb.ReadByte();
            }

            cmd.wBankerUser = rb.ReadShort();
            cmd.cbBankerTime = rb.ReadShort();
            cmd.lBankerWinScore = rb.ReadLong();
            cmd.lBankerScore = rb.ReadLong();
            cmd.bEnableSysBanker = rb.ReadBoolean();

            cmd.lEndBankerScore = rb.ReadLong();
            cmd.lEndUserScore = rb.ReadLong();
            cmd.lEndUserReturnScore = rb.ReadLong();
            cmd.lEndRevenue = rb.ReadLong();

            cmd.cbTimeLeave = rb.ReadByte();
            cmd.cbGameStatus = rb.ReadByte();
            cmd.szGameRoomName = new char[32];
            for (var i = 0; i < cmd.szGameRoomName.Length; i++)
            {
                cmd.szGameRoomName[i] = rb.ReadChar();
            }

            cmd.iBetArrayNum = rb.ReadInt();
            cmd.iBetArray = new long[cmd.iBetArrayNum];
            for (var i = 0; i < cmd.iBetArrayNum; i++)
            {
                cmd.iBetArray[i] = rb.ReadLong();
            }

            return cmd;
        }
    }    
}
