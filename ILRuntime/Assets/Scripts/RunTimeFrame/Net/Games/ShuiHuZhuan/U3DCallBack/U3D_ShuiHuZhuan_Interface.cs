using System;
using System.Collections.Generic;
using System.Text;
using MessagePackage.Struct;

namespace ShuiHuZhuan
{
    //框架回调接口
    public interface U3D_ShuiHuZhuan_Interface
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
        /// 新消息
        /// </summary>
        void GAME_SystemMessageNew(CMD_CM_SystemMessageNew msg);
        /// <summary>
        /// 游戏场景之
        /// </summary>
        /// <param name="free"></param>
        void GAME_SCENE_INFO(ShuiHuZhuan_.CMD_S_GameStatus free);

        /// <summary>
        /// 场景1
        /// </summary>
        void Game_Scene1Start(ShuiHuZhuan_.CMD_S_Scene1Start msg);
        /// <summary>
        /// 场景2
        /// </summary>
        void Game_Scene2Result(ShuiHuZhuan_.CMD_S_Scene2Result msg);
        /// <summary>
        /// 场景3
        /// </summary>
        void Game_Scene3Result(ShuiHuZhuan_.CMD_S_Scene3Result msg);

        //得分消息
        void Game_GetScore(long nowScore);

        void Game_Check_Net();

        void GM_IsMaster(bool isMaster);

        void GM_ControlResult(ShuiHuZhuan_.CMD_S_ControlResult msg);

        void GM_GM_B_OR_WList(ShuiHuZhuan_.CMD_S_GM_B_OR_WList msg);
        void Bet_ABNORMAL();

        /// <summary>
        /// 更新奖池
        /// </summary>
        /// <param name="msg"></param>
        void UpdateRevenue(ShuiHuZhuan_.CMD_S_UpdateRevJackpot msg);
    }
}
