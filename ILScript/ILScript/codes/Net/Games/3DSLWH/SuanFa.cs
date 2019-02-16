using System;
using System.Collections.Generic;
using System.Text;


namespace SLWH
{
    /// <summary>
    /// 森林舞会逻辑接口
    /// </summary>
    public partial class SLWH_
    {
        /// <summary>
        /// 整副牌数目
        /// </summary>
        public const byte FULL_COUNT = 54;


        #region 牌型枚举
        /// <summary>
        /// 牌型
        /// </summary>
        public enum emCardType
        {
            /// <summary>
            /// 错误类型
            /// </summary>
            CT_ERROR = 0,
            /// <summary>
            /// 单牌类型
            /// </summary>
            CT_SINGLE = 1,
            /// <summary>
            /// 对子
            /// </summary>
            CT_DOUBLE = 2,
            /// <summary>
            /// 三条
            /// </summary>
            CT_THREE = 3,
            /// <summary>
            /// 单顺
            /// </summary>
            CT_SINGLE_LINE = 4,
            /// <summary>
            /// 双顺
            /// </summary>
            CT_DOUBLE_LINE = 5,
            /// <summary>
            /// 三顺
            /// </summary>
            CT_THREE_LINE = 6,
            /// <summary>
            /// 飞机单翅
            /// </summary>
            CT_THREE_TAKE_ONE = 7,
            /// <summary>
            /// 飞机双翅
            /// </summary>
            CT_THREE_TAKE_TWO = 8,
            /// <summary>
            /// 4带两单
            /// </summary>
            CT_FOUR_TAKE_ONE = 9,
            /// <summary>
            /// 4带两对
            /// </summary>
            CT_FOUR_TAKE_TWO = 10,
            /// <summary>
            /// 炸弹
            /// </summary>
            CT_BOMB_CARD = 11,
            /// <summary>
            /// 火箭
            /// </summary>
            CT_MISSILE_CARD = 12
        }
        #endregion

        #region 构造扑克
        byte MakeCardData(byte cbValueIndex, byte cbColorIndex)
        {
	        return (byte)((cbColorIndex<<4)|(cbValueIndex+1));
        }
        #endregion



        #region 扑克排序 从大到小
        /// <summary>
        /// 排列扑克
        /// </summary>
        /// <param name="cbCardData"></param>
        const ushort ST_CUSTOM = 3;
        const ushort ST_COUNT = 2;
        const ushort ST_ORDER = 1;
        public void SortCardList(ref byte[] cbCardData, byte cbCardCount)
        {
            //转换数值
            byte[] cbLogicValue = new byte[cbCardCount];

            for (byte i = 0; i < cbCardCount; i++)
            {
                cbLogicValue[i] = GetCardLogicValue(cbCardData[i]);
            }

            //排序操作
            bool bSorted = true;
            byte cbTempData, bLast = (byte)(cbCardCount - 1);

            do
            {
                bSorted = true;

                for (byte i = 0; i < bLast; i++)
                {
                    if ((cbLogicValue[i] <= cbLogicValue[i + 1]) ||
                        ((cbLogicValue[i] == cbLogicValue[i + 1])
                        && (cbCardData[i] <= cbCardData[i + 1])))
                    {
                        //交换位置
                        cbTempData = cbCardData[i];
                        cbCardData[i] = cbCardData[i + 1];
                        cbCardData[i + 1] = cbTempData;
                        cbTempData = cbLogicValue[i];
                        cbLogicValue[i] = cbLogicValue[i + 1];
                        cbLogicValue[i + 1] = cbTempData;
                        bSorted = false;
                    }
                }
                bLast--;
            } while (bSorted == false);

        }
        #endregion

        #region 扑克排序 从小到大
        public void SortCardListByAsc(ref byte[] cbCardData, byte cbCardCount)
        {
            //转换数值
            byte[] cbLogicValue = new byte[cbCardCount];

            for (byte i = 0; i < cbCardCount; i++)
            {
                cbLogicValue[i] = GetCardLogicValue(cbCardData[i]);
            }

            //排序操作
            bool bSorted = true;
            byte cbTempData, bLast = (byte)(cbCardCount - 1);

            do
            {
                bSorted = true;

                for (byte i = 0; i < bLast; i++)
                {
                    if ((cbLogicValue[i] > cbLogicValue[i + 1]) ||
                        ((cbLogicValue[i] == cbLogicValue[i + 1])
                        && (cbCardData[i] > cbCardData[i + 1])))
                    {
                        //交换位置
                        cbTempData = cbCardData[i];
                        cbCardData[i] = cbCardData[i + 1];
                        cbCardData[i + 1] = cbTempData;
                        cbTempData = cbLogicValue[i];
                        cbLogicValue[i] = cbLogicValue[i + 1];
                        cbLogicValue[i + 1] = cbTempData;
                        bSorted = false;
                    }
                }
                bLast--;
            } while (bSorted == false);
        }
        #endregion


        #region 逻辑数值
        //数值掩码
        public const byte LOGIC_MASK_VALUE = 0x0F;
        public const byte LOGIC_MASK_COLOR = 0xF0;
        /// <summary>
        /// 获取数值
        /// </summary>
        /// <param name="cbCardData">一张卡牌</param>
        /// <returns></returns>
        public byte GetCardValue(byte cbCardData)
        {
            return (byte)(cbCardData & LOGIC_MASK_VALUE);
        }

        /// <summary>
        /// 获取花色
        /// </summary>
        /// <param name="cbCardData">一张卡牌</param>
        /// <returns></returns>
        public byte GetCardColor(byte cbCardData)
        {
            return (byte)(cbCardData & LOGIC_MASK_COLOR);
        }

        public byte GetCardLogicValue(byte cbCardData)
        {
            if (cbCardData == 0)
                return 0;
            //扑克属性
            byte cbCardColor = GetCardColor(cbCardData);
            byte cbCardValue = GetCardValue(cbCardData);

            //转换数值
            if (cbCardColor == 0x40) return (byte)(cbCardValue + 2);
     
            return (byte)((cbCardValue <= 2) ? (cbCardValue + 13) : cbCardValue);
        }
        #endregion

    }
}
