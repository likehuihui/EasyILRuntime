using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;
namespace OxBattle
{
    /// <summary>
    /// ////////////////////////////////////////////code by kingsley  10/8/2016 ///////////////////////
    /// </summary>
    public partial class OxBattle_ : Interface_Game
    {
        private OxBattle_()
        {

        }
        private static OxBattle_ Battle_ = null;
        /// <summary>
        ///  U3D回调函数
        /// </summary>
        public U3D_OxBattle_Interface OxBattleInterFace = null;
        public static OxBattle_ OxBattleInstance
        {
            get
            {
                if (Battle_ == null)
                {
                    Battle_ = new OxBattle_();
                }
                return Battle_;
            }
        }
        public void Error(string errStr)
        {
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.Error(errStr);
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
                case GameOperations.MDM_GR_INSURE://银行命令
                    return HandleInsureMsg(mnCmd, sbCmd, readBuffer);
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
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.GAME_GameStatus(msg);
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
            msg.szString = readbuffer.ReadString(msg.wLength);
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.GAME_SystemMessage(msg.szString);
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
        public const byte AREA_COUNT = 5;					//最大数量 
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
            const byte GS_TK_PLACE_JETTON = GAME_STATUS_PLAY;	            //下注状态
            const byte GS_TK_END = GAME_STATUS_PLAY + 1;            //开奖状态

            switch (msg_Status.cbGameStatus)
            {
                case GS_TK_FREE:
                    {
                        CMD_S_StatusFree free = new CMD_S_StatusFree();
                        free.cbTimeLeave = readbuffer.ReadByte();

                        free.lUserMaxScore = readbuffer.ReadLong();
                        free.wBankerUser = readbuffer.ReadUshort();
                        free.cbBankerTime = readbuffer.ReadUshort();

                        free.lBankerWinScore = readbuffer.ReadLong();
                        free.lBankerScore = readbuffer.ReadLong();
                        free.bEnableSysBanker = readbuffer.ReadBoolean();
                        free.lApplyBankerCondition = readbuffer.ReadLong();
                        free.lAreaLimitScore = readbuffer.ReadLong();

                        free.bIsGameCheatUser = readbuffer.ReadBoolean();
                        free.szGameRoomName = readbuffer.ReadString(64);

                        free.szJettonRange = new long[5];
                        for (byte i=0;i<5;i++)
                        {
                            free.szJettonRange[i] = readbuffer.ReadLong();
                        }
                        free.wBankerTimesLimit = readbuffer.ReadUshort();
                        if (OxBattleInterFace != null)
                        {
                            OxBattleInterFace.GAME_SCENE_FREE(free);
                        }
                    }
                    break;
                case GS_TK_PLACE_JETTON:
                case GS_TK_END:
                    {
                        CMD_S_StatusPlay StatusPlay = new CMD_S_StatusPlay();

                        StatusPlay.lAllJettonScore = new long[AREA_COUNT];

                        for (int i = 0; i < AREA_COUNT; i++)
                            StatusPlay.lAllJettonScore[i] = readbuffer.ReadLong();

                        StatusPlay.lUserJettonScore = new long[AREA_COUNT];

                        for (int i = 0; i < AREA_COUNT; i++)
                            StatusPlay.lUserJettonScore[i] = readbuffer.ReadLong();



                        StatusPlay.lUserMaxScore = readbuffer.ReadLong();

                        StatusPlay.lApplyBankerCondition = readbuffer.ReadLong();
                        StatusPlay.lAreaLimitScore = readbuffer.ReadLong();

                        StatusPlay.cbTableCardArray = new byte[AREA_COUNT][];

                        for (int i = 0; i < AREA_COUNT; i++)
                        {
                            StatusPlay.cbTableCardArray[i] = new byte[AREA_COUNT];
                            for (int j = 0; j < AREA_COUNT; j++)
                            {
                                StatusPlay.cbTableCardArray[i][j] = readbuffer.ReadByte();
                            }
                        }

                        StatusPlay.wBankerUser = readbuffer.ReadUshort();
                        StatusPlay.cbBankerTime = readbuffer.ReadUshort();
                        StatusPlay.lBankerWinScore = readbuffer.ReadLong();
                        StatusPlay.lBankerScore = readbuffer.ReadLong();
                        StatusPlay.bEnableSysBanker = readbuffer.ReadBoolean();

                        StatusPlay.lEndBankerScore = readbuffer.ReadLong();
                        StatusPlay.lEndUserScore = readbuffer.ReadLong();
                        StatusPlay.lEndUserReturnScore = readbuffer.ReadLong();
                        StatusPlay.lEndRevenue = readbuffer.ReadLong();


                        StatusPlay.cbTimeLeave = readbuffer.ReadByte();
                        StatusPlay.cbGameStatus = readbuffer.ReadByte();

                        StatusPlay.bIsGameCheatUser = readbuffer.ReadBoolean();

                        StatusPlay.szGameRoomName = readbuffer.ReadString(64);
                        StatusPlay.szJettonRange = new long[5];
                        for (byte i = 0; i < 5; i++)
                        {
                            StatusPlay.szJettonRange[i] = readbuffer.ReadLong();
                        }
                        StatusPlay.wBankerTimesLimit = readbuffer.ReadUshort();

                        if (OxBattleInterFace != null)
                        {
                            OxBattleInterFace.GAME_SCENE_PLACE_JETTON(StatusPlay);
                        }
                    }
                    break;

                default:
                    Error("HandleSceneMsg 未知状态：" + GAME_STATUS_WAIT);
                    break;
            }
            if (OxBattleInterFace != null)
            {
                OxBattleInterFace.GAME_SCENE_OK();
            }
            return true;
        }
    }



    ///////////////////////////////////////////////////////////////////////

    public struct CMD_S_StatusFree
    {
        public byte cbTimeLeave;                       //剩余时间

        public long lUserMaxScore;                         //玩家金币

        public ushort wBankerUser;                          //玩家位置

        public ushort cbBankerTime;                     //庄家局数
        public long lBankerWinScore;                    //庄家成绩
        public long lBankerScore;                       //庄家分数
        public bool bEnableSysBanker;                   //系统做庄

        //控制信息
        public long lApplyBankerCondition;              //申请条件
        public long lAreaLimitScore;					//区域限制
        public bool bIsGameCheatUser;					//是否是管理用户                              
        public string szGameRoomName;           //房间名称  64
        public long[] szJettonRange;                  //筹码列表  5
        public ushort wBankerTimesLimit;					//做庄次数上限
    };

    //游戏状态
    public struct CMD_S_StatusPlay
    {
        //全局下注
        public long[] lAllJettonScore;      //全体总注

        //玩家下注
        public long[] lUserJettonScore;     //个人总注

        //玩家积分
        public long lUserMaxScore;                      //最大下注							

        //控制信息
        public long lApplyBankerCondition;              //申请条件
        public long lAreaLimitScore;                    //区域限制

        //扑克信息
        public byte[][] cbTableCardArray;               //桌面扑克

        //庄家信息
        public ushort wBankerUser;                      //当前庄家
        public ushort cbBankerTime;                     //庄家局数
        public long lBankerWinScore;                    //庄家赢分
        public long lBankerScore;                       //庄家分数
        public bool bEnableSysBanker;                   //系统做庄

        //结束信息
        public long lEndBankerScore;                    //庄家成绩
        public long lEndUserScore;                      //玩家成绩
        public long lEndUserReturnScore;                //返回积分
        public long lEndRevenue;                        //游戏税收

        //全局信息
        public byte cbTimeLeave;                        //剩余时间
        public byte cbGameStatus;						//游戏状态

        public bool bIsGameCheatUser;					//是否是管理用户      
        //房间信息
        public string szGameRoomName;           //房间名称64
        public long[] szJettonRange;                  //筹码列表5
        public ushort    wBankerTimesLimit;					//做庄次数上限
    };


    //////////////////////////////////////////////////////////////////////////
}
