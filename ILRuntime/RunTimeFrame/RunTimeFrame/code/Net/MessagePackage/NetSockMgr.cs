using System;
using System.Collections.Generic;

using System.Text;
using CYNetwork;
using CYNetwork.NetStruct;
using UnityEngine;

namespace MessagePackage
{
    class NetSockMgr
    {
        private CySocket mLogonSock = null;
        private CySocket mGameSock = null;

        /// <summary>
        /// 
        /// </summary>
        
       public CySocket GameSock
        {
            get { return mGameSock; }
        }

        public NetSockMgr()
        {

        }

        /// <summary>
        /// 连接游戏服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="netEvent"></param>
        /// <returns></returns>
        public bool ConnectGameSvr(string host,int port,INetEvent netEvent,ConnectAndSend func)
        {
            if (mGameSock != null)
            {
                mGameSock.CloseCySocket();
                mGameSock = null;
            }
            mGameSock = new CySocket();
            mGameSock.IsGameServer = true;
            //UnityEngine.Debug.Log("mGameSock.IsGameServer" + mGameSock.IsGameServer);
            if (mGameSock.IsConnected == false)
                mGameSock.AsyncConnect(host, port, netEvent, func);
            else  func();
            return true;
        }

        public bool CloseGameSock()
        {
            if (mGameSock != null)
            {
                mGameSock.CloseCySocket();
                mGameSock = null;
            }
                
            return true;
        }

        public bool CloseLogonSock()
        {
            if (mLogonSock != null)
            {
                mLogonSock.CloseCySocket();
                mLogonSock = null;
            }

            return true;
        }

        /// <summary>
        /// 身游戏服务器中写数据
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        public bool WriteGameMsg(ushort mainCmd,ushort subCmd,CyNetWriteBuffer writeBuffer)
        {
            if (null == mGameSock || false == mGameSock.IsConnected)
            {
               // NoticeManager.SendNoticeQuick(NetNotice.GAME_NET_DISCONNECT);
                throw new Exception("网络异常");
            }

            return (mGameSock.WriteData(mainCmd, subCmd, writeBuffer.NetBuffer, writeBuffer.Length) == 0);
        }

        Queue<WriteDataVO> rcQueue = new Queue<WriteDataVO>();

        /// <summary>
        /// 向登陆服务器中写数据
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="netEvent"></param>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="writeBuffer"></param>
        /// <returns></returns>
        public void  WriteLogonMsg(string host, int port, INetEvent netEvent,ushort mainCmd, ushort subCmd, CyNetWriteBuffer writeBuffer)
        {
            //if (mLogonSock != null /*&& mLogonSock.IsConnected*/)
            //{
            //    //mLogonSock.CloseCySocket(false);
            //    mLogonSock = null;
            //}
            //System.Console.WriteLine("链接地址 host： " + host + "  port:" + port);
            
            if (mLogonSock == null || mLogonSock.IsClosed)
            {
                lock(rcQueue)
                {
                    rcQueue.Clear();
                }
                
                //Debug.Log("建立大厅服务器的短链接");
                mLogonSock = new CySocket(true);
                mLogonSock.IsGameServer = false;
                mLogonSock.AsyncConnect(host, port, netEvent, () =>
                {
                    //Debug.Log("大厅服务器的链接成功");
                    mLogonSock.WriteData(mainCmd, subCmd, writeBuffer.NetBuffer, writeBuffer.Length);
                    lock (rcQueue)
                    {
                        while (rcQueue.Count > 0)
                        {
                            WriteDataVO rc = rcQueue.Dequeue();
                            mLogonSock.WriteData(rc);
                        }
                    }
                });
            }
            else
            {
                if (mLogonSock.IsConnected)
                {
                    mLogonSock.WriteData(mainCmd, subCmd, writeBuffer.NetBuffer, writeBuffer.Length);
                }
                else
                {                    
                    rcQueue.Enqueue(new WriteDataVO(mainCmd, subCmd, writeBuffer.NetBuffer, writeBuffer.Length));
                }                
            }
        }
    }
}
