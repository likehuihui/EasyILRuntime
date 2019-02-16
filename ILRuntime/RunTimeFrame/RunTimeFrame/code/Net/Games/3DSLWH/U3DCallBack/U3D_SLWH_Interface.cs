using System;
using System.Collections.Generic;
using System.Text;
using MessagePackage.Struct;
using SLWH;

namespace SLWH
{
    //框架回调接口
    public interface U3D_SLWH_Interface
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



        void Game_FreeGame(SLWH_.CMD_S_GameFree Free);
        /// <summary>
        /// 收到游戏开始消息
        /// </summary>
        void Game_StartGame(SLWH_.CMD_S_GameStart Start);
        
        void Game_PlaceJetton(SLWH_.CMD_S_PlaceJetton Jetton);

        void Game_REAL_PlaceJetton(SLWH_.CMD_S_PlaceJetton Jetton);
        

        void Game_End(SLWH_.CMD_S_GameEnd End);

        void Game_ApplyBanker(SLWH_.CMD_S_ApplyBanker OpUser);
        void Game_CancelBanker(SLWH_.CMD_S_CancelBanker OpUser);

        void Game_ChangeBanker(SLWH_.CMD_S_ChangeBanker Change);

        void Game_Record(SLWH_.tagServerGameRecord[] Record);

        void Game_PlaceJettonFail(SLWH_.CMD_S_PlaceJettonFail Fail);

        void Game_TOTAL_JETTON_INFO(SLWH_.CMD_S_TotalJetton msg);

        void Game_UPDATE_WEAGER_ARR(SLWH_.tagWeagerArr msg);

        void Game_GET_ADMIN(SLWH_.CMD_S_GetAdmin msg);

        void Game_ACK_ADMIN(string msg);

        void Game_GET_STORAGE(SLWH_.CMD_S_StorageInfo msg);

        void Game_GET_PRIZE(SLWH_.CMD_S_PrizeInfo msg);

        void Game_DEL_PRIZE(SLWH_.CMD_S_PrizeInfo msg);

        void Game_GET_PROB(SLWH_.CMD_S_PorbInfo msg);

        void Game_BINGO_PROB(SLWH_.CMD_C_BingoProb msg);
        /// <summary>
        /// 游戏场景之 等待开始
        /// </summary>
        /// <param name="free"></param>
        void GAME_SCENE_FREE(SLWH_.CMD_S_StatusFree free);

        /// <summary>
        /// 游戏进行中
        /// </summary>
        /// <param name="play_"></param>
        void GAME_SCENE_PLAYING(SLWH_.CMD_S_StatusPlay play_);

        /*/// <summary>
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
        void GAME_MATCH_WATI_TIP(CMD_GR_Match_Wait_Tip msg);*/

    }
}
