using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using CYNetwork.NetStruct;

namespace CYNetwork
{
    public class CyNetReadBuffer : CyNetBuffer
    {
        public static readonly char[] filtersChars = new char[] { ' ', '\0' };
        public CyNetReadBuffer()
            : base()
        {
            
        }

        public CyNetReadBuffer(int capacity)
            : base(capacity)
        {
        }
        
        public void OffsetCurPos(int offset)
        {
            this.mCurPos += offset;
        }

        /// <summary>
        /// 读取bool数据
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            if (mCurPos >= dataLength)
                throw new System.ArgumentOutOfRangeException();

            return (buffer[mCurPos++] != 0);
        }

        /// <summary>
        /// 读取byte数据
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            if (mCurPos >= dataLength)
                throw new System.ArgumentOutOfRangeException();

            return buffer[mCurPos++];
        }
        /// <summary>
        /// 读取sbyte数据
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSbyte()
        {
            if (mCurPos >= dataLength)
                throw new ArgumentOutOfRangeException();

            return (sbyte)buffer[mCurPos++];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ushort ReadUshort()
        {
            if (mCurPos >= dataLength || mCurPos + 2 > dataLength)
            {
                throw new ArgumentOutOfRangeException();
            }

            ushort vl = BitConverter.ToUInt16(buffer, mCurPos);
            mCurPos += 2;
            return vl;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            if (mCurPos >= dataLength || mCurPos + 2 > dataLength)
                throw new ArgumentOutOfRangeException();

            short vl = BitConverter.ToInt16(buffer, mCurPos);
            mCurPos += 2;
            return vl;
        }

        public char ReadChar()
        {
            if (mCurPos >= dataLength || mCurPos + 2 > dataLength)
                throw new ArgumentOutOfRangeException();

            char vl = BitConverter.ToChar(buffer, mCurPos);
            mCurPos += 2;
            return vl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public uint ReadUint()
        {
            if (mCurPos >= dataLength || mCurPos + 4 > dataLength)
                throw new ArgumentOutOfRangeException();

            uint vl = BitConverter.ToUInt32(buffer, mCurPos);
            mCurPos += 4;
            return vl;
        }
        /// <summary>
        /// 读取int类型数据
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            if (mCurPos >= dataLength || mCurPos + 4 > dataLength)
                throw new ArgumentOutOfRangeException();

            int vl = BitConverter.ToInt32(buffer, mCurPos);
            mCurPos += 4;
            return vl;
        }
        /// <summary>
        /// 读取long数据
        /// </summary>
        /// <returns></returns>
        public long ReadLong()
        {
            if (mCurPos >= dataLength || mCurPos + 8 > dataLength)
                throw new ArgumentOutOfRangeException();

            long vl = BitConverter.ToInt64(buffer, mCurPos);
            mCurPos += 8;
            return vl;
        }

        /// <summary>
        /// 读取ulong数据
        /// </summary>
        /// <returns></returns>
        public ulong ReadUlong()
        {
            if (mCurPos >= dataLength || mCurPos + 8 > dataLength)
                throw new ArgumentOutOfRangeException();

            ulong vl = BitConverter.ToUInt64(buffer, mCurPos);
            mCurPos += 8;
            return vl;
        }

        public float ReadFloat()
        {
            if (mCurPos >= dataLength || mCurPos + 4 > dataLength)
                throw new ArgumentOutOfRangeException();

            float vl = BitConverter.ToSingle(buffer, mCurPos);
            mCurPos += 4;
            return vl;
        }
        public double ReadDouble()
        {
            if (mCurPos >= dataLength || mCurPos + 8 > dataLength)
                throw new ArgumentOutOfRangeException();

            double vl = BitConverter.ToDouble(buffer, mCurPos);
            mCurPos += 8;
            return vl;
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public string ReadString(int len)
        {
            if (len <= 0) return "";
            if (mCurPos >= dataLength || mCurPos + len > dataLength)
                throw new ArgumentOutOfRangeException();

            byte[] uniArr = Encoding.Convert(Encoding.Unicode, PlatformEncode, buffer, mCurPos, len);
            mCurPos += len;
            return PlatformEncode.GetString(uniArr).TrimEnd(filtersChars);         
        }

        /// <summary>
        /// 在字符流中读取包体的剩余长度，
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            return ReadString(dataLength - mCurPos);
        }

        /// <summary>
        /// 读取byte数组
        /// </summary>
        /// <param name="len">读取长度</param>
        /// <returns></returns>
        public byte[] ReadBytes(int len)
        {
            if (len <= 0) return null;
            if (mCurPos >= dataLength || mCurPos + len > dataLength)
                throw new ArgumentOutOfRangeException();

            byte[] arr = new byte[len];
            Array.Copy(buffer, mCurPos, arr, 0, len);
            mCurPos += len;
            return arr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ByteToStruct(byte[] bytes, int offset,Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            // 分配结构体内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            // 将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, offset, structPtr, size);
            // 将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);
            // 释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }

        /// <summary>
        /// 读取日期
        /// </summary>
        /// <returns></returns>
        public DateTime ReadDateTime()
        {
            if (mCurPos >= dataLength || mCurPos + 16 > dataLength)
                throw new ArgumentOutOfRangeException();

            ushort wYear = ReadUshort();
            ushort wMonth = ReadUshort();
            ushort wDayOfWeek = ReadUshort();
            ushort wDay = ReadUshort();
            ushort wHour = ReadUshort();
            ushort wMinute = ReadUshort();
            ushort wSecond = ReadUshort();
            ushort wMilliseconds = ReadUshort();
           
            if (wYear == 0 || wMonth == 0 || wDay == 0) return new DateTime();
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond,wMilliseconds);
        }

        private object ReadEx(Type t)
        {
            if (typeof(bool).Equals(t))
            {
                return ReadBoolean();
            }
            else if (typeof(byte).Equals(t))
            {
                return ReadByte();
            }
            else if (typeof(sbyte).Equals(t))
            {
                return ReadSbyte();
            }
            else if (typeof(char).Equals(t))
            {
                return ReadChar();
            }
            else if (typeof(short).Equals(t))
            {
                return ReadShort();
            }
            else if (typeof(ushort).Equals(t))
            {
                return ReadUshort();
            }
            else if (typeof(uint).Equals(t))
            {
                return ReadUint();
            }
            else if (typeof(int).Equals(t))
            {
                return ReadInt();
            }
            else if (typeof(long).Equals(t))
            {
                return ReadLong();
            }
            else if (typeof(ulong).Equals(t))
            {
                return ReadUlong();
            }
            else if (typeof(double).Equals(t))
            {
                return ReadDouble();
            }
            else if (typeof(float).Equals(t))
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// 读取结构体或类类类型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadT<T>()
            where T : new()
        {
            T obj = new T();
            try
            {
                Type msgType = typeof(T);
                FieldInfo[] myFields = msgType.GetFields();
                for (int i = 0; i < myFields.Length; i++)
                {
                    MessageItem itemAttr = (MessageItem)Attribute.GetCustomAttribute(myFields[i], typeof(MessageItem));
                    if (typeof(String).Equals(myFields[i].FieldType))
                    {
                        bool isDef = Attribute.IsDefined(myFields[i], typeof(MessageItem));
                        if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem特性");

                        if (itemAttr.ItemType != MsgItemType.STRING) throw new Exception("特性类型错误");

                        if (itemAttr.Size <= 0)
                            myFields[i].SetValue(obj, ReadString().TrimEnd(filtersChars));
                        else if (itemAttr.Size > 0)
                            myFields[i].SetValue(obj, ReadString(itemAttr.Size).TrimEnd(filtersChars));
                    }
                    else if (typeof(Dictionary<ushort, object>).Equals(myFields[i].FieldType))
                    {
                        bool isDef = Attribute.IsDefined(myFields[i], typeof(MessageItem));
                        if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem特性");
                        if (itemAttr.ItemType != MsgItemType.DICTIONARY) throw new Exception("特性类型错误");

                        Dictionary<ushort, object> dict = myFields[i].GetValue(obj) as Dictionary<ushort, object>;
                        if (null == dict) dict = new Dictionary<ushort, object>();

                        while(this.CurrentPos < this.Length)
                        {                           
                            ushort length = ReadUshort();
                            ushort type = ReadUshort();
                            if (30 == type)
                            {
                                byte memberOrder = ReadByte();
                                DateTime dt = ReadDateTime();
                                dict.Add(type, new KeyValuePair<byte, DateTime>(memberOrder, dt));
                            }
                            else dict.Add(type, ReadString(length));

                            if (dict.Count >= itemAttr.MaxItemCount) break;
                        }
                        myFields[i].SetValue(obj, dict);
                    }
                    else if (myFields[i].FieldType.IsArray)
                    {
                        bool isDef = Attribute.IsDefined(myFields[i], typeof(MessageItem));
                        if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem Dimension ,ArrayType特性");

                        Array arr = myFields[i].GetValue(obj) as Array;

                        if (arr == null) arr = Array.CreateInstance(itemAttr.ArrayType, itemAttr.Dimension);
                        if (arr.Rank == 1)
                        {
                            for (int k = 0; k < itemAttr.Dimension[0]; k++)
                            {
                                arr.SetValue(ReadEx(itemAttr.ArrayType), k);
                            }
                        }
                        else if (2 <= arr.Rank)
                        {
                            for(int k = 0;k < itemAttr.Dimension[0];k++)
                            {
                                for(int j = 0;j < itemAttr.Dimension[1];j++)
                                {
                                    arr.SetValue(ReadEx(itemAttr.ArrayType), k, j);
                                }
                            }
                        }
                        myFields[i].SetValue(obj, arr);
                    }
                    else if (typeof(DateTime).Equals(myFields[i].FieldType))
                        myFields[i].SetValue(obj, ReadDateTime());
                    else
                        myFields[i].SetValue(obj, ReadEx(myFields[i].FieldType));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return obj;
        }

        public bool ReadMsg(object obj)
        {
            try
            {
                Type msgType = obj.GetType();          
                FieldInfo[] myFields = msgType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < myFields.Length; i++)
                {
                    MessageItem itemAttr = (MessageItem)Attribute.GetCustomAttribute(myFields[i], typeof(MessageItem));
                    // 字符串
                    if (typeof(String).Equals(myFields[i].FieldType))
                    {
                        bool isDef = Attribute.IsDefined(myFields[i], typeof(MessageItem));
                        if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem特性");
                        if (itemAttr.ItemType != MsgItemType.STRING) throw new Exception("特性类型错误 应为MsgItemType.STRING");

                        if (itemAttr.Size <= 0)
                        {
                            myFields[i].SetValue(obj, ReadString().TrimEnd(filtersChars));
                        }
                        else if (itemAttr.Size > 0)
                        {
                            myFields[i].SetValue(obj,ReadString(itemAttr.Size).TrimEnd(filtersChars));
                        }
                    }
                    // 处理字典
                    else if (typeof(Dictionary<ushort, object>).Equals(myFields[i].FieldType))
                    {
                        bool isDef = Attribute.IsDefined(myFields[i], typeof(MessageItem));
                        if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem特性");
                        if (itemAttr.ItemType != MsgItemType.DICTIONARY) throw new Exception("特性类型错误 应为MsgItemType.DICTIONARY");

                        Dictionary<ushort, object> dict = myFields[i].GetValue(obj) as Dictionary<ushort, object>;
                        if (null == dict) new Dictionary<ushort, object>();

                        while (this.CurrentPos < this.Length)
                        {
                            if (dict.Count > itemAttr.MaxItemCount) break;
                            
                            ushort length = ReadUshort();
                            ushort type = ReadUshort();
                            if (30 == type) dict.Add(type, ReadDateTime());
                            else dict.Add(type, ReadString(length));
                        }
                        myFields[i].SetValue(obj, dict);

                    }
                    else if (myFields[i].FieldType.IsArray)
                    {
                        bool isDef = Attribute.IsDefined(myFields[i], typeof(MessageItem));
                        if (!isDef) throw new Exception("无法识别的参数 请加上MessageItem特性  应为MsgItemType.ARRAY");

                       // MessageItem itemAttr = (MessageItem)Attribute.GetCustomAttribute(myFields[i], typeof(MessageItem));

                        Array arr = myFields[i].GetValue(obj) as Array;

                        if (arr == null) arr = Array.CreateInstance(itemAttr.ArrayType, itemAttr.Dimension);

                        if (arr.Rank == 1)
                        {
                            for (int k = 0; k < itemAttr.Dimension[0]; k++)
                            {
                                arr.SetValue(ReadEx(itemAttr.ArrayType), k);
                            }
                        }
                        else if (2 <= arr.Rank)
                        {
                            for (int k = 0; k < itemAttr.Dimension[0]; k++)
                            {
                                for (int j = 0; j < itemAttr.Dimension[1]; j++)
                                {
                                    arr.SetValue(ReadEx(itemAttr.ArrayType), k, j);
                                }
                            }
                        }
                        myFields[i].SetValue(obj, arr);
                    }
                    else if (typeof(DateTime).Equals(myFields[i].FieldType))
                    {
                        myFields[i].SetValue(obj, ReadDateTime());
                    }
                    else
                    {
                        myFields[i].SetValue(obj, ReadEx(myFields[i].FieldType));
                    }
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

    }
}
