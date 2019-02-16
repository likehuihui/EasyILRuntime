using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Struct;
using MessagePackage.Operation;

namespace MessagePackage
{

    public partial class GameMessageMgr
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleLogonSuccessMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_LogonSuccess msg = new CMD_GR_LogonSuccess();
            msg.dwUserRight = readbuffer.ReadUint();
            msg.dwMasterRight = readbuffer.ReadUint();
            Console.WriteLine("用户权限{0}   管理权限{1}", msg.dwUserRight, msg.dwMasterRight);
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.LogonSucc(msg);
            }

            return true;
        }

        /// <summary>
        /// 登录失败
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleLogonFailureMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_LogonFailure msg = new CMD_GR_LogonFailure();
            msg.lErrorCode = readbuffer.ReadInt();
            msg.szDescribeString = readbuffer.ReadString();
            Console.WriteLine("错误代码{0}  错误信息{1}", msg.lErrorCode, msg.szDescribeString);
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.LogonFailed(msg.szDescribeString);
            }
            return true;
        }
        
        /// <summary>
        /// 房间配置
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleConfigServerMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_ConfigServer msg = new CMD_GR_ConfigServer();
            msg.wTableCount = readbuffer.ReadUshort();
            msg.wChairCount = readbuffer.ReadUshort();
            msg.wServerType = readbuffer.ReadUshort();
            msg.dwServerRule = readbuffer.ReadUint();
            Console.WriteLine("桌子数{0}///椅子数{1}///房间类型{2}///房间规则{3}",
                msg.wTableCount,
                msg.wChairCount,
                msg.wServerType,
                msg.dwServerRule);

            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.ROOM_Config(msg);
            }
            return true;
        }

        /// <summary>
        /// 列表配置
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleConfigColumnMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_ConfigColumn msg = new CMD_GR_ConfigColumn();
            msg.cbColumnCount = readbuffer.ReadByte();
            msg.ColumnItem = new tagColumnItem[msg.cbColumnCount];

            for (int i = 0; i < msg.cbColumnCount; i++)
            {
                msg.ColumnItem[i].cbColumnWidth = readbuffer.ReadByte();
                msg.ColumnItem[i].cbDataDescribe = readbuffer.ReadByte();
                msg.ColumnItem[i].szColumnName = readbuffer.ReadString(32);
                Console.WriteLine("列表名字：{0}", msg.ColumnItem[i].szColumnName);
            }

            return true;
        }

        /// <summary>
        /// 道具配置
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleConfigPropertyMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_ConfigProperty msg = new CMD_GR_ConfigProperty();
            msg.cbPropertyCount = readbuffer.ReadByte();
            msg.PropertyInfo = new tagPropertyInfo[msg.cbPropertyCount];

            for (int i = 0; i < msg.cbPropertyCount; i++)
            {
                msg.PropertyInfo[i].wIndex = readbuffer.ReadUshort();
                msg.PropertyInfo[i].wDiscount = readbuffer.ReadUshort();
                msg.PropertyInfo[i].wIssueArea = readbuffer.ReadUshort();
                msg.PropertyInfo[i].lPropertyGold = readbuffer.ReadLong();
                msg.PropertyInfo[i].dPropertyCash = readbuffer.ReadDouble();
                msg.PropertyInfo[i].lSendLoveLiness = readbuffer.ReadLong();
                msg.PropertyInfo[i].lRecvLoveLiness = readbuffer.ReadLong();
                Console.WriteLine("道具配置信息 道具价格:"+ msg.PropertyInfo[i].lPropertyGold);
            }


            return true;
        }

        /// <summary>
        /// 配置完成
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleConfigFinishMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            //if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(mainCmd, subCmd, null);
            Console.WriteLine("配置信息接受完毕");
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.Config_OK();
            }
            return true;
        }

        /// <summary>
        /// 玩家数据
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleUserInfoEnterMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            tagUserInfoHead userInfo = new tagUserInfoHead();
            userInfo.dwGameID = readbuffer.ReadUint();
            userInfo.dwUserID = readbuffer.ReadUint();
            userInfo.dwGroupID = readbuffer.ReadUint();
            userInfo.wFaceID = readbuffer.ReadUint();
            userInfo.dwCustomID = readbuffer.ReadUint();
            userInfo.cbClientType = readbuffer.ReadByte();
            userInfo.bIsAndroid = readbuffer.ReadBoolean();
            userInfo.cbGender = readbuffer.ReadByte();
            userInfo.cbMemberOrder = readbuffer.ReadByte();
            userInfo.cbMasterOrder = readbuffer.ReadByte();
            userInfo.wTableID = readbuffer.ReadUshort();
            userInfo.wChairID = readbuffer.ReadUshort();
            userInfo.cbUserStatus = readbuffer.ReadByte();
            userInfo.lScore = readbuffer.ReadLong();
            userInfo.lGrade = readbuffer.ReadLong();
            userInfo.lInsure = readbuffer.ReadLong();
            userInfo.lIngot = readbuffer.ReadLong();
            userInfo.dwWinCount = readbuffer.ReadUint();
            userInfo.dwLostCount = readbuffer.ReadUint();
            userInfo.dwDrawCount = readbuffer.ReadUint();
            userInfo.dwFleeCount = readbuffer.ReadUint();
            userInfo.dwExperience = readbuffer.ReadUint();
            userInfo.lLoveLiness = readbuffer.ReadInt();

            if(readbuffer.CurrentPos < readbuffer.Length)
            {
                userInfo.Infos = new Dictionary<int, object>();
                CReadPacketHelper helper = new CReadPacketHelper(readbuffer);
                while (readbuffer.CurrentPos < readbuffer.Length)
                {
                    KeyValuePair<int, object> val = helper.ReadValue();
                    userInfo.Infos.Add(val.Key, val.Value);
                    Console.WriteLine(val.Value);
                    Console.WriteLine(val.Key);
                }
            }

            Console.WriteLine("收到用户进入房间消息");

            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.User_EnterROOM(userInfo);
            }
            return true;
        }

        /// <summary>
        /// 系统消息
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleSystemMessageMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_CM_SystemMessage msg = new CMD_CM_SystemMessage();
            msg.wType = readbuffer.ReadUshort();
            msg.wLength = readbuffer.ReadUshort();
            msg.szString = readbuffer.ReadString();

            Console.WriteLine(msg.szString);
            if (MessageMgr.ROOM_Interface != null)
            {
                //关闭处理
//                 ushort wType = (ushort)msg.wType;
//                 if ((msg.wType & (GameOperations.SMT_CLOSE_ROOM | GameOperations.SMT_CLOSE_LINK)) != 0)
//                 {
//                     //状态设置
//                     MessageMgr.ROOM_Interface.CloseRoom();
//                     if (MessageMgr.CurMsgMgr.GameMessageHelper != null)
//                     {
//                        /* MessageMgr.CurMsgMgr.GameMessageHelper.UserStandUp()*/
//                     }
//                 }

                MessageMgr.ROOM_Interface.SystemMessage(msg.szString);
            }

            return true;
        }

        /// <summary>
        ///  请求失败
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleRequestFailureMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_RequestFailure msg = new CMD_GR_RequestFailure();
            msg.lErrorCode = readbuffer.ReadInt();
            msg.szDescribeString = readbuffer.ReadString(readbuffer.Length - readbuffer.CurrentPos);
            Console.WriteLine("请求失败 原因："+ msg.szDescribeString);

            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.RequestShiBai(msg);
            }
            return true;
        }        

        /// <summary>
        /// 用户聊天
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleUserChatMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_S_UserChat msg = new CMD_GR_S_UserChat();
            msg.wChatLength = readbuffer.ReadUshort();
            msg.dwChatColor = readbuffer.ReadUint();
            msg.dwSendUserID = readbuffer.ReadUint();
            msg.dwTargetUserID = readbuffer.ReadUint();
            msg.szChatString = readbuffer.ReadString(readbuffer.Length - readbuffer.CurrentPos);

            if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 用户表情
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleUserExpressionMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_S_UserExpression msg = new CMD_GR_S_UserExpression();
            msg.wItemIndex = readbuffer.ReadUshort();
            msg.dwSendUserID = readbuffer.ReadUint();
            msg.dwTargetUserID = readbuffer.ReadUint();

            if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 用户私聊
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleWisperChatMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_S_WisperChat msg = new CMD_GR_S_WisperChat();
            msg.wChatLength = readbuffer.ReadUshort();
            msg.dwChatColor = readbuffer.ReadUint();
            msg.dwSendUserID = readbuffer.ReadUint();
            msg.dwTargetUserID = readbuffer.ReadUint();
            msg.szChatString = readbuffer.ReadString();

            if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 私聊表情
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleWisperExpressionMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_S_WisperExpression msg = new CMD_GR_S_WisperExpression();
            msg.wItemIndex = readbuffer.ReadUshort();
            msg.dwSendUserID = readbuffer.ReadUint();
            msg.dwTargetUserID = readbuffer.ReadUint();

            if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 道具失败
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandlePropertyFailureMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_PropertyFailure msg = new CMD_GR_PropertyFailure();
            msg.wRequestArea = readbuffer.ReadUshort();
            msg.lErrorCode = readbuffer.ReadInt();
            msg.szDescribeString = readbuffer.ReadString(518);

            if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 道具成功
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandlePropertySuccessMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_S_PropertySuccess msg = new CMD_GR_S_PropertySuccess();
            msg.cbRequestArea = readbuffer.ReadByte();
            msg.wItemCount = readbuffer.ReadUshort();
            msg.wPropertyIndex = readbuffer.ReadUshort();
            msg.dwSourceUserID = readbuffer.ReadUint();
            msg.dwTargetUserID = readbuffer.ReadUint();

            if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(mainCmd, subCmd, msg);

            return true;
        }

        /// <summary>
        /// 发送喇叭
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleSendTrumpetMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_S_SendTrumpet msg = new CMD_GR_S_SendTrumpet();
            msg.wPropertyIndex = readbuffer.ReadUshort();
            msg.dwSendUserID = readbuffer.ReadUint();
            msg.TrumpetColor = readbuffer.ReadUint();
            msg.szSendNickName = readbuffer.ReadString(64);
            msg.szTrumpetContent = readbuffer.ReadString(256);

            if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(mainCmd, subCmd, msg);
            return true;
        }

        /// <summary>
        /// 邀请用户
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleUserInviteMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_UserInvite msg = new CMD_GR_UserInvite();
            msg.wTableID = readbuffer.ReadUshort();
            msg.dwUserID = readbuffer.ReadUint();

            if (MessageMgr.CurMsgMgr.GameMsgCallBack != null) MessageMgr.CurMsgMgr.GameMsgCallBack(mainCmd, subCmd, msg);

            return true;
        }


        /// <summary>
        /// 桌子状态
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleTableStatusMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_TableStatus msg = new CMD_GR_TableStatus();
            msg.wTableID = readbuffer.ReadUshort();
            msg.TableStatus = new tagTableStatus();
            msg.TableStatus.cbTableLock = readbuffer.ReadByte();
            msg.TableStatus.cbPlayStatus = readbuffer.ReadByte();

            Console.WriteLine("桌子是否有锁{0}  是否在游戏中{1}  桌子ID {2}  桌子状态{3}", msg.TableStatus.cbTableLock, msg.TableStatus.cbPlayStatus
                , msg.wTableID, msg.TableStatus);

            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.Table_Status(msg);
            }
            return true;
        }
        /// <summary>
        /// 桌子信息
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleTableInfo(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_TableInfo ti = new CMD_GR_TableInfo();
            ushort count = ti.wTableCount = readbuffer.ReadUshort();
            if(count > 0)
            {
                ti.TableStatusArray = new tagTableStatus[count];
                for(int i = 0;i < count;i++)
                {
                    tagTableStatus status = new tagTableStatus();
                    status.cbTableLock = readbuffer.ReadByte();	
                    status.cbPlayStatus = readbuffer.ReadByte();
                    status.lCellScore = readbuffer.ReadUint();
                    ti.TableStatusArray[i] = status;
                    Console.WriteLine("桌子是否有锁{0}  是否在游戏中{1}  桌子ID {2}", status.cbTableLock, status.cbPlayStatus, i);
                }
            }

            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.Table_Info(ti);
            }
            return true;
        }
        /// <summary>
        /// 用户状态发送改变
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleUserStatus(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_UserStatus msg = new CMD_GR_UserStatus();
            msg.dwUserID = readbuffer.ReadUint() ;
            msg.wTableID = readbuffer.ReadUshort();
            msg.wChairID = readbuffer.ReadUshort();
            msg.cbUserStatus = readbuffer.ReadByte();
            Console.WriteLine("用户状态发生改变 dwUserID："+ msg.dwUserID);
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.User_Status(msg);
            }
            return true;
        }
        /// <summary>
        /// 用户分数发送改变
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        private bool HandleUserFenShu(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_UserScore msg = new CMD_GR_UserScore();
            msg.dwUserID = readbuffer.ReadUint();
            msg.UserScore.lScore = readbuffer.ReadLong();
            msg.UserScore.lGrade = readbuffer.ReadLong();
            msg.UserScore.lInsure = readbuffer.ReadLong();

            msg.UserScore.dwWinCount = readbuffer.ReadUint();
            msg.UserScore.dwLostCount = readbuffer.ReadUint();
            msg.UserScore.dwDrawCount = readbuffer.ReadUint();
            msg.UserScore.dwFleeCount = readbuffer.ReadUint();

            msg.UserScore.dwUserMedal = readbuffer.ReadUint();
            msg.UserScore.dwExperience = readbuffer.ReadUint();
            msg.UserScore.lLoveLiness = readbuffer.ReadInt();

            Console.WriteLine("用户分数改变鸟：" + msg.dwUserID);
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.User_FenShu(msg);
            }
            return true;
        }

        private bool HandleMatchResult(ushort mainCmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GP_MatchSignupResult msg = new CMD_GP_MatchSignupResult();
            msg.bSignup = readbuffer.ReadBoolean();
            msg.bSuccessed = readbuffer.ReadBoolean();
            Console.WriteLine("报名标识{0}   成功标识{1}", msg.bSignup, msg.bSuccessed);
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.MatchSignupResult(msg);
            }

            return true;
        }
        private bool HandleMatchNum(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_Match_Num msg = new CMD_GR_Match_Num();
            msg.dwWaitting = readbuffer.ReadUint();
            msg.dwTotal = readbuffer.ReadUint();
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.MatchNum(msg);
            }
            return true;
        }

        private bool HandleMatchFee(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_Match_Fee msg = new CMD_GR_Match_Fee();
            msg.lMatchFee = readbuffer.ReadLong();
            msg.szNotifyContent = readbuffer.ReadString();
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.MatchFee(msg);
            }
            return true;
        }

        private bool HandleMatchStatus(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            byte cbMatchStatus = readbuffer.ReadByte();
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.MatchStatus(cbMatchStatus);
            }
            return true;
        }

        private bool HandleMatchGoldUpdate(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_GR_MatchGoldUpdate msg = new CMD_GR_MatchGoldUpdate();
            msg.lCurrGold = readbuffer.ReadLong();
            msg.lCurrIngot = readbuffer.ReadLong();
            msg.dwCurrExprience = readbuffer.ReadUint();
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.MatchGoldUpdate(msg);
            }
            return true;
        }
        private bool HandleCreateRoomMsg(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_RC_S_RoomCardData msg = new CMD_RC_S_RoomCardData();
            msg.bSucess = readbuffer.ReadBoolean();
            msg.dwUserID = readbuffer.ReadUint();
            msg.dwRoomNumKey = readbuffer.ReadUint();
            msg.wTableID = readbuffer.ReadUshort();
            msg.wChairID = readbuffer.ReadUshort();
            msg.wCurPlayCount = readbuffer.ReadUshort();
            msg.wPlayCount = readbuffer.ReadUshort();
            msg.szReasonInfo = readbuffer.ReadString(256);
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.CreateRoomCallBack(msg);
            }
            return true;
        }
        private bool HandleJoinRoomInfo(ushort maincmd, ushort subCmd, CyNetReadBuffer readbuffer)
        {
            CMD_RC_S_RoomCardData msg = new CMD_RC_S_RoomCardData();
            msg.bSucess = readbuffer.ReadBoolean();
            msg.dwUserID = readbuffer.ReadUint();
            msg.dwRoomNumKey = readbuffer.ReadUint();
            msg.wTableID = readbuffer.ReadUshort();
            msg.wChairID = readbuffer.ReadUshort();
            msg.wCurPlayCount = readbuffer.ReadUshort();
            msg.wPlayCount = readbuffer.ReadUshort();
            msg.szReasonInfo = readbuffer.ReadString(256);
            if (MessageMgr.ROOM_Interface != null)
            {
                MessageMgr.ROOM_Interface.JionRoomCallBack(msg);
            }
            return true;
        }
    }
}
