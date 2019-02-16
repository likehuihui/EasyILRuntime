using System;
using System.Collections.Generic;
using System.Text;
using MessagePackage.Struct;
using KCBF;

namespace KCBF
{
    //框架回调接口
    public interface U3D_KCBF_Interface
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
        void GAME_CONFIG(KCBF_.CMD_S_GameConfig game_config);
        void FISH_TRACE(KCBF_.CMD_S_FishTrace fish_reace);
        void EXCHANGE_FISHSCORE(KCBF_.CMD_S_ExchangeFishScore exchangeFishScore);
        void USER_FIRE(KCBF_.CMD_S_UserFire user_fire);
        void CATCH_FISH(KCBF_.CMD_S_CatchFish catchFish);
        void BULLET_ION_TIMEOUT(KCBF_.CMD_S_BulletIonTimeout liZiPao);
        void LOCK_TIMEOUT();
        void CATCH_SWEEP_FISH(KCBF_.CMD_S_CatchSweepFish catchSweepFish);
        void CATCH_SWEEP_FISH_RESULT(KCBF_.CMD_S_CatchSweepFishResult catchSweepFishResult);
        void HIT_FISH_LK(KCBF_.CMD_S_HitFishLK hitFishLK);
        void SWITCH_SCENE(KCBF_.CMD_S_SwitchScene sw);
        void REPLY_NOW_INFO(KCBF_.CMD_S_FishArrayInfo fishArrayInfo);
        void CIRCLE_FISH_INFO(KCBF_.CMD_S_CircleFishArrayInfo circleFishArrayInfo);
        void SPECIAL_FISH_INFO(KCBF_.CMD_S_SpecialInfo tsyu);
        void MESSAGE_FOR_CATCH_FISH(KCBF_.CMD_S_MessageForCatchFish messageForCatchFish);
        void SEND_ADMIN_CONFIG(KCBF_.CMD_S_ADMIN_CONFIG aDMIN_CONFIG);
        void SAVE_ADMIN_CONFIG_RESULT(KCBF_.CMD_S_ADMIN_CONFIG_RESULT aDMIN_CONFIG_RESULT);
    }
}
