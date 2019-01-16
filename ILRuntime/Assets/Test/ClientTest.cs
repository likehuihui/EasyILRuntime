/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
namespace RunTimeFrame
{
    public class ClientTest : MonoBehaviour
    {
        private Socket clientSocket;
        public void ConnectToServer()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8966);
            clientSocket.Connect(ipEndPoint);
        }
        /*
         数据总长度[4个字节] + 版本号[2个字节] + 命令号[2个字节] + 消息内容长度[4个字节] + 不定长数据
        */
        public void Send(string msg)
        {
            if (clientSocket != null && msg != null)
            {
                ByteBuffer writeByteBuffer = new ByteBuffer();
                byte[] content = Encoding.UTF8.GetBytes(msg);
                ushort visionId = 1;
                ushort commandId = 2;
                short contentLength = (short)content.Length;
                writeByteBuffer.WriteInt(contentLength + 10);
                writeByteBuffer.WriteUShort(visionId);
                writeByteBuffer.WriteUShort(commandId);
                writeByteBuffer.WriteString(msg);
                // 输出
                string strs = "";
                byte[] data = writeByteBuffer.ToBytes();
                for (int i = 0; i < data.Length; i++)
                {
                    strs += data[i] + " ";
                }
                Debug.Log("发送数据：" + strs);
                strs = "";
                for (int j = 0; j < content.Length; j++)
                {
                    strs += content[j] + " ";
                }
                Debug.Log("消息内容是：" + strs);
                clientSocket.Send(data);
            }
        }
        public void Close()
        {

        }
        void Start()
        {
            ClientTest client = new ClientTest();
            client.ConnectToServer();
            for (int i = 1; i <= 1; i++)
            {
                Debug.Log("发送第" + i + "次：");
                // Thread.Sleep(100);
                client.Send("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
            }
            //// 防止运行后立即退出
            // lO3qpWp3nQKTuIocQsdOQNEyL6cxjmTQ
            //while (true) { }
        }
    }
}