using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Land
{
    //游戏消息
    public partial class Land_
    {
        #region 客户端发送命令
        public const ushort SUB_C_CALL_SCORE = 1;								//用户叫分
        public const ushort SUB_C_OUT_CARD = 2;									//用户出牌
        public const ushort SUB_C_PASS_CARD = 3;								//用户放弃
        public const ushort SUB_C_TRUSTEE = 4;									//用户托管
        public const ushort SUB_C_GAMESCENE = 5;
        #endregion

        #region 客户端结构体
        /// <summary>
        /// 用户叫分
        /// </summary>
        /// 
        public struct CMD_C_CallScore
        {
            public byte cbCallScore;                           //叫分
        };

        /// <summary>
        /// 用户加倍
        /// </summary>
        public struct CMD_C_AddTimes
        {
            public byte lScore;                    //加倍数目
        };

        /// <summary>
        /// 用户出牌
        /// </summary>
        /// 
        struct CMD_C_OutCard
        {
	        public byte	cbCardCount;				//出牌数目
            public byte[] cbCardData;				//扑克数据
        };


        /// <summary>
        /// 托管
        /// </summary>
        struct CMD_C_TRUSTEE
        {
            public byte bTrustee;				    //托管标志
        };

       #endregion

        /// <summary>
        /// 用户叫分
        /// </summary>
        /// <param name="YN"></param>
        public void NetSendCallScore(byte Score)
        {
            CMD_C_CallScore banke = new CMD_C_CallScore();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            banke.cbCallScore = Score;
            buffer.Write(banke.cbCallScore);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CALL_SCORE, buffer);
            Console.ReadLine();
        }
        public void SendGAME_SCENE()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GAMESCENE, buffer);
        }
        /// <summary>
        /// 用户出牌
        /// </summary>
        /// <param name="score"></param>
        public void NetSendOutCard(byte[] cbCardData)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            if(cbCardData == null ||cbCardData.Length == 0)
            {
                MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME,SUB_C_PASS_CARD, buffer);
            }
            else
            {
                if (cbCardData.Length > MAX_COUNT)
                    return;
                CMD_C_OutCard OutCards = new CMD_C_OutCard();
                OutCards.cbCardCount = (byte)(cbCardData.Length);
                buffer.Write(OutCards.cbCardCount);
                OutCards.cbCardData = new byte[MAX_COUNT];
                Array.Copy(cbCardData, OutCards.cbCardData, cbCardData.Length);
                buffer.Write(OutCards.cbCardData, cbCardData.Length);
                MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_OUT_CARD, buffer);
            }

        }
        /// <summary>
        /// 用户托管
        /// </summary>
        /// <param name="score"></param>
        public void NetSendTrustee(bool bIsTrustee)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            CMD_C_TRUSTEE Trustee = new CMD_C_TRUSTEE();
            Trustee.bTrustee = (byte)(bIsTrustee?1:0);
            buffer.Write(Trustee.bTrustee);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_TRUSTEE, buffer);
        }
    }
}
