using CYNetwork;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Land
{
    public partial class Land_
    {
        #region 服务器回调命令
        //服务器命令结构

        public const ushort SUB_S_GAME_START	=		100;								//游戏开始
        public const ushort SUB_S_CALL_SCORE	=		101;								//用户叫分
        public const ushort SUB_S_BANKER_INFO	=		102;								//庄家信息
        public const ushort SUB_S_OUT_CARD		=		103;								//用户出牌
        public const ushort SUB_S_PASS_CARD		=		104;								//用户放弃
        public const ushort SUB_S_GAME_CONCLUDE	=		105;								//游戏结束
        public const ushort SUB_S_SET_BASESCORE	=		106;								//设置基数
        public const ushort SUB_S_CHEAT_CARD	=		107;								//作弊扑克
        public const ushort SUB_S_TRUSTEE		=		108;								//托管
        #endregion
        #region 服务器返回的结构体
        /// <summary>
        /// 游戏开始
        /// </summary>
        public struct CMD_S_GameStart
        {
            public ushort						wStartUser;								//开始玩家
	        public ushort				 		wCurrentUser;							//当前玩家
	        public byte							cbValidCardData;						//明牌扑克
	        public byte							cbValidCardIndex;						//明牌位置
	        public byte[][]						cbCardData;		                        //扑克列表

            public byte cbCurGameCount;		//当前局数
            public byte cbAllGameCount;		//总共局数
            public ushort wSessionCount;		//当前场次
            public byte cbChampionMode;		//是否是擂主模式(0表示是)
            public byte cbEventnMode;		//赛事（0=日，1=周，2=月，3=季，4=年冠军)
            public byte cbSpecialValue;		//保留值
        };
        /// <summary>
        /// 用户叫分
        /// </summary>
        public struct CMD_S_CallScore
        {
            public ushort           wCurrentUser;						//当前玩家
            public ushort           wCallScoreUser;						//叫分玩家
            public byte             cbCurrentScore;						//当前叫分
            public byte             cbUserCallScore;					//上次叫分
        };
        /// <summary>
        /// 庄家信息
        /// </summary>
        public struct CMD_S_BankerInfo
        {
	        public ushort				 			wBankerUser;						//庄家玩家
	        public ushort				 			wCurrentUser;						//当前玩家
	        public byte							    cbBankerScore;						//庄家叫分
	        public byte[]							cbBankerCard;					    //庄家扑克
            public byte                             cbCardCount;
            public byte[]                           cbCardData;
    
        };

        /// <summary>
        /// 用户出牌
        /// </summary>
        public struct CMD_S_OutCard
        {
	        public byte							cbCardCount;						//出牌数目
	        public ushort				 		wCurrentUser;						//当前玩家
	        public ushort						wOutCardUser;						//出牌玩家
	        public byte[]						cbCardData;				//扑克列表
            public byte                         cbOutCardCount;
            public byte[]                       cbOutCardData;
  
        };

        /// <summary>
        /// 用户放弃出牌
        /// </summary>
        public struct CMD_S_PassCard
        {
            public byte     cbTurnOver;						//一轮结束
            public ushort   wCurrentUser;						//当前玩家
            public ushort   wPassCardUser;						//放弃玩家
        };

        /// <summary>
        /// 游戏结束
        /// </summary>
        public struct CMD_S_GameConclude
        {
	        //积分变量
	        public long							lCellScore;							//单元积分
	        public long[]						lGameScore;			//游戏积分

	        //春天标志
	        public byte							bChunTian;							//春天标志
	        public byte							bFanChunTian;						//春天标志

	        //炸弹信息
	        public byte							cbBombCount;						//炸弹个数
	        public byte[]						cbEachBombCount;		//炸弹个数

	        //游戏信息
	        public byte							cbBankerScore;						//叫分数目
	        public byte[]						cbCardCount;			//扑克数目
            public byte[]                       cbHandCardData;			//扑克列表
            public byte                         cbChampionMode;
        };

        /// <summary>
        /// 托管
        /// </summary>
        public struct CMD_S_TRUSTEE
        {
            public ushort   wTrusteeUser;						//托管玩家
            public byte     bTrustee;							//托管标志
        };

        #endregion
        private bool HandleGameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case SUB_S_GAME_START:
                    return HandleStartMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CALL_SCORE:
                    return HandleCallMsg(maincmd, subCmd, readbuffer);
                case SUB_S_BANKER_INFO:
                    return HandleBankerInfoMsg(maincmd, subCmd, readbuffer);
                case SUB_S_OUT_CARD:
                    return HandleOutCardMsg(maincmd, subCmd, readbuffer);
                case SUB_S_PASS_CARD:
                    return HandlePassMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_CONCLUDE:
                    return HandleConludeMsg(maincmd, subCmd, readbuffer);
                case SUB_S_TRUSTEE:
                    return HandleTrusteeMsg(maincmd, subCmd, readbuffer);
                case SUB_S_SET_BASESCORE:
                    return HandleSetBaseScoreMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CHEAT_CARD:
                    return HandleCheatCardMsg(maincmd, subCmd, readbuffer);
                default:
                    Error("斗地主 未能解析的游戏子命令：" + subCmd);
                    break;
            }
            return true;
        }

        private bool HandleStartMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameStart Start = new CMD_S_GameStart();
            Start.wStartUser = readbuffer.ReadUshort();
            Start.wCurrentUser = readbuffer.ReadUshort();
            Start.cbValidCardData = readbuffer.ReadByte();
            Start.cbValidCardIndex = readbuffer.ReadByte();
            //Dictionary<ushort, byte[]> Dic = new Dictionary<ushort, byte[]>();
            Start.cbCardData = new byte[GAME_PLAYER][];
            for (ushort i = 0; i < GAME_PLAYER;i++ )
            {
                Start.cbCardData[i] = new byte[MAX_COUNT];
                for (byte j = 0; j < MAX_COUNT; j++ )
                {
                    Start.cbCardData[i][j] = readbuffer.ReadByte();
                   
                }
                //Dic.Add(i, Start.cbCardData[i]);
            }
            Start.cbCurGameCount = readbuffer.ReadByte();
            Start.cbAllGameCount = readbuffer.ReadByte();
            Start.wSessionCount = readbuffer.ReadUshort();
            Start.cbChampionMode = readbuffer.ReadByte();
            Start.cbEventnMode = readbuffer.ReadByte();
            Start.cbSpecialValue = readbuffer.ReadByte();
            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.Game_StartGame(Start);
                //ShuiHuZhuanInterFace.GAME_Dic_PuKe(Dic);
            }
            return true;
        }
//         private bool CheckGameData()
//         {
//             if (ShuiHuZhuanInterFace != null)
//             {
//                 ShuiHuZhuanInterFace.CheckGameNet();
//             }
//             return true;
//         }
        private bool HandleCallMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_CallScore Call = new CMD_S_CallScore();
            Call.wCurrentUser = readbuffer.ReadUshort();						//当前玩家
            Call.wCallScoreUser = readbuffer.ReadUshort();						//叫分玩家
            Call.cbCurrentScore = readbuffer.ReadByte();						//当前叫分
            Call.cbUserCallScore = readbuffer.ReadByte();					//上次叫分

            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.Game_UserCall(Call);
            }
            return true;
        }
        private bool HandleBankerInfoMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_BankerInfo Banker = new CMD_S_BankerInfo();
            Banker.wBankerUser = readbuffer.ReadUshort();						//庄家玩家
            Banker.wCurrentUser = readbuffer.ReadUshort();						//当前玩家
            Banker.cbBankerScore = readbuffer.ReadByte();						//庄家叫分
            Banker.cbBankerCard = new byte[3];
            for (byte i = 0; i < 3; i++ )
            {
                Banker.cbBankerCard[i] = readbuffer.ReadByte();
            }
            Banker.cbCardCount = readbuffer.ReadByte();
            Banker.cbCardData = new byte[Banker.cbCardCount];
            for (int i = 0; i < Banker.cbCardCount; i++)
            {
                Banker.cbCardData[i] = readbuffer.ReadByte();
            }
            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.GAME_BankerInfo(Banker);
            }
            return true;
        }

        private bool HandleOutCardMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_OutCard OutCard = new CMD_S_OutCard();
            OutCard.cbCardCount = readbuffer.ReadByte();						//庄家玩家
            OutCard.wCurrentUser = readbuffer.ReadUshort();						//当前玩家
            OutCard.wOutCardUser = readbuffer.ReadUshort();						//庄家叫分
            OutCard.cbCardData = new byte[OutCard.cbCardCount];
            for (byte i = 0; i < OutCard.cbCardCount; i++ )
            {
                OutCard.cbCardData[i] = readbuffer.ReadByte();
            }
            if (MAX_COUNT - OutCard.cbCardCount  > 0)
                readbuffer.OffsetCurPos(MAX_COUNT - OutCard.cbCardCount);
            OutCard.cbOutCardCount = readbuffer.ReadByte();
            OutCard.cbOutCardData = new byte[OutCard.cbOutCardCount];
            for (int i = 0; i < OutCard.cbOutCardCount;i++ )
            {
                OutCard.cbOutCardData[i] = readbuffer.ReadByte();
            }
            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.GAME_OutCardInfo(OutCard);
            }
            return true;
        }

        private bool HandlePassMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PassCard pass = new CMD_S_PassCard();
            pass.cbTurnOver = readbuffer.ReadByte();						//一轮结束
            pass.wCurrentUser= readbuffer.ReadUshort();						//当前玩家
            pass.wPassCardUser= readbuffer.ReadUshort();					//放弃玩家
            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.Game_Pass(pass);
            }
            return true;
        }

        private bool HandleConludeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameConclude Conlude = new CMD_S_GameConclude();
	        //积分变量
	        Conlude.lCellScore = readbuffer.ReadInt();							//单元积分

            Conlude.lGameScore = new long[GAME_PLAYER];
            for(ushort i = 0; i < GAME_PLAYER; i++)
            {
                Conlude.lGameScore[i] = readbuffer.ReadLong();
            }

	        Conlude.bChunTian = readbuffer.ReadByte();							//春天标志
	        Conlude.bFanChunTian = readbuffer.ReadByte();						//春天标志

	        Conlude.cbBombCount = readbuffer.ReadByte();						//炸弹个数

            Conlude.cbEachBombCount = new byte[GAME_PLAYER];
            for(ushort i = 0; i < GAME_PLAYER; i++)
            {
                Conlude.cbEachBombCount[i] = readbuffer.ReadByte();
            }
            Conlude.cbBankerScore = readbuffer.ReadByte();						//叫分数目
            
            Conlude.cbCardCount = new byte[GAME_PLAYER];
            for(ushort i = 0; i < GAME_PLAYER; i++)
            {
                Conlude.cbCardCount[i] = readbuffer.ReadByte();
            }
            Conlude.cbHandCardData = new byte[FULL_COUNT];
            for (ushort j = 0; j < Land_.FULL_COUNT; j++)
            {
                Conlude.cbHandCardData[j] = readbuffer.ReadByte();
            }
            Conlude.cbChampionMode = readbuffer.ReadByte();
            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.Game_End(Conlude);
            }
            return true;
        }

        private bool HandleTrusteeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_TRUSTEE Trustee = new CMD_S_TRUSTEE();
            Trustee.wTrusteeUser = readbuffer.ReadUshort();						//托管玩家
            Trustee.bTrustee = readbuffer.ReadByte();							//托管标志

            if (LandInstanceInterFace != null)
            {
                LandInstanceInterFace.Game_Trustee(Trustee);
            }
            return true;
        }

        private bool HandleSetBaseScoreMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            int cellScore = readbuffer.ReadInt();
            return true;
        }

        private bool HandleCheatCardMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            return true;
        }

        //         private bool HandlePuKeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        //         {
        //             CMD_S_SendCard sc = new CMD_S_SendCard();
        //             sc.cbCardData = new byte[2, 5];
        //             Dictionary<ushort, byte[]> Dic = new Dictionary<ushort, byte[]>();
        //             for (ushort j = 0; j < 2; j++)
        //             {
        //                 byte[] card = new byte[5];
        // 
        //                 for (int i = 0; i < 5; i++)
        //                 {
        //                     card[i] = readbuffer.ReadByte();
        //                     sc.cbCardData[j, i] = card[i];
        //                 }
        //                 Dic.Add(j, card);
        //             }
        //             if (ShuiHuZhuanInterFace != null)
        //             {
        //                 ShuiHuZhuanInterFace.GAME_Dic_PuKe(Dic);
        //             }
        //             return true;
        //         }

    }
}
