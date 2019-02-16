using CYNetwork;
using MessagePackage;
using MessagePackage.Operation;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SHTB
{
    //游戏消息
    public partial class SHTB_
    {
        #region 客户端发送命令
        //public const ushort SUB_C_GM_Get_REQUEST = 25;   //获取配置信息
        //public const ushort SUB_C_GM_Save_REQUEST = 27;  //保存配置信息
        //public const ushort SUB_C_GM_Get_Stock_REQUEST=28;  //获取最新库存信息
        //public const ushort SUB_C_GM_Clear_Stock_REQUEST = 30;  //清空库存请求

        //public const ushort SUB_C_ShowFish_REQUEST = 32;  //请求图鉴数据

        //public const ushort SUB_C_Test_Net = 66; //测试网络请求

        //public const ushort SUB_C_Test_Score = 80; //对分数

        //public const ushort SUB_C_CHANGE_LAUNCHER = 784; //换炮请求
        //public const ushort SUB_C_CHANGE_LAUNCHER_TYPE = 785; //切换到指定炮台请求
        //public const ushort SUB_C_CHANGE_CLIENT_RATE = 786; //加减倍率请求
        //public const ushort SUB_C_CHANGE_RATE = 787; //切换倍率请求
        //public const ushort SUB_C_CHANGE_RATE_TYPE = 788; //切换到指定倍率请求
        //public const ushort SUB_C_USE_SKILL = 794; //使用技能请求
        //public const ushort SUB_C_SKILL_LASER_REQUEST = 801;//使用激光技能请求
        //public const ushort SUB_C_CHECK_FISH_POS = 815;//检测鱼的位置(防作弊)
        //public const ushort SUB_C_CHECK_BULLET_POS = 816;//检测子弹位置(防作弊)


        //public const ushort SUB_C_User_Leave = 515; //用户离开请求


        //public const ushort SUB_C_Get_User_Item = 1537; //获取用户Item请求
        //public const ushort SUB_C_On_Use_Item = 1543; //使用Item请求


        //public const ushort SUB_C_RESET_ROLE_INFO = 2358;//重置角色信息
        //public const ushort SUB_C_ChangeRoleRateValue = 2373;//改变玩家倍率请求


        //public const ushort SUB_C_GetRoleTaskInfo = 3073;
        //public const ushort SUB_C_GetTaskReward = 3078;
        //public const ushort SUB_C_GetOnceTaskInfo = 3081;

        //public const ushort SUB_C_GetChestReward = 4356;//获得一个箱子请求
        //public const ushort SUB_C_CloseChest = 4358;//关闭箱子

        //public const ushort SUB_C_GetGameData = 6657; //请求玩家数据

        //public const ushort SUB_C_GetLotteryReward = 8961;//彩金奖励请求(得奖)
        //public const ushort SUB_C_LotteryUIStates = 8963;// 彩金界面状态
        //public const ushort SUB_C_LotteryUIData = 8966;// 彩金界面数据


        //public const ushort SUB_GR_TAKE_SCORE_REQUEST = 4;                                  //取款请求
        #endregion

        #region 客户端结构体

        //public class tagTestSocre 
        //{
        //    public long Score;
        //}

        //public class NetCmdUseSkill
        //{
        //    public byte SkillID;
        //}

        //public class NetCmdChangeLauncherType
        //{
        //    public ushort Seat;
        //    public byte LauncherType;
        //}

        //public class NetCmdChangeRateType
        //{
        //    public ushort Seat;
        //    public byte RateIndex;
        //}

        //public class NetCmdClientChangeRate
        //{
        //    public bool UpRoDownRate;
        //}

        //public class CL_Cmd_ChangeRoleRateValue
        //{
        //    public Byte RateIndex;
        //}

        //public class CL_Cmd_GetTaskReward
        //{

        //    public Byte TaskID;
        //};

        //public class CL_Cmd_GetOnceTaskInfo
        //{
        //    public Byte TaskID;
        //}

        //public class CL_Cmd_OnUseItem
        //{
        //    public UInt32 ItemOnlyID;
        //    public UInt32 ItemID;
        //    public UInt32 ItemSum;
        //}

        //public class CL_Cmd_GetChestReward
        //{
        //    public Byte ChestOnlyID;
        //    public Byte ChestIndex;
        //}

        //public class CL_Cmd_CloseChest
        //{
        //    public Byte ChestOnlyID;
        //}


        //public class CL_Cmd_GetLotteryReward
        //{               
        //    public Byte LotteryID;
        //}

        //public class CL_Cmd_LotteryUIStates
        //{
        //    public UInt32 dwUserID;
        //    public bool IsOpen;
        //}

        #endregion
        ///// <summary>
        ///// 发送换炮请求
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendRequestCHANGE_LAUNCHER(NetCmdChangeLauncher changlauncher)
        //{                      
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(changlauncher.Seat);
        //    buffer.Write(changlauncher.LauncherType);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CHANGE_LAUNCHER, buffer);
        //}  

        ///// <summary>
        ///// 发送切换到指定炮台请求
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendRequestCHANGE_LAUNCHER_TYPE(NetCmdChangeLauncherType changlaunchert)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(changlaunchert.Seat);
        //    buffer.Write(changlaunchert.LauncherType);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CHANGE_LAUNCHER_TYPE, buffer);
        //}

        ///// <summary>
        ///// 发送切换倍率请求
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendRequestCHANGE_RATE(NetCmdChangeRate changrate)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(changrate.Seat);
        //    buffer.Write(changrate.RateIndex);
        //    buffer.Write(changrate.IsCanUseRate);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CHANGE_RATE, buffer);
        //}

        ///// <summary>
        ///// 发送切换到指定倍率请求
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendRequestCHANGE_RATE_TYPE(NetCmdChangeRateType changrate)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(changrate.Seat);
        //    buffer.Write(changrate.RateIndex);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CHANGE_RATE_TYPE, buffer);
        //}

        ///// <summary>
        ///// 发送加减倍率请求
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendRequestCHANGE_CLIENT_RATE(NetCmdClientChangeRate changcrate)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(changcrate.UpRoDownRate);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CHANGE_CLIENT_RATE, buffer);
        //}

        ///// <summary>
        ///// 发送发子弹请求
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendRequestBullet(NetCmdBullet bullet)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(bullet.BulletID);
        //    buffer.Write(bullet.Degree);
        //    buffer.Write(bullet.LauncherType);
        //    buffer.Write(bullet.Energy);
        //    buffer.Write(bullet.ReboundCount);
        //    buffer.Write(bullet.LockFishID);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_S_BULLET, buffer);
        //}

        ///// <summary>
        ///// 发送使用技能请求
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendRequestUSE_SKILL(NetCmdUseSkill usesk)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(usesk.SkillID);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_USE_SKILL, buffer);
        //}

        ///// <summary>
        ///// 发送使用激光技能请求
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendSKILL_LASER_REQUEST(NetCmdSkillLaser sklaser)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(sklaser.Degree);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_SKILL_LASER_REQUEST, buffer);
        //}


        ///// <summary>
        ///// 发送鱼群位置
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendCHECK_FISH_POS(NetCmdCheckFishPos cfishpos)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(cfishpos.FishNum);
        //    for (int i = 0; i < cfishpos.FishNum; i++)
        //    {
        //        buffer.Write(cfishpos.Fish[i].FishID);
        //        buffer.Write(cfishpos.Fish[i].Time);
        //        buffer.Write(cfishpos.Fish[i].Speed);
        //        //yzb 坐标是不是这样填有待商议
        //        buffer.Write(cfishpos.Fish[i].Pos.x);
        //        buffer.Write(cfishpos.Fish[i].Pos.y);
        //        buffer.Write(cfishpos.Fish[i].Pos.z);

        //        buffer.Write(cfishpos.Fish[i].Rot.x);
        //        buffer.Write(cfishpos.Fish[i].Rot.y);
        //        buffer.Write(cfishpos.Fish[i].Rot.z);
        //        buffer.Write(cfishpos.Fish[i].Rot.w);
        //    }
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CHECK_FISH_POS, buffer);
        //}

        ///// <summary>
        ///// 发送子弹群位置
        ///// </summary>
        ///// <param name="usesk"></param>
        //public void SendCHECK_BULLET_POS(NetCmdCheckBulletPos cbulletpos)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(cbulletpos.Count);
        //    for (int i = 0; i < cbulletpos.Count; i++)
        //    {
        //        buffer.Write(cbulletpos.Bullets[i].ID);

        //        //yzb 坐标是不是这样填有待商议
        //        buffer.Write(cbulletpos.Bullets[i].Pos.x);
        //        buffer.Write(cbulletpos.Bullets[i].Pos.y);
        //        buffer.Write(cbulletpos.Bullets[i].Pos.z);
        //    }
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CHECK_BULLET_POS, buffer);
        //}

        //public void SendRESET_ROLE_INFO()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_RESET_ROLE_INFO, buffer);
        //}

        //public void SendChange_Role_Rate_Value(CL_Cmd_ChangeRoleRateValue chrolerv)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(chrolerv.RateIndex);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ChangeRoleRateValue, buffer);
        //}


        //public void SendGet_Role_Task_Info()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GetRoleTaskInfo, buffer);
        //}

        //public void SendGet_Task_Reward(CL_Cmd_GetTaskReward gtaskreward)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(gtaskreward.TaskID);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GetTaskReward, buffer);
        //}

        //public void SendGet_Once_Task_Info(CL_Cmd_GetOnceTaskInfo gonetaskreward)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(gonetaskreward.TaskID);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GetOnceTaskInfo, buffer);
        //}

        //public void SendLeave_Table()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_User_Leave, buffer);
        //}

        //public void SendGet_User_Item()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_Get_User_Item, buffer);
        //}

        //public void SendOn_Use_Item(CL_Cmd_OnUseItem onuseritem)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(onuseritem.ItemOnlyID);
        //    buffer.Write(onuseritem.ItemID);
        //    buffer.Write(onuseritem.ItemSum);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_On_Use_Item, buffer);
        //}

        //public void SendGet_Chest_Reward(CL_Cmd_GetChestReward getchest)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(getchest.ChestOnlyID);
        //    buffer.Write(getchest.ChestIndex);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GetChestReward, buffer);
        //}

        //public void SendClose_Chest()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_CloseChest, buffer);
        //}

        //public void SendGet_GameData()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GetGameData, buffer);
        //}


        //public void SendGet_Lottery_Reward(CL_Cmd_GetLotteryReward getlottre)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(getlottre.LotteryID);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GetLotteryReward, buffer);
        //}

        //public void SendLottery_UIStates(CL_Cmd_LotteryUIStates lottui)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(lottui.dwUserID);
        //    buffer.Write(lottui.IsOpen);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_LotteryUIStates, buffer);
        //}

        //public void SendLottery_Data()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_LotteryUIData, buffer);
        //}

        //public void SendGM_Get_Stock()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GM_Get_Stock_REQUEST, buffer);
        //}

        //public void SendGM_Clear_Stock()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GM_Clear_Stock_REQUEST, buffer);
        //}

        //public void SendGM_Load_Config()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GM_Get_REQUEST, buffer);
        //}

        //public void SendGM_Save_Config(CMD_GM_S_ConsoleConfig cConfig) 
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    //基本配置
        //    buffer.Write(cConfig.BaseConfig.ChangeRate_0);
        //    buffer.Write(cConfig.BaseConfig.ChangeRate_1);
        //    buffer.Write(cConfig.BaseConfig.SceneChangeTime);
        //    buffer.Write(cConfig.BaseConfig.BulletCount);
        //    buffer.Write(cConfig.BaseConfig.CatchBase_0);
        //    buffer.Write(cConfig.BaseConfig.CatchBase_1);
        //    buffer.Write(cConfig.BaseConfig.FireIntev);
        //    buffer.Write(cConfig.BaseConfig.XPSkillCount);
        //    buffer.Write(cConfig.BaseConfig.XPSAddPer);
        //    buffer.Write(cConfig.BaseConfig.GetCannon3_Need);
        //    buffer.Write(cConfig.BaseConfig.Cannon3_CD);
        //    buffer.Write(cConfig.BaseConfig.GetCannon4_Need);
        //    buffer.Write(cConfig.BaseConfig.Cannon4_CD);
        //    buffer.Write(cConfig.BaseConfig.BulletMultArrLen);
        //    buffer.Write(cConfig.BaseConfig.MultArrMin);
        //    buffer.Write(cConfig.BaseConfig.MultArrMax);
        //    for (int i = 0; i < cConfig.BaseConfig.BulletMultArr.Length; i++)
        //    {
        //        buffer.Write(cConfig.BaseConfig.BulletMultArr[i].Mult);
        //        buffer.Write(cConfig.BaseConfig.BulletMultArr[i].Type);
        //    }
        //    for (int i = 0; i < cConfig.BaseConfig.BulletArr.Length; i++)
        //    {
        //        buffer.Write(cConfig.BaseConfig.BulletArr[i].Type);
        //        buffer.Write(cConfig.BaseConfig.BulletArr[i].Speed);
        //        buffer.Write(cConfig.BaseConfig.BulletArr[i].CatchRang);
        //        buffer.Write(cConfig.BaseConfig.BulletArr[i].FireIntev);
        //    }
        //    //库存配置
        //    buffer.Write(cConfig.StockConfig.Stock);
        //    buffer.Write(cConfig.StockConfig.Pump);
        //    buffer.Write(cConfig.StockConfig.PumpPer);

        //    buffer.Write(cConfig.StockConfig.tempRevenueValue);
        //    buffer.Write(cConfig.StockConfig.limitTempRevenueValue);
        //    buffer.Write(cConfig.StockConfig.tempRevenuePer);

        //    buffer.Write(cConfig.StockConfig.CurDiff);
        //    buffer.Write(cConfig.StockConfig.DiffChangeTime_0);
        //    buffer.Write(cConfig.StockConfig.DiffChangeTime_1);
        //    buffer.Write(cConfig.StockConfig.DiffLimt_Min);
        //    buffer.Write(cConfig.StockConfig.DiffLimt_Max);
        //    //玩家控制
        //    buffer.Write(cConfig.PlayerControl.BlackArrLen);
        //    for (int i = 0; i < cConfig.PlayerControl.BlackArr.Length; i++)
        //    {
        //        buffer.Write(cConfig.PlayerControl.BlackArr[i].GameID);
        //        buffer.Write(cConfig.PlayerControl.BlackArr[i].EatScore);
        //    }
        //    buffer.Write(cConfig.PlayerControl.WhiteArrLen);
        //    for (int i = 0; i < cConfig.PlayerControl.WhiteArr.Length; i++)
        //    {
        //        buffer.Write(cConfig.PlayerControl.WhiteArr[i].GameID);
        //        buffer.Write(cConfig.PlayerControl.WhiteArr[i].GiveScore);
        //    }
        //    buffer.Write(cConfig.PlayerControl.ProbArrLen);
        //    for (int i = 0; i < cConfig.PlayerControl.ProbArr.Length; i++)
        //    {
        //        buffer.Write(cConfig.PlayerControl.ProbArr[i].GameID);
        //        buffer.Write(cConfig.PlayerControl.ProbArr[i].CatchBase);
        //    }
        //    buffer.Write(cConfig.PlayerControl.DiffArrLen);
        //    for (int i = 0; i < cConfig.PlayerControl.DiffArr.Length; i++)
        //    {
        //        buffer.Write(cConfig.PlayerControl.DiffArr[i].GameID);
        //        buffer.Write(cConfig.PlayerControl.DiffArr[i].DiffType);
        //    }
        //    buffer.Write(cConfig.PlayerControl.BigFishArrLen);
        //    for (int i = 0; i < cConfig.PlayerControl.BigFishArr.Length; i++)
        //    {
        //        buffer.Write(cConfig.PlayerControl.BigFishArr[i].GameID);
        //        buffer.Write(cConfig.PlayerControl.BigFishArr[i].Type);
        //        buffer.Write(cConfig.PlayerControl.BigFishArr[i].CatchValue);
        //        buffer.Write(cConfig.PlayerControl.BigFishArr[i].Num);
        //    }
        //    //机器人配置
        //    buffer.Write(cConfig.RobotConfig.FireInv_0);
        //    buffer.Write(cConfig.RobotConfig.FireInv_1);
        //    buffer.Write(cConfig.RobotConfig.FireAngleChange_0);
        //    buffer.Write(cConfig.RobotConfig.FireAngleChange_1);
        //    buffer.Write(cConfig.RobotConfig.ChangMult_0);
        //    buffer.Write(cConfig.RobotConfig.ChangMult_1);
        //    buffer.Write(cConfig.RobotConfig.FireToRestTime_0);
        //    buffer.Write(cConfig.RobotConfig.FireToRestTime_1);
        //    buffer.Write(cConfig.RobotConfig.RestToFireTime_0);
        //    buffer.Write(cConfig.RobotConfig.RestToFireTime_1);
        //    buffer.Write(cConfig.RobotConfig.CatchValue_0);
        //    buffer.Write(cConfig.RobotConfig.CatchValue_1);
        //    buffer.Write(cConfig.RobotConfig.GameTime_0);
        //    buffer.Write(cConfig.RobotConfig.GameTime_1);

        //    buffer.Write(cConfig.RobotConfig.Socre_Min);
        //    buffer.Write(cConfig.RobotConfig.Socre_Max);
        //    buffer.Write(cConfig.RobotConfig.Socre_Per);
        //    //鱼种配置
        //    for (int i = 0; i < cConfig.FishConfig.FishConfigList.Length; i++)
        //    {
        //        buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArrLen);
        //        for (int j = 0; j < cConfig.FishConfig.FishConfigList[i].FishConfigArr.Length; j++)
        //        {
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].ID);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].typeValue);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].multMin);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].multMax);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].speed);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].invMin);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].invMax);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].numMin);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].numMax);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].catchValue);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].dropCan3Value);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].dropCan4Value);
        //            buffer.Write(cConfig.FishConfig.FishConfigList[i].FishConfigArr[j].isGoodFish);
        //        }
        //    }
        //    //抽奖配置
        //    buffer.Write(cConfig.LotteryConfig.Handsel_Base);
        //    buffer.Write(cConfig.LotteryConfig.Handsel_Count);
        //    for (int i = 0; i < cConfig.LotteryConfig.Handsel_1Per_0.Length; i++)
        //    {
        //        buffer.Write(cConfig.LotteryConfig.Handsel_1Per_0[i]);         
        //    }
        //    for (int i = 0; i < cConfig.LotteryConfig.Handsel_1Per_1.Length; i++)
        //    {

        //        buffer.Write(cConfig.LotteryConfig.Handsel_1Per_1[i]);
        //    }


        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_GM_Save_REQUEST, buffer);
        //}


        ///// <summary>
        ///// 银行取款请求
        ///// </summary>
        ///// <param name="user"></param>
        //public void TakeScoreRequest(byte cbActivityGame, long lTakeScore, string szInsurePass)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    buffer.Write(cbActivityGame);
        //    buffer.Write(lTakeScore);
        //    buffer.Write(szInsurePass, 66);
        //    //发送准备
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_INSURE, SUB_GR_TAKE_SCORE_REQUEST, buffer);
        //}

        //public void Send_ShowFish_REQUEST()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_ShowFish_REQUEST, buffer);
        //}

        //public void Send_Test_NetWork()
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer(2);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_Test_Net, buffer);
        //}

        //public void Send_Test_Score(long mscore)
        //{
        //    CyNetWriteBuffer buffer = new CyNetWriteBuffer();
        //    tagTestSocre score = new tagTestSocre();
        //    score.Score = mscore;
        //    buffer.Write(score.Score);
        //    MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, SUB_C_Test_Score, buffer);
        //}

        #region 新协议
        /// <summary>
        /// 心跳协议
        /// </summary>
        /// <param name="cmd"></param>
        public void HeartBeat(CMD_CS_HeartBeat cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.Sign);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_HEART_BEAT, buffer);
        }

        /// <summary>
        /// 切换子弹
        /// </summary>
        /// <param name="cmd"></param>
        public void ChangeBullet(CMD_C_ChangeBullet cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.BulletType);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_CHANGE_BULLET, buffer);
        }

        /// <summary>
        /// 开火
        /// </summary>
        /// <param name="cmd"></param>
        public void RequestFire(CMD_C_RequestFire cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.BulletID);
            buffer.Write(cmd.Angle);
            buffer.Write(cmd.CanonType);
            buffer.Write(cmd.LockFishID);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_REQUEST_FIRE, buffer);
        }

        /// <summary>
        /// 锁定或者切换锁定返回
        /// </summary>
        /// <param name="cmd"></param>
        public void RequestLock(CMD_C_RequestLock cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.Action);
            buffer.Write(cmd.LockFishID);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_REQUEST_LOCK, buffer);
        }
        /// <summary>
        /// 自动切换
        /// </summary>
        public void RequestAuto(CMD_ChangeAutoState cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.State);
            buffer.Write(cmd.ChairID);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_REQ_AUTO_STATE, buffer);
        }
        /// <summary>
        /// 请求开启特殊炮台
        /// </summary>
        /// <param name="cmd"></param>
        public void OpenSpecialCanon(CMD_C_RequestTurnOnSpecialCanon cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.SpecialCanonType);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_OPEN_SPECIAL_CANON, buffer);
        }

        /// <summary>
        /// 请求激光炮
        /// </summary>
        /// <param name="cmd"></param>
        public void RequestLaser(CMD_C_RequestLaser cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.Degree);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_REQUEST_LASER, buffer);
        }

        /// <summary>
        /// 请求图鉴数据
        /// </summary>
        /// <param name="cmd"></param>
        public void RequestShowFishData()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_SHOW_FISH_REQUEST, buffer);
        }

        /// <summary>
        /// 通知服务器客户端碰撞
        /// </summary>
        /// <param name="cmd"></param>
        public void CollideFish(CMD_C_CollideByClientBullet cmd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(cmd.BulletID);
            buffer.Write(cmd.CollideNum);
            for (var i = 0; i < cmd.CollideNum; i++)
            {
                buffer.Write(cmd.FishIDs[i]);
            }

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_REQ_BULLET_IMP, buffer);
        }
        /// <summary>
        /// 请求GM数据
        /// </summary>
        /// <param name="sign"></param>
        public void RequestGmInfo(CMD_GM_C_SendInfo sign)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(sign.sign);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_GM_GET_CONFIG, buffer);
        }
        /// <summary>
        /// 请求保存GM信息
        /// </summary>
        /// <param name="info"></param>
        public void SaveGMInfo(CMD_GM_C_SaveConfig info)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(info.BaseData.CatchBase);
            buffer.Write(info.BaseData.DropPer);
            buffer.Write(info.BaseData.XPSkillCount);
            buffer.Write(info.BaseData.XPSKillPer1);
            buffer.Write(info.BaseData.XPSKillPer2);
            buffer.Write(info.BaseData.XPSMaxCatch);
            buffer.Write(info.BaseData.XPSMaxMult);
            buffer.Write(info.BaseData.GetCannon3_Need);
            buffer.Write(info.BaseData.Cannon3_CD);
            buffer.Write(info.BaseData.GetCannon4_Need);
            buffer.Write(info.BaseData.Cannon4_CD);
            for (int i = 0; i < info.BaseData.BulletConfig.Length; i++)
            {
                buffer.Write(info.BaseData.BulletConfig[i].Id);
                buffer.Write(info.BaseData.BulletConfig[i].Value);
                buffer.Write(info.BaseData.BulletConfig[i].HitPer);
                buffer.Write(info.BaseData.BulletConfig[i].Feather);
                buffer.Write(info.BaseData.BulletConfig[i].Speed);
                buffer.Write(info.BaseData.BulletConfig[i].HitSpace);
                buffer.Write(info.BaseData.BulletConfig[i].Radius);
            }
            for (int i = 0; i < info.BaseData.CanonConfig.Length; i++)
            {
                buffer.Write(info.BaseData.CanonConfig[i].Id);
                buffer.Write(info.BaseData.CanonConfig[i].Name,64);
                buffer.Write(info.BaseData.CanonConfig[i].MaxCatch);
            }
            buffer.Write(info.StockData.Stock);
            buffer.Write(info.StockData.SystemRevenue);
            buffer.Write(info.StockData.SystemRevenuePer);
            buffer.Write(info.StockData.TempRevenueValue);
            buffer.Write(info.StockData.TempRevenueLimitValue);
            buffer.Write(info.StockData.TempRevenuePer);
            buffer.Write(info.StockData.CurDiff);
            buffer.Write(info.StockData.DiffChangeTime0);
            buffer.Write(info.StockData.DiffChangeTime1);
            buffer.Write(info.StockData.DiffLimtMin);
            buffer.Write(info.StockData.DiffLimtMax);

            buffer.Write(info.RobotData.ActionTime);
            buffer.Write(info.RobotData.FirePer);
            buffer.Write(info.RobotData.ResetPer);
            buffer.Write(info.RobotData.ChangMultMinTime);
            buffer.Write(info.RobotData.ChangMultMaxTime);
            buffer.Write(info.RobotData.LockMinTime);
            buffer.Write(info.RobotData.LockMaxTime);
            buffer.Write(info.RobotData.RestTime);
            buffer.Write(info.RobotData.CatchValue);
            buffer.Write(info.RobotData.CatchBossValue);

            for (int i = 0; i < info.FishConfigData.BaseConfig.Length; i++)
            {
                buffer.Write(info.FishConfigData.BaseConfig[i].Id);
                buffer.Write(info.FishConfigData.BaseConfig[i].Name,64);
                buffer.Write(info.FishConfigData.BaseConfig[i].ValueMin);
                buffer.Write(info.FishConfigData.BaseConfig[i].ValueMax);
                buffer.Write(info.FishConfigData.BaseConfig[i].Declare,64);
            }
            for (int i = 0; i < info.FishProbData.FishDiffData.Length; i++)
            {
                for (int j = 0; j < info.FishProbData.FishDiffData[i].BaseConfig.Length; j++)
                {
                    buffer.Write(info.FishProbData.FishDiffData[i].BaseConfig[j].Id);
                    buffer.Write(info.FishProbData.FishDiffData[i].BaseConfig[j].Chance);
                    buffer.Write(info.FishProbData.FishDiffData[i].BaseConfig[j].Frost);
                    buffer.Write(info.FishProbData.FishDiffData[i].BaseConfig[j].Through);
                    buffer.Write(info.FishProbData.FishDiffData[i].BaseConfig[j].KillCondMulti);
                }

            }
           // Debug.Log(buffer.Length);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_GM_SAVE_CONFIG, buffer);
        }
        /// <summary>
        /// 请求,刷新库存
        /// </summary>
        /// <param name="gm"></param>
        public void RepertoryInfo(CMD_GM_C_GetStock gm)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(gm.Sign);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_GM_GET_STOCK, buffer);
        }
        /// <summary>
        /// 请求保存库存
        /// </summary>
        /// <param name="gm"></param>
        public void SaveRepertory(CMD_GM_C_SaveStock gm)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(gm.StockData.Stock);
            buffer.Write(gm.StockData.SystemRevenue);
            buffer.Write(gm.StockData.SystemRevenuePer);
            buffer.Write(gm.StockData.TempRevenueValue);
            buffer.Write(gm.StockData.TempRevenueLimitValue);
            buffer.Write(gm.StockData.TempRevenuePer);
            buffer.Write(gm.StockData.CurDiff);
            buffer.Write(gm.StockData.DiffChangeTime0);
            buffer.Write(gm.StockData.DiffChangeTime1);
            buffer.Write(gm.StockData.DiffLimtMin);
            buffer.Write(gm.StockData.DiffLimtMax);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_GM_SAVE_STOCK, buffer);
        }
        /// <summary>
        /// 请求清理库存
        /// </summary>
        /// <param name="sign"></param>
        public void CleanRepertory(CMD_GM_C_ClearStock sign)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(sign.Sign);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_CLEAR_STOCK, buffer);
        }
        /// <summary>
        /// 请求更新黑白名单
        /// </summary>
        /// <param name="ms"></param>
        public void UpdateBWList(CMD_GM_C_BlackWhite ms)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(ms.Type);
            buffer.Write(ms.OperateIndex);
            buffer.Write(ms.GameID);
            buffer.Write(ms.ControlPer);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_GAME, C2SMsgId.SUB_C_GM_UPDATE_BW_LIST, buffer);
        }
        #endregion
    }
}
