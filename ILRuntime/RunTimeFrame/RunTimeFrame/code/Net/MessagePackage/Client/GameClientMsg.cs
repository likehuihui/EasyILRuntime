using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CYNetwork;
using System.Runtime.InteropServices;
using MessagePackage.Struct;
using MessagePackage.Operation;

namespace MessagePackage
{
    public partial class GameMessageMgr
    {
        /// <summary>
        /// ID登陆
        /// </summary>
        /// <param name="logon"></param>
        public void LogonUserID(ref CMD_GR_LogonUserID logon)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            logon.dwPlazaVersion = MessageMgr.CurMsgMgr.PlazaVersion;
            logon.dwFrameVersion = MessageMgr.CurMsgMgr.FrameVersion;
            logon.dwProcessVersion = MessageMgr.CurMsgMgr.ProcessVersion;
            //logon.dwPlazaVersion = MessageMgr.PROCESS_VERSION(6, 8, 8);
            //logon.dwFrameVersion = MessageMgr.PROCESS_VERSION(6, 8, 8);
            //logon.dwProcessVersion = MessageMgr.PROCESS_VERSION(6, 8, 8);
            logon.szMachineID = "PH-";
            buffer.Write(logon.dwPlazaVersion);
            buffer.Write(logon.dwFrameVersion);
            buffer.Write(logon.dwProcessVersion);
            buffer.Write(logon.dwUserID);
            buffer.Write(logon.szPassword,66);
            buffer.Write(logon.szServerPasswd, 66);
            buffer.Write(logon.szMachineID,66);
            buffer.Write(logon.wKindID);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_LOGON, GameOperations.SUB_GR_LOGON_USERID, buffer);    
        }

        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="logon"></param>
        public void LogonMobile(ref CMD_GR_LogonMobile logon)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(logon.wGameID);
            buffer.Write(logon.dwProcessVersion);
            buffer.Write(logon.cbDeviceType);
            buffer.Write(logon.wBehaviorFlags);
            buffer.Write(logon.wPageTableCount);
            buffer.Write(logon.dwUserID);
            buffer.Write(logon.szPassword,66);
            buffer.Write(logon.szMachineID,66);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_LOGON, GameOperations.SUB_GR_LOGON_MOBILE, buffer);
        }

        /// <summary>
        /// 账号登陆
        /// </summary>
        /// <param name="logon"></param>
        public void LogonAccounts(ref CMD_GR_LogonAccounts logon)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            logon.dwPlazaVersion = MessageMgr.CurMsgMgr.PlazaVersion;
            logon.dwFrameVersion = MessageMgr.CurMsgMgr.FrameVersion;
            logon.dwProcessVersion = MessageMgr.CurMsgMgr.ProcessVersion;
            logon.szMachineID = "PH-";
            buffer.Write(logon.dwPlazaVersion);
            buffer.Write(logon.dwFrameVersion);
            buffer.Write(logon.dwProcessVersion);
            buffer.Write(logon.szPassword,66);
            buffer.Write(logon.szAccounts,64);
            buffer.Write(logon.szMachineID,66);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_LOGON, GameOperations.SUB_GR_LOGON_ACCOUNTS, buffer);
        }

        /// <summary>
        /// 用户规则
        /// </summary>
        /// <param name="user"></param>
        public void UserRule(ref CMD_GR_UserRule user, string tablePasswd)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.cbRuleMask);
            buffer.Write(user.wMinWinRate);
            buffer.Write(user.wMaxFleeRate);
            buffer.Write(user.lMaxGameScore);
            buffer.Write(user.lMinGameScore);

            if (null != tablePasswd && !tablePasswd.Equals(""))
            {
                CSendPacketHelper helper = new CSendPacketHelper(buffer);
                helper.WriteValue(tablePasswd, ConstDefine.DTP_GR_TABLE_PASSWORD);
            }

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_RULE, buffer);
        }

        /// <summary>
        /// 用户举手
        /// </summary>
        /// <param name="user"></param>
        public void UserOK()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer(8);
            //发送准备
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_FRAME, GameOperations.SUB_GF_USER_READY, buffer);
        }

        /// <summary>
        /// 用户旁观
        /// </summary>
        /// <param name="user"></param>
        public void UserLookon(ref CMD_GR_UserLookon user)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wTableID);
            buffer.Write(user.wChairID);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_LOOKON, buffer);
        }

        /// <summary>
        /// 用户坐下
        /// </summary>
        /// <param name="user"></param>
        public void UserSitDown(ref CMD_GR_UserSitDown user)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wTableID);
            buffer.Write(user.wChairID);
            buffer.Write(user.szPassword,66);
            
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_SITDOWN, buffer);
        }

        public void UserLeavematch()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_MATCH, GameOperations.SUB_GR_LEAVE_MATCH, buffer);
        }

        /// <summary>
        /// 起立请求
        /// </summary>
        /// <param name="user"></param>
        public void UserStandUp(ref CMD_GR_UserStandUp user)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wTableID);
            buffer.Write(user.wChairID);
            buffer.Write(user.cbForceLeave);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_STANDUP, buffer);
        }
        /// <summary>
        /// 用户聊天
        /// </summary>
        /// <param name="user"></param>
        public void UserChat(ref CMD_GR_C_UserChat user)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wChatLength);
            buffer.Write(user.dwChatColor);
            buffer.Write(user.dwTargetUserID);
            buffer.Write(user.szChatString);
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_CHAT, buffer);
        }

        /// <summary>
        /// 用户表情
        /// </summary>
        /// <param name="user"></param>
        public void UserExpression(ref CMD_GR_C_UserExpression user)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wItemIndex);
            buffer.Write(user.dwTargetUserID);
           
            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_EXPRESSION, buffer);
        }

        /// <summary>
        /// 用户私聊
        /// </summary>
        /// <param name="user"></param>
        public void WisperChat(ref CMD_GR_C_WisperChat user)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wChatLength);
            buffer.Write(user.dwChatColor);
            buffer.Write(user.dwTargetUserID);
            buffer.Write(user.szChatString, user.wChatLength);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_WISPER_CHAT, buffer);
        }

        /// <summary>
        //私聊表情
        /// </summary>
        /// <param name="logon"></param>
        public void WisperExpression(ref CMD_GR_C_WisperExpression wisper)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(wisper.wItemIndex);
            buffer.Write(wisper.dwTargetUserID);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_WISPER_EXPRESSION, buffer);
        }

        /// <summary>
        /// 购买道具
        /// </summary>
        /// <param name="proper"></param>
        public void PropertyBuy(ref CMD_GR_C_PropertyBuy proper)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(proper.cbRequestArea);
            buffer.Write(proper.cbConsumeScore);
            buffer.Write(proper.wItemCount);
            buffer.Write(proper.wPropertyIndex);
            buffer.Write(proper.dwTargetUserID);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_PROPERTY_BUY, buffer);
        }

        /// <summary>
        /// 发送喇叭
        /// </summary>
        /// <param name="send"></param>
        public void SendTrumpet(ref CMD_GR_C_SendTrumpet send)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(send.cbRequestArea);
            buffer.Write(send.wPropertyIndex);
            buffer.Write(send.TrumpetColor);
            buffer.Write(send.szTrumpetContent, 256);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_PROPERTY_TRUMPET, buffer);
        }

        /// <summary>
        /// 邀请用户请求
        /// </summary>
        /// <param name="user"></param>
        public void UserInviteReq(ref CMD_GR_UserInviteReq user)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wTableID);
            buffer.Write(user.dwUserID);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_INVITE_REQ, buffer);
        }
        
        /// <summary>
        /// 用户拒绝黑名单坐下
        /// </summary>
        /// <param name="user"></param>
        public void UserRepulseSit(ref CMD_GR_UserRepulseSit user){

           CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wTableID);
            buffer.Write(user.wChairID);
            buffer.Write(user.dwUserID);
            buffer.Write(user.dwRepulseUserID);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_REPULSE_SIT, buffer);
        }

        /// <summary>
        /// 踢出用户
        /// </summary>
        /// <param name="kick"></param>
        public void KickUser(ref CMD_GR_KickUser kick)
        {
           CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(kick.dwTargetUserID);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_KICK_USER, buffer);
        }

        /// <summary>
        /// 请求用户信息
        /// </summary>
        /// <param name="user"></param>
        public void UserInfoReq(ref CMD_GR_UserInfoReq user)
        {
           CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.dwUserIDReq);
            buffer.Write(user.wTablePos);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_USER, GameOperations.SUB_GR_USER_INFO_REQ, buffer);
        }

        /// <summary>
        /// 游戏配置
        /// </summary>
        /// <param name="option"></param>
        public void GameOption(ref CMD_GF_GameOption option)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(option.cbAllowLookon);
            buffer.Write(option.dwFrameVersion);
            buffer.Write(option.dwClientVersion);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_FRAME, GameOperations.SUB_GF_GAME_OPTION, buffer);
        }
        
        /// <summary>
        /// 用户聊天
        /// </summary>
        /// <param name="user"></param>
        public void GameUserChat(ref CMD_GF_C_UserChat user)
        {
           CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wChatLength);
            buffer.Write(user.dwChatColor);
            buffer.Write(user.dwTargetUserID);
            buffer.Write(user.szChatString,256);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_FRAME, GameOperations.SUB_GF_USER_CHAT, buffer);
        }

        /// <summary>
        /// 用户表情
        /// </summary>
        /// <param name="user"></param>
        public void GameUserExpression(ref CMD_GF_C_UserExpression user)
        {
           CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.wItemIndex);
            buffer.Write(user.dwTargetUserID);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_FRAME, GameOperations.SUB_GF_USER_EXPRESSION, buffer);
        }
         
        /// <summary>
        /// 旁观配置
        /// </summary>
        /// <param name="user"></param>
        public void LookonConfig(ref CMD_GF_LookonConfig user)
        {
           CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(user.dwUserID);
            buffer.Write(user.cbAllowLookon);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GF_FRAME, GameOperations.SUB_GF_LOOKON_CONFIG, buffer);
        }

        //报名比赛
        public void SendUserSignup(ref CMD_GP_MatchSignup MatchSignup)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MatchSignup.szMachineID = "PH-";
            buffer.Write(MatchSignup.wServerID);
            buffer.Write(MatchSignup.dwMatchID);
            buffer.Write(MatchSignup.dwMatchNO);
            buffer.Write(MatchSignup.dwUserID);
            buffer.Write(MatchSignup.szPassword, 66);
            buffer.Write(MatchSignup.szMachineID, 66);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_MATCH_SIGNUP, buffer);
        }

        //取消报名
        public void SendUserUnSignup(ref CMD_GP_MatchUnSignup MatchUnSignup)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            MatchUnSignup.szMachineID = "PH-";
            buffer.Write(MatchUnSignup.wServerID);
            buffer.Write(MatchUnSignup.dwMatchID);
            buffer.Write(MatchUnSignup.dwMatchNO);
            buffer.Write(MatchUnSignup.dwUserID);
            buffer.Write(MatchUnSignup.szPassword, 66);
            buffer.Write(MatchUnSignup.szMachineID, 66);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(ConstDefine.MDM_GP_USER_SERVICE, UserOperations.SUB_GP_MATCH_UNSIGNUP, buffer);
        }

        public void SendMatchFee(ref long lMatchFee)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(lMatchFee);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_MATCH, GameOperations.SUB_GR_MATCH_FEE, buffer);
        }

        //创建房卡房间
        public void SendCreateRoomCardRoom(uint dwUserID, ushort wKindID, ushort wServerID, byte[] cbRule)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(dwUserID);
            buffer.Write(wKindID);
            buffer.Write(wServerID);
            buffer.Write(cbRule, 3);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_ROOM_CARD, GameOperations.SUB_RC_C_ROOM_CREATE, buffer);
        }
        //创建房卡房间
        public void SendJoinRoomCardRoom(uint dwUserID, ushort wKindID, ushort wServerID, uint dwRoomNumKey)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(dwUserID);
            buffer.Write(wKindID);
            buffer.Write(wServerID);
            buffer.Write(dwRoomNumKey);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_ROOM_CARD, GameOperations.SUB_RC_C_ROOM_JOIN, buffer);
        }
        //解散请求
        public void SendRoomCardDissolve(uint dwUserID, uint dwRoomNumKey, byte cbAgree ,byte cbResult)
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(dwUserID);
            buffer.Write(dwRoomNumKey);
            buffer.Write(cbAgree);
            buffer.Write(cbResult);

            MessageMgr.CurMsgMgr.SendMsg2GameSvr(GameOperations.MDM_GR_ROOM_CARD, GameOperations.SUB_RC_C_ROOM_DISSOLVE, buffer);
        }
    }
}

