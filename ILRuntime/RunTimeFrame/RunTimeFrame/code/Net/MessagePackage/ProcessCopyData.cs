

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using MessagePackage;
using CYNetwork;

namespace ProcessWM_CopyData
{
    public class CProcessWM_CopyData
    {
        #region
        public IntPtr m_taghWnd;
        public IntPtr m_hWnd;
        /// <summary>  
        /// 发送windows消息方便user32.dll中的SendMessage函数使用  
        /// </summary>  
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        //user32.dll中的SendMessage  
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, ref COPYDATASTRUCT lParam);
        //user32.dll中的获得窗体句柄  
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);
        //宏定义   
        private const ushort IPC_VER = 1;
        private const int IDT_ASYNCHRONISM = 0x0201;
        private const uint WM_COPYDATA = 0x004A;
        private const int IPC_BUFFER = 10240;//最大缓冲长度  
        //查找的窗体  
        private IntPtr hWndPalaz;
        //数据包头配合使用  
        public unsafe struct IPC_Head
        {
            public ushort wVersion;
            public ushort wPacketSize;
            public ushort wMainCmdID;
            public ushort wSubCmdID;

        }
        public unsafe struct IPC_Buffer
        {
            public IPC_Head Head;  //IPC_Head结构  
            public fixed byte cbBuffer[IPC_BUFFER]; //指针        存放json数据 利用byte[]接收存放   
        }
        #endregion


        /// <summary>  
        /// 初始化窗口句柄
        /// </summary>  
        public bool InitSendHWND(string strClassName, string strWindowName,string strTagClassName, string strTagWindowName)
        {

            IntPtr hMyWndPalaz = FindWindow(null, strWindowName);//就是窗体的的标题  

            if (hMyWndPalaz != null)
            {
                //获得游戏本身句柄   
                m_hWnd = FindWindow(strClassName, null);
            }

            IntPtr hWndPalaz = FindWindow(null, strTagWindowName);//就是窗体的的标题  

            if (hWndPalaz != null)
            {
                //获得目标句柄   
                m_taghWnd = FindWindow(strTagClassName, null);
            }

            return (m_taghWnd != null) && (m_hWnd != null);
        }

        //////////////////////////////发送
        /// <summary>  
        /// SendMessage发送  
        /// </summary>  
        /// <param name="hWndServer">指针</param>  
        /// <param name="wMainCmdID">主命令</param>  
        /// <param name="wSubCmdID">次命令</param>  
        /// <param name="pData">json转换的指针</param>  
        /// <param name="wDataSize">数据大小</param>  
        /// <returns></returns>  
        public unsafe bool SendData(ushort wMainCmdID, ushort wSubCmdID, IntPtr pData, ushort wDataSize)
        {
            //给IPCBuffer结构赋值  
            IPC_Buffer IPCBuffer;
            IPCBuffer.Head.wVersion = IPC_VER;
            IPCBuffer.Head.wSubCmdID = wSubCmdID;
            IPCBuffer.Head.wMainCmdID = wMainCmdID;
            IPCBuffer.Head.wPacketSize = (ushort)Marshal.SizeOf(typeof(IPC_Head));


            //内存操作  
            if (pData != null)
            {
                //效验长度  
                if (wDataSize > 16384) return false;
                //拷贝数据  
                IPCBuffer.Head.wPacketSize += wDataSize;


                byte[] bytes = new byte[IPC_BUFFER];
                Marshal.Copy(pData, bytes, 0, wDataSize);


                for (int i = 0; i < IPC_BUFFER; i++)
                {
                    IPCBuffer.cbBuffer[i] = bytes[i];
                }
            }


            //发送数据  
            COPYDATASTRUCT CopyDataStruct;
            IPC_Buffer* pPCBuffer = &IPCBuffer;
            CopyDataStruct.lpData = (IntPtr)pPCBuffer;
            CopyDataStruct.dwData = (IntPtr)IDT_ASYNCHRONISM;
            CopyDataStruct.cbData = IPCBuffer.Head.wPacketSize;
            SendMessage(m_hWnd, WM_COPYDATA, (int)m_taghWnd, ref CopyDataStruct);

            return true;
        }

        public unsafe bool SendData(ushort wMainCmdID, ushort wSubCmdID, CyNetWriteBuffer writeBuffer)
        {
            //给IPCBuffer结构赋值  
            IPC_Buffer IPCBuffer;
            IPCBuffer.Head.wVersion = IPC_VER;
            IPCBuffer.Head.wSubCmdID = wSubCmdID;
            IPCBuffer.Head.wMainCmdID = wMainCmdID;
            IPCBuffer.Head.wPacketSize = (ushort)Marshal.SizeOf(typeof(IPC_Head));


            //内存操作  
            if (writeBuffer.Length > 0)
            {
                //效验长度  
                if (writeBuffer.Length > 16384) return false;
                //拷贝数据  
                IPCBuffer.Head.wPacketSize += (ushort)writeBuffer.Length;


                byte[] bytes = new byte[IPC_BUFFER];
                Array.Copy(writeBuffer.NetBuffer, 0, bytes, 0, writeBuffer.Length);


                for (int i = 0; i < IPC_BUFFER; i++)
                {
                    IPCBuffer.cbBuffer[i] = bytes[i];
                }
            }


            //发送数据  
            COPYDATASTRUCT CopyDataStruct;
            IPC_Buffer* pPCBuffer = &IPCBuffer;
            CopyDataStruct.lpData = (IntPtr)pPCBuffer;
            CopyDataStruct.dwData = (IntPtr)IDT_ASYNCHRONISM;
            CopyDataStruct.cbData = IPCBuffer.Head.wPacketSize;
            SendMessage(m_hWnd, WM_COPYDATA, (int)m_taghWnd, ref CopyDataStruct);

            return true;
        }

        //////////////////////////////接收
        //钩子接收消息的结构  
        public struct CWPSTRUCT
        {
            public int lparam;
            public int wparam;
            public uint message;
            public IntPtr hwnd;
        }
        //建立钩子  
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, uint dwThreadId);


        //移除钩子  
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);


        //把信息传递到下一个监听  
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, int lParam);
        //回调委托  
        private delegate int HookProc(int nCode, int wParam, int lParam);
        //钩子  
        int idHook = 0;
        //是否安装了钩子  
        bool isHook = false;
        GCHandle gc;
        private const int WH_CALLWNDPROC = 4;  //钩子类型 全局钩子  

        public void Start()
        {
            //安装钩子  
            HookLoad();
        }


        void AnDestroy()
        {
            //关闭钩子  
            HookClosing();
        }
        private void HookLoad()
        {
            //Debug.Log("开始运行");  
            //安装钩子  
            {
                //钩子委托  
                HookProc lpfn = new HookProc(Hook);
                //关联进程的主模块  
                IntPtr hInstance = m_hWnd;// GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);  
                idHook = SetWindowsHookEx(WH_CALLWNDPROC, lpfn, hInstance, (uint)AppDomain.GetCurrentThreadId());
                if (idHook > 0)
                {
                    //Debug.Log("钩子[" + idHook + "]安装成功");  
                    isHook = true;
                    //保持活动 避免 回调过程 被垃圾回收  
                    gc = GCHandle.Alloc(lpfn);
                }
                else
                {
                    //Debug.Log("钩子安装失败");  
                    isHook = false;
                    UnhookWindowsHookEx(idHook);
                }
            }
        }


        //卸载钩子  
        private void HookClosing()
        {
            if (isHook)
            {
                UnhookWindowsHookEx(idHook);
            }
        }


        private bool _bCallNext;
        public bool CallNextProc
        {
            get { return _bCallNext; }
            set { _bCallNext = value; }
        }


        //钩子回调  
        private unsafe int Hook(int nCode, int wParam, int lParam)
        {
            try
            {
                IntPtr p = new IntPtr(lParam);
                CWPSTRUCT m = (CWPSTRUCT)Marshal.PtrToStructure(p, typeof(CWPSTRUCT));

                if (m.message == 74)
                {
                    COPYDATASTRUCT entries = (COPYDATASTRUCT)Marshal.PtrToStructure((IntPtr)m.lparam, typeof(COPYDATASTRUCT));
                    IPC_Buffer data = (IPC_Buffer)Marshal.PtrToStructure((IntPtr)entries.lpData, typeof(IPC_Buffer));

                    CyNetReadBuffer readbuffer = new CyNetReadBuffer();
                    for (int i = 0; i < data.Head.wPacketSize; i++)
                        readbuffer.NetBuffer[i] = data.cbBuffer[i];

                    readbuffer.Length = data.Head.wPacketSize;
                    //数据回调
                    MessageMgr.CurMsgMgr.GameMessageHelper.HandleReadNetData(data.Head.wMainCmdID, data.Head.wSubCmdID,readbuffer);
                    //Debug.Log("数据：" + str);  
                }
                if (CallNextProc)
                {
                    return CallNextHookEx(idHook, nCode, wParam, lParam);
                }
                else
                {
                    //return 1;  
                    return CallNextHookEx(idHook, nCode, wParam, lParam);
                }
            }
            catch (Exception ex)
            {
                //Debug.Log(ex.Message);  
                return 0;
            }

        }  

    }
}
