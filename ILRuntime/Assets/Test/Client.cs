/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;

namespace RunTimeFrame
{
    public class Client : MonoBehaviour
    {

        /// <summary>
        /// 数据发送队列
        /// </summary>
        List<ArraySegment<byte>> _sendBufferList = new List<ArraySegment<byte>>();
        public string ip = "127.0.0.1";
        public int port = 8885;
        SocketAsyncEventArgs _sendEA;
        protected Socket _socket;
        SocketAsyncEventArgs _receiveEA;
        protected byte[] _receiveBuffer;
        /// <summary>
        /// 是否正在发送数据
        /// </summary>
        bool _isSending = false;
        ushort bufferSize = 1024;
        /// <summary>
        /// 缓冲区可用字节长度
        /// </summary>
        protected int _bufferAvailable = 0;
        /// <summary>
        /// 是否关闭，进行检查，如果返回true，则表示该远端代理结束
        /// </summary>
        bool IsFade
        {
            get
            {
                if (null == _socket)
                {
                    return true;
                }

                return false;
            }
        }
        private void Start()
        {
          
            _receiveBuffer = new byte[bufferSize];
            _sendEA = new SocketAsyncEventArgs();
            _sendEA.Completed += OnSendCompleted;
            _receiveEA = new SocketAsyncEventArgs();
            _receiveEA.Completed += OnReceiveCompleted;
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), port);
            _socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs connectEA = new SocketAsyncEventArgs();
            connectEA.RemoteEndPoint = ipe;
            connectEA.Completed += OnConnectCompleted;
            if (!_socket.ConnectAsync(connectEA))
            {
                OnConnectCompleted(null, connectEA);
            }
        }
        private void OnConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Completed -= OnConnectCompleted;
            if (null == e.ConnectSocket)
            {
                DispatchConnectFailEvent();
                return;
            }
            _socket = e.ConnectSocket;
            DispatchConnectSuccessEvent();
            StartReceive();
            Debug.Log(sender);
        }
        void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            lock (this)
            {
                if (e.SocketError == SocketError.Success)
                {
                    _isSending = false;
                    //尝试一次发送
                    SendBufferList();
                }
                else
                {
                    Close();
                }
            }
        }
        protected void DispatchConnectFailEvent()
        {
            //  onConnectFail?.Invoke(this, this);
        }
        protected void DispatchConnectSuccessEvent()
        {
            Debug.Log("连接成功");
            // onConnectSuccess?.Invoke(this, this);
        }
        /// <summary>
        /// 开始接受数据
        /// </summary>
        protected void StartReceive()
        {
            if (IsFade)
            {
                return;
            }

            _receiveEA.SetBuffer(_receiveBuffer, _bufferAvailable, _receiveBuffer.Length - _bufferAvailable);

            if (!_socket.ReceiveAsync(_receiveEA))
            {
                OnReceiveCompleted(null, _receiveEA);
            }
        }
        /// <summary>
        /// 处理接收到的消息（多线程事件）
        /// </summary>        
        void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {

        }
        public void SendTest()
        {
            BaseTcpProtocolBody obj = new BaseTcpProtocolBody();
            obj.value = DateTime.Now.ToFileTimeUtc().ToString();
           // Send(_pp.Pack(obj));
        }
        public void Send(byte[] bytes)
        {
            lock (this)
            {
                if (null == _socket)
                {
                    return;
                }


                _sendBufferList.Add(new ArraySegment<byte>(bytes));

                SendBufferList();
            }
        }
        void SendBufferList()
        {
            //如果没有在发送状态，则调用发送
            if (_isSending || _sendBufferList.Count == 0)
            {
                return;
            }

            _isSending = true;
            _sendEA.BufferList = _sendBufferList;

            _sendBufferList.Clear();

            if (!_socket.SendAsync(_sendEA))
            {
                OnSendCompleted(null, _sendEA);
            }
        }

        /// <summary>
        /// 断开客户端连接
        /// </summary>
        public void Close()
        {
            if (null != _socket)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Send);
                }
                catch (Exception) { }
                _socket.Close();
                _socket = null;
                _receiveBuffer = null;
                _bufferAvailable = 0;

                //onDisconnect?.Invoke(this, this);
            }
        }
        private void OnDestroy()
        {
            Close();
        }
    }
}