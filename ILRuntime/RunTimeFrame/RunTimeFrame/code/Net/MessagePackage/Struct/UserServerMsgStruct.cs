using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using CYNetwork.NetStruct;
using CYNetwork;

namespace MessagePackage.Struct
{
    /// <summary>
    /// 查询信息
    /// </summary>
    public struct CMD_GP_QueryIndividual
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 用户密码 33
        /// </summary>

        public string szPassword;
    };
    /// <summary>
    /// 修改密码
    /// </summary>
    public struct CMD_GP_ModifyLogonPassBack
    {
        public string szNewLogonPassword; //修改后新的用户登录密码 
    };

    /// <summary>
    /// 修改资料
    /// </summary>
    public struct CMD_GP_ModifyIndividual
    {
        /// <summary>
        /// 用户性别
        /// </summary>
        public byte cbGender;
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;

        /// <summary>
        /// 登录密码
        /// </summary>
        public string szPassword;

        /// <summary>
        /// 用户信息
        /// </summary>
        [MessageItem(MsgItemType.DICTIONARY)]
        public Dictionary<ushort, object> dictStr;
    }


    /// <summary>
    /// 查询签到
    /// </summary>
    public struct CMD_GP_CheckInQueryInfo
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 登录密码 33
        /// </summary>
        public string szPassword;
    };


    /// <summary>
    /// 绑定机器
    /// </summary>
    public struct CMD_GP_ModifyMachine
    {
        /// <summary>
        /// 绑定标志
        /// </summary>
        public byte cbBind;
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 用户密码 33
        /// </summary>
        public string szPassword;
        /// <summary>
        /// 机器序列 33
        /// </summary>
        public string szMachineID;
    };

    /// <summary>
    /// 购买道具
    /// </summary>
    public struct CMD_GPR_PropertyRequest
    {
        /// <summary>
        /// 使用对象
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 购买数目
        /// </summary>
        public ushort wItemCount;
        /// <summary>
        /// 道具索引
        /// </summary>
        public ushort wPropertyIndex;
        /// <summary>
        /// 用户密码 33
        /// </summary>				
        public string szPassword;
        /// <summary>
        /// 机器序列 33
        /// </summary>
        public string szMachineID;
    };

    /// <summary>
    /// 魅力兑换
    /// </summary>
    public struct CMD_GP_ModifyChangCharm
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 兑换数目
        /// </summary>
        public long lChangCount;
        /// <summary>
        /// 机器序列 33
        /// </summary> 		
        public string szMachineID;

    };

    /// <summary>
    /// 魅力查询
    /// </summary>
    public struct CMD_GP_ModifyQueryCharm
    {
        /// <summary>
        /// 查询魅力
        /// </summary>
        public uint dwUserID;
    };

    /// <summary>
    /// 签到信息
    /// </summary>
    public struct CMD_GP_CheckInInfo
    {
        /// <summary>
        /// 连续日期
        /// </summary>
        public uint wSeriesDate;
        /// <summary>
        /// 签到标识
        /// </summary>
        public bool bTodayChecked;
        /// <summary>
        /// 奖励金币 7
        /// </summary>	
        public long[] lRewardGold;

        public uint[] dwPropImageID;                //道具图片7

        public string[] szPropName;					//道具名字7,32
    };
    public struct CMD_GP_CheckInInfo_NEW
    {
        public string[] szPropDescribe;				//道具名字7,128
    };
    /// <summary>
    /// 签到结果
    /// </summary>
    [MessageBody]
    public struct CMD_GP_CheckInResult
    {
        /// <summary>
        /// 成功标识
        /// </summary>
        public bool bSuccessed;
        /// <summary>
        /// 当前金币
        /// </summary>
        public long lScore;
        /// <summary>
        /// 提示内容 128
        /// </summary>
        [MessageItem(MsgItemType.STRING, 0)]
        public string szNotifyContent;
    };
    //每日抽奖
    //请求抽奖信息
    public struct CMD_GP_DayRewardQueryInfo
    {
        public uint dwUserID;                           //用户标识
        public string szPassword;                           //登录密码33
    };

    //抽奖信息返回
    public struct CMD_GP_DayRewardInfo
    {
        public ushort wSeriesTodayReward;               //抽奖次数
        public ushort wFreeCount;                       //免费次数
        public ushort dwUseTimes;                       //砸蛋卡数量
    };

    //执行抽奖请求
    public struct CMD_GP_DayRewardDone
    {
        public uint dwUserID;                       //用户标识
        public string szPassword;                       //登录密码33
        public byte cbMode;                         //抽奖方式
        public string szMachineID;                  //机器序列33
    };

    //抽奖结果返回
    public struct CMD_GP_DayRewardResult
    {
        public bool bSuccessed;                     //成功标识
        public ushort wSeriesTodayReward;               //抽奖次数
        public uint dwPropID;                       //中奖奖品
        public uint DayRewardTypeID;                //类型
        public string szNotifyContent;              //提示内容128
    };



    /// <summary>
    /// 修改密码
    /// </summary>
    public struct CMD_GP_ModifyLogonPass
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 用户密码 33
        /// </summary>
        public string szDesPassword;
        /// <summary>
        /// 用户密码 33
        /// </summary>
        public string szScrPassword;
        /// <summary>
        /// 密保答案 32
        /// </summary>
        ///  public string szSafeAnswer;
    };


    //开通银行
    public struct CMD_GR_S_UserInsureEnableResult
    {
        public byte cbActivityGame;                     //游戏动作
        public byte cbInsureEnabled;					//使能标识
        public string szDescribeString;				//描述消息
    };

    /// <summary>
    /// 排行数据
    /// </summary>
    public struct CMD_GP_WealthRankTopResult
    {
        public uint dwRank;                               //排名
        public string szNickName;                       //玩家昵称
        public ushort wFaceID;                           //头像标识
        public long lScore;                                //金币数额
        public int iVipLvl;                             //VIP等级
        public string szDeclaration;                   //个性签名
    };


    /// <summary>
    /// 修改签名
    /// </summary>
    [MessageBody]
    public struct CMD_GP_ModifyUnderWrite
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary> 
        /// 用户密码 33
        /// </summary>
        [MessageItem(MsgItemType.STRING, 66)]
        public string szPassword;
        /// <summary>
        /// 个性签名 32
        /// </summary>
        [MessageItem(MsgItemType.STRING, 0)]
        public string szUnderWrite;
    };

    /// <summary>
    /// 修改密码
    /// </summary>
    public struct CMD_GP_ModifyInsurePass
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 用户密码
        /// </summary>			
        public string szDesPassword;
        /// <summary>
        /// 用户密码
        /// </summary>
        public string szScrPassword;
    };

    /// <summary>
    /// 修改头像(系统头像)
    /// </summary>
    [MessageBody]
    public struct CMD_GP_SystemFaceInfo
    {
        /// <summary>
        /// 头像标识
        /// </summary>
        public uint wFaceID;
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 用户密码 33
        /// </summary>
        [MessageItem(MsgItemType.STRING, 66)]
        public string szPassword;
        /// <summary>
        /// 机器序列 33
        /// </summary>
        [MessageItem(MsgItemType.STRING, 66)]
        public string szMachineID;
    };

    //修改昵称
    public struct CMD_GP_ChangeNickName
    {
        public uint dwUserID;                         //用户ID
        public ushort wType;                             //修改类型 0微信强制修改
        public string szNewNickName;      //用户昵称 64

        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(dwUserID);
            buf.Write(wType);
            buf.Write(szNewNickName, 64);
            return buf;
        }
    };

    //修改昵称
    public struct CMD_GP_ChangeNickNameResult
    {
        public ushort wResultCode;                       //结果标记 0成功 1失败
        public string szNewNickName;      //用户昵称 64
        public string szErrorMessage;              //错误提示 256

        public CMD_GP_ChangeNickNameResult(CyNetReadBuffer rb)
        {
            wResultCode = rb.ReadUshort();
            szNewNickName = rb.ReadString(64);
            szErrorMessage = rb.ReadString(256);
        }
    };

    /// <summary>
    /// 修改密码返回
    /// </summary>
    public struct CMD_GPA_ModifyPasswdResult
    {
        /// <summary>
        /// 操作代码
        /// </summary>
        public int lResultCode;
        /// <summary>
        /// 成功消息 128
        /// </summary>
        public string szDescribeString;
    };

    /// <summary>
    /// 修改头像
    /// </summary>
    public struct CMD_GP_CustomFaceData
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 用户密码 33
        /// </summary>
        public string szPassword;
        /// <summary>
        /// 机器序列 33
        /// </summary>
        public string szMachineID;
        /// <summary>
        /// 图片信息
        /// </summary>
        public byte[] cbCustomFace;
    };

    /// <summary>
    /// 个人资料
    /// </summary>
    [MessageBody]
    public struct CMD_GP_UserIndividual
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;

        // public byte bGender;                        // 性别
        [MessageItem(MsgItemType.DICTIONARY, MaxItemCount = 7)]
        public Dictionary<int, object> dictStr;     // 个人资料
    };

    /// <summary>
    /// 个人资料
    /// </summary>
    public struct DBO_GP_UserIndividual
    {
        //用户信息
        /// <summary>
        /// 用户 I D
        /// </summary>
        public long dwUserID;
        /// <summary>
        /// 真实名字 16
        /// </summary>
        public string szCompellation;

        //电话号码
        /// <summary>
        /// 固定电话 16
        /// </summary>
        public string szCity;
        /// <summary>
        /// 移动电话 12
        /// </summary>
        public string szProvice;

        //联系资料
        /// <summary>
        /// 电子邮件 33
        /// </summary>
        public string szEMail;
        /// <summary>
        /// 身份信息 19
        /// </summary>
        public string szIDCard;
        /// <summary>
        /// 联系地址 128
        /// </summary>
        public string szDwellingPlace;
    };


    /// <summary>
    /// 开通银行
    /// </summary>
    public struct CMD_GP_UserEnableInsure
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// 登录密码 33
        /// </summary>		
        public string szLogonPassWord;
        /// 银行密码 33
        /// </summary>		
        public string szInsurePassword;
        /// <summary>
        /// 机器序列 33
        /// </summary>

        public string szMachineID;
    }


    /// <summary>
    /// 存款操作
    /// </summary>
    public struct CMD_GP_UserSaveScore
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 存入金币
        /// </summary>
        public long lSaveScore;
        /// <summary>
        /// 机器序列 33
        /// </summary>

        public string szMachineID;
    }


    /// <summary>
    /// 取款操作
    /// </summary>
    public struct CMD_GP_UserTakeScore
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 提取金币
        /// </summary>
        public long lTakeScore;
        /// <summary>
        /// 银行密码 33
        /// </summary>		
        public string szPassword;
        /// <summary>
        /// 机器序列 33
        /// </summary>
        public string szMachineID;
    };

    /// <summary>
    /// 转账操作
    /// </summary>
    public struct CMD_GP_UserTransferScore
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public uint dwUserId;
        /// <summary>
        /// 转账金币
        /// </summary>
        public long lTransferScore;
        /// <summary>
        /// 银行密码33
        /// </summary>	
        public string szPassword;
        /// <summary>
        /// 目标用户
        /// </summary>
        public string szAccounts;
        /// <summary>
        /// 机器序列33
        /// </summary>
        public string szMachineID;
        /// <summary>
        /// 转账备注
        /// </summary>
        public string szTransRemark;
        /// <summary>
        /// 校验MD5
        /// </summary>
        public string tCheckSign;
    };

    /// <summary>
    /// 银行资料
    /// </summary>
    public struct CMD_GP_UserInsureInfo
    {
        /// <summary>
        /// 税收比例
        /// </summary>
        public ushort wRevenueTake;
        /// <summary>
        /// 税收比例
        /// </summary>
        public ushort wRevenueTransfer;
        /// <summary>
        /// 房间标识
        /// </summary>
        public ushort wServerID;
        /// <summary>
        /// 用户金币
        /// </summary>
        public long lUserScore;
        /// <summary>
        /// 银行金币
        /// </summary>
        public long lUserInsure;
        /// <summary>
        /// 转账条件
        /// </summary>
        public long lTransferPrerequisite;
    };
    /// <summary>
    /// 查询银行密码
    /// </summary>
    public struct CMD_GP_CheckBankPassword
    {
        public ushort wResult;        //0成功，1失败
        public string szDescribeString;//描述256
    }
    /// <summary>
    /// 转账数据
    /// </summary>
    public struct CMD_GP_TransferRecord
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 开始索引
        /// </summary>
        public int iStartIndex;
        /// <summary>
        /// 结束索引
        /// </summary>
        public int iEndIndex;
        /// <summary>
        /// 查询类型 0 发送   1 接收
        /// </summary>
        public int iQueryType;
        /// <summary>
        /// 查询种类 0 转账   1 红包
        /// </summary>
        public int iQueryKind;
    }

    /// <summary>
    /// 转账记录数据
    /// </summary>
    public struct TransferRecordVO
    {
        /// <summary>
        /// 发送者/接收者玩家昵称
        /// </summary>
        public string szSourceNickName;
        /// <summary>
        /// 发送者/接收者I D
        /// </summary>
        public uint dwSourceUserID;
        /// <summary>
        /// //发送者/接收者游戏I D
        /// </summary>
        public uint dwSourceGameID;
        /// <summary>
        /// 操作金币
        /// </summary>
        public long lOperationScore;
        /// <summary>
        /// 扣除的税收
        /// </summary>
        public long lTaxScore;
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime sOperationTime;
        /// <summary>
        /// 转帐备注
        /// </summary>
        public string szTransRemark;
    };

    /// <summary>
    /// 转账记录返回
    /// </summary>
    public struct CMD_GP_TransferRecordResult
    {
        /// <summary>
        /// 开始索引
        /// </summary>
        public int iStartIndex;
        /// <summary>
        /// 结束索引
        /// </summary>
        public int iEndIndex;
        /// <summary>
        /// 查询类型 0 发送   1 接收
        /// </summary>
        public int iQueryType;
        /// <summary>
        /// 查询种类 0 转账   1 红包
        /// </summary>
        public int iQueryKind;
        /// <summary>
        /// 最大索引
        /// </summary>
        public int iMaxIndex;
        /// <summary>
        /// 本轮查询转账记录条数
        /// </summary>
        public int iThisTurnLength;
        /// <summary>
        /// 转账记录数据
        /// </summary>
        public TransferRecordVO[] trRecord;
    };

    /// <summary>
    /// 返回税收比例（千分比）
    /// </summary>
    public struct CMD_GP_QueryTaxResult
    {
        public int iqueryType; //查询类型 0 查询当前税收比例 1 查询该帐号的税收详情
        public int iGoldMult;//金币与Money兑换比例(1￥可以兑换多少金币)
    }

    /// <summary>
    /// 查询税收详情
    /// </summary>
    public struct CMD_GP_QueryTaxDetail
    {
        public uint dwUserID; //查询者UserID
        public int iStartIndex; //开始索引
        public int iEndIndex; //结束索引
        public int iQueryType; //查询类型  1未领取  0已领取
        public int iQueryKind; //查询种类  0 转账   1 红包	2 转账+红包
    };

    /// <summary>
    /// 税收领取
    /// </summary>
    public struct CMD_GP_GetTax
    {
        public uint dwUserID;                         //领取者UserID
        public int iGetKind;							//领取种类  0 转账   1 红包	2 转账+红包
    };

    /// <summary>
    /// 税收详情结构体
    /// </summary>
    public struct TaxDetailRecordVO
    {
        public int iGameID; //目标用户GameID
        public long lTaxScore; //税收分数
        public DateTime tRecordTime; //时间
        public int iStatus; //状态 1未领取  0已领取        
    };

    /// <summary>
    /// 税收详情返回结果
    /// </summary>
    public struct CMD_GP_QueryTaxDetailResult
    {
        public long lTotalScore; //累计税收金币
        public long lEnableScore; //当前可用金币
        public int iStartIndex; //开始索引
        public int iEndIndex; //结束索引
        public int iMaxIndex; //最大索引
        public int iThisTurnLength; //本轮查询转账记录条数
        public TaxDetailRecordVO[] trRecord; //税收详情数据
    };

    public struct AgentStructVO
    {
        public uint dwUserID;                         //用户 I D
        public uint dwGameID;                         //游戏 I D
        public string szNickName;         //用户昵称
        public string szNotice;			//显示信息(微信等)
    }

    /// <summary>
    /// 代理
    /// </summary>
    public struct CMD_GP_AgentReply
    {
        public int iShowNum;                           //显示条数
        public AgentStructVO[] asShowContent;		//内容详情
    }

    //银行快捷操作
    public struct CMD_GP_BankQuickOperate
    {
        public int iOperaType;                         //操作类型	0查询  1更新
        public uint iOperaUserID;                       //操作人UserID
        public long[] iOperaData;       //银行偏好设置数据
    };

    //支付宝请求订单
    public struct CMD_GP_RequestRechargeOrderAlipay
    {
        public uint dwUserID;         //用户ID
        public long dwRechargeValue;   //充值金额

        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(dwUserID);
            buf.Write(dwRechargeValue);
            return buf;
        }
    };

    //支付宝请求订单返回结果
    public struct CMD_GP_RequestRechargeOrderAlipayResult
    {
        public string szBackUrl;   //充值订单链接 size:[999]

        public CMD_GP_RequestRechargeOrderAlipayResult(CyNetReadBuffer rb)
        {
            szBackUrl = StringUtil.TrimUnusefulChar(rb.ReadString(999 * 2));
        }
    };

    //微信请求订单
    public struct CMD_GP_RequestRechargeOrderWeiXin
    {
        public uint dwUserID;         //用户ID
        public long dwRechargeValue;   //充值金额

        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(dwUserID);
            buf.Write(dwRechargeValue);
            return buf;
        }
    };

    //微信请求订单返回结果
    public struct CMD_GP_RequestRechargeOrderWeiXinResult
    {
        public string szBackUrl;   //充值订单链接 size:[999]

        public CMD_GP_RequestRechargeOrderWeiXinResult(CyNetReadBuffer rb)
        {
            szBackUrl = StringUtil.TrimUnusefulChar(rb.ReadString(999 * 2));
        }
    };

    //充值请求订单数据
    public struct CMD_GP_RequestRechargeOrder
    {
        public uint dwUserID; //用户 I D
        public long dwRechargeValue; //充值金额
    };

    public struct CMD_GP_RequestRechargeOrder_3322
    {
        public uint dwUserID;                         //用户 I D
        public ushort wRechargeType;                     //充值平台 1微信  2支付宝
        public long dwRechargeValue;                   //充值金额
        public string szGoodTitle;                  //商品标题 [32]
        public string szGoodDesc;                       //商品描述 [32]
        public ushort wOsPlatform;						//使用系统  1AndroidApp  2IosApp  5Android手机浏览器  6Ios手机浏览器
        public string szAndroidPackageName;
        public string szIosBundleId;

        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buffer = new CyNetWriteBuffer();
            buffer.Write(dwUserID);
            buffer.Write(wRechargeType);
            buffer.Write(dwRechargeValue);
            buffer.Write(szGoodTitle, 64);
            buffer.Write(szGoodDesc, 64);
            buffer.Write(wOsPlatform);
            buffer.Write(szAndroidPackageName, 32);
            buffer.Write(szIosBundleId, 32);
            return buffer;
        }
    }
    //IOS请求充值返回订单数据
    public struct CMD_GP_RequestRechargeOrderResultByIos
    {
        public int iResultCode;                        //结果标记 0充值成功  其他失败
        public string szSuccessTransactionID;         //成功的订单66
        public string szNoticeMessage;             //提示信息256
    };
    /// <summary>
    /// 请求验证苹果内购订单
    /// </summary>
    public struct CMD_GP_RequestRechargeOrderByIos
    {
        public ushort wChargeSign;                       //充值验证类型  0沙盒测试  1正式充值
        public uint dwUserID;							//用户 I D
        public string szReceiptData;                    //19998长度
    }
    //充值返回订单数据
    [StructLayout(LayoutKind.Sequential)]
    public struct CMD_GP_RequestRechargeOrderResult
    {
        public uint dwUserID; //用户 I D
        public long llRechargeValue; //充值金额
        public string cParterChars; //商户 I D
        public long llOrderID; //订单号 I D
        public string cMd5Chars; //MD5加密结果

        public CMD_GP_RequestRechargeOrderResult(CyNetReadBuffer rb)
        {
            dwUserID = rb.ReadUint();
            llRechargeValue = rb.ReadLong();
            cParterChars = rb.ReadString(64);
            llOrderID = rb.ReadLong();
            cMd5Chars = rb.ReadString(64);
        }
    };

    public struct CMD_GP_RequestRechargeOrderResult_3322
    {
        public string szBackUrl;    //充值订单链接 [999]

        public CMD_GP_RequestRechargeOrderResult_3322(CyNetReadBuffer rb)
        {
            szBackUrl = rb.ReadString(999);
            szBackUrl = StringUtil.TrimUnusefulChar(szBackUrl);
        }
    }


    //银行快捷操作结果
    public struct CMD_GP_BankQuickOperateResult
    {
        public int iOperaResult;                       //操作结果	0查询成功  1更新成功	
        public uint iOperaUserID;                       //操作人UserID
        public long[] iOperaData;       //银行偏好设置数据
    };

    //心跳
    public struct CMD_GP_HeartBeat
    {
        public uint dwUserID; //请求心跳的用户UserID
    };

    public struct CMD_GP_HeartBeatResult
    {
        public int iIsOnline; //是否掉线
    };


    /// <summary>
    /// 税收领取
    /// </summary>
    public struct CMD_GP_GetTaxResult
    {
        public int iErrorCode; //错误码 0 领取成功 1 暂时没有可以领取的
        public long lGetScore; //领取的分数        
    };


    /// <summary>
    /// 救济金查询
    /// </summary>
    public struct CMD_GP_BaseEnsureQuery
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public uint dwUserID;
    };

    /// <summary>
    /// 查询银行
    /// </summary>
    public struct CMD_GP_QueryInsureInfo
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 用户密码33
        /// </summary>	
        public string szPassword;
    };

    /// <summary>
    /// 银行成功
    /// </summary>
    public struct CMD_GP_UserInsureSuccess
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public long dwUserID;
        /// <summary>
        /// 用户金币
        /// </summary>
        public long lUserScore;
        /// <summary>
        /// 银行金币
        /// </summary>
        public long lUserInsure;
        /// <summary>
        /// 备注 128
        /// </summary>		
        public string szDescribestring;
    };


    /// <summary>
    /// 银行成功
    /// </summary>
    public struct CMD_GP_UserTransferSuccess
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 交易税收
        /// </summary>
        public long lRevenue;
        /// <summary>
        /// 用户金币
        /// </summary>
        public long lUserScore;
        /// <summary>
        /// 银行金币
        /// </summary>
        public long lUserInsure;
    };

    /// <summary>
    /// 银行失败
    /// </summary>
    public struct CMD_GP_UserInsureFailure
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int lResultCode;
        /// <summary>
        /// 描述消息128
        /// </summary>
        public string szDescribestring;
    };
    /// <summary>
    /// 背包请求
    /// </summary>
    public struct CMD_GP_BagMsg
    {
        public uint dwUserID;							//玩家ID
    };
    /// <summary>
    /// 使用背包道具
    /// </summar
    public struct CMD_GP_UsePropMsg
    {
        public uint dwIndexID;
        public uint dwUserID;
        public uint dwTargetUserID;
        public uint dwSpecialValue;
        public string tCheckSign;
        public string strSpecialString;
    }
    /// <summary>
    /// 使用背包道具结果
    /// </summar
    public struct CMD_GP_UsePropResult
    {
        public ushort wUseResult;								//使用结果(0成功)
        public uint dwPropType;								//使用类型
        public uint dwValue;								    //值类型
        public bool bUpdate;								    //是否更新背包
        public string strValue;                                 //改名
    }
    /// <summary>
    /// 使用礼包返回
    /// </summary>
    public struct CMD_GP_SimpleGiftResultProp
    {
        public uint dwPropID;                 //道具ID  0:金币 1:钻石 其余:物品
        public long iPropNum;                 //数量
        public uint dwImageID;                //图标
        public string strPropName;	          //道具名称64
    }
    /// <summary>
    /// 背包信息
    /// </summary>
    ///
    public struct CMD_GP_BagPropInfoMsg
    {
        public byte cbStatus;                           //使用状态
        public ushort wBind;								//绑定信息
        public uint dwIndexID;
        public uint dwPropID;							//道具ID
        public uint dwUseType;							//使用类型
        public uint dwPropType;							//道具类型
        public uint dwPropValue;						//道具数据
        public uint dwImageID;							//图像ID
        public uint dwUseTimes;							//使用次数
        public uint dwLevelLimit;						//等级限定
        public uint dwRemainDay;						//剩余天数
        public uint dwSpecialValue;                     //特殊数值
        public string strSpecialInfo;	                    //特殊字符128
        public DateTime EndTime;							//道具到期时间
        public uint dwFlID;                             //道具分类
        public byte bCanBeAdd;							//是否可以叠加
        public string strPropName;		                //道具名称length32
        public string strDescribe;                      //描述信息length128

    };

    /// <summary>
    /// 道具信息
    /// </summary>
    public struct CMD_GP_PROPINFO
    {
        public uint dwPropID;							//道具ID
        public uint dwUseType;							//使用类型(主动、被动)
        public uint dwPropType;							//道具类型
        public uint dwPropValue;						//道具数据(期限、金币)
        public uint dwLevelLimit;						//等级限制
        public uint dwImageID;							//图片ID

        public uint dwShopID;                           //商店ID
        public uint dwPropPrize;                        //道具价格
        public uint dwPropRebate;                       //道具返利
        public uint dwPropBeyond;                       //道具类型
        public byte bCanBeAdd;							//是否可以叠加
        public uint dwFlID;
        public string strPropName;			            //道具名称length32
        public string strDescribe;		                //描述信息length128
    };
    /// <summary>
    /// 道具购买信息
    /// </summary>
    public struct CMD_GP_BuyPropMsg
    {
        public uint dwUserID;							//玩家ID
        public uint dwShopID;							//道具ID
        public uint dwNum;								//购买数量
    };
    /// <summary> 
    /// 购买道具成功
    /// </summary>
    public struct CMD_GP_PropertySuccess
    {
        /// <summary>
        /// 道具标识
        /// </summary>
        public ushort wPropertyID;
        /// <summary>
        /// 购买数量
        /// </summary>
        public ushort wPropertyCount;
        /// <summary>
        /// 会员等级
        /// </summary>
        public ushort wMemberOrder;
        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime MemberOverDate;
        /// <summary>
        /// 花费游戏币
        /// </summary>
        public long lConsumeGold;
    };


    /// <summary>
    /// 购买道具失败
    /// </summary>
    public struct CMD_GP_PropertyFailure
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int lResultCode;
        /// <summary>
        /// 描述消息 128
        /// </summary>
        public string szDescribestring;
    };

    /// <summary>
    /// 魅力兑换成功
    /// </summary>
    public struct CMD_GP_ChangedCharmSuccess
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 剩余魅力值
        /// </summary>
        public long lLessCharm;
        /// <summary>
        /// 已兑换魅力值
        /// </summary>
        public long lAllChangedCharm;
        /// <summary>
        /// 银行金币
        /// </summary>
        public long lUserInsure;
    };
    /// <summary>
    /// 魅力兑换失败
    /// </summary>
    public struct CMD_GP_ChangedCharmFailure
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public long lResultCode;
        /// <summary>
        /// 描述消息128
        /// </summary>
        public string szDescribestring;
    };

    /// <summary>
    /// 魅力查询成功
    /// </summary>
    public struct CMD_GP_QueryCharmSuccess
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 剩余魅力值
        /// </summary>
        public long lLessCharm;
        /// <summary>
        /// 已兑换魅力值
        /// </summary>
        public long lAllChangedCharm;
        /// <summary>
        /// 银行金币
        /// </summary>
        public long lUserInsure;
    };

    /// <summary>
    /// 魅力查询失败
    /// </summary>
    public struct CMD_GP_QueryCharmFailure
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int lResultCode;
        /// <summary>
        /// 描述消息 128
        /// </summary>
        public string szDescribestring;
    };

    /// <summary>
    /// 挂机用户条件查询
    /// </summary>
    public struct CMD_GP_ModifyQueryIsOnlineAward
    {
        /// <summary>
        /// 查询是否挂机用户
        /// </summary>
        public uint dwGameID;
    };

    /// <summary>
    /// 查询当前挂机状态
    /// </summary>
    public struct CMD_GP_ModifyQueryOnlineAward
    {
        /// <summary>
        /// 查询当前挂机状态
        /// </summary>
        public uint dwGameID;
    };

    /// <summary>
    /// 是否挂机用户查询结果
    /// </summary>
    public struct CMD_GP_QueryUserIsOnlineAWard
    {
        public long lResultCode;
    };

    /// <summary>
    /// 在线判断挂机用户状态
    /// </summary>
    public struct CMD_GP_QueryUserOnlineAWard
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int lResultCode;
        /// <summary>
        /// 描述消息128
        /// </summary>
        public string szDescribestring;
    };

    /// <summary>
    /// 提取结果
    /// </summary>
    public struct CMD_GP_UserTakeResult
    {
        /// <summary>
        /// 用户 I D
        /// </summary>
        public long dwUserID;
        /// <summary>
        /// 用户金币
        /// </summary>
        public long lUserScore;
        /// <summary>
        /// 银行金币
        /// </summary>
        public long lUserInsure;
    };


    /// <summary>
    /// 用户头像
    /// </summary>
    public struct CMD_GP_UserFaceInfo
    {
        /// <summary>
        /// 头像标识
        /// </summary>
        public uint wFaceID;
        /// <summary>
        /// 自定标识
        /// </summary>
        public uint dwCustomID;
    };

    /// <summary>
    /// 执行签到
    /// </summary>
    public struct CMD_GP_CheckInDone
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 登录密码 33
        /// </summary>			
        public string szPassword;
        /// <summary>
        /// 机器序列 33
        /// </summary>
        public string szMachineID;
    };

    //领取低保
    public struct CMD_GP_BaseEnsureTake
    {
        public uint dwUserID;            //用户 I D
        public string szPassword;        //登录密码
        public string szMachineID;      //机器序列
    };

    /// <summary>
    /// 查询用户
    /// </summary>
    public struct CMD_GP_QueryUserInfoRequest
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public uint dwDstGameID;
    };

    /// <summary>
    /// 用户信息
    /// </summary>
    public struct CMD_GP_UserTransferUserInfo
    {
        /// <summary>
        /// 目标用户
        /// </summary>
        public uint dwTargetGameID;
        /// <summary>
        /// 目标用户32
        /// </summary>

        public string szNickName;
    };
    //////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 操作失败
    /// </summary>
    public struct CMD_GP_OperateFailure
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int lResultCode;
        /// <summary>
        /// 描述消息128
        /// </summary>
        public string szDescribestring;
    };


    /// <summary>
    /// 查询银行信息
    /// </summary>
    public struct CMD_GPR_QueryBankRecord
    {
        public uint dwSrcGameID;
        public uint dwDstGameID;
        public ushort wDiffDay;
        public uint dwPageIndex;
        public string szPassWord;
    };

    /// <summary>
    /// 查询银行信息(响应)
    /// </summary>
    public struct CMD_GPA_InsureRecord
    {
        public byte cbPageCount;
        public uint dwTotalCount;
        public tagBankRecord bankRecord;
    }

    /// <summary>
    /// 银行信息
    /// </summary>
    public struct tagBankRecord
    {
        public uint dwSrcGameID;
        public uint dwDstGameID;
        public string szSrcNickName;
        public string szDstNickName;
        public long lOperateScore;
        public long lRevenue;
        public int nTradeType;
        public DateTime stOperateTime;
    };

    /// <summary>
    /// 登陆银行
    /// </summary>
    public struct CMD_GPR_LogonBank
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint dwUserID;
        /// <summary>
        /// 用户密码 33
        /// </summary>
        public string szPassWord;
    };

    /// <summary>
    /// 登陆银行结果
    /// </summary>
    public struct CMD_GPA_LogonBank
    {
        public byte cbLogonResult;
    };
    /// <summary>
    /// 操作成功
    /// </summary>
    [MessageBody]
    public struct CMD_GP_OperateSuccess
    {
        /// <summary>
        /// 操作代码
        /// </summary>
        public int lResultCode;
        /// <summary>
        /// 成功消息128
        /// </summary>
        [MessageItem(MsgItemType.STRING, 0)]
        public string szDescribestring;
    };


    /// <summary>
    /// 协调查找
    /// </summary>
    public struct CMD_GP_C_SearchCorrespond
    {
        /// <summary>
        /// 游戏标识
        /// </summary>
        public long dwGameID;
        /// <summary>
        /// 用户昵称 32
        /// </summary>	
        public string szNickName;
    };

    /// <summary>
    /// 协调查找
    /// </summary>
    public struct CMD_GP_S_SearchCorrespond
    {
        /// <summary>
        /// 用户数目
        /// </summary>
        public ushort wUserCount;
        /// <summary>
        /// 用户信息
        /// </summary>
        public tagUserRemoteInfo[] UserRemoteInfo;
    };

    /// <summary>
    /// 用户信息
    /// </summary>
    public struct tagUserRemoteInfo
    {
        //用户信息
        /// <summary>
        /// 用户标识
        /// </summary>
        public long dwUserID;
        /// <summary>
        /// 游戏标识
        /// </summary>
        public long dwGameID;
        /// <summary>
        /// 用户昵称 32
        /// </summary>	
        public string szNickName;

        //等级信息
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

        //位置信息
        /// <summary>
        /// 类型标识
        /// </summary>
        public ushort wKindID;
        /// <summary>
        /// 房间标识
        /// </summary>
        public ushort wServerID;
        /// <summary>
        /// 房间位置 32
        /// </summary>	
        public string szGameServer;
    }

    //低保参数
    public struct CMD_GP_BaseEnsureParamter
    {
        public long lScoreCondition;                  //游戏币条件
        public long lScoreAmount;                     //游戏币数量
        public byte cbTakeTimes;                       //领取次数    
        public byte cbAlreadyTakeTimes;                //今日已领取次数
        public byte bindingState;                      //是否已经绑定手机  0未绑定  1已绑定
    };


    //低保结果
    public struct CMD_GP_BaseEnsureResult
    {
        public bool bSuccessed;                    //成功标识
        public long lGameScore;                    //当前游戏币
        public string szNotifyContent;             //提示内容
    };

    //邮件数据
    public struct CMD_GP_MailData
    {
        public uint dwMailID;							//邮件ID
        public uint dwSenderID;							//发件人ID
        public uint dwReceiverID;						//接收人ID
        public uint dwPropID;                           //道具附件
        public ushort wPropNum;							//道具数量
        public uint dwImageID;                          //图片ID
        public string strInformation;                       //消息length256
        public string strName;						    //主题length256
        public ushort wIsRead;                          //是否已读（0：未读，1：已读）
        public DateTime BeginTime;							//创建时间
        public ushort wLimitDays;							//到期时间
        public ushort wMailType;                            //邮件类型
        public string SenderNickname;				        //发送者昵称length256
    };
    ////发送邮件时先查询用户信息
    //查询用户
    public struct CMD_GP_SendMail_UserInfoRequest
    {
        public byte cbByNickName;                       //昵称赠送
        public string szAccounts;			                //目标用户
    };
    //邮件发送失败
    public struct CMD_GP_SendMailFailure
    {
        public int lResultCode;					//错误代码
        public string szDescribeString;				//描述消息length256
    };
    //用户信息
    public struct CMD_GP_UserMailUserInfo
    {
        public uint dwTargetGameID;                 //目标用户
        public string szAccounts;			            //目标用户length32/宽字符要读64
        public uint dwTargetRealGameID;             //目标用户-gameID
        public uint dwVipLevel;                     //目标用户Vip等级

    };
    //发送邮件信息
    public struct CMD_GP_SendMail
    {
        public uint dwSenderUserID;						//Send
        public uint dwRecverUserTD;                     //Recv
        public string szSenderNickName;                 //发送者昵称length32
        public string szRecverNickName;                 //收信者昵称length32
        public string szMailName;                           //邮件主题length256
        public string szMailInfo;					        //邮件信息length256
        public uint dwPropIndexID;                      //道具附件
        public ushort wPropNum;							//道具数量
        public uint dwSpecialValue;						//特殊字段
    };
    //发送邮件结果
    public struct CMD_GP_SendMailResult
    {
        public int lResultCode;						//操作代码
        public string szDescribeString;				    //描述消息length128
    };
    //邮件操作
    public struct CMD_GP_MailOperation
    {
        public int lResultCode;						//操作代码
        public uint dwMailID;							//邮件ID
        public uint dwoperatorID;						//操作者ID
    };

    //CDKEY
    public struct CMD_GP_Request_CDKEY
    {
        public uint dwUserID;                       //领取玩家
        public string szCDKEY;                      //CDKEY码 Size:40

        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(dwUserID);
            buf.Write(szCDKEY, 40);
            return buf;
        }
    };

    //CDKEY
    public struct CMD_GP_RequestCdkeyResult
    {
        public ushort wResult;                       //0成功   1失败
        public string wErrorMsg;                   //错误提示 256
        public CMD_GP_RequestCdkeyResult(CyNetReadBuffer rb)
        {
            wResult = rb.ReadUshort();
            wErrorMsg = rb.ReadString(256);
        }
    };

    //操作结果
    public struct CMD_GP_MailOperation_Result
    {
        public int lOperCode;                           //操作代码
        public uint dwMailID;                           //邮件ID
        public uint dwoperatorID;                       //操作者ID
        public int lResultCode;                     //是否成功返回
        public string szDescribeString;				    //描述消息length128
    };
    //广播消息(走马灯)
    public struct CMD_GP_MarqueeMsg
    {
        public string strInfomation;                        //走马灯文字信息length128
        public ushort wCycleNum;                            //循环次数
        public byte cbType;								//类型
    };
    public struct CMD_GP_AddRechargeMoney
    {
        public uint dwUserID;							//用户 I D
        public uint dwGameRMB;							//重置货币
    }

    //等级服务

    //查询等级
    public struct CMD_GP_GrowLevelQueryInfo
    {
        public uint dwUserID;                           //用户标识
        public string szPassword;                           //用户密码33

        //附加信息
        public string szMachineID;                      //机器序列33
    };

    //等级配置
    public struct CMD_GP_GrowLevelConfig
    {
        public ushort wLevelCount;                      //等级数目
        public tagGrowLevelConfig[] GrowLevelItem;                      //等级配置60
    };

    //等级参数
    public struct CMD_GP_GrowLevelParameter
    {
        public ushort wCurrLevelID;                     //当前等级
        public uint dwExperience;                       //当前经验
        public uint dwCurgradeExperience;//当前等级经验
        public uint dwUpgradeExperience;                //下级经验
        public long lUpgradeRewardGold;                 //升级奖励
        public long lUpgradeRewardIngot;                //升级奖励
    };

    //等级升级
    public struct CMD_GP_GrowLevelUpgrade
    {
        public long lCurrScore;                         //当前游戏币
        public long lCurrIngot;                         //当前元宝
        public string szNotifyContent;              //提示内容128
    };
    public struct CMD_GP_RefreshMoneyResult
    {
        //用户信息
        public long lScore;                             //游戏币
        public long lInsure;                            //银行币
        public uint dwUserMedal;                        //元宝
        public double dBeans;                               //充值货币
        public ushort isRechargeFirst;//首充信息 0未领取 1已领取
        //提示信息
        public string szNotifyContent;              //提示内容256
    };
    /// <summary>
    /// 大厅jackpot
    /// </summary>
    public struct CMD_GP_ServerUpdateJackpot
    {
        public int GameID;         //游戏类别ID
        public long RecordJack;        //记录的Jack池子大小
        public long AddSpeed;      //每秒增加的值
        public uint RecordTime;       //记录时间(时间戳 单位s)
                                      //  public uint NowTime;		//现在时间(时间戳 单位s)
    }
    //#define LEN_NICKNAME 32				//昵称长度
    //#define LEN_PROPNAME 32				//物品名称长度
    //#define LEN_PROPDESCRIBE 128		//物品描述长度
    //#define LEN_QQ 16					//QQ长度
    //#define LEN_MOBILE_PHONE 12			//手机号长度
    //#define LEN_DWELLING_PLACE 256		//地址信息长度
    //请求抽奖物品列表
    public struct CMD_GP_LotteryInfo               //SUB_GP_LOTTERY_BASE_REQUEST
    {
        //1 请求抽奖剩余次数
        //2 请求转盘列表物品信息
        //3 请求抽奖
        //4 请求自己的抽奖记录信息
        //5 请求全服的抽奖记录信息
        public ushort sign;                      //通信标志位
        public uint dwUserID;                 //用户ID
        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(sign);
            buf.Write(dwUserID);
            return buf;
        }
    };
    //请求抽奖返回
    public struct CMD_GP_RequestLotteryBack        //SUB_GP_LOTTERY_RESULT_BACK
    {
        public ushort sign;                      //结果类型	0成功  1活动已关闭  2剩余抽奖次数不足
        public LotteryItemInfo lGotItem;                   //获得的物品
        public int iRecordID;                  //对应的获取记录ID
        public CMD_GP_RequestLotteryBack(CyNetReadBuffer rb)
        {
            sign = rb.ReadUshort();
            lGotItem = new LotteryItemInfo(rb);
            iRecordID = rb.ReadInt();
        }
    };
    //玩家抽奖剩余次数返回
    public struct CMD_GP_LotteryChanceBack         //SUB_GP_LOTTERY_CHANCE_BACK
    {
        public int iLeftLotteryTimes;          //剩余抽奖次数
    };

    //抽奖物品信息                                                                    
    public struct LotteryItemInfo
    {
        public ushort wItemIndex;                    //物品索引                        
        public string tItemName;                //物品名称   32                          
        public int iItemNum;                   //物品数量                              
        public int iItemType;                  //物品类型(对应资源图标)                
        public string tItemDescribe;           //物品描述   128      
        public LotteryItemInfo(CyNetReadBuffer rb)
        {
            wItemIndex = rb.ReadUshort();
            tItemName = rb.ReadString(64);
            iItemNum = rb.ReadInt();
            iItemType = rb.ReadInt();
            tItemDescribe = rb.ReadString(256);
        }
    };
    //抽奖物品列表返回
    public struct CMD_GP_LotteryItemBack           //SUB_GP_LOTTERY_ITEM_BACK
    {
        public LotteryItemInfo[] lItemData;               //抽奖物品列表8
    };
    //简易抽奖记录
    public struct SimpleLotteryRecord
    {
        public string tNickName;     //中奖玩家昵称32
        public string iItemName;       //中奖物品名称32
        public int iItemNum;                   //中奖物品数量
        public SimpleLotteryRecord(CyNetReadBuffer rb)
        {
            tNickName = rb.ReadString(64);
            iItemName = rb.ReadString(64);
            iItemNum = rb.ReadInt();
        }
    };
    //详细抽奖记录
    public struct DetailLotteryRecord
    {
        public int iRecordID;              //记录ID
                                           // public SimpleLotteryRecord sItemData;              //中奖物品信息
        public string iItemName;//[LEN_NICKNAME];	中奖物品名称32
        public int iItemNum;                    //中奖物品数量
        public int iPlatformItemID;			//对应平台物品ID(对应资源图标)
        public DateTime sLotteryTime;            //抽中奖励的时间
        public ushort wState;                    //兑奖状态
        public DetailLotteryRecord(CyNetReadBuffer rb)
        {
            iRecordID = rb.ReadInt();
            iItemName = rb.ReadString(64);
            iItemNum = rb.ReadInt();
            iPlatformItemID = rb.ReadInt();
            sLotteryTime = rb.ReadDateTime();
            wState = rb.ReadUshort();
        }
    };
    //自己最近的抽奖记录
    public struct CMD_GP_SelfLotteryRecordBack     //SUB_GP_SELF_LOTTERY_RECORD_BACK
    {
        public ushort wIsAdd;                   //0全部数据 1单条数据
        public ushort wRecordNum;                	//中奖记录条数
        public DetailLotteryRecord[] sRecordData;            //中奖数据99
        public CMD_GP_SelfLotteryRecordBack(CyNetReadBuffer rb)
        {
            wIsAdd = rb.ReadUshort();
            wRecordNum = rb.ReadUshort();
            sRecordData = new DetailLotteryRecord[wRecordNum];
            for (int i = 0; i < wRecordNum; i++)
            {
                sRecordData[i] = new DetailLotteryRecord(rb);
            }
        }
    };
    //全服最近24h抽奖记录
    public struct CMD_GP_WholeLotteryRecordBack    //SUB_GP_WHOLE_LOTTERY_RECORD_BACK
    {
        public ushort wIsAdd;                        //0全部数据 1单条数据
        public ushort wRecordNum;					//中奖记录条数
        public SimpleLotteryRecord[] sRecordData;            //中奖数据99
        public CMD_GP_WholeLotteryRecordBack(CyNetReadBuffer rb)
        {
            wIsAdd = rb.ReadUshort();
            wRecordNum = rb.ReadUshort();
            sRecordData = new SimpleLotteryRecord[wRecordNum];
            for (int i = 0; i < wRecordNum; i++)
            {
                sRecordData[i] = new SimpleLotteryRecord(rb);
            }
        }
    };

    //手动领取
    public struct CMD_GP_ReceiveLottery            //SUB_GP_RECEIVE_LOTTERY
    {
        public int iRecordID;                      //领取的记录ID
        public uint dwUserID;                     //用户ID
        public string tPlayerName;        //领奖人姓名(可为空)32
        public string tQQ;                  //领奖人QQ(可为空)16
        public string tPhone;     //领奖人手机号(可为空)12
        public string tAdress;  //领奖人地址(可为空)256
        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(iRecordID);
            buf.Write(dwUserID);
            buf.Write(tPlayerName,64);
            buf.Write(tQQ,32);
            buf.Write(tPhone,24);
            buf.Write(tAdress,512);
            return buf;
        }
    };

    //闲玩每日分享
    public struct CMD_GP_DailyShareWeiXin
    {
        public uint dwUserID;                         //玩家ID
        public CyNetWriteBuffer Serialize()
        {
            CyNetWriteBuffer buf = new CyNetWriteBuffer();
            buf.Write(dwUserID);            
            return buf;
        }
    };

    //手动领取返回
    public struct CMD_GP_ReceiveLotteryBack        //SUB_GP_RECEIVE_LOTTERY_BACK
    {
        public int iRecordID;                  //领取的记录ID
        public ushort sign;						//1领取成功-普通物品  2领取成功-审核中  3不存在的抽奖记录  4已领取,无法继续领取

        public CMD_GP_ReceiveLotteryBack(CyNetReadBuffer rb)
        {
            iRecordID = rb.ReadInt();
            sign = rb.ReadUshort();
        }
    };
}
