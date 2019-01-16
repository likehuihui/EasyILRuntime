using CYNetwork;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OxBattle
{
    public partial class OxBattle_
    {
        /// <summary>
        /// ////////////////////////////////////////////code by kingsley  6/27/2016 ///////////////////////
        /// </summary>
        #region 服务器回调命令


        public const ushort SUB_S_GAME_FREE = 99;								//游戏空闲
        public const ushort SUB_S_GAME_START = 100;							//游戏开始
        public const ushort SUB_S_PLACE_JETTON = 101;							//用户下注
        public const ushort SUB_S_GAME_END = 102;							//游戏结束
        public const ushort SUB_S_APPLY_BANKER = 103;							//申请庄家
        public const ushort SUB_S_CHANGE_BANKER = 104;							//切换庄家
        public const ushort SUB_S_CHANGE_USER_SCORE = 105;							//更新积分
        public const ushort SUB_S_SEND_RECORD = 106;							//游戏记录
        public const ushort SUB_S_PLACE_JETTON_FAIL = 107;							//下注失败
        public const ushort SUB_S_CANCEL_BANKER = 108;                           //取消申请
        public const ushort SUB_S_BANKER_LIST = 109;							//上庄列表
        public const ushort SUB_S_AMDIN_COMMAND = 110;							//管理员命令
        public const ushort SUB_S_UPDATE_STORAGE = 111;                           //更新库存

        public const ushort SUB_S_REFRESH_TRUTH_PLAYER = 112;							//刷新真实玩家
        public const ushort SUB_S_STORAGE_ALLINFO = 121;							//难度信息
        public const ushort SUB_GR_USER_INSURE_SUCCESS = 101;                               //银行成功
        public const ushort SUB_GR_USER_INSURE_FAILURE = 102;                               //银行失败

        public const ushort SUB_S_CONTINUE_JETTON = 113;									//续押注
        public const ushort SUB_S_CONTINUE_JETTON_FAIL = 114;                                   //续押注失败

        public const ushort SUB_S_GAME_HEART = 200;									//游戏心跳
        #endregion
        #region 服务器返回的结构体



        /// <summary>
        /// 难度信息
        /// </summary>
        public struct CMD_S_Storage_AllInfo
        {
            public long lStorageStart;                      //库存数值
            public int nStorageDeduct;                      //扣取比例
            public long lStorageDeductAll;					//扣除库存

            //库存抽水率
            public long tempRevenueValue;
            //库存抽水仓库数值
            public long limitTempRevenueValue;
            //库存抽水仓库存入库存的要求数值
            public int tempRevenuePer;


            public long lWinRate;                               //当前难度
            public int[] uiWinRate;                         //难度档位概率
            public int RateTimeMin;
            public int RateTimeMax;
            public int RateStockMin;
            public int RateStockMax;
            public bool bIsGameCheatUser;                   //是否是管理用户
        };

        /// <summary>
        /// 空闲状态
        /// </summary>
        public struct CMD_S_GameFree
        {
            public byte cbTimeLeave;                       //剩余时间    
            public int nListUserCount;                     //列表人数  
            public long lStorageStart;                     //库存（彩池）
        };


        /// <summary>
        /// 游戏开始
        /// </summary>
        public struct CMD_S_GameStart
        {
            public ushort wBankerUser;						//当前庄家
            public long lBankerScore;                     //库庄家金币
            public long lUserMaxScore;                     //我的金币
            public byte cbTimeLeave;                       //剩余时间 
            public ushort wBankerTimesLimit;					//庄家上庄上限    
            public bool ContiueCard;					//系统做庄
            public int nChipRobotCount;                     ///人数上限k
        };

        /// <summary>
        /// 游戏结束
        /// </summary>
        public struct CMD_S_GameEnd
        {
            public byte cbTimeLeave;                       //剩余时间
            public byte[][] cbTableCardArray;              //桌面扑克
            public byte cbLeftCardCount;					//扑克数目
            public byte bcFirstCard;                          //
            public long lBankerScore;						//庄家成绩
            public long lBankerTotallScore;					//庄家成绩
            public int nBankerTime;                         //做庄次数
            public long[] lCurUserScore;                    //玩家当次成绩
            public long[] lCurUserReturnScore;              //玩家当次返回积分
            public long lUserScore;						//玩家成绩
            public long lUserReturnScore;					//返回积分
            public long lRevenue;                     //游戏税收                                                  
        };

        /// <summary>
        /// 用户下注
        /// </summary>
        public struct CMD_S_PlaceJetton
        {
            public ushort wChairID;                          //玩家位置
            public byte cbJettonArea;                       //筹码区域
            public long lJettonScore;                          //下注数量   
            public bool bIsAndroid;					//是否机器人
            public bool bAndroid;					//是否机器人           
        };


        /// <summary>
        /// 下注失败
        /// </summary>
        public struct CMD_S_PlaceJettonFail
        {
            public ushort wChairID;                          //玩家位置
            public byte lJettonArea;                       //下注动物
            public long lPlaceScore;							//筹码数量
            public ushort wFailReason;						//失败原因emJettonFailReason
        };


        //记录信息 续压信息
        public struct stUserJetton
        {
            public byte cbJettonArea;                      //筹码区域
            public long lJettonScore;                      //加注数目

        };


        //续押注失败
        public struct CMD_S_ContinueJettonFail     //SUB_S_CONTINUE_JETTON_FAIL
        {
            public ushort wChairID;                          //下注玩家位置
            public stUserJetton[] szUserJetton;          //各个区域押注AREA_COUNT=4

        };

        //续押注
      public  struct CMD_S_ContinueJetton     //SUB_S_CONTINUE_JETTON
        {
            public ushort wChairID;                          //玩家位置
            public stUserJetton[] szUserJetton;          //各个区域押注
        };


        /// <summary>
        /// 更新积分
        /// </summary>
        public struct CMD_S_ChangeUserScore
        {
            public ushort wChairID;                          //玩家位置
            public long lScore;                             //玩家积分	     
            public long wCurrentBankerChairID;              //当前庄家
            public byte cbBankerTime;                       //庄家局数
            public long lCurrentBankerScore;				//庄家分数     
        };


        /// <summary>
        /// 申请庄家
        /// </summary>
        public struct CMD_S_ApplyBanker
        {
            public ushort wChairID;                          //申请玩家
        };


        public struct CMD_S_BankerList
        {
            public ushort m_wIsEnd;                          //是否发送完毕 1表示完毕
            public ushort m_wApplyerNum;                     //申请人数
            public ushort[] m_szApplyerList;      //申请者座位号
        };

        //真实玩家
        public struct stTruthPlayerInfo
        {
            public uint dwRobotFlag;                      //机器人标识,0表示不是机器人,1表示机器人
            public uint dwUserGameID;                     //用户 游戏I D
            public string szNickName;         //用户昵称
            public long lScore;                               //用户分数
            public uint dwTableID;                            //桌号
        };

        //刷新真实玩家
        public struct CMD_S_RefreshTruthPlayerInfo         //SUB_S_REFRESH_TRUTH_PLAYER
        {
            public int m_nIsEnd;                               //是否结束 1表示结束 0表示未结束
            public int m_nIsGameCheatUser;                     //是否是管理用户 1表示是 0表示不是
            public int m_nPlayerNum;                           //	真实玩家个数
            public stTruthPlayerInfo[] m_szTruthPlayerInfo;             //	真实玩家信息        
        };


        /// <summary>
        /// 取消申请
        /// </summary>
        public struct CMD_S_CancelBanker
        {
            public ushort wChairID;                          //取消申请
        };

        /// <summary>
        /// 切换庄家
        /// </summary>
        public struct CMD_S_ChangeBanker
        {
            public ushort wChairID;                    //取消申请
            public long lBankerScore;				//庄家金币  
        };

        //更新库存
        public struct CMD_S_UpdateStorage
        {
            public long lStorage;							//新库存值
            public long lStorageDeduct;						//库存衰减
            public long lStorageDeductAll;                  //扣除库存
                                                            //库存抽水率

            //库存抽水率
            public long tempRevenueValue;
            //库存抽水仓库数值
            public long limitTempRevenueValue;
            //库存抽水仓库存入库存的要求数值
            public int tempRevenuePer;
        };
        //银行成功
        public struct CMD_GR_S_UserInsureSuccess
        {
            public byte cbActivityGame;                     //游戏动作
            public long lUserScore;                         //身上金币
            public long lUserInsure;                        //银行金币
            public string szDescribeString;             //描述消息128
        };

        //银行失败
        public struct CMD_GR_S_UserInsureFailure
        {
            public byte cbActivityGame;                     //游戏动作
            public long lErrorCode;                         //错误代码
            public string szDescribeString;             //描述消息128
        };
        #endregion
        private bool HandleGameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case SUB_S_GAME_FREE:
                    return HandGameFreeMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_START:
                    return HandleStartMsg(maincmd, subCmd, readbuffer);
                case SUB_S_PLACE_JETTON:
                    return HandBetMsg(maincmd, subCmd, readbuffer);
                case SUB_S_PLACE_JETTON_FAIL:
                    return HandBetFailMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_END:
                    return HandleGameEndMsg(maincmd, subCmd, readbuffer);
                case SUB_S_APPLY_BANKER:
                    return ApplyBankerMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CANCEL_BANKER:
                    return CancelBankerMsg(maincmd, subCmd, readbuffer);
                case SUB_S_BANKER_LIST:
                    return BankerListMSg(maincmd, subCmd, readbuffer);
                case SUB_S_CHANGE_BANKER:
                    return ChangeBankerMsg(maincmd, subCmd, readbuffer);
                case SUB_S_SEND_RECORD:
                    return SendRecordMsg(maincmd, subCmd, readbuffer);
                case SUB_S_REFRESH_TRUTH_PLAYER:
                    return RealPlayerMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CONTINUE_JETTON:
                    return BetAgainMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CONTINUE_JETTON_FAIL:
                    return BetAgainFailMsg(maincmd, subCmd, readbuffer);
                case SUB_S_STORAGE_ALLINFO:
                    return StorageAllInfoMsg(maincmd, subCmd, readbuffer);
                case SUB_S_UPDATE_STORAGE:
                    return UpDateStroageMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_HEART:
                    return HeartBeatMsg(maincmd, subCmd, readbuffer);
                default:
                    Error("百人牛牛 未能解析的子命令：" + subCmd);
                    break;
            }
            return true;
        }
        private bool HandleInsureMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case SUB_GR_USER_INSURE_SUCCESS:
                    return InsureSuccessMsg(maincmd, subCmd, readbuffer);
                case SUB_GR_USER_INSURE_FAILURE:
                    return InsureFailureMsg(maincmd, subCmd, readbuffer);
                default:
                    Error("百人牛牛 未能解析的子命令：" + subCmd);
                    break;
            }
            return true;
        }




        //记录信息
        public struct tagServerGameRecord
        {
            public bool bWinShunMen;						//顺门胜利
            public bool bWinDuiMen;							//对门胜利
            public bool bWinDaoMen;							//倒门胜利
            public bool bWinHuang;							//倒门胜利
        };
        private bool SendRecordMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            int nLenght = readbuffer.Length / 4;

            tagServerGameRecord[] GameRecord = new tagServerGameRecord[nLenght];
            for (int i = 0; i < nLenght; i++)
            {
                GameRecord[i].bWinShunMen = readbuffer.ReadBoolean();
                GameRecord[i].bWinDuiMen = readbuffer.ReadBoolean();
                GameRecord[i].bWinDaoMen = readbuffer.ReadBoolean();
                GameRecord[i].bWinHuang = readbuffer.ReadBoolean();
            }

            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_RecordInfo(GameRecord, nLenght);
            }
            return true;
        }
        private bool RealPlayerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {


            CMD_S_RefreshTruthPlayerInfo player = new CMD_S_RefreshTruthPlayerInfo();
            player.m_nIsEnd = readbuffer.ReadInt();
            player.m_nIsGameCheatUser = readbuffer.ReadInt();
            player.m_nPlayerNum = readbuffer.ReadInt();
            player.m_szTruthPlayerInfo = new stTruthPlayerInfo[player.m_nPlayerNum];
            for (int i = 0; i < player.m_nPlayerNum; i++)
            {
                player.m_szTruthPlayerInfo[i].dwRobotFlag = readbuffer.ReadUint();
                player.m_szTruthPlayerInfo[i].dwUserGameID = readbuffer.ReadUint();
                player.m_szTruthPlayerInfo[i].szNickName = readbuffer.ReadString(64);
                player.m_szTruthPlayerInfo[i].lScore = readbuffer.ReadLong();
                player.m_szTruthPlayerInfo[i].dwTableID = readbuffer.ReadUint();
            }

            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_RealPlayerList(player);
            }
            return true;
        }
        //续压
        private bool BetAgainMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ContinueJetton info = new CMD_S_ContinueJetton();
            info.wChairID = readbuffer.ReadUshort();
            info.szUserJetton = new stUserJetton[4];
            for (int i = 0; i < 4; i++)
            {
                info.szUserJetton[i].cbJettonArea = readbuffer.ReadByte();
                info.szUserJetton[i].lJettonScore = readbuffer.ReadLong();
            }

            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_BetAgain(info);
            }
            return true;
        }
        //续压失败
        private bool BetAgainFailMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ContinueJettonFail info = new CMD_S_ContinueJettonFail();
            info.wChairID = readbuffer.ReadUshort();
            info.szUserJetton=new stUserJetton[4];
            for (int i = 0; i < 4; i++)
            {
                info.szUserJetton[i].cbJettonArea = readbuffer.ReadByte();
                info.szUserJetton[i].lJettonScore = readbuffer.ReadLong();
            }

            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_BetAgainFail(info);
            }
            return true;
            return true;
        }

        private bool HandGameFreeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameFree GameFree = new CMD_S_GameFree();
            GameFree.cbTimeLeave = readbuffer.ReadByte();
            GameFree.nListUserCount = readbuffer.ReadInt();
            GameFree.lStorageStart = readbuffer.ReadLong();
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_Free(GameFree);
            }
            return true;
        }
        private bool HandleGameEndMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            CMD_S_GameEnd EndDate = new CMD_S_GameEnd();
            EndDate.cbTimeLeave = readbuffer.ReadByte();
            EndDate.cbTableCardArray = new byte[5][];
            for (byte i = 0; i < 5; i++)
            {
                EndDate.cbTableCardArray[i] = new byte[5];
                for (byte j = 0; j < 5; j++)
                {
                    EndDate.cbTableCardArray[i][j] = readbuffer.ReadByte();
                }
            }
            EndDate.cbLeftCardCount = readbuffer.ReadByte();					//扑克数目
            EndDate.bcFirstCard = readbuffer.ReadByte();                         //转盘动物
            EndDate.lBankerScore = readbuffer.ReadLong();						//庄家成绩
            EndDate.lBankerTotallScore = readbuffer.ReadLong();					//庄家成绩
            EndDate.nBankerTime = readbuffer.ReadInt();                         //做庄次数
            EndDate.lCurUserScore = new long[5];
            for (byte i = 0; i < 5; i++)
            {
                EndDate.lCurUserScore[i] = readbuffer.ReadLong();
            }
            EndDate.lCurUserReturnScore = new long[5];
            for (byte i = 0; i < 5; i++)
            {
                EndDate.lCurUserReturnScore[i] = readbuffer.ReadLong();
            }
            EndDate.lUserScore = readbuffer.ReadLong();						    //庄家成绩
            EndDate.lUserReturnScore = readbuffer.ReadLong();					//庄家成绩
            EndDate.lRevenue = readbuffer.ReadLong();                           //玩家最后得分 
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_End(EndDate);
            }
            return true;
        }

        private bool HandleExitMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            return true;
        }

        private bool HandBetFailMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            CMD_S_PlaceJettonFail BetFail = new CMD_S_PlaceJettonFail();

            BetFail.wChairID = readbuffer.ReadUshort();
            BetFail.lJettonArea = readbuffer.ReadByte();
            BetFail.lPlaceScore = readbuffer.ReadLong();
            BetFail.wFailReason = readbuffer.ReadUshort();
            Console.WriteLine("下注失败");
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_BetFail(BetFail);
            }
            return true;
        }

        private bool HandBetMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            CMD_S_PlaceJetton PlayBet = new CMD_S_PlaceJetton();

            PlayBet.wChairID = readbuffer.ReadUshort();
            PlayBet.cbJettonArea = readbuffer.ReadByte();
            PlayBet.lJettonScore = readbuffer.ReadLong();
            PlayBet.bIsAndroid = readbuffer.ReadBoolean();
            PlayBet.bAndroid = readbuffer.ReadBoolean();

            Console.WriteLine("下注开始");
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_UserHandBet(PlayBet);
            }
            return true;
        }
        private bool HandleStartMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameStart start = new CMD_S_GameStart();

            Console.WriteLine("游戏开始");

            start.wBankerUser = readbuffer.ReadUshort();                        //当前庄家
            start.lBankerScore = readbuffer.ReadLong(); ;                     //库庄家金币
            start.lUserMaxScore = readbuffer.ReadLong(); ;                     //我的金币
            start.cbTimeLeave = readbuffer.ReadByte();                       //剩余时间  
            start.wBankerTimesLimit = readbuffer.ReadUshort();
            start.ContiueCard = readbuffer.ReadBoolean();                   //系统做庄
            start.nChipRobotCount = readbuffer.ReadInt();                     ///人数上限                                                           

            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_StartGame(start);
            }
            return true;
        }

        //申请庄家
        private bool ApplyBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            CMD_S_ApplyBanker yBanker = new CMD_S_ApplyBanker();

            yBanker.wChairID = readbuffer.ReadUshort();

            Console.WriteLine("申请上庄");
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_ApplyBanker(yBanker);
            }
            return true;
        }


        //取消申请
        private bool CancelBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {


            CMD_S_CancelBanker Banker = new CMD_S_CancelBanker();

            Banker.wChairID = readbuffer.ReadUshort();


            Console.WriteLine("下注失败");
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_CancelBanker(Banker);
            }
            return true;
        }

        private bool BankerListMSg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_BankerList b = new CMD_S_BankerList();
            b.m_wIsEnd = readbuffer.ReadUshort();
            b.m_wApplyerNum = readbuffer.ReadUshort();
            b.m_szApplyerList = new ushort[b.m_wApplyerNum];
            for (byte i = 0; i < b.m_szApplyerList.Length; i++)
            {
                b.m_szApplyerList[i] = readbuffer.ReadUshort();
            }

            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_BankerList(b);
            }

            return true;
        }

        //切换庄家
        private bool ChangeBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {


            CMD_S_ChangeBanker ChangeBanker = new CMD_S_ChangeBanker();

            ChangeBanker.wChairID = readbuffer.ReadUshort();

            ChangeBanker.lBankerScore = readbuffer.ReadLong();

            Console.WriteLine("下注失败");
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_ChangeBanker(ChangeBanker);
            }
            return true;
        }

        private bool StorageAllInfoMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_Storage_AllInfo AllInfo = new CMD_S_Storage_AllInfo();

            AllInfo.lStorageStart = readbuffer.ReadLong();
            AllInfo.nStorageDeduct = readbuffer.ReadInt();
            AllInfo.lStorageDeductAll = readbuffer.ReadLong();

            AllInfo.tempRevenueValue = readbuffer.ReadLong();
            AllInfo.limitTempRevenueValue = readbuffer.ReadLong();
            AllInfo.tempRevenuePer = readbuffer.ReadInt();

            AllInfo.lWinRate = readbuffer.ReadLong();

            AllInfo.uiWinRate = new int[5];
            for (byte i = 0; i < 5; i++)
                AllInfo.uiWinRate[i] = readbuffer.ReadInt();

            AllInfo.RateTimeMin = readbuffer.ReadInt();
            AllInfo.RateTimeMax = readbuffer.ReadInt();
            AllInfo.RateStockMin = readbuffer.ReadInt();
            AllInfo.RateStockMax = readbuffer.ReadInt();
            AllInfo.bIsGameCheatUser = readbuffer.ReadBoolean();
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_StorageAllInfo(AllInfo);
            }
            return true;
        }
        private bool UpDateStroageMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_UpdateStorage UpdateInfo = new CMD_S_UpdateStorage();
            UpdateInfo.lStorage = readbuffer.ReadLong();
            UpdateInfo.lStorageDeduct = readbuffer.ReadLong();
            UpdateInfo.lStorageDeductAll = readbuffer.ReadLong();

            UpdateInfo.tempRevenueValue = readbuffer.ReadLong();
            UpdateInfo.limitTempRevenueValue = readbuffer.ReadLong();
            UpdateInfo.tempRevenuePer = readbuffer.ReadInt();

            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_UpdateStroageInfo(UpdateInfo);
            }
            return true;
        }
        private bool InsureSuccessMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_S_UserInsureSuccess InsureInfo = new CMD_GR_S_UserInsureSuccess();
            InsureInfo.cbActivityGame = readbuffer.ReadByte();
            InsureInfo.lUserScore = readbuffer.ReadLong();
            InsureInfo.lUserInsure = readbuffer.ReadLong();
            InsureInfo.szDescribeString = readbuffer.ReadString(readbuffer.Length - 17);
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_InsureSuccess(InsureInfo);
            }
            return true;
        }
        private bool InsureFailureMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_S_UserInsureFailure InsureInfo = new CMD_GR_S_UserInsureFailure();
            InsureInfo.cbActivityGame = readbuffer.ReadByte();
            InsureInfo.lErrorCode = readbuffer.ReadLong();
            InsureInfo.szDescribeString = readbuffer.ReadString(readbuffer.Length - 9);
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_InsureFailure(InsureInfo);
            }
            return true;
        }
        private bool HeartBeatMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Game_HeartBeat();
            }
            return true;
        }
    }
}
