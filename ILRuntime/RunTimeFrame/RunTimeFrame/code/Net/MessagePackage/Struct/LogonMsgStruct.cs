

using CYNetwork;
using CYNetwork.NetStruct;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;



namespace MessagePackage.Struct
{
    // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    // [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    // 结构体长度84
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [MessageBody()]
    public struct CMD_GP_LogonAccounts
    {
        /// <summary>
        /// 广场版本
        /// </summary>
        public uint dwPlazaVersion { get; set; }

        /// <summary>
        /// 机器序列 33
        /// </summary>
        public string szMachineID { get; set; }
        /// <summary>
        /// 校验标识
        /// </summary>
        public byte cbValidateFlags { get; set; }

        /// <summary>
        /// 登录密码 33
        /// </summary>
        public string szPassword { get; set; }
        /// <summary>
        /// 登录帐号 33
        /// </summary>
        public string szAccounts { get; set; }

        //身份证
        public string szPassPortID { get; set; }

    }

    /// <summary>
    /// 快速登陆
    /// </summary>
    public struct CMD_GP_QuickLogon
    {
        //系统信息
        public uint dwPlazaVersion; //广场版本
        //类别标识
        public int iLoginType; //0快速登录  1微信登录
        //快速登录
        public string cLocalSign; //本地缓存快速登录标记 size:64
        public string cQuickSign; //机器码快速登录标记 size:64
        //微信登录
        public string cWinXinSign; //微信登录标记 size:64
        //推广码
        public string cStrSpreader;
    }

    /// <summary>
    /// 快速注册返回缓存数据
    /// </summary>
    public struct CMD_GP_QuickLogonBack
    {
        /// <summary>
        /// 本地缓存快速登陆标记
        /// </summary>
        public string cLocalSign;

        /// <summary>
        /// 本地缓存密码
        /// </summary>
        public string cLocalPassword;
    }

    /// <summary>
    /// 登陆完成
    /// </summary>
    public struct CMD_GP_LogonFinish
    {
        /// <summary>
        /// 中断时间
        /// </summary>
        public ushort wIntermitTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        public ushort wOnLineCountTime;

        public uint uTimeMarqueeData;
        public bool bIsCountDataTimer;
        public string tCheckSign; //校验MD5 66
    };

    /// <summary>
    /// 注册成功
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [MessageBody()]
    //注册帐号
    public struct CMD_GP_RegisterAccounts
    {
        //系统信息
        public uint dwPlazaVersion;                       //广场版本
        public string szMachineID;      //机器序列

        //密码变量
        public string szLogonPass;             //登录密码

        //注册信息
        public short wFaceID;                           //头像标识
        public byte cbGender;                          //用户性别
        public string szAccounts;            //登录帐号
        public string szNickName;            //用户昵称
        public string szSpreader;           //推荐帐号
        public string szPassPortID;          //证件号码
        public string szCompellation;       //真实名字
        public byte cbValidateFlags;                   //校验标识

        public string flagCodeKey;       //通讯标志位
        public string vCodeMsg;       //验证码

    };
    [MessageBody()]
    //手机注册帐号
    public struct CMD_GP_RegisterAccountsPhone
    {
        //系统信息
        public uint dwPlazaVersion;                       //广场版本
        public string szMachineID;      //机器序列

        //密码变量
        public string szLogonPass;             //登录密码

        //注册信息
        public ushort wFaceID;                           //头像标识
        public byte cbGender;                          //用户性别
        public string szAccounts;            //登录帐号
        public string szNickName;            //用户昵称
        public string szSpreader;           //推荐帐号
        public string szPassPortID;          //证件号码
        public string szCompellation;       //真实名字
        public byte cbValidateFlags;                   //校验标识      
        public int vCodeMsg;       //验证码
        public string szMobilePhone;   //手机号码(24)
        public string szExtraShuntMechineID;//线上分流平台机器码40
    };
    /// <summary>
    /// 请求验证码
    /// </summary>
    [MessageBody()]
    public struct CMD_GP_RequestVerificationCode
    {
        public int requestType;                        //0：请求验证码		1：刷新验证码
        public string flagCodeKey;   //通讯标志位
    };

    [MessageBody()]
    public struct CMD_GP_RequestVerificationCodeResult
    {
        [MessageItem(MsgItemType.STRING, Size = 96)]
        public string flagCodeKey;   //通讯标志位
        public int codeIndex;                          //验证码index
    };


    /// <summary>
    /// 登录成功
    /// </summary>
    [MessageBody, Serializable]
    public class CMD_GP_LogonSuccess
    {
        /// <summary>
        /// 头像标识
        /// </summary>
        public uint wFaceID;
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 游戏标识
        /// </summary>
        public uint dwGameID;
        /// <summary>
        /// 社团标识
        /// </summary>
        public uint dwGroupID;
        /// <summary>
        /// 自定索引
        /// </summary>
        public uint dwCustomID;

        /// <summary>
        /// 经验数值
        /// </summary>
        public uint dwExperience;
        /// <summary>
        /// 用户魅力
        /// </summary>
        public uint dwLoveLiness;
        /// <summary>
        /// 背包格子
        /// </summary>
        public ushort cbBagGridCount;
        /// <summary>
        /// 背包道具数
        /// </summary>
        public ushort cbBagPropCount;
        /// <summary>
        /// 用户金币
        /// </summary>
        public long lUserScore;
        /// <summary>
        /// 用户银行
        /// </summary>		
        public long lUserInsure;
        /// <summary>
        /// 用户元宝
        /// </summary>
        public long lUserIngot;
        /// <summary>
        /// 用户RMB
        /// </summary>
        public double dUserBeans;

        /// <summary>
        /// 用户性别
        /// </summary>
        public byte cbGender;
        /// <summary>
        /// 锁定机器
        /// </summary>
        public byte cbMoorMachine;
        /// <summary>
        /// 登录帐号
        /// </summary>	
        [MessageItem(MsgItemType.STRING, Size = 64)]
        public string szAccounts;
        /// <summary>
        /// 用户昵称
        /// </summary>
        [MessageItem(MsgItemType.STRING, Size = 64)]
        public string szNickName;
        /// <summary>
        /// 动态密码
        /// </summary>
        [MessageItem(MsgItemType.STRING, Size = 66)]
        public string szDynamicPass;
        /// <summary>
        /// 社团名字
        /// </summary>
        [MessageItem(MsgItemType.STRING, Size = 64)]
        public string szGroupName;

        /// <summary>
        /// 银行使能标识
        /// </summary>
        public byte cbInsureEnabled;

        /// <summary>
        /// 显示服务器状态(配置信息)
        /// </summary>
        public byte cbShowServerStatus;

        public uint dGameLogonTimes;

        /// <summary>
        /// 绑定的手机号
        /// </summary>
        [MessageItem(MsgItemType.STRING, Size = 24)]
        public string szphoneNumber;    //12	

        /// <summary>
        /// 手机绑定的标识（0 表示未绑定，1表示绑定)
        /// </summary>

        public byte cbBind;

        /// <summary>
        /// 会员等级
        /// </summary>
        public byte cbMemberOrder;
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime MemberOverDate;

        /// <summary>
        /// 平台功能开关
        /// </summary>
        public uint platformFucSigh;
        /// <summary>
        /// 是否是正式帐号 0不是 1是
        /// </summary>
        public ushort wIsFormalAccount;
        /// <summary>
        /// 是否已经领取首充礼包 0不是 1是
        /// </summary>
        public ushort wIsFirstRecharge;
        //         [MessageItem(MsgItemType.DICTIONARY, MaxItemCount = 2)]
        //         public Dictionary<int, object> attachInfo;

        /// <summary>
        /// 微信每日分享
        /// 今日是否已经分享过 0未分享 1已分享
        /// </summary>
        public ushort wIsWeiXinSharedToday;

        public CMD_GP_LogonSuccess(CyNetReadBuffer rb)
        {
            wFaceID = rb.ReadUint();
            dwUserID = rb.ReadUint();
            dwGameID = rb.ReadUint();
            dwGroupID = rb.ReadUint();
            dwCustomID = rb.ReadUint();
            dwExperience = rb.ReadUint();
            dwLoveLiness = rb.ReadUint();
            cbBagGridCount = rb.ReadUshort();
            cbBagPropCount = rb.ReadUshort();
            lUserScore = rb.ReadLong();
            lUserInsure = rb.ReadLong();
            lUserIngot = rb.ReadLong();
            dUserBeans = rb.ReadDouble();
            cbGender = rb.ReadByte();
            cbMoorMachine = rb.ReadByte();
            szAccounts = StringUtil.TrimUnusefulChar(rb.ReadString(64));
            szNickName = StringUtil.TrimUnusefulChar(rb.ReadString(64));
            szDynamicPass = StringUtil.TrimUnusefulChar(rb.ReadString(66));
            szGroupName = StringUtil.TrimUnusefulChar(rb.ReadString(64));
            cbInsureEnabled = rb.ReadByte();
            cbShowServerStatus = rb.ReadByte();
            dGameLogonTimes = rb.ReadUint();
            szphoneNumber = StringUtil.TrimUnusefulChar(rb.ReadString(24));
            cbBind = rb.ReadByte();
            cbMemberOrder = rb.ReadByte();
            MemberOverDate = rb.ReadDateTime();
            platformFucSigh = rb.ReadUint();
            wIsFormalAccount = rb.ReadUshort();
            //wIsFirstRecharge = rb.ReadUshort();


           // if (DC.channel == EChannel.DA_HENG_xianwan|| DC.channel == EChannel.DA_HENG_BengBeng)
          //  {
                wIsFirstRecharge = rb.ReadUshort();
                wIsWeiXinSharedToday = rb.ReadUshort();
           /// }
        }
    };


    public struct tagDTP_GP_MemberInfo
    {
        public byte cbMemberOrder;						//会员等级
        public DateTime MemberOverDate;						//到期时间
    };

    /// <summary>
    /// 登陆失败
    /// </summary>
    public struct CMD_GP_LogonFailure
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int lResultCode;
        /// <summary>
        /// 描述消息
        /// </summary>
        [MessageItem(ItemType = MsgItemType.STRING)]
        public string szDescribeString;

    }

    //请求手机验证码
    public struct CMD_GP_RequestPhoneVerificationCode
    {
        public string phone;          //手机 24
        public ushort checkSign;                         //是否检查 0不检查 1检查

        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(phone, 24);
            buf.Write(checkSign);
            return buf;
        }
    };

    //请求手机验证码结果
    public struct CMD_GP_RequestPhoneVerificationCodeResult
    {
        public ushort resultCode;            //0成功	1错误
        public string errorMsg;        //错误提示 256

        public CMD_GP_RequestPhoneVerificationCodeResult(CyNetReadBuffer rb)
        {
            resultCode = rb.ReadUshort();
            errorMsg = rb.ReadString(256);
        }
    };

    //请求绑定手机
    public struct CMD_GP_RequestBindPhone
    {
        public int iVeriCode;          //验证码
        public uint dwUserID;         //绑定的玩家UserID
        public string phone;  //绑定的手机号 24
        public ushort wIsFormalAC;//是否是正式帐号 0不是(非正式帐号需要重新填写帐号、昵称、密码)  1是
        public string tNewAccount;//新的账户64
        public string tNewNickName;//新的昵称64
        public string tNewPassword;//新的密码66
        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(iVeriCode);
            buf.Write(dwUserID);
            buf.Write(phone, 24);
            buf.Write(wIsFormalAC);
            buf.Write(tNewAccount, 64);
            buf.Write(tNewNickName, 64);
            buf.Write(tNewPassword, 66);
            return buf;
        }
    };

    //请求绑定手机返回
    public struct CMD_GP_RequestBindPhoneBack
    {
        public ushort resultCode;            //0成功	1错误
        public string nowPhone;          //现在绑定的手机号 24
        public string errorMsg;          //错误提示 256
        public ushort wIsFormalAC;       //是否是正式帐号 0不是(非正式帐号需要重新填写帐号、昵称、密码)  1是
        public string tNewAccount;      //新的账户32
        public string tNewNickName;     //新的昵称32
        public string tNewPassword;     //新的密码33
        public CMD_GP_RequestBindPhoneBack(CyNetReadBuffer rb)
        {
            resultCode = rb.ReadUshort();
            nowPhone = rb.ReadString(24);
            errorMsg = rb.ReadString(256);
            wIsFormalAC = rb.ReadUshort();
            tNewAccount = rb.ReadString(64);
            tNewNickName = rb.ReadString(64);
            tNewPassword = rb.ReadString(66);
        }
    };

    //请求重置密码
    public struct CMD_GP_RequestResetPasswordByPhone
    {
        public int iVeriCode;          //验证码
        public ushort wType;             //0重置登录密码  1重置银行密码
        public string phone;  //绑定的手机号 24
        public string newPassword; //密码 66

        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(iVeriCode);
            buf.Write(wType);
            buf.Write(phone, 24);
            buf.Write(newPassword, 66);
            return buf;
        }
    };

    //请求重置密码返回
    public struct CMD_GP_RequestResetPasswordByPhoneBack
    {
        public ushort resultCode;            //0成功	1错误
        public string errorMsg;        //错误提示 256

        public CMD_GP_RequestResetPasswordByPhoneBack(CyNetReadBuffer rb)
        {
            resultCode = rb.ReadUshort();
            errorMsg = rb.ReadString(256);
        }
    };

}

