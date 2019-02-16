using System;
using System.Collections.Generic;
using System.Text;

namespace CYNetwork.NetStruct
{
    public class MessageBody : Attribute
    {
        public ushort MainCmd { set; get; }

        public ushort SubCmd { set; get; }

        public int Size { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public MessageBody()
            : this(ushort.MaxValue, ushort.MaxValue,-1)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainCmd"></param>
        /// <param name="subCmd"></param>
        /// <param name="size"></param>
        public MessageBody(ushort mainCmd,ushort subCmd,int size)
        {
            MainCmd = mainCmd;
            SubCmd = subCmd;
            Size = size;
        }
    }
}
