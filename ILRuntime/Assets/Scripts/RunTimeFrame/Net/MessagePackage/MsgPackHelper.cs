using System;
using System.Collections.Generic;
using System.Text;
using CYNetwork;
using MessagePackage.Struct;

namespace MessagePackage
{
    public class CSendPacketHelper
    {
        private CyNetWriteBuffer mBuffer;

        public CSendPacketHelper(CyNetWriteBuffer writeBuffer)
        {
            mBuffer = writeBuffer;
        }

        //public KeyValuePair<int,string> ReadString()
        //{
        //    KeyValuePair<int, string> pairValue = new KeyValuePair<int, string>();
        //    ushort size = mBuffer
        //    return pairValue;
        //}

        public void WriteValue(string str, int type)
        {
            if (str.Length <= 0) return;
            byte[] strArr = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, Encoding.UTF8.GetBytes(str));

            mBuffer.Write((ushort)strArr.Length);
            mBuffer.Write((ushort)type);
            mBuffer.Write(strArr, strArr.Length);
        }
    }

    public class CReadPacketHelper
    {
        private CyNetReadBuffer mBuffer;
        public CReadPacketHelper(CyNetReadBuffer readBuffer)
        {
            mBuffer = readBuffer;
        }

        /// <summary>
        /// 读取特定字符串
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<int,object> ReadValue()
        {
            int len = mBuffer.ReadUshort();
            int type = mBuffer.ReadUshort();

            string str = mBuffer.ReadString(len);
            return new KeyValuePair<int, object>(type,  str);
        }

        /// <summary>
        /// 读取日期
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<byte,DateTime> ReadDateTime()
        {
            int len = mBuffer.ReadUshort();
            int type = mBuffer.ReadUshort();
            if(2 == type)
            {
                byte memberOrder = mBuffer.ReadByte();
                return new KeyValuePair<byte,DateTime>(memberOrder,mBuffer.ReadDateTime());
            }
            else
                mBuffer.OffsetCurPos(mBuffer.CurrentPos - 2);

            return new KeyValuePair<byte,DateTime>( 0,new DateTime());
        }

        public bool ReadMemberInfo(ref tagDTP_GP_MemberInfo memInfo)
        {
            return true;
        }
    }
}
