using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeatsAndBirds
{
    //用户下注
    public struct CMD_C_PlaceJetton
    {
        public Byte cbJettonArea;                       //筹码区域
        public Int64 lJettonScore;                      //加注数目
    };

    /// <summary>
    /// 控制数据对象
    /// </summary>
    struct CMD_C_ControlApplication
    {
        public byte cbControlAppType;          //申请类型
        public byte[] cbControlArea;             //控制区域
        public byte cbControlTimes;            //控制次数
    };

    //库存控制
    public struct CMD_C_Update_Stock
    {
        public long lSystemStock;                      //系统库存
        public long lSystemRevenueValue;               //系统抽水数值
        public int iSystemRevenuePer;                  //系统抽水比例 千分比

        public long lTempRevenueValue;                 //库存抽水值
        public long lTempRevenueLimitValue;                //库存抽水阈值
        public int iTempRevenuePer;                    //库存抽水比例 千分比

        public long lRobotStock;                       //机器人库存
        public long lRobotStockInitial;                       //机器人库存起始值
        public long lRobotStockSection;                       //机器人库存增减区间

        public int iTempCode;                       //操作码		0：无操作   1：操作成功   2：操作失败
    };

    public partial class BeatsAndBirds_
    {
        const byte SUB_C_ADMIN_COMMDN_C_CA_UPDATE = 1;       //更新
        const byte SUB_C_ADMIN_COMMDN_C_CA_SET = 2;          //设置
        const byte SUB_C_ADMIN_COMMDN_C_CA_CANCELS = 3;      //取消

        public const int SUB_C_BATCH_JETTON = 9;        //批量下注
        public const int SUB_C_CANCEL_JETTON = 10;      //用户取消下注
        public const int SUB_C_PLACE_JETTON = 11;       //用户下注
        public const int SUB_C_APPLY_BANKER = 12;       //申请庄家
        public const int SUB_C_CANCEL_BANKER = 13;      //取消申请
        public const int SUB_C_ADMIN_COMMDN = 16;       //系统控制同步
        public const int SUB_C_ADMIN_STOCK = 17;        //库存控制
        public const int SUB_C_UPDATE_SETTING = 18;     //游戏配置更新

        /// <summary>
        /// 用户下注
        /// </summary>
        /// <param name="area"></param>
        /// <param name="score"></param>
        public void Bet(CMD_C_PlaceJetton cmd)
        {            
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.cbJettonArea);
            buffer.Write(cmd.lJettonScore);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_PLACE_JETTON, buffer);
        }

        /// <summary>
        /// 请求系统控制信息
        /// </summary>
        public void SyncAdmin()
        {
            CMD_C_ControlApplication cmd = new CMD_C_ControlApplication();
            cmd.cbControlAppType = SUB_C_ADMIN_COMMDN_C_CA_UPDATE;
            cmd.cbControlArea = new byte[]{ 0, 0 };

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.cbControlAppType);
            buffer.Write(cmd.cbControlArea);
            buffer.Write(cmd.cbControlTimes);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ADMIN_COMMDN, buffer);
        }

        /// <summary>
        /// 中奖区域控制
        /// </summary>
        public void AreaControl(byte[] area, byte times)
        {
            CMD_C_ControlApplication cmd = new CMD_C_ControlApplication();
            cmd.cbControlAppType = SUB_C_ADMIN_COMMDN_C_CA_SET;
            cmd.cbControlArea = area;
            cmd.cbControlTimes = times;

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.cbControlAppType);
            buffer.Write(cmd.cbControlArea[0]);
            buffer.Write(cmd.cbControlArea[1]);
            buffer.Write(cmd.cbControlTimes);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ADMIN_COMMDN, buffer);
        }

        /// <summary>
        /// 取消区域控制
        /// </summary>
        public void CancelAreaControl()
        {
            CMD_C_ControlApplication cmd = new CMD_C_ControlApplication();
            cmd.cbControlAppType = SUB_C_ADMIN_COMMDN_C_CA_CANCELS;
            cmd.cbControlArea = new byte[] { 0, 0 };

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.cbControlAppType);
            buffer.Write(cmd.cbControlArea);
            buffer.Write(cmd.cbControlTimes);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ADMIN_COMMDN, buffer);
        }

        /// <summary>
        /// 提交系统控制信息
        /// </summary>
        public void AdminStock(CMD_C_Update_Stock cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.lSystemStock);
            buffer.Write(cmd.lSystemRevenueValue);
            buffer.Write(cmd.iSystemRevenuePer);
            buffer.Write(cmd.lTempRevenueValue);
            buffer.Write(cmd.lTempRevenueLimitValue);
            buffer.Write(cmd.iTempRevenuePer);
            buffer.Write(cmd.lRobotStock);
            buffer.Write(cmd.lRobotStockInitial);
            buffer.Write(cmd.lRobotStockSection);
            buffer.Write(cmd.iTempCode);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ADMIN_STOCK, buffer);
        }

        /// <summary>
        /// 取消下注
        /// </summary>
        public void CancelJetton()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CANCEL_JETTON, buffer);
        }

        /// <summary>
        /// 批量下注
        /// </summary>
        /// <param name="cmd"></param>
        public void BatchPlaceJetton(BeatsAndBirds_.CMD_S_BatchPlaceJetton cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.wChairID);
            for (var i = 0; i < cmd.lJettonScore.Length; i++)
            {
                buffer.Write(cmd.lJettonScore[i]);
            }
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_BATCH_JETTON, buffer);
        }

        /// <summary>
        /// 取消下注
        /// </summary>
        public void UpdateSetting(CMD_C_UPDATE_SETTING cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.iAREA_1);
            buffer.Write(cmd.iAREA_2);
            buffer.Write(cmd.iAREA_3);
            buffer.Write(cmd.iAREA_4);
            buffer.Write(cmd.iAREA_5);
            buffer.Write(cmd.iAREA_6);
            buffer.Write(cmd.iAREA_7);
            buffer.Write(cmd.iAREA_8);
            buffer.Write(cmd.iAREA_9);
            buffer.Write(cmd.iAREA_10);
            buffer.Write(cmd.iAREA_11);
            buffer.Write(cmd.iAREA_12);
            buffer.Write(cmd.iGOLD_EX_PER1);
            buffer.Write(cmd.iGOLD_EX_PER2);
            buffer.Write(cmd.iGOLD_EX_PER3);
            buffer.Write(cmd.iGOLD_EX_PER4);
            buffer.Write(cmd.iTempCode);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_UPDATE_SETTING, buffer);
        }
    }
}
