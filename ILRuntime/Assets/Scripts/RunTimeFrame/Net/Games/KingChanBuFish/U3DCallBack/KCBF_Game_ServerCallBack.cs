using CYNetwork;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KCBF
{
    public partial class KCBF_
    {
        #region 服务器回调命令

        public enum SceneKind
        {
            SCENE_KIND_1 = 0,
            SCENE_KIND_2,
            SCENE_KIND_3,
            SCENE_KIND_4,
            SCENE_KIND_5,
            SCENE_KIND_6,
            SCENE_KIND_7,
            SCENE_KIND_8,
            SCENE_KIND_9,
            SCENE_KIND_COUNT
        };

        public enum FishKind
        {
            FISH_KIND_1 = 0,
            FISH_KIND_2,
            FISH_KIND_3,
            FISH_KIND_4,
            FISH_KIND_5,
            FISH_KIND_6,
            FISH_KIND_7,
            FISH_KIND_8,
            FISH_KIND_9,
            FISH_KIND_10,
            FISH_KIND_11,
            FISH_KIND_12,
            FISH_KIND_13,
            FISH_KIND_14,
            FISH_KIND_15,
            FISH_KIND_16,
            FISH_KIND_17,
            FISH_KIND_18,
            FISH_KIND_19,
            FISH_KIND_20, // 企鹅
            FISH_KIND_21, // 李逵
            FISH_KIND_22, // 定屏炸弹
            FISH_KIND_23, // 局部炸弹
            FISH_KIND_24, // 超级炸弹
            FISH_KIND_25, // 大三元1
            FISH_KIND_26, // 大三元2
            FISH_KIND_27, // 大三元3
            FISH_KIND_28, // 大四喜1
            FISH_KIND_29, // 大四喜2
            FISH_KIND_30, // 大四喜3
            FISH_KIND_31, // 鱼王1
            FISH_KIND_32, // 鱼王2
            FISH_KIND_33, // 鱼王3
            FISH_KIND_34, // 鱼王4
            FISH_KIND_35, // 鱼王5
            FISH_KIND_36, // 鱼王6
            FISH_KIND_37, // 鱼王7
            FISH_KIND_38, // 鱼王8
            FISH_KIND_39, // 鱼王9
            FISH_KIND_40, // 鱼王10
            FISH_KIND_41, //特殊鱼， 一段时间后会死掉
            FISH_KIND_42, //特殊大鱼
            FISH_KIND_43,   //小鱼
            FISH_KIND_44,//小鱼
            FISH_KIND_45,//小鱼
            FISH_KIND_46,       //一箭双雕
            FISH_KIND_47,       //一箭三雕
            FISH_KIND_48,       //一箭五雕
            FISH_KIND_49,       //蓝泡
            FISH_KIND_50,       //绿泡
            FISH_KIND_51,       //补充的大鱼
            FISH_KIND_52,       //大金鱼
            FISH_KIND_53,       //凤凰
            FISH_KIND_COUNT
        };


        //服务器命令结构
        public const ushort SUB_S_GAME_CONFIG = 100;//客户端启动时， 服务端发给客户端的配置文件， 包含一些鱼的游动速度 以及相当配置
        public const ushort SUB_S_FISH_TRACE = 101;//服务端发送鱼的游动路径给客户端
        public const ushort SUB_S_EXCHANGE_FISHSCORE = 102;//如果游戏需要上分时， 用来支持上分的
        public const ushort SUB_S_USER_FIRE = 103;//服务端广播客户端的开火信息（包括给该玩家）
        public const ushort SUB_S_CATCH_FISH = 104;//当判定玩家捕获到鱼时， 服务端广播下去的（包括给该玩家）
        public const ushort SUB_S_BULLET_ION_TIMEOUT = 105;//离子子炮时间到了
        public const ushort SUB_S_LOCK_TIMEOUT = 106;//定 屏 时间到了
        public const ushort SUB_S_CATCH_SWEEP_FISH = 107;//打中炸弹类型的鱼， 局部 全局，鱼王红鱼等， 发下去让客户端 统计到底打中多少鱼
        public const ushort SUB_S_CATCH_SWEEP_FISH_RESULT = 108;//广播打中炸弹类型鱼的结果， 即到底打中了哪些鱼
        public const ushort SUB_S_HIT_FISH_LK = 109;//广播击中了FISH_KIND_21号鱼， 客户端就会将该鱼显示的倍数+1
        public const ushort SUB_S_SWITCH_SCENE = 110;//换场景, 会先来一个鱼阵
        public const ushort SUB_S_STOCK_OPERATE_RESULT = 111;//已经无效
        public const ushort SUB_S_SCENE_END = 112;//发送场景结束的消息， 客户端目前无处理
        public const ushort SUB_S_REPLY_NOW_INFO = 113;//回复当前界面鱼阵信息， 用来给后入的玩家， 恢复在场的玩家看到的鱼阵信息
        public const ushort SUB_S_CIRCLE_FISH_INFO = 114;//发送圈鱼信息
        public const ushort SUB_S_SPECIAL_FISH_INFO = 115;//发送特殊鱼的倒计时信息， 即 FISH_KIND_41 号
        public const ushort SUB_S_MESSAGE_FOR_CATCH_FISH = 116;//当玩家打中高倍数鱼时， 服务端会发送广播
        public const ushort SUB_S_SEND_ADMIN_CONFIG = 117;//向客户端发送当前配置信息
        public const ushort SUB_S_SAVE_ADMIN_CONFIG_RESULT = 118;//向客户端发送保存当前配置结果的消息  bool值
        public const ushort SUB_S_REPLY_NOW_FISH_INFO = 123;    //返回当前界面鱼信息，是想实现玩家一进入游戏就能有鱼， 即还原出当前鱼所在的位置， 后因会给程序带来不稳定， 且后台保存数据量有总增加， 且前台计算量也增加， 但收效极小取消
        #endregion

        public const int BULLET_KIND_COUNT = 9;
        public const int FISH_KIND_COUNT = 53;
        #region 服务器返回的结构体
        public struct CMD_S_GameConfig
        {
            public int exchange_ratio_userscore;
            public int exchange_ratio_fishscore;
            public int exchange_count;
            public int mode;               //mode=1表示需要上分， 0表示不需要上分

            public int min_bullet_multiple;
            public int max_bullet_multiple;
            //add by lucas (config package)
            public int def_bullet_multiple;
            public int[] bullet_multiple;//[20]
            public int bullet_number;
            public int def_canSize;
            public int[] cannon_Size;//[20]
                                     //add end;

            public int bomb_range_width;
            public int bomb_range_height;

            public double[] fish_multiple;//[FishKind.FISH_KIND_COUNT];
            public int[] fish_speed;//[FISH_KIND_COUNT];
            public int[] fish_bounding_box_width;//[FISH_KIND_COUNT];
            public int[] fish_bounding_box_height;//[FISH_KIND_COUNT];
            public int[] fish_hit_radius;///[FISH_KIND_COUNT];

            public int[] bullet_speed;//[BULLET_KIND_COUNT];
            public int[] net_radius;//[BULLET_KIND_COUNT];
            public int[] bullet_bounding_box_width_;//[BULLET_KIND_COUNT];  //对应下标编号的子弹的宽度
            public int[] bullet_bounding_box_height_;//[BULLET_KIND_COUNT]; //对应下标编号的子弹的高度
            public bool lockFishCertainlyHit;                          //锁定的鱼是不是必定打得到

            public int bulletReboundTimes;                                 //子弹回弹次数
            public ushort fireInterval;                            //开火间隔时间
            public ushort exchangeInterval;                        //上分间隔时间
            public float speedMul;                                         //速度倍数
            public int laserMul;                                           //设置需要打几炮满mul子弹  才能获得一个激光炮， -1表示永远不可能， 即关闭激光炮玩法
            public int laserReboundTimes;                                  //设置激光子弹可以反弹的次数
        };

        public enum TraceType
        {
            TRACE_LINEAR = 0,
            TRACE_BEZIER,
            TRACE_PARABOLA          //抛物线的行走方式
        };



        public struct FPoint
        {
            public float x;
            public float y;
        };

        public struct IPoint
        {
            public int x;
            public int y;
        };

        public struct FPointAngle
        {
            public float x;
            public float y;
            public float angle;
        };

        public struct IPointAngle
        {
            public int x;
            public int y;
            public float angle;
        };

        public struct CMD_S_FishTrace
        {
            public FPoint[] init_pos;//[10];
            public int init_count;
            public FishKind fish_kind;
            public int fish_id;
            public TraceType trace_type;
            public byte[] fishKindInfo;//[5];   //保存一箭多雕时的鱼类信息
            public bool redFish;           //标志是否为红鱼
        };
        public struct CMD_S_ExchangeFishScore
        {
            public ushort chair_id;
            public long swap_fish_score;
            public long exchange_fish_score;
            public long[] uesr_score;//[5]
        };

        public struct CMD_S_UserFire
        {
            public BulletKind bullet_kind;
            public int bullet_id;
            public ushort chair_id;
            public float angle;
            public int bullet_mulriple;
            public int lock_fishid;
            public bool android;
            public int laserValue; //激光炮累积值， -1表示获得一个激光炮
        };
        public struct CMD_S_CatchFish
        {
            public ushort chair_id;
            public int fish_id;
            public FishKind fish_kind;         //判断是否需要做效果
            public bool bullet_ion;            //是否有附加状态， 导致离子炮的产生
            public long fish_score;
        };

        public struct CMD_S_BulletIonTimeout
        {
            public ushort chair_id;
        };
        public struct CMD_S_CatchSweepFish
        {
            public ushort chair_id;
            public int fish_id;
            public bool android;//是否是机器人的？
        };
        public struct CMD_S_CatchSweepFishResult
        {
            public ushort chair_id;
            public int fish_id;
            public long fish_score;
            public int catch_fish_count;
            public int[] catch_fish_id;//[300]
            public int bulletMul;  //子弹倍数
        };
        public struct CMD_S_HitFishLK
        {
            //WORD chair_id;
            public int fish_id;
            public int fish_mulriple;
        };
        public struct CMD_S_SwitchScene//鱼阵信息
        {
            public SceneKind scene_kind;
            public int fish_count;
            public FishKind[] fish_kind;//[300];    //鱼的类型
            public int[] fish_id;//[300];   //鱼的id
            public bool[] redFish;//[300];  //是否为红鱼
        };

        //当前的鱼阵信息
        public struct CMD_S_FishArrayInfo
        {
            public bool isFishArray;                   //是否处在鱼阵中
            public SceneKind sceneKind;                //哪种鱼阵
            public CMD_S_SwitchScene initSwitchScene;  //都有哪些鱼
            public int[] dieFishId;//[300];                 //死掉的鱼的id
            public int dieFishCount;                   //死掉鱼的数量
            public uint passTime;                 //已经过去的时间
        };

        //一圈鱼的信息
        public struct CMD_S_CircleFishArrayInfo
        {
            public FPoint circlePoint;         //圆心点
            public int fishNum;                //一共几条鱼
            public int[] fishId;//[30];         //鱼的id
            public FishKind fishKind;  //鱼的类型
            public int redFishId;              //-1表示没有红鱼， 其它表示红鱼的fishId
        };
        //特殊鱼信息
        public struct CMD_S_SpecialInfo
        {
            public int fishId;
            public int countdown;//倒计时
        };
        public struct CMD_S_MessageForCatchFish
        {
            public ushort roomId;            //房间，
            public ushort tableId;           //桌子
            public ushort chairId;           //椅子
            public FishKind fishKind;      // 鱼类型
            public long score;            //分数
            public int bulletMul;          //子弹倍数
            public float fishMul;          //鱼的倍数
            public BulletKind bulletKind;  //子弹类型
            public string playName;//[64]   //玩家名字
        };

        public struct MSGHEAD
        {
            public int msgid;
        };

        public struct CMD_S_ADMIN_CONFIG
        {
            public MSGHEAD head;                                       //SUB_PC_SAVEINI
            public int exchange_ratio_userscore;                   //表示金币跟鱼币的兑换比值
            public int exchange_ratio_fishscore;                   //表示金币跟鱼币的兑换比值
            public int exchange_count;                             //表示一次上分能上多少
            public int mode;                                       //mode=1表示需要上分， 0表示不需要上分
            public int thresholdValue;                             //阀值该值是指元宝） 当 用户输掉或者赢到的钱大于此值便会进行存盘， 该值的设定，最好根据房间的倍数来， 太小了过于频繁读取数据库会造成压力， 太大了则起不到应有的作用， 如果没设定， 默认100000  ， 读取类型为int
            public float androidValue;                             // <!-- 机器人打中概率乘上该值 -->
            public int androidFireIntervalBase;                    //<!-- 机器人开火间隔时间 至少间隔base毫秒，
            public int androidFireIntervalRand;                    //随机范围在rand毫秒之间-->
            public float ionCannonMultiple;                            ////获得离子炮的前提, 需要打中多少倍数以上的鱼
            public float ionCannonProbability;                         //在前提条件满足情况下， 有多少概率会获得
            public float ionCannonAwardMultiple;                       //获得离子炮后，  打中鱼后奖励翻多少倍
            public int ionCannonDuration;                          //离子炮的持续时间， 单位ms
            public uint[] GameIdWhite;//[NUMBER];64                      //受控游戏ID，静态变量， 同一房间内， 所有的桌子共享
            public uint[] GameIdBlack;//[NUMBER];64                      //受控游戏ID，静态变量， 同一房间内， 所有的桌子共享
            public float[] blackValue;//[NUMBER];64                           //<!-- 黑名单的人， 打中鱼的概率乘上该值-->
            public float[] whiteValue;//[NUMBER];64                           //<!-- 白名单的人， 打中鱼的概率乘上该值-->
            public float blackValue1;                              //<!-- 黑名单的人， 打中鱼的概率乘上该值-->
            public float whiteValue1;                              //<!-- 白名单的人， 打中鱼的概率乘上该值-->
            public int stockValue;                                 //库存初始值
            public long nowStockValue;                             //当前库存值
            public int[] stockScore;//[10];                             //库存分数阀值, 大于该值打中鱼的概率提升相应值
            public float[] stockIncreaseProbability;//[10];             //提升的值
            public int stockCount;                                 //配置数量， 至少要有  库存为0时的配置

            #region 2017/11/16 by Jing
            public long tempRevenueValue;                           //库存抽水值
            public long tempRevenueLimitValue;                      //库存抽水阈值
            public float tempRevenuePer;                            //库存抽水比例
            #endregion

            //<!-- 打鱼概率计算方式  如果mode为auto, 则表示全自动计算概率(计算方式为所有鱼  （打中概率 == keyValue/打中的倍数）建议keyValue为0.95到0.99之间	-->
            //	<!-- 打鱼概率计算方式  如果mode为semiauto, 则表示半自动计算概率(计算方式为范围鱼(炸弹， 红鱼， 一箭几箭之类的)  （打中概率 == keyValue/打中的倍数）建议keyValue为0.95到0.99之间	-->
            //	<!-- 打鱼概率计算方式  如果mode为manual, 则表示手动计算概率，打中鱼的概率依据配置文件来-->
            //	<!-- 打鱼概率计算方式  如果mode为autolow, 则表示全自动计算概率(计算方式为所有鱼  （打中概率 == keyValue/打中的倍数）, 如果计算出来的概率比配置的还高时， 则取配置文件的， 建议keyValue为0.95到0.99之间-->
            //	<!-- scale  表示计算出来的概率再乘上该值 ， 本套概率计算 方式计算后 ，并非最终概率， 会受黑白名单， 库存影响 -->
            //	<!-- androidWelfare  表示机器人福利 ， 表示机器人打中范围鱼时计算出来的概率再乘上该值（这样会多触发些特效， 界面会炫丽一点） -->
            //	<!-- 如果设置有错， 则默认为auto， keyValue为0.95， scale为1.0 androidWelfare=2.0 -->
            public byte[] fishProbabilityMode;//[15];
            public float fishProbailityKeyValue;
            public float fishProbailityScale;
            public float fishProbailityAndroidWelfare;
            public int[] bullet_multiple;//[20];                        //子弹倍数选项值（比如有10倍炮， 20倍数）
            public int bullet_number;                              //共有多少个子弹倍数选项
            public int defIndex;                                   //表示 bullet_multiple及cannon_Size数组的下标，  默认炮筒及倍数
            public int[] cannon_Size;//[20];                            //子弹倍数对应的炮筒大小
            public int bomb_range_width;                           //局部炸弹  炸弹的范围宽度
            public int bomb_range_height;                          //局部炸弹  炸弹的范围高度
            public double[] fish_min_multiple;//[FISH_KIND_COUNT];              //对应下标鱼的最小倍数
            public double[] fish_max_multiple;//[FISH_KIND_COUNT];              //对应下标鱼的最高倍数
            public double[] fish_probability;//[FISH_KIND_COUNT];           //对应下标鱼的打中概率
            public int[] fish_speed;//[FISH_KIND_COUNT];                //对应下标鱼的游动速度
            public int[] bullet_speed;//[BULLET_KIND_COUNT];            //子弹的飞行速度
            public bool lockFishCertainlyHit;                      //锁定的鱼是不是必定打得到
            public int bulletReboundTimes;                         //子弹回弹次数
            public ushort fireInterval;                                //开火间隔时间
            public ushort exchangeInterval;                            //上分间隔时间
            public float speedMul;                                 //速度倍数
            public int laserMul;                                   //设置需要打几炮满mul子弹  才能获得一个激光炮， -1表示永远不可能， 即关闭激光炮玩法
            public int laserReboundTimes;                          //设置激光子弹可以反弹的次数+
            public float tax;                                      //税收比例
            public long nowTaxValue;                               //当前积累税收值
        };
        public struct CMD_S_ADMIN_CONFIG_RESULT
        {
            public bool isSaveSuccess;     //保存配置文件是否成功
        };
        #endregion
        private bool HandleGameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            Console.WriteLine("金蟾捕鱼 游戏子命令：" + subCmd);
            switch (subCmd)
            {
                case SUB_S_GAME_CONFIG:
                    return HandleGAME_CONFIG(maincmd, subCmd, readbuffer);
                case SUB_S_FISH_TRACE:
                    return HandleFISH_TRACE(maincmd, subCmd, readbuffer);
                case SUB_S_EXCHANGE_FISHSCORE:
                    return HandleEXCHANGE_FISHSCORE(maincmd, subCmd, readbuffer);
                case SUB_S_USER_FIRE:
                    return HandleUSER_FIRE(maincmd, subCmd, readbuffer);
                case SUB_S_CATCH_FISH:
                    return HandleCATCH_FISH(maincmd, subCmd, readbuffer);
                case SUB_S_BULLET_ION_TIMEOUT:
                    return HandleBULLET_ION_TIMEOUT(maincmd, subCmd, readbuffer);
                case SUB_S_LOCK_TIMEOUT:
                    return HandleLOCK_TIMEOUT(maincmd, subCmd, readbuffer);
                case SUB_S_CATCH_SWEEP_FISH:
                    return HandleCATCH_SWEEP_FISH(maincmd, subCmd, readbuffer);
                case SUB_S_CATCH_SWEEP_FISH_RESULT:
                    return HandleCATCH_SWEEP_FISH_RESULT(maincmd, subCmd, readbuffer);
                case SUB_S_HIT_FISH_LK:
                    return HandleHIT_FISH_LK(maincmd, subCmd, readbuffer);
                case SUB_S_SWITCH_SCENE:
                    return HandleSWITCH_SCENE(maincmd, subCmd, readbuffer);
                case SUB_S_REPLY_NOW_INFO:
                    return HandleREPLY_NOW_INFO(maincmd, subCmd, readbuffer);
                case SUB_S_CIRCLE_FISH_INFO:
                    return HandleCIRCLE_FISH_INFO(maincmd, subCmd, readbuffer);
                case SUB_S_SPECIAL_FISH_INFO:
                    return HandleSPECIAL_FISH_INFO(maincmd, subCmd, readbuffer);
                case SUB_S_MESSAGE_FOR_CATCH_FISH:
                    return HandleMESSAGE_FOR_CATCH_FISH(maincmd, subCmd, readbuffer);
                case SUB_S_SEND_ADMIN_CONFIG:
                    return HandleSEND_ADMIN_CONFIG(maincmd, subCmd, readbuffer);
                case SUB_S_SAVE_ADMIN_CONFIG_RESULT:
                    return HandleSAVE_ADMIN_CONFIG_RESULT(maincmd, subCmd, readbuffer);
                case SUB_S_REPLY_NOW_FISH_INFO:
                    return HandleREPLY_NOW_FISH_INFO(maincmd, subCmd, readbuffer);
                default:
                    Error("金蟾捕鱼 未能解析的游戏子命令：" + subCmd);
                    break;
            }
            return true;
        }
        private bool HandleREPLY_NOW_FISH_INFO(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            Console.WriteLine("没接");
            return true;
        }
        private bool HandleSAVE_ADMIN_CONFIG_RESULT(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ADMIN_CONFIG_RESULT ADMIN_CONFIG_RESULT = new KCBF.KCBF_.CMD_S_ADMIN_CONFIG_RESULT();
            ADMIN_CONFIG_RESULT.isSaveSuccess = readbuffer.ReadBoolean();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.SAVE_ADMIN_CONFIG_RESULT(ADMIN_CONFIG_RESULT);
            }
            return true;
        }
        private bool HandleSEND_ADMIN_CONFIG(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {         
            CMD_S_ADMIN_CONFIG ADMIN_CONFIG = new KCBF.KCBF_.CMD_S_ADMIN_CONFIG();

            ADMIN_CONFIG.head = new KCBF.KCBF_.MSGHEAD();
            ADMIN_CONFIG.head.msgid = readbuffer.ReadInt();
            ADMIN_CONFIG.exchange_ratio_userscore = readbuffer.ReadInt();
            ADMIN_CONFIG.exchange_ratio_fishscore = readbuffer.ReadInt();
            ADMIN_CONFIG.exchange_count = readbuffer.ReadInt();
            ADMIN_CONFIG.mode = readbuffer.ReadInt();
            ADMIN_CONFIG.thresholdValue = readbuffer.ReadInt();
            ADMIN_CONFIG.androidValue = readbuffer.ReadFloat();
            ADMIN_CONFIG.androidFireIntervalBase = readbuffer.ReadInt();
            ADMIN_CONFIG.androidFireIntervalRand = readbuffer.ReadInt();
            ADMIN_CONFIG.ionCannonMultiple = readbuffer.ReadFloat();
            ADMIN_CONFIG.ionCannonProbability = readbuffer.ReadFloat();
            ADMIN_CONFIG.ionCannonAwardMultiple = readbuffer.ReadFloat();
            ADMIN_CONFIG.ionCannonDuration = readbuffer.ReadInt();
            int NUMBER = 64;
            ADMIN_CONFIG.GameIdWhite = new uint[NUMBER];
            for (int i = 0; i < ADMIN_CONFIG.GameIdWhite.Length; i++)
            {
                ADMIN_CONFIG.GameIdWhite[i] = readbuffer.ReadUint();
            }
            ADMIN_CONFIG.GameIdBlack = new uint[NUMBER];
            for (int i = 0; i < ADMIN_CONFIG.GameIdBlack.Length; i++)
            {
                ADMIN_CONFIG.GameIdBlack[i] = readbuffer.ReadUint();
            }
            ADMIN_CONFIG.blackValue = new float[NUMBER];
            for (int i = 0; i < ADMIN_CONFIG.blackValue.Length; i++)
            {
                ADMIN_CONFIG.blackValue[i] = readbuffer.ReadFloat();
            }
            ADMIN_CONFIG.whiteValue = new float[NUMBER];
            for (int i = 0; i < ADMIN_CONFIG.whiteValue.Length; i++)
            {
                ADMIN_CONFIG.whiteValue[i] = readbuffer.ReadFloat();
            }
            ADMIN_CONFIG.blackValue1 = readbuffer.ReadFloat();
            ADMIN_CONFIG.whiteValue1 = readbuffer.ReadFloat();
            ADMIN_CONFIG.fishProbabilityMode = new byte[15];
            for (int i = 0; i < ADMIN_CONFIG.fishProbabilityMode.Length; i++)
            {
                ADMIN_CONFIG.fishProbabilityMode[i] = readbuffer.ReadByte();
            }
            ADMIN_CONFIG.fishProbailityKeyValue = readbuffer.ReadFloat();            
            ADMIN_CONFIG.fishProbailityScale = readbuffer.ReadFloat();
            ADMIN_CONFIG.fishProbailityAndroidWelfare = readbuffer.ReadFloat();
            ADMIN_CONFIG.bullet_multiple = new int[20];
            for (int i = 0; i < ADMIN_CONFIG.bullet_multiple.Length; i++)
            {
                ADMIN_CONFIG.bullet_multiple[i] = readbuffer.ReadInt();
            }
            ADMIN_CONFIG.bullet_number = readbuffer.ReadInt();
            ADMIN_CONFIG.defIndex = readbuffer.ReadInt();


            ADMIN_CONFIG.cannon_Size = new int[20];
            for (int i = 0; i < ADMIN_CONFIG.cannon_Size.Length; i++)
            {
                ADMIN_CONFIG.cannon_Size[i] = readbuffer.ReadInt();
            }
            ADMIN_CONFIG.bomb_range_width = readbuffer.ReadInt();
            ADMIN_CONFIG.bomb_range_height = readbuffer.ReadInt();
            ADMIN_CONFIG.fish_min_multiple = new double[FISH_KIND_COUNT];
            for (int i = 0; i < ADMIN_CONFIG.fish_min_multiple.Length; i++)
            {
                ADMIN_CONFIG.fish_min_multiple[i] = readbuffer.ReadDouble();
            }


            

            ADMIN_CONFIG.fish_max_multiple = new double[FISH_KIND_COUNT];
            for (int i = 0; i < ADMIN_CONFIG.fish_max_multiple.Length; i++)
            {
                ADMIN_CONFIG.fish_max_multiple[i] = readbuffer.ReadDouble();
            }

            ADMIN_CONFIG.fish_probability = new double[FISH_KIND_COUNT];
            for (int i = 0; i < ADMIN_CONFIG.fish_probability.Length; i++)
            {
                ADMIN_CONFIG.fish_probability[i] = readbuffer.ReadDouble();
            }

            ADMIN_CONFIG.fish_speed = new int[FISH_KIND_COUNT];
            for (int i = 0; i < ADMIN_CONFIG.fish_speed.Length; i++)
            {
                ADMIN_CONFIG.fish_speed[i] = readbuffer.ReadInt();
            }

            ADMIN_CONFIG.bullet_speed = new int[BULLET_KIND_COUNT];
            for (int i = 0; i < ADMIN_CONFIG.bullet_speed.Length; i++)
            {
                ADMIN_CONFIG.bullet_speed[i] = readbuffer.ReadInt();
            }
            //mark

            ADMIN_CONFIG.lockFishCertainlyHit = readbuffer.ReadBoolean();
            ADMIN_CONFIG.bulletReboundTimes = readbuffer.ReadInt();
            ADMIN_CONFIG.fireInterval = readbuffer.ReadUshort();
            ADMIN_CONFIG.exchangeInterval = readbuffer.ReadUshort();
            ADMIN_CONFIG.speedMul = readbuffer.ReadFloat();
            ADMIN_CONFIG.laserMul = readbuffer.ReadInt();
            ADMIN_CONFIG.laserReboundTimes = readbuffer.ReadInt();

            //--------------------------------------------------------
            ADMIN_CONFIG.stockScore = new int[10];
            for (int i = 0; i < ADMIN_CONFIG.stockScore.Length; i++)
            {
                ADMIN_CONFIG.stockScore[i] = readbuffer.ReadInt();
            }

            ADMIN_CONFIG.stockIncreaseProbability = new float[10];
            for (int i = 0; i < ADMIN_CONFIG.stockIncreaseProbability.Length; i++)
            {
                ADMIN_CONFIG.stockIncreaseProbability[i] = readbuffer.ReadFloat();
            }

            ADMIN_CONFIG.stockCount = readbuffer.ReadInt();

            ADMIN_CONFIG.stockValue = readbuffer.ReadInt();
            ADMIN_CONFIG.nowStockValue = readbuffer.ReadLong();

            ADMIN_CONFIG.tempRevenueValue = readbuffer.ReadLong();
            ADMIN_CONFIG.tempRevenueLimitValue = readbuffer.ReadLong();
            ADMIN_CONFIG.tempRevenuePer = readbuffer.ReadFloat();

            //----------------------------------------------------------

            ADMIN_CONFIG.tax = readbuffer.ReadFloat();
            ADMIN_CONFIG.nowTaxValue = readbuffer.ReadLong();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.SEND_ADMIN_CONFIG(ADMIN_CONFIG);
            }
            return true;
        }
        private bool HandleMESSAGE_FOR_CATCH_FISH(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_MessageForCatchFish MessageForCatchFish = new KCBF.KCBF_.CMD_S_MessageForCatchFish();
            MessageForCatchFish.roomId = readbuffer.ReadUshort();
            MessageForCatchFish.tableId = readbuffer.ReadUshort();
            MessageForCatchFish.chairId = readbuffer.ReadUshort();
            MessageForCatchFish.fishKind = (FishKind)readbuffer.ReadInt();
            MessageForCatchFish.score = readbuffer.ReadLong();
            MessageForCatchFish.bulletMul = readbuffer.ReadInt();
            MessageForCatchFish.fishMul = readbuffer.ReadFloat();
            MessageForCatchFish.bulletKind = (BulletKind)readbuffer.ReadInt();
            MessageForCatchFish.playName = readbuffer.ReadString();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.MESSAGE_FOR_CATCH_FISH(MessageForCatchFish);
            }
            return true;
        }
        private bool HandleSPECIAL_FISH_INFO(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_SpecialInfo tsyu = new KCBF.KCBF_.CMD_S_SpecialInfo();
            tsyu.fishId= readbuffer.ReadInt();
            tsyu.countdown= readbuffer.ReadInt();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.SPECIAL_FISH_INFO(tsyu);
            }
            return true;
        }
        private bool HandleCIRCLE_FISH_INFO(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_CircleFishArrayInfo CircleFishArrayInfo = new CMD_S_CircleFishArrayInfo();
            CircleFishArrayInfo.circlePoint.x = readbuffer.ReadFloat();
            CircleFishArrayInfo.circlePoint.y = readbuffer.ReadFloat();
            CircleFishArrayInfo.fishNum = readbuffer.ReadInt();
            CircleFishArrayInfo.fishId = new int[30];
            for (int i = 0; i < CircleFishArrayInfo.fishId.Length; i++)
            {
                CircleFishArrayInfo.fishId[i]= readbuffer.ReadInt();
            }
            CircleFishArrayInfo.fishKind = (FishKind)readbuffer.ReadInt();
            CircleFishArrayInfo.redFishId = readbuffer.ReadInt();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.CIRCLE_FISH_INFO(CircleFishArrayInfo);
            }
            return true;
        }
        private bool HandleREPLY_NOW_INFO(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_FishArrayInfo FishArrayInfo = new KCBF.KCBF_.CMD_S_FishArrayInfo();
            FishArrayInfo.isFishArray = readbuffer.ReadBoolean();
            FishArrayInfo.sceneKind = (SceneKind)readbuffer.ReadInt();

            CMD_S_SwitchScene sw = new KCBF.KCBF_.CMD_S_SwitchScene();
            sw.scene_kind = (SceneKind)readbuffer.ReadInt();
            sw.fish_count = readbuffer.ReadInt();
            sw.fish_kind = new FishKind[300];
            for (int i = 0; i < sw.fish_kind.Length; i++)
            {
                sw.fish_kind[i] = (FishKind)readbuffer.ReadInt();
            }
            sw.fish_id = new int[300];
            for (int i = 0; i < sw.fish_id.Length; i++)
            {
                sw.fish_id[i] = readbuffer.ReadInt();
            }
            sw.redFish = new bool[300];
            for (int i = 0; i < sw.redFish.Length; i++)
            {
                sw.redFish[i] = readbuffer.ReadBoolean();
            }
            FishArrayInfo.initSwitchScene = sw;
            FishArrayInfo.dieFishId = new int[300];
            for (int i = 0; i < FishArrayInfo.dieFishId.Length; i++)
            {
                FishArrayInfo.dieFishId[i] = readbuffer.ReadInt();
            }
            FishArrayInfo.dieFishCount = readbuffer.ReadInt();
            FishArrayInfo.passTime = readbuffer.ReadUint();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.REPLY_NOW_INFO(FishArrayInfo);
            }
            return true;
        }
        private bool HandleSWITCH_SCENE(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_SwitchScene sw = new KCBF.KCBF_.CMD_S_SwitchScene();
            sw.scene_kind = (SceneKind)readbuffer.ReadInt();
            sw.fish_count = readbuffer.ReadInt();
            sw.fish_kind = new FishKind[300];
            for (int i = 0; i < sw.fish_kind.Length; i++)
            {
                sw.fish_kind[i] = (FishKind)readbuffer.ReadInt();
            }
            sw.fish_id = new int[300];
            for (int i = 0; i < sw.fish_id.Length; i++)
            {
                sw.fish_id[i] = readbuffer.ReadInt();
            }
            sw.redFish = new bool[300];
            for (int i = 0; i < sw.redFish.Length; i++)
            {
                sw.redFish[i] = readbuffer.ReadBoolean();
            }
            if (KCBFInterFace != null)
            {
                KCBFInterFace.SWITCH_SCENE(sw);
            }
            return true;
        }
        private bool HandleHIT_FISH_LK(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_HitFishLK HitFishLK = new KCBF.KCBF_.CMD_S_HitFishLK();
            HitFishLK.fish_id = readbuffer.ReadInt();
            HitFishLK.fish_mulriple = readbuffer.ReadInt();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.HIT_FISH_LK(HitFishLK);
            }
            return true;
        }
        private bool HandleCATCH_SWEEP_FISH_RESULT(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_CatchSweepFishResult CatchSweepFishResult = new KCBF.KCBF_.CMD_S_CatchSweepFishResult();
            CatchSweepFishResult.chair_id = readbuffer.ReadUshort();
            CatchSweepFishResult.fish_id = readbuffer.ReadInt();
            CatchSweepFishResult.fish_score = readbuffer.ReadLong();
            CatchSweepFishResult.catch_fish_count = readbuffer.ReadInt();
            CatchSweepFishResult.catch_fish_id = new int[300];//[300]
            for (int i = 0; i < CatchSweepFishResult.catch_fish_id.Length; i++)
            {
                CatchSweepFishResult.catch_fish_id[i] = readbuffer.ReadInt();
            }
            CatchSweepFishResult.bulletMul = readbuffer.ReadInt();  //子弹倍数
            if (KCBFInterFace != null)
            {
                KCBFInterFace.CATCH_SWEEP_FISH_RESULT(CatchSweepFishResult);
            }
            return true;
        }
        private bool HandleCATCH_SWEEP_FISH(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_CatchSweepFish CatchSweepFish = new CMD_S_CatchSweepFish();
            CatchSweepFish.chair_id = readbuffer.ReadUshort();
            CatchSweepFish.fish_id = readbuffer.ReadInt();
            CatchSweepFish.android = readbuffer.ReadBoolean();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.CATCH_SWEEP_FISH(CatchSweepFish);
            }
            return true;
        }
        private bool HandleLOCK_TIMEOUT(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            if (KCBFInterFace != null)
            {
                KCBFInterFace.LOCK_TIMEOUT();
            }
            return true;
        }
        private bool HandleBULLET_ION_TIMEOUT(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_BulletIonTimeout LiZiPao = new KCBF.KCBF_.CMD_S_BulletIonTimeout();
            LiZiPao.chair_id = readbuffer.ReadUshort();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.BULLET_ION_TIMEOUT(LiZiPao);
            }
            return true;
        }
        private bool HandleCATCH_FISH(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_CatchFish CatchFish = new CMD_S_CatchFish();
            CatchFish.chair_id = readbuffer.ReadUshort();
            CatchFish.fish_id = readbuffer.ReadInt();
            CatchFish.fish_kind = (FishKind)readbuffer.ReadInt();         //判断是否需要做效果
            CatchFish.bullet_ion = readbuffer.ReadBoolean();            //是否有附加状态， 导致离子炮的产生
            CatchFish.fish_score = readbuffer.ReadLong();
            if (KCBFInterFace != null)
            {
                KCBFInterFace.CATCH_FISH(CatchFish);
            }
            return true;
        }
        private bool HandleUSER_FIRE(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_UserFire user_fire = new CMD_S_UserFire();
            user_fire.bullet_kind = (BulletKind)readbuffer.ReadInt();
            user_fire.bullet_id = readbuffer.ReadInt();
            user_fire.chair_id = readbuffer.ReadUshort();
            user_fire.angle = readbuffer.ReadFloat();
            user_fire.bullet_mulriple = readbuffer.ReadInt();
            user_fire.lock_fishid = readbuffer.ReadInt();
            user_fire.android = readbuffer.ReadBoolean();
            user_fire.laserValue = readbuffer.ReadInt(); //激光炮累积值， -1表示获得一个激光炮
            if (KCBFInterFace != null)
            {
                KCBFInterFace.USER_FIRE(user_fire);
            }
            return true;
        }
        private bool HandleEXCHANGE_FISHSCORE(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_ExchangeFishScore ExchangeFishScore = new CMD_S_ExchangeFishScore();
            ExchangeFishScore.chair_id = readbuffer.ReadUshort();
            ExchangeFishScore.swap_fish_score = readbuffer.ReadLong();
            ExchangeFishScore.exchange_fish_score = readbuffer.ReadLong();
            ExchangeFishScore.uesr_score = new long[6]; 
            for (int i = 0; i < 6; i++)
            {
                ExchangeFishScore.uesr_score[i] = readbuffer.ReadLong();
            }
            if (KCBFInterFace != null)
            {
                KCBFInterFace.EXCHANGE_FISHSCORE(ExchangeFishScore);
            }
            return true;
        }
        private bool HandleFISH_TRACE(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_FishTrace fish_reace = new CMD_S_FishTrace();
            fish_reace.init_pos = new FPoint[10];//[10];
            for (int i = 0; i < fish_reace.init_pos.Length; i++)
            {
                fish_reace.init_pos[i] = new FPoint();
                fish_reace.init_pos[i].x = readbuffer.ReadFloat();
                fish_reace.init_pos[i].y = readbuffer.ReadFloat();
            }
            fish_reace.init_count = readbuffer.ReadInt();
            fish_reace.fish_kind = (FishKind)readbuffer.ReadInt();
            fish_reace.fish_id = readbuffer.ReadInt();
            fish_reace.trace_type = (TraceType)readbuffer.ReadInt();
            fish_reace.fishKindInfo = new byte[5];//[5];   //保存一箭多雕时的鱼类信息
            for (int i = 0; i < fish_reace.fishKindInfo.Length; i++)
            {
                fish_reace.fishKindInfo[i] = readbuffer.ReadByte();
            }
            fish_reace.redFish = readbuffer.ReadBoolean();         //标志是否为红鱼
            if (KCBFInterFace != null)
            {
                KCBFInterFace.FISH_TRACE(fish_reace);
            }
            return true;
        }
        private bool HandleGAME_CONFIG(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_S_GameConfig game_config = new KCBF.KCBF_.CMD_S_GameConfig();
            game_config.exchange_ratio_userscore = readbuffer.ReadInt();
            game_config.exchange_ratio_fishscore = readbuffer.ReadInt();
            game_config.exchange_count = readbuffer.ReadInt();
            game_config.mode = readbuffer.ReadInt();              //mode=1表示需要上分， 0表示不需要上分

            game_config.min_bullet_multiple = readbuffer.ReadInt();
            game_config.max_bullet_multiple = readbuffer.ReadInt();
            //add by lucas (config package)
            game_config.def_bullet_multiple = readbuffer.ReadInt();
            game_config.bullet_multiple = new int[20];//[20]
            for (int i = 0; i < game_config.bullet_multiple.Length; i++)
            {
                game_config.bullet_multiple[i] = readbuffer.ReadInt();
            }
            game_config.bullet_number = readbuffer.ReadInt();
            game_config.def_canSize = readbuffer.ReadInt();
            game_config.cannon_Size = new int[20];//[20]
            for (int i = 0; i < game_config.cannon_Size.Length; i++)
            {
                game_config.cannon_Size[i] = readbuffer.ReadInt();
            }
            //add end;

            game_config.bomb_range_width = readbuffer.ReadInt();
            game_config.bomb_range_height = readbuffer.ReadInt();
            game_config.fish_multiple = new double[FISH_KIND_COUNT];//[FishKind.FISH_KIND_COUNT];
            for (int i = 0; i < game_config.fish_multiple.Length; i++)
            {
                game_config.fish_multiple[i] = readbuffer.ReadDouble();
            }
            game_config.fish_speed = new int[FISH_KIND_COUNT];//[FISH_KIND_COUNT];
            for (int i = 0; i < game_config.fish_speed.Length; i++)
            {
                game_config.fish_speed[i] = readbuffer.ReadInt();
            }
            game_config.fish_bounding_box_width = new int[FISH_KIND_COUNT];//[FISH_KIND_COUNT];
            for (int i = 0; i < game_config.fish_bounding_box_width.Length; i++)
            {
                game_config.fish_bounding_box_width[i] = readbuffer.ReadInt();
            }
            game_config.fish_bounding_box_height = new int[FISH_KIND_COUNT];//[FISH_KIND_COUNT];
            for (int i = 0; i < game_config.fish_bounding_box_height.Length; i++)
            {
                game_config.fish_bounding_box_height[i] = readbuffer.ReadInt();
            }
            game_config.fish_hit_radius = new int[FISH_KIND_COUNT];///[FISH_KIND_COUNT];
            for (int i = 0; i < game_config.fish_hit_radius.Length; i++)
            {
                game_config.fish_hit_radius[i] = readbuffer.ReadInt();
            }

            game_config.bullet_speed = new int[BULLET_KIND_COUNT];//[BULLET_KIND_COUNT];
            for (int i = 0; i < game_config.bullet_speed.Length; i++)
            {
                game_config.bullet_speed[i] = readbuffer.ReadInt();
            }
            game_config.net_radius = new int[BULLET_KIND_COUNT];//[BULLET_KIND_COUNT];
            for (int i = 0; i < game_config.net_radius.Length; i++)
            {
                game_config.net_radius[i] = readbuffer.ReadInt();
            }
            game_config.bullet_bounding_box_width_ = new int[BULLET_KIND_COUNT];//[BULLET_KIND_COUNT];  //对应下标编号的子弹的宽度
            for (int i = 0; i < game_config.bullet_bounding_box_width_.Length; i++)
            {
                game_config.bullet_bounding_box_width_[i] = readbuffer.ReadInt();
            }
            game_config.bullet_bounding_box_height_ = new int[BULLET_KIND_COUNT];//[BULLET_KIND_COUNT]; //对应下标编号的子弹的高度
            for (int i = 0; i < game_config.bullet_bounding_box_height_.Length; i++)
            {
                game_config.bullet_bounding_box_height_[i] = readbuffer.ReadInt();
            }
            game_config.lockFishCertainlyHit = readbuffer.ReadBoolean();                          //锁定的鱼是不是必定打得到

            game_config.bulletReboundTimes = readbuffer.ReadInt();                                //子弹回弹次数
            game_config.fireInterval = readbuffer.ReadUshort();                            //开火间隔时间
            game_config.exchangeInterval = readbuffer.ReadUshort();                        //上分间隔时间
            game_config.speedMul = readbuffer.ReadFloat();                                         //速度倍数
            game_config.laserMul = readbuffer.ReadInt();                                         //设置需要打几炮满mul子弹  才能获得一个激光炮， -1表示永远不可能， 即关闭激光炮玩法
            game_config.laserReboundTimes = readbuffer.ReadInt();                                  //设置激光子弹可以反弹的次数
            if (KCBFInterFace != null)
            {
                KCBFInterFace.GAME_CONFIG(game_config);
            }
            return true;
        }
    }
}
