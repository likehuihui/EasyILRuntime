using CYNetwork;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLWH
{
    public partial class SLWH_
    {
        #region 服务器回调命令
        //服务器命令结构

        public const ushort SUB_S_GAME_FREE				=99	;								    //游戏空闲
        public const ushort SUB_S_GAME_START			=100;									//游戏开始
        public const ushort SUB_S_PLACE_JETTON			=101;									//用户下注
        public const ushort SUB_S_GAME_END				=102;									//游戏结束
        public const ushort SUB_S_APPLY_BANKER			=103;									//申请庄家
        public const ushort SUB_S_CHANGE_BANKER			=104;									//切换庄家
        public const ushort SUB_S_CHANGE_USER_SCORE		=105;									//更新积分
        public const ushort SUB_S_SEND_RECORD			=106;									//游戏记录
        public const ushort SUB_S_PLACE_JETTON_FAIL		=107;									//下注失败
        public const ushort SUB_S_CANCEL_BANKER			=108;									//取消申请
        public const ushort SUB_S_CLEAR_JETTON		    =109;									//下注失败
        public const ushort SUB_S_SEND_INFO				=110;
        public const ushort SUB_S_ACK_UPDATE_STORAGE	=111;
        public const ushort SUB_S_ACK_SET_PRIZE			=112;
        public const ushort SUB_S_TOTAL_JETTON          =113;									//总注
        public const ushort SUB_S_UPDATE_WEAGER_ARR     =114;								    //更新下注级别
        public const ushort SUB_S_GET_ADMIN = 115;								    //是不是管理员
        public const ushort SUB_S_GET_STORAGE = 116;                              //获得库存
        public const ushort SUB_S_CONTROL_GET_PRIZE = 117;							//管理员获取开奖结果
        public const ushort SUB_S_CONTROL_DEL_PRIZE = 118;							//管理员取消开奖结果
        public const ushort SUB_S_CONTROL_GET_PROB = 119;							//管理员获取概率列表
        public const ushort SUB_S_CONTROL_SET_PROB = 120;							//管理员设置概率列表成功

        public const ushort SUB_S_REAL_USER_BET = 121;							//管理员知道真实玩家下注情况
        public const ushort SUB_S_CONTROL_BINGO_PROB = 122;							//管理员设置大奖概率
        #endregion

        #region 服务器返回的结构体
        // 游戏类型
        public enum eGambleType
        {
	        eGambleType_Invalid = -1,
	        eGambleType_AnimalGame = 0,	//3d动物
	        eGambleType_EnjoyGame,		//小游戏，庄闲和
	        eGambleType_Max,
        };
        //动物类型定义
        public enum eAnimalType
        {
	        eAnimalType_Invalid	= -1,
	        eAnimalType_Lion	= 0,
	        eAnimalType_Panda,
	        eAnimalType_Monkey,
	        eAnimalType_Rabbit,

	        eAnimalType_Max, //最大值
        };

        //颜色类型定义
        public enum eColorType
        {
	         eColorType_Invalid		= -1,
	         eColorType_Red		= 0,
	         eColorType_Green,
	         eColorType_Yellow,
	         eColorType_Max, //最大值
        };

        //动物模式的开奖模式
        public enum eAnimalPrizeMode
        {
	        eAnimalPrizeMode_Invalid = -1,
	        eAnimalPrizeMode_SingleAnimalSingleColor = 0,//普通开奖
	        eAnimalPrizeMode_AllAnimalSingleColr,   //大四喜
	        eAnimalPrizeMode_AllColorSingleAnimal,//大小三元
	        eAnimalPrizeMode_SysPrize, //随机点灯
	        eAnimalPrizeMode_RepeatTime,//四海归一
	        eAnimalPrizeMode_Flash,//霹雳闪电

	        eAnimalPrizeMode_Max,
        };

        //庄闲和游戏定义
        public enum eEnjoyGameType
        {
	        eEnjoyGameType_Invalid = -1,
	        eEnjoyGameType_Zhuang = 0,
	        eEnjoyGameType_Xian,
	        eEnjoyGameType_He,

	        eEnjoyGameType_Max, //最大值
        };

        //开奖类型
        public enum RewardType
        {
            RewardNull = -1,
            Everywhere,     //纵横四海
            DaSiXi,         //大四喜
            DaSanYuan,      //大三元
            TwoDragons,     //小三元
            RandomLighting1,//随机点灯1  二个动物
            RandomLighting2,//随机点灯2  三个动物
            RandomLighting3,//随机点灯3  四个动物
            RedLion,        //红色狮子
            GreenLion,      //绿色狮子
            YellowLion,     //黄色狮子
            RedPanda,       //红色熊猫
            GreenPanda,     //绿色熊猫
            YellowPanda,    //黄色熊猫
            RedMonkey,      //红色猴子
            GreenMonkey,    //绿色猴子
            YellowMonkey,   //黄色猴子
            RedRabbit,      //红色兔子
            GreenRabbit,    //绿色兔子
            YellowRabbit,   //黄色兔子
            MaxAnimal       //最大动物
        };


        //庄家信息
        public  struct CMD_BANKER_INFO
        {
                public string szBankerName;					//庄玩家32 64
                public string szBankerAccounts;               //32 64
                public uint dwUserID;							//用户ID
                public long iBankerScore;						//庄金币
                public uint wBankCount;							//做庄次数
        };


        //动物类型
        public struct tagSTAnimalInfo
        {
            public eAnimalType eAnimal;//动物
            public eColorType eColor;//颜色

        };
        //动物属性
        public struct tagSTAnimalAtt
        {
	        public tagSTAnimalInfo stAnimal;//动物类型
	        public uint  dwMul;//动物开奖倍率
	        public ulong  qwJettonLimit;//动物下注最高限制
        };

        // 庄闲和属性
         public struct tagSTEnjoyGameInfo
        {
	        public eEnjoyGameType eEnjoyGame;//庄闲和类型
            public uint dwMul;//倍率
	        //UINT64  qwJetton;
            public ulong qwJettonLimit;//下注最高限制
        };

        //开奖信息
        public struct tagSTPrizeInfo
        {
            public eGambleType eGamble;//开奖类型
            public tagSTAnimalInfo stWinAnimal;//开奖动物
            public tagSTEnjoyGameInfo stEnjoyGame;//开奖庄闲和
        };

        //动物开奖信息
        public struct  tagSTAnimalPrize
        {
	        public tagSTAnimalInfo stAnimalInfo;
	        public eAnimalPrizeMode ePrizeMode;

	        /*
	        当prizemode=eAnimalPrizeMode_SysPrize时，qwFlag表示开出来的系统彩金，
	        当prizemode=eAnimalPrizeMode_RepeatTime时，qwFlag表示重复次数
	        当prizemode=eAnimalPrizeMode_Flash时，qwFlag表示系统倍率
	        */
	        public ulong qwFlag;

	        //在repeat下，另外再开的动物列表,最高20个
            public tagSTAnimalInfo[] arrstRepeatModePrize; //[20]
            public int [][] PrizeIndex;                      //[2][12] 0颜色 1动物

        };

        //庄闲和开奖信息
        public struct tagSTEnjoyGamePrizeInfo
        {
	        public eEnjoyGameType ePrizeGameType;
            public ulong qwFlag;
        };

        /// <summary>
        /// 游戏开始
        /// </summary>
         public struct CMD_S_GameStart
         {
             public long iUserScore;						//我的金币

             //全局信息
             public byte cbTimeLeave;						//剩余时间

             // 下注限制信息
             public tagSTAnimalAtt[][] arrSTAnimalJettonLimit; //动物属性eAnimalType_Max/eColorType_Max
             public uint[] arrColorRate;//颜色分布概率eColorType_Max
             public tagSTEnjoyGameInfo[] arrSTEnjoyGameJettonLimit;//庄闲和属性eEnjoyGameType_Max

             public CMD_BANKER_INFO stBankerInfo;						//庄家信息

             public byte cbBankerFlag;					      //庄家表示 0： 非庄家，1： 庄家
             public int[] stColor;                                     //转盘颜色分布[24]
         };

        //游戏空闲
         public struct CMD_S_GameFree
         {
             public long iUserScore;						//我的金币
              public byte cbTimeLeave;						//剩余时间
             //BYTE							cbGameRecord;						//本次开出的结果
             //STAnimalPrize stAnimalPrize;
             //STEnjoyGamePrizeInfo stEnjoyGamePrizeInfo;
              public ulong qwGameTimes;						   //当前是游戏启动以来的第几局

             public  CMD_BANKER_INFO stBankerInfo;						//庄家信息
             public byte   cbCanCancelBank;					//是否可以申请下庄（0： 不能下庄，1：能下庄）

         };

         //用户下注
         public struct CMD_S_PlaceJetton
         {
             public bool isAndroid; // 是不是机器人
             public ushort wChairID;							//用户位置
             public eGambleType eGamble;
             public tagSTAnimalInfo stAnimalInfo;
             public eEnjoyGameType eEnjoyGameInfo;
             public ulong iPlaceJettonScore;				    //当前下注
             public byte cbBanker;							//是否是庄家，0： 非庄家，1：庄家
             public ulong iTotalPlayerJetton;				    //庄家时候，显示其他玩家下注总和
         };

        //用户总注
        public struct CMD_S_TotalJetton
        {
            public ulong[][] m_arriTotalAnimalJettonScore;					 //所有动物下注总额 eAnimalType_Max eColorType_Max
	        public ulong [] m_arriTotalEnjoyGameJettonScore;							//所有庄闲和下注总额	eEnjoyGameType_Max 
            public byte mCode; // 0 更新总下注情况  1 清空我的下注清空
            public bool isAndroid; //是不是机器人
        };

        //申请庄家
        public struct CMD_S_ApplyBanker
        {
            public ushort wApplyUser;						//申请玩家
        };

        //取消申请
        public struct CMD_S_CancelBanker
        {
            public string szCancelUser;					//取消玩家 [32] 64
            public ushort dwUserID;				        //取消玩家ID
        };

        //切换庄家
        public struct CMD_S_ChangeBanker
        {
            public ushort wBankerUser;						//当庄玩家
            public long iBankerScore;						//庄家金币
        };

        //失败结构  加注失败 与 CMD_S_PlaceJetton一起用
        //dwErrorCode:说明：
        //1：积分不够
        //2: 达到个体下注上线
        //3: 不在下注时间
        public struct CMD_S_PlaceJettonFail
        {
            public eGambleType eGamble;//类型
            public tagSTAnimalInfo stAnimalInfo;//动物加注信息
            public eEnjoyGameType eEnjoyGameInfo;//庄闲和加注信息
            public ulong iPlaceJettonScore;				    //当前下注
            public uint dwErrorCode;
            //BYTE							lJettonArea;						//下注区域
            //__int64							iPlaceScore;				    //当前下注
        };


        //游戏结束
        public struct CMD_S_GameEnd
        {
            //下一局信息
            public uint dwTimeLeave;						//剩余时间
            //UINT32							dwEnjoyGameTimeLeave;			//剩余时间
            public tagSTAnimalPrize stWinAnimal;						//开奖动物
            public tagSTEnjoyGamePrizeInfo stWinEnjoyGameType;//开奖庄闲和
            //玩家成绩
            public long iUserScore;							  //玩家成绩
            //全局信息
            public long iRevenue;							  //游戏税收

            public CMD_BANKER_INFO stBankerInfo;			  //庄家信息
            public long iBankerScore;						  //庄本次得分

        };


        //记录信息
        public struct tagServerGameRecord
        {
            public long cbGameTimes;										//第几局
            //UINT		cbRecord;											//范围在ID_BIG_TIGER到ID_SMALL_SNAKE
            public tagSTAnimalPrize stAnimalPrize;
            public tagSTEnjoyGamePrizeInfo stEnjoyGamePrizeInfo;
        };

        public struct CMD_S_Control
        {
	        public long lStorageScore;
	        public uint	 nStorageDeduct;
	        public ulong [][]animalJettonScore;//[eAnimalType_Max][eColorType_Max]
	        public ulong []enjoyGameJettonScore;//[eEnjoyGameType_Max]
	        public string []userNickName;//[100][64]
        };

        public struct CMD_S_ACK_Control
        {
            public string msg;//[1024] 2048
        };

        public struct CMD_S_StorageInfo
        {
            public long lStorageScore;   //库存
            public long lDecuteScore;   //抽水值
            public uint nStorageDecute;  //抽水比例
            public long tempRevenueValue;       //库存抽水
            public long tempRevenueLimitValue;  //库存抽水阈值
            public double tempRevenuePer;       //库存抽水比例
        };

        public struct CMD_S_ClearJetton
        {
	        // 0: 成功，1：失败
	        uint			dwErrorCode;
        };

        public struct tagWeagerArr
        {
            public int ArrLen; //下注级别数组的长度
            public ulong[] weagerArr; //下注级别
        };


        public struct CMD_S_GetAdmin
        {
            public bool IsAdmin;
        };

        public struct CMD_S_PrizeInfo
        {
            public bool bOK; //是否设置成功
            public int times;  //控制局数 默认为1
            public RewardType eAnimalIndexes;    //开奖索引 	eAnimalType_Invalid     19 霹雳闪电
            public eAnimalType []eAnimal; //动物  12
            public eColorType []eColor;  //颜色   12
            public eEnjoyGameType eEnjoyGame;  //庄闲和
        };

        public struct CMD_S_PorbInfo
        {
            public int[] EatProbArr; //吃分概率表 10
            public int[] SendProbArr; //送分概率表 10
            public int[] ComeProbArr; //出现概率表 10
        };


        /*public struct RealUserBet
        {
	        public long RealUserAnimalJettonScore[eAnimalType_Max][eColorType_Max];                     //真实玩家各动物下注情况
	        public long RealUserIdleJettonScore[eEnjoyGameType_Max];                                    //真实玩家庄闲和下注情况
        };*/

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
                case SUB_S_ACK_UPDATE_STORAGE:
                    return HandleUpdateStorage(maincmd,subCmd,readbuffer);
                case SUB_S_ACK_SET_PRIZE:
                    return HandleSetPrize(maincmd, subCmd, readbuffer);
                case SUB_S_TOTAL_JETTON:
                    return HandleTOTAL_JETTON(maincmd, subCmd, readbuffer);
                case SUB_S_UPDATE_WEAGER_ARR:
                    return HandleUPDATE_WEAGER_ARR(maincmd, subCmd, readbuffer);
                case SUB_S_GET_ADMIN:
                    return HandleGET_ADMIN(maincmd, subCmd, readbuffer);
                case SUB_S_GET_STORAGE:
                    return HandleGET_STORAGE(maincmd, subCmd, readbuffer);
                case SUB_S_CONTROL_GET_PRIZE:
                    return HandleGET_PRIZE(maincmd, subCmd, readbuffer);
                case SUB_S_CONTROL_DEL_PRIZE:
                    return HandleDEL_PRIZE(maincmd, subCmd, readbuffer);
                case SUB_S_CONTROL_GET_PROB:
                    return HandleGET_PROB(maincmd, subCmd, readbuffer);
                case SUB_S_CONTROL_SET_PROB:
                    return HandleSetProb(maincmd, subCmd, readbuffer);
                case SUB_S_REAL_USER_BET:							
                    return HandleREALPlaceJettonMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CONTROL_BINGO_PROB:							
                    return HandleBinGoProbMsg(maincmd, subCmd, readbuffer);
                    
                default:
                    Error("森林舞会 未能解析的游戏子命令：" + subCmd);
                    break;
            }
            return true;
        }

//         private bool CheckGameData()
//         {
//             if (SLWHInterFace != null)
//             {
//                 SLWHInterFace.CheckGameNet();
//             }
//             return true;
//         }


        private bool HandleFreeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameFree Free = new CMD_S_GameFree();
            Free.iUserScore=readbuffer.ReadLong();
            Free.cbTimeLeave=readbuffer.ReadByte();
            Free.qwGameTimes=readbuffer.ReadUlong();

            //Free.stBankerInfo = readbuffer.ReadT<CMD_BANKER_INFO>();
            Free.stBankerInfo.szBankerName=readbuffer.ReadString(64);
            Free.stBankerInfo.szBankerAccounts = readbuffer.ReadString(64);
            Free.stBankerInfo.dwUserID = readbuffer.ReadUint();
            Free.stBankerInfo.iBankerScore = readbuffer.ReadLong();
            Free.stBankerInfo.wBankCount = readbuffer.ReadUint();

            Free.cbCanCancelBank = readbuffer.ReadByte();

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_FreeGame(Free);
            }
            return true;
        }

        private bool HandleStartMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameStart Start = new CMD_S_GameStart();
            Start.iUserScore = readbuffer.ReadLong();
            Start.cbTimeLeave = readbuffer.ReadByte();
            int iAnimalType_Max = (int)eAnimalType.eAnimalType_Max;
            int iColorType_Max = (int)eColorType.eColorType_Max;
            int iEnjoyGameType = (int)eEnjoyGameType.eEnjoyGameType_Max;
            Start.arrSTAnimalJettonLimit = new tagSTAnimalAtt[iAnimalType_Max][];
            for (byte i = 0; i < iAnimalType_Max; i++)
            {
                Start.arrSTAnimalJettonLimit[i] = new tagSTAnimalAtt[iColorType_Max];
                for (byte j = 0; j < iColorType_Max; j++)
                {
                    Start.arrSTAnimalJettonLimit[i][j].stAnimal.eAnimal = (eAnimalType)readbuffer.ReadInt();
                    Start.arrSTAnimalJettonLimit[i][j].stAnimal.eColor = (eColorType)readbuffer.ReadInt();
                    Start.arrSTAnimalJettonLimit[i][j].dwMul = readbuffer.ReadUint();
                    Start.arrSTAnimalJettonLimit[i][j].qwJettonLimit = readbuffer.ReadUlong();
                }
            }
            Start.arrColorRate = new uint[iColorType_Max];
            for (byte i = 0; i < iColorType_Max; i++)
            {
                Start.arrColorRate[i] = readbuffer.ReadUint();
            }
            Start.arrSTEnjoyGameJettonLimit = new tagSTEnjoyGameInfo[iEnjoyGameType];
            for (byte i = 0; i < iEnjoyGameType;i++ )
            {
                Start.arrSTEnjoyGameJettonLimit[i].eEnjoyGame = (eEnjoyGameType)readbuffer.ReadInt();
                Start.arrSTEnjoyGameJettonLimit[i].dwMul = readbuffer.ReadUint();
                Start.arrSTEnjoyGameJettonLimit[i].qwJettonLimit = readbuffer.ReadUlong();
            }
            //Start.stBankerInfo = readbuffer.ReadT<CMD_BANKER_INFO>();
            Start.stBankerInfo = new CMD_BANKER_INFO();
            Start.stBankerInfo.szBankerName = readbuffer.ReadString(64);
            Start.stBankerInfo.szBankerAccounts = readbuffer.ReadString(64);
            Start.stBankerInfo.dwUserID = readbuffer.ReadUint();
            Start.stBankerInfo.iBankerScore = readbuffer.ReadLong();
            Start.stBankerInfo.wBankCount = readbuffer.ReadUint();


            Start.cbBankerFlag = readbuffer.ReadByte();

            Start.stColor = new int[24];
            for (int i = 0; i < Start.stColor.Length; i++)
            {
                Start.stColor[i]=readbuffer.ReadInt();
            }

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_StartGame(Start);
            }
            return true;
        }

        private bool HandlePlaceJettonMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlaceJetton Jetton = new CMD_S_PlaceJetton();
            Jetton.isAndroid = readbuffer.ReadBoolean();
            Jetton.wChairID = readbuffer.ReadUshort();
            Jetton.eGamble = (eGambleType)readbuffer.ReadInt();

            Jetton.stAnimalInfo.eAnimal = (eAnimalType)readbuffer.ReadInt();
            Jetton.stAnimalInfo.eColor = (eColorType)readbuffer.ReadInt();

            Jetton.eEnjoyGameInfo = (eEnjoyGameType)readbuffer.ReadInt();
            Jetton.iPlaceJettonScore = readbuffer.ReadUlong();
            Jetton.cbBanker = readbuffer.ReadByte();
            Jetton.iTotalPlayerJetton = readbuffer.ReadUlong();

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_PlaceJetton(Jetton);
            }
            return true;
        }

        private bool HandleREALPlaceJettonMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlaceJetton Jetton = new CMD_S_PlaceJetton();
            Jetton.isAndroid = readbuffer.ReadBoolean();
            Jetton.wChairID = readbuffer.ReadUshort();
            Jetton.eGamble = (eGambleType)readbuffer.ReadInt();

            Jetton.stAnimalInfo.eAnimal = (eAnimalType)readbuffer.ReadInt();
            Jetton.stAnimalInfo.eColor = (eColorType)readbuffer.ReadInt();

            Jetton.eEnjoyGameInfo = (eEnjoyGameType)readbuffer.ReadInt();
            Jetton.iPlaceJettonScore = readbuffer.ReadUlong();
            Jetton.cbBanker = readbuffer.ReadByte();
            Jetton.iTotalPlayerJetton = readbuffer.ReadUlong();

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_REAL_PlaceJetton(Jetton);
            }
            return true;
        }
        

        private bool HandleGameEndMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameEnd End = new CMD_S_GameEnd();
            End.dwTimeLeave = readbuffer.ReadUint();

            End.stWinAnimal.stAnimalInfo.eAnimal = (eAnimalType)readbuffer.ReadInt();
            End.stWinAnimal.stAnimalInfo.eColor = (eColorType)readbuffer.ReadInt();
            End.stWinAnimal.ePrizeMode = (eAnimalPrizeMode)readbuffer.ReadInt();
            End.stWinAnimal.qwFlag = readbuffer.ReadUlong();
            End.stWinAnimal.arrstRepeatModePrize = new tagSTAnimalInfo[20];
            for (int i = 0; i < 20; i++)
            {
                End.stWinAnimal.arrstRepeatModePrize[i].eAnimal = (eAnimalType)readbuffer.ReadInt();
                End.stWinAnimal.arrstRepeatModePrize[i].eColor = (eColorType)readbuffer.ReadInt();
            }

            End.stWinAnimal.PrizeIndex = new int[2][];
            for (int i = 0; i < 2; i++)
            {
                End.stWinAnimal.PrizeIndex[i] = new int[12];
                for (int j = 0; j < 12; j++)
                {
                    End.stWinAnimal.PrizeIndex[i][j] = readbuffer.ReadInt();
                }
            }

            End.stWinEnjoyGameType.ePrizeGameType = (eEnjoyGameType)readbuffer.ReadInt();
            End.stWinEnjoyGameType.qwFlag = readbuffer.ReadUlong();

            End.iUserScore = readbuffer.ReadLong();
            End.iRevenue = readbuffer.ReadLong();
            //End.stBankerInfo = readbuffer.ReadT<CMD_BANKER_INFO>();
            End.stBankerInfo = new CMD_BANKER_INFO();
            End.stBankerInfo.szBankerName = readbuffer.ReadString(64);
            End.stBankerInfo.szBankerAccounts = readbuffer.ReadString(64);
            End.stBankerInfo.dwUserID = readbuffer.ReadUint();
            End.stBankerInfo.iBankerScore = readbuffer.ReadLong();
            End.stBankerInfo.wBankCount = readbuffer.ReadUint();
            End.iBankerScore = readbuffer.ReadLong();

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_End(End);
            }
            return true;
        }

        private bool HandleApplyBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ApplyBanker OpUser = readbuffer.ReadT<CMD_S_ApplyBanker>();
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_ApplyBanker(OpUser);
            }
            return true;
        }
        private bool HandleCancelBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_CancelBanker OpUser = new CMD_S_CancelBanker();
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_CancelBanker(OpUser);
            }
            return true;
        }

        private bool HandleChangeBankerMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ChangeBanker Change = readbuffer.ReadT<CMD_S_ChangeBanker>();

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_ChangeBanker(Change);
            }
            return true;
        }

        private bool HandleChangeUserScoreMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            if (SLWHInterFace != null)
            {
                //SLWHInterFace.Game_StartGame(Start);
            }
            return true;
        }
        private bool HandleSendRecordMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            int RecordCount = readbuffer.Length / 296;//192;//System.Runtime.InteropServices.Marshal.SizeOf(new tagServerGameRecord());//96; //结构体大小
            tagServerGameRecord[] Record = new tagServerGameRecord[RecordCount];
            for (int i = 0; i < RecordCount;i++ )
            {
                Record[i].cbGameTimes = readbuffer.ReadLong();

                Record[i].stAnimalPrize.stAnimalInfo.eAnimal = (eAnimalType)readbuffer.ReadInt();
                Record[i].stAnimalPrize.stAnimalInfo.eColor = (eColorType)readbuffer.ReadInt();
                Record[i].stAnimalPrize.ePrizeMode = (eAnimalPrizeMode)readbuffer.ReadInt();
                Record[i].stAnimalPrize.qwFlag = readbuffer.ReadUlong();
                Record[i].stAnimalPrize.arrstRepeatModePrize = new tagSTAnimalInfo[20];
                for (int j = 0; j < 20; j++) 
                {
                    Record[i].stAnimalPrize.arrstRepeatModePrize[j].eAnimal = (eAnimalType)readbuffer.ReadInt();
                    Record[i].stAnimalPrize.arrstRepeatModePrize[j].eColor = (eColorType)readbuffer.ReadInt();
                }

                Record[i].stAnimalPrize.PrizeIndex = new int[2][];
                for (int k = 0; k < 2; k++)
                {
                    Record[i].stAnimalPrize.PrizeIndex[k] = new int[12];
                    for (int j = 0; j < 12; j++)
                    {
                        Record[i].stAnimalPrize.PrizeIndex[k][j] = readbuffer.ReadInt();
                    }
                }

                Record[i].stEnjoyGamePrizeInfo.ePrizeGameType = (eEnjoyGameType)readbuffer.ReadInt();
                Record[i].stEnjoyGamePrizeInfo.qwFlag = readbuffer.ReadUlong();
            }
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_Record(Record);
            }
            return true;
        }
        private bool HandlePlaceJettonFailMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlaceJettonFail Fail = new CMD_S_PlaceJettonFail();
            Fail.eGamble=(eGambleType)readbuffer.ReadInt();

            Fail.stAnimalInfo.eAnimal=(eAnimalType)readbuffer.ReadInt();
            Fail.stAnimalInfo.eColor=(eColorType)readbuffer.ReadInt();

            Fail.eEnjoyGameInfo=(eEnjoyGameType)readbuffer.ReadInt();
            Fail.iPlaceJettonScore = readbuffer.ReadUlong();
            Fail.dwErrorCode = readbuffer.ReadUint();

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_PlaceJettonFail(Fail);
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
            CMD_S_ACK_Control control = new CMD_S_ACK_Control();
            control.msg = readbuffer.ReadString(2048);
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_ACK_ADMIN(control.msg);
            }
            return true;
        }

        private bool HandleSetProb(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ACK_Control control = new CMD_S_ACK_Control();
            control.msg = "概率表设置成功了!";
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_ACK_ADMIN(control.msg);
            }
            return true;
        }

        private bool HandleSetPrize(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ACK_Control control = new CMD_S_ACK_Control();
            control.msg = "特殊控制设置成功了!";
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_ACK_ADMIN(control.msg);
            }
            return true;
        }

        private bool HandleTOTAL_JETTON(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_TotalJetton tjetton = new CMD_S_TotalJetton();
            tjetton.m_arriTotalAnimalJettonScore = new ulong[(int)eAnimalType.eAnimalType_Max][];
            for (int i = 0; i < tjetton.m_arriTotalAnimalJettonScore.Length; i++)
            {
                tjetton.m_arriTotalAnimalJettonScore[i] = new ulong[(int)eColorType.eColorType_Max];
                for (int j = 0; j < tjetton.m_arriTotalAnimalJettonScore[i].Length; j++)
                {
                    tjetton.m_arriTotalAnimalJettonScore[i][j] = readbuffer.ReadUlong();
                }
            }
            tjetton.m_arriTotalEnjoyGameJettonScore = new ulong[(int)eEnjoyGameType.eEnjoyGameType_Max];
            for (int i = 0; i < tjetton.m_arriTotalEnjoyGameJettonScore.Length; i++)
            {
                tjetton.m_arriTotalEnjoyGameJettonScore[i] = readbuffer.ReadUlong();
            }
            tjetton.mCode = readbuffer.ReadByte();
            tjetton.isAndroid = readbuffer.ReadBoolean();
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_TOTAL_JETTON_INFO(tjetton);
            }
            return true;
        }


        private bool HandleUPDATE_WEAGER_ARR(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer) 
        {
            tagWeagerArr wArr = new tagWeagerArr();
            wArr.ArrLen = readbuffer.ReadInt();
            wArr.weagerArr = new ulong[wArr.ArrLen];
            for (int i = 0; i < wArr.weagerArr.Length; i++)
            {
                wArr.weagerArr[i] = readbuffer.ReadUlong();
            }
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_UPDATE_WEAGER_ARR(wArr);
            }
            return true;
        }

        private bool HandleGET_ADMIN(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer) 
        {
            CMD_S_GetAdmin ad = new CMD_S_GetAdmin();
            ad.IsAdmin = readbuffer.ReadBoolean();
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_GET_ADMIN(ad);
            }
            return true;
        }

        private bool HandleGET_STORAGE(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer) 
        {
            CMD_S_StorageInfo stinfo = new CMD_S_StorageInfo();
            stinfo.lStorageScore = readbuffer.ReadLong();
            stinfo.lDecuteScore = readbuffer.ReadLong();
            stinfo.nStorageDecute = readbuffer.ReadUint();

            stinfo.tempRevenueValue = readbuffer.ReadLong();
            stinfo.tempRevenueLimitValue = readbuffer.ReadLong();
            stinfo.tempRevenuePer = readbuffer.ReadDouble();

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_GET_STORAGE(stinfo);
            }
            return true;
        }

        private bool HandleGET_PROB(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer) 
        {
            CMD_S_PorbInfo pinfo = new CMD_S_PorbInfo();
            pinfo.EatProbArr = new int[10];
            pinfo.SendProbArr = new int[10];
            pinfo.ComeProbArr = new int[10];

            for (int i = 0; i < pinfo.EatProbArr.Length; i++)
            {
                pinfo.EatProbArr[i] = readbuffer.ReadInt();
            }
            for (int i = 0; i < pinfo.SendProbArr.Length; i++)
            {
                pinfo.SendProbArr[i] = readbuffer.ReadInt();
            }
            for (int i = 0; i < pinfo.ComeProbArr.Length; i++)
            {
                pinfo.ComeProbArr[i] = readbuffer.ReadInt();
            }

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_GET_PROB(pinfo);
            }
            return true;
        }
        

        private bool HandleDEL_PRIZE(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer) 
        {
            CMD_S_PrizeInfo pinfo = new CMD_S_PrizeInfo();
            pinfo.bOK = readbuffer.ReadBoolean();
            pinfo.times = readbuffer.ReadInt();
            pinfo.eAnimalIndexes = (RewardType)readbuffer.ReadInt();
            pinfo.eAnimal = new eAnimalType[12];
            for (int i = 0; i < pinfo.eAnimal.Length; i++)
            {
                pinfo.eAnimal[i] = (eAnimalType)readbuffer.ReadInt();
            }
            pinfo.eColor = new eColorType[12];
            for (int i = 0; i < pinfo.eColor.Length; i++)
            {
                pinfo.eColor[i] = (eColorType)readbuffer.ReadInt();
            }
            pinfo.eEnjoyGame = (eEnjoyGameType)readbuffer.ReadInt();

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_DEL_PRIZE(pinfo);
            }
            return true;
        }


        private bool HandleGET_PRIZE(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PrizeInfo pinfo = new CMD_S_PrizeInfo();
            pinfo.bOK = readbuffer.ReadBoolean();
            pinfo.times = readbuffer.ReadInt();
            pinfo.eAnimalIndexes = (RewardType)readbuffer.ReadInt();
            pinfo.eAnimal = new eAnimalType[12];
            for (int i = 0; i < pinfo.eAnimal.Length; i++)
            {
                pinfo.eAnimal[i] = (eAnimalType)readbuffer.ReadInt();
            }
            pinfo.eColor = new eColorType[12];
            for (int i = 0; i < pinfo.eColor.Length; i++)
            {
                pinfo.eColor[i] = (eColorType)readbuffer.ReadInt();
            }
            pinfo.eEnjoyGame = (eEnjoyGameType)readbuffer.ReadInt();

            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_GET_PRIZE(pinfo);
            }
            return true;
        }

        private bool HandleBinGoProbMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer) 
        {
            CMD_C_BingoProb bprob = new CMD_C_BingoProb();
            bprob.isGet = readbuffer.ReadBoolean();
            bprob.BingoProb = readbuffer.ReadInt();
            if (SLWHInterFace != null)
            {
                SLWHInterFace.Game_BINGO_PROB(bprob);
            }
            return true;
        }
    }
}
