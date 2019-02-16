using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;

namespace ShuiHuZhuan
{
    public partial class ShuiHuZhuan_ : Interface_Game
    {
        private ShuiHuZhuan_()
        {

        }
        private static ShuiHuZhuan_ ShuiHuZhuan__ = null;
        /// <summary>
        /// 水浒传U3D回调函数
        /// </summary>
        public U3D_ShuiHuZhuan_Interface ShuiHuZhuanInterFace = null;
        public static ShuiHuZhuan_ ShuiHuZhuanInstance
        {
            get
            {
                if (ShuiHuZhuan__ == null)
                {
                    ShuiHuZhuan__ = new ShuiHuZhuan_();
                }
                return ShuiHuZhuan__;
            }
        }
        public void Error(string errStr)
        {
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.Error(errStr);
            }
        }
        public bool HandleReadNetData(ushort mnCmd, ushort sbCmd, CyNetReadBuffer readBuffer)
        {
            switch (mnCmd)
            {
                case GameOperations.MDM_GF_FRAME://框架命令
                    return HandleFrameMsg(mnCmd, sbCmd, readBuffer);
                case GameOperations.MDM_GF_GAME://游戏命令
                    return HandleGameMsg(mnCmd, sbCmd, readBuffer);
                case GameOperations.SUB_GF_SYSTEM_MESSAGE_NEW:
                    return HandleSceneMsgNew(mnCmd, sbCmd, readBuffer);
                default:
                    Error("未能解析的主命令：" + mnCmd);
                    break;
            }
            return true;
            
        }

        /// <summary>
        /// 框架命令
        /// </summary>
        /// <param name="maincmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleFrameMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            switch (subCmd)
            {
                case GameOperations.SUB_GF_GAME_STATUS:
                    return HandleGameStatusMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GF_SYSTEM_MESSAGE:
                    return HandleFrameSystemMessageMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GF_GAME_SCENE:
                    return HandleSceneMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GF_SYSTEM_MESSAGE_NEW:
                    return HandleSceneMsgNew(maincmd, subCmd, readbuffer);
                    // case 203SUB_GF_SYSTEM_MESSAGE_NEW
            }
            Error("未能解析的错误 ZML:" + maincmd + "  ZIML:" + subCmd);
            return true;
        }

        /// <summary>
        /// 桌子状态
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleGameStatusMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GF_GameStatus msg = new CMD_GF_GameStatus();
            msg.cbGameStatus = readbuffer.ReadByte();
            msg.cbAllowLookon = readbuffer.ReadByte();
            Console.WriteLine("游戏状态{0} 是否旁观{1}", msg.cbGameStatus, msg.cbAllowLookon);
            msg_Status = msg;
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.GAME_GameStatus(msg);
            }
            return true;
        }
        /// <summary>
        /// 当收到游戏状态时 此值保存给断线重连使用
        /// </summary>
        public CMD_GF_GameStatus msg_Status;
        /// <summary>
        /// 发送框架系统消息
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleFrameSystemMessageMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_CM_SystemMessage msg = new CMD_CM_SystemMessage();
            msg.wType = readbuffer.ReadUshort();
            msg.wLength = readbuffer.ReadUshort();
            msg.szString = readbuffer.ReadString(msg.wLength * 2);
            Console.WriteLine("水浒传:" + msg.szString);
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.GAME_SystemMessage(msg.szString);
            }
            return true;
        }
        private bool HandleSceneMsgNew(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_CM_SystemMessageNew msg = new CMD_CM_SystemMessageNew(readbuffer);
            //NoticeManager.SendNoticeQuick(HallNotice.NEW_MARQUREE, new MarqueeDataVO(msg));
            return true;
        }
        /// <summary>
        /// 收到场景消息
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleSceneMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            const byte GAME_STATUS_FREE = 0;					    //空闲状态
            const byte GAME_STATUS_PLAY = 100;						//游戏状态
            //结束原因
            //#define GER_NO_PLAYER					    0x10								//没有玩家
            ////游戏状态
            const byte GS_TK_FREE = GAME_STATUS_FREE;	            //等待开始
            const byte GS_TK_PLAYING = GAME_STATUS_PLAY;		    //游戏进行
            switch (msg_Status.cbGameStatus)
            {
                case GS_TK_FREE:
                case GS_TK_PLAYING:
                    {
                        CMD_S_GameStatus tagScene = new CMD_S_GameStatus();

                        tagScene.game_version = readbuffer.ReadByte();
                        tagScene.exchange_credit_score_ = readbuffer.ReadInt();
                        tagScene.credit_score_ = readbuffer.ReadLong();
                        tagScene.kMaxBetScore = readbuffer.ReadInt();
                        tagScene.kCellBetScore = readbuffer.ReadInt();
                        if (ShuiHuZhuanInterFace != null)
                        {
                            ShuiHuZhuanInterFace.GAME_SCENE_INFO(tagScene);
                        }
                    }
                    break;
                default:
                    Error("HandleSceneMsg 未知状态：" + msg_Status.cbGameStatus);
                    break;
            }
            if (ShuiHuZhuanInterFace != null)
            {
                ShuiHuZhuanInterFace.GAME_SCENE_OK();
            }
            return true;
        }
    
        //空闲状态
        public struct CMD_S_GameStatus
        {
            public byte                         game_version;						
            public int                          exchange_credit_score_;
            public long                          credit_score_;
            public int                          kMaxBetScore;
            public int                          kCellBetScore;
        };
    }
}
