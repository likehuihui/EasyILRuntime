using System;
using System.Collections.Generic;
using System.Text;

namespace MessagePackage.Operation
{
    public class UserOperations
    {

        #region 服务命令 3 主命令在ConstDefine
        //////////////////////////////////////////////////////////////////////////////////
        //服务命令
        //账号服务
        public const ushort SUB_GP_MODIFY_MACHINE = 100;								//修改机器
        public const ushort SUB_GP_MODIFY_LOGON_PASS = 101;							    //修改密码
        public const ushort SUB_GP_MODIFY_INSURE_PASS = 102;							    //修改密码
        public const ushort SUB_GP_MODIFY_UNDER_WRITE = 103;                                //修改签名

        public const ushort SUB_GP_REQUEST_RECHARGE = 104;                                //请求充值
        public const ushort SUB_GP_REQUEST_RECHARGE_RESULT = 105;                                //充值返回订单数据

        //修改头像
        public const ushort SUB_GP_USER_FACE_INFO = 120;							    //头像信息
        public const ushort SUB_GP_SYSTEM_FACE_INFO = 122;							    //系统头像
        public const ushort SUB_GP_CUSTOM_FACE_INFO = 123;							    //自定头像
        public const ushort SUB_GP_CHANGE_NAME = 124;							    //修改昵称
        public const ushort SUB_GP_CHANGE_NAME_RESULT = 125;                                //修改昵称结果
        public const ushort SUB_GP_REQUEST_IOS_RECHARGE = 106;                                //applePay请求二次验证
        public const ushort SUB_GP_REQUEST_IOS_RECHARGE_BACK = 107;                                //applePay二次验证返回数据
        //心跳协议
        public const ushort SUB_REQUEST_HEARTBEAT = 130;							    //心跳请求
        public const ushort SUB_REQUEST_HEARTBEAT_RESULT = 131;							    //心跳返回

        //个人资料
        public const ushort SUB_GP_USER_INDIVIDUAL = 140;							    //个人资料
        public const ushort SUB_GP_QUERY_INDIVIDUAL = 141;							    //查询信息
        public const ushort SUB_GP_MODIFY_INDIVIDUAL = 152;                                //修改资料
        public const ushort SUB_GP_MODIFY_LOGON_PASS_BACK = 153;							    //修改密码返回

        //银行服务
        public const ushort SUB_GP_USER_ENABLE_INSURE = 160;							    //开通银行
        public const ushort SUB_GP_USER_SAVE_SCORE = 161;    						    //存款操作
        public const ushort SUB_GP_USER_TAKE_SCORE = 162;								//取款操作
        public const ushort SUB_GP_USER_TRANSFER_SCORE = 163;								//转账操作
        public const ushort SUB_GP_USER_INSURE_INFO = 164;								//银行资料
        public const ushort SUB_GP_QUERY_INSURE_INFO = 165;								//查询银行
        public const ushort SUB_GP_USER_INSURE_SUCCESS = 166;								//银行成功
        public const ushort SUB_GP_USER_INSURE_FAILURE = 167;								//银行失败
        public const ushort SUB_GP_QUERY_USER_INFO_REQUEST = 168;								//查询用户
        public const ushort SUB_GP_QUERY_USER_INFO_RESULT = 169;								//用户信息
        public const ushort SUB_GP_USER_INSURE_ENABLE_RESULT = 170;							    //开通结果
        public const ushort SUB_GP_TRANSFER_RECORD = 171;                                //转账记录查询
        public const ushort SUB_GP_TRANSFER_RECORD_RESULT = 172;							    //转账记录查询结果

        public const ushort SUB_GP_GET_GAME_JACKPOT = 7;//获取jackpot
        public const ushort SUB_GP_BACK_GAME_JACKPOT = 107;//返回jackpot
        //税收比例查询
        public const ushort SUB_GP_QUERY_TAX = 173;
        //税收比例查询结果
        public const ushort SUB_GP_QUERY_TAX_RESULT = 174;
        //税收详情查询
        public const ushort SUB_GP_QUERY_TAX_DETAIL = 175;
        //税收详情查询结果
        public const ushort SUB_GP_QUERY_TAX_DETAIL_RESULT = 176;
        //领取税收
        public const ushort SUB_GP_GET_TAX = 177;
        //领取税收结果
        public const ushort SUB_GP_GET_TAX_RESULT = 178;
        //银行快捷操作
        public const ushort SUB_GP_BANK_QUICK_OPERA = 179;
        //银行快捷操作结果
        public const ushort SUB_GP_BANK_QUICK_OPERA_RESULT = 180;

        //支付宝充值请求
        public const ushort SUB_GP_REQUEST_RECHARGE_ALIPAY = 190;
        //支付宝充值请求返回
        public const ushort SUB_GP_REQUEST_RECHARGE_ALIPAY_RESULT = 191;
        //微信充值请求
        public const ushort SUB_GP_REQUEST_RECHARGE_WEIXIN = 192;
        //微信充值请求返回
        public const ushort SUB_GP_REQUEST_RECHARGE_WEIXIN_RESULT = 193;

        //比赛服务
        public const ushort SUB_GP_MATCH_SIGNUP = 200;							    //比赛报名
        public const ushort SUB_GP_MATCH_UNSIGNUP = 201;								//取消报名
        public const ushort SUB_GP_MATCH_SIGNUP_RESULT = 202;								//报名结果

        //每日分享
        public const ushort SUB_GP_DAILY_SHARE_WEIXIN = 210;                         //每日分享（闲玩）

        //签到服务
        public const ushort SUB_GP_CHECKIN_QUERY = 220;								//查询签到
        public const ushort SUB_GP_CHECKIN_INFO = 221;								//签到信息
        public const ushort SUB_GP_CHECKIN_DONE = 222;								//执行签到
        public const ushort SUB_GP_CHECKIN_RESULT = 223;								//签到结果
        public const ushort SUB_GP_CHECKIN_INFO_NEW = 224;

        //每日抽奖
        public const ushort SUB_GP_DAYREWARD_QUERY = 230;								//抽奖信息请求
        public const ushort SUB_GP_DAYREWARD_INFO = 231;								//抽奖信息返回
        public const ushort SUB_GP_DAYREWARD_COUNT = 237;								//抽奖次数返回

        public const ushort SUB_GP_DAYREWARD_DONE = 232;								//执行抽奖请求
        public const ushort SUB_GP_DAYREWARD_RESULT = 233;								//抽奖结果返回

        public const ushort SUB_GP_GET_DAYREWARD_DONE = 234;								//执行一键领奖请求
        public const ushort SUB_GP_GET_DAYREWARD_RESULT = 235;								//一键领取奖品结果
        public const ushort SUB_GP_GET_DAYREWARD_FINISH = 236;								//一键领取奖品完成

        //任务服务
        public const ushort SUB_GP_TASK_LOAD = 240;								//加载任务
        public const ushort SUB_GP_TASK_TAKE = 241;								//领取任务
        public const ushort SUB_GP_TASK_REWARD = 242;								//任务奖励
        public const ushort SUB_GP_TASK_INFO = 243;								//任务信息
        public const ushort SUB_GP_TASK_LIST = 244;								//任务信息
        public const ushort SUB_GP_TASK_RESULT = 245;								//任务结果

        //低保服务
        public const ushort SUB_GP_BASEENSURE_LOAD = 260;								//加载低保
        public const ushort SUB_GP_BASEENSURE_TAKE = 261;								//领取低保
        public const ushort SUB_GP_BASEENSURE_PARAMETER = 262;								//低保参数
        public const ushort SUB_GP_BASEENSURE_RESULT = 263;							    //低保结果

        //推广服务
        public const ushort SUB_GP_SPREAD_QUERY = 280;								//推广奖励
        public const ushort SUB_GP_SPREAD_INFO = 281;								//奖励参数

        //等级服务
        public const ushort SUB_GP_GROWLEVEL_QUERY = 300;							    //查询等级
        public const ushort SUB_GP_GROWLEVEL_PARAMETER = 301;								//等级参数
        public const ushort SUB_GP_GROWLEVEL_UPGRADE = 302;								//等级升级

        //兑换服务
        public const ushort SUB_GP_EXCHANGE_QUERY = 320;								//兑换参数
        public const ushort SUB_GP_EXCHANGE_PARAMETER = 321;								//兑换参数
        public const ushort SUB_GP_PURCHASE_MEMBER = 322;							    //购买会员
        public const ushort SUB_GP_PURCHASE_RESULT = 323;								//购买结果
        public const ushort SUB_GP_EXCHANGE_SCORE = 324;								//兑换游戏币
        public const ushort SUB_GP_EXCHANGE_RESULT = 325;								//兑换结果
        //刷新
        public const ushort SUB_GP_REFRESH_MONEY_QUERY = 330;								//刷新货币信息请求
        public const ushort SUB_GP_REFRESH_MONEY_RESULT = 331;								//刷新货币信息结果
        public const ushort SUB_GP_ADD_RECHARGE_MONEY = 333;                                //添加充值金额

        public const ushort SUB_GP_LIKE_GAME_QUERY = 340;								//修改喜爱游戏

        public const ushort SUB_GP_WEALTH_RANK_TOP = 350;                                //请求排行榜
        public const ushort SUB_GP_WEALTH_RANK_TOP_RESULT = 351;                                //排行榜返回

        //操作结果
        public const ushort SUB_GP_OPERATE_SUCCESS = 500;								//操作成功
        public const ushort SUB_GP_OPERATE_FAILURE = 501;								//操作失败

        //////////////////////////////////////////////////////////////////////////////////

        //道具商城
        public const ushort SUB_GP_PROP_DATA_REQUEST = 101;								//道具数据请求
        public const ushort SUB_GP_PROP_DATA_BACK = 102;								//数据接收返回
        public const ushort SUB_GP_PROP_DATA_FINISH = 103;								//数据接收返回
        public const ushort SUB_GP_PROP_BUY = 106;							    //购买道具
        public const ushort SUB_GP_PROP_BUY_SUCCESS = 107;							    //购买道具成功
        public const ushort SUB_GP_PROP_BUY_FAILED = 108;								//购买道具失败
        public const ushort SUB_GP_PROP_GIFT = 109;                              //赠送道具(使用邮件的结构体）
        public const ushort SUB_REQUEST_AGENT = 110;									//代理查询
        public const ushort SUB_REQUEST_AGENT_RESULT = 111;									//代理查询结果

        //背包
        public const ushort SUB_GP_BAG_REQUEST = 900;								//请求背包道具信息
        public const ushort SUB_GP_BAG_DATA_BACK = 901;								//返回背包信息
        public const ushort SUB_GP_BAG_DATA_FINISH = 902;								//
        public const ushort SUB_GP_BAG_PROP_USE = 903;								//
        public const ushort SUB_GP_USE_PROP_BACK = 904;								//使用道具返回
        public const ushort SUB_GP_USE_PROP_THREW = 905;								//删除道具
        public const ushort SUB_GP_THREW_PROP_BACK = 906;                              //删除道具返回
        public const ushort SUB_GP_USE_GROUP_PROP_BACK = 907;								//使用礼包返回


        //邮件
        public const ushort SUB_GP_SEND_MAIL = 1001;								//发送邮件
        public const ushort SUB_GP_RECEIVE_MAIL = 1002;								//接收邮件
        public const ushort SUB_GP_GET_MAIL_DATA = 1003;								//获取邮件
        public const ushort SUB_GP_MAIL_DATA_BACK = 1004;								//邮件数据返回
        public const ushort SUB_GP_MAIL_DATA_BACK_FINISH = 1005;								//邮件数据完成
        public const ushort SUB_GP_SENDMAIL_FAILURE = 906;								//发送邮件失败
        public const ushort SUB_GP_MAIL_OPERATION = 907;							    //邮件操作

        //发送邮件
        public const ushort SUB_GP_SENDMAIL_USERINFO_REQUEST = 968;								//发送邮件查询用户
        public const ushort SUB_GP_SENDMAIL_USERINFO_RESULT = 969;								//发送邮件用户信息

        public const ushort SUB_GP_SEND_MAIL_RESULT = 971;					            //发送邮件回执
        public const ushort SUB_GP__MAIL_OPERATION_RESULT = 972;					            //邮件操作回执


        public const ushort MAIL_OPERATION_DEL = 1;                                 //删除邮件
        public const ushort MAIL_OPERATION_GETGOODS = 2;			                        //提取物品
        public const ushort MAIL_OPERATION_READ = 3;			                        //邮件读取

        //请求数据
        public const ushort SUB_GP_GET_MARQUEE_DATA = 160;                                  //获取数据
                                                                                            //数据信息
        public const ushort SUB_GP_MARQUEE_DATA_BACK = 171;									//数据返回
        public const ushort SUB_GP_MARQUEE_DATA_FINISH = 172;                                   //数据接收完成

        //抽奖

        public const ushort SUB_GP_LOTTERY_BASE_REQUEST = 360;                          //请求转盘基础信息
        public const ushort SUB_GP_LOTTERY_CHANCE_BACK = 361;                           //剩余抽奖次数 - 返回
        public const ushort SUB_GP_LOTTERY_ITEM_BACK = 362;                         //转盘抽奖物品信息 - 返回
        public const ushort SUB_GP_LOTTERY_RESULT_BACK = 363;                           //请求抽奖 - 返回
        public const ushort SUB_GP_SELF_LOTTERY_RECORD_BACK = 364;                          //查询自身抽奖记录 - 返回
        public const ushort SUB_GP_WHOLE_LOTTERY_RECORD_BACK = 365;                         //查询所有抽奖记录 - 返回
        public const ushort SUB_GP_RECEIVE_LOTTERY = 366;                           //手动领取
        public const ushort SUB_GP_RECEIVE_LOTTERY_BACK = 367;							//手动领取 - 返回

        #endregion


        #region 跑马灯信息
        public const ushort MDM_GP_MARQUEE = 6;                     //主协议

        public const ushort SUB_GP_GET_MARQUEE_NEW = 161;           //获取数据(新)
        public const ushort SUB_GP_MARQUEE_NEW_BACK = 173;          //新的广播数据返回
        public const ushort SUB_GP_MARQUEE_NEW_FINSH = 174;         //新的广播数据接收完成
        #endregion


        #region CDKey
        public const ushort SUB_GP_REQUEST_CDKEY = 999;             //CDKEY兑换请求
        public const ushort SUB_GP_REQUEST_CDKEY_BACK = 9999;       //CDKEY兑换请求返回
        #endregion


    }
}
