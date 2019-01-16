using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;

namespace Land
{
    public partial class Land_ : Interface_Game
    {
        private Land_()
        {

        }
        private static Land_ Land__ = null;
        /// <summary>
        /// 斗地主U3D回调函数
        /// </summary>
        public U3D_Land_Interface LandInstanceInterFace = null;
        public static Land_ LandInstance
        {
            get
            {
                if (Land__ == null)
                {
                    Land__ = new Land_();
                }
                return Land__;
            }
        }
        public void Error(string errStr)
        {
            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.Error(errStr);
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
                    return HandleMatchInfoMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_WAIT_TIP:
                    return HandleMatchWaitTip(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_RESULT:     //淘汰 优胜 奖状
                    return HandleMatchResult(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_USER_WAIT_DISTRIBUTE:
                    return true;
                case GameOperations.SUB_GR_MATCH_PROMOTION: //晋级
                    return HandleMatchPromotion(maincmd, subCmd, readbuffer);
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
            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.GAME_GameStatus(msg);
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
            Console.WriteLine("斗地主:"+msg.szString);
            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.GAME_SystemMessage(msg.szString);
            }
            return true;
        }
        private bool HandleFrameSystemMessageMsgNew(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_CM_SystemMessageNew msg = new CMD_CM_SystemMessageNew(readbuffer);
            Loom.QueueOnMainThread(() =>
            {
              //  NoticeManager.SendNoticeQuick(HallNotice.NEW_MARQUREE, new MarqueeDataVO(msg));
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
            const byte GS_TK_CALL = GAME_STATUS_PLAY;	            //叫庄状态
            const byte GS_TK_PLAYING = GAME_STATUS_PLAY + 1;		//游戏进行
            switch (msg_Status.cbGameStatus)
            {
                case GS_TK_FREE:
                    {
                        CMD_S_StatusFree free = new CMD_S_StatusFree();
                        //空闲状态
                        free.lCellScore = readbuffer.ReadInt();
                        free.cbTimeOutCard = readbuffer.ReadByte();
                        free.cbTimeCallScore = readbuffer.ReadByte();
                        free.cbTimeStartGame = readbuffer.ReadByte();
                        free.cbTimeHeadOutCard = readbuffer.ReadByte();

                        free.lTurnScore = new long[GAME_PLAYER];
                        for (byte i = 0; i < GAME_PLAYER;i++ )
                        {
                            free.lTurnScore[i] = readbuffer.ReadLong();
                        }

                        free.lCollectScore = new long[GAME_PLAYER];
                        for (byte i = 0; i < GAME_PLAYER; i++)
                        {
                            free.lCollectScore[i] = readbuffer.ReadLong();
                        }
                        free.cbCurGameCount = readbuffer.ReadByte();
                        free.cbAllGameCount = readbuffer.ReadByte();
                        free.wSessionCount = readbuffer.ReadUshort();
                        free.cbChampionMode = readbuffer.ReadByte();
                        free.cbEventnMode = readbuffer.ReadByte();
                        free.cbSpecialValue = readbuffer.ReadByte();
                        if (LandInstanceInterFace != null)
                        {
                            LandInstanceInterFace.GAME_SCENE_FREE(free);
                        }
                    }
                    break;
                case GS_TK_CALL:
                    {
                        CMD_S_StatusCall Call = new CMD_S_StatusCall();
                        Call.lCellScore = readbuffer.ReadInt();
                        Call.cbTimeOutCard = readbuffer.ReadByte();
                        Call.cbTimeCallScore = readbuffer.ReadByte();
                        Call.cbTimeStartGame = readbuffer.ReadByte();
                        Call.cbTimeHeadOutCard = readbuffer.ReadByte();

                        Call.lTurnScore = new long[GAME_PLAYER];
                        for (byte i = 0; i < GAME_PLAYER; i++)
                        {
                            Call.lTurnScore[i] = readbuffer.ReadLong();
                        }

                        Call.lCollectScore = new long[GAME_PLAYER];
                        for (byte i = 0; i < GAME_PLAYER; i++)
                        {
                            Call.lCollectScore[i] = readbuffer.ReadLong();
                        }

                        Call.wCurrentUser = readbuffer.ReadUshort();
                        Call.cbBankerScore = readbuffer.ReadByte();

                        Call.cbScoreInfo = new byte[GAME_PLAYER];
                        for (byte i = 0; i < GAME_PLAYER; i++)
                        {
                            Call.cbScoreInfo[i] = readbuffer.ReadByte();
                        }

                        Call.cbHandCardData = new byte[NORMAL_COUNT];
                        for (byte i = 0; i < NORMAL_COUNT; i++)
                        {
                            Call.cbHandCardData[i] = readbuffer.ReadByte();
                        }
                        Call.cbPlayerCardData = new byte[GAME_PLAYER][];
                        for (ushort i = 0; i < GAME_PLAYER; i++)
                        {
                            Call.cbPlayerCardData[i] = new byte[MAX_COUNT];
                            for (byte j = 0; j < MAX_COUNT; j++)
                            {
                                Call.cbPlayerCardData[i][j] = readbuffer.ReadByte();

                            }
                            //Dic.Add(i, Start.cbCardData[i]);
                        }
                        Call.cbCurGameCount = readbuffer.ReadByte();
                        Call.cbAllGameCount = readbuffer.ReadByte();
                        Call.wSessionCount = readbuffer.ReadUshort();
                        Call.cbChampionMode = readbuffer.ReadByte();
                        Call.cbEventnMode = readbuffer.ReadByte();
                        Call.cbSpecialValue = readbuffer.ReadByte();
                        if (LandInstanceInterFace != null)
                        {
                            LandInstanceInterFace.GAME_SCENE_CALL(Call);
                        }
                    }
                    break;
                case GS_TK_PLAYING:
                    {
                        CMD_S_StatusPlay Play_ = new CMD_S_StatusPlay();
                        Play_.lCellScore = readbuffer.ReadInt();
                        Play_.cbTimeOutCard = readbuffer.ReadByte();
                        Play_.cbTimeCallScore = readbuffer.ReadByte();
                        Play_.cbTimeStartGame = readbuffer.ReadByte();
                        Play_.cbTimeHeadOutCard = readbuffer.ReadByte();

                        Play_.lTurnScore = new long[GAME_PLAYER];
                        for (byte i = 0; i < GAME_PLAYER; i++)
                        {
                            Play_.lTurnScore[i] = readbuffer.ReadLong();
                        }

                        Play_.lCollectScore = new long[GAME_PLAYER];
                        for (byte i = 0; i < GAME_PLAYER; i++)
                        {
                            Play_.lCollectScore[i] = readbuffer.ReadLong();
                        }
                        	    //游戏变量
	                    Play_.cbBombCount = readbuffer.ReadByte();						//炸弹次数
	                    Play_.wBankerUser = readbuffer.ReadUshort();						//庄家用户
	                    Play_.wCurrentUser = readbuffer.ReadUshort();						//当前玩家
	                    Play_.cbBankerScore = readbuffer.ReadByte();						//庄家叫分

	                    //出牌信息
	                    Play_.wTurnWiner = readbuffer.ReadUshort();							//胜利玩家
	                    Play_.cbTurnCardCount = readbuffer.ReadByte();					//出牌数目

                        Play_.cbTurnCardData = new byte[MAX_COUNT];
                        for(byte i =0 ; i < MAX_COUNT;i++)
                        {
                            Play_.cbTurnCardData[i] = readbuffer.ReadByte();
                        }

                        Play_.cbBankerCard = new byte[3];
                        for(byte i =0 ; i < 3;i++)
                        {
                            Play_.cbBankerCard[i] = readbuffer.ReadByte();
                        }

                        Play_.cbHandCardData = new byte[MAX_COUNT];
                        for(byte i =0 ; i < MAX_COUNT;i++)
                        {
                            Play_.cbHandCardData[i] = readbuffer.ReadByte();
                        }

                        Play_.cbHandCardCount = new byte[GAME_PLAYER];
                        for(byte i =0 ; i < GAME_PLAYER;i++)
                        {
                            Play_.cbHandCardCount[i] = readbuffer.ReadByte();
                        }
                        Play_.cbCardData = new byte[GAME_PLAYER][];
                        for (byte i = 0; i < GAME_PLAYER;i++ )
                        {
                            Play_.cbCardData[i] = new byte[MAX_COUNT];
                            for (byte j = 0; j < MAX_COUNT; j++)
                            {
                                Play_.cbCardData[i][j] = readbuffer.ReadByte();
                            }
                        }
                        Play_.cbCurGameCount = readbuffer.ReadByte();
                        Play_.cbAllGameCount = readbuffer.ReadByte();
                        Play_.wSessionCount = readbuffer.ReadUshort();
                        Play_.cbChampionMode = readbuffer.ReadByte();
                        Play_.cbEventnMode = readbuffer.ReadByte();
                        Play_.cbSpecialValue = readbuffer.ReadByte();
                        if (LandInstanceInterFace != null)
                        {
                            LandInstanceInterFace.GAME_SCENE_PLAYING(Play_);
                        }
                    }
                    break;
                default:
                    Error("HandleSceneMsg 未知状态：" + GAME_STATUS_WAIT);
                    break;
            }
            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.GAME_SCENE_OK();
            }
            return true;
        }

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

            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.GAME_MATCH_INFO(msg);
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

            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.GAME_MATCH_PROMOTION(msg);
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

            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.GAME_MATCH_WATI_TIP(msg);
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

            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.GAME_MATCH_RESULT(msg);
            }
            
            return true;
        }
    }

    //空闲状态
    public struct CMD_S_StatusFree
    {
        //游戏单元
	    public int							lCellScore;							//基础积分

	    //时间信息
	    public byte							cbTimeOutCard;						//出牌时间
	    public byte							cbTimeCallScore;					//叫分时间
	    public byte							cbTimeStartGame;					//开始时间
	    public byte							cbTimeHeadOutCard;					//首出时间

	    //历史积分
        public long[]                       lTurnScore;			                //积分信息
        public long[]                       lCollectScore;			            //积分信息

        public byte cbCurGameCount;		//当前局数
        public byte cbAllGameCount;		//总共局数
        public ushort wSessionCount;		//当前场次
        public byte cbChampionMode;		//是否是擂主模式(0表示是)
        public byte cbEventnMode;		//赛事（0=日，1=周，2=月，3=季，4=年冠军)
        public byte cbSpecialValue;		//保留值
    };
    
    //叫分状态
    public struct CMD_S_StatusCall
    {
        //游戏单元
        public int                          lCellScore;							//基础积分

        //时间信息
        public byte                         cbTimeOutCard;						//出牌时间
        public byte                         cbTimeCallScore;					//叫分时间
        public byte                         cbTimeStartGame;					//开始时间
        public byte                         cbTimeHeadOutCard;					//首出时间

        //历史积分
        public long[]                     lTurnScore;			                //积分信息
        public long[]                     lCollectScore;			            //积分信息

	    //游戏信息
        public ushort                       wCurrentUser;				//当前玩家
	    public byte							cbBankerScore;				//庄家叫分
	    public byte[]						cbScoreInfo;			    //叫分信息
	    public byte[]						cbHandCardData;		        //手上扑克

        public byte[][]                     cbPlayerCardData;           //3,20

        public byte cbCurGameCount;		//当前局数
        public byte cbAllGameCount;		//总共局数
        public ushort wSessionCount;		//当前场次
        public byte cbChampionMode;		//是否是擂主模式(0表示是)
        public byte cbEventnMode;		//赛事（0=日，1=周，2=月，3=季，4=年冠军)
        public byte cbSpecialValue;		//保留值
    };
    
    //游戏状态
    public struct CMD_S_StatusPlay
    {
	    //游戏单元
        public int                      lCellScore;							//基础积分
	    //时间信息
	    public byte						cbTimeOutCard;						//出牌时间
	    public byte						cbTimeCallScore;					//叫分时间
	    public byte						cbTimeStartGame;					//开始时间
	    public byte						cbTimeHeadOutCard;					//首出时间
	    //历史积分
        public long[]                   lTurnScore;			                //积分信息
        public long[]                   lCollectScore;			            //积分信息


	    //游戏变量
	    public byte						cbBombCount;						//炸弹次数
	    public ushort					wBankerUser;						//庄家用户
	    public ushort					wCurrentUser;						//当前玩家
	    public byte						cbBankerScore;						//庄家叫分

	    //出牌信息
	    public ushort					wTurnWiner;							//胜利玩家
	    public byte						cbTurnCardCount;					//出牌数目
	    public byte[]					cbTurnCardData;			//出牌数据

	    //扑克信息
	    public byte[]					cbBankerCard;			//游戏底牌
	    public byte[]					cbHandCardData;			//手上扑克
	    public byte[]					cbHandCardCount;		//扑克数目

        public byte[][] cbCardData;		                        //扑克列表

        public byte cbCurGameCount;		//当前局数
        public byte cbAllGameCount;		//总共局数
        public ushort wSessionCount;		//当前场次
        public byte cbChampionMode;		//是否是擂主模式(0表示是)
         public byte cbEventnMode;		//赛事（0=日，1=周，2=月，3=季，4=年冠军)
         public byte cbSpecialValue;		//保留值
    };
}
