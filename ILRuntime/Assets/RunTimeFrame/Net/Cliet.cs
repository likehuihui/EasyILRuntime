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
using System.Text;
using System.Threading;
using UnityEngine;

namespace RunTimeFrame
{
    public class Cliet
    {
        //客户端负责接收服务端发来的数据消息的线程
        Thread threadClient = null;
        //创建客户端套接字，负责连接服务器
        Socket socketClient = null;
        public int prot = 8080;     
        public string ip = "127.0.0.1";           
        public Cliet(string ip,int port)
        {
            //获得文本框中的IP地址对象
            IPAddress address = IPAddress.Parse(ip);
            //创建包含IP和端口的网络节点对象
            IPEndPoint endpoint = new IPEndPoint(address, prot);
            //创建客户端套接字，负责连接服务器
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //客户端连接到服务器
                socketClient.Connect(endpoint);
            }
            catch (SocketException ex)
            {
                Debug.Log("客户端连接服务器发生异常：" + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.Log("客户端连接服务器发生异常：" + ex.Message);
            }
            threadClient = new Thread(ReceiveMsg);  //将ReceiveMsg传入
            threadClient.IsBackground = true;     //设置threadClient变量IsBackground为true
            threadClient.Start();                   //调用Start方法
            SendMessage("sever");                     //调用SendMessage发送消息到服务器
        }
        //接收服务器返回的消息
        void ReceiveMsg()
        {
            while (true)
            {
                //定义一个接收消息用的字节数组缓冲区（2M大小）
                byte[] arrMsgRev = new byte[1024 * 1024 * 2];
                //将接收到的数据存入arrMsgRev,并返回真正接收到数据的长度
                int length = -1;
                try
                {
                    length = socketClient.Receive(arrMsgRev);
                }
                catch (SocketException ex)
                {
                    System.Console.WriteLine("客户端接收消息时发生异常：" + ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("客户端接收消息时发生异常：" + ex.Message);
                    break;
                }

                //此时是将数组的所有元素（每个字节）都转成字符串，而真正接收到只有服务端发来的几个字符
                string strMsgReceive = Encoding.UTF8.GetString(arrMsgRev, 0, length);
                ParseData(strMsgReceive);
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage(string data)
        {
            //将字符串转成方便网络传送的二进制数组
            byte[] arrMsg = Encoding.UTF8.GetBytes(data);
            byte[] arrMsgSend = new byte[arrMsg.Length + 1];
            arrMsgSend[0] = 0;//设置标识位，0代表发送的是文字
            Buffer.BlockCopy(arrMsg, 0, arrMsgSend, 1, arrMsg.Length);
            try
            {
                socketClient.Send(arrMsgSend);
            }
            catch (SocketException ex)
            {
                System.Console.WriteLine("客户端发送消息时发生异常：" + ex.Message);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("客户端发送消息时发生异常：" + ex.Message);
            }
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        public void ParseData(string message)
        {
            string[] datas = message.Split('|');  //将message分割
            Debug.Log(datas);
           // EventManager.instance.AddEventData(datas[0], datas[1]); //调用EventManager中的AddEventData方法，添加消息

        }
    }
}
