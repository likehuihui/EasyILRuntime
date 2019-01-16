using CYNetwork;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BumperCarBattle
{
    public partial class BumperCarBattle_
    {
        #region 服务器回调命令
        //服务器命令结构
        public const ushort SUB_S_GAME_FREE			= 99;								//游戏空闲
        public const ushort SUB_S_GAME_START		= 100;								//游戏开始
        public const ushort SUB_S_PLACE_JETTON		= 101;								//用户下注
        public const ushort SUB_S_GAME_END			= 102;								//游戏结束
        public const ushort SUB_S_APPLY_BANKER		= 103;								//申请庄家
        public const ushort SUB_S_CHANGE_BANKER		= 104;								//切换庄家
        public const ushort SUB_S_CHANGE_USER_SCORE	= 105;								//更新积分
        public const ushort SUB_S_SEND_RECORD		= 106;								//游戏记录
        public const ushort SUB_S_PLACE_JETTON_FAIL	= 107;								//下注失败
        public const ushort SUB_S_CANCEL_BANKER		= 108;								//取消申请
        public const ushort SUB_S_CHECK_IMAGE		= 109;								//取消申请
        public const ushort SUB_S_ADMIN_COMMDN		= 110;								//系统控制
        #endregion

        #region 服务器返回的结构体

        /// <summary>
        /// 游戏开始
        /// </summary>
        public struct CMD_S_GameStart
        {
            public ushort wBankerUser;						//庄家位置
            public long lBankerScore;						//庄家金币
            public long lUserMaxScore;						//我的金币
            public byte cbTimeLeave;						//剩余时间	
            public bool bContiueCard;						//继续发牌
            public int nChipRobotCount;					//人数上限 (下注机器人)
        };

        //游戏空闲
        public struct CMD_S_GameFree
        {
	        public byte							cbTimeLeave;						//剩余时间
        };

         //用户下注
         public struct CMD_S_PlaceJetton
        {
	       public ushort							wChairID;							//用户位置
	       public byte							cbJettonArea;						//筹码区域
	       public long						lJettonScore;						//加注数目
	       public byte							cbAndroid;							//机器人
        };

        //申请庄家
        public struct CMD_S_ApplyBanker
        {
	        public ushort							wApplyUser;							//申请玩家
        };

        //取消申请
        public struct CMD_S_CancelBanker
        {
	        public string							szCancelUser;					//取消玩家[64]
        };

        //切换庄家
        public struct CMD_S_ChangeBanker
        {
	        public ushort							wBankerUser;						//当庄玩家
	        public long						lBankerScore;						//庄家金币
        };

        //失败结构
        public struct CMD_S_PlaceJettonFail
        {
	        public ushort     						wPlaceUser;							//下注玩家
	        public byte							lJettonArea;						//下注区域
	        public long						lPlaceScore;						//当前下注
        };


        //游戏结束
        public struct CMD_S_GameEnd
        {
	        //下局信息
	        public byte							cbTimeLeave;						//剩余时间

	        //扑克信息
	        public byte[][]							cbTableCardArray;				//桌面扑克[1][1]
	        public byte							cbLeftCardCount;					//扑克数目

	        public byte							bcFirstCard;
 
	        //庄家信息
	        public long						lBankerScore;						//庄家成绩
	        public long						lBankerTotallScore;					//庄家成绩
	        public int								nBankerTime;						//做庄次数

	        //玩家成绩
	        public long						lUserScore;							//玩家成绩
	        public long						lUserReturnScore;					//返回积分

	        //全局信息
	        public long						lRevenue;							//游戏税收
        };


        //记录信息
        public struct tagServerGameRecord
        {
            /// <summary>
            /// 8个
            /// </summary>
	        public byte[]							bWinMen;						//顺门胜利[9]
        };

        //更新积分
        public struct CMD_S_ChangeUserScore
        {
	       public ushort							wChairID;							//椅子号码
	       public double							lScore;								//玩家积分
	        //庄家信息
	       public ushort							wCurrentBankerChairID;				//当前庄家
	       public byte							cbBankerTime;						//庄家局数
	       public double							lCurrentBankerScore;				//庄家分数
        };

        #endregion
        private bool HandleGameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            
            switch (subCmd)
            {
                case SUB_S_GAME_FREE:								//游戏空闲
                    return HandleFreeMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_START:								//游戏开始
                    return HandleStartMsg(maincmd, subCmd, readbuffer);
                case SUB_S_PLACE_JETTON:							//用户下注
                    return HandlePlaceJettonMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_END:								//游戏结束
                    return HandleGameEndMsg(maincmd, subCmd, readbuffer);
                case SUB_S_APPLY_BANKER:								//申请庄家
                    return HandleApplyBankerMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CHANGE_BANKER:							//切换庄家
                    return HandleChangeBankerMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CHANGE_USER_SCORE:							//更新积分
                    return HandleChangeUserScoreMsg(maincmd, subCmd, readbuffer);
                case SUB_S_SEND_RECORD:							//游戏记录
                    return HandleSendRecordMsg(maincmd, subCmd, readbuffer);
                case SUB_S_PLACE_JETTON_FAIL:							//下注失败
                    return HandlePlaceJettonFailMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CANCEL_BANKER:							//取消申请
                    return HandleCancelBankerMsg(maincmd, subCmd, readbuffer);
                default:
                    Error("豪车飘移 未能解析的游戏子命令：" + subCmd);
                    break;
            }
            return true;
        }

//         private bool CheckGameData()
//         {
//             if (BumperCarBattleInterFace != null)
//             {
//                 BumperCarBattleInterFace.CheckGameNet();
//             }
//             return true;
//         }


        private bool HandleFreeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameFree Free = new CMD_S_GameFree();
            Free.cbTimeLeave=readbuffer.ReadByte();

            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Game_FreeGame(Free);
            }
            return true;
        }

        private bool HandleStartMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameStart Start = new CMD_S_GameStart();
            Start.wBankerUser = readbuffer.ReadUshort();
            Start.lBankerScore = readbuffer.ReadLong();
            Start.lUserMaxScore = readbuffer.ReadLong();
            Start.cbTimeLeave = readbuffer.ReadByte();
            Start.bContiueCard = readbuffer.ReadBoolean();
            Start.nChipRobotCount = readbuffer.ReadInt();

            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Game_StartGame(Start);
            }
            return true;
        }

        private bool HandlePlaceJettonMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlaceJetton Jetton = new CMD_S_PlaceJetton();
            Jetton.wChairID = readbuffer.ReadUshort();
            Jetton.cbJettonArea = readbuffer.ReadByte();
            Jetton.lJettonScore = readbuffer.ReadLong();
            Jetton.cbAndroid = readbuffer.ReadByte();
            
            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Game_PlaceJetton(Jetton);
            }
            return true;
        }

        private bool HandleGameEndMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameEnd End = new CMD_S_GameEnd();
            End.cbTimeLeave = readbuffer.ReadByte();
            End.cbTableCardArray = new byte[1][];
            End.cbTableCardArray[0] = new byte[1];
            End.cbTableCardArray[0][0] = readbuffer.ReadByte();

            End.cbLeftCardCount = readbuffer.ReadByte();
            End.bcFirstCard = readbuffer.ReadByte();

            End.lBankerScore = readbuffer.ReadLong();
            End.lBankerTotallScore = readbuffer.ReadLong();
            End.nBankerTime = readbuffer.ReadInt();

            End.lUserScore = readbuffer.ReadLong();
            End.lUserReturnScore = readbuffer.ReadLong();
            End.lRevenue = readbuffer.ReadLong();

            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Game_End(End);
            }
            return true;
        }

        private bool HandleApplyBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ApplyBanker OpUser = new CMD_S_ApplyBanker();
            OpUser.wApplyUser = readbuffer.ReadUshort();
            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Game_ApplyBanker(OpUser);
            }
            return true;
        }
        private bool HandleCancelBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_CancelBanker OpUser = new CMD_S_CancelBanker();
            OpUser.szCancelUser = readbuffer.ReadString(64);
            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Game_CancelBanker(OpUser);
            }
            return true;
        }

        private bool HandleChangeBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ChangeBanker Change = new CMD_S_ChangeBanker();          
            Change.wBankerUser = readbuffer.ReadUshort();
            Change.lBankerScore = readbuffer.ReadLong();

            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Game_ChangeBanker(Change);
            }
            return true;
        }

        private bool HandleChangeUserScoreMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            if (BumperCarBattleInterFace != null)
            {
                //BumperCarBattleInterFace.Game_StartGame(Start);
            }
            return true;
        }
        private bool HandleSendRecordMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            int RecordCount = readbuffer.Length / 9;
            tagServerGameRecord[] Record = new tagServerGameRecord[RecordCount];
            for (int i = 0; i < RecordCount; i++)
            {
                Record[i].bWinMen = new byte[9];
                for (int j = 0; j < 9; j++)
                {
                    Record[i].bWinMen[j] = readbuffer.ReadByte();
                }
            }
            
            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Game_Record(Record);
            }
            return true;
        }
        private bool HandlePlaceJettonFailMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlaceJettonFail Fail = new CMD_S_PlaceJettonFail();
            Fail.wPlaceUser = readbuffer.ReadUshort();
            Fail.lJettonArea = readbuffer.ReadByte();
            Fail.lPlaceScore = readbuffer.ReadLong();

            if (BumperCarBattleInterFace != null)
            {
                BumperCarBattleInterFace.Game_PlaceJettonFail(Fail);
            }
            return true;
        }


        private bool HandleSetBaseScoreMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            int cellScore = readbuffer.ReadInt();
            return true;
        }


    }
}
