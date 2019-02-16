using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GOLDS
{
    //游戏消息
    public partial class GOLDS_
    {
        #region 客户端发送命令

        //客户端命令结构

        public const ushort SUB_C_PLACE_JETTON		=11;								//用户下注
        public const ushort SUB_C_APPLY_BANKER		=12;								//申请庄家
        public const ushort SUB_C_CANCEL_BANKER		=13;								//取消申请
        public const ushort SUB_C_ADMIN_COMMDN		=16;								//系统控制
        public const ushort SUB_C_UPDATE_STORAGE	=17;								//更新库存
        #endregion

        #region 客户端结构体
        //用户下注
        public struct CMD_C_PlaceJetton
        {
            public byte cbJettonArea;						//筹码区域
            public long lJettonScore;						//加注数目

        };
        public struct CMD_S_ControlReturns
        {
            public byte cbReturnsType;				//回复类型
            public byte cbControlArea;				//控制区域
            public byte cbControlTimes;			//控制次数
        };

        public struct CMD_C_ControlApplication
        {
            public byte cbControlAppType;			//申请类型
            public byte cbControlArea;				//控制区域
            public byte cbControlTimes;			//控制次数
        };

        //更新库存
        public struct CMD_C_UpdateStorage
        {
            public byte cbReqType;						//请求类型
            public long lStorage;						//新库存值
            public long lStorageDeduct;					//库存衰减
        };

       #endregion


        public void SendPlaceJetton(CMD_C_PlaceJetton msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            buffer.Write(msg.cbJettonArea);
            buffer.Write(msg.lJettonScore);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_PLACE_JETTON, buffer);

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
            buffer.Write(banker.szCancelUser);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CANCEL_BANKER, buffer);
        }

        public void SendADMIN_Control(CMD_C_ControlApplication msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            buffer.Write(msg.cbControlAppType);
            buffer.Write(msg.cbControlArea);
            buffer.Write(msg.cbControlTimes);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ADMIN_COMMDN, buffer); //待定
        }

        public void SendUPDATE_STORAGE(CMD_C_UpdateStorage msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            buffer.Write(msg.cbReqType);
            buffer.Write(msg.lStorage);
            buffer.Write(msg.lStorageDeduct);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_UPDATE_STORAGE, buffer); //待定
        }
        
    }
}
