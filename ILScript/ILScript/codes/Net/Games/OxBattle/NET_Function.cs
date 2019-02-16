using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OxBattle
{
    public partial class OxBattle_
    {
        /// <summary>
        /// ////////////////////////////////////////////code by kingsley 10/8/2016 ///////////////////////
        /// </summary>
        #region 客户端发送命令

        public const ushort SUB_C_PLACE_JETTON = 1;								//用户下注
        public const ushort SUB_C_APPLY_BANKER = 2;								//申请庄家
        public const ushort SUB_C_CANCEL_BANKER = 3;								//取消申请
        public const ushort SUB_C_CONTINUE_CARD = 4;								//继续发牌
        public const ushort SUB_C_AMDIN_COMMAND = 5;								//管理员命令
        public const ushort SUB_C_UPDATE_STORAGE = 6;								//更新库存
        public const ushort SUB_C_STORAGE_READ = 7;								    //读取
        public const ushort SUB_C_STORAGE_SAVE = 8;                                 //保存
        public const ushort SUB_C_BANKER_LIST = 9;                                 //上庄列表
        public const ushort SUB_C_REFRESH_TRUTH_PLAYER = 10;						//刷新真实玩家
        public const ushort SUB_GR_TAKE_SCORE_REQUEST = 4;                           //取款请求
        public const ushort SUB_C_CONTINUE_JETTON = 11;                           //续押注

        #endregion
        #region 客户端结构体



        //下注消息
        public struct CMD_C_PlaceJetton
        {
            public byte cbJettonArea;                       //筹码区域
            public long lJettonScore;                       //加注数目
        };

        //更新库存
        public struct CMD_C_UpdateStorage
        {
            public byte cbReqType;                       //请求类型
            public long lStorage;                       //新库存值
            public long lStorageDeduct;                 //库存衰减

            //库存抽水率
            public long tempRevenueValue;
            //库存抽水仓库数值
            public long limitTempRevenueValue;
            //库存抽水仓库存入库存的要求数值
            public int tempRevenuePer;
        };
        //取款请求
        public struct CMD_GR_C_TakeScoreRequest
        {
            public byte cbActivityGame;                     //游戏动作
            public long lTakeScore;                         //取款数目
            public string szInsurePass;			//银行密码33
        };

        public const ushort CS_BANKER_LOSE = 1;
        public const ushort CS_BANKER_WIN = 2;
        public const ushort CS_BET_AREA = 3;
        public struct tagAdminReq
        {
            public byte m_cbExcuteTimes;                    //执行次数
            public byte m_cbControlStyle;                   //控制方式
            public bool[] m_bWinArea;                           //赢家区域
        };

        public const byte RQ_SET_WIN_AREA = 1;
        public const byte RQ_RESET_CONTROL = 2;
        public const byte RQ_PRINT_SYN = 3;
        public struct CMD_C_AdminReq
        {
            public byte cbReqType;

            public byte[] cbExtendData;         //附加数据
        };


        public struct CMD_C_Storage_Save
        {
            public long lStorageStart;                      //库存数值
            public int nStorageDeduct;                      //扣取比例
            public long lStorageDeductAll;                  //扣除库存

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
        };


        #endregion


        /// <summary>
        /// 用户下注
        /// </summary>
        /// <param name="score"></param>
        public void NetPlaceJetton(byte cbJettonArea, long score)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            CMD_C_PlaceJetton JiaZhu = new CMD_C_PlaceJetton();

            JiaZhu.cbJettonArea = cbJettonArea;
            buffer.Write(JiaZhu.cbJettonArea);
            JiaZhu.lJettonScore = score;
            buffer.Write(JiaZhu.lJettonScore);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_PLACE_JETTON, buffer);
        }

        /// <summary>
        /// 申请庄家
        /// </summary>
        /// <param name="user"></param>
        public void UserApplyBanker()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            //发送准备
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_APPLY_BANKER, buffer);
        }

        /// <summary>
        /// 取消申请庄家
        /// </summary>
        /// <param name="user"></param>
        public void UserCancelBanker()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            //发送准备
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CANCEL_BANKER, buffer);
        }

        public bool GetIsConnet()
        { 
            if(MessageMgr.CurMsgMgr == null || MessageMgr.CurMsgMgr.GameSock == null)
            {
                return false;
            }
            return MessageMgr.CurMsgMgr.GameSock.IsConnected;
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="user"></param>
        public void Admin_request_ReSet()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            CMD_C_AdminReq Req = new CMD_C_AdminReq();
            Req.cbReqType = RQ_RESET_CONTROL;
            buffer.Write(Req.cbReqType);
            Req.cbExtendData = new byte[20];
            for (int i = 0; i < 20; i++)
                buffer.Write(Req.cbExtendData[i]);
            //发送准备
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_AMDIN_COMMAND, buffer);
        }


        /// <summary>
        /// 当前设置
        /// </summary>
        /// <param name="user"></param>
        public void Admin_request_Refresh()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            CMD_C_AdminReq Req = new CMD_C_AdminReq();
            Req.cbReqType = RQ_PRINT_SYN;
            buffer.Write(Req.cbReqType);
            Req.cbExtendData = new byte[20];
            for (int i = 0; i < 20; i++)
                buffer.Write(Req.cbExtendData[i]);
            //发送准备
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_AMDIN_COMMAND, buffer);
        }

        /// <summary>
        /// 执行控制
        /// </summary>
        /// <param name="user"></param>
        public void Admin_request_Excute(byte cbExcuteTimes, byte cbControlStyle, bool[] WinArea, byte WinAreaCount)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            CMD_C_AdminReq Req = new CMD_C_AdminReq();
            Req.cbReqType = RQ_SET_WIN_AREA;
            buffer.Write(Req.cbReqType);
            Req.cbExtendData = new byte[20];

            buffer.Write(cbExcuteTimes);
            buffer.Write(cbControlStyle);
            for (byte i = 0; i < WinAreaCount; i++)
            {
                buffer.Write(WinArea[i]);
            }

            bool[] Buf = new bool[14];
            for (byte i = 0; i < 14; i++)
            {
                buffer.Write(Buf[i]);
            }
            //发送准备
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_AMDIN_COMMAND, buffer);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="user"></param>
        public void Admin_Storage_Read()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            //发送准备
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_STORAGE_READ, buffer);
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="user"></param>
        public void Admin_Storage_Save(CMD_C_Storage_Save SaveInfo)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            buffer.Write(SaveInfo.lStorageStart);
            buffer.Write(SaveInfo.nStorageDeduct);
            buffer.Write(SaveInfo.lStorageDeductAll);
            buffer.Write(SaveInfo.tempRevenueValue);
            buffer.Write(SaveInfo.limitTempRevenueValue);
            buffer.Write(SaveInfo.tempRevenuePer);
            buffer.Write(SaveInfo.lWinRate);
            for (byte i = 0; i < 5; i++)
            {
                buffer.Write(SaveInfo.uiWinRate[i]);
            }
            buffer.Write(SaveInfo.RateTimeMin);
            buffer.Write(SaveInfo.RateTimeMax);
            buffer.Write(SaveInfo.RateStockMin);
            buffer.Write(SaveInfo.RateStockMax);

            //发送准备
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_STORAGE_SAVE, buffer);
        }

        /// <summary>
        /// 更新库存
        /// </summary>
        /// <param name="user"></param>
        public void Admin_UpdateStorage(CMD_C_UpdateStorage UpdateInfo)
        {

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(UpdateInfo.cbReqType);
            buffer.Write(UpdateInfo.lStorage);
            buffer.Write(UpdateInfo.lStorageDeduct);
            buffer.Write(UpdateInfo.tempRevenueValue);
            buffer.Write(UpdateInfo.limitTempRevenueValue);
            buffer.Write(UpdateInfo.tempRevenuePer);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_UPDATE_STORAGE, buffer);
        }
        /////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 银行取款请求
        /// </summary>
        /// <param name="user"></param>
        public void TakeScoreRequest(byte cbActivityGame, long lTakeScore, string szInsurePass)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cbActivityGame);
            buffer.Write(lTakeScore);
            buffer.Write(szInsurePass, 66);
            //发送准备
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_INSURE, SUB_GR_TAKE_SCORE_REQUEST, buffer);
        }

        public void BankerListRequest()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_BANKER_LIST, buffer);
        }

        public void RealPlayerListRequest()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_REFRESH_TRUTH_PLAYER, buffer);
        }

        //续压
        public void BetAgainRequest(stUserJetton[] betValue)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();    
            for (byte i=0;i<betValue.Length;i++)
            {
                buffer.Write(betValue[i].cbJettonArea);
                buffer.Write(betValue[i].lJettonScore);
            }
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CONTINUE_JETTON, buffer);
        }
    }
}
