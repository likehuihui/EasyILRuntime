using CYNetwork;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SHTB
{
    /// <summary>
    /// 常量定义
    /// </summary>
    public class Define
    {
        public const ushort KIND_ID = 8801;                        //游戏 I D
        public const ushort GAME_PLAYER = 4;                       //游戏人数
        public const string GAME_NAME = "深海探宝";                //游戏名字
        public const ushort MAX_BULLET = 20;                       //每个玩家最多有多少子弹同屏
        public const ushort BULLET_ID_BASE = 1000;                 //子弹ID计算基准
        public const ushort LEN_NICKNAME = 32;                      //昵称长度
        public const ushort FREEZE_CANON = 3;		    //炮台类型-凝霜
        public const ushort CROSS_CANON = 4;		    //炮台类型-穿云
    }

    #region 枚举类型 (服务器发过来的枚举类型为 int)

    /// <summary>
    /// 鱼被击时的反应 
    /// (服务器发过来的枚举类型为 int)
    /// </summary>    
    public enum FishActionEventType
    {
        //1.捕获 2.受击 3.播放特效(冰冻减速)
        ACTION_EVENT_NONE = 0,
        ACTION_EVENT_CATCHED,
        ACTION_EVENT_ATTACK,
        ACTION_EVENT_EFFECT
    };

    //掉落物品
    public enum DropRewardItem
    {
        //1凝霜	2穿云
        DROP_REWARD_ITEM_NONE = 0,
        DROP_REWARD_ITEM_FROZEN,
        DROP_REWARD_ITEM_CROSS
    };

    //错误代码
    public enum ErrorCodeEnum
    {
        /// <summary>
        /// 错误的子弹类型  无法切换子弹
        /// </summary>
        ERROR_BULLET_TYPE = 0,
        /// <summary>
        /// 能量不够 无法使用能量跑
        /// </summary>
        ERROR_NO_ENOUGH_ENERGY,
        /// <summary>
        /// 碎片不够 无法使用特殊炮台
        /// </summary>
        ERROR_NO_ENOUGH_CHIPS,
        /// <summary>
        /// 子弹类型错误 开火失败
        /// </summary>
        ERROR_NO_SUCH_BULLET_DATA,
        /// <summary>
        /// 剩余金币不够 已破产 开火失败
        /// </summary>
        ERROR_NO_ENOUGH_GOLD,
        /// <summary>
        /// 同时存在的子弹过多 开火失败
        /// </summary>
        ERROR_TOO_MUCH_BULLET,
        /// <summary>
        /// 存在ID相同的子弹 K开火失败
        /// </summary>
        ERROR_EXIST_SAME_ID_BULLET,
        /// <summary>
        /// 不存在该鱼 锁定失败
        /// </summary>
        ERROR_LOCK_WITH_NO_FISH,
        /// <summary>
        /// 正在使用特殊炮台 无法使用新的特殊炮台
        /// </summary>
        ERROR_IS_USING_SPECIAL_CANON,
    };

    //技能的类型
    public enum FishSkillEventType
    {
        SKILL_EVENT_NONE = 0,
        /// <summary>
        /// 闪电
        /// </summary>
        SKILL_EVENT_LIGHTING,
        /// <summary>
        /// 冰冻
        /// </summary>
        SKILL_EVENT_FROZEN,
        /// <summary>
        /// 暴风
        /// </summary>
        SKILL_EVENT_STORM,
        /// <summary>
        /// 陨石
        /// </summary>
        SKILL_EVENT_VOLCANO
    };

    #endregion

    #region 通信协议ID
    /// <summary>
    /// 客户端到服务器的消息ID
    /// </summary>
    public class C2SMsgId
    {
        public const ushort SUB_C_HEART_BEAT = 1;            //心跳包        
        public const ushort SUB_C_CHANGE_BULLET = 10;           //切换子弹
        public const ushort SUB_C_REQUEST_FIRE = 11;           //请求开火
        public const ushort SUB_C_REQUEST_LOCK = 12;           //请求锁定
        public const ushort SUB_C_REQ_AUTO_STATE = 16;              //切换自动状态
        public const ushort SUB_C_OPEN_SPECIAL_CANON = 13;           //请求开启特殊炮台(凝霜 穿云)
        public const ushort SUB_C_REQUEST_LASER = 14;           //请求发射激光炮
        public const ushort SUB_C_REQ_BULLET_IMP = 15;           //游戏客户端请求子弹碰撞结果

        public const ushort SUB_C_SHOW_FISH_REQUEST = 20;           //请求图鉴数据



        public const ushort SUB_C_GM_GET_CONFIG = 30;               //请求GM控制信息
        public const ushort SUB_C_GM_SAVE_CONFIG = 31;              //保存GM控制信息
        public const ushort SUB_C_GM_GET_STOCK = 32;            //请求库存信息
        public const ushort SUB_C_GM_SAVE_STOCK = 33;               //保存库存信息
        public const ushort SUB_C_CLEAR_STOCK = 34;				//清空库存
        public const ushort SUB_C_GM_UPDATE_BW_LIST = 35;         //更新黑白名单

    }

    /// <summary>
    /// 服务器到客户端消息ID
    /// </summary>
    public class S2CMsgId
    {
        public const ushort SUB_S_HEART_BEAT = 2;			//心跳包
        public const ushort SUB_S_INIT_GAME_DATA = 100;			//进入更新炮台等数据
        public const ushort SUB_S_INIT_SCENE_FISH = 101;			//进入更新当前场景鱼的数据
        public const ushort SUB_S_INIT_SCENE_BULLET = 102;			//进入更新所有玩家子弹的数据
        public const ushort SUB_S_PLAYER_JOIN = 103;			//玩家进入房间
        public const ushort SUB_S_PLAYER_LEAVE = 104;			//玩家离开房间

        public const ushort SUB_S_CHANGE_BULLET = 110;			//切换子弹
        public const ushort SUB_S_BULLET_FIRE = 111;			//玩家开火
        public const ushort SUB_S_LOCK_FISH = 112;			//锁定某条鱼
        public const ushort SUB_S_OPEN_SPECIAL_CANON = 113;			//开启特殊炮台(凝霜 穿云)
        public const ushort SUB_S_CLEAR_SCENE = 114;          //清理场景
        public const ushort SUB_S_BACK_AUTO_STATE = 115;    //切换状态	

        public const ushort SUB_S_LAUNCH_GROUP_FISH = 120;			//发送一组鱼

        public const ushort SUB_S_CATCH_FISH = 130;          //子弹打中鱼
        public const ushort SUB_S_LASER_BACK = 131;			//请求发射激光炮

        public const ushort SUB_S_FIALED_CODE = 140;          //错误码返回
        public const ushort SUB_S_SHOW_FISH_REQUEST = 141;			//图鉴数据返回
        public const ushort SUB_S_GM_GET_CONFIG = 150;          //请求GM控制信息返回
        public const ushort SUB_S_GM_SAVE_CONFIG = 151;          //保存GM控制信息返回(总的保存)
        public const ushort SUB_S_GM_GET_STOCK = 152;           //请求库存信息返回
        public const ushort SUB_S_BACK_FOR_STOCK = 153;         //保存/修改库存信息返回(只保存库存)
        public const ushort SUB_S_GM_UPDATE_BW_LIST = 154;         //请求黑白名单
    }

    #endregion

    #region 结构体

    //心跳协议
    public struct CMD_CS_HeartBeat                 //SUB_C_HEART_BEAT  SUB_S_HEART_BEAT
    {
        public ushort Sign;
    };

    //客户端请求子弹碰撞鱼
    public struct CMD_C_CollideByClientBullet
    {
        public ushort BulletID;                //子弹ID
        public ushort CollideNum;              //碰撞的鱼数量
        public ushort[] FishIDs;         //被碰到的鱼的ID
    };

    //子弹的基础数据
    public class BulletBaseData
    {
        /// <summary>
        /// 子弹类型
        /// </summary>
        public ushort BulletType;
        /// <summary>
        /// 子弹消耗的金币
        /// </summary>
        public long Value;
        /// <summary>
        /// 炮台类型
        /// </summary>
        public ushort CanonType;
        /// <summary>
        /// 子弹速度
        /// </summary>
        public float Speed;
        /// <summary>
        /// 子弹开火间隔
        /// </summary>                 
        public float FireIntev;
    };

    //游戏初始化数据
    public struct CMD_S_InitGameData               //SUB_S_INIT_GAME_DATA   替换JoinRoomData
    {
        public ushort ChairID;               //座位
        public byte BackgroundImage;       //背景

        public int Energy;                 //当前激光炮能量值
        public int MaxEnergy;              //发射激光炮需要的能量值

        public int FrostNowNum;            //凝霜碎片当前数量
        public int FrostNeedMax;           //凝霜炮台发动需要的碎片数量
        public uint FrostLeftTime;            //凝霜炮台剩余时间

        public int CrossNowNum;            //穿云碎片当前数量
        public int CrossNeedMax;           //穿云炮台发动需要的碎片数量
        public uint CrossLeftTime;            //穿云炮台剩余时间

        public ushort MaxBulletType;           //子弹炮台配置数量
        public BulletBaseData[] BulletData;       //子弹炮台数据 size:9
    };

    public struct RealTimeFish
    {
        public ushort FishID;
        public ushort GroupID;
        public float FishTime;
        public ushort PathGroup;
        public ushort PathIdx;
        public bool IsActiveEvent;         //嘲讽时间
        public ushort ElapsedTime;         //
        public byte DelayType;             //减速类型
        public byte DelayScaling;          //减速百分比
        public byte DelayDuration1;            //
        public byte DelayDuration2;            //
        public byte DelayDuration3;            //
        public ushort DelayCurrentTime;        //
        //客户端时间，自己使用
        public uint tick;
    };


    //初始化场景中的鱼
    public struct CMD_S_InitSceneFish              //SUB_S_INIT_SCENE_FISH
    {
        public ushort CurrentNum;                          //当前屏幕上鱼的总数量
        public RealTimeFish[] Fishes;		//屏幕上实时鱼数据
    };

    public struct RealTimeBullet
    {
        public ushort BulletID;
        public short Degree;
        public ushort Time;
        public ushort BulletType; //子弹的类型(决定消耗的金币 速度等)
        public ushort CanonType; //炮台类型(决定子弹外形)        
        public ushort LockFishID;
    };

    public struct PlayerBaseData
    {
        public uint UesrId;
        public ushort ChairID;
        public string PlayerName;
        public uint FaceId;
        public long NowScore;
        public ushort BulletType;
        public ushort CanonType;
    };

    //初始化场景中所有玩家的子弹
    public struct CMD_S_InitSceneBullet            //SUB_S_INIT_SCENE_BULLET
    {
        /// <summary>
        /// 玩家数量
        /// </summary>
        public ushort PlayerNum;
        public PlayerBaseData[] playerData;
        public ushort CurrentNum;                          //当前屏幕上子弹的总数量
        public RealTimeBullet[] Bullets;   //屏幕上实时子弹数据
        //客户端时间，自己使用
        public uint tick;
    };

    //进入房间
    public struct CMD_S_PlayerJoin                 //SUB_S_PLAYER_JOIN
    {
        public ushort ChairID;                  //玩家座位
        public uint UserID;                     //玩家UserId
        public string NickName;                 //昵称
        public uint FaceID;                     //头像
        public long BringGold;                  //携带金币数量
        public ushort BulletType;               //子弹类型
        public ushort CanonType;                //炮台类型
        public long BulletValue;                //开火子弹需要消耗的金币
    };

    //退出房间
    public struct CMD_S_PlayerLeave                //SUB_S_PLAYER_LEAVE
    {
        public ushort ChairID;
    };

    //请求切换子弹
    public struct CMD_C_ChangeBullet               //SUB_C_CHANGE_BULLET
    {
        public ushort BulletType;
    };

    //请求切换子弹返回
    public struct CMD_S_ChangeBullet               //SUB_S_CHANGE_BULLET
    {
        public ushort ChairID;
        public ushort BulletType;
        public ushort CanonType;
        public long BulletValue;
    };

    //请求开火
    public struct CMD_C_RequestFire                //SUB_C_REQUEST_FIRE
    {
        public ushort BulletID;                //子弹ID
        public short Angle;                    //角度
        public ushort CanonType;               //炮台类型                
        public ushort LockFishID;              //锁定鱼ID
    };

    //开火返回数据
    public struct CMD_S_RequestFire                //SUB_S_BULLET_FIRE
    {
        public ushort ChairID;
        public long BulletValue;           //子弹消耗的金币
        public ushort BulletID;                //子弹ID
        public short Angle;                    //角度
        public ushort CanonType;               //炮台类型
        public uint Energy;                    //能量
        public ushort LockFishID;              //锁定鱼ID
        public uint tick;
    };

    //请求锁定
    public struct CMD_C_RequestLock                //SUB_C_REQUEST_LOCK
    {
        public ushort Action;                  //0为解锁   1为锁定
        public ushort LockFishID;              //锁定鱼ID
    };

    //锁定或者切换锁定返回
    public struct CMD_S_ChangeLock                 //SUB_S_LOCK_FISH
    {
        public ushort ChairID;
        public ushort Action;                  //0为解锁   1为锁定
        public ushort LockFishID;              //锁定鱼ID
    };
    /// <summary>
    /// 自动切换
    /// </summary>
    public struct CMD_ChangeAutoState
    {
        public ushort ChairID;//座位号码
        public ushort State;//	0取消自动		1自动中...

    }
    //请求开启特殊炮台
    public struct CMD_C_RequestTurnOnSpecialCanon  //SUB_C_OPEN_SPECIAL_CANON
    {
        public ushort SpecialCanonType;        //请求特殊炮台的类型
    };

    //开启特殊炮台
    public struct CMD_S_TurnOnSpecialCanon         //SUB_S_OPEN_SPECIAL_CANON
    {
        public ushort ChairID;
        public ushort BulletType;              //子弹类型
        public ushort CanonType;               //炮台类型
        public long BulletValue;           //每一发子弹需要消耗的金币
        public uint Time;                  //剩余时间
    };

    //产生鱼阵
    public struct CMD_S_CreateFishGroup            //SUB_S_LAUNCH_GROUP_FISH
    {
        public ushort GroupID;             //组ID
        public ushort PathID;                  //路径ID
        public ushort StartID;             //起始ID    
        //收到命令的时间(仅客户端使用)
        public uint tick;
    };

    //打死鱼的结构体
    public class KillFishData
    {
        public long UserNowScore;          //计算完后 该用户当前的金币数目
        public int CatchedFishNum;         //打死多少条鱼(不计算技能打死的)
        public FishCatched[] CatchedFishData;    //打死的鱼的数据(不计算技能打死的)
        public int CollideFishNum;         //碰到但没杀死的鱼的数量(主要用在减速炮减速对应的鱼)
        public ushort[] CollideFishsID;      //碰到但没杀死的鱼的ID
        public FishSkillEventType SkillType;       //触发的技能类型
        public ushort SkillMainFishID;          //触发技能的主鱼
        public int SkillCatchedFishNum;        //技能打死多少条鱼
        public FishCatched[] SkillCatchedFishData;   //技能打死的鱼的数据

        public KillFishData(CyNetReadBuffer rb)
        {
            try
            {
                UserNowScore = rb.ReadLong();
                CatchedFishNum = rb.ReadInt();
                CatchedFishData = new FishCatched[CatchedFishNum];
                for (var i = 0; i < CatchedFishData.Length; i++)
                {
                    FishCatched vo = new FishCatched(rb);
                    CatchedFishData[i] = vo;
                }
                CollideFishNum = rb.ReadInt();
                CollideFishsID = new ushort[CollideFishNum];
                for (var i = 0; i < CollideFishsID.Length; i++)
                {
                    CollideFishsID[i] = rb.ReadUshort();
                }

                SkillType = (FishSkillEventType)rb.ReadInt();
                SkillMainFishID = rb.ReadUshort();
                SkillCatchedFishNum = rb.ReadInt();
                SkillCatchedFishData = new FishCatched[SkillCatchedFishNum];
                for (var i = 0; i < SkillCatchedFishData.Length; i++)
                {
                    FishCatched vo = new FishCatched(rb);
                    SkillCatchedFishData[i] = vo;
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    };

    //普通子弹动作数据
    public struct CMD_S_BulletActionHandler
    {
        public ushort BulletID;                //子弹ID
        public uint GoldNum;               //总共获得的金币数量				
        public ushort ChairID;               //椅子ID
        public KillFishData ThisKillData;          //杀死鱼的数据
        public uint tick;
    };

    //失败返回错误码
    public struct CMD_S_FailedCode
    {
        public ErrorCodeEnum ErrorCode;
    };

    public struct FishCatched
    {
        public FishActionEventType ActionEvent;//被击的反应
        public float MultAg;                 //当前鱼倍率
        public uint GoldAg;                  //返还分数
        public ushort FishID;                 //鱼的ID
        public DropRewardItem Reward;      //掉落物品

        public FishCatched(CyNetReadBuffer rb)
        {
            ActionEvent = (FishActionEventType)rb.ReadInt();
            MultAg = rb.ReadFloat();
            GoldAg = rb.ReadUint();
            FishID = rb.ReadUshort();
            Reward = (DropRewardItem)rb.ReadInt();
            //if (Reward != DropRewardItem.DROP_REWARD_ITEM_NONE)
            //{
            //    Debug.Log("掉落物品：" + Reward);
            //}
        }
    };

    //图鉴鱼结构体
    public struct ShowFishData
    {
        public ushort ClientFishID;     //鱼ID
        public string FishName;       //鱼名称 size:64
        public float ValueMin;                     //最小倍数
        public float ValueMax;                     //最大倍数
        public string Declare;        //描述信息 size:64

        public ShowFishData(CyNetReadBuffer rb)
        {
            ClientFishID = rb.ReadUshort();
            FishName = rb.ReadString(64);
            ValueMin = rb.ReadFloat();
            ValueMax = rb.ReadFloat();

            Declare = rb.ReadString(64);
        }
    }

    public struct CMD_S_ShowFishRequest
    {
        public int FishKindNum;                    //鱼种数目
        public ShowFishData[] FishData;            //鱼钟数据
    };

    //激光炮请求
    public struct CMD_C_RequestLaser
    {
        public short Degree;                   //角度
    };

    //激光炮返回
    public struct CMD_S_LaserActionHandler
    {
        public ushort LaserType;               //激光类型	(同炮台类型)
        public uint GoldNum;               //总共获得的金币数量
        public ushort ChairID;               //椅子ID
        public short Degree;                   //角度
        public KillFishData ThisKillData;          //杀死鱼的数据
        public uint tick;
    };

    //清理场景
    public struct CMD_S_ClearScene
    {
        public byte ClearType;             //0从左到右，1从右到左
        public uint tick;
    };
    #endregion




    #region GM结构体
    //子弹数据结构体
    public struct BulletConfig
    {
        public int Id;                         //子弹ID
        public long Value;                     //消耗的金币
        public float HitPer;                       //击中概率
        public int Feather;                    //子弹外形种类
        public float Speed;                        //子弹速度
        public float HitSpace;                 //开火间隔
        public float Radius;                       //子弹碰撞半径
        public BulletConfig(CyNetReadBuffer rb)
        {
            Id = rb.ReadInt();
            Value = rb.ReadLong();
            HitPer = rb.ReadFloat();
            Feather = rb.ReadInt();
            Speed = rb.ReadFloat();
            HitSpace = rb.ReadFloat();
            Radius = rb.ReadFloat();
        }
    };

    //炮台子弹类型结构体
    public struct CononConfig
    {
        public int Id;                         //子弹外形ID
        public string Name;           //名称  size:64
        public int MaxCatch;                   //最多击中数量
        public CononConfig(CyNetReadBuffer rb)
        {
            Id = rb.ReadInt();
            Name = rb.ReadString(64);
            MaxCatch = rb.ReadInt();
        }
    };

    //鱼类基础信息结构体
    public struct FishBaseConfig
    {
        public int Id;                         //鱼ID
        public string Name;           //名称 size:64
        public float ValueMin;                 //打中最小倍数
        public float ValueMax;                 //打中最大倍数(若为0则取最小倍数)
        public string Declare;        //描述 size:64
        public FishBaseConfig(CyNetReadBuffer rb)
        {
            Id = rb.ReadInt();
            Name = rb.ReadString(64);
            ValueMin = rb.ReadFloat();
            ValueMax = rb.ReadFloat();
            Declare = rb.ReadString(64);
        }
    };

    //鱼类基础信息结构体
    public struct FishProbConfig
    {
        public int Id;                         //鱼ID(对应BaseConfig中的ID)
        public float Chance;                       //打中概率
        public float Frost;                        //凝霜碎片掉率
        public float Through;                  //穿云碎片掉率
        public int KillCondMulti;              //距离上次Boss被打死,需要消耗金币的倍数(按照当前炮弹的值)
        public FishProbConfig(CyNetReadBuffer rb)
        {
            Id = rb.ReadInt();
            Chance = rb.ReadFloat();
            Frost = rb.ReadFloat();
            Through = rb.ReadFloat();
            KillCondMulti = rb.ReadInt();
        }
    };

    /// <summary>
    /// 基本配置
    /// </summary>
    public struct CMD_GM_BaseConfig
    {
        public float CatchBase;            //子弹打死鱼基础概率
        public float DropPer;          //碎片掉落概率
        public uint XPSkillCount;      //激光炮需要子弹数量
        public float XPSKillPer1;      //激光炮杀死小鱼的概率
        public float XPSKillPer2;      //激光炮杀死大鱼的概率
        public uint XPSMaxCatch;       //激光炮最多打死多少条鱼
        public uint XPSMaxMult;            //激光炮最多打死的鱼的倍数
        public uint GetCannon3_Need;   //凝霜所需碎片
        public uint Cannon3_CD;            //凝霜使用时限(秒)
        public uint GetCannon4_Need;   //穿云所需碎片
        public uint Cannon4_CD;            //穿云使用时限(秒)
        public BulletConfig[] BulletConfig;   //子弹配置 [9]
        public CononConfig[] CanonConfig;     //炮台配置 [5]
        public CMD_GM_BaseConfig(CyNetReadBuffer rb)
        {
            try
            {
                CatchBase = rb.ReadFloat();
                DropPer = rb.ReadFloat();
                XPSkillCount = rb.ReadUint();
                XPSKillPer1 = rb.ReadFloat();
                XPSKillPer2 = rb.ReadFloat();
                XPSMaxCatch = rb.ReadUint();
                XPSMaxMult = rb.ReadUint();
                GetCannon3_Need = rb.ReadUint();
                Cannon3_CD = rb.ReadUint();
                GetCannon4_Need = rb.ReadUint();
                Cannon4_CD = rb.ReadUint();
                BulletConfig = new SHTB.BulletConfig[9];
                for (var i = 0; i < BulletConfig.Length; i++)
                {
                    BulletConfig vo = new SHTB.BulletConfig(rb);
                    BulletConfig[i] = vo;
                }
                CanonConfig = new CononConfig[5];
                for (var i = 0; i < CanonConfig.Length; i++)
                {
                    CononConfig vo = new CononConfig(rb);
                    CanonConfig[i] = vo;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    /// <summary>
    /// 库存配置
    /// </summary>
    public struct CMD_GM_StockConfig
    {
        public long Stock;                         //库存
        public long SystemRevenue;                 //系统抽水
        public int SystemRevenuePer;             //系统抽水比例(千分之)
        public long TempRevenueValue;              //库存抽水值
        public long TempRevenueLimitValue;         //库存抽水放入库存中的限定值
        public int TempRevenuePer;                  //库存抽水比例 千分比
        public int CurDiff;                        //当前难度
        public uint DiffChangeTime0;               //难度变化周期
        public uint DiffChangeTime1;               //难度变化周期
        public long DiffLimtMin;                   //库存下限
        public long DiffLimtMax;                   //库存上限
        public CMD_GM_StockConfig(CyNetReadBuffer rb)
        {
            Stock = rb.ReadLong();
            SystemRevenue = rb.ReadLong();
            SystemRevenuePer = rb.ReadInt();
            TempRevenueValue = rb.ReadLong();
            TempRevenueLimitValue = rb.ReadLong();
            TempRevenuePer = rb.ReadInt();
            CurDiff = rb.ReadInt();
            DiffChangeTime0 = rb.ReadUint();
            DiffChangeTime1 = rb.ReadUint();
            DiffLimtMin = rb.ReadLong();
            DiffLimtMax = rb.ReadLong();
        }

    };

    /// <summary>
    /// 机器人配置
    /// </summary>
    public struct CMD_GM_RobotConfig
    {
        public uint ActionTime;                        //操作时间
        public uint FirePer;                       //开火几率权重
        public uint ResetPer;                      //休息几率权重
        public uint ChangMultMinTime;              //切换炮台最短时间
        public uint ChangMultMaxTime;              //切换炮台最长时间
        public uint LockMinTime;                   //锁定最短时间
        public uint LockMaxTime;                   //锁定最长时间
        public uint RestTime;                      //休息时间
        public uint CatchValue;                        //打死小鱼的概率
        public uint CatchBossValue;                    //打死Boss的概率
        public CMD_GM_RobotConfig(CyNetReadBuffer rb)
        {
            ActionTime = rb.ReadUint();
            FirePer = rb.ReadUint();
            ResetPer = rb.ReadUint();
            ChangMultMinTime = rb.ReadUint();
            ChangMultMaxTime = rb.ReadUint();
            LockMinTime = rb.ReadUint();
            LockMaxTime = rb.ReadUint();
            RestTime = rb.ReadUint();
            CatchValue = rb.ReadUint();
            CatchBossValue = rb.ReadUint();
        }
    };

    /// <summary>
    /// 鱼种配置
    /// </summary>
    public struct CMD_GM_FishConfig
    {
        public FishBaseConfig[] BaseConfig;                  //鱼配置 [26]

        public CMD_GM_FishConfig(CyNetReadBuffer rb)
        {
            BaseConfig = new FishBaseConfig[26];
            for (int i = 0; i < BaseConfig.Length; i++)
            {
                BaseConfig[i] = new FishBaseConfig(rb);
            }
        }
    };

    public struct FishProbData
    {
        public FishProbConfig[] BaseConfig;                  //一套鱼难度配置[26]
        public FishProbData(CyNetReadBuffer rb)
        {
            BaseConfig = new FishProbConfig[26];
            for (int i = 0; i < BaseConfig.Length; i++)
            {
                BaseConfig[i] = new FishProbConfig(rb);
            }
        }
    };

    /// <summary>
    /// 鱼种难度配置
    /// </summary>
    public struct CMD_GM_FishProb
    {
        public FishProbData[] FishDiffData;               //总的鱼难度配置[5]
        public CMD_GM_FishProb(CyNetReadBuffer rb)
        {
            FishDiffData = new FishProbData[5];
            for (int i = 0; i < FishDiffData.Length; i++)
            {
                FishDiffData[i] = new FishProbData(rb);
            }
        }
    };
    /// <summary>
    /// 自动开火
    /// </summary>
    public struct ChangeAutoState
    {
        public ushort State;//	0取消自动		1自动中...
    }
    //请求GM面板数据
    public struct CMD_GM_C_GetConfig
    {
        public CMD_GM_BaseConfig BaseData;         //游戏基础配置
        public CMD_GM_StockConfig StockData;           //库存配置
        public CMD_GM_RobotConfig RobotData;           //机器人配置
        public CMD_GM_FishConfig FishConfigData;       //鱼基础配置
        public CMD_GM_FishProb FishProbData;       //鱼难度配置
    };
    /// <summary>
    /// 保存GM面板数据（全部保存）
    /// </summary>
    public struct CMD_GM_C_SaveConfig
    {
        public CMD_GM_BaseConfig BaseData;         //游戏基础配置
        public CMD_GM_StockConfig StockData;           //库存配置
        public CMD_GM_RobotConfig RobotData;           //机器人配置
        public CMD_GM_FishConfig FishConfigData;       //鱼基础配置
        public CMD_GM_FishProb FishProbData;       //鱼难度配置
    }
    /// <summary>
    /// 请求GM面板
    /// </summary>
    public struct CMD_GM_C_SendInfo
    {
        public ushort sign;
    }
    /// <summary>
    /// 保存GM面板返回
    /// </summary>
    public struct CMD_GM_S_SaveConfig
    {
        public ushort Success;           //0失败 1成功
    };
    /// <summary>
    /// 刷新库存
    /// </summary>
    public struct CMD_GM_C_GetStock
    {
        public ushort Sign;
    }
    //刷新库存返回
    public struct CMD_GM_S_GetStock                    //SUB_S_GM_GET_STOCK
    {
        public CMD_GM_StockConfig StockData;
    };
    //保存库存
    public struct CMD_GM_C_SaveStock                   //SUB_C_GM_SAVE_STOCK
    {
        public CMD_GM_StockConfig StockData;
    };
    //清理库存
    public struct CMD_GM_C_ClearStock                  //SUB_C_CLEAR_STOCK
    {
        public ushort Sign;
    };
    //库存操作返回
    public struct CMD_GM_S_BackForStock                //SUB_S_BACK_FOR_STOCK
    {
        public ushort Sign;          //  0失败  1保存成功  2清零成功
    };
    /// <summary>
    /// 玩家控制
    /// </summary>
    public struct CMD_GM_C_BlackWhite
    {
        public byte Type;          //0查询  1删除  2添加
        public byte OperateIndex;  //0黑名单  1白名单
        public uint GameID;           //操作的玩家GameID
        public float ControlPer;		//控制概率(附加基值)
    }
    /// <summary>
    /// 玩家
    /// </summary>
    public struct BW_Struct
    {
        public uint GameID;//玩家GameID
        public float ControlPer;		//控制概率(附加基值)
        public BW_Struct(CyNetReadBuffer rb)
        {
            GameID = rb.ReadUint();
            ControlPer = rb.ReadFloat();
        }
    }
    /// <summary>
    /// 所有黑白名单
    /// </summary>
    public struct CMD_GM_S_BlackWhite                  //SUB_S_GM_UPDATE_BW_LIST
    {
        public ushort BlackListNum;          //黑名单数量
        public BW_Struct[] BlackData;   //黑名单内容
        public ushort WhiteListNum;          //白名单数量
        public BW_Struct[] WhiteData;   //白名单内容
        public CMD_GM_S_BlackWhite(CyNetReadBuffer rb)
        {
            try
            {
                BlackListNum = rb.ReadUshort();


                BlackData = new BW_Struct[999];
                for (var i = 0; i < BlackData.Length; i++)
                {
                    BW_Struct vo = new BW_Struct(rb);
                    BlackData[i] = vo;
                }

                WhiteListNum = rb.ReadUshort();

                WhiteData = new BW_Struct[999];
                for (var i = 0; i < WhiteData.Length; i++)
                {
                    BW_Struct vo = new BW_Struct(rb);
                    WhiteData[i] = vo;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

    };
    #endregion
}
