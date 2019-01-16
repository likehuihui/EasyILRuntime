using System;
using System.Collections.Generic;
using System.Text;

namespace MessagePackage.Operation
{
    public class GameOperations
    {

        #region 框架命令 100
        /// <summary>
        /// 框架命令
        /// </summary>
        public const ushort MDM_GF_FRAME = 100;

        //////////////////////////////////////////////////////////////////////////////////
        //框架命令

        //用户命令
        public const ushort SUB_GF_GAME_OPTION = 1;                     //游戏配置
        public const ushort SUB_GF_USER_READY = 2;                      //用户准备
        public const ushort SUB_GF_LOOKON_CONFIG = 3;                       //旁观配置

        //聊天命令
        public const ushort SUB_GF_USER_CHAT = 10;                      //用户聊天
        public const ushort SUB_GF_USER_EXPRESSION = 11;                        //用户表情

        //游戏信息
        public const ushort SUB_GF_GAME_STATUS = 100;                           //游戏状态
        public const ushort SUB_GF_GAME_SCENE = 101;                            //游戏场景
        public const ushort SUB_GF_LOOKON_STATUS = 102;                             //旁观状态

        //系统消息
        public const ushort SUB_GF_SYSTEM_MESSAGE = 200;                                //系统消息
        public const ushort SUB_GF_ACTION_MESSAGE = 201;                                //动作消息
        public const ushort SUB_GF_SYSTEM_MESSAGE_NEW = 203;                             //系统消息(新)

        //////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region 配置信息 2
        /// <summary>
        /// 配置信息
        /// </summary>
        public const ushort MDM_GR_CONFIG = 2;

        //////////////////////////////////////////////////////////////////////////////////
        //配置命令

        public const ushort SUB_GR_CONFIG_COLUMN = 100;                         //列表配置
        public const ushort SUB_GR_CONFIG_SERVER = 101;                         //房间配置
        public const ushort SUB_GR_CONFIG_PROPERTY = 102;                           //道具配置
        public const ushort SUB_GR_CONFIG_FINISH = 103;                           //配置完成
        public const ushort SUB_GR_CONFIG_USER_RIGHT = 104;                         //玩家权限
        #endregion

        #region 系统命令 1000
        //////////////////////////////////////////////////////////////////////////////////

        public const ushort MDM_CM_SYSTEM = 1000;                   //系统命令
        public const ushort SUB_CM_SYSTEM_MESSAGE = 1;                  //系统消息
        public const ushort SUB_CM_ACTION_MESSAGE = 2;                   //动作消息
        public const ushort SUB_CM_DOWN_LOAD_MODULE = 3;                    //下载消息

            //控制掩码
        public const ushort SMT_CLOSE_ROOM  =0x0100;								//关闭房间
        public const ushort SMT_CLOSE_GAME  =0x0200;								//关闭游戏
        public const ushort SMT_CLOSE_LINK  = 0x0400;								//中断连接
        public const ushort SMT_CLOSE_INSURE=0x0800;								//关闭银行
            //////////////////////////////////////////////////////////////////////////////////
        #endregion 


        #region 状态命令 4
        //////////////////////////////////////////////////////////////////////////////////
        //状态命令

        public const ushort MDM_GR_STATUS = 4;                      //状态信息
        public const ushort SUB_GR_TABLE_INFO = 100;                       //桌子信息
        public const ushort SUB_GR_TABLE_STATUS = 101;                      //桌子状态

        //////////////////////////////////////////////////////////////////////////////////
        #endregion


        #region 登录命令 1
        //////////////////////////////////////////////////////////////////////////////////
        //登录命令

        public const ushort MDM_GR_LOGON = 1;                   //登录信息

        //登录模式
        public const ushort SUB_GR_LOGON_USERID = 1;                        //I D 登录
        public const ushort SUB_GR_LOGON_MOBILE = 2;                        //手机登录
        public const ushort SUB_GR_LOGON_ACCOUNTS = 3;                  //帐户登录

        //登录结果
        public const ushort SUB_GR_LOGON_SUCCESS = 100;                     //登录成功
        public const ushort SUB_GR_LOGON_FAILURE = 101;                     //登录失败
        public const ushort SUB_GR_LOGON_FINISH = 102;                          //登录完成

        //升级提示
        public const ushort SUB_GR_UPDATE_NOTIFY = 200;						//升级提示

        //////////////////////////////////////////////////////////////////////////////////
        #endregion


        #region 用户命令 3
        //////////////////////////////////////////////////////////////////////////////////
        //用户命令
        public const ushort MDM_GR_USER = 3;

        //用户动作
        public const ushort SUB_GR_USER_RULE = 1;                   //用户规则
        public const ushort SUB_GR_USER_LOOKON = 2;                 //旁观请求
        public const ushort SUB_GR_USER_SITDOWN = 3;                    //坐下请求
        public const ushort SUB_GR_USER_STANDUP = 4;                    //起立请求
        public const ushort SUB_GR_USER_INVITE = 5;                 //用户邀请
        public const ushort SUB_GR_USER_INVITE_REQ = 6;                 //邀请请求
        public const ushort SUB_GR_USER_REPULSE_SIT = 7;                    //拒绝玩家坐下
        public const ushort SUB_GR_USER_KICK_USER = 8;                     //踢出用户
        public const ushort SUB_GR_USER_INFO_REQ = 9;                     //请求用户信息
        public const ushort SUB_GR_USER_CHAIR_REQ = 10;                     //请求更换位置
        public const ushort SUB_GR_USER_CHAIR_INFO_REQ = 11;                     //请求椅子用户信息
        public const ushort SUB_GR_USER_WAIT_DISTRIBUTE = 12;									//等待分配

        //用户状态
        public const ushort SUB_GR_USER_ENTER = 100;                            //用户进入
        public const ushort SUB_GR_USER_SCORE = 101;                            //用户分数
        public const ushort SUB_GR_USER_STATUS = 102;                           //用户状态
        public const ushort SUB_GR_REQUEST_FAILURE = 103;                           //请求失败

        //聊天命令
        public const ushort SUB_GR_USER_CHAT = 201;                     //聊天消息
        public const ushort SUB_GR_USER_EXPRESSION = 202;                       //表情消息
        public const ushort SUB_GR_WISPER_CHAT = 203;                       //私聊消息
        public const ushort SUB_GR_WISPER_EXPRESSION = 204;                     //私聊表情
        public const ushort SUB_GR_COLLOQUY_CHAT = 205;                       //会话消息
        public const ushort SUB_GR_COLLOQUY_EXPRESSION = 206;                       //会话表情

        //道具命令
        public const ushort SUB_GR_PROPERTY_BUY = 300;                          //购买道具
        public const ushort SUB_GR_PROPERTY_SUCCESS = 301;                          //道具成功
        public const ushort SUB_GR_PROPERTY_FAILURE = 302;                          //道具失败
        public const ushort SUB_GR_PROPERTY_MESSAGE = 303;                        //道具消息
        public const ushort SUB_GR_PROPERTY_EFFECT = 304;                        //道具效应
        public const ushort SUB_GR_PROPERTY_TRUMPET = 305;                         //喇叭消息


        //////////////////////////////////////////////////////////////////////////////////
        #endregion


        #region 比赛命令 9
        //////////////////////////////////////////////////////////////////////////////////
        //比赛命令 7

        public const ushort MDM_GR_MATCH = 9;                       //比赛命令
        public const ushort SUB_GR_MATCH_FEE = 400;                 //报名费用
        public const ushort SUB_GR_MATCH_NUM = 401;                 //等待人数
        public const ushort SUB_GR_LEAVE_MATCH = 402;               //退出比赛
        public const ushort SUB_GR_MATCH_INFO = 403;                //比赛信息
        public const ushort SUB_GR_MATCH_WAIT_TIP = 404;            //等待提示
        public const ushort SUB_GR_MATCH_RESULT = 405;              //比赛结果
        public const ushort SUB_GR_MATCH_STATUS = 406;              //比赛状态
        public const ushort SUB_GR_MATCH_USER_COUNT = 407;          //参赛人数
        public const ushort SUB_GR_MATCH_DESC = 408;                //比赛描述
        public const ushort SUB_GR_MATCH_GOLDUPDATE = 409;			//金币更新
        public const ushort SUB_GR_MATCH_ELIMINATE = 410;			//比赛淘汰
        public const ushort SUB_GR_MATCH_PROMOTION = 411;			//比赛晋级
        //////////////////////////////////////////////////////////////////////////////////
        #endregion

        //////////////////////////////////////////////////////////////////////////////////
        //游戏命令

        public const ushort MDM_GF_GAME = 200;						//游戏命令

        public const ushort MDM_GR_INSURE = 5;                      //银行命令


        public const ushort MDM_GR_ROOM_CARD = 10;                  //房卡命令
        public const ushort SUB_RC_C_ROOM_CREATE = 1;               //创建房间
        public const ushort SUB_RC_C_ROOM_JOIN = 2;					//加入房间
        public const ushort SUB_RC_S_ROOM_CREATE = 3;               //创建返回
        public const ushort SUB_RC_S_ROOM_JOIN = 4;                 //加入返回
        public const ushort SUB_RC_C_ROOM_DISSOLVE = 6;             //解散请求
        public const ushort SUB_RC_C_ROOM_AGREE = 7;                //同意请求
                                                                    //////////////////////////////////////////////////////////////////////////////////


    }
}
