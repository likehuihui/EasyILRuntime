
using System;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using UnityEngine;
using Like;
using CYNetwork.NetStruct;

namespace CYNetwork
{
    /// <summary>
    /// 客户端扩展接口
    /// </summary>
    public interface Extension_Interface
    {
        /// <summary>
        /// 客户端扩展接口命令回调
        /// </summary>
        /// <param name="mnCmd"></param>
        /// <param name="sbCmd"></param>
        /// <param name="readBuffer"></param>
        /// <returns></returns>
        bool HandleReadNetData(ushort mnCmd, ushort sbCmd, CyNetReadBuffer readBuffer);
    }
    //网络内核

    public delegate void ConnectAndSend();

    class NetRecvParam
    {
        public NetworkStream netStream;
        public CyNetReadBuffer netBuffer;
        public int bodyBufferSize;
    }

    class ConnectAsync
    {
        public TcpClient tc;
        public ConnectAndSend func;
        public object param;
    }

    /// <summary>
    /// 客户端与服务器通信类，负责收发数据
    /// 如果是短连接请用同步连接，
    /// 长连接是请用异步连接
    /// </summary>
    public class CySocket
    {
        /// <summary>
        /// U3D扩展接口 提供给客户扩展用
        /// </summary>
        public static Extension_Interface Extension_interface_ = null;

        const ushort MDM_KN_COMMAND = 0;									//内核命令
        const ushort SUB_KN_DETECT_SOCKET = 1;									//检测命令
        const ushort SUB_KN_VALIDATE_SOCKET = 2;                                    //验证命令

        const int SOCKET_TCP_BUFFER = 16384;                                       //网络缓冲  4096  

#if LINUX_SVR
        public const ushort HEADER_SIZE = 16;
        private const int HEAD_INFO_SIZE = 12;
        private const uint g_dwPacketKey = 0xAA55AA55;
        private const byte SOCKET_VER = 0x77;
#else
        public const ushort HEADER_SIZE = 8;
        private const int HEAD_INFO_SIZE = 4;
        private const uint g_dwPacketKey = 0xA55AA55A;
        private const byte SOCKET_VER = 0x05;
        //数据类型
        private const byte DK_MAPPED = 0x01;                //映射类型
        private const byte DK_ENCRYPT = 0x02;                //加密类型
        private const byte DK_COMPRESS = 0x04;                //压缩类型
#endif
        // 网络版本号

        // 网络连接socket
        private TcpClient mConSock;
        // 网络回调事件
        private INetEvent mDataCallBack;
        // 接收数据缓冲
        private byte[] rdata = new byte[16384];

        private NetworkStream mStream;

        private readonly byte[] g_SendByteMap = new byte[256]
        {
            0x70,0x2F,0x40,0x5F,0x44,0x8E,0x6E,0x45,0x7E,0xAB,0x2C,0x1F,0xB4,0xAC,0x9D,0x91,
            0x0D,0x36,0x9B,0x0B,0xD4,0xC4,0x39,0x74,0xBF,0x23,0x16,0x14,0x06,0xEB,0x04,0x3E,
            0x12,0x5C,0x8B,0xBC,0x61,0x63,0xF6,0xA5,0xE1,0x65,0xD8,0xF5,0x5A,0x07,0xF0,0x13,
            0xF2,0x20,0x6B,0x4A,0x24,0x59,0x89,0x64,0xD7,0x42,0x6A,0x5E,0x3D,0x0A,0x77,0xE0,
            0x80,0x27,0xB8,0xC5,0x8C,0x0E,0xFA,0x8A,0xD5,0x29,0x56,0x57,0x6C,0x53,0x67,0x41,
            0xE8,0x00,0x1A,0xCE,0x86,0x83,0xB0,0x22,0x28,0x4D,0x3F,0x26,0x46,0x4F,0x6F,0x2B,
            0x72,0x3A,0xF1,0x8D,0x97,0x95,0x49,0x84,0xE5,0xE3,0x79,0x8F,0x51,0x10,0xA8,0x82,
            0xC6,0xDD,0xFF,0xFC,0xE4,0xCF,0xB3,0x09,0x5D,0xEA,0x9C,0x34,0xF9,0x17,0x9F,0xDA,
            0x87,0xF8,0x15,0x05,0x3C,0xD3,0xA4,0x85,0x2E,0xFB,0xEE,0x47,0x3B,0xEF,0x37,0x7F,
            0x93,0xAF,0x69,0x0C,0x71,0x31,0xDE,0x21,0x75,0xA0,0xAA,0xBA,0x7C,0x38,0x02,0xB7,
            0x81,0x01,0xFD,0xE7,0x1D,0xCC,0xCD,0xBD,0x1B,0x7A,0x2A,0xAD,0x66,0xBE,0x55,0x33,
            0x03,0xDB,0x88,0xB2,0x1E,0x4E,0xB9,0xE6,0xC2,0xF7,0xCB,0x7D,0xC9,0x62,0xC3,0xA6,
            0xDC,0xA7,0x50,0xB5,0x4B,0x94,0xC0,0x92,0x4C,0x11,0x5B,0x78,0xD9,0xB1,0xED,0x19,
            0xE9,0xA1,0x1C,0xB6,0x32,0x99,0xA3,0x76,0x9E,0x7B,0x6D,0x9A,0x30,0xD6,0xA9,0x25,
            0xC7,0xAE,0x96,0x35,0xD0,0xBB,0xD2,0xC8,0xA2,0x08,0xF3,0xD1,0x73,0xF4,0x48,0x2D,
            0x90,0xCA,0xE2,0x58,0xC1,0x18,0x52,0xFE,0xDF,0x68,0x98,0x54,0xEC,0x60,0x43,0x0F
        };


        private byte[] g_RecvByteMap = new byte[256]
        {
            0x51,0xA1,0x9E,0xB0,0x1E,0x83,0x1C,0x2D,0xE9,0x77,0x3D,0x13,0x93,0x10,0x45,0xFF,
            0x6D,0xC9,0x20,0x2F,0x1B,0x82,0x1A,0x7D,0xF5,0xCF,0x52,0xA8,0xD2,0xA4,0xB4,0x0B,
            0x31,0x97,0x57,0x19,0x34,0xDF,0x5B,0x41,0x58,0x49,0xAA,0x5F,0x0A,0xEF,0x88,0x01,
            0xDC,0x95,0xD4,0xAF,0x7B,0xE3,0x11,0x8E,0x9D,0x16,0x61,0x8C,0x84,0x3C,0x1F,0x5A,
            0x02,0x4F,0x39,0xFE,0x04,0x07,0x5C,0x8B,0xEE,0x66,0x33,0xC4,0xC8,0x59,0xB5,0x5D,
            0xC2,0x6C,0xF6,0x4D,0xFB,0xAE,0x4A,0x4B,0xF3,0x35,0x2C,0xCA,0x21,0x78,0x3B,0x03,
            0xFD,0x24,0xBD,0x25,0x37,0x29,0xAC,0x4E,0xF9,0x92,0x3A,0x32,0x4C,0xDA,0x06,0x5E,
            0x00,0x94,0x60,0xEC,0x17,0x98,0xD7,0x3E,0xCB,0x6A,0xA9,0xD9,0x9C,0xBB,0x08,0x8F,
            0x40,0xA0,0x6F,0x55,0x67,0x87,0x54,0x80,0xB2,0x36,0x47,0x22,0x44,0x63,0x05,0x6B,
            0xF0,0x0F,0xC7,0x90,0xC5,0x65,0xE2,0x64,0xFA,0xD5,0xDB,0x12,0x7A,0x0E,0xD8,0x7E,
            0x99,0xD1,0xE8,0xD6,0x86,0x27,0xBF,0xC1,0x6E,0xDE,0x9A,0x09,0x0D,0xAB,0xE1,0x91,
            0x56,0xCD,0xB3,0x76,0x0C,0xC3,0xD3,0x9F,0x42,0xB6,0x9B,0xE5,0x23,0xA7,0xAD,0x18,
            0xC6,0xF4,0xB8,0xBE,0x15,0x43,0x70,0xE0,0xE7,0xBC,0xF1,0xBA,0xA5,0xA6,0x53,0x75,
            0xE4,0xEB,0xE6,0x85,0x14,0x48,0xDD,0x38,0x2A,0xCC,0x7F,0xB1,0xC0,0x71,0x96,0xF8,
            0x3F,0x28,0xF2,0x69,0x74,0x68,0xB7,0xA3,0x50,0xD0,0x79,0x1D,0xFC,0xCE,0x8A,0x8D,
            0x2E,0x62,0x30,0xEA,0xED,0x2B,0x26,0xB9,0x81,0x7C,0x46,0x89,0x73,0xA2,0xF7,0x72
        };

        byte m_cbSendRound;                     //字节映射
        byte m_cbRecvRound;                     //字节映射
        uint m_dwSendXorKey;                        //发送密钥
        uint m_dwRecvXorKey;                        //接收密钥
        uint m_dwSendPacketCount;               //发送计数
        uint m_dwRecvPacketCount;				//接受计数
        uint m_dwSendTickCount;                  // 发送时间
        uint m_dwRecvTickCount;                  // 接收时间          


        private Timer heartTimer;

        bool mIsShortConnect;

        /// <summary>
        /// 网络连接状态查询
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (null == mConSock) return false;
                return mConSock.Connected;
            }
        }

        bool _closed = false;

        /// <summary>
        /// 链接是否已关闭
        /// </summary>
        public bool IsClosed
        {
            get { return _closed; }
        }

        long _lastWriteTime = 0;

        // 接收时间
        internal uint RecvTickCount { get { return m_dwRecvTickCount; } }
        // 发送时间
        internal uint SendTickCount { get { return m_dwSendTickCount; } }
        public delegate void CloseSocketCallBack();
        public static CloseSocketCallBack CSCB = null;
        public bool IsGameServer = true;
        public CySocket(bool isShortConnect = false)
        {
            m_cbSendRound = 0;
            m_cbRecvRound = 0;
            m_dwSendXorKey = 0;
            m_dwRecvXorKey = 0;
            m_dwSendPacketCount = 0;
            m_dwRecvPacketCount = 0;
            mIsShortConnect = isShortConnect;
            CloseCySocket();
        }

        //将Byte转换为结构体类型
        public static byte[] StructToBytes(object structObj, int size)
        {
            //StructDemo sd;
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷贝到byte 数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return bytes;

        }

        //将Byte转换为结构体类型
        public static object ByteToStruct(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            // 分配结构体内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            // 将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            // 将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);
            // 释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }

        /**
         * 同步连接服务器
         * @param host      ip地址
         * @param port      端口号
         * @param callback  网络回调接口
         */
        public int Connect(string host, int port, INetEvent callback)
        {
            try
            {
                CloseCySocket();
                m_cbSendRound = 0;
                m_cbRecvRound = 0;
                m_dwSendXorKey = 0x12345678;
                m_dwRecvXorKey = 0x12345678;

                mDataCallBack = callback;
                mConSock = new TcpClient();
                mConSock.Connect(host, port);

                if (mConSock.Connected)
                {
                    mStream = mConSock.GetStream();
                    ReadData();
                }

                _closed = false;
            }
            catch (Exception e)
            {
                CloseCySocket();
                if (IsGameServer)
                {
                    if (CSCB != null)
                    {
                        CSCB();
                    }
                    CSCB = null;
                }
                if (null != mDataCallBack) mDataCallBack.Error(0, e.Message);
                return 1;
            }
            return 0;
        }



        /** 
         * 异步连接服务器
         * @param host      ip地址
         * @param port      端口号
         * @param callback  网络回调接口
         */
        public void AsyncConnect(string host, int port, INetEvent callback, ConnectAndSend sendFunc)
        {
            try
            {
                CloseCySocket();
                _closed = false;
                m_cbSendRound = 0;
                m_cbRecvRound = 0;
                m_dwSendXorKey = 0x12345678;
                m_dwRecvXorKey = 0x12345678;
               // Debug.Log("ip前：" + host);
                host = GetIpv6.GetIpV6(host);
               // Debug.Log("ip后：" + host);
                IPAddress[] address = Dns.GetHostAddresses(host);

                if (address[0].AddressFamily == AddressFamily.InterNetworkV6)
                {
                   // Debug.Log("当前网络ipv6");
                    mConSock = new TcpClient(AddressFamily.InterNetworkV6);
                }
                else
                {
                   // Debug.Log("当前网络ipv4");
                    mConSock = new TcpClient();
                }


                mConSock.SendTimeout = 30000;
                mConSock.ReceiveTimeout = 30000;

                mDataCallBack = callback;

                ConnectAsync ca = new ConnectAsync();
                ca.tc = mConSock;
                ca.func = sendFunc;
                mConSock.BeginConnect(host, port, new AsyncCallback(this.ConnectCallback), ca);
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                if (null != mDataCallBack) mDataCallBack.Error(0, e.Message);
            }
        }

        private void ConnectCallback(IAsyncResult iar)
        {
            if (null == mConSock) return;
            ConnectAsync ca = iar.AsyncState as ConnectAsync;
            try
            {
                mConSock.EndConnect(iar);
                mStream = mConSock.GetStream();

                if (null != ca) ca.func();
                ReadData();

                if(mIsShortConnect)
                {
                    StartShortConnectCheck();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                CloseCySocket();
                if (IsGameServer)
                {
                    if (CSCB != null)
                    {
                        CSCB();
                    }
                    CSCB = null;
                }
                if (null != mDataCallBack) mDataCallBack.Error(-999, e.Message);
            }
        }

        /// <summary>
        /// 关闭网络
        /// </summary>
        public void CloseCySocket()
        {
            queue.Clear();
            isWrite = true;
            if (null != mConSock)
            {
                try
                {
                    mConSock.Client.Shutdown(SocketShutdown.Send);
                    if (mStream != null) mStream.Close();
                    mConSock.Client.Shutdown(SocketShutdown.Receive);
                    mConSock.Close();
                }
                catch (Exception e)
                {
                    mConSock.Close();
                }
            }
            mStream = null;
            mConSock = null;

            _closed = true;
        }

        /**
		 * 
		 */
        private void ReadData()
        {
            if (null == mConSock || !mConSock.Connected) return;

            ReadHeader();
        }


        private void ReadHeader()
        {
            try
            {
                NetRecvParam param = new NetRecvParam();
                param.netStream = mStream;

                param.netBuffer = new CyNetReadBuffer();

                if (IsCombo && m_DataSize >= HEADER_SIZE)
                {
                    ushort PacketSize = BitConverter.ToUInt16(m_Data, 2);
                    mStream.BeginRead(param.netBuffer.NetBuffer, 0, PacketSize - m_DataSize, new AsyncCallback(this.ReadBodyCallback), param);
                }
                else
                {
                    mStream.BeginRead(param.netBuffer.NetBuffer, 0, HEADER_SIZE - m_DataSize, new AsyncCallback(this.ReadHeaderCallBack), param);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                CloseCySocket();
                if (IsGameServer)
                {
                    if (CSCB != null)
                    {
                        CSCB();
                    }
                    CSCB = null;
                }
                if (null != mDataCallBack) mDataCallBack.Error(1, e.Message);
            }

        }
        private void ReadHeaderCallBack(IAsyncResult iar)
        {
            if (mConSock.Connected == false) return;
            try
            {
                NetRecvParam param = (NetRecvParam)iar.AsyncState;
                temp = param.netStream.EndRead(iar);
                //Console.WriteLine("收到字节 !!!!!!!!!! "+temp);
                Array.Copy(param.netBuffer.NetBuffer, 0, m_Data, m_DataSize, temp);
                m_DataSize += temp;
                if (m_DataSize >= HEADER_SIZE)
                {
                    int len = BitConverter.ToUInt16(m_Data, 2);
                    if (len > param.netBuffer.Capacity)
                        param.netBuffer.Capacity = len + 64;
                    param.netBuffer.Length = len;
                    len -= HEADER_SIZE;
                    param.bodyBufferSize = len;
                    ReadBody(len, ref param);
                }
                else
                {
                    ReadData();
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
                CloseCySocket();
                if (IsGameServer)
                {
                    if (CSCB != null)
                    {
                        CSCB();
                    }
                    CSCB = null;
                }
                if (null != mDataCallBack) mDataCallBack.Error(1, e.Message);
            }
        }

        private void ReadBody(int len, ref NetRecvParam param)
        {
            if (null == mConSock || null == mStream || mConSock.Connected == false) return;
            try
            {

                param.netStream = mStream;

                mStream.BeginRead(param.netBuffer.NetBuffer, 0, len, new AsyncCallback(this.ReadBodyCallback), param);
            }
            catch (Exception e)
            {
                CloseCySocket();
                if (IsGameServer)
                {
                    if (CSCB != null)
                    {
                        CSCB();
                    }
                    CSCB = null;
                }
                if (null != mDataCallBack) mDataCallBack.Error(1, e.Message + "长度" + temp);
            }
        }


        //
        bool IsCombo = false;
        //数据缓冲
        byte[] m_Data = new byte[SOCKET_TCP_BUFFER];
        //数据大小
        int m_DataSize = 0;
        int temp = 0;
        private void ReadBodyCallback(IAsyncResult iar)
        {
            if (null == mConSock || null == mStream || mConSock.Connected == false) return;

            try
            {
                NetRecvParam param = (NetRecvParam)iar.AsyncState;
                temp = param.netStream.EndRead(iar);
                Array.Copy(param.netBuffer.NetBuffer, 0, m_Data, m_DataSize, temp);
                m_DataSize += temp;

                if (m_DataSize >= HEADER_SIZE)
                {
                    byte kind = m_Data[0];
                    ushort PacketSize = BitConverter.ToUInt16(m_Data, 2);
                    //只支持映射
                    if (kind != 1)
                        return;
                    //效验长度
                    if (m_DataSize < PacketSize)
                    {
                        IsCombo = true;
                        ReadData();
                        Console.WriteLine("收到服务器消息组合包长度 ： " + m_DataSize);
                        return;
                    }

                    int len = CrevasseBuffer(m_Data, PacketSize);
                    if (len < HEADER_SIZE) throw new Exception("解析不正确");

                    ushort mainCmd = BitConverter.ToUInt16(m_Data, 4);
                    ushort subCmd = BitConverter.ToUInt16(m_Data, 6);
                    Console.WriteLine("************ 收到服务器消息：manCmd=" + mainCmd + "   subCmd=" + subCmd + " " + len);
                    //if (mainCmd >= 100)
                    //{
                    //    SendNetHeartbeat(null);
                    //}
                    CyNetReadBuffer netBuffer = new CyNetReadBuffer();
                    Array.Copy(m_Data, netBuffer.NetBuffer, PacketSize);
                    netBuffer.Length = PacketSize;

                    m_DataSize -= PacketSize;
                    m_Data = new byte[SOCKET_TCP_BUFFER];

                    if (Extension_interface_ != null)
                    {
                        //客户端扩展接口
                        Extension_interface_.HandleReadNetData(mainCmd, subCmd, netBuffer);
                    }

                    if (MDM_KN_COMMAND == mainCmd && SUB_KN_DETECT_SOCKET == subCmd)
                    {
                        //侦测命令
                        //SendNetHeartbeat(netBuffer);
                    }
                    else if (mDataCallBack != null)
                    {
                        mDataCallBack.HandleReadNetData(mainCmd, subCmd, netBuffer);
                    }

                    IsCombo = false;
                }

                ReadData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                CloseCySocket();
                if (IsGameServer)
                {
                    if (CSCB != null)
                    {
                        CSCB();
                    }
                    CSCB = null;
                }
                if (null != mDataCallBack) mDataCallBack.Error(1, e.Message + "长度" + temp);
            }

        }
        System.Collections.Generic.Queue<System.Action<object>> queue = new System.Collections.Generic.Queue<System.Action<object>>();
        bool isWrite = true;

        /// <summary>
        /// 向网络写入数据
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int WriteData(WriteDataVO vo)
        {
            return WriteData(vo.mainCmd, vo.subCmd, vo.data, vo.dataSize);
        }

        /**
         * 向网络写入数据
         * @param mainCmd   主命令
         * @param subCmd    子命令
         * @param data      写入的数据
         */
        public int WriteData(ushort mainCmd, ushort subCmd, byte[] data, int len)
        {
            lock (queue)
            {
                queue.Enqueue((x) =>
                {
                    try
                    {
                        if (null == mConSock || mConSock.Connected == false || null == mStream) throw new Exception("网络未连接");

                        CyNetWriteBuffer writeBuffer = new CyNetWriteBuffer((len + 8) > CyNetBuffer.mDefaultSize ? len + 64 : CyNetBuffer.mDefaultSize);
                        writeBuffer.Write(new byte());
                        writeBuffer.Write(new byte());
                        writeBuffer.Write(new byte());
                        writeBuffer.Write(new byte());
                        writeBuffer.Write(mainCmd);
                        writeBuffer.Write(subCmd);
                        writeBuffer.Write(data, len);

                        int size = EncryptBuffer(writeBuffer.NetBuffer, (ushort)writeBuffer.Length, (ushort)writeBuffer.Capacity);
                        _lastWriteTime = DateTime.UtcNow.ToFileTimeUtc();
                        //Debug.LogFormat("[time:{0}]发送消息  mainCmd：{1}  subCmd：{2}  size:{3}", _lastWriteTime, mainCmd, subCmd, writeBuffer.Length);                       
                        
                        mStream.BeginWrite(writeBuffer.NetBuffer, 0, size, new AsyncCallback(WriteCallback), mStream);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("++++++" + e.ToString());
                        CloseCySocket();
                        if (IsGameServer)
                        {
                            if (CSCB != null)
                            {
                                CSCB();
                            }
                            CSCB = null;
                        }
                        if (null != mDataCallBack) mDataCallBack.Error(2, e.Message);
                    }
                });
                if (isWrite)
                {
                    isWrite = false;
                    queue.Dequeue()(null);
                }
            }
            return 0;
        }

        //public int WriteData (ushort mainCmd,ushort subCmd,ref object msgStruct)
        //{
        //    try
        //    {
        //        if (null == mStream || null == mConSock || mConSock.Connected == false) throw new Exception("网络未连接");

        //        CyNetWriteBuffer writeBuffer = new CyNetWriteBuffer();

        //        writeBuffer.Write(mainCmd);
        //        writeBuffer.Write(subCmd);
        //        writeBuffer.Write((ushort)0);
        //        writeBuffer.Write((ushort)0);
        //        writeBuffer.Write(ref msgStruct);

        //        int size = EncryptBuffer(writeBuffer.NetBuffer, (ushort)writeBuffer.Length, (ushort)writeBuffer.Capacity);
        //        Console.WriteLine("％％％％％发送消息　mainCmd：" + mainCmd + "　subCmd：" + subCmd + "  " + size);
        //        mStream.BeginWrite(writeBuffer.NetBuffer, 0, size, new AsyncCallback(this.WriteCallback), mStream);
        //    }
        //    catch (Exception e)
        //    {
        //       Console.WriteLine(e.ToString());
        //        CloseCySocket();
        //        if (null != mDataCallBack) mDataCallBack.Error(2,e.Message);
        //        return 1;
        //    }
        //    return 0;
        //}

        private void WriteCallback(IAsyncResult iar)
        {
            try
            {
                lock (queue)
                {
                    NetworkStream nstrea = (NetworkStream)iar.AsyncState;
                    isWrite = true;
                    if (null == mStream || null == mConSock || mConSock.Connected == false) return;
                    nstrea.EndRead(iar);
                    if (isWrite && queue.Count > 0)
                    {
                        isWrite = false;
                        queue.Dequeue()(null);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                CloseCySocket();
                if (IsGameServer)
                {
                    if (CSCB != null)
                    {
                        CSCB();
                    }
                    CSCB = null;
                }
                if (null != mDataCallBack) mDataCallBack.Error(2, e.Message);
            }
        }

        //映射发送数据
        private byte MapSendByte(byte cbData)
        {
            byte cbMap = g_SendByteMap[(byte)(cbData + m_cbSendRound)];
            m_cbSendRound += 3;
            return cbMap;
        }
        private byte MapRecvByte(byte cbData)
        {
            byte cbMap = (byte)(g_RecvByteMap[(byte)cbData] - m_cbRecvRound);
            m_cbRecvRound += 3;
            return cbMap;
        }
        //随机映射
        private ushort SeedRandMap(ushort wSeed)
        {
            uint dwHold = wSeed;
            return (ushort)((dwHold = dwHold * 241103 + 2533101) >> 16);
        }
        public struct GUID
        {
            public uint Data1;
            public ushort Data2;
            public ushort Data3;
            public uint Data4;
        }



        private ushort EncryptBuffer(byte[] pcbDataBuffer, ushort wDataSize, ushort wBufferSize)
        {
            if (wDataSize < HEADER_SIZE) return 0;
            if (wBufferSize < wDataSize + 2 * sizeof(uint)) return 0;
            //调整长度
            ushort wEncryptSize = (ushort)(wDataSize - HEAD_INFO_SIZE), wSnapCount = 0; // CMD_Command
            if ((wEncryptSize % sizeof(uint)) != 0)
            {
                wSnapCount = (ushort)(sizeof(uint) - (wEncryptSize % sizeof(uint)));
                Array.Clear(pcbDataBuffer, HEAD_INFO_SIZE + wEncryptSize, wSnapCount);
            }

            //效验码与字节映射
            byte cbCheckCode = 0;
            for (int i = HEAD_INFO_SIZE; i < wDataSize; i++)
            {
                cbCheckCode += pcbDataBuffer[i];
                pcbDataBuffer[i] = MapSendByte(pcbDataBuffer[i]);
            }

            //填写信息头
            pcbDataBuffer[0] = DK_MAPPED;
            pcbDataBuffer[1] = (byte)(~cbCheckCode + 1);
            BitConverter.GetBytes(wDataSize).CopyTo(pcbDataBuffer, 2);

            //创建密钥
            uint dwXorKey = m_dwSendXorKey;
            if (m_dwSendPacketCount == 0)
            {
                string str = Guid.NewGuid().ToString("N");
                GUID guid = new GUID();
                guid.Data1 = Convert.ToUInt32(str.Substring(0, 8), 0x10);
                guid.Data2 = Convert.ToUInt16(str.Substring(8, 4), 0x10);
                guid.Data3 = Convert.ToUInt16(str.Substring(12, 4), 0x10);
                guid.Data4 = Convert.ToUInt32(str.Substring(0x10, 8), 0x10);

                System.Random rand = new System.Random();
                //生成第一次随机种子
                dwXorKey = (uint)(System.Environment.TickCount * System.Environment.TickCount);
                dwXorKey ^= guid.Data1;
                dwXorKey ^= guid.Data2;
                dwXorKey ^= guid.Data3;
                dwXorKey ^= guid.Data4;
                //随机映射种子
                dwXorKey = SeedRandMap((ushort)dwXorKey);
                dwXorKey |= ((uint)SeedRandMap((ushort)(dwXorKey >> 16))) << 16;
                dwXorKey ^= g_dwPacketKey;

                m_dwSendXorKey = dwXorKey;
                m_dwRecvXorKey = dwXorKey;
            }

            //加密数据
            ushort wEncrypCount = (ushort)((wEncryptSize + wSnapCount));
            for (ushort i = 0; i < wEncrypCount; i += 4)
            {
                uint oldXorKey = dwXorKey;
                int curIndex = HEAD_INFO_SIZE + i;
                uint temp = BitConverter.ToUInt32(pcbDataBuffer, curIndex) ^ dwXorKey;
                BitConverter.GetBytes(temp).CopyTo(pcbDataBuffer, curIndex);
                ushort wSeed = BitConverter.ToUInt16(pcbDataBuffer, curIndex);
                dwXorKey = SeedRandMap((ushort)wSeed);
                wSeed = BitConverter.ToUInt16(pcbDataBuffer, curIndex + 2);
                dwXorKey |= (uint)(SeedRandMap(wSeed) << 16);
                dwXorKey ^= g_dwPacketKey;
            }

            //插入密钥
            if (m_dwSendPacketCount == 0)
            {
                byte[] srcData = (byte[])pcbDataBuffer.Clone();
                Array.Copy(srcData, HEADER_SIZE, pcbDataBuffer, HEADER_SIZE + sizeof(uint), wDataSize - HEADER_SIZE + wSnapCount);

                wDataSize += sizeof(uint);
                BitConverter.GetBytes(wDataSize).CopyTo(pcbDataBuffer, 2);
                BitConverter.GetBytes(m_dwSendXorKey).CopyTo(pcbDataBuffer, HEADER_SIZE);
            }
            //设置变量
            m_dwSendPacketCount++;
            m_dwSendXorKey = dwXorKey;

            return wDataSize;
        }

        //解密数据
        private int CrevasseBuffer(byte[] pcbDataBuffer, int wDataSize)
        {
            if (wDataSize + 2 * sizeof(uint) > pcbDataBuffer.Length) return 0;
            //调整长度
            ushort wSnapCount = 0;
            // 校验位 将数据缓冲补充到4的整数倍
            if ((wDataSize % sizeof(uint)) != 0)
            {
                wSnapCount = (ushort)(sizeof(uint) - wDataSize % sizeof(uint));
                Array.Clear(pcbDataBuffer, wDataSize, wSnapCount);
            }
#if TEST
            if (m_dwRecvPacketCount == 0)
            {
                byte[] srcData = (byte[])pcbDataBuffer.Clone();
                Array.Copy(srcData, HEADER_SIZE + sizeof(uint), pcbDataBuffer, HEADER_SIZE, wDataSize - HEADER_SIZE + wSnapCount);
                wDataSize -= sizeof(uint);
                BitConverter.GetBytes(wDataSize).CopyTo(pcbDataBuffer, 2);
                Array.Clear(pcbDataBuffer, wDataSize - wSnapCount, 4);
            }
#endif
            //解密数据
            uint dwXorKey = m_dwRecvXorKey;
            ushort wEncrypCount = (ushort)(wDataSize + wSnapCount - HEAD_INFO_SIZE);

            for (int i = 0; i < wEncrypCount; i += 4)
            {
                if (((i + 4) == wEncrypCount) && (wSnapCount > 0))
                {
                    byte[] recvXorKey = BitConverter.GetBytes(m_dwRecvXorKey);
                    Array.Copy(recvXorKey, 4 - wSnapCount, pcbDataBuffer, wDataSize, wSnapCount);
                }

                int curIndex = HEAD_INFO_SIZE + i;
                //计算下一个异或键值
                dwXorKey = SeedRandMap(BitConverter.ToUInt16(pcbDataBuffer, curIndex));
                dwXorKey |= ((uint)SeedRandMap(BitConverter.ToUInt16(pcbDataBuffer, curIndex + 2))) << 16;
                dwXorKey ^= g_dwPacketKey;
                // 还原当前数据
                uint pdwXor = (BitConverter.ToUInt32(pcbDataBuffer, curIndex) ^ m_dwRecvXorKey);
                BitConverter.GetBytes(pdwXor).CopyTo(pcbDataBuffer, curIndex);

                m_dwRecvXorKey = dwXorKey;
            }

            //效验码与字节映射
            byte cbCheckCode = pcbDataBuffer[1];
            for (int i = HEAD_INFO_SIZE; i < wDataSize; i++)
            {
                pcbDataBuffer[i] = MapRecvByte(pcbDataBuffer[i]);
                cbCheckCode += pcbDataBuffer[i];
            }
            m_dwRecvPacketCount++;
            if (cbCheckCode != 0)
            {
                Console.WriteLine("收到服务器消息组合包长度 ：~~~ " + temp);
                throw new Exception("数据包效验码错误" + cbCheckCode);
            }
            return wDataSize;
        }


        Thread _shortConnectCheckThread = null;


        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <param name="buffer"></param>
        void StartShortConnectCheck()
        {
            if (_shortConnectCheckThread == null)
            {
                _shortConnectCheckThread = new Thread(ShortConnectCheck);
                _shortConnectCheckThread.IsBackground = true;
                _shortConnectCheckThread.Start();
            }
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        void ShortConnectCheck()
        {
            while (true)
            {
                try
                {
                    if(_closed)
                    {
                        break;
                    }

                    Thread.Sleep(1000);
                    long now = DateTime.UtcNow.ToFileTimeUtc();
                    long pastMS = now - _lastWriteTime;                    
                    pastMS = pastMS / 10000;
                    //Debug.LogFormat("[time:{0}]距离上次请求的时间差：{1}   Thread:{2}", now, pastMS, Thread.CurrentThread.ManagedThreadId);
                    bool shortConnnectOverTime = false;
                    if (pastMS >= 60*1000 && mIsShortConnect)
                    {                        
                        shortConnnectOverTime = true;
                    }

                    bool isOnline;
                    if (null == mConSock)
                    {
                        isOnline = false;
                    }
                    else
                    {
                        isOnline = !((mConSock.Client.Poll(1000, SelectMode.SelectRead) && (mConSock.Client.Available == 0)) || !mConSock.Client.Connected);
                    }

                    if (shortConnnectOverTime || false == isOnline)
                    {
                        //Debug.Log("短连接失效");
                        CloseCySocket();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogFormat("短连接检测出错：{0}", ex.Message);
                    return;
                }
            }
            _shortConnectCheckThread = null;
        }
        
    }
}