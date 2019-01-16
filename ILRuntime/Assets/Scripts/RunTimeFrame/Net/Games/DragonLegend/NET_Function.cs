using CYNetwork;
using MessagePackage;
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
        #region 客户端发送命令
        //客户端命令结构
        public const ushort SUB_C_EXCHANGE_CHIP = 101;                          //兑换筹码
        public const ushort SUB_C_PLAY_BET = 102;                           //下注消息
        public const ushort SUB_C_BET_CLEAR = 103;                            //清除下注
        public const ushort SUB_C_RENEWAL = 104;                            //续压操作

        public const ushort SUB_C_AMDIN_COMMAND = 4;  //管理员控制
        public const ushort SUB_C_GM_IsMaster = 16;               // 请求是不是管理员
        #endregion
        #region 客户端结构体

        //兑换筹码
        public struct CMD_C_Chip
        {
            public long lChip;                             //筹码数量
        };

        //下注消息
         public struct CMD_C_PlayBet
        {
            public byte nAnimalIndex;                       //下注动物
            public long lBetChip;                          //筹码数量
        };

        //续压消息
         public struct CMD_C_PlayRenewal
        {
            public long[]lRenewalBet;           //续压筹码数量
        };

         //清除下注
        public struct CMD_C_BetClear
        {
        };

        public enum ReqType 
        {
            RQ_SET_WIN_AREA = 1,
            RQ_RESET_CONTROL,
            RQ_PRINT_SYN,
            RQ_PRINT_ALL,
            RQ_SET_ALL,
            RQ_SET_STORAGE,
            RQ_SET_RATE		
        }

        public struct CMD_C_AdminReq
        {
	        public byte cbReqType;
          /*#define RQ_SET_WIN_AREA  	1
            #define RQ_RESET_CONTROL	2
            #define RQ_PRINT_SYN		3
            #define RQ_PRINT_ALL		4
            #define RQ_SET_ALL			5
            #define RQ_SET_STORAGE		6
            #define RQ_SET_RATE			7*/
            public byte[] cbExtendData;		//附加数据 64
        };

        #endregion

        //兑换筹码
        public void NetExchangeChip(byte Chip)
        {
            CMD_C_Chip ExchangeChip = new CMD_C_Chip();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            ExchangeChip.lChip = Chip;
            buffer.Write(ExchangeChip.lChip);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_EXCHANGE_CHIP, buffer);
            Console.ReadLine();
        }

        /// <summary>
        /// 用户下注
        /// </summary>
        /// <param name="score"></param>
        public void NetPlayBet(byte nAnimalIndex,long score)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            CMD_C_PlayBet JiaZhu = new CMD_C_PlayBet();

            JiaZhu.nAnimalIndex = nAnimalIndex;
            buffer.Write(JiaZhu.nAnimalIndex);
            JiaZhu.lBetChip = score;
            buffer.Write(JiaZhu.lBetChip);            
            
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_PLAY_BET, buffer);
        }

        /// <summary>
        /// 用户续压
        /// </summary>
        /// <param name="score"></param>
        public void NetOperationRenewal(long[] cbRenewalData)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            CMD_C_PlayRenewal Renewal = new CMD_C_PlayRenewal();

            Renewal.lRenewalBet = new long[ANIMAL_MAX];//Animal_type.ANIMAL_MAX         

            Array.Copy(cbRenewalData, Renewal.lRenewalBet, cbRenewalData.Length);
        
            for(int i=0;i< cbRenewalData.Length; i++)
             buffer.Write(Renewal.lRenewalBet[i]);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_RENEWAL, buffer);
        }

        //清除下注
        public void NetBetClearChip()
        {
            CMD_C_BetClear BetClear = new CMD_C_BetClear();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
       
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_BET_CLEAR, buffer);
            Console.ReadLine();
        }
        public bool GetIsConnet()
        {
            return MessageMgr.CurMsgMgr.GameSock.IsConnected;
        }

        public void NetGMReq(DragonLegend_.CMD_C_AdminReq req)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(req.cbReqType);
            buffer.Write(req.cbExtendData);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_AMDIN_COMMAND, buffer);
            Console.ReadLine();
        }

        public void GMSendIsMaster()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GM_IsMaster, buffer);
        }
        /////////////////////////////////////////////////////////////////////////////////////
    }
}
