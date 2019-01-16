using CYNetwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeatsAndBirds
{
    public partial class BeatsAndBirds_
    {
        #region 常量定义
        /// <summary>
        /// 失败
        /// </summary>
        public const byte SUB_C_ADMIN_COMMDN_S_CR_FAILURE = 0;             //失败
        /// <summary>
        /// 更新成功
        /// </summary>
        public const byte SUB_C_ADMIN_COMMDN_S_CR_UPDATE_SUCCES = 1;       //更新成功
        /// <summary>
        /// 设置成功
        /// </summary>
        public const byte SUB_C_ADMIN_COMMDN_S_CR_SET_SUCCESS = 2;         //设置成功
        /// <summary>
        /// 取消成功
        /// </summary>
        public const byte SUB_C_ADMIN_COMMDN_S_CR_CANCEL_SUCCESS = 3;      //取消成功
        //兔子 //燕子 //鸽子 //孔雀 //老鹰 //狮子 //熊猫 //猴子 //鲨鱼 //飞禽 //走兽
        public const int AREA_COUNT = 11;               //下注区域数目

        //兔子 //燕子 //鸽子 //孔雀 //老鹰 //狮子 //熊猫 //猴子 //银鲨鱼 //金鲨鱼 //通吃 //通陪
        public const int AREA_ALL = 12;                 //动物种类(金鲨鱼跟银鲨鱼需要分开算，但是下注区域一样)
        const int ANIMAL_COUNT = 28;             //动物数量
        const int RATE_TWO_PAIR = 12;            //对子赔率
        const int SERVER_LEN = 32;               //房间长度
        #endregion

        #region 协议ID
        public const int SUB_S_BATCH_JETTON = 97;           //用户批量下注
        public const int SUB_S_CANCEL_JETTON = 98;          //用户取消下注
        public const int SUB_S_GAME_FREE = 99;              //游戏空闲
        public const int SUB_S_GAME_START = 100;            //游戏开始
        public const int SUB_S_PLACE_JETTON = 101;          //用户下注
        public const int SUB_S_GAME_END = 102;              //游戏结束
        public const int SUB_S_APPLY_BANKER = 103;          //申请庄家
        public const int SUB_S_CHANGE_BANKER = 104;         //切换庄家
        public const int SUB_S_CHANGE_USER_SCORE = 105;	    //更新积分
        public const int SUB_S_SEND_RECORD = 106;           //游戏记录
        public const int SUB_S_PLACE_JETTON_FAIL = 107;     //下注失败
        public const int SUB_S_CANCEL_BANKER = 108;         //取消申请
        public const int SUB_S_ADMIN_COMMDN = 110;          //系统控制
        public const int SUB_S_ADMIN_STOCK = 111;           //库存控制
        public const int SUB_S_UPDATE_JACKPOT = 112;        //更新奖池
        public const int SUB_S_UPDATE_SETTING = 113;        //游戏设置更新


        #endregion

        #region 结构体

        public struct CMD_S_CancelJetton
        {
            public ushort wChairID;                         //用户位置
            public long[] lJettonScore;                     //取消筹码数据（对应各筹码区域）
        };

        //下注失败
        public struct CMD_S_PlaceJettonFail
        {
            public UInt16 wPlaceUser;						//下注玩家
            public Byte lJettonArea;						//下注区域
            public Int64 lPlaceScore;						//当前下注
        };

        //更新积分
        public struct CMD_S_ChangeUserScore
        {
            public ushort wChairID;							//椅子号码
            public long lScore;								//玩家积分

            //庄家信息
            //public Byte wCurrentBankerChairID;				//当前庄家
            //public Byte cbBankerTime;						//庄家局数
            //public double lCurrentBankerScore;				//庄家分数
        };

        //申请庄家
        public struct CMD_S_ApplyBanker
        {
            public Byte wApplyUser;							//申请玩家
        };

        //取消申请
        public struct CMD_S_CancelBanker
        {
            public Char[] szCancelUser;      //取消玩家
        };

        //切换庄家
        public struct CMD_S_ChangeBanker
        {
            public Int16 wBankerUser;						//当庄玩家
            public Int64 lBankerScore;						//庄家金币
        };

        //游戏状态
        public struct CMD_S_StatusFree
        {
            //全局信息
            public Byte cbTimeLeave;						//剩余时间

            //玩家信息
            public Int64 lUserMaxScore;						//玩家金币
            public int[] nAnimalPercent;    //开中比例
            //庄家信息
            public Int16 wBankerUser;						//当前庄家
            public Int16 cbBankerTime;						//庄家局数
            public Int64 lBankerWinScore;					//庄家成绩
            public Int64 lBankerScore;						//庄家分数
            public bool bEnableSysBanker;					//系统做庄

            //控制信息
            public Int64 lApplyBankerCondition;				//申请条件
            public Int64 lAreaLimitScore;					//区域限制
            public Char[] szGameRoomName;    //房间名称 
            public int iBetArrayNum;                        //押注可选列表
            public long[] iBetArray;                     //押注可选列表
        };

        //游戏状态
        public struct CMD_S_StatusPlay
        {
            //全局下注
            public Int64[] lAllJettonScore;		//全体总注

            //玩家下注
            public Int64[] lUserJettonScore;    //个人总注

            //玩家积分
            public Int64 lUserMaxScore;						    //最大下注							
            public int[] nAnimalPercent;    //开中比例
            //控制信息
            public Int64 lApplyBankerCondition;				//申请条件
            public Int64 lAreaLimitScore;					//区域限制

            //扑克信息
            public Byte[] cbTableCardArray;   //桌面扑克
            public Byte[] cbShaYuAddMulti;	//附加倍率  

            //庄家信息
            public Int16 wBankerUser;						//当前庄家
            public Int16 cbBankerTime;						//庄家局数
            public Int64 lBankerWinScore;					//庄家赢分
            public Int64 lBankerScore;						//庄家分数
            public bool bEnableSysBanker;					//系统做庄

            //结束信息
            public Int64 lEndBankerScore;					//庄家成绩
            public Int64 lEndUserScore;						//玩家成绩
            public Int64 lEndUserReturnScore;				//返回积分
            public Int64 lEndRevenue;						//游戏税收

            //全局信息
            public Byte cbTimeLeave;						//剩余时间
            public Byte cbGameStatus;						//游戏状态
            public Char[] szGameRoomName;    //房间名称 

            public int iBetArrayNum;                        //押注可选列表
            public long[] iBetArray;                     //押注可选列表
        };

        //游戏空闲
        public struct CMD_S_GameFree
        {
            public Byte cbTimeLeave;						//剩余时间
        };

        //游戏开始
        public struct CMD_S_GameStart
        {
            public ushort wBankerUser;						//庄家位置
            public long lBankerScore;						//庄家金币
            public long lUserMaxScore;						//我的金币
            public byte cbTimeLeave;						//剩余时间	
            public bool bContiueCard;						//继续发牌
            public int nChipRobotCount;					    //人数上限 (下注机器人)
        };

        //用户下注
        public struct CMD_S_PlaceJetton
        {
            public ushort wChairID;							//用户位置
            public Byte cbJettonArea;						//筹码区域
            public Int64 lJettonScore;						//加注数目
            public Byte cbAndroid;							//机器人
        };

        //游戏结束
        public struct CMD_S_GameEnd
        {
            //下局信息
            public Byte cbTimeLeave;						//剩余时间  -----------转的总时间

            //扑克信息
            public Byte[] cbTableCardArray;   //桌面扑克
            public Byte[] cbShaYuAddMulti;	//附加倍率  
            //庄家信息
            public Int64 lBankerScore;						//庄家成绩
            public Int64 lBankerTotallScore;                //庄家成绩
            public int nBankerTime;						    //做庄次数

            //玩家成绩
            public Int64 lUserScore;						//玩家成绩
            public Int64 lUserReturnScore;					//返回积分

            //全局信息
            public Int64 lRevenue;							//游戏税收

            public int[] nAnimalPercent;                    //开中比例
        };

        //更新奖池大小
        public struct CMD_S_UPDATE_JACKPOT
        {
            public Int64 lSystemStock;                      //系统库存
            public Int64 lRobotStock;						//机器人库存
        };


        public struct CMD_S_ControlReturns
        {
            public byte cbReturnsType;             //回复类型
            public byte[] cbControlArea;             //控制区域
            public byte cbControlTimes;            //控制次数
        };

        public struct CMD_C_UPDATE_SETTING
        {
            //几率控制
            public int iAREA_1;                            //兔子 百分之N
            public int iAREA_2;                            //燕子 百分之N
            public int iAREA_3;                            //鸽子 百分之N
            public int iAREA_4;                            //孔雀 百分之N
            public int iAREA_5;                            //老鹰 百分之N
            public int iAREA_6;                            //狮子 百分之N
            public int iAREA_7;                            //熊猫 百分之N
            public int iAREA_8;                            //猴子 百分之N
            public int iAREA_9;                            //银鲨鱼 百分之N
            public int iAREA_10;                           //金鲨鱼 百分之N
            public int iAREA_11;                           //通吃 百分之N
            public int iAREA_12;                           //通陪 百分之N

            //金鲨鱼额外倍数概率
            public int iGOLD_EX_PER1;                      //1 - 30 加权
            public int iGOLD_EX_PER2;                      //31 - 60 加权
            public int iGOLD_EX_PER3;                      //61 - 90 加权
            public int iGOLD_EX_PER4;                      //91 - 120 加权
            public int iTempCode;							//操作码		0：无操作   1：操作成功
        };

        //库存控制
        public struct CMD_S_Update_Stock
        {
            public long lSystemStock;                      //系统库存
            public long lSystemRevenueValue;               //系统抽水数值
            public int iSystemRevenuePer;                  //系统抽水比例 千分比

            public long lTempRevenueValue;                 //库存抽水值
            public long lTempRevenueLimitValue;                //库存抽水阈值
            public int iTempRevenuePer;                    //库存抽水比例 千分比

            public long lRobotStock;                       //机器人库存
            public long lRobotStockInitial;                       //机器人库存起始值
            public long lRobotStockSection;                       //机器人库存增减区间

            public int iTempCode;                          //操作码		0：无操作   1：操作成功
        };

        //用户批量下注
        public struct CMD_S_BatchPlaceJetton
        {
            public ushort wChairID;                            //用户位置
            public long[] lJettonScore;                        //批量加注筹码（对应各筹码区域）
        };

        //记录信息
        public struct CMD_S_SEND_RECORD
        {
            public byte[] bWinMen;						//顺门胜利
        };
        #endregion

        private bool HandleGameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case SUB_S_BATCH_JETTON:
                    return HandBatchJetton(maincmd, subCmd, readbuffer);
                case SUB_S_CANCEL_JETTON:
                    return HandCancelJetton(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_FREE:
                    return HandGameFreeMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_START:
                    return HandleStartMsg(maincmd, subCmd, readbuffer);
                case SUB_S_PLACE_JETTON:
                    return HandlePlaceJetton(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_END:
                    return HandGameEndMsg(maincmd, subCmd, readbuffer);
                case SUB_S_APPLY_BANKER:
                    return HandApplyBanker(maincmd, subCmd, readbuffer);
                case SUB_S_CHANGE_BANKER:
                    return HandChangeBanker(maincmd, subCmd, readbuffer);
                case SUB_S_CHANGE_USER_SCORE:
                    return HandChangeUserScore(maincmd, subCmd, readbuffer);
                case SUB_S_SEND_RECORD:
                    return HandSendRecord(maincmd, subCmd, readbuffer);
                case SUB_S_PLACE_JETTON_FAIL:
                    return HandPlaceJettonFail(maincmd, subCmd, readbuffer);
                case SUB_S_CANCEL_BANKER:
                    return HandCancelBanker(maincmd, subCmd, readbuffer);
                case SUB_S_ADMIN_COMMDN:
                    return HandAdminCommdn(maincmd, subCmd, readbuffer);
                case SUB_S_ADMIN_STOCK:
                    return HandAdminStock(maincmd, subCmd, readbuffer);
                case SUB_S_UPDATE_SETTING:
                    return HandUpdateSetting(maincmd, subCmd, readbuffer);
                case SUB_S_UPDATE_JACKPOT:
                    return HandUpdateJackpot(maincmd, subCmd, readbuffer);
                default:
                    Error("飞禽走兽 未能解析的子命令：" + subCmd);
                    break;
            }
            return true;
        }

        private bool HandBatchJetton(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_BatchPlaceJetton cmd = new CMD_S_BatchPlaceJetton();
            cmd.wChairID = rb.ReadUshort();
            cmd.lJettonScore = new long[AREA_COUNT + 1];
            for (var i = 0; i < cmd.lJettonScore.Length; i++)
            {
                cmd.lJettonScore[i] = rb.ReadLong();
            }

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_BatchPlaceJetton(cmd);
            }
            return true;
        }


        private bool HandCancelJetton(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_CancelJetton cmd = new CMD_S_CancelJetton();
            cmd.wChairID = rb.ReadUshort();
            cmd.lJettonScore = new long[AREA_COUNT];
            for(var i= 0; i < cmd.lJettonScore.Length; i++)
            {
                cmd.lJettonScore[i] = rb.ReadLong();
            } 

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_CancelJetton(cmd);
            }
            return true;
        }

        private bool HandGameFreeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameFree cmd = new CMD_S_GameFree();
            cmd.cbTimeLeave = readbuffer.ReadByte();

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_Free(cmd);
            }
            return true;
        }

        private bool HandleStartMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameStart cmd = new CMD_S_GameStart();
            cmd.wBankerUser = readbuffer.ReadUshort();
            cmd.lBankerScore = readbuffer.ReadLong();
            cmd.lUserMaxScore = readbuffer.ReadLong();
            cmd.cbTimeLeave = readbuffer.ReadByte();
            cmd.bContiueCard = readbuffer.ReadBoolean();            
            cmd.nChipRobotCount = readbuffer.ReadInt();

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_StartGame(cmd);
            }
            return true;
        }

        private bool HandlePlaceJetton(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlaceJetton cmd = new CMD_S_PlaceJetton();
            cmd.wChairID = readbuffer.ReadUshort();
            cmd.cbJettonArea = readbuffer.ReadByte();
            cmd.lJettonScore = readbuffer.ReadLong();
            cmd.cbAndroid = readbuffer.ReadByte();           

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_PlaceJetton(cmd);
            }
            return true;
        }
        private bool HandGameEndMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameEnd cmd = new CMD_S_GameEnd();
            cmd.cbTimeLeave = readbuffer.ReadByte();
            cmd.cbTableCardArray = new byte[3];
            for(int i = 0; i < cmd.cbTableCardArray.Length; i++)
            {
                cmd.cbTableCardArray[i] = readbuffer.ReadByte();
            }
            cmd.cbShaYuAddMulti = new byte[2];
            for (int i = 0; i < cmd.cbShaYuAddMulti.Length; i++)
            {
                cmd.cbShaYuAddMulti[i] = readbuffer.ReadByte();
            }

            cmd.lBankerScore = readbuffer.ReadLong();
            cmd.lBankerTotallScore = readbuffer.ReadLong();
            cmd.nBankerTime = readbuffer.ReadInt();
            cmd.lUserScore = readbuffer.ReadLong();
            cmd.lUserReturnScore = readbuffer.ReadLong();
            cmd.lRevenue = readbuffer.ReadLong();

            cmd.nAnimalPercent = new int[AREA_ALL];
            for(int i = 0; i < cmd.nAnimalPercent.Length; i++)
            {
                cmd.nAnimalPercent[i] = readbuffer.ReadInt();
            }

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_End(cmd);
            }
            return true;
        }
        private bool HandApplyBanker(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            //游戏没用到，暂不处理
            return true;
        }
        private bool HandChangeBanker(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            //游戏没用到，暂不处理
            return true;
        }
        private bool HandChangeUserScore(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ChangeUserScore cmd = new CMD_S_ChangeUserScore();
            cmd.wChairID = readbuffer.ReadUshort();

            cmd.lScore = readbuffer.ReadLong();
            //cmd.wCurrentBankerChairID = readbuffer.ReadByte();
            //cmd.cbBankerTime = readbuffer.ReadByte();
            //cmd.lCurrentBankerScore = readbuffer.ReadDouble();

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_ChangeUserScore(cmd);
            }
            return true;
        }
        private bool HandSendRecord(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            var rcdSize = AREA_COUNT + 1;
            int rcdCount = rb.Length / rcdSize;
            CMD_S_SEND_RECORD[] rcdList = new CMD_S_SEND_RECORD[rcdCount];
            for(var i = 0; i < rcdList.Length; i++)
            {
                CMD_S_SEND_RECORD cmd = new CMD_S_SEND_RECORD();
                cmd.bWinMen = new byte[rcdSize];
                for (var j = 0; j < cmd.bWinMen.Length; j++)
                {
                    cmd.bWinMen[j] = rb.ReadByte();
                }
                rcdList[i] = cmd;
            }

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_SendRecord(rcdList);
            }
            return true;            
        }
        private bool HandPlaceJettonFail(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlaceJettonFail cmd = new CMD_S_PlaceJettonFail();
            cmd.wPlaceUser = readbuffer.ReadUshort();
            cmd.lJettonArea = readbuffer.ReadByte();
            cmd.lPlaceScore = readbuffer.ReadLong();
            
            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_BetFail(cmd);
            }
            return true;
        }
        private bool HandCancelBanker(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            //游戏没用到，暂不处理
            return true;
        }

        private bool HandUpdateJackpot(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_UPDATE_JACKPOT cmd = new CMD_S_UPDATE_JACKPOT();
            cmd.lSystemStock = rb.ReadLong();
            cmd.lRobotStock = rb.ReadLong();
            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_UpdateJackpot(cmd);
            }
            return true;
        }
        

        private bool HandAdminCommdn(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_ControlReturns cmd = new CMD_S_ControlReturns();
            cmd.cbReturnsType = rb.ReadByte();
            cmd.cbControlArea = new byte[2];
            cmd.cbControlArea[0] = rb.ReadByte();
            cmd.cbControlArea[1] = rb.ReadByte();
            cmd.cbControlTimes = rb.ReadByte();

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_AMDIN_COMMAND(cmd);
            }
            return true;            
        }
        private bool HandAdminStock(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_Update_Stock cmd = new CMD_S_Update_Stock();
            cmd.lSystemStock = rb.ReadLong();
            cmd.lSystemRevenueValue = rb.ReadLong();
            cmd.iSystemRevenuePer = rb.ReadInt();
            cmd.lTempRevenueValue = rb.ReadLong();
            cmd.lTempRevenueLimitValue = rb.ReadLong();
            cmd.iTempRevenuePer = rb.ReadInt();
            cmd.lRobotStock = rb.ReadLong();
            cmd.lRobotStockInitial = rb.ReadLong();
            cmd.lRobotStockSection = rb.ReadLong();
            cmd.iTempCode = rb.ReadInt();

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_UpdateStock(cmd);
            }
            return true;            
        }
        private bool HandUpdateSetting(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_C_UPDATE_SETTING cmd = new CMD_C_UPDATE_SETTING();
            cmd.iAREA_1 = rb.ReadInt();
            cmd.iAREA_2 = rb.ReadInt();
            cmd.iAREA_3 = rb.ReadInt();
            cmd.iAREA_4 = rb.ReadInt();
            cmd.iAREA_5 = rb.ReadInt();
            cmd.iAREA_6 = rb.ReadInt();
            cmd.iAREA_7 = rb.ReadInt();
            cmd.iAREA_8 = rb.ReadInt();
            cmd.iAREA_9 = rb.ReadInt();
            cmd.iAREA_10 = rb.ReadInt();
            cmd.iAREA_11 = rb.ReadInt();
            cmd.iAREA_12 = rb.ReadInt();
            cmd.iGOLD_EX_PER1 = rb.ReadInt();
            cmd.iGOLD_EX_PER2 = rb.ReadInt();
            cmd.iGOLD_EX_PER3 = rb.ReadInt();
            cmd.iGOLD_EX_PER4 = rb.ReadInt();

            if (BeatsAndBirdsInterface != null)
            {
                BeatsAndBirdsInterface.Game_UpdateSetting(cmd);
            }
            return true;            
        }
    }
}
