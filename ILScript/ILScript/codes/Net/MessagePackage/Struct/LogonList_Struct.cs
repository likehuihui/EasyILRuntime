using System;
using System.Collections.Generic;
using System.Text;

namespace MessagePackage.Struct
{

    //////////////////////////////////////////////////////////////////////////////////
    //游戏列表
    //游戏类型
    public struct tagGameType
    {
        public ushort wJoinID;                           //挂接索引
        public ushort wSortID;                           //排序索引
        public ushort wTypeID;                           //类型索引
        public ushort wHeatNum;                         //推荐星级
        public string szTypeName;             //种类名字 64
    };

    //游戏种类
    public struct tagGameKind
    {
        public ushort wTypeID;                           //类型索引
        public ushort wJoinID;                           //挂接索引
        public ushort wSortID;                           //排序索引
        public ushort wKindID;                           //类型索引
        public ushort wGameID;                           //模块索引
        public ushort wGameTypeID;                       //游戏类型
        public ushort wHeatNum;                          //火爆指数
        public uint dwOnLineCount;                        //在线人数
        public uint dwAndroidCount;                      //在线人数
        public uint dwFullCount;                      //满员人数
        public string szKindName;             //游戏名字64
        public string szProcessName;           //进程名字64
    };


    //游戏房间
    public struct tagGameServer
    {
        public ushort wKindID;                           //名称索引
        public ushort wNodeID;                           //节点索引
        public ushort wSortID;                           //排序索引
        public ushort wServerID;                         //房间索引
        public ushort wServerKind;                       //游戏
        public ushort wServerType;                       //类型
        public ushort wServerPort;                       //房间端口
        public long   lCellScore;						 //单元积分
        public long   lEnterScore;                        //进入积分
        public uint   dwServerRule;                         //房间规则
        public uint   dwOnLineCount;                        //在线人数
        public uint   dwAndroidCount;                         //房间规则
        public uint   dwFullCount;                      //满员人数
        public string szServerAddr;                      //房间名称64
        public string szServerName;                     //房间名称64
        public ushort wNowState;							//0空闲  1流畅  2火爆
    };

    public struct tagGameMatch
    {
        public ushort wServerID;					//房间标识
        public uint dwMatchID;						//比赛标识	
        public uint dwMatchNO;						//比赛场次	
        public byte cbMatchType;				    //比赛类型
        public string szMatchName;					//比赛名称64

        public byte cbMemberOrder;					//会员等级
        public byte cbMatchFeeType;			        //扣费类型
        public long lMatchFee;						//比赛费用	

        public string szPropName;				    //报名道具名称64
	    public uint dwImageID;					    //报名道具图片

        //比赛信息
        public ushort wStartUserCount;				//开赛人数
        public ushort wMatchPlayCount;				//比赛局数	

        //比赛奖励
        public ushort wRewardCount;					//比赛奖励

        //比赛时间
        public DateTime MatchStartTime;			    //开始时间
        public DateTime MatchEndTime;			    //结束时间

    }

}
