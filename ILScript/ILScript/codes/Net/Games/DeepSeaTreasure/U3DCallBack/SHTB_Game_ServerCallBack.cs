using CYNetwork;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SHTB
{
    public partial class SHTB_
    {
        #region 服务器回调命令

        //服务器命令结构 
        public const ushort SUB_S_GM_RESPONSE = 26;           //配置信息
        public const ushort SUB_S_GM_Get_Stock_RESPONSE = 29;  //获取最新库存信息
        public const ushort SUB_S_GM_Save_RESULT = 31;  //保存配置信息结果

        public const ushort SUB_S_ShowFish_RESPONSE = 33;  //图鉴数据

        public const ushort SUB_S_GM_CurDiff = 35;  //当前难度

        //public const ushort SUB_S_Update_BulletRate = 37;  //更新子弹倍率

        public const ushort SUB_S_Test_Net = 66; //测试网络

        public const ushort SUB_S_BIGHORN_Sys = 68;  //中奖播报 子命令 

        public const ushort SUB_S_PLAYER_JOIN = 782;//玩家进入游戏信息
        public const ushort SUB_S_PLAYER_LEAVE = 783;//玩家退出游戏信息
        /////////////////////////////MAIN_Game/////////////////////////////////////
        //public const ushort SUB_S_FISH = 780;   //加入鱼(群)
        public const ushort SUB_S_LAUNCH_GROUP_FISH = 110;
        public const ushort SUB_S_SYNC_FISH = 789; //异步加入鱼(群)
        public const ushort SUB_S_BULLET = 779; //发子弹
        public const ushort SUB_S_SYNC_BULLET = 791; //发子弹
        public const ushort SUB_S_CHANGE_LAUNCHER = 784;//换炮
        public const ushort SUB_S_CATCHED = 781;//抓鱼
        public const ushort SUB_S_CHANGE_RATE = 787;//切换倍率
        public const ushort SUB_S_SKILL_DISASTER_RESPONSE = 804;//使用技能天灾回调
        public const ushort SUB_S_SKILL_LOCK_RESPONSE = 806; //使用锁定技能回调
        public const ushort SUB_S_SKILL_FREEZE_RESPONSE = 798; //使用冰冻技能回调
        public const ushort SUB_S_SKILL_LASER_RESPONSE = 802; //使用激光技能回调
        public const ushort SUB_S_SKILL_LIGHTING_RESPONSE = 800; //使用闪电技能回调
        public const ushort SUB_S_SKILL_TORNADO_RESPONSE = 796; //使用台风技能回调
        //public const ushort SUB_S_FISH_PACKAGE = 814;   //收到鱼掉落物
        public const ushort SUB_S_LAUNCH_PACKAGE_FISH = 111;   //收到鱼掉落物
        public const ushort SUB_S_BULLET_REDUCTION = 807;//子弹使鱼减速
        public const ushort SUB_S_REDUCTION = 793;//激光炮命中停滞
        public const ushort SUB_S_CLEAR_SCENE = 817;//清场
        public const ushort SUB_S_LAUNCH_LASER_FAILED = 818;//发射激光失败
        public const ushort SUB_S_SKILL_FAILLED = 819;//技能释放失败
        /////////////////////////////MAIN_Table/////////////////////////////////////
        public const ushort SUB_S_JoinTable = 514;//玩家加入桌子配置信息
        public const ushort SUB_S_Other_User_Info = 516; //其他玩家信息
        public const ushort SUB_S_User_Leave = 517;  //其他玩家离开
        public const ushort SUB_S_Me_SeatID = 518; //我的座位ID
        public const ushort SUB_S_Table_Change_Role_Globe = 526; //改变桌子上其他玩家金币（去银行取钱）
        public const ushort SUB_S_Reset_Other_User_Info = 542; //重置其他玩家信息
        public const ushort SUB_S_Leave_Table_ByServer = 543; //被服务器踢了
        /////////////////////////////MAIN_Item/////////////////////////////////////     
        public const ushort SUB_S_Get_User_Item = 1538; //获取用户Item
        public const ushort SUB_S_Add_User_Item = 1540; //添加用户Item
        public const ushort SUB_S_Del_User_Item = 1541; //删除用户Item
        public const ushort SUB_S_Change_User_Item = 1542; //改变用户Item      
        public const ushort SUB_S_On_Use_Item = 1544; //使用Item

        /////////////////////////////MAIN_Role/////////////////////////////////////
        public const ushort SUB_S_ChangeRoleGlobe = 2310;//改变玩家金币
        //public const ushort SUB_S_ChangeRoleMedal = 2311;//改变玩家奖牌
        //public const ushort SUB_S_ChangeRoleCurrency = 2312;//改变玩家钻石
        //public const ushort SUB_S_ChangeRoleTitle = 2313;//改变玩家称号
        //public const ushort SUB_S_ChangeRoleAchievementPoint = 2314;//改变玩家成就点
        public const ushort SUB_S_ChangeRoleTaskStates = 2318;//改变玩家任务状态
        //public const ushort SUB_S_ChangeRoleAchievementStates = 2319;//改变玩家成就状态
        //public const ushort SUB_S_ChangeRoleActionStates = 2320;//改变玩家活动状态
        //public const ushort SUB_S_ChangeRoleAchievementIndex = 2356;//改变玩家成就Index
        public const ushort SUB_S_ResetRoleGlobel = 2357;//重置玩家金币
        public const ushort SUB_S_RESET_ROLE_INFO = 2359;//重置玩家信息
        //public const ushort SUB_S_ChangeRoleExChangeState = 2366;//改变玩家汇率
        //public const ushort SUB_S_ChangeRoleTotalRecharge = 2367;//改变玩家总消费
        //public const ushort SUB_S_ChangeRoleParticularStates = 2383;//改变玩家特殊状态   
        public const ushort SUB_S_ChangeRoleRateValue = 2372;//改变玩家倍率

        /////////////////////////////MAIN_Task/////////////////////////////////////
        public const ushort SUB_S_GetRoleTaskInfo = 3074;  //获取角色任务信息
        public const ushort SUB_S_TaskAllEventFinish = 3077; //角色任务完成
        public const ushort SUB_S_GetTaskReward = 3079;      //获取任务奖励
        public const ushort SUB_S_GetOnceTaskInfo = 3082;//获取一个任务
        public const ushort SUB_S_JoinTask = 3084; //参加一个任务
        public const ushort SUB_S_ClearTask = 3085;//清空任务
        public const ushort SUB_S_DelTask = 3086;//删除任务
        /////////////////////////////MAIN_Month/////////////////////////////////////
        //public const ushort SUB_S_DelTask = 3841;

        /////////////////////////////MAIN_Chest/////////////////////////////////////
        public const ushort SUB_S_ActionChest = 4354;//箱子活动
        public const ushort SUB_S_ChestEnd = 4355;//箱子活动结束  
        public const ushort SUB_S_GetChestReward = 4357;//获得一个箱子     
        public const ushort SUB_S_ResetChest = 4359;//重置箱子
                                                    /////////////////////////////MAIN_GameData/////////////////////////////////////

        public const ushort SUB_S_GendGameData = 6658; //玩家数据
        /////////////////////////////Main_Launcher/////////////////////////////////////
        public const ushort SUB_S_Launcher_Data = 7169;//炮台数据

        /////////////////////////////Main_RoleMessage/////////////////////////////////////
        public const ushort SUB_S_RoleMessageChange = 8449;//角色消息改变  
        /////////////////////////////Main_Lottery/////////////////////////////////////
        public const ushort SUB_S_GetLotteryReward = 8962;//彩金奖励(得奖) 
        public const ushort SUB_S_LotteryUIStates = 8964;//彩金界面状态
        public const ushort SUB_S_LotteryUIData = 8967;// 彩金界面数据
        #endregion



        public const ushort SUB_GR_USER_INSURE_SUCCESS = 101;                               //银行成功
        public const ushort SUB_GR_USER_INSURE_FAILURE = 102;                               //银行失败

        #region 服务器返回的结构体

        public class JoinRoomData
        {
            public byte Result;
            public byte RoomID;
            public uint RoomRateUser; //房间兑换比率A
            public uint RoomRateFish; //房间兑换比率B
            public uint[][] ChipConfig; //碎片配置  凝霜  穿云 2-2 当前数量 最大数量 时间
            public ushort Seat;
            public byte LauncherType;
            public byte BackgroundImage;
            public byte RateIndex;
            public int Energy;
            public int MaxEnergy;
            public byte MaxLotteryFishSum;   //最大彩金鱼数量
            public float[][] LauncherData;  //炮台数据   0  开火间隔  1 速度
            public uint[] BulletRateArr;
        }

        #region Main_Game
        public class NetCmdPlayerJoin
        {
            public ushort Seat;
            public byte LauncherType;   //最高位是否有效,后七位是炮台索引
            public byte rateIndex;
            public PlayerBaseInfo PlayerInfo;
        }

        public class PlayerBaseInfo
        {
            public uint ID;
            public byte Lvl;
            public uint ImgCrc;
            public bool Sex;
            public long GoldNum;
            public string Name;
        };

        public class NetCmdPlayerLeave
        {
            public ushort Seat;
        }

        public class LC_Cmd_ResetRoleInfo
        {
            public UInt32 OperateIP;
            public tagRoleInfo RoleInfo;
        }

        public class tagRoleInfo
        {
            public UInt32 dwUserID;
            public string NickName;
            public UInt16 wLevel;//玩家点击
            public UInt32 dwExp;//玩家经验
            public UInt32 dwFaceID;//玩家头像ID
            public bool bGender; //玩家性别
            public long gold;
            public UInt32 dwMedalNum;
            public UInt32 dwCurrencyNum;  //钻石
            public UInt32 dwAchievementPoint;
            public UInt16 dwChairID;//椅子ID;
            public UInt32 TitleID;//现改为桌子ID
            public UInt32 dwProduction;//当天获得的金币数量
            public UInt32 dwGameTime;//当天的游戏时间
            public Byte SendGiffSum;//发送赠送的次数 当天
            public Byte AcceptGiffSum;//接收赠送的次数 当天
            public int256 TaskStates;
            public int256 AchievementStates;
            public int256 ActionStates;
            public UInt16 OnlineMin;
            public UInt32 OnlineRewardStates;
            public UInt32[] CharmArray;
            public bool IsCanResetAccount;
            public UInt32 AchievementPointIndex;
            public UInt32 ClientIP;
            public string IPAddress;
            public UInt32 CheckData;
            public bool IsShowIPAddress;//默认值为true
            public UInt32 ExChangeStates;
            public UInt32 TotalRechargeSum;
            public bool bIsFirstPayGlobel;
            public bool bIsFirstPayCurrcey;
            public UInt32 LotteryScore;
            public Byte LotteryFishSum;
            //Vip数据
            public Byte VipLevel;//不涉及数据库的
            //月卡数据
            public Byte MonthCardID;
            public Int64 MonthCardEndTime;
            public Int64 GetMonthCardRewardTime;
            //倍率
            public int256 RateValue;//开启的倍率的数值
            public byte CashSum;
            public Byte benefitCount;//
            public UInt32 benefitTime;//
            public UInt32 TotalUseMedal;//
            public UInt32 ParticularStates;//
            public UInt32 GameID;//
            public bool bShareStates;//
            public UInt32 TotalCashSum;//
        }

        public class tagShowFish
        {
            public ushort[][] FishData;  // 0  最小价值  1 最大价值   2 是不是奖金鱼
        }

        public class int256
        {
            public Byte[] Value;
        }

        //public class CMD_S_CreateFishGroup
        //{
        //    public ushort GroupID;
        //    public ushort PathID;
        //    public ushort StartID;
        //    public uint tick; //收到命令的时间
        //}

        public class NetCmdSyncFish
        {
            public ushort FishID;
            public ushort GroupID;
            public float FishTime;
            public ushort PathGroup;
            public ushort PathIdx;
            public bool IsActiveEvent;
            public ushort ElapsedTime;
            public byte DelayType;
            public byte DelayScaling;	//减速百分比
            public byte DelayDuration1;
            public byte DelayDuration2;
            public byte DelayDuration3;
            public ushort DelayCurrentTime;
            public byte Package;
            public uint tick;
        }

        public class NetCmdBullet
        {
            public ushort BulletID;
            public short Degree;
            public byte LauncherType;
            public uint Energy;
            public byte ReboundCount;
            public ushort LockFishID;
            public uint tick;
        }

        public class NetCmdSyncBullet
        {
            public byte Num;//当前子弹数量
            public SyncBulletData[] Bullets;
            public uint tick;
        }

        public class SyncBulletData
        {
            public ushort BulletID;
            public short Degree;
            public ushort Time;
            public byte BulletType;
            public byte RateIdx;
            public byte ReboundCount;
            public ushort LockFishID;
            /*
            public byte     CollideCount;
            public ushort   PauseTime;
            public ushort   SpeedScaling;
            */
        }

        public class NetCmdChangeLauncher
        {
            public ushort Seat;
            public byte LauncherType;   //发送时：1表示向上加，0表示向下减。接收时：最高位表示是否有效。
        }

        //public class NetCmdCatched
        //{
        //    public ushort BulletID;
        //    public ushort Combo;
        //    public uint Nrank;//鱼的等级
        //    public uint GoldNum;
        //    public uint Handsel;	//获得的彩金基值
        //    public uint mosaicgold; //彩金
        //    public uint TotalNum;
        //    public ushort Seat;
        //    public long UserScore;
        //    public NetFishCatched[] Fishs;
        //    public uint tick;
        //}

        //public class NetFishCatched
        //{
        //    public byte CatchEvent;
        //    public int beilv;
        //    public ushort FishID;
        //    public ushort nReward;
        //    public ushort LightingFishID;
        //}

        public class NetCmdChangeRate
        {
            public ushort Seat;
            public byte RateIndex;
            public byte LauncherType;//炮台类型
            public bool IsCanUseRate;//当前的倍率玩家是否可以使用 发送的时候 无须携带
        }

        public class NetCmdSkillDisaster
        {
            public ushort Seat;
            public uint GoldNum;
            public uint Handsel;	//获得的彩金基值
            public uint mosaicgold; //彩金
            public uint TotalNum;
            public long UserScore;
            public NetFishDeadTime[] FishID;
            public uint tick;
        }

        public class NetFishDeadTime
        {
            public ushort FishID;
            public byte DeadTime;
            public ushort nReward;
            public ushort LightingFishID;
            public int beilv;//当前鱼倍率
        }

        public class NetCmdSkillLock
        {
            public ushort Seat;
        }

        public class NetCmdSkillFreeze
        {
            public ushort Seat;
            public uint GoldNum;
            public uint Handsel;	//获得的彩金基值
            public uint mosaicgold; //彩金
            public uint TotalNum;
            public long UserScore;
            public NetFishDeadTime[] FishID;
            public uint tick;
        }

        public class NetCmdSkillLaser
        {
            public short Degree;
        }

        public class NetCmdSkillLaserResponse
        {
            public ushort Seat;
            public long UserScore;
            public byte LaserType;
            public short Degree;
            public uint GoldNum;
            public uint Handsel;	//获得的彩金基值
            public uint mosaicgold; //彩金
            public uint TotalNum;
            public ushort[] FishType;//[3];      
            public NetFishDeadTime[] FishID;
            public uint tick;
        }

        public class NetCmdSkillLighting
        {
            public ushort Seat;
            public long UserScore;
            public uint GoldNum;
            public uint Handsel;	//获得的彩金基值
            public uint mosaicgold; //彩金
            public uint TotalNum;
            public NetFishDeadTime[] FishID;
            public uint tick;
        }

        public class NetCmdSkillTornado
        {
            public ushort Seat;
            public uint GoldNum;
            public uint Handsel;	//获得的彩金基值
            public uint mosaicgold; //彩金
            public uint TotalNum;
            public long UserScore;
            public NetFishDeadTime[] FishID;
            public uint tick;
        }

        public class CMD_S_CreateFishPackage
        {
            public ushort FishID;
            public byte PackageType;
        }

        //public class NetCmdBulletReduction
        //{
        //    public ushort BulletID;
        //    public ushort Combo;
        //    public uint Nrank;//鱼的等级
        //    public uint GoldNum;
        //    public uint Handsel;	//获得的彩金基值
        //    public uint mosaicgold; //彩金
        //    public uint TotalNum;
        //    public ushort Seat;
        //    public long UserScore;
        //    public NetFishCatched[] FishID;
        //    public uint tick;
        //}

        public class NetCmdReduction
        {
            public byte ReductionType;// 1: Laser,
            public byte LaserType;
            public uint TotalNum;
            public ushort[] FishID;

            public uint tick;
        }

        public class NetCmdClearScene
        {
            public byte ClearType;      //0从左到右，1从右到左，2重连清场。
            public uint tick;
        }

        public class NetCmdLaunchFailed
        {
            public byte FailedType;
            public uint Energy;
            public uint MaxEnergy;
        }

        public class NetCmdSkillFailed
        {
            public byte FailedType;
            public byte bySkillType;
            public ushort uTimes;
        }


        public class NetCmdVector2
        {
            public NetCmdVector2()
            {

            }
            public NetCmdVector2(float _x, float _y)
            {
                x = _x;
                y = _y;
            }
            public NetCmdVector2(UnityEngine.Vector2 pos)
            {
                x = pos.x;
                y = pos.y;
            }
            public float x;
            public float y;
        }
        public class NetCmdVector3
        {
            public NetCmdVector3()
            {

            }
            public NetCmdVector3(float _x, float _y, float _z)
            {
                x = _x;
                y = _y;
                z = _z;
            }
            public NetCmdVector3(UnityEngine.Vector3 pos)
            {
                x = pos.x;
                y = pos.y;
                z = pos.z;
            }
            public float x;
            public float y;
            public float z;
        }
        public class NetCmdVector4
        {
            public NetCmdVector4()
            {

            }
            public NetCmdVector4(float _x, float _y, float _z, float _w)
            {
                x = _x;
                y = _y;
                z = _z;
                w = _w;
            }
            public NetCmdVector4(UnityEngine.Vector4 pos)
            {
                x = pos.x;
                y = pos.y;
                z = pos.z;
                w = pos.w;
            }
            public NetCmdVector4(UnityEngine.Quaternion pos)
            {
                x = pos.x;
                y = pos.y;
                z = pos.z;
                w = pos.w;
            }
            public float x;
            public float y;
            public float z;
            public float w;
        }

        public class CheckFishPos
        {
            public ushort FishID;
            public float Time;
            public float Speed;
            public NetCmdVector3 Pos;
            public NetCmdVector4 Rot;
        }
        public class NetCmdCheckFishPos
        {
            public ushort FishNum;
            public CheckFishPos[] Fish;
        }
        public class CheckBulletPos
        {
            public ushort ID;
            public NetCmdVector3 Pos;
        }
        public class NetCmdCheckBulletPos
        {
            public ushort Count;
            public CheckBulletPos[] Bullets;
        }

        #endregion
        #region Main_Table
        public class LC_Cmd_OtherUserInfo
        {
            public tagRemoteRoleInfo UserInfo;
        }

        public class tagRemoteRoleInfo
        {
            public ushort dwChairID;
            public UInt32 dwUserID;
            public string NickName;
            public UInt16 wLevel;//玩家点击
            public UInt32 dwFaceID;//玩家头像ID
            public bool bGender; //玩家性别
            public long gold; //玩家金币
            public UInt32[] CharmArray;
            public UInt32 dwAchievementPoint;
            public UInt32 TitleID;  //现改为桌子ID
            public string IPAddress;
            public Byte VipLevel;
            public bool IsInMonthCard;
            public UInt32 GameID;
        }

        public class LC_Cmd_OtherUserLeave
        {
            public UInt32 dwUserID;
        }
        public class LC_Cmd_ResetOtherUserInfo
        {
            public tagRemoteRoleInfo UserInfo;
        }

        public class LC_Cmd_MeSeatID
        {
            public ushort SeatID;
        }

        public class LC_Cmd_LeaveTableByServer
        {
            public bool IsReturnLogon;
        }

        public class LC_Cmd_TableChangeRoleGlobe
        {
            public UInt32 dwDestUserID;
            public long dwGlobelSum;
        }
        #endregion

        #region Main_Item
        public class LC_Cmd_GetUserItem
        {
            public Byte States;
            public UInt16 Sum;
            public tagItemInfo[] Array;
        }
        public class LC_Cmd_AddUserItem
        {
            public tagItemInfo ItemInfo;
        }

        public class tagItemInfo//玩家具体的数据
        {
            public UInt32 ItemOnlyID;//物品的唯一ID  主键ID
            public UInt32 ItemID;//物品ID
            public UInt32 ItemSum;//物品数量
            public Int64 EndTime;//物品结束时间
        };

        public class LC_Cmd_DelUserItem
        {
            public UInt32 ItemOnlyID;
        }
        public class LC_Cmd_ChangeUserItem
        {
            public UInt32 ItemOnlyID;
            public UInt32 ItemSum;
        };

        public class LC_Cmd_OnUseItem
        {
            public bool Result;
            public UInt32 ItemOnlyID;
            public UInt32 ItemID;
            public UInt32 ItemSum;
        }
        #endregion
        #region Main_Role
        public class LC_Cmd_ChangeRoleGlobe
        {
            public Int32 dwGlobeNum;
        }

        public class LC_Cmd_ChangeRoleTaskStates
        {
            public Byte Index;
            public bool States;
        }

        public class LC_Cmd_ResetRoleGlobel
        {
            public long TotalGlobelSum;
            public UInt32 LotteryScore;
        }

        public class LC_Cmd_ChangeRoleRateValue
        {
            public int256 RateValue;
            public Byte OpenRateIndex;
        }

        #endregion

        #region Main_Task
        public class LC_Cmd_GetRoleTaskInfo
        {
            public Byte States;
            public UInt16 Sum;
            public tagRoleTaskInfo[] Array;
        }

        public class tagRoleTaskInfo//任务的结构数据 玩家的任务结构
        {
            public Byte TaskID;//任务ID 从0到 256  可以直接建立数组
            public UInt32 TaskValue;
        }

        public class LC_Cmd_TaskAllEventFinish
        {
            public Byte TaskID;
        }

        public class LC_Cmd_GetTaskReward
        {
            public Byte TaskID;
        }


        public class LC_Cmd_GetOnceTaskInfo
        {
            public tagRoleTaskInfo TaskInfo;
        }
        public class LC_Cmd_JoinTask
        {
            public Byte TaskID;
        }

        public class LC_Cmd_DelTask
        {
            public Byte TaskID;
        }

        #endregion

        #region Main_Chest
        public class LC_Cmd_ActionChest
        {
            public bool IsReset;
            public Byte ChestOnlyID;
            public Byte ChestTypeID;
            public Byte ChestSum;
            public Byte OpenSum;
            public UInt32 EndSec;
            public ChestOnceStates[] ChestArray;
        }
        public class LC_Cmd_ChestEnd
        {
            public Byte ChestOnlyID;
        }

        public class LC_Cmd_GetChestReward
        {
            public bool Result;
            public Byte ChestOnlyID;
            public ChestOnceStates ChestStates;
        }

        public class ChestOnceStates
        {
            public Byte Index;
            public Byte RewardID;
            public Byte RewardOnlyID;
        }
        #endregion

        #region Main_GameData
        public class tagRoleGameData
        {
            public UInt32 RoleMonthSigupSum;//玩家报名比赛的次数
            public UInt32 RoleMonthRewardSum;//
            public UInt32 RoleMonthFirstSum;
            public UInt32 RoleMonthSecondSum;
            public UInt32 RoleMonthThreeSum;
            public UInt32 RoleCatchFishTotalSum;
            public long RoleGetGlobelSum;
            public UInt32 NonMonthGameSec;//玩家进行非比赛游戏时间的秒杀
            public UInt32 TotalGameSec;//玩家进行总游戏时间的秒数
            public UInt32 CatchFishSum_9;//玩家捕获指定类型鱼的数量
            public UInt32 CatchFishSum_18;
            public UInt32 CatchFishSum_20;
            public UInt32 CatchFishSum_1;
            public UInt32 CatchFishSum_3;
            public UInt32 CatchFishSum_19;
            public UInt32 MaxCombSum;
        }

        public class LC_Cmd_GendGameData
        {
            public tagRoleGameData GameData;
        }
        #endregion

        #region Main_Launcher
        public class LC_Cmd_LauncherData
        {
            public UInt32 LauncherData;
        }
        #endregion

        #region Main_RoleMessage
        public class LC_Cmd_RoleMessageChange
        {
            public UInt32 RoleMessageData;
        }
        #endregion

        #region Main_Lottery

        public class LC_Cmd_GetLotteryReward
        {
            public Byte LotteryID;
            public UInt16 RewardID;
            public bool Result;
        }

        public class LC_Cmd_LotteryUIStates
        {
            public UInt32 dwUserID;
            public bool IsOpen;
        }

        public class LC_Cmd_LotteryData
        {
            public uint[] Money;//彩金数据[6]
        }
        #endregion

        #region GM_Console
        /// <summary>
        /// 控制台配置
        /// </summary>
        public class CMD_GM_S_ConsoleConfig
        {
            public CMD_GM_S_BaseConfig BaseConfig;
            public CMD_GM_S_StockConfig StockConfig;
            public CMD_GM_S_PlayerControl PlayerControl;
            public CMD_GM_S_RobotConfig RobotConfig;
            public CMD_GM_S_FishConfig FishConfig;
            public CMD_GM_S_LotteryConfig LotteryConfig;
        }
        /// <summary>
        /// 保存配置结果
        /// </summary>
        public class CMD_GM_S_SaveResult
        {
            public bool isSucced;
        }

        /// <summary>
        /// 当前难度更新
        /// </summary>
        public class CMD_GM_S_UpdateCurDif
        {
            public byte CurDif;
        }


        /// <summary>
        /// 基本配置
        /// </summary>
        public class CMD_GM_S_BaseConfig
        {
            public uint ChangeRate_0;        //兑换比例
            public uint ChangeRate_1;
            public uint SceneChangeTime;     //场景切换时间
            public uint BulletCount;         //子弹数量
            public double CatchBase_0;         //捕获基值
            public double CatchBase_1;
            public uint FireIntev;           //开火间隔(毫秒)
            public uint XPSkillCount;        //激光炮需要子弹数量
            public uint XPSAddPer;           //激光炮增加捕获概率百分比
            public uint GetCannon3_Need;     //凝霜所需碎片
            public uint Cannon3_CD;          //凝霜使用时限
            public uint GetCannon4_Need;     //穿云所需碎片
            public uint Cannon4_CD;          //穿云使用时限
            public int BulletMultArrLen; //子弹倍数数组长度
            public byte MultArrMin;    //子弹倍率显示的最小索引
            public byte MultArrMax;    //子弹倍率显示的最大索引
            public tagBulletMultAndType[] BulletMultArr;   //子弹倍数以及炮台种类  //长度不确定
            public tagBulletConfig[] BulletArr;         //炮台子弹属性  长度为5
        }



        /// <summary>
        /// 库存配置
        /// </summary>
        public class CMD_GM_S_StockConfig
        {
            public long Stock;               //库存
            public long Pump;                //抽水
            public double PumpPer;             //抽水比例(千分之)
            public long tempRevenueValue;        //库存抽水值
            public long limitTempRevenueValue;   //库存抽水放入库存中的限定值
            public double tempRevenuePer;          //库存抽水比例 千分比
            public int CurDiff;             //当前难度
            public uint DiffChangeTime_0;    //难度变化周期
            public uint DiffChangeTime_1;    //难度变化周期
            public long DiffLimt_Min;        //库存下限
            public long DiffLimt_Max;        //库存上限
        }
        /// <summary>
        /// 玩家控制
        /// </summary>
        public class CMD_GM_S_PlayerControl
        {
            public int BlackArrLen; //黑名单长度
            public tagBlackInfo[] BlackArr;   //黑名单
            public int WhiteArrLen; //白名单长度
            public tagWhiteInfo[] WhiteArr;   //白名单
            public int ProbArrLen; //黑名单长度
            public tagProbInfo[] ProbArr;    //概率控制
            public int DiffArrLen; //难度控制长度
            public tagDiffInfo[] DiffArr;     //难度控制
            public int BigFishArrLen; //赠送大鱼长度
            public tagGiveBigFish[] BigFishArr;     //赠送大鱼
        }
        /// <summary>
        /// 机器人配置
        /// </summary>
        public class CMD_GM_S_RobotConfig
        {
            public uint FireInv_0;           //开火间隔
            public uint FireInv_1;
            public uint FireAngleChange_0;   //开火角度变化
            public uint FireAngleChange_1;
            public uint ChangMult_0;         //子弹切换频率
            public uint ChangMult_1;
            public uint FireToRestTime_0;          //开火到休息间隔
            public uint FireToRestTime_1;
            public uint RestToFireTime_0;          //休息到开火间隔
            public uint RestToFireTime_1;
            public float CatchValue_0;        //捕获基值
            public float CatchValue_1;
            public uint GameTime_0;          //游戏频率
            public uint GameTime_1;

            public long Socre_Min;           //取款下限
            public long Socre_Max;           //取款上限
            public double Socre_Per;           //现金百分比
        }
        /// <summary>
        /// 鱼种配置
        /// </summary>
        public class CMD_GM_S_FishConfig
        {
            public tagFishConfigArr[] FishConfigList; //长度为5 表示每种难度 对应的 鱼种属性
        }

        /// <summary>
        /// 抽奖配置
        /// </summary>
        public class CMD_GM_S_LotteryConfig
        {
            public uint Handsel_Base;                         //彩金基值
            public uint Handsel_Count;                        //彩金需击杀
            public double[] Handsel_1Per_0;                       //彩金
            public double[] Handsel_1Per_1;                       //彩金抽中几率
        }

        public class tagBulletMultAndType
        {
            public uint Mult;           //子弹倍数
            public int Type;            //炮台种类
        }

        public class tagBulletConfig
        {
            public int Type;           //炮台种类
            public float Speed;          //子弹速度
            public int CatchRang;      //捕获范围
            public float FireIntev;      //开炮间隔
        }

        public class tagBlackInfo
        {
            public uint GameID;
            public long EatScore;          //吃分
        }

        public class tagWhiteInfo
        {
            public uint GameID;
            public long GiveScore;     //送分
        }

        public class tagProbInfo
        {
            public uint GameID;
            public float CatchBase;   //捕获基值
        }

        public class tagDiffInfo
        {
            public uint GameID;
            public int DiffType;     //难度类型
        }

        public class tagGiveBigFish
        {
            public uint GameID;
            public int Type;            //鱼的类型
            public float CatchValue;    //捕获概率
            public uint Num;            //赠送数量
        }

        public class tagFishConfigArr
        {
            public int FishConfigArrLen; //数组长度
            public tagFishConfig[] FishConfigArr; //鱼的属性数组
        }

        public class tagFishConfig
        {
            public int ID;             //ID
            public int typeValue;     //鱼种
            public int multMin;       //最小倍数
            public int multMax;       //最大倍数
            public float speed;        //速度
            public int invMin;        //最小间隔
            public int invMax;        //最大间隔
            public int numMin;        //最小数量
            public int numMax;        //最大数量
            public float catchValue;   //捕获概率
            public float dropCan3Value; //掉落凝霜
            public float dropCan4Value; //掉落穿云
            public int isGoodFish;      //是不是奖金鱼
        }

        public class CMD_S_Stock_Score
        {
            public double lStock_Score;
            public double lPump_Score;
            public double Pump_Per;
            public double tempRevenueValue;         //库存抽水值
            public double limitTempRevenueValue;    //库存抽水放入库存中的限定值
            public double tempRevenuePer;           //库存抽水比例 千分比
        }

        public class CMD_S_Update_BulletRate
        {
            public ushort[] BulletRateArr;
        }
        #endregion

        public struct CMD_GR_S_UserInsureSuccess
        {
            public byte cbActivityGame;                     //游戏动作
            public long lUserScore;							//身上金币
            public long lUserInsure;						//银行金币
            public string szDescribeString;				//描述消息128
        };

        //银行失败
        public struct CMD_GR_S_UserInsureFailure
        {
            public byte cbActivityGame;                     //游戏动作
            public long lErrorCode;							//错误代码
            public string szDescribeString;				//描述消息128
        };

        public struct tagBIGHORN
        {
            public string NickName;    //玩家昵称
            public uint FishID;         //鱼ID
            public uint MultRate;       //倍率
            public long lScore;        //得分  
        };
        #endregion
        private bool HandleGameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            //Debug.Log(string.Format("深海捕鱼S2C 主协议:{0} 子协议:{1}", maincmd, subCmd));

            if (SHTBInterFace == null)
            {
                return false;
            }

#if UNITY_EDITOR
            try
            {
#endif

                switch (subCmd)
                {
                    case S2CMsgId.SUB_S_HEART_BEAT:
                        return HeartBeatHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_INIT_GAME_DATA:
                        return InitGameDataHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_INIT_SCENE_FISH:
                        return InitSceneFishHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_INIT_SCENE_BULLET:
                        return InitSceneBulletHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_PLAYER_JOIN:
                        return PlayerJoinHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_PLAYER_LEAVE:
                        return PlayerLeaveHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_CHANGE_BULLET:
                        return ChangeBulletHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_BULLET_FIRE:
                        return BulletFireHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_LOCK_FISH:
                        return LockFishHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_BACK_AUTO_STATE:
                        return AutoState(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_OPEN_SPECIAL_CANON:
                        return OpenSpecialCanonHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_CLEAR_SCENE:
                        return ClearSceneHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_LAUNCH_GROUP_FISH:
                        return LaunchGroupFishHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_CATCH_FISH:
                        return CatchFishHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_FIALED_CODE:
                        return FialedCodeHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_LASER_BACK:
                        return LaserBackHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_SHOW_FISH_REQUEST:
                        return ShowFishRequestHandler(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_GM_GET_CONFIG:
                        return ShowGMInfoBack(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_GM_SAVE_CONFIG:
                        return SaveGMInfoBack(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_GM_GET_STOCK:
                        return RepertoryRefreshBack(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_BACK_FOR_STOCK:
                        return RepertoryBack(maincmd, subCmd, readbuffer);
                    case S2CMsgId.SUB_S_GM_UPDATE_BW_LIST:
                        return RepertoryBlackWhite(maincmd, subCmd, readbuffer);
                    default:
                        Error("深海捕鱼 未能解析的游戏子命令：" + subCmd);
                        break;
                }

#if UNITY_EDITOR
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
#endif

            return true;
        }

        #region 新加协议
        bool HeartBeatHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_CS_HeartBeat msg = new CMD_CS_HeartBeat();
            msg.Sign = rb.ReadUshort();
            if (SHTBInterFace != null)
            {
                SHTBInterFace.HeartBeatResponse(msg);
            }
            return true;
        }

        bool InitGameDataHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_InitGameData msg = new CMD_S_InitGameData();
            msg.ChairID = rb.ReadUshort();
            msg.BackgroundImage = rb.ReadByte();
            msg.Energy = rb.ReadInt();
            msg.MaxEnergy = rb.ReadInt();
            msg.FrostNowNum = rb.ReadInt();
            msg.FrostNeedMax = rb.ReadInt();
            msg.FrostLeftTime = rb.ReadUint();
            msg.CrossNowNum = rb.ReadInt();
            msg.CrossNeedMax = rb.ReadInt();
            msg.CrossLeftTime = rb.ReadUint();
            msg.MaxBulletType = rb.ReadUshort();
            msg.BulletData = new BulletBaseData[9];
            for (var i = 0; i < msg.BulletData.Length; i++)
            {
                BulletBaseData bbd = new BulletBaseData();
                bbd.BulletType = rb.ReadUshort();
                bbd.Value = rb.ReadLong();
                bbd.CanonType = rb.ReadUshort();
                bbd.Speed = rb.ReadFloat();
                bbd.FireIntev = rb.ReadFloat();
                msg.BulletData[i] = bbd;
            }

            //Debug.Log("游戏初始化协议");
            if (SHTBInterFace != null)
            {
                SHTBInterFace.InitGameDataResponse(msg);
            }
            return true;
        }


        bool InitSceneFishHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_InitSceneFish msg = new CMD_S_InitSceneFish();
            msg.Fishes = new RealTimeFish[rb.ReadUshort()];
            for (var i = 0; i < msg.Fishes.Length; i++)
            {
                RealTimeFish vo = new RealTimeFish();
                vo.FishID = rb.ReadUshort();
                vo.GroupID = rb.ReadUshort();
                vo.FishTime = rb.ReadFloat();
                vo.PathGroup = rb.ReadUshort();
                vo.PathIdx = rb.ReadUshort();
                vo.IsActiveEvent = rb.ReadBoolean();
                vo.ElapsedTime = rb.ReadUshort();
                vo.DelayType = rb.ReadByte();
                vo.DelayScaling = rb.ReadByte();
                vo.DelayDuration1 = rb.ReadByte();
                vo.DelayDuration2 = rb.ReadByte();
                vo.DelayDuration3 = rb.ReadByte();
                vo.DelayCurrentTime = rb.ReadUshort();
                msg.Fishes[i] = vo;
            }

            if (SHTBInterFace != null)
            {
                SHTBInterFace.InitSceneFishResponse(msg);
            }
            return true;
        }

        bool InitSceneBulletHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_InitSceneBullet msg = new CMD_S_InitSceneBullet();
            msg.PlayerNum = rb.ReadUshort();
            msg.playerData = new PlayerBaseData[msg.PlayerNum];
            for (var i = 0; i < msg.playerData.Length; i++)
            {
                PlayerBaseData vo = new PlayerBaseData();
                vo.UesrId = rb.ReadUint();
                vo.ChairID = rb.ReadUshort();
                vo.PlayerName = rb.ReadString(64);
                vo.FaceId = rb.ReadUint();
                vo.NowScore = rb.ReadLong();
                vo.BulletType = rb.ReadUshort();
                vo.CanonType = rb.ReadUshort();
                msg.playerData[i] = vo;
            }

            msg.CurrentNum = rb.ReadUshort();
            msg.Bullets = new RealTimeBullet[msg.CurrentNum];
            for (var i = 0; i < msg.Bullets.Length; i++)
            {
                RealTimeBullet vo = new RealTimeBullet();
                vo.BulletID = rb.ReadUshort();
                vo.Degree = rb.ReadShort();
                vo.Time = rb.ReadUshort();
                vo.BulletType = rb.ReadUshort();
                vo.CanonType = rb.ReadUshort();
                vo.LockFishID = rb.ReadUshort();
                msg.Bullets[i] = vo;
            }

            if (SHTBInterFace != null)
            {
                SHTBInterFace.InitSceneBulletResponse(msg);
            }
            return true;
        }

        bool PlayerJoinHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_PlayerJoin msg = new CMD_S_PlayerJoin();
            msg.ChairID = rb.ReadUshort();
            msg.UserID = rb.ReadUint();
            msg.NickName = rb.ReadString(64);
            msg.FaceID = rb.ReadUint();
            msg.BringGold = rb.ReadLong();
            msg.BulletType = rb.ReadUshort();
            msg.CanonType = rb.ReadUshort();
            msg.BulletValue = rb.ReadLong();
            //Debug.Log(string.Format("收到玩家进入协议:{0}", MiniJSON.jsonEncode(msg)));
            if (SHTBInterFace != null)
            {
                SHTBInterFace.PlayerJoinResponse(msg);
            }
            return true;
        }

        bool PlayerLeaveHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_PlayerLeave msg = new CMD_S_PlayerLeave();
            msg.ChairID = rb.ReadUshort();
            //Debug.Log(string.Format("收到玩家离开协议:{0}", MiniJSON.jsonEncode(msg)));
            if (SHTBInterFace != null)
            {
                SHTBInterFace.PlayerLeaveResponse(msg);
            }
            return true;
        }

        bool ChangeBulletHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_ChangeBullet msg = new CMD_S_ChangeBullet();
            msg.ChairID = rb.ReadUshort();
            msg.BulletType = rb.ReadUshort();
            msg.CanonType = rb.ReadUshort();
            msg.BulletValue = rb.ReadLong();
            //Debug.Log(string.Format("收到换炮台协议:{0}", JsonUtility.ToJson(msg)));
            if (SHTBInterFace != null)
            {
                SHTBInterFace.ChangeBulletResponse(msg);
            }
            return true;
        }

        bool BulletFireHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_RequestFire msg = new CMD_S_RequestFire();
            msg.ChairID = rb.ReadUshort();
            msg.BulletValue = rb.ReadLong();
            msg.BulletID = rb.ReadUshort();
            msg.Angle = rb.ReadShort();
            msg.CanonType = rb.ReadUshort();
            msg.Energy = rb.ReadUint();
            msg.LockFishID = rb.ReadUshort();
            //Debug.Log(string.Format("收到开火协议:{0}", JsonUtility.ToJson(msg)));

            if (SHTBInterFace != null)
            {
                SHTBInterFace.BulletFireResponse(msg);
            }
            return true;
        }

        bool LockFishHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_ChangeLock msg = new CMD_S_ChangeLock();
            msg.ChairID = rb.ReadUshort();
            msg.Action = rb.ReadUshort();
            msg.LockFishID = rb.ReadUshort();
            //Debug.Log(string.Format("收到开锁定鱼协议:{0}", JsonUtility.ToJson(msg)));
            if (SHTBInterFace != null)
            {
                SHTBInterFace.LockFishResponse(msg);
            }
            return true;
        }
        bool AutoState(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_ChangeAutoState ms = new CMD_ChangeAutoState();
            ms.ChairID = rb.ReadUshort();
            ms.State = rb.ReadUshort();
            if (SHTBInterFace != null)
            {
                SHTBInterFace.AutoState(ms);
            }
               
            return true;
        }
        bool OpenSpecialCanonHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_TurnOnSpecialCanon msg = new CMD_S_TurnOnSpecialCanon();
            msg.ChairID = rb.ReadUshort();
            msg.BulletType = rb.ReadUshort();
            msg.CanonType = rb.ReadUshort();
            msg.BulletValue = rb.ReadLong();
            msg.Time = rb.ReadUint();
            //Debug.Log(string.Format("收到开启特殊炮协议:{0}", MiniJSON.jsonEncode(msg)));
            if (SHTBInterFace != null)
            {
                SHTBInterFace.OpenSpecialCanonResponse(msg);
            }
            return true;
        }

        bool ClearSceneHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_ClearScene msg = new CMD_S_ClearScene();
            msg.ClearType = rb.ReadByte();
            if (SHTBInterFace != null)
            {
                SHTBInterFace.ClearScene(msg);
            }
            return true;
        }

        bool LaunchGroupFishHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_CreateFishGroup msg = new CMD_S_CreateFishGroup();
            msg.GroupID = rb.ReadUshort();
            msg.PathID = rb.ReadUshort();
            msg.StartID = rb.ReadUshort();

            if (SHTBInterFace != null)
            {
                SHTBInterFace.LaunchGroupFishResponse(msg);
            }
            return true;
        }

        bool CatchFishHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_BulletActionHandler msg = new CMD_S_BulletActionHandler();
            msg.BulletID = rb.ReadUshort();
            msg.GoldNum = rb.ReadUint();
            msg.ChairID = rb.ReadUshort();
            msg.ThisKillData = new KillFishData(rb);
            //Debug.Log(string.Format("收到捕获鱼协议:{0}    杀鱼数据:{1}", JsonUtility.ToJson(msg), JsonUtility.ToJson(msg.ThisKillData)));
            if (SHTBInterFace != null)
            {
                SHTBInterFace.CatchFishResponse(msg);
            }
            return true;
        }

        bool FialedCodeHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_FailedCode msg = new CMD_S_FailedCode();
            msg.ErrorCode = (ErrorCodeEnum)rb.ReadUshort();

            if (SHTBInterFace != null)
            {
                SHTBInterFace.FialedCodeResponse(msg);
            }
            return true;
        }

        bool LaserBackHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_LaserActionHandler msg = new CMD_S_LaserActionHandler();
            msg.LaserType = rb.ReadUshort();
            msg.GoldNum = rb.ReadUint();
            msg.ChairID = rb.ReadUshort();
            msg.Degree = rb.ReadShort();
            msg.ThisKillData = new KillFishData(rb);

            if (SHTBInterFace != null)
            {
                SHTBInterFace.LaserAction(msg);
            }
            return true;
        }

        bool ShowFishRequestHandler(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_S_ShowFishRequest msg = new CMD_S_ShowFishRequest();
            msg.FishKindNum = rb.ReadInt();
            msg.FishData = new ShowFishData[msg.FishKindNum];
            for (var i = 0; i < msg.FishData.Length; i++)
            {
                ShowFishData data = new ShowFishData(rb);
                data.Declare = StringUtil.TrimUnusefulChar(data.Declare);
                msg.FishData[i] = data;
            }

            if (SHTBInterFace != null)
            {
                SHTBInterFace.ShowFishRequest(msg);
            }
            return true;
        }
        bool ShowGMInfoBack(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GM_C_GetConfig gm = new CMD_GM_C_GetConfig();
            gm.BaseData = new CMD_GM_BaseConfig(rb);
            gm.StockData = new CMD_GM_StockConfig(rb);
            gm.RobotData = new CMD_GM_RobotConfig(rb);
            gm.FishConfigData = new CMD_GM_FishConfig(rb);
            gm.FishProbData = new CMD_GM_FishProb(rb);
            if (SHTBInterFace != null)
            {
                SHTBInterFace.ShowGMInfoBack(gm);
            }
            return true;
        }
        bool SaveGMInfoBack(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GM_S_SaveConfig gm = new CMD_GM_S_SaveConfig();
            gm.Success = rb.ReadUshort();
            if (SHTBInterFace != null)
            {
                SHTBInterFace.SaveGMInfoBack(gm);
            }
            return true;
        }
        bool RepertoryRefreshBack(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GM_S_GetStock r = new CMD_GM_S_GetStock();
            r.StockData = new CMD_GM_StockConfig(rb);
            if (SHTBInterFace != null)
            {
                SHTBInterFace.RepertoryRefreshBack(r);
            }

            return true;
        }
        bool RepertoryBack(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GM_S_BackForStock r = new CMD_GM_S_BackForStock();
            r.Sign = rb.ReadUshort();
            if (SHTBInterFace != null)
            {
                SHTBInterFace.RepertoryBack(r);
            }
            return true;
        }
        bool RepertoryBlackWhite(ushort maincmd, ushort subCmd, CyNetReadBuffer rb)
        {
            CMD_GM_S_BlackWhite r = new CMD_GM_S_BlackWhite(rb);
            if (SHTBInterFace != null)
            {
                SHTBInterFace.RepertoryBlackWhite(r);
            }
            return true;

        }
        #endregion

    }
}
