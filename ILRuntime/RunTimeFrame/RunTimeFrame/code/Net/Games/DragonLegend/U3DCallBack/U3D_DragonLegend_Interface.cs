using System;
using System.Collections.Generic;
using System.Text;
using MessagePackage.Struct;

namespace DragonLegend
{
    /// <summary>
    /// ////////////////////////////////////////////code by kingsley  6/27/2016 ///////////////////////
    /// </summary>
    public interface U3D_DragonLegend_Interface
    {
        /// <summary>
        /// 错误提示
        /// </summary>
        /// <param name="errStr"></param>
        void Error(string errStr);
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
        ///// <summary>
        ///// 下注
        ///// </summary>
        ///// <param name="HandBet"></param>
        void Game_UserHandBet(DragonLegend_.CMD_S_PlayBet HandBet);
        ///// <summary>
        ///// 收到游戏开始消息
        ///// </summary>
        ///// <param name="banker"></param>
        void Game_StartGame(DragonLegend_.CMD_S_GameStart Start);

        ///// <summary>
        ///// 清理筹码
        ///// </summary>
        ///// <param name="BetClear"></param>
        void GAME_BetClear(DragonLegend_.CMD_S_BetClear BetClear);
        ///// <summary>
        ///// 下注失败
        ///// </summary>
        ///// <param name="BetFail"></param>
        void Game_BetFail(DragonLegend_.CMD_S_PlayBetFail BetFail);
        ///// <summary>
        ///// 本局游戏结束
        ///// </summary>
        ///// <param name="endDate"></param>
        void Game_End(DragonLegend_.CMD_S_GameEnd endDate);
        ///// <summary>
        /////游戏空闲时间
        ///// </summary>
        ///// <param name="endDate"></param>
        void Game_Free(DragonLegend_.CMD_S_GameFree FreeDate);
        ///// <summary>
        ///// 游戏场景之 等待开始
        ///// </summary>
        ///// <param name="free"></param>
        void GAME_SCENE_FREE(CMD_S_StatusFree free);
        ///// <summary>
        ///// 下注状态
        ///// </summary>
        ///// <param name="sCORE"></param>
        void GAME_SCENE_BET(CMD_S_StatusPlay sCORE);

        ///// <summary>
        ///// 开奖状态
        ///// </summary>
        ///// <param name="sCORE"></param>
        void GAME_SCENE_END(CMD_S_StatusPlay sCORE);


        void Game_AMDIN_COMMAND(DragonLegend_.CMD_S_CommandResult msg);

        void GM_IsMaster(bool isMaster);
    }
}
