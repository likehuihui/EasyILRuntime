using CYNetwork;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonLegend
{
    public partial class DragonLegend_
    {
        /// <summary>
        /// ////////////////////////////////////////////code by kingsley  6/27/2016 ///////////////////////
        /// </summary>
        #region 服务器回调命令
        //服务器命令结构
        public const ushort SUB_S_GAME_FREE = 101;                             //游戏空闲
        public const ushort SUB_S_GAME_START = 102;                             //游戏开始
        public const ushort SUB_S_GAME_END = 103;                              //游戏结束
        public const ushort SUB_S_PLAY_BET = 104;                                //用户下注
        public const ushort SUB_S_PLAY_BET_FAIL = 105;                              //用户下注失败
        public const ushort SUB_S_BET_CLEAR = 106;                               //清除下注
        public const ushort SUB_S_BET_QUIT_GAME = 107;                               //退出游戏    
        public const ushort SUB_S_AMDIN_COMMAND = 109;									//系统控制
        public const ushort SUB_S_GM_IsMaster = 110;               // 是不是管理员
        #endregion
        #region 服务器返回的结构体

       // public const byte ANIMAL_MAX = 13;					//最大数量 

        /// <summary>
        /// 游戏开始
        /// </summary>
        public struct CMD_S_GameStart
        {
            public byte cbTimeLeave;                       //剩余时间         
            public byte[] cbAnimalMultiple;                   //动物倍数 
            public byte cbLuDanNum;
            public byte[]  cbLuDan;                         //路单  
            public long lStorageStart;                     //库存（彩池）
        };

        /// <summary>
        /// 游戏结束
        /// </summary>
        public struct CMD_S_GameEnd
        {
            public byte cbTimeLeave;                       //剩余时间
            public byte cbTurnTimes;                        //转盘次数

            public byte iAnimalIndex;                       //动物索引
            public byte nAnimalID;                          //转盘动物

            public byte nRandomAnimal;                      //随机动物
            public byte nRandomAnimalIndex;                 //随机动物

            public long[] lPlaybetScore;                    //2
            public long lPlayWinScore;                     //玩家最后得分
                                                            

            public long lPlayShowPrizes;                   //显示彩金

            public byte[] nWinLight;                         //3

        };

        /// <summary>
        /// 用户下注
        /// </summary>
        public struct CMD_S_PlayBet
        {
            public ushort wChairID;                          //玩家位置
            public byte cbAnimalIndex;                       //下注动物
            public long lBetChip;                          //下注数量
            public long[] lPlayerBet;                    //当前玩家总下注
            public long[] lAllBet;                         //所有下注
            public long[] lPlayBet;				        //玩家自己下注
            public bool bAndroid;
        };


        /// <summary>
        /// 下注失败
        /// </summary>
        public struct CMD_S_PlayBetFail
        {

            public ushort wChairID;                          //玩家位置
            public byte cbAnimalIndex;                       //下注动物
            public long lBetChip;							//筹码数量
        };


        /// <summary>
        /// 清除下注
        /// </summary>
        public struct CMD_S_BetClear
        {
            public ushort wChairID;                          //玩家位置
            public long[] lAllPlayBet;				            //玩家清除数量          
        };


        /// <summary>
        /// 空闲消息
        /// </summary>
        public struct CMD_S_GameFree
        {
            public byte cbTimeLeave;            //剩余时间  
        };

        public enum AckType
        {
            ACK_SET_WIN_AREA=1,
            ACK_RESET_CONTROL,
            ACK_PRINT_SYN,
            ACK_PRINT_ALL,
            ACK_SET_FINISH		
        }


        public struct CMD_S_CommandResult
        {
	        public byte  cbAckType;					//回复类型
          /*#define ACK_SET_WIN_AREA 	 1
            #define ACK_RESET_CONTROL	 2
            #define ACK_PRINT_SYN    	 3
            #define ACK_PRINT_ALL		 4
            #define ACK_SET_FINISH		 5*/
	        public byte  cbResult;
            /*#define CR_ACCEPT  2			//接受
            #define CR_REFUSAL 3			//拒绝*/
            public byte[] cbExtendData;		//附加数据//128
        };

        struct CMD_S_GM_Master
        {
            public bool bIsMaster;
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
                case SUB_S_PLAY_BET:
                    return HandBetMsg(maincmd, subCmd, readbuffer);
                case SUB_S_PLAY_BET_FAIL:
                    return HandBetFailMsg(maincmd, subCmd, readbuffer);
                case SUB_S_BET_CLEAR:
                    return HandBetClearMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GAME_END:
                    return HandleGameEndMsg(maincmd, subCmd, readbuffer);
                case SUB_S_AMDIN_COMMAND:
                    return HandleAMDIN_COMMANDMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GM_IsMaster:
                    return HandleIsMasterMsg(maincmd, subCmd, readbuffer);
                default:
                    Error("二人牛牛 未能解析的子命令：" + subCmd);
                    break;
            }
            return true;
        }

        private bool HandGameFreeMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameFree GameFree = new CMD_S_GameFree();
            GameFree.cbTimeLeave = readbuffer.ReadByte();

            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.Game_Free(GameFree);
            }
            return true;
        }
        private bool HandleGameEndMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {

            CMD_S_GameEnd EndDate = new CMD_S_GameEnd();

            EndDate.cbTimeLeave = readbuffer.ReadByte();
            EndDate.cbTurnTimes = readbuffer.ReadByte();
            EndDate.iAnimalIndex = readbuffer.ReadByte();
            EndDate.nAnimalID = readbuffer.ReadByte();
            EndDate.nRandomAnimal = readbuffer.ReadByte();
            EndDate.nRandomAnimalIndex = readbuffer.ReadByte();

            EndDate.lPlaybetScore = new long[2];

            EndDate.lPlaybetScore[0] = readbuffer.ReadLong();
            EndDate.lPlaybetScore[1] = readbuffer.ReadLong();

            EndDate.lPlayWinScore = readbuffer.ReadLong();
            EndDate.lPlayShowPrizes = readbuffer.ReadLong();

            EndDate.nWinLight = new byte[3];

            EndDate.nWinLight[0] = readbuffer.ReadByte();
            EndDate.nWinLight[1] = readbuffer.ReadByte();
            EndDate.nWinLight[2] = readbuffer.ReadByte();

            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.Game_End(EndDate);
            }
            return true;
        }

        private bool HandleExitMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            return true;
        }

        private bool HandBetFailMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlayBetFail BetFail = new CMD_S_PlayBetFail();

            BetFail.wChairID = readbuffer.ReadUshort();
            BetFail.cbAnimalIndex = readbuffer.ReadByte();
            BetFail.lBetChip = readbuffer.ReadLong();

            Console.WriteLine("下注失败" );
            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.Game_BetFail(BetFail);
            }
            return true;
        }

        private bool HandBetClearMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_BetClear BetClear = new CMD_S_BetClear();

            BetClear.wChairID = readbuffer.ReadUshort();

            BetClear.lAllPlayBet = new long[ANIMAL_MAX];
            for (int i = 0; i < ANIMAL_MAX; i++) 
                BetClear.lAllPlayBet[i] = readbuffer.ReadLong();

            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.GAME_BetClear(BetClear);
            }
            return true;
        }

        private bool HandBetMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_PlayBet PlayBet = new CMD_S_PlayBet();

            PlayBet.wChairID= readbuffer.ReadUshort();						
            PlayBet.cbAnimalIndex = readbuffer.ReadByte();	
            PlayBet.lBetChip= readbuffer.ReadLong();

            PlayBet.lPlayerBet = new long[ANIMAL_MAX];

            for (int i = 0; i < ANIMAL_MAX; i++)//13不确定
            {
                PlayBet.lPlayerBet[i] = readbuffer.ReadLong();
            }
    

            PlayBet.lAllBet = new long[ANIMAL_MAX];

            for (int i = 0; i < ANIMAL_MAX; i++)//13不确定
            {
                PlayBet.lAllBet[i] = readbuffer.ReadLong();          
            }

            PlayBet.lPlayBet = new long[ANIMAL_MAX];

            for (int i = 0; i<ANIMAL_MAX; i++)//13不确定
            {
                PlayBet.lPlayBet[i] = readbuffer.ReadLong();  
            }
            PlayBet.bAndroid = readbuffer.ReadBoolean();
            Console.WriteLine("下注开始");
            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.Game_UserHandBet(PlayBet);
            }
            return true;
        }
        private bool HandleStartMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameStart start = new CMD_S_GameStart();

            Console.WriteLine("飞龙传说游戏开始");
            start.cbTimeLeave = readbuffer.ReadByte();
            
            //readbuffer.OffsetCurPos(3);
            start.cbAnimalMultiple = new byte[ANIMAL_MAX];
            //byte a = readbuffer.ReadByte();
            for (byte i = 0; i < ANIMAL_MAX; i++)
                start.cbAnimalMultiple[i] = readbuffer.ReadByte();           

            start.cbLuDanNum = readbuffer.ReadByte();
            start.cbLuDan = new byte[start.cbLuDanNum];
            for (byte i = 0; i < start.cbLuDanNum; i++)
                start.cbLuDan[i] = readbuffer.ReadByte();

            start.lStorageStart = readbuffer.ReadLong();
            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.Game_StartGame(start);
            }
            return true;
        }


        private bool HandleAMDIN_COMMANDMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_CommandResult result = new CMD_S_CommandResult();
            result.cbAckType = readbuffer.ReadByte();
            result.cbResult = readbuffer.ReadByte();
            result.cbExtendData = new byte[128];
            for (int i = 0; i < result.cbExtendData.Length; i++)
            {
                result.cbExtendData[i]=readbuffer.ReadByte();
            }
            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.Game_AMDIN_COMMAND(result);
            }
            return true;
        }

        public bool HandleIsMasterMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GM_Master master = new CMD_S_GM_Master();
            master.bIsMaster = readbuffer.ReadBoolean();
            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.GM_IsMaster(master.bIsMaster);
            }
            return true;
        }
    }
}
