using CYNetwork;
using CYNetwork.NetStruct;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagePackage.Struct
{

    /// <summary>
    /// I D 登录
    /// </summary>
    public struct CMD_GR_LogonUserID
    {
        //版本信息
        /// <summary>
        /// 广场版本
        /// </summary>
        public uint dwPlazaVersion;
        /// <summary>
        /// 框架版本
        /// </summary>
        public uint dwFrameVersion;
        /// <summary>
        /// 进程版本
        /// </summary>
        public uint dwProcessVersion;

        //登录信息
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 登录密码 33
        /// </summary>
        public string szPassword;
        /// <summary>
        /// 房间密码 33
        /// </summary>
        public string szServerPasswd;
        /// <summary>
        /// 机器序列 33
        /// </summary>
        public string szMachineID;
        /// <summary>
        /// 类型索引
        /// </summary>
        public ushort wKindID;
    };


    /// <summary>
    /// 手机登录
    /// </summary>
    public struct CMD_GR_LogonMobile
    {
        //版本信息
        /// <summary>
        /// 游戏标识
        /// </summary>
        public ushort wGameID;
        /// <summary>
        /// 进程版本
        /// </summary>
        public uint dwProcessVersion;

        //桌子区域
        /// <summary>
        /// 设备类型
        /// </summary>
        public byte cbDeviceType;
        /// <summary>
        /// 行为标识
        /// </summary>
        public ushort wBehaviorFlags;
        /// <summary>
        /// 分页桌数
        /// </summary>
        public ushort wPageTableCount;

        //登录信息
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 登录密码 33
        /// </summary>		
        public string szPassword;
        /// <summary>
        /// 机器标识 33
        /// </summary>
        public string szMachineID;
    };


    //帐号登录
    public struct CMD_GR_LogonAccounts
    {
        //版本信息
        /// <summary>
        /// 广场版本
        /// </summary>
        public uint dwPlazaVersion;
        /// <summary>
        /// 框架版本
        /// </summary>
        public uint dwFrameVersion;
        /// <summary>
        /// 进程版本
        /// </summary>
        public uint dwProcessVersion;

        //登录信息
        /// <summary>
        /// 登录密码33
        /// </summary>
        public string szPassword;
        /// <summary>
        /// 登录帐号32
        /// </summary>
        public string szAccounts;
        /// <summary>
        /// 机器序列33
        /// </summary>
        public string szMachineID;
    }

    /// <summary>
    /// 用户进入
    /// </summary>
    public struct CMD_CS_C_UserEnter
    {
        //用户信息
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 游戏标识
        /// </summary>
        public uint dwGameID;
        /// <summary>
        /// 用户昵称 32
        /// </summary>	
        public string szNickName;
        //辅助信息
        /// <summary>
        /// 用户性别
        /// </summary>
        public byte cbGender;
        /// <summary>
        /// 会员等级
        /// </summary>
        public byte cbMemberOrder;
        /// <summary>
        /// 管理等级
        /// </summary>
        public byte cbMasterOrder;
    };

    /// <summary>
    /// 登录成功
    /// </summary>
    public struct CMD_GR_LogonSuccess
    {
        /// <summary>
        /// 用户权限
        /// </summary>
        public uint dwUserRight;
        /// <summary>
        /// 管理权限
        /// </summary>
        public uint dwMasterRight;
    };

    /// <summary>
    /// 登录失败
    /// </summary>
    [MessageBody]
    public struct CMD_GR_LogonFailure
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int lErrorCode;
        /// <summary>
        /// 描述消息 128
        /// </summary>
        [MessageItem(MsgItemType.STRING)]
        public string szDescribeString;
    };

    public struct CMD_GR_UpdateNotify
    {
        //升级标志
        public bool cbMustUpdatePlaza;					    //强行升级
        public bool cbMustUpdateClient;					   //强行升级
        public bool cbAdviceUpdateClient;				    //建议升级

        //当前版本
        public uint dwCurrentPlazaVersion;				//当前版本
        public uint dwCurrentFrameVersion;				//当前版本
        public uint dwCurrentClientVersion;				//当前版本
    };


    /// <summary>
    /// 房间配置
    /// </summary>
    public struct CMD_GR_ConfigServer
    {
        //房间属性
        /// <summary>
        /// 桌子数目
        /// </summary>
        public ushort wTableCount;
        /// <summary>
        /// 椅子数目
        /// </summary>
        public ushort wChairCount;

        //房间配置
        /// <summary>
        /// 房间类型
        /// </summary>
        public ushort wServerType;
        /// <summary>
        /// 房间规则
        /// </summary>
        public uint dwServerRule;
    };


    /// <summary>
    /// 列表配置
    /// </summary>
    public struct CMD_GR_ConfigColumn
    {
        /// <summary>
        /// 列表数目
        /// </summary>
        public byte cbColumnCount;
        /// <summary>
        /// 列表描述 32
        /// </summary>	
        public tagColumnItem[] ColumnItem;
    };

    /// <summary>
    /// 列表子项
    /// </summary>
    public struct tagColumnItem
    {
        /// <summary>
        /// 列表宽度
        /// </summary>
        public byte cbColumnWidth;
        /// <summary>
        /// 字段类型
        /// </summary>
        public byte cbDataDescribe;
        /// <summary>
        /// 列表名字16
        /// </summary>
        public string szColumnName;
    };

    /// <summary>
    /// 道具配置
    /// </summary>
    public struct CMD_GR_ConfigProperty
    {
        /// <summary>
        /// 道具数目
        /// </summary>
        public byte cbPropertyCount;
        /// <summary>
        /// 道具描述 128
        /// </summary>
        public tagPropertyInfo[] PropertyInfo;
    };

    /// <summary>
    /// 道具信息
    /// </summary>
    public struct tagPropertyInfo
    {
        //道具信息
        /// <summary>
        /// 道具标识
        /// </summary>
        public ushort wIndex;
        /// <summary>
        /// 会员折扣
        /// </summary>
        public ushort wDiscount;
        /// <summary>
        /// 发布范围
        /// </summary>
        public ushort wIssueArea;

        //销售价格
        /// <summary>
        /// 道具金币
        /// </summary>
        public long lPropertyGold;
        /// <summary>
        /// 道具价格
        /// </summary>
        public double dPropertyCash;

        //赠送魅力
        /// <summary>
        /// 赠送魅力
        /// </summary>
        public long lSendLoveLiness;
        /// <summary>
        /// 接受魅力
        /// </summary>
        public long lRecvLoveLiness;
    };


    /// <summary>
    /// 用户信息
    /// </summary>
    public class tagUserInfoHead
    {
        //用户属性
        /// <summary>
        /// 游戏 I D
        /// </summary>
        public uint dwGameID;
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 社团 I D
        /// </summary>
        public uint dwGroupID;

        //头像信息
        /// <summary>
        /// 头像索引
        /// </summary>
        public uint wFaceID;
        /// <summary>
        /// 自定标识
        /// </summary>
        public uint dwCustomID;
        /// <summary>
        /// 客户端类型
        /// </summary>
        public byte cbClientType;

        /// <summary>
        /// AI标示
        /// </summary>
        public bool bIsAndroid;
        //用户属性
        /// <summary>
        /// 用户性别
        /// </summary>
        public byte cbGender;
        /// <summary>
        /// 会员等级
        /// </summary>
        public byte cbMemberOrder;
        /// <summary>
        /// 管理等级
        /// </summary>
        public byte cbMasterOrder;

        //用户状态
        /// <summary>
        /// 桌子索引
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 椅子索引
        /// </summary>
        public ushort wChairID;
        /// <summary>
        /// 用户状态
        /// </summary>
        public byte cbUserStatus;

        //积分信息
        /// <summary>
        /// 用户分数
        /// </summary>
        public long lScore;
        /// <summary>
        /// 用户成绩
        /// </summary>
        public long lGrade;
        /// <summary>
        /// 用户银行
        /// </summary>
        public long lInsure;
        /// <summary>
        /// 用户元宝
        /// </summary>
        public long lIngot;

        //游戏信息
        /// <summary>
        /// 胜利盘数
        /// </summary>
        public uint dwWinCount;
        /// <summary>
        /// 失败盘数
        /// </summary>
        public uint dwLostCount;
        /// <summary>
        /// 和局盘数
        /// </summary>
        public uint dwDrawCount;
        /// <summary>
        /// 逃跑盘数
        /// </summary>
        public uint dwFleeCount;
        /// <summary>
        /// 用户经验
        /// </summary>
        public uint dwExperience;
        /// <summary>
        /// 用户魅力
        /// </summary>
        public int lLoveLiness;

        /// <summary>
        /// 附加字段
        /// </summary>
        public Dictionary<int, object> Infos;
        //10									//用户昵称
        //11									//社团名字
        //12									//个性签名

    };
    /// <summary>
    /// 新消息
    /// </summary>
    public struct CMD_CM_SystemMessageNew
    {
        public byte cbType;
        public uint wUserID;                  //玩家userID
        public ushort wIsRobot;                  //是否是机器人 0不是  1是
        public string tNickName;  //玩家昵称64
        public string tServerName;    //房间名称64
        public uint uMultNum;                  //赢奖倍数
        public string tPrizeName; //奖项名称64
        public long lScore;                        //奖项赢分
        public string szMarqueeMsg;//基础字符串256
        public CMD_CM_SystemMessageNew(CyNetReadBuffer readbuffer)
        {
            cbType = readbuffer.ReadByte();
            wUserID = readbuffer.ReadUint();
            wIsRobot = readbuffer.ReadUshort();
            tNickName = readbuffer.ReadString(64);
            tNickName = StringUtil.TrimUnusefulChar(tNickName);
            tServerName = readbuffer.ReadString(64);
            tServerName = StringUtil.TrimUnusefulChar(tServerName);
            uMultNum = readbuffer.ReadUint();
            tPrizeName = readbuffer.ReadString(64);
            tPrizeName = StringUtil.TrimUnusefulChar(tPrizeName);
            lScore = readbuffer.ReadLong();
            szMarqueeMsg = readbuffer.ReadString(256);
            szMarqueeMsg = StringUtil.TrimUnusefulChar(szMarqueeMsg);
        }
    };
    /// <summary>
    /// 系统消息
    /// </summary>
    [MessageBody]
    public struct CMD_CM_SystemMessage
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public ushort wType;
        /// <summary>
        /// 消息长度
        /// </summary>
        public ushort wLength;
        /// <summary>
        /// 消息内容1024
        /// </summary>
        [MessageItem(MsgItemType.STRING)]
        public string szString;
    };

    /// <summary>
    /// 用户规则
    /// </summary>
    public struct CMD_GR_UserRule
    {
        /// <summary>
        /// 规则掩码
        /// </summary>
        public byte cbRuleMask;
        /// <summary>
        /// 最低胜率
        /// </summary>
        public ushort wMinWinRate;
        /// <summary>
        /// 最高逃率
        /// </summary>
        public ushort wMaxFleeRate;
        /// <summary>
        /// 最高分数
        /// </summary>
        public int lMaxGameScore;
        /// <summary>
        /// 最低分数
        /// </summary>
        public int lMinGameScore;
    };


    /// <summary>
    /// 旁观请求
    /// </summary>
    public struct CMD_GR_UserLookon
    {
        /// <summary>
        /// 桌子位置
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 椅子位置
        /// </summary>
        public ushort wChairID;
    };

    /// <summary>
    /// 坐下请求
    /// </summary>
    public struct CMD_GR_UserSitDown
    {
        /// <summary>
        /// 桌子位置
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 椅子位置
        /// </summary>
        public ushort wChairID;
        /// <summary>
        /// 桌子密码33
        /// </summary>			
        public string szPassword;
    };

    /// <summary>
    /// 起立请求
    /// </summary>
    public struct CMD_GR_UserStandUp
    {
        /// <summary>
        /// 桌子位置
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 椅子位置
        /// </summary>
        public ushort wChairID;
        /// <summary>
        /// 强行离开
        /// </summary>
        public byte cbForceLeave;
    };

    /// <summary>
    /// 邀请用户
    /// </summary> 
    public struct CMD_GR_UserInvite
    {
        /// <summary>
        /// 桌子号码
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
    };

    /// <summary>
    /// 用户积分
    /// </summary>
    public struct tagUserScore
    {
        //积分信息
        /// <summary>
        /// 用户分数
        /// </summary>
        public long lScore;
        /// <summary>
        /// 用户成绩
        /// </summary>
        public long lGrade;
        /// <summary>
        /// 用户银行
        /// </summary>
        public long lInsure;

        //输赢信息
        /// <summary>
        /// 胜利盘数
        /// </summary>
        public uint dwWinCount;
        /// <summary>
        /// 失败盘数
        /// </summary>
        public uint dwLostCount;
        /// <summary>
        /// 和局盘数
        /// </summary>
        public uint dwDrawCount;
        /// <summary>
        /// 逃跑盘数
        /// </summary>
        public uint dwFleeCount;

        //全局信息
        /// <summary>
        /// 用户奖牌
        /// </summary>
        public uint dwUserMedal;
        /// <summary>
        /// 用户经验
        /// </summary>
        public uint dwExperience;
        /// <summary>
        /// 用户魅力
        /// </summary>
        public int lLoveLiness;
    };

    /// <summary>
    /// 用户聊天 发送方
    /// </summary>
    public struct CMD_GR_C_UserChat
    {
        /// <summary>
        /// 信息长度
        /// </summary>
        public ushort wChatLength;
        /// <summary>
        /// 信息颜色
        /// </summary>
        public uint dwChatColor;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
        /// <summary>
        /// 聊天信息128
        /// </summary>		
        public string szChatString;
    };

    /// <summary>
    /// 用户聊天 接收方
    /// </summary>
    public struct CMD_GR_S_UserChat
    {
        /// <summary>
        /// 信息长度
        /// </summary>
        public ushort wChatLength;
        /// <summary>
        /// 信息颜色
        /// </summary>
        public uint dwChatColor;
        /// <summary>
        /// 发送用户
        /// </summary>
        public uint dwSendUserID;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
        /// <summary>
        /// 聊天信息128
        /// </summary>		
        public string szChatString;
    };

    /// <summary>
    /// 用户表情
    /// </summary>
    public struct CMD_GR_C_UserExpression
    {
        /// <summary>
        /// 表情索引
        /// </summary>
        public ushort wItemIndex;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
    };

    /// <summary>
    /// 用户表情
    /// </summary>
    public struct CMD_GR_S_UserExpression
    {
        /// <summary>
        /// 表情索引
        /// </summary>
        public ushort wItemIndex;
        /// <summary>
        /// 发送用户
        /// </summary>
        public uint dwSendUserID;
        /// <summary>
        //目标用户 
        /// </summary>
        public uint dwTargetUserID;
    }

    /// <summary>
    /// 用户私聊 发送
    /// </summary>
    public struct CMD_GR_C_WisperChat
    {
        /// <summary>
        /// 信息长度
        /// </summary>
        public ushort wChatLength;
        /// <summary>
        /// 信息颜色
        /// </summary>
        public uint dwChatColor;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
        /// <summary>
        /// 聊天信息128
        /// </summary>

        public string szChatString;
    };

    /// <summary>
    /// 用户私聊 响应
    /// </summary>
    public struct CMD_GR_S_WisperChat
    {
        /// <summary>
        /// 信息长度
        /// </summary>
        public ushort wChatLength;
        /// <summary>
        /// 信息颜色
        /// </summary>
        public uint dwChatColor;
        /// <summary>
        /// 发送用户
        /// </summary>
        public uint dwSendUserID;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
        /// <summary>
        /// 聊天信息128
        /// </summary>	
        public string szChatString;
    };

    /// <summary>
    /// 私聊表情
    /// </summary>
    public struct CMD_GR_C_WisperExpression
    {
        /// <summary>
        /// 表情索引
        /// </summary>
        public ushort wItemIndex;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
    };

    //私聊表情
    public struct CMD_GR_S_WisperExpression
    {
        /// <summary>
        /// 表情索引
        /// </summary>
        public ushort wItemIndex;
        /// <summary>
        /// 发送用户
        /// </summary>
        public uint dwSendUserID;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
    };

    /// <summary>
    /// 用户会话
    /// </summary>
    public struct CMD_GR_ColloquyChat
    {
        /// <summary>
        /// 信息长度
        /// </summary>
        public ushort wChatLength;
        /// <summary>
        /// 信息颜色
        /// </summary>
        public uint dwChatColor;
        /// <summary>
        /// 发送用户
        /// </summary>
        public uint dwSendUserID;
        /// <summary>
        /// 会话标识
        /// </summary>
        public uint dwConversationID;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint[] dwTargetUserID;
        /// <summary>
        /// 聊天信息
        /// </summary>		
        public string szChatString;
    };

    /// <summary>
    /// 邀请用户
    /// </summary>
    public struct CMD_GR_C_InviteUser
    {
        /// <summary>
        /// 桌子号码
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 发送用户
        /// </summary>
        public uint dwSendUserID;
    };

    /// <summary>
    /// 邀请用户
    /// </summary>
    public struct CMD_GR_S_InviteUser
    {
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
    };

    /// <summary>
    /// 购买道具
    /// </summary>
    public struct CMD_GR_C_PropertyBuy
    {
        /// <summary>
        /// 请求范围
        /// </summary>
        public byte cbRequestArea;
        /// <summary>
        /// 积分消费
        /// </summary>
        public byte cbConsumeScore;
        /// <summary>
        /// 购买数目
        /// </summary>
        public ushort wItemCount;
        /// <summary>
        /// 道具索引
        /// </summary>
        public ushort wPropertyIndex;
        /// <summary>
        /// 使用对象
        /// </summary>
        public uint dwTargetUserID;
    };

    /// <summary>
    /// 道具成功
    /// </summary>
    public struct CMD_GR_S_PropertySuccess
    {
        /// <summary>
        /// 使用环境
        /// </summary>
        public byte cbRequestArea;
        /// <summary>
        /// 购买数目
        /// </summary>
        public ushort wItemCount;
        /// <summary>
        /// 道具索引
        /// </summary>
        public ushort wPropertyIndex;
        /// <summary>
        /// 目标对象
        /// </summary>
        public uint dwSourceUserID;
        /// <summary>
        /// 使用对象
        /// </summary>
        public uint dwTargetUserID;
    };

    /// <summary>
    /// 道具失败
    /// </summary>
    public struct CMD_GR_PropertyFailure
    {
        /// <summary>
        /// 请求区域
        /// </summary>
        public ushort wRequestArea;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int lErrorCode;
        /// <summary>
        /// 描述信息256
        /// </summary>
        public string szDescribeString;
    };

    /// <summary>
    /// 道具消息
    /// </summary>
    public struct CMD_GR_S_PropertyMessage
    {
        //道具信息
        /// <summary>
        /// 道具索引
        /// </summary>
        public ushort wPropertyIndex;
        /// <summary>
        /// 道具数目
        /// </summary>
        public ushort wPropertyCount;
        /// <summary>
        /// 目标对象
        /// </summary>
        public uint dwSourceUserID;
        /// <summary>
        /// 使用对象
        /// </summary>
        public uint dwTargerUserID;
    };


    /// <summary>
    /// 道具效应
    /// </summary>
    public struct CMD_GR_S_PropertyEffect
    {
        /// <summary>
        /// 用 户I D
        /// </summary>
        public uint wUserID;
        /// <summary>
        /// 会员等级
        /// </summary>
        public byte cbMemberOrder;
    };

    /// <summary>
    /// 发送喇叭
    /// </summary>
    public struct CMD_GR_C_SendTrumpet
    {
        /// <summary>
        /// 请求范围
        /// </summary>
        public byte cbRequestArea;
        /// <summary>
        /// 道具索引
        /// </summary>
        public ushort wPropertyIndex;
        /// <summary>
        /// 喇叭颜色
        /// </summary>
        public uint TrumpetColor;
        /// <summary>
        /// 喇叭内容128
        /// </summary>        
        public string szTrumpetContent;
    };

    /// <summary>
    /// 发送喇叭
    /// </summary>
    public struct CMD_GR_S_SendTrumpet
    {
        /// <summary>
        /// 道具索引
        /// </summary>
        public ushort wPropertyIndex;
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwSendUserID;
        /// <summary>
        /// 喇叭颜色
        /// </summary>
        public uint TrumpetColor;
        /// <summary>
        /// 玩家昵称 32
        /// </summary>
        public string szSendNickName;
        /// <summary>
        /// 喇叭内容128
        /// </summary>	    
        public string szTrumpetContent;
    };


    /// <summary>
    /// 用户拒绝黑名单坐下
    /// </summary>
    public struct CMD_GR_UserRepulseSit
    {
        /// <summary>
        /// 桌子号码
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 椅子位置
        /// </summary>
        public ushort wChairID;
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwRepulseUserID;
    };

    //邀请用户请求 
    public struct CMD_GR_UserInviteReq
    {
        /// <summary>
        /// 桌子号码
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
    };

    //用户分数
    public struct CMD_GR_UserScore
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 积分信息
        /// </summary>
        public tagUserScore UserScore;
    };

    /// <summary>
    /// 用户分数
    /// </summary>
    public struct CMD_GR_MobileUserScore
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 积分信息
        /// </summary>
        public tagMobileUserScore UserScore;
    };

    /// <summary>
    /// 用户状态
    /// </summary>
    public struct CMD_GR_UserStatus
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;

        /// <summary>
        /// 桌子索引
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 椅子位置
        /// </summary>
        public ushort wChairID;
        /// <summary>
        /// 用户状态
        /// </summary>
        public byte cbUserStatus;
    };

    /// <summary>
    /// 请求失败
    /// </summary>
    public struct CMD_GR_RequestFailure
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int lErrorCode;
        /// <summary>
        /// 描述信息256
        /// </summary>
        public string szDescribeString;
    };

    /// <summary>
    /// 用户积分
    /// </summary>
    public struct tagMobileUserScore
    {
        //积分信息
        /// <summary>
        /// 用户分数
        /// </summary>
        public long lScore;

        //输赢信息
        /// <summary>
        /// 胜利盘数
        /// </summary>
        public uint dwWinCount;
        /// <summary>
        /// 失败盘数
        /// </summary>
        public uint dwLostCount;
        /// <summary>
        /// 和局盘数
        /// </summary>
        public uint dwDrawCount;
        /// <summary>
        /// 逃跑盘数
        /// </summary>
        public uint dwFleeCount;

        //全局信息
        /// <summary>
        /// 用户经验
        /// </summary>
        public uint dwExperience;
    };


    /// <summary>
    /// 用户状态
    /// </summary>
    public struct tagUserStatus
    {
        /// <summary>
        /// 桌子索引
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 椅子位置
        /// </summary>
        public ushort wChairID;
        /// <summary>
        /// 用户状态
        /// </summary>
        public byte cbUserStatus;
    };

    /// <summary>
    /// 踢出用户
    /// </summary>
    public struct CMD_GR_KickUser
    {
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
    };

    /// <summary>
    /// 请求用户信息
    /// </summary>
    public struct CMD_GR_UserInfoReq
    {
        /// <summary>
        /// 请求用户
        /// </summary>
        public uint dwUserIDReq;
        /// <summary>
        /// 桌子位置
        /// </summary>
        public ushort wTablePos;
    };

    /// <summary>
    /// 请求用户信息
    /// </summary>
    public struct CMD_GR_ChairUserInfoReq
    {
        /// <summary>
        /// 桌子号码
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 椅子位置
        /// </summary>
        public ushort wChairID;
    };

    /// <summary>
    /// 游戏配置
    /// </summary>
    public struct CMD_GF_GameOption
    {
        /// <summary>
        /// 旁观标志
        /// </summary>
        public byte cbAllowLookon;
        /// <summary>
        /// 框架版本
        /// </summary>
        public uint dwFrameVersion;
        /// <summary>
        /// 游戏版本
        /// </summary>
        public uint dwClientVersion;
    };

    /// <summary>
    /// 游戏环境
    /// </summary>
    public struct CMD_GF_GameStatus
    {
        /// <summary>
        /// 游戏状态
        /// </summary>
        public byte cbGameStatus;
        /// <summary>
        /// 旁观标志
        /// </summary>
        public byte cbAllowLookon;
    };

    /// <summary>
    /// 桌子规则－底注
    /// </summary>
    public struct CMD_GF_TableGold
    {
        /// <summary>
        /// 底注标志
        /// </summary>
        public bool bTableGold;
        /// <summary>
        /// 桌子底注
        /// </summary>
        public long lTableGold;
    };


    /// <summary>
    /// 桌子状态
    /// </summary>
    public struct CMD_GR_TableStatus
    {
        /// <summary>
        /// 桌子号码
        /// </summary>
        public ushort wTableID;
        /// <summary>
        /// 桌子状态
        /// </summary>
        public tagTableStatus TableStatus;
    };


    /// <summary>
    /// 桌子状态
    /// </summary>
    public struct tagTableStatus
    {
        /// <summary>
        /// 锁定标志
        /// </summary>
        public byte cbTableLock;
        /// <summary>
        /// 游戏标志
        /// </summary>
        public byte cbPlayStatus;

        public uint lCellScore;
    };

    /// <summary>
    /// 用户聊天
    /// </summary>
    public struct CMD_GF_C_UserChat
    {
        /// <summary>
        /// 信息长度
        /// </summary>
        public ushort wChatLength;
        /// <summary>
        /// 信息颜色
        /// </summary>
        public uint dwChatColor;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
        /// <summary>
        /// 聊天信息128
        /// </summary>
        public string szChatString;
    };


    /// <summary>
    /// 用户表情
    /// </summary>
    public struct CMD_GF_C_UserExpression
    {
        /// <summary>
        /// 表情索引
        /// </summary>
        public ushort wItemIndex;
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetUserID;
    };

    /// <summary>
    /// 旁观配置
    /// </summary>
    public struct CMD_GF_LookonConfig
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 允许旁观
        /// </summary>
        public byte cbAllowLookon;
    };

    public struct CMD_GR_TableInfo
    {
        public ushort wTableCount;                  //桌子数目
        public tagTableStatus[] TableStatusArray;				//桌子状态
    };

    //比赛报名
    public struct CMD_GP_MatchSignup
    {
        //比赛信息
        public ushort wServerID;							//房间标识
        public uint dwMatchID;							//比赛标识
        public uint dwMatchNO;                          //比赛场次

        //用户信息
        public uint dwUserID;							//用户标识
        public string szPassword;                           //登录密码66

        //机器信息
        public string szMachineID;		                //机器序列66
    };

    //取消报名
    public struct CMD_GP_MatchUnSignup
    {
        //比赛信息
        public ushort wServerID;							//房间标识
        public uint dwMatchID;							//比赛标识
        public uint dwMatchNO;                          //比赛场次

        //用户信息
        public uint dwUserID;							//用户标识
        public string szPassword;                           //登录密码

        //机器信息
        public string szMachineID;		                //机器序列
    };

    //报名结果
    public struct CMD_GP_MatchSignupResult
    {
        //比赛信息
        public bool bSignup;							//报名标识
        public bool bSuccessed;							//成功标识
        public string szDescribeString;					//描述信息256
    };

    public struct CMD_GR_Match_Num
    {
        public uint dwWaitting;							//等待人数
        public uint dwTotal;							//开赛人数
    };
    public struct CMD_GR_Match_Fee
    {
        public long lMatchFee;							//报名费用
        public string szNotifyContent;				    //提示内容256
    };
    public struct CMD_GR_MatchGoldUpdate
    {
        public long lCurrGold;							//当前金币
        public long lCurrIngot;						    //当前元宝
        public uint dwCurrExprience;					//当前经验
    };

    public struct CMD_GR_Match_Info
    {
        public string[] szTitle;					    //信息标题128
        public ushort wGameCount;                       //游戏局数
        public ushort wKindID;                          //游戏ID
        public byte cbMatchType;                        //比赛类型（即时,定时,循环)
        public byte cbMatchScope;						//比赛规模
    };

    public struct CMD_GR_MatchPromotion
    {
        public uint dwMatchID;                          //比赛标识
        public uint dwMatchNO;                          //比赛场次
        public ushort wRankID;                          //比赛名次	

        public ushort wCurGameCount;                    //当前局数
        public ushort wGameCount;                       //总共局数
        public string szMatchName;			            //比赛名称64
    };

    //比赛结果
    public struct CMD_GR_MatchResult
    {
        //第一个为自己信息
        public long[] lGold;                            //金币奖励 6
        public uint[] dwIngot;                          //元宝奖励 6
        public uint[] dwExperience;                     //经验奖励 6
        public string[] szDescribe;                     //得奖描述 6 512
        public long[] lMatchScore;                      //比赛分数 6
        public ushort[] wRankID;                        //比赛名次 6
        public string[] szPropName;                     //奖励的道具 6 64
        public string szMatchName;					    //比赛名称 64
    };

    //比赛等待提示
    public struct CMD_GR_Match_Wait_Tip
    {
        public long lScore;                             //当前积分
        public ushort wRank;                            //当前名次
        public ushort wCurTableRank;                    //本桌名次
        public ushort wUserCount;                       //当前人数
        public ushort wCurGameCount;                    //当前局数
        public ushort wGameCount;                       //总共局数
        public ushort wPlayingTable;                    //游戏桌数
        public string szMatchName;			             //比赛名称64  
    };


    //等级配置
    public struct tagGrowLevelConfig
    {
        public ushort wLevelID;							//等级 I D
        public uint dwExperience;						//相应经验
    };

    //等级参数
    struct tagGrowLevelParameter
    {
        public ushort wCurrLevelID;						//当前等级
        public uint dwExperience;						//当前经验
        public uint dwUpgradeExperience;				//下级经验
        public long lUpgradeRewardGold;					//升级奖励
        public long lUpgradeRewardIngot;				//升级奖励
    };
    //房卡数据
    public struct CMD_RC_S_RoomCardData
    {
        public bool bSucess;							   //成功标识
        public uint dwUserID;							   //玩家
        public uint dwRoomNumKey;						   //房号
        public ushort wTableID;							   //桌号
        public ushort wChairID;							   //椅子
        public ushort wCurPlayCount;						   //当前局数
        public ushort wPlayCount;							   //局数
        public string szReasonInfo;		                   //创建提示128
    };
    public struct tagRoomCardData
    {
        public uint dwUserID;							   //房主
        public uint dwRoomNumKey;						   //房号
        public ushort wTableID;							   //桌号
        public ushort wCurPlayCount;						   //当前局数
        public ushort wPlayCount;						   //局数
        public byte[] cbRule;				               //创建规则3
    };
    public struct CMD_RC_C_Create
    {
        public uint dwUserID;							//玩家
        public ushort wKindID;							//游戏类型
        public ushort wServerID;							//游戏服务器
        public byte[] cbRule;				                //创建规则3
    };

    public struct CMD_RC_C_Join
    {
        public uint dwUserID;							//玩家
        public ushort wKindID;							//游戏类型
        public ushort wServerID;							//游戏服务器
        public uint dwRoomNumKey;						//房号
    };
    public struct CMD_RC_C_DISSOLVE
    {
        public uint dwUserID;							//玩家
        public uint dwRoomNumKey;						//房号
        public byte cbAgree;							//同意解散（-1）不同意（1）同意 （0）等待
        public byte cbResult;							//解散结果（-1）失败（1）成功 （0）等待
    };
    public struct CMD_GP_MarqueeMsgNew
    {
        public byte cbType;
        public ushort wCycleNum;                 //循环次数
        public uint wUserID;                  //玩家userID
        public ushort wIsRobot;                  //是否是机器人 0不是  1是
        public string tNickName;  //玩家昵称 64
        public string tServerName;    //房间名称 64
        public uint uMultNum;                  //赢奖倍数
        public string tPrizeName; //奖项名称 64
        public long lScore;                        //奖项赢分
        public string szMarqueeMsg;//基础字符串 256

        public CMD_GP_MarqueeMsgNew(CyNetReadBuffer rb)
        {
            cbType = rb.ReadByte();
            wCycleNum = rb.ReadUshort();
            wUserID = rb.ReadUint();
            wIsRobot = rb.ReadUshort();
            tNickName = rb.ReadString(64);
            tNickName = StringUtil.TrimUnusefulChar(tNickName);
            tServerName = rb.ReadString(64);
            tServerName = StringUtil.TrimUnusefulChar(tServerName);
            uMultNum = rb.ReadUint();
            tPrizeName = rb.ReadString(64);
            tPrizeName = StringUtil.TrimUnusefulChar(tPrizeName);
            lScore = rb.ReadLong();
            szMarqueeMsg = rb.ReadString(256);
            szMarqueeMsg = StringUtil.TrimUnusefulChar(szMarqueeMsg);
        }
    };
}
