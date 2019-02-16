using System;
using System.Collections.Generic;

using System.Text;

namespace MessagePackage
{
    public class ConstDefine
    {
        public const uint LEN_MACHINE_ID = 33;      // 机器码长度
        public const uint LEN_MD5 = 33;             // MD5串长度
        public const uint LEN_ACCOUNTS = 33;        // 帐号长度
        public const uint LEN_NICKNAME = 33;        // 昵称长度
        public const uint LEN_PASS_PORT_ID = 19;    // 身份证号码长度
        public const uint LEN_COMPELLATION = 16;    // 真实姓名长度
        public const uint LEN_SAFE_ANSWER = 33;     // 安全问题长度
        public const uint LEN_MOBILE_PHONE = 12;    // 手机号长度
        public const uint LEN_QQ = 16;              // qq号长度

        public const uint LEN_GROUP_NAME = 32;
        public const uint LEN_UNDER_WRITE = 32;


        public const uint LEN_BANK_QUICK_MAX = 8;   // 快捷操作数量

        #region 辅助字符串类型
        /// <summary>
        /// 用户昵称
        /// </summary>
        public const int DTP_GP_UI_NICKNAME	= 1;		//
        /// <summary>
        /// 用户说明
        /// </summary>
        public const int DTP_GP_UI_USER_NOTE = 2;		//
        /// <summary>
        /// 个性签名
        /// </summary>
        public const int DTP_GP_UI_UNDER_WRITE = 3;		//
        /// <summary>
        /// Q Q 号码
        /// </summary>
        public const int DTP_GP_UI_QQ =	4;				//
        /// <summary>
        /// 电子邮件
        /// </summary>
        public const int DTP_GP_UI_EMAIL = 5;			//
        /// <summary>
        /// 固定电话
        /// </summary>
        public const int DTP_GP_UI_PROVICE = 6;			//
        /// <summary>
        /// 移动电话
        /// </summary>
        public const int DTP_GP_UI_CITY	= 7;			//
        /// <summary>
        /// 真实名字
        /// </summary>
        public const int DTP_GP_UI_COMPELLATION	= 8;	//
        /// <summary>
        /// 联系地址
        /// </summary>
        public const int DTP_GP_UI_DWELLING_PLACE =	9;	//
        /// <summary>
        /// 用记身份证
        /// </summary>
        public const int DTP_GP_UI_IDCARD = 10;

        /// <summary>
        /// 用户会员信息
        /// </summary>
        public const int DTP_GP_MEMBER_INFO = 30;



        /// <summary>
        /// 桌子密码
        /// </summary>
        public const int DTP_GR_TABLE_PASSWORD = 1;									//桌子密码

        //用户属性
        /// <summary>
        /// 用户昵称
        /// </summary>
        public const int DTP_GR_NICK_NAME = 10;									//用户昵称
        /// <summary>
        /// 社团名字
        /// </summary>
        public const int DTP_GR_GROUP_NAME = 11;									//社团名字
        /// <summary>
        /// 个性签名
        /// </summary>
        public const int DTP_GR_UNDER_WRITE	= 12;									//个性签名

        //附加信息
        /// <summary>
        /// 用户备注
        /// </summary>
        public const int DTP_GR_USER_NOTE = 20;									//用户备注
        /// <summary>
        /// 自定头像
        /// </summary>
        public const int DTP_GR_CUSTOM_FACE = 21;									//自定头像
        #endregion

        // 消息主命令
        #region 大厅登录命令
        /// <summary>
        /// 登陆服务器主命令
        /// </summary>
        public const ushort MDM_GP_LOGON = 1;
        // 消息子命令
        /// <summary>
        /// 以ID登陆游戏
        /// </summary>
        public const ushort SUB_GP_LOGON_GAMEID = 1;
        /// <summary>
        /// 帐号登陆
        /// </summary>
        public const ushort SUB_GP_LOGON_ACCOUNTS = 2;
        /// <summary>
        /// 注册帐号
        /// </summary>
        public const ushort SUB_GP_REGISTER_ACCOUNTS = 3;
        public const ushort SUB_GP_LOGON_MANAGETOOL = 4;                                //管理工具

        /// <summary>
        /// 请求验证码
        /// </summary>
        public const ushort SUB_GP_REQUEST_VCCODE = 7;
        /// <summary>
        /// 验证码
        /// </summary>
        public const ushort SUB_GP_REQUEST_VCCODE_RESULT = 8;

        /// <summary>
        /// 快速登录
        /// </summary>
        public const ushort SUB_GP_QUICK_LOGON = 9;

        /// <summary>
        /// 快速登录注册返回缓存的账户密码
        /// </summary>
        public const ushort SUB_GP_QUICK_LOGON_BACK = 10;

        /// <summary>
        /// 请求手机验证码
        /// </summary>
        public const ushort SUB_GP_REQUEST_PHONE_VCCODE = 11;

        /// <summary>
        /// 请求手机验证码结果
        /// </summary>
        public const ushort SUB_GP_REQUEST_PHONE_VCCODE_RESULT = 12;

        /// <summary>
        /// 请求绑定手机
        /// </summary>
        public const ushort SUB_GP_BIND_PHONE = 13;

        /// <summary>
        /// 请求绑定手机结果
        /// </summary>
        public const ushort SUB_GP_BIND_PHONE_BACK = 14;

        /// <summary>
        /// 请求重置密码
        /// </summary>
        public const ushort SUB_GP_RESET_PASSWORD = 15;

        /// <summary>
        /// 请求重置密码结果
        /// </summary>
        public const ushort SUB_GP_RESET_PASSWORD_BACK = 16;							

        /// <summary>
        /// 登录成功
        /// </summary>
        public const ushort SUB_GP_LOGON_SUCCESS = 100;
        /// <summary>
        /// 登录失败
        /// </summary>
        public const ushort SUB_GP_LOGON_FAILURE = 101;
        /// <summary>
        /// 登录完成
        /// </summary>

        public const ushort SUB_GP_LOGON_FINISH = 102;
        /// <summary>
        /// 登录失败
        /// </summary>
        public const ushort SUB_GP_VALIDATE_MBCARD = 103;
        public const ushort SUB_GP_GROWLEVEL_CONFIG = 107;                  //等级配置
        //升级提示
        public const ushort SUB_GP_UPDATE_NOTIFY = 200;                         //升级提示



        #endregion

        #region 列表命令
        /// <summary>
        /// 列表命令
        /// </summary>
        public const ushort MDM_GP_SERVER_LIST = 2;

        //获取命令
        public const ushort SUB_GP_GET_LIST = 1;                            //获取列表
        public const ushort SUB_GP_GET_SERVER = 2;                          //获取房间
        public const ushort SUB_GP_GET_MATCH = 3;                           //获取在线
        public const ushort SUB_GP_GET_COLLECTION = 4;                          //获取收藏

        //列表信息
        public const ushort SUB_GP_LIST_TYPE = 100;                         //类型列表
        public const ushort SUB_GP_LIST_KIND = 101;                         //种类列表
        public const ushort SUB_GP_LIST_NODE = 102;                         //节点列表
        public const ushort SUB_GP_LIST_PAGE = 103;                         //定制列表
        public const ushort SUB_GP_LIST_SERVER = 104;                       //房间列表
        public const ushort SUB_GP_VIDEO_OPTION = 106;                      //视频配置
        public const ushort SUB_GP_LIST_MATCH = 105;                        //比赛列表   

                                                                                //完成信息
        public const ushort SUB_GP_LIST_FINISH = 200;                               //发送完成
        public const ushort SUB_GP_SERVER_FINISH = 201;                             //房间完成

        //在线信息
        public const ushort SUB_GR_KINE_ONLINE = 300;                               //类型在线
        public const ushort SUB_GR_SERVER_ONLINE = 301;								//房间在线
        #endregion


        /// <summary>
        /// 用户服务
        /// </summary>
        public const ushort MDM_GP_USER_SERVICE = 3;//子命令在 UserOperations



        ////用户汇总
        //public const ushort MDM_CS_USER_COLLECT = 3;        
        ////用户状态                                                    
        //public const ushort SUB_CS_C_USER_ENTER = 1;                            //用户进入
        //public const ushort SUB_CS_C_USER_LEAVE = 2;                           //用户离开
        //public const ushort SUB_CS_C_USER_FINISH = 3;                           //用户完成

        ////用户状态
        //public const ushort SUB_CS_S_COLLECT_REQUEST = 100;								//汇总请求
        public const ushort MDM_GP_PROP = 7;				 					//道具商城

        public const ushort MDM_GP_PLATFORM = 8;    //大厅功能

        public const ushort MDM_GP_BAG = 9;         //背包

        public const ushort MDM_GP_MARQUEE=	6;									//广播信息

    }

    //网络状态
    public enum NetStatus
    {
        SOCK_NONE,              // 未创建网络
        SOCK_INIT,              // 初始化状态
        SOCK_CONNECED,          // 已连接
        SOCK_CLOSED,            // 关闭
        SOCK_ERROR              // 网络错误
    }
}
