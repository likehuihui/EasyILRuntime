using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CYNetwork;
using System.Runtime.InteropServices;
using MessagePackage.Operation;
using MessagePackage.Struct;
using ProcessWM_CopyData;

namespace MessagePackage
{

    /// <summary>
    /// 接收消息回调函数
    /// </summary>
    /// <param name="mainCmd">主命令</param>
    /// <param name="subCmd">子命令</param>
    /// <param name="msgStrcut">消息结构体</param>
    public delegate bool MsgCallback(ushort mainCmd, ushort subCmd, object msgStrcut);

    /// <summary>
    /// 用户自处理消息接口
    /// </summary>
    /// <param name="mainCmd">主命令</param>
    /// <param name="subCmd">子命令</param>
    /// <param name="buffer">消息数据</param>
    /// <returns>返回false 表示用户 未处理消息,true 用户已处理消息</returns>
    public delegate bool HandleMsg(ushort mainCmd, ushort subCmd, CyNetReadBuffer buffer);

    /// <summary>
    /// 网络状态通知
    /// </summary>
    public delegate void NetCloseEvent();

    /// <summary>
    /// 网络错误消息通知
    /// </summary>
    /// <param name="errStr"></param>
    public delegate void NetNetError(int status,string errStr);


    public partial class MessageMgr : INetEvent
    {
        private static MessageMgr msgMgr = null;

        private NetSockMgr mNetSockMgr = null;

        private DelegateMsgBase mGameMsgMgr = null;

        private DelegateMsgBase mLogonMsgMgr = null;

        //
        private CProcessWM_CopyData mProcessWM_CopyData = null;
        /// <summary>
        /// U3D游戏客户端接口
        /// </summary>
        public static Interface_U3D_DT DT_Interface = null;
        /// <summary>
        /// U3D游戏房间接口
        /// </summary>
        public static Interface_U3D_ROOM ROOM_Interface = null;
        /// <summary>
        /// U3D比赛接口
        /// </summary>
        public static Interface_U3D_MATCH Match_Interface = null;
        /// <summary>
        /// U3D所有子游戏统一接口
        /// </summary>
        public static Interface_Game Game_Interface = null;
        public static uint PROCESS_VERSION(uint cbMainVer, uint cbSubVer, uint cbBuildVer)
        {
            return (uint)(
            (((byte)(6)) << 24) +
            (((byte)(cbMainVer)) << 16) +
            ((byte)(cbSubVer) << 8) +
            (byte)(cbBuildVer));
        }
        /// <summary>
        /// 游戏类型字典
        /// </summary>
        public Dictionary<ushort, tagGameType> Dic_GameType = new Dictionary<ushort, tagGameType>();
        public Dictionary<ushort, tagGameKind> Dic_GameKind = new Dictionary<ushort, tagGameKind>();
        public Dictionary<ushort, tagGameServer> Dic_GameRoom = new Dictionary<ushort, tagGameServer>();
        public Dictionary<ushort, tagGameMatch> Dic_GameMatch = new Dictionary<ushort, tagGameMatch>();
        /// <summary>
        /// 构造函数
        /// </summary>
        private MessageMgr()
        {
            mNetSockMgr = new NetSockMgr();
            mGameMsgMgr = new GameMessageMgr();
            mLogonMsgMgr = new LogonMessageMgr();
        }

        //设置并启用进程通信模式
        public bool SetProcessWM_CopyDataMode(string strClassName, string strWindowName, string strTagClassName, string strTagWindowName)
        {
            mProcessWM_CopyData = new CProcessWM_CopyData();
            if (!mProcessWM_CopyData.InitSendHWND(strClassName, strWindowName, strTagClassName, strTagWindowName))
            {
                mProcessWM_CopyData = null;
                return false;
            }
            mProcessWM_CopyData.Start();
            return true;
        }

        public LogonMessageMgr LogonMessageHelper
        {
            get { return mLogonMsgMgr as LogonMessageMgr; }
        }

        public GameMessageMgr GameMessageHelper
        {
            get { return mGameMsgMgr as GameMessageMgr; }
        }

        //登陆服务器的ip和端口
        private string logonHost = "";

        private int logonPort = 0;

        //版本定义
        private uint dwPlazaVersion = 0;
        private uint dwFrameVersion = 0;
        private uint dwProcessVersion = 0;

        //版本接口
        public void SetPlazaVersion(uint cbMainVer, uint cbSubVer, uint cbBuildVer)
        {
           if (dwPlazaVersion == 0)
                dwPlazaVersion =  PROCESS_VERSION(cbMainVer, cbSubVer, cbBuildVer);
        }
        public void SetFrameVersion(uint cbMainVer, uint cbSubVer, uint cbBuildVer)
        {
            if (dwFrameVersion == 0)
                dwFrameVersion = PROCESS_VERSION(cbMainVer, cbSubVer, cbBuildVer);
        }
        public void SetProcessVersion(uint cbMainVer, uint cbSubVer, uint cbBuildVer)
        {
            if (dwProcessVersion == 0)
                dwProcessVersion = PROCESS_VERSION(cbMainVer, cbSubVer, cbBuildVer);
        }

        public uint PlazaVersion
        {
            get { return dwPlazaVersion; }
        }
        public uint FrameVersion
        {
            get { return dwFrameVersion; }
        }
        public uint ProcessVersion
        {
            get { return dwProcessVersion; }
        }

        /// <summary>
        ///  登陆服务器的ip
        /// </summary>
        public string LogonHost
        {
            set { logonHost = value; }
            get { return logonHost; }
        }
        /// <summary>
        ///  登陆服务器端口
        /// </summary>
        public int LogonPort
        {
            set { logonPort = value; }   
            get { return logonPort; }
        }

        /// <summary>
        /// 获取消息管理类实例
        /// </summary>
        /// <returns></returns>
        public static MessageMgr CurMsgMgr
        {
            get
            {
                if (msgMgr == null) msgMgr = new MessageMgr();
                return msgMgr;
            }
        }

        public CySocket GameSock
        {
            get { return mNetSockMgr.GameSock; }
        }

        private MsgCallback msgCallback = null;
        private MsgCallback mGameCallBack = null;

        private HandleMsg mHandleMsg = null;

        private HandleMsg mGameHandleMsg = null;

       // private NetCloseEvent mMetNotify = null;

        private NetNetError mHallNetError = null;
        private NetNetError mGameNetError = null;

        /// <summary>
        ///  设置获消息回调函数
        /// </summary>
        public MsgCallback MsgCallFunc
        {
            set { msgCallback = value; }
            get { return msgCallback; }
        }

        /// <summary>
        /// 游戏sock事件回调接口
        /// </summary>
        public MsgCallback GameMsgCallBack
        {
            set { mGameCallBack = value; }
            get { return mGameCallBack; }
        }
        /// <summary>
        /// 用户自己处理消息
        /// </summary>
        public HandleMsg UserHandleMsg
        {
            set { mHandleMsg = value; }
            get { return mHandleMsg; }
        }

        public HandleMsg GameUserHandleMsg
        {
            set { mGameHandleMsg = value; }
            get { return mGameHandleMsg; }
        }

        /// <summary>
        ///  网络错误通知
        /// </summary>
        public NetNetError NetHallErrorNotify
        {
            set { mHallNetError = value; }
            get { return mHallNetError; }
        }

        public NetNetError NetGameErrorNotify
        {
            set { mGameNetError = value; }
            get { return mGameNetError; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool ConnectGameSvr(string host,int port,ConnectAndSend func)
        {
            return mNetSockMgr.ConnectGameSvr(host, port, mGameMsgMgr, func);
        }

        /// <summary>
        /// 底层调用错误信息
        /// </summary>
        /// <param name="errStr"></param>
        public void Error(int status,string errStr)
        {
            Console.WriteLine("MessageMgr:"+errStr);
            //if (null != mMetNotify) mMetNotify((int)NetStatus.SOCK_ERROR);
            if (Game_Interface != null)
            {
                DT_Interface.Error(errStr,0);
            }
            else if (ROOM_Interface != null)
            {
                ROOM_Interface.Error(errStr);
            }
            else if (DT_Interface!=null)
            {
                DT_Interface.Error(errStr,0);
            }
             
        }

        public void CloseEvent()
        {

        }

        /// <summary>
        /// 向登陆服务器写入数据
        /// </summary>
        /// <param name="mainCmd">主命令</param>
        /// <param name="subCmd">子命令</param>
        /// <param name="writeBuffer">数据缓冲</param>
        public void SendMsg2LgnSvr(ushort mainCmd, ushort subCmd, CyNetWriteBuffer writeBuffer)
        {            
            if (logonHost == null || logonHost == "" || logonPort == 0)
            {
                if (mHallNetError != null) mHallNetError(0, "请正确输入ip和端口号");
                return;
            }

            if (DT_Interface != null && DT_Interface.CheckNet() == false)
            {
                return;
            }

            mNetSockMgr.WriteLogonMsg(logonHost, logonPort, mLogonMsgMgr, mainCmd, subCmd, writeBuffer);
        }

        public void SendMsg2GameSvr(ushort mainCmd, ushort subCmd, CyNetWriteBuffer writeBuffer)
        {
//             if(Game_Interface!=null && Game_Interface.CheckGame()==false)
//                 return;

            if (mProcessWM_CopyData != null)
            {
                
                mProcessWM_CopyData.SendData(mainCmd,subCmd,writeBuffer);
            }
            else
            {
                mNetSockMgr.WriteGameMsg(mainCmd, subCmd, writeBuffer);
            }

        }

        /// <summary>
        /// 关闭游戏套接字连接
        /// </summary>
        /// <returns></returns>
        public bool CloseGameConnect()
        {            
            return mNetSockMgr.CloseGameSock();
        }

        /// <summary>
        /// 关闭登录服务器连接
        /// </summary>
        /// <returns></returns>
        public bool CloseLogonConnect()
        {
            return mNetSockMgr.CloseLogonSock();
        }

        /// <summary>
        /// 接收所有的消息
        /// </summary>
        /// <param name="mnCmd"></param>
        /// <param name="sbCmd"></param>
        /// <param name="readbuffer"></param>
        /// <returns></returns>
        public bool HandleReadNetData(ushort mnCmd, ushort sbCmd, CyNetReadBuffer readbuffer)
        {
            //System.Console.WriteLine("收到消息 mnCmd： " + mnCmd + "  sbCmd:" + sbCmd);
            //if (mHandleMsg != null && mHandleMsg(mnCmd, sbCmd, readbuffer)) return 0;

            //if (mLogonMsgMgr.OnMessage(mnCmd, sbCmd, readbuffer)) return 0;

            //if (mGameMsgMgr.OnMessage(mnCmd, sbCmd, readbuffer)) return 0;

            return false;
        }

    }
}
