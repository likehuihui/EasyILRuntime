using System;
using System.Collections.Generic;
using System.Text;
using MessagePackage.Struct;

namespace Land
{
    //框架回调接口
    public interface U3D_Land_Interface
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
        /// <summary>
        /// 游戏状态
        /// </summary>
        void GAME_GameStatus(CMD_GF_GameStatus msg);
        /// <summary>
        /// 系统框架消息
        /// </summary>
        void GAME_SystemMessage(string szString);

        /// <summary>
        /// 收到游戏开始消息
        /// </summary>
        void Game_StartGame(Land_.CMD_S_GameStart Start);
        /// <summary>
        /// 收到用户叫分消息
        /// </summary>
        void Game_UserCall(Land_.CMD_S_CallScore Call);
        /// <summary>
        /// 用户地主信息
        /// </summary>
        void GAME_BankerInfo(Land_.CMD_S_BankerInfo Banker);
        /// <summary>
        /// 收到用户出牌
        /// </summary>
        void GAME_OutCardInfo(Land_.CMD_S_OutCard OutCard);
        /// <summary>
        /// 收到用户不要
        /// </summary>
        void Game_Pass(Land_.CMD_S_PassCard Pass);
        /// <summary>
        /// 本局游戏结束
        /// </summary>
        /// <param name="endDate"></param>
        void Game_End(Land_.CMD_S_GameConclude endDate);
        /// <summary>
        /// 托管
        /// </summary>
        /// <param name="endDate"></param>
        void Game_Trustee(Land_.CMD_S_TRUSTEE endDate);

        /// <summary>
        /// 收到扑克
        /// </summary>
        void GAME_Dic_PuKe(Dictionary<ushort, byte[]> dic);

        /// <summary>
        /// 游戏场景之 等待开始
        /// </summary>
        /// <param name="free"></param>
        void GAME_SCENE_FREE(CMD_S_StatusFree free);
        /// <summary>
        /// 游戏场景之 叫庄状态
        /// </summary>
        /// <param name="cALL"></param>
        void GAME_SCENE_CALL(CMD_S_StatusCall cALL);

        /// <summary>
        /// 游戏进行中
        /// </summary>
        /// <param name="play_"></param>
        void GAME_SCENE_PLAYING(CMD_S_StatusPlay play_);

        /// <summary>
        /// 比赛信息
        /// </summary>
        /// <param name="play_"></param>
        void GAME_MATCH_INFO(CMD_GR_Match_Info msg);

        /// <summary>
        /// 比赛信息
        /// </summary>
        /// <param name="play_"></param>
        void GAME_MATCH_PROMOTION(CMD_GR_MatchPromotion msg);

        /// <summary>
        /// 比赛信息
        /// </summary>
        /// <param name="play_"></param>
        void GAME_MATCH_RESULT(CMD_GR_MatchResult msg);

        /// <summary>
        /// 比赛信息
        /// </summary>
        /// <param name="play_"></param>
        void GAME_MATCH_WATI_TIP(CMD_GR_Match_Wait_Tip msg);

    }
}
