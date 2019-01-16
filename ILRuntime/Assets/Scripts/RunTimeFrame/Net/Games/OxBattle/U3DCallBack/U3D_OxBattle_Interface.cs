using System;
using System.Collections.Generic;
using System.Text;
using MessagePackage.Struct;

namespace OxBattle
{
    /// <summary>
    /// ////////////////////////////////////////////code by kingsley 10/8/2016 ///////////////////////
    /// </summary>
    public interface U3D_OxBattle_Interface
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
        void Game_UserHandBet(OxBattle_.CMD_S_PlaceJetton HandBet);
        void Game_BetAgain(OxBattle_.CMD_S_ContinueJetton BetMsg);
        void Game_BetAgainFail(OxBattle_.CMD_S_ContinueJettonFail BetFailMsg);
        ///// <summary>
        ///// 收到游戏开始消息
        ///// </summary>
        ///// <param name="banker"></param>
        void Game_StartGame(OxBattle_.CMD_S_GameStart Start);

        ///// <summary>
        ///// 下注失败
        ///// </summary>
        ///// <param name="BetFail"></param>
        void Game_BetFail(OxBattle_.CMD_S_PlaceJettonFail BetFail);
        ///// <summary>
        ///// 本局游戏结束
        ///// </summary>
        ///// <param name="endDate"></param>
        void Game_End(OxBattle_.CMD_S_GameEnd endDate);
        ///// <summary>
        /////游戏空闲时间
        ///// </summary>
        ///// <param name="FreeDate"></param>
        void Game_Free(OxBattle_.CMD_S_GameFree FreeDate);
        ///// <summary>
        ///// 游戏场景之 等待开始
        ///// </summary>
        ///// <param name="free"></param>
        void GAME_SCENE_FREE(CMD_S_StatusFree free);
        ///// <summary>
        ///// 下注状态
        ///// </summary>
        ///// <param name="PLACE_JETTON"></param>
        void GAME_SCENE_PLACE_JETTON(CMD_S_StatusPlay sCORE);

        ///// <summary>
        ///// 开奖状态
        ///// </summary>
        ///// <param name="SCENE_END"></param>
        void GAME_SCENE_END(CMD_S_StatusPlay sCORE);


        ///// <summary>
        ///// 申请上庄
        ///// </summary>
        ///// <param name="ApplyBanker"></param>
        void Game_ApplyBanker(OxBattle_.CMD_S_ApplyBanker ApplyBanker);

        ///// <summary>
        ///// 取消申请
        ///// </summary>
        ///// <param name="CancelBanker"></param>
        void Game_CancelBanker(OxBattle_.CMD_S_CancelBanker CancelBanker);

        ///// <summary>
        ///// 切换庄家
        ///// </summary>
        ///// <param name="ChangeBanker"></param>

        void Game_BankerList(OxBattle_.CMD_S_BankerList bankerList);

        void Game_RealPlayerList(OxBattle_.CMD_S_RefreshTruthPlayerInfo playerList);

        void Game_ChangeBanker(OxBattle_.CMD_S_ChangeBanker ChangeBanker);

        ///// <summary>
        ///// 发送记录
        ///// </summary>
        ///// <param name="ChangeBanker"></param>
        void Game_RecordInfo(OxBattle_.tagServerGameRecord[] GameRecord, int nLenght);

        ///// <summary>
        ///// 难度信息
        ///// </summary>
        ///// <param name="ChangeBanker"></param>
        void Game_StorageAllInfo(OxBattle_.CMD_S_Storage_AllInfo AllInfo);

        ///// <summary>
        ///// 更新库存
        ///// </summary>
        ///// <param name="ChangeBanker"></param>
        void Game_UpdateStroageInfo(OxBattle_.CMD_S_UpdateStorage UpdateInfo);

        //游戏中银行取款
        void Game_InsureSuccess(OxBattle_.CMD_GR_S_UserInsureSuccess InsureSuccessInfo);
        void Game_InsureFailure(OxBattle_.CMD_GR_S_UserInsureFailure InsureFailureInfo);

        void Game_HeartBeat();
    }
}
