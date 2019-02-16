using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLWH
{
    //游戏消息
    public partial class SLWH_
    {
        #region 客户端发送命令

        public const ushort SUB_C_PLACE_JETTON			=    1;								//用户下注
        public const ushort SUB_C_CLEAR_JETTON			=    2;								//清除下注
        public const ushort SUB_C_APPLY_BANKER			=    3;								//申请庄家
        public const ushort SUB_C_CANCEL_BANKER			=    4;								//取消申请
        public const ushort SUB_C_CONTROL_GET_INFO		=	5;							//管理员获取系统信息
        public const ushort SUB_C_CONTROL_SET_PRIZE		=	6;							//管理员设置开奖结果
        public const ushort SUB_C_CONTROL_UPDATE_STORAGE = 7;							//更新库存
        public const ushort SUB_C_CONTROL_GET_STORAGE = 8;								//获得库存
        public const ushort SUB_C_GET_JETTON = 10;							//获取最新下注级别
        public const ushort SUB_C_GET_ADMIN = 11;                              //是否管理员
        public const ushort SUB_C_CONTROL_GET_PRIZE = 12;							//管理员获取开奖结果
        public const ushort SUB_C_CONTROL_DEL_PRIZE = 13;							//管理员取消开奖结果
        public const ushort SUB_C_CONTROL_GET_PROB = 14;							//管理员获取概率列表
        public const ushort SUB_C_CONTROL_SET_PROB = 15;							//管理员设置概率列表
        public const ushort SUB_C_CONTROL_BINGO_PROB = 16;							//管理员设置大奖概率
        #endregion

        #region 客户端结构体
        //用户下注
        public struct CMD_C_PlaceJetton
        {
            public eGambleType eGamble;
            public tagSTAnimalInfo stAnimalInfo;
            public eEnjoyGameType eEnjoyGameInfo;
            public long iPlaceJettonScore;										//当前下注
        };

        //切换庄家
        public struct CMD_C_ChangeBanker
        {
            public ushort wBankerUser;						//当庄玩家
            public long lBankerScore;						//庄家分数
        };

        /// <summary>
        /// 下注失败
        /// </summary>
        public struct CMD_C_PlaceBetFail
        {

            public ushort wPlaceUser;							//下注玩家
            public byte lBetArea;							//下注区域
            public long lPlaceScore;						//当前下注
        };

        //用户控制指令
        public struct CMD_C_Control
        {
            public byte cbCmdType;  //控制类型 库存？ 开什么动物？庄闲和？ 5管理员获取系统信息 6管理员设置开奖结果 7更新(设置)库存 8获取最新库存信息
            public ushort wChairID; //椅子号
            public int times;  //控制局数 默认为1
            public RewardType eAnimalIndexes;    //开奖索引 	eAnimalType_Invalid 19 霹雳闪电
            public eAnimalType[] eAnimal; //动物 12
            public eColorType[] eColor;  //颜色  12
            public eEnjoyGameType eEnjoyGame;  //庄闲和
            public long lStorageScore;   //库存
            public long lDecuteScore;   //抽水值
            public uint nStorageDecute;  //抽水比例

            public long tempRevenueValue;       //库存抽水值
            public long tempRevenueLimitValue;  //库存抽水阈值
            public double tempRevenuePer;       //库存抽水率
        };


        public struct CMD_C_BingoProb
        {
            public bool isGet;  //ture 获取  false 设置
            public int BingoProb; //大奖出现概率
        };

        //记录信息
        public struct tagClientGameRecord
        {
            //enOperateResult					enOperateFlags;						//操作标识
            //BYTE							cbPlayerCount;						//闲家点数
            //BYTE							cbBankerCount;						//庄家点数
            //BYTE							cbKingWinner;						//天王赢家
            //bool							bPlayerTwoPair;						//对子标识
            //bool							bBankerTwoPair;						//对子标识

            public  tagSTPrizeInfo stAnimalWinRecord;
            public  tagSTEnjoyGamePrizeInfo stEnjoyGameRecord;
        };

       #endregion


        public void SendPlaceJetton(CMD_C_PlaceJetton msg)
        {
            CMD_C_PlaceJetton Jetton = new CMD_C_PlaceJetton();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            Jetton.eEnjoyGameInfo = msg.eEnjoyGameInfo;
            Jetton.eGamble = msg.eGamble;
            Jetton.iPlaceJettonScore = msg.iPlaceJettonScore;
            //bet.stAnimalInfo = msg.stAnimalInfo;
            Jetton.stAnimalInfo.eAnimal = msg.stAnimalInfo.eAnimal;
            Jetton.stAnimalInfo.eColor = msg.stAnimalInfo.eColor;
            
            buffer.Write((int)Jetton.eGamble);
            buffer.Write((int)Jetton.stAnimalInfo.eAnimal);
            buffer.Write((int)Jetton.stAnimalInfo.eColor);
            buffer.Write((int)Jetton.eEnjoyGameInfo);
            buffer.Write(Jetton.iPlaceJettonScore);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_PLACE_JETTON, buffer);

        }


        public void SendCLEAR_JETTON()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CLEAR_JETTON, buffer);
        }
        
        public void SendGET_New_JETTON()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GET_JETTON, buffer);
        }

        public void SendApplyBanker(ushort wOpUser)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            CMD_S_ApplyBanker banker = new CMD_S_ApplyBanker();
            banker.wApplyUser = wOpUser;
            buffer.Write(banker.wApplyUser);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_APPLY_BANKER, buffer);
        }

        public void SendCancelBanker(string CancelUser, ushort wOpUser)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            CMD_S_CancelBanker banker = new CMD_S_CancelBanker();
            banker.szCancelUser = CancelUser;
            banker.dwUserID = wOpUser;
            buffer.Write(banker.szCancelUser);
            buffer.Write(banker.dwUserID);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CANCEL_BANKER, buffer);
        }

        public void SendGM_Control(CMD_C_Control control,byte type=0)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            if (type > 0)
            {
                control.cbCmdType = type;
                control.eAnimal = new eAnimalType[12];
                control.eColor = new eColorType[12];
                for (int i = 0; i < control.eAnimal.Length; i++)
                {
                    control.eAnimal[i] = eAnimalType.eAnimalType_Invalid;
                    control.eColor[i] = eColorType.eColorType_Invalid;
                }
            }
            buffer.Write(control.cbCmdType);
            buffer.Write(control.wChairID);
            buffer.Write(control.times);
            buffer.Write((int)control.eAnimalIndexes);
            for (int i = 0; i < control.eAnimal.Length; i++)
            {
                buffer.Write((int)control.eAnimal[i]);
            }
            for (int i = 0; i < control.eColor.Length; i++)
            {
                buffer.Write((int)control.eColor[i]);
            }
            buffer.Write((int)control.eEnjoyGame);
            buffer.Write(control.lStorageScore);
            buffer.Write(control.lDecuteScore);
            buffer.Write(control.nStorageDecute);

            buffer.Write(control.tempRevenueValue);
            buffer.Write(control.tempRevenueLimitValue);
            buffer.Write(control.tempRevenuePer);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, control.cbCmdType, buffer); //待定  SUB_C_CONTROL_SET_PRIZE
        }


        public bool GetIsConnet()
        {
            return MessageMgr.CurMsgMgr.GameSock.IsConnected;
        }

        public void SendReqGM()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GET_ADMIN, buffer);
        }


        public void SendGM_GET_STORAGE()
        {
            SendGM_Control(new CMD_C_Control(), (byte)SUB_C_CONTROL_GET_STORAGE);
        }

        public void SendGM_GET_PrizeInfo()
        {
            SendGM_Control(new CMD_C_Control(), (byte)SUB_C_CONTROL_GET_PRIZE);
            
        }

        public void SendGM_GET_ProbInfo()
        {
            SendGM_Control(new CMD_C_Control(), (byte)SUB_C_CONTROL_GET_PROB);
        }
        

        public void SendGM_DEL_PrizeInfo()
        {
            SendGM_Control(new CMD_C_Control(), (byte)SUB_C_CONTROL_DEL_PRIZE);
        }

        public void SendGM_SET_ProbInfo(SLWH_.CMD_S_PorbInfo msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            for (int i = 0; i < msg.EatProbArr.Length; i++)
            {
                buffer.Write(msg.EatProbArr[i]);        
            }
            for (int i = 0; i < msg.SendProbArr.Length; i++)
            {
                buffer.Write(msg.SendProbArr[i]);
            }
            for (int i = 0; i < msg.ComeProbArr.Length; i++)
            {
                buffer.Write(msg.ComeProbArr[i]);
            }
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CONTROL_SET_PROB, buffer); 
        }


        public void SendGM_BinGo_ProbInfo(SLWH_.CMD_C_BingoProb msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msg.isGet);
            buffer.Write(msg.BingoProb);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CONTROL_BINGO_PROB, buffer);
        }
    }
}
