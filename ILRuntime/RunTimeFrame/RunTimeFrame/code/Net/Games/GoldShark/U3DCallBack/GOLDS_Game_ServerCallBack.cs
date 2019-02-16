using CYNetwork;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GOLDS
{
    public partial class GOLDS_
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
        public const ushort SUB_S_UPDATE_STORAGE	= 111;								//更新库存
        public const ushort SUB_S_CONTROL_RESULT    = 112;
        #endregion

        #region 服务器返回的结构体
        //失败结构
        public struct CMD_S_PlaceJettonFail
        {
	        public ushort							wPlaceUser;							//下注玩家
	        public byte							lJettonArea;						//下注区域
	        public long						lPlaceScore;						//当前下注
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

        //申请庄家
        public struct CMD_S_ApplyBanker
        {
	        public ushort							wApplyUser;							//申请玩家
        };

        //取消申请
        public struct CMD_S_CancelBanker
        {
	        public string							szCancelUser;	//[64]				//取消玩家
        };

        //切换庄家
        public struct CMD_S_ChangeBanker
        {
	        public ushort							wBankerUser;						//当庄玩家
	        public long						lBankerScore;						//庄家金币
        };

        //游戏状态
        public struct CMD_S_StatusFree
        {
	        //全局信息
	        public Byte							cbTimeLeave;						//剩余时间

	        //玩家信息
	        public Int64						lUserMaxScore;						//玩家金币
            public Int64 lStorageScore;						//库存
	        public int[]							nAnimalPercent;	//[9]		//开中比例
	        //庄家信息
	        public ushort							wBankerUser;						//当前庄家
	        public ushort							cbBankerTime;						//庄家局数
	        public long						lBankerWinScore;					//庄家成绩
	        public long						lBankerScore;						//庄家分数
	        public bool							bEnableSysBanker;					//系统做庄

	        //控制信息
	        public long						lApplyBankerCondition;				//申请条件
	        public long						lAreaLimitScore;					//区域限制
	
	        public string							szGameRoomName;		//[32]			//房间名称 
        };


        //游戏状态
        public struct CMD_S_StatusPlay
        {
	        //全局下注
	        public long[]						lAllJettonScore;		//全体总注[12]
	        public long						lStorageScore;						//库存

	        //玩家下注
	        public long[]						lUserJettonScore;		//个人总注[12]

	        //玩家积分
	        public long						lUserMaxScore;						//最大下注							
	        public int[]								nAnimalPercent;			//开中比例[9]
	        //控制信息
	        public long						lApplyBankerCondition;				//申请条件
	        public long						lAreaLimitScore;					//区域限制

	        //扑克信息
	        public byte[]							cbTableCardArray;				//桌面扑克[2]

	        //庄家信息
	        public ushort							wBankerUser;						//当前庄家
	        public ushort							cbBankerTime;						//庄家局数
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
	        public string							szGameRoomName;					//房间名称 [64]
        };

        //游戏空闲
        public struct CMD_S_GameFree
        {
	        public byte							cbTimeLeave;						//剩余时间
	        public long						lStorageScore;						//库存
        };

        //游戏开始
        public struct CMD_S_GameStart
        {
	        public ushort							wBankerUser;						//庄家位置
	        public Int64						lBankerScore;						//庄家金币
            public Int64 lUserMaxScore;					//我的金币
	        public byte							cbTimeLeave;						//剩余时间	
	        public bool							bContiueCard;						//继续发牌
	        public int								nChipRobotCount;					//人数上限 (下注机器人)
        };

        //用户下注
        public struct CMD_S_PlaceJetton
        {
	        public ushort							wChairID;							//用户位置
	        public byte							cbJettonArea;						//筹码区域
	        public long						lJettonScore;						//加注数目
	        public byte							cbAndroid;							//机器人
        };

        //游戏结束
        public struct CMD_S_GameEnd
        {
	        //下局信息
	        public byte							cbTimeLeave;						//剩余时间

	        //扑克信息
            public byte[] cbTableCardArray;				//桌面扑克[2]
	        public byte                            cbShaYuAddMulti;					//附加倍率
	        //庄家信息
	        public long						lBankerScore;						//庄家成绩
	        public long						lBankerTotallScore;					//庄家成绩
	        public int								nBankerTime;						//做庄次数

	        //玩家成绩
	        public long						lUserScore;							//玩家成绩
	        public long						lUserReturnScore;					//返回积分

	        //全局信息
	        public long						lRevenue;							//游戏税收
	        public int[]								nAnimalPercent;			//开中比例[9]
        };

        //更新库存
        public struct CMD_S_UpdateStorage
        {
	        public long						lStorage;							//新库存值
	        public long						lStorageDeduct;						//库存衰减
        };

        public struct CMD_S_ControlApplication
        {
	        public byte cbControlAppType;			//申请类型
	        public byte cbControlArea;				//控制区域
	        public byte cbControlTimes;			//控制次数
        };

        //游戏记录
        public struct tagGameRecord
        {
            //游戏信息
            public ushort wDrawCount;							//游戏局数	
            public byte cbWinerSide;						//胜利玩家
            public byte cbPlayerPoint;						//闲家牌点
            public byte cbBankerPoint;						//庄家牌点	

            //我的信息
            public long lGameScore;							//游戏成绩	
            public long lMyAddGold;							//游戏下注	

            //下注总量
            public long lDrawTieScore;						//买平总注
            public long lDrawBankerScore;					//买庄总注
            public long lDrawPlayerScore;					//买闲总注
        };

        //记录信息
        public struct tagServerGameRecord
        {
            public byte[] cbWinMen;				//顺门胜利[AREA_COUNT+1]
            public byte cbWinIndex;							//索引
            public byte cbWinIndexAdded;					//附加索引
        };


        #endregion
        private bool HandleGameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            Console.WriteLine("金鲨银鲨子命令 " + subCmd);
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
                case SUB_S_CHECK_IMAGE:
                    return HandleCHECK_IMAGE(maincmd,subCmd,readbuffer);
                case SUB_S_ADMIN_COMMDN:
                    return HandleADMIN_COMMDN(maincmd, subCmd, readbuffer);
                case SUB_S_UPDATE_STORAGE:
                    return HandleUPDATE_STORAGE(maincmd, subCmd, readbuffer);
                case SUB_S_CONTROL_RESULT:
                    return HandleCONTROL_RESULT(maincmd, subCmd, readbuffer);
                default:
                    Error("金鲨银鲨 未能解析的游戏子命令：" + subCmd);
                    break;
            }
            return true;
        }

//         private bool CheckGameData()
//         {
//             if (GOLDSInterFace != null)
//             {
//                 GOLDSInterFace.CheckGameNet();
//             }
//             return true;
//         }


        private bool HandleFreeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameFree Free = new CMD_S_GameFree();
            Free.cbTimeLeave=readbuffer.ReadByte();
            Free.lStorageScore=readbuffer.ReadLong();

            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Game_FreeGame(Free);
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
            
            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Game_StartGame(Start);
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

            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Game_PlaceJetton(Jetton);
            }
            return true;
        }

        private bool HandleGameEndMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameEnd End = new CMD_S_GameEnd();
            End.cbTimeLeave = readbuffer.ReadByte();
            End.cbTableCardArray=new byte[2];
            End.cbTableCardArray = readbuffer.ReadBytes(2);
            End.cbShaYuAddMulti = readbuffer.ReadByte();
            End.lBankerScore = readbuffer.ReadLong();
            End.lBankerTotallScore = readbuffer.ReadLong();
            End.nBankerTime = readbuffer.ReadByte();
            End.lUserScore = readbuffer.ReadByte();
            End.lUserReturnScore = readbuffer.ReadByte();
            End.lRevenue = readbuffer.ReadByte();
            End.nAnimalPercent = new int[9];
            for (int i = 0; i < End.nAnimalPercent.Length; i++)
            {
                End.nAnimalPercent[i] = readbuffer.ReadInt();
            }

            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Game_End(End);
            }
            return true;
        }

        private bool HandleApplyBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ApplyBanker OpUser = readbuffer.ReadT<CMD_S_ApplyBanker>();
            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Game_ApplyBanker(OpUser);
            }
            return true;
        }
        private bool HandleCancelBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_CancelBanker OpUser = new CMD_S_CancelBanker();
            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Game_CancelBanker(OpUser);
            }
            return true;
        }

        private bool HandleChangeBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ChangeBanker Change = readbuffer.ReadT<CMD_S_ChangeBanker>();

            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Game_ChangeBanker(Change);
            }
            return true;
        }

        private bool HandleChangeUserScoreMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            if (GOLDSInterFace != null)
            {
                //GOLDSInterFace.Game_StartGame(Start);
            }
            return true;
        }
        private bool HandleSendRecordMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            //Console.WriteLine(sizeof());

            tagServerGameRecord[] record = new tagServerGameRecord[readbuffer.Length / 14];
            for (int i = 0; i < record.Length; i++)
            {

                record[i].cbWinMen = new byte[12];
                for (int j = 0; j < record[i].cbWinMen.Length; j++)
                {
                    record[i].cbWinMen[j]=readbuffer.ReadByte();
                }
                record[i].cbWinIndex = readbuffer.ReadByte();
                record[i].cbWinIndexAdded = readbuffer.ReadByte();
                /*record[i].wDrawCount = readbuffer.ReadUshort();
                record[i].cbWinerSide = readbuffer.ReadByte();
                record[i].cbPlayerPoint = readbuffer.ReadByte();
                record[i].cbBankerPoint = readbuffer.ReadByte();
                record[i].lGameScore = readbuffer.ReadLong();
                record[i].lMyAddGold = readbuffer.ReadLong();
                record[i].lDrawTieScore = readbuffer.ReadLong();
                record[i].lDrawBankerScore = readbuffer.ReadLong();
                record[i].lDrawPlayerScore = readbuffer.ReadLong();*/
            }
            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Game_Record(record);
            }
            return true;
        }
        private bool HandlePlaceJettonFailMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlaceJettonFail Fail = new CMD_S_PlaceJettonFail();
            Fail.wPlaceUser=readbuffer.ReadUshort();
            Fail.lJettonArea=readbuffer.ReadByte();
            Fail.lPlaceScore=readbuffer.ReadLong();

            if (GOLDSInterFace != null)
            {
                GOLDSInterFace.Game_PlaceJettonFail(Fail);
            }
            return true;
        }


        private bool HandleSetBaseScoreMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            int cellScore = readbuffer.ReadInt();
            return true;
        }

        private bool HandleUpdateStorage(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            return true;
        }

        private bool HandleSetPrize(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            return true;
        }

        private bool HandleCHECK_IMAGE(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            return true;
        }
        private bool HandleADMIN_COMMDN(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            return true;
        }
        private bool HandleUPDATE_STORAGE(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            return true;
        }
        private bool HandleCONTROL_RESULT(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            return true;
        }
    }
}
