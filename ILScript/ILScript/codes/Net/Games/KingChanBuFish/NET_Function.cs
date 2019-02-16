using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KCBF
{
    public enum BulletKind
    {
        BULLET_KIND_1_NORMAL = 0,
        BULLET_KIND_2_NORMAL,
        BULLET_KIND_3_NORMAL,
        BULLET_KIND_4_NORMAL,       //正常炮
        BULLET_KIND_1_ION,      //离子炮
        BULLET_KIND_2_ION,
        BULLET_KIND_3_ION,
        BULLET_KIND_4_ION,
        //真正的激光炮
        BULLET_KIND_LASER,
        BULLET_KIND_COUNT
    };
    //游戏消息
    public partial class KCBF_
    {
        #region 客户端发送命令
        public const ushort SUB_C_EXCHANGE_FISHSCORE = 1;//用户上分请求
        public const ushort SUB_C_USER_FIRE = 2;//用户开火请求
        public const ushort SUB_C_CATCH_FISH = 3;//用户打中范围鱼（炸弹，　红鱼类）
        public const ushort SUB_C_CATCH_SWEEP_FISH = 4;//用户发送范围鱼
        public const ushort SUB_C_HIT_FISH_I = 5;//击中 Fish_kind_21
        public const ushort SUB_C_STOCK_OPERATE = 6;//无用
        public const ushort SUB_C_USER_FILTER = 7;//无用
        public const ushort SUB_C_ANDROID_STAND_UP = 8;// 无用
        public const ushort SUB_C_FISH20_CONFIG = 9;// 无用
        public const ushort SUB_C_ANDROID_BULLET_MUL = 10;// 无用
        public const ushort SUB_C_REQUEST_NOW_INFO = 11;//请求当前界面信息， 包括是否在鱼阵
        public const ushort SUB_C_CHAT_MESSAGE = 12;//用户聊天， 已经无用， 
        public const ushort SUB_C_OPEN_ADMIN = 13;//打开管理界面, 请求当前 配置信息  
        public const ushort SUB_C_UPDATE_ADMIN_CONFIG = 14;//发送管理员修改后的配置信息给服务端
        #endregion

        #region 客户端结构体
        public struct CMD_C_ExchangeFishScore
        {
            public bool increase;
            public bool fast;
            //SCORE score;		//分数
        };

        public struct CMD_C_UserFire
        {
            public BulletKind bullet_kind;
            public float angle;
            public int bullet_mulriple;
            public int lock_fishid;
        };

        public struct CMD_C_CatchFish
        {
            public ushort chair_id;
            public int fish_id;
            public BulletKind bullet_kind;
            public int bullet_id;
            public int bullet_mulriple;
        };

        public struct CMD_C_CatchSweepFish
        {
            public ushort chair_id;
            public int fish_id;
            public int catch_fish_count;
            public int[] catch_fish_id;//[300]
        };

        public struct CMD_C_HitFishLK
        {
            public int fish_id;
        };
        #endregion
        public void SendExchangeFishScore(bool increase, bool fast)
        {
            CMD_C_ExchangeFishScore exchange_fishscore;
            exchange_fishscore.increase = increase;
            exchange_fishscore.fast = fast;
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(exchange_fishscore.increase);
            buffer.Write(exchange_fishscore.fast);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_EXCHANGE_FISHSCORE, buffer);
        }
        public void SendFire(BulletKind bullet_kind, float angle, int bullet_mulriple, int lock_fishid)
        {
            CMD_C_UserFire user_fire;
            user_fire.bullet_kind = bullet_kind;
            user_fire.angle = angle;
            user_fire.bullet_mulriple = bullet_mulriple;
            user_fire.lock_fishid = lock_fishid;
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write((int)user_fire.bullet_kind);
            buffer.Write(user_fire.angle);
            buffer.Write(user_fire.bullet_mulriple);
            buffer.Write(user_fire.lock_fishid);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_USER_FIRE, buffer);
        }
        public void SendCatchFish(int fish_id, ushort firer_chair_id, int bullet_id, BulletKind bullet_kind, int bullet_mulriple)
        {
            CMD_C_CatchFish catch_fish;
            catch_fish.chair_id = firer_chair_id;
            catch_fish.bullet_id = bullet_id;
            catch_fish.bullet_kind = bullet_kind;
            catch_fish.fish_id = fish_id;
            catch_fish.bullet_mulriple = bullet_mulriple;
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(catch_fish.chair_id);
            buffer.Write(catch_fish.fish_id);
            buffer.Write((int)catch_fish.bullet_kind);
            buffer.Write(catch_fish.bullet_id);
            buffer.Write(catch_fish.bullet_mulriple);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CATCH_FISH, buffer);
        }

        public void SendCatchSweepFish(ushort chair_id, int fish_id, int catch_fish_count, int[] catch_fish_id)
        {
            CMD_C_CatchSweepFish catch_Sweep_fish;
            catch_Sweep_fish.chair_id = chair_id;
            catch_Sweep_fish.fish_id = fish_id;
            catch_Sweep_fish.catch_fish_count = catch_fish_count;
            catch_Sweep_fish.catch_fish_id = catch_fish_id;

            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(catch_Sweep_fish.chair_id);
            buffer.Write(catch_Sweep_fish.fish_id);
            buffer.Write(catch_Sweep_fish.catch_fish_count);
            for (int i = 0; i < 300; i++)
                buffer.Write(catch_Sweep_fish.catch_fish_id[i]);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CATCH_SWEEP_FISH, buffer);
        }


        public void SendHitFishLK(ushort firer_chair_id, int fish_id)
        {
            CMD_C_HitFishLK hit_fish;
            hit_fish.fish_id = fish_id;
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(hit_fish.fish_id);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_HIT_FISH_I, buffer);
        }
        //发送请求去获取当前的鱼 信息， 比如是否在鱼阵， 初始化完成后请求一次
        public void SendRequestFishInfo()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_REQUEST_NOW_INFO, buffer);
        }

        //请求控制面板内容
        public void RequestAdminInfo()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_OPEN_ADMIN, buffer);
        }

        //保存控制面板内容
        public void SaveAdminInfo(CMD_S_ADMIN_CONFIG cfg)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();

            buffer.Write(cfg.head.msgid);
            buffer.Write(cfg.exchange_ratio_userscore);
            buffer.Write(cfg.exchange_ratio_fishscore);
            buffer.Write(cfg.exchange_count);
            buffer.Write(cfg.mode);
            buffer.Write(cfg.thresholdValue);
            buffer.Write(cfg.androidValue);
            buffer.Write(cfg.androidFireIntervalBase);
            buffer.Write(cfg.androidFireIntervalRand);
            buffer.Write(cfg.ionCannonMultiple);
            buffer.Write(cfg.ionCannonProbability);
            buffer.Write(cfg.ionCannonAwardMultiple);
            buffer.Write(cfg.ionCannonDuration);

            for (int i = 0; i < cfg.GameIdWhite.Length; i++)
            {
                buffer.Write(cfg.GameIdWhite[i]);
            }

            for (int i = 0; i < cfg.GameIdBlack.Length; i++)
            {
                buffer.Write(cfg.GameIdBlack[i]);
            }

            for (int i = 0; i < cfg.blackValue.Length; i++)
            {
                buffer.Write(cfg.blackValue[i]);
            }

            for (int i = 0; i < cfg.whiteValue.Length; i++)
            {
                buffer.Write(cfg.whiteValue[i]);
            }
            buffer.Write(cfg.blackValue1);
            buffer.Write(cfg.whiteValue1);
            buffer.Write(cfg.fishProbabilityMode);
            buffer.Write(cfg.fishProbailityKeyValue);
            buffer.Write(cfg.fishProbailityScale);
            buffer.Write(cfg.fishProbailityAndroidWelfare);

            for (int i = 0; i < cfg.bullet_multiple.Length; i++)
            {
                buffer.Write(cfg.bullet_multiple[i]);
            }
            buffer.Write(cfg.bullet_number);
            buffer.Write(cfg.defIndex);

            for (int i = 0; i < cfg.cannon_Size.Length; i++)
            {
                buffer.Write(cfg.cannon_Size[i]);
            }
            buffer.Write(cfg.bomb_range_width);
            buffer.Write(cfg.bomb_range_height);

            for (int i = 0; i < cfg.fish_min_multiple.Length; i++)
            {
                buffer.Write(cfg.fish_min_multiple[i]);
            }

            for (int i = 0; i < cfg.fish_max_multiple.Length; i++)
            {
                buffer.Write(cfg.fish_max_multiple[i]);
            }


            for (int i = 0; i < cfg.fish_probability.Length; i++)
            {
                buffer.Write(cfg.fish_probability[i]);
            }


            for (int i = 0; i < cfg.fish_speed.Length; i++)
            {
                buffer.Write(cfg.fish_speed[i]);
            }


            for (int i = 0; i < cfg.bullet_speed.Length; i++)
            {
                buffer.Write(cfg.bullet_speed[i]);
            }


            buffer.Write(cfg.lockFishCertainlyHit);
            buffer.Write(cfg.bulletReboundTimes);
            buffer.Write(cfg.fireInterval);
            buffer.Write(cfg.exchangeInterval);
            buffer.Write(cfg.speedMul);
            buffer.Write(cfg.laserMul);
            buffer.Write(cfg.laserReboundTimes);            

            for (int i = 0; i < cfg.stockScore.Length; i++)
            {
                buffer.Write(cfg.stockScore[i]);
            }

            for (int i = 0; i < cfg.stockIncreaseProbability.Length; i++)
            {
                buffer.Write(cfg.stockIncreaseProbability[i]);
            }

            buffer.Write(cfg.stockCount);

            buffer.Write(cfg.stockValue);
            buffer.Write(cfg.nowStockValue);

            buffer.Write(cfg.tempRevenueValue);
            buffer.Write(cfg.tempRevenueLimitValue);
            buffer.Write(cfg.tempRevenuePer);

            buffer.Write(cfg.tax);
            buffer.Write(cfg.nowTaxValue);


            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_UPDATE_ADMIN_CONFIG, buffer);
        }

    }
}
