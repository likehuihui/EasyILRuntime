using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShuiHuZhuan
{
    //游戏消息
    public partial class ShuiHuZhuan_
    {


        #region 客户端发送命令
        public const ushort SUB_C_ADD_CREDIT_SCORE         =     1;               // 上分
        public const ushort SUB_C_REDUCE_CREDIT_SCORE      =     2;               // 下分
        public const ushort SUB_C_SCENE1_START             =     3;               // 开始场景1
        public const ushort SUB_C_SCENE2_BUY_TYPE          =     4;               // 买大小
        public const ushort SUB_C_SCORE                    =     5;               // 得分
        public const ushort SUB_C_SCENE3_START             =     6;               // 玛丽
        public const ushort SUB_C_CHECK_NETWORK = 250;  //网络监测

        public const ushort SUB_C_STOCK_OPERATE = 8; //库存操作
        public const ushort SUB_C_ADMIN_CONTROL = 9; //大奖及黑白名单
        public const ushort SUB_C_ADMIN_CONTROL_MAX_RESULT = 13;            //全屏最大倍数
        public const ushort SUB_C_DEGREE_OPERATE = 14; //难度操作
        public const ushort SUB_C_GM_IsMaster = 16;               // 请求是不是管理员
        public const ushort SUB_C_GM_B_OR_WList = 17; //请求黑白名单
        #endregion

        #region 客户端结构体

        struct CMD_C_Scene1Start {
          public long total_bet_score;
        };

        struct CMD_C_Scene2BuyType {
          public byte double_type;
          public byte buy_type;
          public long total_bet_score;
        };

        struct CMD_C_Scene3Start {
          public int bonus_game_times;
          public long total_bet_score;
        };

        public struct CMD_C_StockOperate
        {
            public byte operate_code;  // 0查询 1 增加 2 减少 3 清除 
            public uint perRevenue;

            //库存抽水率
            public long temp_revenue_value;
            //库存抽水仓库数值
            public long limit_temp_revenue_value;
            //库存抽水仓库存入库存的要求数值
            public uint temp_revenue_per;
            //机器人库存
            public long robot_revenue_value;
        };

        public struct CMD_C_DegreeOperate
        {
            public int curDegree;
            public int degreeTimeMin;
            public int degreeTimeMax;
            public int degreeStockMin;
            public int degreeStockMax;
        };

        public struct CMD_C_AdminControl_Max_Result
        {
            public uint mali1_max_result_times;
            public uint mali2_max_result_times;
            public uint fullhuman_max_result_times;
            public uint fullwuqi_max_result_times;
            public uint bounsgame_min_times;
            public uint bounsgame_max_times;
        };

        public struct CMD_C_AdminControl {
          public byte type;         // 0大奖 1白名单 2黑名单 3无
          public uint game_id;
          public AdminCtrlType icon;
          public long limit_score;
        };

        public enum AdminCtrlType
        {
            AC_FUTOU = 0,     // 斧头
            AC_YINGQIANG,     // 银枪
            AC_DADAO,         // 大刀
            AC_LU,            // 鲁智深
            AC_LIN,           // 林冲
            AC_SONG,          // 宋江
            AC_TITIANXINGDAO, // 替天行道
            AC_ZHONGYITANG,   // 忠义堂
            AC_SHUIHUZHUAN,   // 水浒传
            AC_ALLHUMAN,      //全屏人物
            AC_ALLWUQI,       //全屏武器
            AC_MALI_1,         // 1次小泽玛利亚
            AC_MALI_2,         // 1次小泽玛利亚
            AC_MALI_3,         // 1次小泽玛利亚
            AC_COUNT
        };

        public struct CMD_GM_DelB_OR_WItem
        {
          public uint game_id;
        };
        

       #endregion

        #region 普通函数
        /// <summary>
        /// 加注
        /// </summary>
        public void NetSendAddCredit(long total_bet_score)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ADD_CREDIT_SCORE, buffer);
        }
        /// <summary>
        /// 减注
        /// </summary>
        public void NetSendReduceCredit(long total_bet_score)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_REDUCE_CREDIT_SCORE, buffer);
        }
        /// <summary>
        /// 场景1发送开始
        /// </summary>
        public void NetSendScene1Start(long total_bet_score)
        {
            CMD_C_Scene1Start Scene1 = new CMD_C_Scene1Start();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            Scene1.total_bet_score = total_bet_score;
            buffer.Write(Scene1.total_bet_score);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_SCENE1_START, buffer);
        }

        public bool GetIsConnet()
        {
            if(null == MessageMgr.CurMsgMgr || null == MessageMgr.CurMsgMgr.GameSock)
            {
                return false;
            }
            return MessageMgr.CurMsgMgr.GameSock.IsConnected;
        }

        /// <summary>
        /// 得分
        /// </summary>
        public void NetSendGetScore()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_SCORE, buffer);
        }
        /// <summary>
        /// 比倍
        /// </summary>
        public void NetSendScene2BuyType(byte double_type_,byte bt,long total_bet_score_)
        {
            CMD_C_Scene2BuyType scene2_buy_type = new CMD_C_Scene2BuyType();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            scene2_buy_type.double_type = double_type_;
            scene2_buy_type.buy_type = bt;
            scene2_buy_type.total_bet_score = total_bet_score_;
            buffer.Write(scene2_buy_type.double_type);
            buffer.Write(scene2_buy_type.buy_type);
            buffer.Write(scene2_buy_type.total_bet_score);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_SCENE2_BUY_TYPE, buffer);
        }
        /// <summary>
        /// 玛丽
        /// </summary>
        public void NetSendScene3Start(int bonus_game_times, long total_bet_score)
        {
            CMD_C_Scene3Start Scene3 = new CMD_C_Scene3Start();
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            Scene3.bonus_game_times = bonus_game_times;
            Scene3.total_bet_score = total_bet_score;
            buffer.Write(Scene3.bonus_game_times);
            buffer.Write(Scene3.total_bet_score);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_SCENE3_START, buffer);
        }

        public void SendCheck_NetWork()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CHECK_NETWORK, buffer);
        }

        #endregion
        public void GMSendIsMaster()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GM_IsMaster, buffer);
        }

        public void GMSendStockOperate(ShuiHuZhuan_.CMD_C_StockOperate msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msg.operate_code);
            buffer.Write(msg.perRevenue);
            buffer.Write(msg.temp_revenue_per);
            buffer.Write(msg.temp_revenue_value);
            buffer.Write(msg.limit_temp_revenue_value);
            buffer.Write(msg.robot_revenue_value);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_STOCK_OPERATE, buffer);
        }

        public void GMSendADMIN_CONTROL(ShuiHuZhuan_.CMD_C_AdminControl msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msg.type);
            buffer.Write(msg.game_id);
            buffer.Write((int)msg.icon);
            buffer.Write(msg.limit_score);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ADMIN_CONTROL, buffer);
        }

        public void GMSendAdminControl_Max(ShuiHuZhuan_.CMD_C_AdminControl_Max_Result msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msg.mali1_max_result_times);
            buffer.Write(msg.mali2_max_result_times);
            buffer.Write(msg.fullhuman_max_result_times);
            buffer.Write(msg.fullwuqi_max_result_times);
            buffer.Write(msg.bounsgame_min_times);
            buffer.Write(msg.bounsgame_max_times);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ADMIN_CONTROL_MAX_RESULT, buffer);
        }

        public void GMSendDegreeOperate(ShuiHuZhuan_.CMD_C_DegreeOperate msg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(msg.curDegree);
            buffer.Write(msg.degreeTimeMin);
            buffer.Write(msg.degreeTimeMax);
            buffer.Write(msg.degreeStockMin);
            buffer.Write(msg.degreeStockMax);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_DEGREE_OPERATE, buffer);
        }

        public void GMSendReqB_OR_WList(uint gameID=0)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(gameID);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GM_B_OR_WList,buffer);
        }
    }
}
