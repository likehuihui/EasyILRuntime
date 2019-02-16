using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Baccarat
{
    //游戏消息
    public partial class Baccarat_
    {
        #region 客户端发送命令


        public const ushort SUB_C_PLACE_JETTON = 1;							//用户下注
        public const ushort SUB_C_APPLY_BANKER = 2;							//申请庄家
        public const ushort SUB_C_CANCEL_BANKER = 3;                        //取消申请
        public const ushort SUB_C_CONTINUE_JETTON = 4;                      //续押注
        public const ushort SUB_C_APPLY_BANKER_LIST = 5;                    //申请庄家列表
        public const ushort SUB_C_PLAYER_LIST = 6;                          //玩家列表
        public const ushort SUB_C_AMDIN_COMMAND = 11;						//管理员命令 //开奖控制
        public const ushort SUB_C_UPDATE_STORAGE = 12;						//更新库存
        public const ushort SUB_C_STORAGE_READ = 13;                        //读取设置(GM库存控制)
        public const ushort SUB_C_STORAGE_SAVE = 14;						//保存设置
        public const ushort SUB_C_REFRESH_TRUTH_PLAYER = 15;                //刷新真实玩家


        #endregion

        #region 客户端结构体
        //用户下注
        struct CMD_C_PlaceJetton
        {
            public byte cbJettonArea;                      //筹码区域
            public long lJettonScore;                      //加注数目
        };


        //记录信息
        public struct stUserJetton
        {
            public byte cbJettonArea;                      //筹码区域
            public long lJettonScore;                      //加注数目
        };

        //用户续押注
        struct CMD_C_ContinueJetton
        {
            public stUserJetton[] szUserJetton; //各个区域押注 //长度为9
            //续押注     
        };

        ////申请庄家      取消申请
        //public struct CMD_C_OpBanker
        //{
        //    public ushort wOpUser;							//申请玩家  和取消申请共用
        //};

        //切换庄家
        public struct CMD_C_ChangeBanker
        {
            public ushort wBankerUser;						//当庄玩家
            public long lBankerScore;                       //庄家分数

        };

        // 下注失败
        public struct CMD_C_PlaceBetFail
        {

            public ushort wPlaceUser;							//下注玩家
            public byte lBetArea;							//下注区域
            public long lPlaceScore;						//当前下注
        };

        //更新库存
        public struct CMD_C_UpdateStorage                                              //SUB_C_UPDATE_STORAGE
        {
            public byte cbReqType;                         //请求类型  1更新库存   2清零库存
            public long m_lNowStockTake;                       //库存抽水值
            public long m_lStockTakeLimit;                     //库存抽水阀值
            public ushort m_wBankerRate;                         //庄家阀值抽水比例(千分比)
            public ushort m_wPlayerRate;                         //闲家阀值抽水比例(千分比)

            public long m_lNowStock;                           //当前库存
            public long m_lTakeAwayNum;                            //抽水统计
            public ushort m_wTakeAwayByBankerRate;               //庄家抽水值比例(千分比)
            public ushort m_wTakeAwayByPlayerRate;               //闲家抽水值比例(千分比)
            public ushort m_wSystemStockRate;						//系统抽数值比例(百分比)
        };
        //保存设置
        public struct CMD_C_SaveStorageAllInfo
        {
          //  public stStockControlInfo m_kStockControlInfo;
        };

        //开奖控制
        public struct stAdminReqInfo
        {
            public int m_nBankerControl;                       //庄家控制方式 CS_BANKER_LOSE-1   CS_BANKER_WIN-2
            public byte m_cbWinner;                            //区域,闲、庄、和
            public byte m_cbKingWinner;                        //区域,闲天、庄天、同点和
            public byte m_cbPlayerPair;                        //闲对
            public byte m_cbBankerPair;                     //庄对
        };

        //开奖结果控制
        public struct CMD_C_AdminReq                                                   //SUB_C_AMDIN_COMMAND
        {
            public byte m_cbReqType;//设置开奖控制 1	    请求已有得开奖控制2	
            public stAdminReqInfo[] m_szAdminReqInfo;			//附加数据 长度为10
        };

        #endregion


        // 用户下注
        public void SendPlaceBet(byte cbBetArea, long lBetScore)
        {
            CMD_C_PlaceJetton bet = new CMD_C_PlaceJetton();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            bet.cbJettonArea = cbBetArea;
            bet.lJettonScore = lBetScore;
            buffer.Write(bet.cbJettonArea);
            buffer.Write(bet.lJettonScore);

            Debug.Log("下注数据发送....");
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_PLACE_JETTON, buffer);
        }

        // 用户续押注
        public void betAgain(stUserJetton[] betData)
        {
            // CMD_C_ContinueJetton bet = new CMD_C_ContinueJetton();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            //bet.szUserJetton = betData;
            for (int i = 0; i < betData.Length; i++)
            {
                buffer.Write(betData[i].cbJettonArea);
                buffer.Write(betData[i].lJettonScore);
                Debug.Log("续压发送的分数----->>" + betData[i].lJettonScore);
            }


            Debug.Log("续下注数据发送....");
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CONTINUE_JETTON, buffer);
        }
        //上庄申请
        public void SendApplyBanker()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            // CMD_C_OpBanker banker = new CMD_C_OpBanker();
            //banker.wOpUser = wOpUser;
            //buffer.Write(banker.wOpUser);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_APPLY_BANKER, buffer);
        }

        //取消上庄申请
        public void SendCancelBanker()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CANCEL_BANKER, buffer);
        }

        //玩家列表
        public void SendPlayerList()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_PLAYER_LIST, buffer);
        }

        //上庄列表
        public void SendBankerList()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_APPLY_BANKER_LIST, buffer);
        }
        //刷新真实玩家列表
        public void RefreshPlayerList()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_REFRESH_TRUTH_PLAYER, buffer);
        }

        public void LoadGMConfig()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_STORAGE_READ, buffer);
        }

        //更新库存
        public void UpdateStock(CMD_C_UpdateStorage updateStock)
        {
            CMD_C_UpdateStorage update = new CMD_C_UpdateStorage();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            update.cbReqType = updateStock.cbReqType;
            update.m_lNowStockTake = updateStock.m_lNowStockTake;
            update.m_lStockTakeLimit = updateStock.m_lStockTakeLimit;
            update.m_wBankerRate = updateStock.m_wBankerRate;
            update.m_wPlayerRate = updateStock.m_wPlayerRate;
            update.m_lNowStock = updateStock.m_lNowStock;
            update.m_lTakeAwayNum = updateStock.m_lTakeAwayNum;
            update.m_wTakeAwayByBankerRate = updateStock.m_wTakeAwayByBankerRate;
            update.m_wTakeAwayByPlayerRate = updateStock.m_wTakeAwayByPlayerRate;
            update.m_wSystemStockRate = updateStock.m_wSystemStockRate;

            buffer.Write(update.cbReqType);
            buffer.Write(update.m_lNowStockTake);
            buffer.Write(update.m_lStockTakeLimit);
            buffer.Write(update.m_wBankerRate);
            buffer.Write(update.m_wPlayerRate);
            buffer.Write(update.m_lNowStock);
            buffer.Write(update.m_lTakeAwayNum);
            buffer.Write(update.m_wTakeAwayByBankerRate);
            buffer.Write(update.m_wTakeAwayByPlayerRate);
            buffer.Write(update.m_wSystemStockRate);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_UPDATE_STORAGE, buffer);

        }
        //清零库存(--修改库存--)
        public void clearStock(CMD_C_UpdateStorage clearStock)
        {
            CMD_C_UpdateStorage update = new CMD_C_UpdateStorage();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            update.cbReqType = clearStock.cbReqType;
            update.m_lNowStockTake = clearStock.m_lNowStockTake;
            update.m_lStockTakeLimit = clearStock.m_lStockTakeLimit;
            update.m_wBankerRate = clearStock.m_wBankerRate;
            update.m_wPlayerRate = clearStock.m_wPlayerRate;
            update.m_lNowStock = clearStock.m_lNowStock;
            update.m_lTakeAwayNum = clearStock.m_lTakeAwayNum;
            update.m_wTakeAwayByBankerRate = clearStock.m_wTakeAwayByBankerRate;
            update.m_wTakeAwayByPlayerRate = clearStock.m_wTakeAwayByPlayerRate;
            update.m_wSystemStockRate = clearStock.m_wSystemStockRate;

            buffer.Write(update.cbReqType);
            buffer.Write(update.m_lNowStockTake);
            buffer.Write(update.m_lStockTakeLimit);
            buffer.Write(update.m_wBankerRate);
            buffer.Write(update.m_wPlayerRate);
            buffer.Write(update.m_lNowStock);
            buffer.Write(update.m_lTakeAwayNum);
            buffer.Write(update.m_wTakeAwayByBankerRate);
            buffer.Write(update.m_wTakeAwayByPlayerRate);
            buffer.Write(update.m_wSystemStockRate);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_UPDATE_STORAGE, buffer);
        }

        //保存设置
        //public void SaveConfig(stStockControlInfo saveConfig)
        //{
        //    CMD_C_SaveStorageAllInfo save = new CMD_C_SaveStorageAllInfo();
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    save.m_kStockControlInfo = saveConfig;
        //    buffer.Write(save.m_kStockControlInfo.m_lNowStockTake);
        //    buffer.Write(save.m_kStockControlInfo.m_lStockTakeLimit);
        //    buffer.Write(save.m_kStockControlInfo.m_wBankerRate);
        //    buffer.Write(save.m_kStockControlInfo.m_wPlayerRate);

        //    buffer.Write(save.m_kStockControlInfo.m_lNowStock);
        //    buffer.Write(save.m_kStockControlInfo.m_lTakeAwayNum);

        //    buffer.Write(save.m_kStockControlInfo.m_wTakeAwayByBankerRate);
        //    buffer.Write(save.m_kStockControlInfo.m_wTakeAwayByPlayerRate);
        //    buffer.Write(save.m_kStockControlInfo.m_wSystemStockRate);
        //    buffer.Write(save.m_kStockControlInfo.m_wDiffcultLevel);

        //    for (int i = 0; i < 5; i++)
        //    {
        //        for (int j = 0; j < 2; j++)
        //        {
        //            buffer.Write(save.m_kStockControlInfo.m_szLevelWinRange[i][j]);
        //        }
        //    }
        //    for (int i = 0; i < 5; i++)
        //    {
        //        buffer.Write(save.m_kStockControlInfo.m_szLevelWinRate[i]);
        //    }
        //    for (int i = 0; i < 22; i++)
        //    {
        //        buffer.Write(save.m_kStockControlInfo.m_szCardTypeRate[i]);
        //    }
        //    buffer.Write(save.m_kStockControlInfo.m_wRateTimeMin);
        //    buffer.Write(save.m_kStockControlInfo.m_wRateTimeMax);
        //    buffer.Write(save.m_kStockControlInfo.m_lRateStockMin);
        //    buffer.Write(save.m_kStockControlInfo.m_lRateStockMax);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_STORAGE_SAVE, buffer);

        //}

        //读取开奖结果配置
        public void loadResultCtrlConfig()
        {    
            CMD_C_AdminReq resultCtrl = new CMD_C_AdminReq();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            resultCtrl.m_cbReqType = 2;
            resultCtrl.m_szAdminReqInfo = new stAdminReqInfo[10];
            buffer.Write(resultCtrl.m_cbReqType);
            for (int i=0;i<10;i++)
            {
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_nBankerControl);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbWinner);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbKingWinner);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbPlayerPair);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbBankerPair);
            }
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_AMDIN_COMMAND, buffer);
    }
        //保存开奖结果配置
        public void saveResultCtrlAll(stAdminReqInfo[] info)
        {
            CMD_C_AdminReq resultCtrl = new CMD_C_AdminReq();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            resultCtrl.m_cbReqType = 1;
            resultCtrl.m_szAdminReqInfo = info;
            buffer.Write(resultCtrl.m_cbReqType);
            for (int i = 0; i < 10; i++)
            {
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_nBankerControl);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbWinner);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbKingWinner);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbPlayerPair);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbBankerPair);
            }
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_AMDIN_COMMAND, buffer);
        }


        //保存开奖结果配置
        public void saveResultCtrlAll(stAdminReqInfo[] info,int length)
        {
            CMD_C_AdminReq resultCtrl = new CMD_C_AdminReq();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            resultCtrl.m_cbReqType = 1;
            resultCtrl.m_szAdminReqInfo = info;
            buffer.Write(resultCtrl.m_cbReqType);
            for (int i = 0; i < length; i++)
            {
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_nBankerControl);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbWinner);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbKingWinner);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbPlayerPair);
                buffer.Write(resultCtrl.m_szAdminReqInfo[i].m_cbBankerPair);
            }
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_AMDIN_COMMAND, buffer);
        }
    }
}
