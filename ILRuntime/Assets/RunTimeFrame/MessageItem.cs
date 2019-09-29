using System;
using System.Collections.Generic;
using System.Text;

namespace RunTimeFrame
{

    /// <summary>
    /// 字段类型
    /// </summary>
    public enum MsgItemType
    {
        /// <summary>
        /// 
        /// </summary>
        BASE = 0,
        /// <summary>
        /// 
        /// </summary>
        STRUCT = 1,
        /// <summary>
        /// 
        /// </summary>
        ARRAY = 2,
        /// <summary>
        /// 
        /// </summary>
        STRING = 3,
        /// <summary>
        /// 
        /// </summary>
        DICTIONARY = 4,
    }
    /// <summary>
    /// 
    /// </summary>
    public class MessageItem : Attribute
    {

        /// <summary>
        /// 类型
        /// </summary>
        public MsgItemType ItemType { get; set; }
        #region 字符串使用
        /// <summary>
        /// 长度 
        /// 大于0 指定长度
        /// 等于0 读取剩余长度
        /// 小于0 长度内置
        /// </summary>
        public int Size { get; set; }
        #endregion

        #region  数组使用
        public Type ArrayType { get; set; }

        /// <summary>
        /// 数组维度长度
        /// </summary>
        public int[] Dimension { get; set; }

        #endregion

        #region
        /// <summary>
        /// 最大项数
        /// </summary>
        public int MaxItemCount { get; set; }

        #endregion


        public MessageItem()
            : this(MsgItemType.BASE, 0)
        {

        }
        public MessageItem(MsgItemType it)
           : this(it, 0)
        {

        }

        public MessageItem(MsgItemType it, int size)
        {
            ItemType = it;
            Size = size;
        }

    }
}
