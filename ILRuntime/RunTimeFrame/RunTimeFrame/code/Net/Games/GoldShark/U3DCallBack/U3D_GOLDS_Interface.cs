using System;
using System.Collections.Generic;
using System.Text;
using MessagePackage.Struct;
using GOLDS;

namespace GOLDS
{
    //框架回调接口
    public interface U3D_GOLDS_Interface
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


        void Game_Record(GOLDS_.tagServerGameRecord[] record);

        void Game_FreeGame(GOLDS_.CMD_S_GameFree Free);
        /// <summary>
        /// 收到游戏开始消息
        /// </summary>
        void Game_StartGame(GOLDS_.CMD_S_GameStart Start);
        
        void Game_PlaceJetton(GOLDS_.CMD_S_PlaceJetton Jetton);

        void Game_End(GOLDS_.CMD_S_GameEnd End);

        void Game_ApplyBanker(GOLDS_.CMD_S_ApplyBanker OpUser);
        void Game_CancelBanker(GOLDS_.CMD_S_CancelBanker OpUser);

        void Game_ChangeBanker(GOLDS_.CMD_S_ChangeBanker Change);

        void Game_PlaceJettonFail(GOLDS_.CMD_S_PlaceJettonFail Fail);
        /// <summary>
        /// 游戏场景之 等待开始
        /// </summary>
        /// <param name="free"></param>
        void GAME_SCENE_FREE(GOLDS_.CMD_S_StatusFree free);
        /// <summary>
        /// 游戏进行中
        /// </summary>
        /// <param name="play_"></param>
        void GAME_SCENE_PLAYING(GOLDS_.CMD_S_StatusPlay play_);

    }
}
