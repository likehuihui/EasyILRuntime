using System;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;
namespace DragonLegend
{
    /// <summary>
    /// ////////////////////////////////////////////code by kingsley  6/27/2016 ///////////////////////
    /// </summary>
    public partial class DragonLegend_ : Interface_Game
    {        
        private DragonLegend_()
        {

        }
        private static DragonLegend_ Dragon_ = null;
        /// <summary>
        /// 飞龙传说 U3D回调函数
        /// </summary>
        public U3D_DragonLegend_Interface DragonLegendInterFace = null;
        public static DragonLegend_ DragonLegendInstance
        {
            get
            {
                if (Dragon_ == null)
                {
                    Dragon_ = new DragonLegend_();
                }
                return Dragon_;
            }
        }
        public void Error(string errStr)
        {
            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.Error(errStr);
            }
        }
        //         public bool CheckGame()
        //         {
        //             return true;
        //         }
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
            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.GAME_GameStatus(msg);
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
            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.GAME_SystemMessage(msg.szString);
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
        public const byte RECORD_COUNT_MAX = 10;                    //最大数量 
        public const byte ANIMAL_MAX = 13;					//最大数量 
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
                        free.lCellScore = readbuffer.ReadInt();
                        free.lPlayScore = readbuffer.ReadLong();
                        free.lStorageStart = readbuffer.ReadLong();

                        free.lMinBetScore = readbuffer.ReadLong();
                        free.lMaxBetScore = readbuffer.ReadLong();
                        free.lAreaLimitScore = readbuffer.ReadLong();
                        free.lPlayLimitScore = readbuffer.ReadLong();
                        free.lUserLimitDragonScore = readbuffer.ReadLong();

                        free.nTurnTableRecord = new int[RECORD_COUNT_MAX];
                        for (int i = 0; i < RECORD_COUNT_MAX; i++)
                            free.nTurnTableRecord[i] = readbuffer.ReadInt();

                        free.szGameRoomName = readbuffer.ReadString(64);

                        if (DragonLegendInterFace != null)
                        {
                            DragonLegendInterFace.GAME_SCENE_FREE(free);
                        }
                    }
                    break;
                case GS_TK_BET:
                    {
                        CMD_S_StatusPlay StatusPlay = new CMD_S_StatusPlay();

                        StatusPlay.cbTimeLeave = readbuffer.ReadByte();
                        StatusPlay.lCellScore = readbuffer.ReadInt();
                        StatusPlay.lPlayScore = readbuffer.ReadLong();
                        StatusPlay.lPlayChip = readbuffer.ReadLong();
                        StatusPlay.lStorageStart = readbuffer.ReadLong();

                        StatusPlay.lMinBetScore = readbuffer.ReadLong();
                        StatusPlay.lMaxBetScore = readbuffer.ReadLong();
                        StatusPlay.lAreaLimitScore = readbuffer.ReadLong();
                        StatusPlay.lPlayLimitScore = readbuffer.ReadLong();
                        StatusPlay.lUserLimitDragonScore = readbuffer.ReadLong();

                        StatusPlay.cbAnimalMultiple = new byte[ANIMAL_MAX];
                        StatusPlay.lAllBet = new long[ANIMAL_MAX];
                        StatusPlay.lPlayBet = new long[ANIMAL_MAX];

                        for (int i = 0; i < ANIMAL_MAX; i++)
                        {
                            StatusPlay.cbAnimalMultiple[i] = readbuffer.ReadByte();           
                        }

                        for (int i = 0; i < ANIMAL_MAX; i++)
                        {                            
                            StatusPlay.lAllBet[i] = readbuffer.ReadLong();                       
                        }

                        for (int i = 0; i < ANIMAL_MAX; i++)
                        {        
                            StatusPlay.lPlayBet[i] = readbuffer.ReadLong();
                        }


                        StatusPlay.nTurnTableRecord = new int[RECORD_COUNT_MAX];
                        for (int i = 0; i < RECORD_COUNT_MAX; i++)
                            StatusPlay.nTurnTableRecord[i] = readbuffer.ReadInt();

                        StatusPlay.szGameRoomName = readbuffer.ReadString(64);


                        if (DragonLegendInterFace != null)
                        {
                            DragonLegendInterFace.GAME_SCENE_BET(StatusPlay);
                        }
                    }
                    break;
                case GS_TK_END:
                    {
                        CMD_S_StatusPlay StatusPlay = new CMD_S_StatusPlay();

                        StatusPlay.cbTimeLeave = readbuffer.ReadByte();
                        StatusPlay.lCellScore = readbuffer.ReadInt();
                        StatusPlay.lPlayScore = readbuffer.ReadLong();
                        StatusPlay.lPlayChip = readbuffer.ReadLong();
                        StatusPlay.lStorageStart = readbuffer.ReadLong();

                        StatusPlay.lMinBetScore = readbuffer.ReadLong();
                        StatusPlay.lMaxBetScore = readbuffer.ReadLong();
                        StatusPlay.lAreaLimitScore = readbuffer.ReadLong();
                        StatusPlay.lPlayLimitScore = readbuffer.ReadLong();
                        StatusPlay.lUserLimitDragonScore = readbuffer.ReadLong();

                        StatusPlay.cbAnimalMultiple = new byte[ANIMAL_MAX];
                        StatusPlay.lAllBet = new long[ANIMAL_MAX];
                        StatusPlay.lPlayBet = new long[ANIMAL_MAX];

                        for (int i = 0; i < ANIMAL_MAX; i++)
                        {
                            StatusPlay.cbAnimalMultiple[i] = readbuffer.ReadByte();
                        }

                        for (int i = 0; i < ANIMAL_MAX; i++)
                        {
                            StatusPlay.lAllBet[i] = readbuffer.ReadLong();
                        }

                        for (int i = 0; i < ANIMAL_MAX; i++)
                        {
                            StatusPlay.lPlayBet[i] = readbuffer.ReadLong();
                        }

                        StatusPlay.nTurnTableRecord = new int[RECORD_COUNT_MAX];
                        for (int i = 0; i<RECORD_COUNT_MAX; i++)
                            StatusPlay.nTurnTableRecord[i] = readbuffer.ReadInt();

                         StatusPlay.szGameRoomName = readbuffer.ReadString(64);

                       
                        if (DragonLegendInterFace != null)
                        {
                            DragonLegendInterFace.GAME_SCENE_END(StatusPlay);
                        }
                    }
                    break;                
                default:
                    Error("HandleSceneMsg 未知状态：" + GAME_STATUS_WAIT);
                    break;
            }
            if (DragonLegendInterFace != null)
            {
                DragonLegendInterFace.GAME_SCENE_OK();
            }
            return true;
        }
    }

  

    ///////////////////////////////////////////////////////////////////////

   public struct CMD_S_StatusFree
    {
        public byte cbTimeLeave;                       //剩余时间

        public int lCellScore;                            //底分
        public long lPlayScore;                            //玩家分数
        public long lStorageStart;                     //库存（彩池）

        public long lMinBetScore;                      //最小注码
        public long lMaxBetScore;                      //最大注码
        public long lAreaLimitScore;                   //区域限制
        public long lPlayLimitScore;                   //玩家限制
        public long lUserLimitDragonScore;             //玩家压龙

        public int[] nTurnTableRecord;              //游戏记录     10
                                      
        public string szGameRoomName;           //房间名称  64
    };

    //游戏状态
    public struct CMD_S_StatusPlay
    {
        public byte cbTimeLeave;                       //剩余时间

        public int lCellScore;                            //底分
        public long lPlayScore;                            //玩家分数
        public long lPlayChip;                         //玩家筹码
        public long lStorageStart;                     //库存（彩池）		

        public long lMinBetScore;                      //最小注码
        public long lMaxBetScore;                      //最大注码
        public long lAreaLimitScore;                   //区域限制
        public long lPlayLimitScore;                   //玩家限制
        public long lUserLimitDragonScore;             //玩家压龙

        public byte[] cbAnimalMultiple;        //动物倍数  Animal_type.ANIMAL_MAX
        public long[] lAllBet;               //总下注   Animal_type.ANIMAL_MAX
        public long[] lPlayBet;              //玩家下注  Animal_type.ANIMAL_MAX

        public int[] nTurnTableRecord;          //游戏记录 10

        //房间信息
        public string szGameRoomName;           //房间名称64
    };

    //游戏状态
    public struct CMD_S_StatusEnd
    {
        public byte cbTimeLeave;                       //剩余时间

        public bool bTurnTimes;                            //转盘次数

        public int iAnimalIndex;                       //动物索引
        public int nAnimalID;                          //转盘动物
        public int nRandomAnimal;                      //随机动物
        public int nRandomAnimalIndex;

        public long[] lPlaybetScore;           //2
        public long lPlayWinScore;                     //玩家最后得分
                                                       //LONGLONG						lPlayPrizes;						//玩家彩金

        public long lPlayShowPrizes;                   //显示彩金
    };
    //////////////////////////////////////////////////////////////////////////
}
