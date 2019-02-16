using CYNetwork;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace JourneyToTheWest
{
    public partial class JourneyToTheWest_
    {
        #region 服务器回调命令
        //服务器命令结构

        public const ushort SUB_S_SCENE1_START  =       100;             // 滚动结果
        public const ushort SUB_S_SCENE2_RESULT =       101;             // 骰子结果
        public const ushort SUB_S_SCENE3_RESULT =       102;             // 玛丽结果

        public const ushort SUB_S_SCORE_FINISH = 104;                       //得分消息

        public const ushort SUB_S_CHECK_NETWORK = 251;

        public const ushort SUB_S_STOCK_RESULT = 103; //库存操作结果
        public const ushort SUB_S_GM_IsMaster = 110;               // 是不是管理员
        public const ushort SUB_S_GM_B_OR_WList = 111; //黑白名单
        public const ushort SUB_S_ABNORMAL = 112; //玩家非法押注
        public const ushort SUB_S_UPDATE_REVENUE = 140;     //奖池库存
        #endregion
        #region 服务器返回的结构体
        /// <summary>
        /// 游戏场景1开始
        /// </summary>
        public struct CMD_S_Scene1Start
        {
            public byte[][] result_icons;
            public long win_score;
        };

        /// <summary>
        /// 奖池库存
        /// </summary>
        public struct CMD_S_UpdateRevJackpot
        {
            public long MainStock;
            public long RobotStock;
        };

        /// <summary>
        /// 场景2
        /// </summary>
        public struct CMD_S_Scene2Result
        {
            public ushort dice_points;
            public long win_score;
        };

        public struct BonusResult
        {
            /// <summary>
            /// 中间开奖面板(4个结果)
            /// </summary>
            public ushort[] rolling_result_icons;
            /// <summary>
            /// 开奖结果
            /// </summary>
            public ushort rotate_result;
            /// <summary>
            /// 开奖得分
            /// </summary>
            public long win_score;
        }

        /// <summary>
        /// 场景3
        /// </summary>
        public struct CMD_S_Scene3Result
        {
            public long curBetScore;           //押注分数
            public long totalWinSoce;          //总得分
            public int resultNum;                  //结果数目
            public BonusResult[] result;		//每一轮的结果
        };


        struct CMD_S_GM_Master
        {
            public bool bIsMaster;
        };

        public struct CMD_S_ControlResult
        {
            public long stock;
            public long stock_revenue;
            public uint stock_revenue_per;

            //库存抽水率
            public long temp_revenue_value;
            //库存抽水仓库数值
            public long limit_temp_revenue_value;
            //库存抽水仓库存入库存的要求数值
            public uint temp_revenue_per;
            //机器人库存
            public long robot_revenue_value;

            public int cur_degree;
            public int all_degreenum;
            public int time_degreemin;
            public int time_degreemax;
            public int stock_degreemin;
            public int stock_degreemax;

            public uint mali1_max_result_times;
            public uint mali2_max_result_times;
            public uint fullhuman_max_result_times;
            public uint fullwuqi_max_result_times;

            public uint bounsgame_min_times;
            public uint bounsgame_max_times;
        };



        public struct CMD_S_GM_B_OR_WList
        {
            public uint[][] GameID;//2 128   黑 白
            public long[][] Scores;//2 128   黑 白
            public string[][] Nicks;//2 128   黑 白
        };

        public struct CMD_Score_Abnormal
        {
            public uint game_id;
        };
        #endregion


        //         private bool CheckGameData()
        //         {
        //             if (BaccaratInterFace != null)
        //             {
        //                 BaccaratInterFace.CheckGameNet();
        //             }
        //             return true;
        //         }

        private bool HandleGameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case SUB_S_SCENE1_START:
                    return HandleScene1StartMsg(maincmd, subCmd, readbuffer);
                case SUB_S_SCENE2_RESULT:
                    return HandleScene2ResultMsg(maincmd, subCmd, readbuffer);
                case SUB_S_SCENE3_RESULT:
                    return HandleScene3ResultMsg(maincmd, subCmd, readbuffer);
                case SUB_S_SCORE_FINISH:
                    return HandleScoreFinishMsg(maincmd, subCmd, readbuffer);
                case SUB_S_CHECK_NETWORK:
                    return HandleCheckNetMsg(maincmd, subCmd, readbuffer);

                case SUB_S_GM_IsMaster:
                    return HandleIsMasterMsg(maincmd, subCmd, readbuffer);
                case SUB_S_STOCK_RESULT:
                    return HandleGM_STOCK_RESULTMsg(maincmd, subCmd, readbuffer);
                case SUB_S_GM_B_OR_WList:
                    return HandleGM_B_OR_WListMsg(maincmd, subCmd, readbuffer);
                case SUB_S_ABNORMAL:
                    return HandleBet_ABNORMALMsg(maincmd, subCmd, readbuffer);
                case SUB_S_UPDATE_REVENUE:
                    return HandleUpdateRevenueMsg(maincmd, subCmd, readbuffer);
                default:
                    Error("斗地主 未能解析的游戏子命令：" + subCmd);
                    break;
            }
            return true;
        }

        #region 普通函数
        private bool HandleScene1StartMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_Scene1Start Start = new CMD_S_Scene1Start();

            Start.result_icons = new byte[3][];
            for (int i = 0; i < 3;i++ )
            {
                Start.result_icons[i] = new byte[5];
                for (int j = 0; j < 5;j++ )
                {
                    Start.result_icons[i][j] = readbuffer.ReadByte();
                }
            }
            Start.win_score = readbuffer.ReadLong();
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.Game_Scene1Start(Start);
                //BaccaratInterFace.GAME_Dic_PuKe(Dic);
            }
            return true;
        }
        private bool HandleScene2ResultMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_Scene2Result Scene2 = new CMD_S_Scene2Result();
            Scene2.dice_points = readbuffer.ReadUshort();
            Scene2.win_score = readbuffer.ReadLong();

            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.Game_Scene2Result(Scene2);
            }
            return true;
        }
        private bool HandleScene3ResultMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_Scene3Result msg = new CMD_S_Scene3Result();

            msg.curBetScore = rb.ReadLong();
            msg.totalWinSoce = rb.ReadLong();
            msg.resultNum = rb.ReadInt();
            msg.result = new BonusResult[msg.resultNum];
            for (var i = 0; i < msg.result.Length; i++)
            {
                BonusResult br = new BonusResult();
                br.rolling_result_icons = new ushort[4];
                for (byte j = 0; j < 4; j++)
                {
                    br.rolling_result_icons[j] = rb.ReadUshort();
                }
                br.rotate_result = rb.ReadUshort();
                br.win_score = rb.ReadLong();
                msg.result[i] = br;
                //Debug.Log(string.Format("开奖内容 [{0},{1},{2},{3}] {4} {5}", br.rolling_result_icons[0], br.rolling_result_icons[1], br.rolling_result_icons[2], br.rolling_result_icons[3],br.rotate_result, br.win_score));
            }

            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.Game_Scene3Result(msg);
            }
            return true;
        }
        private bool HandleScoreFinishMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            long nowScore = readbuffer.ReadLong();
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.Game_GetScore(nowScore);
            }
            return true;
        }


        public bool HandleCheckNetMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.Game_Check_Net();
            }
            return true;
        }
        #endregion

        public bool HandleIsMasterMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GM_Master master = new CMD_S_GM_Master();
            master.bIsMaster = readbuffer.ReadBoolean();
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.GM_IsMaster(master.bIsMaster);
            }
            return true;
        }

        public bool HandleGM_STOCK_RESULTMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ControlResult result = new CMD_S_ControlResult();
            result.stock = readbuffer.ReadLong();
            result.stock_revenue = readbuffer.ReadLong();
            result.stock_revenue_per = readbuffer.ReadUint();

            result.temp_revenue_value = readbuffer.ReadLong();
            result.limit_temp_revenue_value = readbuffer.ReadLong();
            result.temp_revenue_per = readbuffer.ReadUint();
            result.robot_revenue_value = readbuffer.ReadLong();



            result.cur_degree = readbuffer.ReadInt();
            result.all_degreenum = readbuffer.ReadInt();
            result.time_degreemin = readbuffer.ReadInt();
            result.time_degreemax = readbuffer.ReadInt();
            result.stock_degreemin = readbuffer.ReadInt();
            result.stock_degreemax = readbuffer.ReadInt();

            result.mali1_max_result_times = readbuffer.ReadUint();
            result.mali2_max_result_times = readbuffer.ReadUint();
            result.fullhuman_max_result_times = readbuffer.ReadUint();
            result.fullwuqi_max_result_times = readbuffer.ReadUint();

            result.bounsgame_min_times = readbuffer.ReadUint();
            result.bounsgame_max_times = readbuffer.ReadUint();

            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.GM_ControlResult(result);
            }
            return true;
        }

        public bool HandleGM_B_OR_WListMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GM_B_OR_WList list = new CMD_S_GM_B_OR_WList();
            list.GameID = new uint[2][];
            for (int i = 0; i < list.GameID.Length; i++)
            {
                list.GameID[i] = new uint[128];
                for (int j = 0; j < list.GameID[i].Length; j++)
                {
                    list.GameID[i][j] = readbuffer.ReadUint();
                }
            }
            list.Scores = new long[2][];
            for (int i = 0; i < list.Scores.Length; i++)
            {
                list.Scores[i] = new long[128];
                for (int j = 0; j < list.Scores[i].Length; j++)
                {
                    list.Scores[i][j] = readbuffer.ReadLong();
                }
            }
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.GM_GM_B_OR_WList(list);
            }
            return true;
        }

        public bool HandleBet_ABNORMALMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.Bet_ABNORMAL();
            }
            return true;
        }

        public bool HandleUpdateRevenueMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_UpdateRevJackpot msg = new CMD_S_UpdateRevJackpot();

            msg.MainStock = readbuffer.ReadLong();
            msg.RobotStock = readbuffer.ReadLong();

            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.UpdateRevenue(msg);
            }
            return true;
        }

    }
}
