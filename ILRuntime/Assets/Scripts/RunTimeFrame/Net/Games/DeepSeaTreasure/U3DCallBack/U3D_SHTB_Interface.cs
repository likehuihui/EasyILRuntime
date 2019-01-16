using System;
using System.Collections.Generic;
using System.Text;
using MessagePackage.Struct;
using SHTB;

namespace SHTB
{
    //框架回调接口
    public interface U3D_SHTB_Interface
    {
        #region 框架层内容
        /// <summary>
        /// 错误内容
        /// </summary>
        /// <param name="err"></param>
        void Error(string err);

        /// <summary>
        /// 游戏场景接收完毕
        /// </summary>
        void GAME_SCENE_OK();

        ///// <summary>
        ///// 游戏状态
        ///// </summary>
        ///// <param name="msg"></param>
        void GAME_GameStatus(CMD_GF_GameStatus msg);

        ///// <summary>
        ///// 系统框架消息
        ///// </summary>
        ///// <param name="szString"></param>
        void GAME_SystemMessage(string szString);
        #endregion

        /// <summary>
        /// 心跳包
        /// </summary>
        /// <param name="msg"></param>
        void HeartBeatResponse(CMD_CS_HeartBeat msg);
        /// <summary>
        /// 进入更新炮台等数据
        /// </summary>
        /// <param name="msg"></param>
        void InitGameDataResponse(CMD_S_InitGameData msg);
        /// <summary>
        /// 进入更新当前场景鱼的数据
        /// </summary>
        /// <param name="msg"></param>
        void InitSceneFishResponse(CMD_S_InitSceneFish msg);
        /// <summary>
        /// 进入更新所有玩家子弹的数据
        /// </summary>
        /// <param name="msg"></param>
        void InitSceneBulletResponse(CMD_S_InitSceneBullet msg);
        /// <summary>
        /// 玩家进入房间
        /// </summary>
        /// <param name="msg"></param>
        void PlayerJoinResponse(CMD_S_PlayerJoin msg);
        /// <summary>
        /// 玩家离开房间
        /// </summary>
        /// <param name="msg"></param>
        void PlayerLeaveResponse(CMD_S_PlayerLeave msg);
        /// <summary>
        /// 切换子弹
        /// </summary>
        /// <param name="msg"></param>
        void ChangeBulletResponse(CMD_S_ChangeBullet msg);
        /// <summary>
        /// 玩家开火
        /// </summary>
        /// <param name="msg"></param>
        void BulletFireResponse(CMD_S_RequestFire msg);
        /// <summary>
        /// 锁定某条鱼
        /// </summary>
        /// <param name="msg"></param>
        void LockFishResponse(CMD_S_ChangeLock msg);
        /// <summary>
        /// 自动状态
        /// </summary>
        /// <param name="ms"></param>
        void AutoState(CMD_ChangeAutoState ms);
        /// <summary>
        /// 开启特殊炮台(凝霜 穿云)
        /// </summary>
        /// <param name="msg"></param>
        void OpenSpecialCanonResponse(CMD_S_TurnOnSpecialCanon msg);
        /// <summary>
        /// 发送一组鱼
        /// </summary>
        /// <param name="msg"></param>
        void LaunchGroupFishResponse(CMD_S_CreateFishGroup msg);
        /// <summary>
        /// 子弹打中鱼
        /// </summary>
        /// <param name="msg"></param>
        void CatchFishResponse(CMD_S_BulletActionHandler msg);
        /// <summary>
        /// 错误码返回
        /// </summary>
        /// <param name="msg"></param>
        void FialedCodeResponse(CMD_S_FailedCode msg);
        /// <summary>
        /// 清理场景
        /// </summary>
        void ClearScene(CMD_S_ClearScene msg);

        /// <summary>
        /// 激光炮
        /// </summary>
        /// <param name="msg"></param>
        void LaserAction(CMD_S_LaserActionHandler msg);

        /// <summary>
        /// 显示鱼
        /// </summary>
        /// <param name="msg"></param>
        void ShowFishRequest(CMD_S_ShowFishRequest msg);
        /// <summary>
        /// GM工具返回信息
        /// </summary>
        /// <param name="gm"></param>
        void ShowGMInfoBack(CMD_GM_C_GetConfig gm);
        /// <summary>
        /// gm保存返回
        /// </summary>
        /// <param name="gm"></param>
        void SaveGMInfoBack(CMD_GM_S_SaveConfig gm);
        /// <summary>
        /// 刷新库存返回
        /// </summary>
        /// <param name="gm"></param>
        void RepertoryRefreshBack(CMD_GM_S_GetStock gm);
        /// <summary>
        ///库存操作返回
        /// </summary>
        /// <param name="gm"></param>
        void RepertoryBack(CMD_GM_S_BackForStock gm);
        /// <summary>
        /// 更新黑白名单
        /// </summary>
        /// <param name="gm"></param>
        void RepertoryBlackWhite(CMD_GM_S_BlackWhite gm);

    }
}
