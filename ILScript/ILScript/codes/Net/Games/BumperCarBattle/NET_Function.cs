using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BumperCarBattle
{
    //游戏消息
    public partial class BumperCarBattle_
    {
        #region 客户端发送命令

        public const ushort SUB_C_PLACE_JETTON			=    1;								//用户下注
        public const ushort SUB_C_CLEAR_JETTON			=    2;								//清除下注
        public const ushort SUB_C_APPLY_BANKER			=    3;								//申请庄家
        public const ushort SUB_C_CANCEL_BANKER			=    4;								//取消申请
        public const ushort SUB_C_CONTROL_GET_INFO		=	5;							//管理员获取系统信息
        public const ushort SUB_C_CONTROL_SET_PRIZE		=	6;							//管理员设置开奖结果
        public const ushort SUB_C_CONTROL_UPDATE_STORAGE = 7;							//更新库存
        #endregion

        #region 客户端结构体
        //用户下注
        public struct CMD_C_PlaceJetton
        {
            public byte cbJettonArea;						//筹码区域
            public long lJettonScore;						//加注数目
        };

        public struct CMD_C_CheckImage
        {
            public int Index;
        };

        public struct CMD_C_ApplyBanker
        {
            public ushort wOpUser;							//申请玩家
        };

        public struct CMD_C_CancelBanker
        {
            public string szCancelUser;					//取消玩家 [32] 64
            public int dwUserID;				        //取消玩家ID
        };

        //切换庄家
        public struct CMD_C_ChangeBanker
        {
            public ushort wBankerUser;						//当庄玩家
            public long lBankerScore;						//庄家分数
        };

        /*/// <summary>
        /// 下注失败
        /// </summary>
        public struct CMD_C_PlaceBetFail
        {

            public ushort wPlaceUser;							//下注玩家
            public byte lBetArea;							//下注区域
            public long lPlaceScore;						//当前下注
        };*/

        //用户控制指令
        public struct CMD_C_ControlApplication
        {
            public byte cbControlAppType;			//申请类型
            public byte cbControlArea;				//控制区域
            public byte cbControlTimes;			//控制次数
        };

        

       #endregion


        public void SendPlaceJetton(CMD_C_PlaceJetton msg)
        {
            CMD_C_PlaceJetton Jetton = new CMD_C_PlaceJetton();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            Jetton.cbJettonArea = msg.cbJettonArea;
            Jetton.lJettonScore = msg.lJettonScore;

            buffer.Write((int)Jetton.cbJettonArea);
            buffer.Write(Jetton.lJettonScore);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_PLACE_JETTON, buffer);

        }

        public void SendApplyBanker(ushort wOpUser)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            CMD_C_ApplyBanker banker = new CMD_C_ApplyBanker();
            banker.wOpUser = wOpUser;
            buffer.Write(banker.wOpUser);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_APPLY_BANKER, buffer);
        }

        public void SendCancelBanker(string CancelUser, ushort wOpUser)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            CMD_C_CancelBanker banker = new CMD_C_CancelBanker();
            banker.szCancelUser = CancelUser;
            banker.dwUserID = wOpUser;
            buffer.Write(banker.szCancelUser);
            buffer.Write(banker.dwUserID);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CANCEL_BANKER, buffer);
        }
    }
}
