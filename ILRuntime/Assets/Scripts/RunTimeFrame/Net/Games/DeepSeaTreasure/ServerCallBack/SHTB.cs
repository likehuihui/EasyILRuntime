using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Operation;
using MessagePackage.Struct;

namespace SHTB
{
    public partial class SHTB_ : Interface_Game
    {
        private SHTB_()
        {

        }
        private static SHTB_ SHTB__ = null;
        /// <summary>
        /// 深海探宝U3D回调函数
        /// </summary>
        public SHTB.U3D_SHTB_Interface SHTBInterFace = null;
        public static SHTB_ SHTBInstance
        {
            get
            {
                if (SHTB__ == null)
                {
                    SHTB__ = new SHTB_();
                }
                return SHTB__;
            }
        }
        public void Error(string errStr)
        {
            if (SHTBInterFace != null)
            {
                SHTBInterFace.Error(errStr);
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
                //case GameOperations.MDM_GR_INSURE://银行命令
                //    return HandleInsureMsg(mnCmd, sbCmd, readBuffer);
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
                case GameOperations.SUB_GR_MATCH_INFO:
                //return HandleMatchInfoMsg(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_WAIT_TIP:
                //return HandleMatchWaitTip(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_RESULT:     //淘汰 优胜 奖状
                                                             //return HandleMatchResult(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_USER_WAIT_DISTRIBUTE:
                    return true;
                case GameOperations.SUB_GR_MATCH_PROMOTION: //晋级
                                                            //return HandleMatchPromotion(maincmd, subCmd, readbuffer);
                case GameOperations.SUB_GR_MATCH_ELIMINATE:
                    return true;
                case GameOperations.SUB_GF_SYSTEM_MESSAGE_NEW:
                    return HandleFrameSystemMessageMsgNew(maincmd, subCmd, readbuffer);
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
            if (SHTBInterFace != null)
            {
                SHTBInterFace.GAME_GameStatus(msg);
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
            msg.szString = readbuffer.ReadString(msg.wLength*2);            
            if (SHTBInterFace != null)
            {
                SHTBInterFace.GAME_SystemMessage(msg.szString);
            }
            return true;
        }
        public bool HandleFrameSystemMessageMsgNew(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_CM_SystemMessageNew msg = new CMD_CM_SystemMessageNew(readbuffer);
            Loom.QueueOnMainThread(() =>
            {
                //NoticeManager.SendNoticeQuick(HallNotice.NEW_MARQUREE, new MarqueeDataVO(msg));
            });
          
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
            const byte GAME_SCENE_END = 101;

            switch (msg_Status.cbGameStatus)
            {
                case GAME_STATUS_FREE:
                    {

                    }
                    break;
                case GAME_STATUS_PLAY:
                case GAME_SCENE_END:
                    {

                    }
                    break;
                default:
                    // Error("HandleSceneMsg 未知状态：" + GAME_STATUS_WAIT);
                    break;
            }
            if (SHTBInterFace != null)
            {
                SHTBInterFace.GAME_SCENE_OK();
            }
            return true;
        }
    }   
}
