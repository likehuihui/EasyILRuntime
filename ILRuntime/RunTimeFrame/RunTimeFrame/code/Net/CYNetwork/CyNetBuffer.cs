using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using CYNetwork.NetStruct;

namespace CYNetwork
{

    public class CyNetBuffer
    {
        public const int mDefaultSize = 16384;
        protected byte[] buffer;
        protected int mCurPos = 0;
        protected int dataLength = 0;

        protected static readonly Encoding PlatformEncode = Encoding.UTF8;
        public CyNetBuffer()
            : this(mDefaultSize)
        {
          
        }

        public CyNetBuffer(int capacity)
        {
            mCurPos = 0;
            buffer = new byte[capacity];
        }

        /// <summary>
        /// 缓存中的数据长度
        /// </summary>
        public int Length { set { dataLength = value; } get { return dataLength; } }

        /// <summary>
        /// 获取当前存储位置
        /// </summary>
        virtual public int CurrentPos
        {
            get { return mCurPos; }
        }

        //public void ResetCapacity(int capacity)
        //{
        //    if (capacity <= buffer.Length) return;

        //    lock (buffer)
        //    {
        //        byte[] oldBuffer = buffer;
        //        buffer = new byte[capacity];
        //        oldBuffer.CopyTo(buffer, 0);
        //    }
        //}

        /// <summary>
        /// 缓存容量
        /// </summary>
        public int Capacity
        {
            get
            {
                if (buffer == null) return 0;
                return buffer.Length;
            }
            set
            {
                if(value > 0 && buffer == null)
                {
                    buffer = new byte[value];
                }
                else if(value > buffer.Length)
                {
                    byte[] oldBuffer = buffer;
                    buffer = new byte[value];
                    Array.Copy(oldBuffer, buffer,dataLength);
                }
            }

        }
        /// <summary>
        /// 缓存数据
        /// </summary>
        public byte[] NetBuffer
        {
            get
            {
                return buffer;
            }
        }

    }



    /// <summary>
    /// 写入缓冲。
    /// 将数据加入到区，再转发到网络中
    /// </summary>
    public class CyNetWriteBuffer : CyNetBuffer
    {
        public CyNetWriteBuffer()
            :base()
        {
        }

        public CyNetWriteBuffer(int size)
            : base(size)
        {
        }

        public void Write(bool vl)
        {
            if (mCurPos >= buffer.Length || mCurPos + 1 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            buffer[mCurPos++] = (byte)(vl ? 1 :0);
            dataLength = mCurPos;
        }

        /// <summary>
        ///  写入数据
        /// </summary>
        /// <param name="vl"></param>
        public void Write(byte vl)
        {
            if(mCurPos>= buffer.Length || mCurPos + 1 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            buffer[mCurPos++] = vl;
            dataLength = mCurPos;
        }
        /// <summary>
        /// 写入sbyte数据
        /// </summary>
        /// <param name="vl"></param>
        public void Write(sbyte vl)
        {
            if(mCurPos>= buffer.Length || mCurPos + 1 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            buffer[mCurPos++] =(byte) vl;
            dataLength = mCurPos;
        }
        /// <summary>
        /// 写入short数据
        /// </summary>
        /// <param name="vl"></param>
        public void Write(short vl)
        {
            if(mCurPos>= buffer.Length || mCurPos + 2 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            BitConverter.GetBytes(vl).CopyTo(buffer, mCurPos);
            mCurPos += 2;
            dataLength = mCurPos;
        }
        /// <summary>
        /// 写入ushyort数据
        /// </summary>
        /// <param name="vl"></param>
        public void Write(ushort vl)
        {
            if(mCurPos>= buffer.Length || mCurPos + 2 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            BitConverter.GetBytes(vl).CopyTo(buffer, mCurPos);
            mCurPos += 2;
            dataLength = mCurPos;
        }

        /// <summary>
        /// 写入Char数组
        /// </summary>
        /// <param name="chars"></param>
        public void WriteChars(char[] chars)
        {
            if (null == chars || chars.Length <= 0) return;

            if (mCurPos >= buffer.Length || mCurPos + chars.Length > buffer.Length)
                throw new ArgumentOutOfRangeException();

            Byte[] ba = Encoding.Default.GetBytes(chars);
            ba.CopyTo(buffer, mCurPos);
            mCurPos += chars.Length;
            dataLength = mCurPos;
        }

        /// <summary>
        /// 写入int数据
        /// </summary>
        /// <param name="vl"></param>
        public void Write(int vl)
        {
            if(mCurPos>= buffer.Length || mCurPos + 4 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            BitConverter.GetBytes(vl).CopyTo(buffer, mCurPos);
            mCurPos += 4;
            dataLength = mCurPos;
        }
        /// <summary>
        /// 写入uint数据
        /// </summary>
        /// <param name="vl"></param>
        public void Write(uint vl)
        {
            if(mCurPos>= buffer.Length || mCurPos + 4 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            BitConverter.GetBytes(vl).CopyTo(buffer, mCurPos);
            mCurPos += 4;
            dataLength = mCurPos;
        }

        /// <summary>
        /// 写入long
        /// </summary>
        /// <param name="vl"></param>
        public void Write(long vl)
        {
            if(mCurPos>= buffer.Length || mCurPos + 8 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            BitConverter.GetBytes(vl).CopyTo(buffer, mCurPos);
            mCurPos += 8;
            dataLength = mCurPos;
        }
        /// <summary>
        /// 写一个ulong类型
        /// </summary>
        /// <param name="vl"></param>
        public void Write(ulong vl)
        {
            if(mCurPos>= buffer.Length || mCurPos + 8 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            BitConverter.GetBytes(vl).CopyTo(buffer, mCurPos);
            mCurPos += 8;
            dataLength = mCurPos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vl"></param>
        public void Write(double vl)
        {
            if (mCurPos >= buffer.Length || mCurPos + 8 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            BitConverter.GetBytes(vl).CopyTo(buffer, mCurPos);
            mCurPos += 8;
            dataLength = mCurPos;
        }
        public void Write(float vl)
        {
            if (mCurPos >= buffer.Length || mCurPos + 4 > buffer.Length)
                throw new ArgumentOutOfRangeException();

            BitConverter.GetBytes(vl).CopyTo(buffer, mCurPos);
            mCurPos += 4;
            dataLength = mCurPos;
        }

        


        /// <summary>
        ///  写入字符串 (没指定长度，慎用)
        /// </summary>
        /// <param name="vl"></param>
        public void Write(string vl)
        {
            
            if (vl == null || vl.Length <= 0) return;

            if(null == vl || mCurPos>= buffer.Length)
                throw new ArgumentOutOfRangeException();

            byte[] str = Encoding.Convert(PlatformEncode, Encoding.Unicode, PlatformEncode.GetBytes(vl));

            int strlen = (str.Length % 2) + str.Length;
           
            if ((mCurPos + strlen) > buffer.Length)
               throw new ArgumentOutOfRangeException();

            str.CopyTo(buffer, mCurPos);
            mCurPos += strlen;
            dataLength = mCurPos;
        }
        /// <summary>
        /// 写入一个指定长度的字符串
        /// 如果字符串的长度大于len 字符串将被截取len长度
        /// 如果字符串长度小于len-1 将以0补齐指定长度
        /// 如果写入的长度大于缓冲区容量将抛出异常
        /// </summary>
        /// <param name="vl"></param>
        /// <param name="len"></param>
        public void Write(string vl,int len)
        {
           if (len <= 0) return;
           if(mCurPos>= buffer.Length)
               throw new ArgumentOutOfRangeException();

            if(vl != null && vl.Length> 0)
            {
                byte[] str = Encoding.Convert(PlatformEncode, Encoding.Unicode, PlatformEncode.GetBytes(vl));
                if (mCurPos + str.Length > buffer.Length)
                    throw new ArgumentOutOfRangeException();

                System.Buffer.BlockCopy(str, 0, buffer, mCurPos, str.Length >= len ? len - 1 : str.Length);
                mCurPos += len;
            }
            else
            {
                mCurPos += len;
            }
            dataLength = mCurPos;
        }

        /// <summary>
        /// 写入一个数组，(没有长度 慎用）
        /// </summary>
        /// <param name="vl"></param>
        public void Write(byte[] vl)
        {
            if(null == vl || vl.Length <= 0) return;

            if(mCurPos>= buffer.Length || mCurPos + vl.Length > buffer.Length)
                throw new ArgumentOutOfRangeException();

            vl.CopyTo(buffer, mCurPos);
            mCurPos += vl.Length;
            dataLength = mCurPos;
        }

        /**
         * 将数组内容写入缓冲
         * @param vl    vl可为空，
         * @param len   
         */ 
        public void Write(byte[] vl,int len)
        {
            if(len <= 0) return;
            if(mCurPos >=  buffer.Length || mCurPos + len > buffer.Length )
                throw new ArgumentOutOfRangeException();

            if(vl != null)
            {
                int length = (vl.Length > len ? len : vl.Length);
                System.Buffer.BlockCopy(vl, 0, buffer, mCurPos, length);
            }
                
            mCurPos += len;
            dataLength = mCurPos;
        }

        /// <summary>
        /// 将结构体转换为字节数组
        /// </summary>
        /// <param name="structObj"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(object structObj, int size)
        {
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

        public void Write(DateTime  dt)
        {
            if (mCurPos >= buffer.Length) throw new ArgumentOutOfRangeException();
            if ((mCurPos + 16) >= buffer.Length) throw new ArgumentOutOfRangeException();

            Write((ushort)dt.Year);
            Write((ushort)dt.Month);
            Write((ushort)dt.DayOfWeek);
            Write((ushort)dt.Day);
            Write((ushort)dt.Hour);
            Write((ushort)dt.Minute);
            Write((ushort)dt.Second);
            Write((ushort)dt.Millisecond);
        }

        

        private void WriteEx(object obj)
        {
            if (typeof(bool).Equals(obj.GetType()))
            {              
                Write((bool)obj);
            }
            else if (typeof(byte).Equals(obj.GetType()))
            {
                Write((byte)obj);
            }
            else if (typeof(sbyte).Equals(obj.GetType()))
            {
                Write((sbyte)obj);
            }
            else if (typeof(char).Equals(obj.GetType()))
            {
                Write((char)obj);
            }
            else if (typeof(short).Equals(obj.GetType()))
            {
                Write((short)obj);
            }
            else if (typeof(ushort).Equals(obj.GetType()))
            {
                Write((ushort)obj);
            }
            else if (typeof(uint).Equals(obj.GetType()))
            {
               Write((uint)obj);
            }
            else if (typeof(int).Equals(obj.GetType()))
            {
                Write((int)obj);
            }
            else if (typeof(long).Equals(obj.GetType()))
            {
                Write((long)obj);
            }
            else if (typeof(ulong).Equals(obj.GetType()))
            {
                Write((ulong)obj);
            }
            else if (typeof(double).Equals(obj.GetType()))
            {
                Write((double)obj);
            }
            else if (typeof(float).Equals(obj.GetType()))
            {
                Write((float)obj);
            }
            else
            {
                Write(ref obj);
            }
        }


        public void Write<T>(ref T obj)
        {
            if (null == obj) throw new NullReferenceException();

            Type msgType = obj.GetType();
            FieldInfo[] myFields = msgType.GetFields(BindingFlags.Public | BindingFlags.Instance);
           
            foreach (var item in myFields)
            {
                if (null == item) continue;
                 MessageItem itemAttr = (MessageItem)Attribute.GetCustomAttribute(item, typeof(MessageItem));
                if (typeof(String).Equals(item.FieldType))
                {
                    bool isDef = Attribute.IsDefined(item, typeof(MessageItem));
                    if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem特性");

                    if (itemAttr.ItemType != MsgItemType.STRING) throw new Exception("特性类型错误");

                    if (itemAttr.Size <= 0)
                        Write(item.GetValue(obj) as string);
                    else if (itemAttr.Size > 0)
                            Write(item.GetValue(obj) as string, itemAttr.Size);
                }
                // 处理字典
                else if (typeof(Dictionary<ushort, object>).Equals(item.FieldType))
                {
                    Dictionary<ushort, object> dictItem = item.GetValue(obj) as Dictionary<ushort, object>;

                    if (null == dictItem) continue;
                    foreach (var vl in dictItem)
                    {
                        if (vl.Value == null) continue;
                        if (30 == vl.Key)
                        {
                            Write((ushort)16);
                            Write((ushort)vl.Key);
                            Write((DateTime)vl.Value);
                        }
                        else
                        {
                            if (vl.Value == null || (vl.Value as string).Length == 0)
                            {
                                Write((ushort)0);
                                Write((ushort)vl.Key);
                            }
                            else
                            {
                                byte[] strArr = Encoding.Convert(PlatformEncode, Encoding.Unicode, PlatformEncode.GetBytes(vl.Value as string));
                                Write((ushort)(strArr.Length + 2));
                                Write(vl.Key);
                                Write(strArr, strArr.Length + 2);
                            }                       
                        }
                    }
                }
                else if (item.FieldType.IsArray)
                {
                    bool isDef = Attribute.IsDefined(item, typeof(MessageItem));
                    if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem特性");

                    if (null == itemAttr) throw new Exception("请指定类型参数");
                    Array arr = item.GetValue(obj) as Array;
                    if (arr.Rank == 1)
                    {
                        for (int i = 0; i < arr.GetLength(0); i++)
                            WriteEx(arr.GetValue(i));
                    }
                    else if (arr.Rank == 2)
                    {
                        for (int k = 0; k < itemAttr.Dimension[0]; k++)
                        {
                            for (int j = 0; j < itemAttr.Dimension[j]; j++)
                            {
                                WriteEx(arr.GetValue(k, j));
                            }
                        }
                    }
                }
                else if (typeof(DateTime).Equals(item.FieldType))
                {
                    Write((DateTime)item.GetValue(obj));
                }
                else if (item.FieldType.IsValueType) // 值类型
                {
                    WriteEx(item.GetValue(obj));
                }
                else
                {
                    throw new Exception("未知类型");
                }
            }
        }

        public void Write(ref object obj)
        {
            if (null == obj) throw new NullReferenceException();

            Type msgType = obj.GetType();
            FieldInfo[] myFields = msgType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in myFields)
            {
                if (null == item) continue;
                MessageItem itemAttr = (MessageItem)Attribute.GetCustomAttribute(item, typeof(MessageItem));
                if (typeof(String).Equals(item.FieldType))
                {
                    bool isDef = Attribute.IsDefined(item,typeof(MessageItem));
                    if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem特性");
                
                    if (itemAttr.ItemType != MsgItemType.STRING) throw new Exception("特性类型错误");

                    if (itemAttr.Size <= 0)
                        Write(item.GetValue(obj) as string);
                    else 
                    if (itemAttr.Size > 0)
                        Write(item.GetValue(obj) as string, itemAttr.Size);
                }
                // 处理字典
                else if ( typeof(Dictionary<ushort, object>).Equals(item.FieldType)) 
                {
                    Dictionary<ushort, object> dictItem = item.GetValue(obj) as Dictionary<ushort, object>;

                    if (null == dictItem) continue;
     
                    foreach(var vl in dictItem)
                    {
                        if (vl.Value == null) continue;

                        if(30 == vl.Key)
                        {
                            Write((ushort)16);
                            Write((ushort)vl.Key);
                            Write((DateTime)vl.Value);
                        }
                        else
                        {
                            if ((vl.Value as string).Length == 0)
                            {

                                Write((ushort)0);
                                Write((ushort)vl.Key);
                            }
                            else
                            {       
                                if (vl.Value == null) continue;
                                byte[] strArr = Encoding.Convert(PlatformEncode, Encoding.Unicode, PlatformEncode.GetBytes(vl.Value as string));
                                Write((ushort)strArr.Length);
                                Write(strArr, strArr.Length);
                            }  
                        }
                    }
                }
                else if(item.FieldType.IsArray)
                {
                    bool isDef = Attribute.IsDefined(item, typeof(MessageItem));
                    if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem特性");

                    Array arr = item.GetValue(obj) as Array;
                    if (arr.Rank == 1)
                    {
                        for (int i = 0; i < arr.GetLength(0); i++)
                            WriteEx(arr.GetValue(i));
                    }
                    else if (arr.Rank == 2)
                    {
                        for (int k = 0; k < itemAttr.Dimension[0]; k++)
                        {
                            for (int j = 0; j < itemAttr.Dimension[j]; j++)
                            {
                                WriteEx(arr.GetValue(k,j));
                            }
                        }
                    }
                }
                else if(typeof(DateTime).Equals(item.FieldType))
                {
                    Write((DateTime)item.GetValue(obj));
                }
                else if (item.FieldType.IsValueType) // 值类型
                {
                    WriteEx(item.GetValue(obj));
                }
                else
                {
                    throw new Exception("未知类型");
                }
            }
        }



    }
}
