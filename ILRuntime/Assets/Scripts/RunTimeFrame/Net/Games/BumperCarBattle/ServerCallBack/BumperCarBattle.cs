using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;

namespace BumperCarBattle
{
    public partial class BumperCarBattle_ : Interface_Game
    {
        private BumperCarBattle_()
        {

        }
        private static BumperCarBattle_ BumperCarBattle__ = null;
        /// <summary>
        /// 豪车飘移U3D回调函数
        /// </summary>
        public BumperCarBattle.U3D_BumperCarBattle_Interface BumperCarBattleInterFace = null;
        public static BumperCarBattle_ BumperCarBattleInstance
        {
            get
            {
                if (BumperCarBattle__ == null)
                {
                    BumperCarBattle__ = new BumperCarBattle_();
                }
                return BumperCarBattle__;
            }
        }
        public void Error(string errStr)
        {
            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Error(errStr);
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
            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.GAME_GameStatus(msg);
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
            Console.WriteLine("豪车飘移:"+msg.szString);
            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.GAME_SystemMessage(msg.szString);
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
                        free.cbTimeLeave=readbuffer.ReadByte();
                        free.lUserMaxScore=readbuffer.ReadLong();
                        free.wBankerUser = readbuffer.ReadUshort();
                        free.cbBankerTime = readbuffer.ReadUshort();
                        free.lBankerWinScore=readbuffer.ReadLong();
                        free.lBankerScore=readbuffer.ReadLong();
                        free.bEnableSysBanker=readbuffer.ReadBoolean();
                        free.lApplyBankerCondition=readbuffer.ReadLong();
                        free.lAreaLimitScore=readbuffer.ReadLong();
                        free.CheckImage=readbuffer.ReadInt();
                        free.szGameRoomName = readbuffer.ReadString(64);

                        if (BumperCarBattleInterFace != null)
                        {
                            BumperCarBattleInterFace.GAME_SCENE_FREE(free);
                        }
                    }
                    break;
                case GAME_STATUS_PLAY:
                case GAME_SCENE_END:
                    {
                        CMD_S_StatusPlay Play_ = new CMD_S_StatusPlay();
                        Play_.lAllJettonScore =new long[9];
                        for (int i = 0; i < 9; i++) 
                        {
                            Play_.lAllJettonScore[i]=readbuffer.ReadLong();
                        }
                            
                        Play_.lUserJettonScore =new long[9];
                        for (int i = 0; i < 9; i++) 
                        {
                            Play_.lUserJettonScore[i] = readbuffer.ReadLong();
                        }

                        Play_.lUserMaxScore = readbuffer.ReadLong();
                        Play_.lApplyBankerCondition = readbuffer.ReadLong();
                        Play_.lAreaLimitScore = readbuffer.ReadLong();
                        Play_.cbTableCardArray =new byte[1][];
                        Play_.cbTableCardArray[0] = new byte[1];
                        Play_.cbTableCardArray[0][0] = readbuffer.ReadByte();

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
                        Play_.CheckImage = readbuffer.ReadInt();
                        Play_.szGameRoomName = readbuffer.ReadString(64);

                        if (BumperCarBattleInterFace != null)
                        {
                            BumperCarBattleInterFace.GAME_SCENE_PLAYING(Play_);
                        }
                    }
                    break;
                default:
                   // Error("HandleSceneMsg 未知状态：" + GAME_STATUS_WAIT);
                    break;
            }
            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.GAME_SCENE_OK();
            }
            return true;
        }

        //空闲状态
        public struct CMD_S_StatusFree
        {
	        //全局信息
            public byte cbTimeLeave;						//剩余时间

	        //玩家信息
            public long lUserMaxScore;							//玩家金币

	        //庄家信息
            public ushort wBankerUser;						//当前庄家
            public ushort cbBankerTime;						//庄家局数
            public long lBankerWinScore;					//庄家成绩
            public long lBankerScore;						//庄家分数
            public bool bEnableSysBanker;					//系统做庄

	        //控制信息
            public long lApplyBankerCondition;				//申请条件
            public long lAreaLimitScore;					//区域限制
            public int CheckImage;

            public string szGameRoomName;			//房间名称 [64]
        };

        //游戏状态
        public struct CMD_S_StatusPlay
        {
	        //全局下注
	        public long[]						lAllJettonScore;		//全体总注[9]

	        //玩家下注
	        public long[]					lUserJettonScore;		//个人总注[9]

	        //玩家积分
	        public long						lUserMaxScore;						//最大下注							

	        //控制信息
	        public long						lApplyBankerCondition;				//申请条件
	        public long						lAreaLimitScore;					//区域限制

	        //扑克信息
	        public byte[][]							cbTableCardArray;				//桌面扑克[1][1]

	        //庄家信息
	        public ushort							wBankerUser;						//当前庄家
            public ushort cbBankerTime;						//庄家局数
	        public long						lBankerWinScore;					//庄家赢分
	        public long						lBankerScore;						//庄家分数
	        public bool							bEnableSysBanker;					//系统做庄

	        //结束信息
	        public long						lEndBankerScore;					//庄家成绩
	        public long						lEndUserScore;						//玩家成绩
	        public long						lEndUserReturnScore;				//返回积分
	        public long						lEndRevenue;						//游戏税收

	        //全局信息
	        public byte							cbTimeLeave;						//剩余时间
	        public byte							cbGameStatus;						//游戏状态
	        public int								CheckImage;
	        public string							szGameRoomName;			//房间名称 [64]
        };
    }

   
}
