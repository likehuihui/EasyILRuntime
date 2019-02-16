using System;
using System.Collections.Generic;
using System.Text;


namespace Land
{
    /// <summary>
    /// 斗地主逻辑接口
    /// </summary>
    public partial class Land_
    {

        /// <summary>
        /// 最大玩家人数
        /// </summary>
        public const byte GAME_PLAYER = 3;

        /// <summary>
        /// 最大牌数目
        /// </summary>
        public const byte MAX_COUNT = 20;

        /// <summary>
        /// 最大牌数目
        /// </summary>
        public const byte NORMAL_COUNT = 17;

        /// <summary>
        /// 整副牌数目
        /// </summary>
        public const byte FULL_COUNT = 54;


        /// <summary>
        /// 索引变量
        /// </summary>
        const byte cbIndexCount = 5;

        #region 分析结构
        /// <summary>
        /// 分析结构
        /// </summary>
        public struct tagAnalyseResult
        {
            public byte[] cbBlockCount;		             //扑克数目 [4]
            public byte[,] cbCardData;			        //扑克数据 [4] [MAX_COUNT]
            public tagAnalyseResult(int v)
            {
                cbBlockCount = new byte[4];
                cbCardData = new byte[4, Land_.MAX_COUNT];
            }
        }

        //搜索结果
        public struct tagSearchCardResult
        {
            public byte cbSearchCount;					//结果数目
            public byte[] cbCardCount;				    //扑克数目
            public byte[][] cbResultCard;	            //结果扑克

            public tagSearchCardResult(int v)
            {
                cbSearchCount = 0;
                cbCardCount = new byte[Land_.MAX_COUNT];
                cbResultCard = new byte[Land_.MAX_COUNT][];
                for (byte i = 0; i < Land_.MAX_COUNT; i++)
                {
                    cbResultCard[i] = new byte[Land_.MAX_COUNT];
                }
            }


        };

        //分布信息
        public struct tagDistributing
        {
            public byte cbCardCount;		//扑克数目
            public byte[][] cbDistributing;     //分布信息
            public tagDistributing(int v)
            {
                cbCardCount = 0;
                cbDistributing = new byte[15][];
                for (byte i = 0; i < 15; i++)
                {
                    cbDistributing[i] = new byte[6];
                }
            }
        };
        #endregion


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

        #region 分析牌标记
        public void AnalysebCardData(byte[] cbCardData, byte cbCardCount, ref tagAnalyseResult AnalyseResult)
        {
            //清空
            AnalyseResult = new tagAnalyseResult(0);


            //设置结果

            //扑克分析
            for (int i = 0; i < cbCardCount; i++)
            {
                //变量定义
                int cbSameCount = 1;
                byte cbLogicValue = GetCardLogicValue(cbCardData[i]);

                //搜索同牌
                for (int j = i + 1; j < cbCardCount; j++)
                {
                    //获取扑克
                    if (GetCardLogicValue(cbCardData[j]) != cbLogicValue) break;

                    //设置变量
                    cbSameCount++;
                }

                if (cbSameCount > 4)
                {
                    //设置无效结果
                    AnalyseResult.cbBlockCount = new byte[4];		            //扑克数目 [4]
                    AnalyseResult.cbCardData = new byte[4, MAX_COUNT];			//扑克数据 [4] [MAX_COUNT]
                    return;
                }

                //设置结果
                byte cbIndex = AnalyseResult.cbBlockCount[cbSameCount - 1]++;
                for (byte j = 0; j < cbSameCount; j++) AnalyseResult.cbCardData[cbSameCount - 1, cbIndex * cbSameCount + j] = cbCardData[i + j];

                //设置索引
                i += cbSameCount - 1;
            }
        }
        #endregion

        #region 分析分布
        //分析分布
        public void AnalysebDistributing(byte[] cbCardData, byte cbCardCount, ref tagDistributing Distributing)
        {
            Distributing = new tagDistributing(0);

	        //设置变量
	        for (byte i=0;i<cbCardCount;i++)
	        {
		        if (cbCardData[i]==0) continue;

		        //获取属性
		        byte cbCardColor=GetCardColor(cbCardData[i]);
		        byte cbCardValue=GetCardValue(cbCardData[i]);

		        //分布信息
		        Distributing.cbCardCount++;
		        Distributing.cbDistributing[cbCardValue-1][cbIndexCount]++;
		        Distributing.cbDistributing[cbCardValue-1][cbCardColor>>4]++;
	        }

	        return;
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
            byte[] cbLogicValue = new byte[MAX_COUNT];

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
            //数目过虑
// 	    if (cbCardCount==0) return;
// 	    if (cbSortType==ST_CUSTOM) return;
// 
// 	    //转换数值
// 	    byte[] cbSortValue = new byte[MAX_COUNT];
//         for (byte i = 0; i < cbCardCount; i++) cbSortValue[i] = GetCardLogicValue(cbCardData[i]);	
// 
// 	    //排序操作
// 	    bool bSorted=true;
//         byte cbSwitchData = 0, cbLast =(byte)(cbCardCount - 1);
// 	    do
// 	    {
// 		    bSorted=true;
//             for (byte i = 0; i < cbLast; i++)
// 		    {
// 			    if ((cbSortValue[i]<cbSortValue[i+1])||
// 				    ((cbSortValue[i]==cbSortValue[i+1])&&(cbCardData[i]<cbCardData[i+1])))
// 			    {
// 				    //设置标志
// 				    bSorted=false;
// 
// 				    //扑克数据
// 				    cbSwitchData=cbCardData[i];
// 				    cbCardData[i]=cbCardData[i+1];
// 				    cbCardData[i+1]=cbSwitchData;
// 
// 				    //排序权位
// 				    cbSwitchData=cbSortValue[i];
// 				    cbSortValue[i]=cbSortValue[i+1];
// 				    cbSortValue[i+1]=cbSwitchData;
// 			    }	
// 		    }
// 		    cbLast--;
// 	    } while(bSorted==false);
// 
// 	    //数目排序
// 	    if (cbSortType==ST_COUNT)
// 	    {
// 		    //变量定义
// 		    byte cbCardIndex=0;
// 
// 		    //分析扑克
// 		    tagAnalyseResult AnalyseResult = new tagAnalyseResult(0);
//             byte[] cbTempCardData = new byte[(cbCardCount - cbCardIndex)];
//             Array.Copy(cbCardData, cbCardIndex, cbTempCardData, 0, (cbCardCount - cbCardIndex));
//             AnalysebCardData(cbTempCardData, (byte)(cbCardCount - cbCardIndex), ref AnalyseResult);
// 
// 		    //提取扑克
// 		    for (byte i=0;i<AnalyseResult.cbBlockCount.Length;i++)
// 		    {
// 			    //拷贝扑克
//                 byte cbIndex = (byte)(AnalyseResult.cbBlockCount.Length - i - 1);
//                // Array.Copy(AnalyseResult.cbCardData[cbIndex], cbCardData, AnalyseResult.cbBlockCount[cbIndex] * (cbIndex + 1) * sizeof(byte));
// 
// 			    //设置索引
// 			    cbCardIndex+=(byte)(AnalyseResult.cbBlockCount[cbIndex]*(cbIndex+1)*sizeof(byte));
// 		    }
// 	    }

        }
        #endregion

        #region 扑克排序 从小到大
        public void SortCardListByAsc(ref byte[] cbCardData, byte cbCardCount)
        {
            //转换数值
            byte[] cbLogicValue = new byte[MAX_COUNT];

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

        #region 获取牌型
        /// <summary>
        /// 获取牌的类型
        /// </summary>
        /// <param name="cbCardData">一组牛牛的牌</param>        
        /// <param name="cbCardCount">牌的张数</param>
        /// <returns></returns>
        public emCardType GetCardType(byte[] cbCardData, byte cbCardCount)
        {
            //简单牌型
            switch (cbCardCount)
            {
                case 0:	//空牌
                    {
                        return emCardType.CT_ERROR;
                    }
                case 1: //单牌
                    {
                        return emCardType.CT_SINGLE;
                    }
                case 2:	//对牌火箭
                    {
                        //牌型判断
                        if ((cbCardData[0] == 0x4F) && (cbCardData[1] == 0x4E)) return emCardType.CT_MISSILE_CARD;
                        if (GetCardLogicValue(cbCardData[0]) == GetCardLogicValue(cbCardData[1])) return emCardType.CT_DOUBLE;

                        return emCardType.CT_ERROR;
                    }
            }

            //分析扑克
            tagAnalyseResult AnalyseResult = new tagAnalyseResult(0);
            AnalysebCardData(cbCardData, cbCardCount, ref AnalyseResult);

            //四牌判断
            if (AnalyseResult.cbBlockCount[3] > 0)
            {
                //牌型判断
                if ((AnalyseResult.cbBlockCount[3] == 1) && (cbCardCount == 4)) return emCardType.CT_BOMB_CARD;
                if ((AnalyseResult.cbBlockCount[3] == 1) && (cbCardCount == 6)) return emCardType.CT_FOUR_TAKE_ONE;
                if ((AnalyseResult.cbBlockCount[3] == 1) && (cbCardCount == 8) && (AnalyseResult.cbBlockCount[1] == 2)) return emCardType.CT_FOUR_TAKE_TWO;

                return emCardType.CT_ERROR;
            }

            //三牌判断
            if (AnalyseResult.cbBlockCount[2] > 0)
            {
                //连牌判断
                if (AnalyseResult.cbBlockCount[2] > 1)
                {
                    //变量定义
                    byte cbCardData1 = AnalyseResult.cbCardData[2, 0];
                    byte cbFirstLogicValue = GetCardLogicValue(cbCardData1);

                    //错误过虑
                    if (cbFirstLogicValue >= 15) return emCardType.CT_ERROR;

                    //连牌判断
                    for (byte i = 1; i < AnalyseResult.cbBlockCount[2]; i++)
                    {
                        byte cbCardData2 = AnalyseResult.cbCardData[2, i * 3];
                        if (cbFirstLogicValue != (GetCardLogicValue(cbCardData2) + i)) return emCardType.CT_ERROR;
                    }
                }
                else if (cbCardCount == 3) return emCardType.CT_THREE;

                //牌形判断
                if (AnalyseResult.cbBlockCount[2] * 3 == cbCardCount) return emCardType.CT_THREE_LINE;
                if (AnalyseResult.cbBlockCount[2] * 4 == cbCardCount) return emCardType.CT_THREE_TAKE_ONE;
                if ((AnalyseResult.cbBlockCount[2] * 5 == cbCardCount) && (AnalyseResult.cbBlockCount[1] == AnalyseResult.cbBlockCount[2])) return emCardType.CT_THREE_TAKE_TWO;

                return emCardType.CT_ERROR;
            }

            //两张类型
            if (AnalyseResult.cbBlockCount[1] >= 3)
            {
                //变量定义
                byte cbCardData3 = AnalyseResult.cbCardData[1, 0];
                byte cbFirstLogicValue = GetCardLogicValue(cbCardData3);

                //错误过虑
                if (cbFirstLogicValue >= 15) return emCardType.CT_ERROR;

                //连牌判断
                for (byte i = 1; i < AnalyseResult.cbBlockCount[1]; i++)
                {
                    byte cbCardData4 = AnalyseResult.cbCardData[1, i * 2];
                    if (cbFirstLogicValue != (GetCardLogicValue(cbCardData4) + i)) return emCardType.CT_ERROR;
                }

                //二连判断
                if ((AnalyseResult.cbBlockCount[1] * 2) == cbCardCount) return emCardType.CT_DOUBLE_LINE;

                return emCardType.CT_ERROR;
            }

            //单张判断
            if ((AnalyseResult.cbBlockCount[0] >= 5) && (AnalyseResult.cbBlockCount[0] == cbCardCount))
            {
                //变量定义
                byte cbCardData5 = AnalyseResult.cbCardData[0, 0];
                byte cbFirstLogicValue = GetCardLogicValue(cbCardData5);

                //错误过虑
                if (cbFirstLogicValue >= 15) return emCardType.CT_ERROR;

                //连牌判断
                for (byte i = 1; i < AnalyseResult.cbBlockCount[0]; i++)
                {
                    byte cbCardData6 = AnalyseResult.cbCardData[0, i];
                    if (cbFirstLogicValue != (GetCardLogicValue(cbCardData6) + i)) return emCardType.CT_ERROR;
                }

                return emCardType.CT_SINGLE_LINE;
            }

            return emCardType.CT_ERROR;
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

        #region 删除接口
        public bool RemoveCardList(byte[] cbRemoveCard,byte cbRemoveCount,ref byte[] cbCardData, byte cbCardCount)
        {
	        //定义变量
	        byte cbDeleteCount=0;
            byte[] cbTempCardData = new byte[MAX_COUNT];
	        if (cbCardCount>MAX_COUNT) return false;
            cbCardData.CopyTo(cbRemoveCard, 0);

	        //置零扑克
	        for (byte i=0;i<cbRemoveCount;i++)
	        {
		        for (byte j=0;j<cbCardCount;j++)
		        {
			        if (cbRemoveCard[i]==cbTempCardData[j])
			        {
				        cbDeleteCount++;
				        cbTempCardData[j]=0;
				        break;
			        }
		        }
	        }
	        if (cbDeleteCount!=cbRemoveCount) return false;

	        //清理扑克
	        byte cbCardPos=0;
	        for (byte i=0;i<cbCardCount;i++)
	        {
		        if (cbTempCardData[i]!=0) cbCardData[cbCardPos++]=cbTempCardData[i];
	        }

	        return true;
        }

       public bool RemoveCard(byte[] cbRemoveCard, byte cbRemoveCount,ref byte[] cbCardData, byte cbCardCount)
       {

	        //定义变量
	        byte cbDeleteCount=0;
            byte[] cbTempCardData = new byte[MAX_COUNT];
	        if (cbCardCount>MAX_COUNT) return false;
            Array.Copy(cbCardData,cbTempCardData,cbCardCount);

	        //置零扑克
	        for (byte i=0;i<cbRemoveCount;i++)
	        {
                for (byte j = 0; j < cbCardCount; j++)
		        {
			        if (cbRemoveCard[i]==cbTempCardData[j])
			        {
				        cbDeleteCount++;
				        cbTempCardData[j]=0;
				        break;
			        }
		        }
	        }
	        if (cbDeleteCount!=cbRemoveCount) return false;

	        //清理扑克
            byte cbCardPos = 0;
            for (byte i = 0; i < cbCardCount; i++)
	        {
		        if (cbTempCardData[i]!=0) cbCardData[cbCardPos++]=cbTempCardData[i];
	        }

	        return true;
       }
        #endregion

        #region 比较
       public bool CompareCard(byte[] cbFirstCard, byte[] cbNextCard, byte cbFirstCount, byte cbNextCount)
       {
           //获取类型
           emCardType cbNextType = GetCardType(cbNextCard, cbNextCount);
           emCardType cbFirstType = GetCardType(cbFirstCard, cbFirstCount);

           //类型判断
           if (cbNextType == emCardType.CT_ERROR) return false;
           if (cbNextType == emCardType.CT_MISSILE_CARD) return true;

           //炸弹判断
           if ((cbFirstType != emCardType.CT_BOMB_CARD) && (cbNextType == emCardType.CT_BOMB_CARD)) return true;
           if ((cbFirstType == emCardType.CT_BOMB_CARD) && (cbNextType != emCardType.CT_BOMB_CARD)) return false;

           //规则判断
           if ((cbFirstType != cbNextType) || (cbFirstCount != cbNextCount)) return false;

           //开始对比
           switch (cbNextType)
           {
               case emCardType.CT_SINGLE:
               case emCardType.CT_DOUBLE:
               case emCardType.CT_THREE:
               case emCardType.CT_SINGLE_LINE:
               case emCardType.CT_DOUBLE_LINE:
               case emCardType.CT_THREE_LINE:
               case emCardType.CT_BOMB_CARD:
                   {
                       //获取数值
                       byte cbNextLogicValue = GetCardLogicValue(cbNextCard[0]);
                       byte cbFirstLogicValue = GetCardLogicValue(cbFirstCard[0]);

                       //对比扑克
                       return cbNextLogicValue > cbFirstLogicValue;
                   }
               case emCardType.CT_THREE_TAKE_ONE:
               case emCardType.CT_THREE_TAKE_TWO:
                   {
                       tagAnalyseResult NextResult = new tagAnalyseResult(0);
                       tagAnalyseResult FirstResult = new tagAnalyseResult(0);
                       //获取数值
                       AnalysebCardData(cbFirstCard, cbFirstCount, ref FirstResult);
                       AnalysebCardData(cbNextCard, cbNextCount,ref NextResult);
                       byte cbFirstLogicValue = GetCardLogicValue(FirstResult.cbCardData[2,0]);
                       byte cbNextLogicValue = GetCardLogicValue(NextResult.cbCardData[2,0]);


                       //对比扑克
                       return cbNextLogicValue > cbFirstLogicValue;
                   }
               case emCardType.CT_FOUR_TAKE_ONE:
               case emCardType.CT_FOUR_TAKE_TWO:
                   {
                       //分析扑克
                       tagAnalyseResult NextResult = new tagAnalyseResult(0);
                       tagAnalyseResult FirstResult = new tagAnalyseResult(0);
                       AnalysebCardData(cbNextCard, cbNextCount,ref NextResult);
                       AnalysebCardData(cbFirstCard, cbFirstCount,ref  FirstResult);

                       //获取数值
                       byte cbNextLogicValue = GetCardLogicValue(NextResult.cbCardData[3,0]);
                       byte cbFirstLogicValue = GetCardLogicValue(FirstResult.cbCardData[3, 0]);

                       //对比扑克
                       return cbNextLogicValue > cbFirstLogicValue;
                   }
           }

           return false;
       }

       #endregion


        #region 搜索出牌
       public byte SearchOutCard(byte[] cbHandCardData, byte cbHandCardCount, byte[] cbTurnCardData, byte cbTurnCardCount, 
		ref tagSearchCardResult pSearchCardResult)
       {

	        //变量定义
	        byte cbResultCount = 0;
	        tagSearchCardResult tmpSearchCardResult = new tagSearchCardResult(0);
            //
            pSearchCardResult = new tagSearchCardResult(0);
            //构造扑克
            byte[] cbCardData = new byte[MAX_COUNT];
	        byte cbCardCount=cbHandCardCount;
            Array.Copy(cbHandCardData,cbCardData, cbHandCardCount);

	        //排列扑克
	        SortCardList(ref cbCardData,cbCardCount);

	        //获取类型
	        emCardType cbTurnOutType=GetCardType(cbTurnCardData,cbTurnCardCount);

	        //出牌分析
	        switch (cbTurnOutType)
	        {
	        case emCardType.CT_ERROR:					//错误类型
		        {


			        //是否一手出完
			        if( GetCardType(cbCardData,cbCardCount) != emCardType.CT_ERROR )
			        {
				        pSearchCardResult.cbCardCount[cbResultCount] = cbCardCount;
                        pSearchCardResult.cbResultCard[cbResultCount]=(byte[])cbCardData.Clone();
				        cbResultCount++;
			        }

			        //如果最小牌不是单牌，则提取
			        byte cbSameCount = 0;
			        if( cbCardCount > 1 && GetCardValue(cbCardData[cbCardCount-1]) == GetCardValue(cbCardData[cbCardCount-2]) )
			        {
				        cbSameCount = 1;
				        pSearchCardResult.cbResultCard[cbResultCount][0] = cbCardData[cbCardCount-1];
				        byte cbCardValue = GetCardValue(cbCardData[cbCardCount-1]);
				        for( int i = cbCardCount-2; i >= 0; i-- )
				        {
					        if( GetCardValue(cbCardData[i]) == cbCardValue )
					        {
						        pSearchCardResult.cbResultCard[cbResultCount][cbSameCount++] = cbCardData[i];
					        }
					        else break;
				        }

				        pSearchCardResult.cbCardCount[cbResultCount] = cbSameCount;
				        cbResultCount++;
			        }

			        //单牌
			        byte cbTmpCount = 0;
			        if( cbSameCount != 1 )
			        {
				        cbTmpCount = SearchSameCard( cbCardData,cbCardCount,0,1,ref tmpSearchCardResult );
				        if( cbTmpCount > 0 )
				        {
					        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];

					        Array.Copy(tmpSearchCardResult.cbResultCard[0], pSearchCardResult.cbResultCard[cbResultCount],
						                tmpSearchCardResult.cbCardCount[0] );
					        cbResultCount++;
				        }
			        }

			        //对牌
			        if( cbSameCount != 2 )
			        {
				        cbTmpCount = SearchSameCard( cbCardData,cbCardCount,0,2,ref tmpSearchCardResult );
				        if( cbTmpCount > 0 )
				        {
					        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];
					        Array.Copy( tmpSearchCardResult.cbResultCard[0],pSearchCardResult.cbResultCard[cbResultCount],
						                 tmpSearchCardResult.cbCardCount[0] );
					        cbResultCount++;
				        }
			        }

			        //三条
			        if( cbSameCount != 3 )
			        {
				        cbTmpCount = SearchSameCard( cbCardData,cbCardCount,0,3,ref tmpSearchCardResult );
				        if( cbTmpCount > 0 )
				        {
					        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];
					        Array.Copy( tmpSearchCardResult.cbResultCard[0],pSearchCardResult.cbResultCard[cbResultCount],
						            tmpSearchCardResult.cbCardCount[0] );
					        cbResultCount++;
				        }
			        }

			        //三带一单
			        cbTmpCount = SearchTakeCardType( cbCardData,cbCardCount,0,3,1,ref tmpSearchCardResult );
			        if( cbTmpCount > 0 )
			        {
				        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];
				        Array.Copy( tmpSearchCardResult.cbResultCard[0],pSearchCardResult.cbResultCard[cbResultCount],
					                tmpSearchCardResult.cbCardCount[0] );
				        cbResultCount++;
			        }

			        //三带一对
			        cbTmpCount = SearchTakeCardType( cbCardData,cbCardCount,0,3,2,ref tmpSearchCardResult );
			        if( cbTmpCount > 0 )
			        {
				        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];
				        Array.Copy( tmpSearchCardResult.cbResultCard[0],pSearchCardResult.cbResultCard[cbResultCount],
					                tmpSearchCardResult.cbCardCount[0] );
				        cbResultCount++;
			        }

			        //单连
			        cbTmpCount = SearchLineCardType( cbCardData,cbCardCount,0,1,0,ref tmpSearchCardResult );
			        if( cbTmpCount > 0 )
			        {
				        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];
				        Array.Copy( tmpSearchCardResult.cbResultCard[0],pSearchCardResult.cbResultCard[cbResultCount],
					            tmpSearchCardResult.cbCardCount[0] );
				        cbResultCount++;
			        }

			        //连对
			        cbTmpCount = SearchLineCardType( cbCardData,cbCardCount,0,2,0,ref tmpSearchCardResult );
			        if( cbTmpCount > 0 )
			        {
				        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];
				        Array.Copy( tmpSearchCardResult.cbResultCard[0],pSearchCardResult.cbResultCard[cbResultCount],
					                tmpSearchCardResult.cbCardCount[0] );
				        cbResultCount++;
			        }

			        //三连
			        cbTmpCount = SearchLineCardType( cbCardData,cbCardCount,0,3,0,ref tmpSearchCardResult );
			        if( cbTmpCount > 0 )
			        {
				        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];
				        Array.Copy( tmpSearchCardResult.cbResultCard[0],pSearchCardResult.cbResultCard[cbResultCount],
					                tmpSearchCardResult.cbCardCount[0] );
				        cbResultCount++;
			        }

			        ////飞机
			        //cbTmpCount = SearchThreeTwoLine( cbCardData,cbCardCount,&tmpSearchCardResult );
			        //if( cbTmpCount > 0 )
			        //{
			        //	pSearchCardResult->cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];
			        //	CopyMemory( pSearchCardResult->cbResultCard[cbResultCount],tmpSearchCardResult.cbResultCard[0],
			        //		sizeof(BYTE)*tmpSearchCardResult.cbCardCount[0] );
			        //	cbResultCount++;
			        //}

			        //炸弹
			        if( cbSameCount != 4 )
			        {
				        cbTmpCount = SearchSameCard( cbCardData,cbCardCount,0,4,ref tmpSearchCardResult );
				        if( cbTmpCount > 0 )
				        {
					        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[0];
					        Array.Copy( tmpSearchCardResult.cbResultCard[0],pSearchCardResult.cbResultCard[cbResultCount],
						            tmpSearchCardResult.cbCardCount[0] );
					        cbResultCount++;
				        }
			        }

			        //搜索火箭
			        if ((cbCardCount>=2)&&(cbCardData[0]==0x4F)&&(cbCardData[1]==0x4E))
			        {
				        //设置结果
				        pSearchCardResult.cbCardCount[cbResultCount] = 2;
				        pSearchCardResult.cbResultCard[cbResultCount][0] = cbCardData[0];
				        pSearchCardResult.cbResultCard[cbResultCount][1] = cbCardData[1];

				        cbResultCount++;
			        }

			        pSearchCardResult.cbSearchCount = cbResultCount;
			        return cbResultCount;
		        }
	        case emCardType.CT_SINGLE:					//单牌类型
	        case emCardType.CT_DOUBLE:					//对牌类型
	        case emCardType.CT_THREE:					//三条类型
		        {
			        //变量定义
			        byte cbReferCard=cbTurnCardData[0];
			        byte cbSameCount = 1;
			        if( cbTurnOutType == emCardType.CT_DOUBLE ) cbSameCount = 2;
			        else if( cbTurnOutType == emCardType.CT_THREE ) cbSameCount = 3;

			        //搜索相同牌
			        cbResultCount = SearchSameCard( cbCardData,cbCardCount,cbReferCard,cbSameCount,ref pSearchCardResult );

			        break;
		        }
	        case emCardType.CT_SINGLE_LINE:		//单连类型
	        case emCardType.CT_DOUBLE_LINE:		//对连类型
	        case emCardType.CT_THREE_LINE:				//三连类型
		        {
			        //变量定义
			        byte cbBlockCount = 1;
			        if( cbTurnOutType == emCardType.CT_DOUBLE_LINE ) cbBlockCount = 2;
			        else if( cbTurnOutType == emCardType.CT_THREE_LINE ) cbBlockCount = 3;

			        byte cbLineCount = (byte)(cbTurnCardCount/cbBlockCount);

			        //搜索边牌
			        cbResultCount = SearchLineCardType( cbCardData,cbCardCount,cbTurnCardData[0],cbBlockCount,cbLineCount,ref pSearchCardResult );

			        break;
		        }
	        case emCardType.CT_THREE_TAKE_ONE:	//三带一单
	        case emCardType.CT_THREE_TAKE_TWO:	//三带一对
		        {
			        //效验牌数
			        if( cbCardCount < cbTurnCardCount ) break;

			        //如果是三带一或三带二
			        if( cbTurnCardCount == 4 || cbTurnCardCount == 5 )
			        {
				        byte cbTakeCardCount = (byte)(cbTurnOutType==emCardType.CT_THREE_TAKE_ONE?1:2);

				        //搜索三带牌型
				        cbResultCount = SearchTakeCardType( cbCardData,cbCardCount,cbTurnCardData[2],3,cbTakeCardCount,ref pSearchCardResult );
			        }
			        else
			        {
				        //变量定义
				        byte cbBlockCount = 3;
				        byte cbLineCount = (byte)(cbTurnCardCount/(cbTurnOutType==emCardType.CT_THREE_TAKE_ONE?4:5));
				        byte cbTakeCardCount = (byte)(cbTurnOutType==emCardType.CT_THREE_TAKE_ONE?1:2);

				        //搜索连牌
				        byte[] cbTmpTurnCard = new byte[MAX_COUNT];
                        Array.Copy(cbTurnCardData,cbTmpTurnCard,cbTurnCardCount);
				        SortCardList(ref cbTmpTurnCard,cbTurnCardCount );
				        cbResultCount = SearchLineCardType( cbCardData,cbCardCount,cbTmpTurnCard[0],cbBlockCount,cbLineCount,ref pSearchCardResult );

				        //提取带牌
				        bool bAllDistill = true;
				        for( byte i = 0; i < cbResultCount; i++ )
				        {
					        byte cbResultIndex = (byte)(cbResultCount-i-1);

					        //变量定义
					        byte[] cbTmpCardData = new byte[MAX_COUNT];
					        byte cbTmpCardCount = cbCardCount;

					        //删除连牌
					        Array.Copy( cbCardData,cbTmpCardData,cbCardCount );
					        RemoveCard( pSearchCardResult.cbResultCard[cbResultIndex],pSearchCardResult.cbCardCount[cbResultIndex],ref cbTmpCardData,cbTmpCardCount ) ;
					        cbTmpCardCount -= pSearchCardResult.cbCardCount[cbResultIndex];

					        //分析牌
					        tagAnalyseResult  TmpResult = new tagAnalyseResult(0);
					        AnalysebCardData( cbTmpCardData,cbTmpCardCount,ref TmpResult );

					        //提取牌
					        byte[] cbDistillCard = new byte[MAX_COUNT];
					        byte cbDistillCount = 0;
					        for( byte j = (byte)(cbTakeCardCount-1); j < TmpResult.cbBlockCount.Length; j++ )
					        {
						        if( TmpResult.cbBlockCount[j] > 0 )
						        {
							        if( j+1 == cbTakeCardCount && TmpResult.cbBlockCount[j] >= cbLineCount )
							        {
								        byte cbTmpBlockCount = TmpResult.cbBlockCount[j];
                                        cbDistillCount = (byte)((j+1)*cbLineCount);
                                        for(byte c = 0; c < cbDistillCount; c++)
                                        {
                                            cbDistillCard[c] = TmpResult.cbCardData[j,((cbTmpBlockCount-cbLineCount)*(j+1))+c];
                                        }

								        break;
							        }
							        else
							        {
								        for( byte k = 0; k < TmpResult.cbBlockCount[j]; k++ )
								        {
									        byte cbTmpBlockCount = TmpResult.cbBlockCount[j];
                                            for(byte c = cbDistillCount; c < cbTakeCardCount; c++)
                                            {
                                                cbDistillCard[c] = TmpResult.cbCardData[j,((cbTmpBlockCount-k-1)*(j+1))+c];
                                            }
                                            cbDistillCount += cbTakeCardCount;

									        //提取完成
									        if( cbDistillCount == cbTakeCardCount*cbLineCount ) break;
								        }
							        }
						        }

						        //提取完成
						        if( cbDistillCount == cbTakeCardCount*cbLineCount ) break;
					        }

					        //提取完成
					        if( cbDistillCount == cbTakeCardCount*cbLineCount )
					        {
						        //复制带牌
						        byte cbCount = pSearchCardResult.cbCardCount[cbResultIndex];
                                for(byte c = 0; c < cbDistillCount; c++)
                                {
                                    cbDistillCard[c] = TmpResult.cbCardData[cbResultIndex,cbCount+c];
                                }
						        pSearchCardResult.cbCardCount[cbResultIndex] += cbDistillCount;
					        }
					        //否则删除连牌
					        else
					        {
						        bAllDistill = false;
						        pSearchCardResult.cbCardCount[cbResultIndex] = 0;
					        }
				        }

				        //整理组合
				        if( !bAllDistill )
				        {
					        pSearchCardResult.cbSearchCount = cbResultCount;
					        cbResultCount = 0;
					        for( byte i = 0; i < pSearchCardResult.cbSearchCount; i++ )
					        {
						        if( pSearchCardResult.cbCardCount[i] != 0 )
						        {
							        tmpSearchCardResult.cbCardCount[cbResultCount] = pSearchCardResult.cbCardCount[i];
							        Array.Copy( pSearchCardResult.cbResultCard[i],tmpSearchCardResult.cbResultCard[cbResultCount],
								                pSearchCardResult.cbCardCount[i] );
							        cbResultCount++;
						        }
					        }
					        tmpSearchCardResult.cbSearchCount = cbResultCount;
                            pSearchCardResult = tmpSearchCardResult;
				        }
			        }

			        break;
		        }
	        case emCardType.CT_FOUR_TAKE_ONE:		//四带两单
	        case emCardType.CT_FOUR_TAKE_TWO:		//四带两双
		        {
			        byte cbTakeCount = (byte)(cbTurnOutType==emCardType.CT_FOUR_TAKE_ONE?1:2);

                    byte[] cbTmpTurnCard = new byte[MAX_COUNT];
			        Array.Copy( cbTurnCardData,cbTmpTurnCard,cbTurnCardCount );
			        SortCardList(ref cbTmpTurnCard,cbTurnCardCount );

			        //搜索带牌
			        cbResultCount = SearchTakeCardType( cbCardData,cbCardCount,cbTmpTurnCard[0],4,cbTakeCount,ref pSearchCardResult );

			        break;
		        }
	        }

	        //搜索炸弹
	        if ((cbCardCount>=4)&&(cbTurnOutType!=emCardType.CT_MISSILE_CARD))
	        {
		        //变量定义
		        byte cbReferCard = 0;
		        if (cbTurnOutType==emCardType.CT_BOMB_CARD) cbReferCard=cbTurnCardData[0];

		        //搜索炸弹
		        byte cbTmpResultCount = SearchSameCard( cbCardData,cbCardCount,cbReferCard,4,ref tmpSearchCardResult );
		        for( byte i = 0; i < cbTmpResultCount; i++ )
		        {
			        pSearchCardResult.cbCardCount[cbResultCount] = tmpSearchCardResult.cbCardCount[i];
                    Array.Copy(tmpSearchCardResult.cbResultCard[i], pSearchCardResult.cbResultCard[cbResultCount],
				                tmpSearchCardResult.cbCardCount[i] );
			        cbResultCount++;
		        }
	        }

	        //搜索火箭
	        if (cbTurnOutType!=emCardType.CT_MISSILE_CARD&&(cbCardCount>=2)&&(cbCardData[0]==0x4F)&&(cbCardData[1]==0x4E))
	        {
		        //设置结果
		        pSearchCardResult.cbCardCount[cbResultCount] = 2;
		        pSearchCardResult.cbResultCard[cbResultCount][0] = cbCardData[0];
		        pSearchCardResult.cbResultCard[cbResultCount][1] = cbCardData[1];

		        cbResultCount++;
	        }

	        pSearchCardResult.cbSearchCount = cbResultCount;
	        return cbResultCount;
       }
       #endregion

        #region 同牌搜索
       public byte SearchSameCard(byte[] cbHandCardData, byte cbHandCardCount, byte cbReferCard, byte cbSameCardCount,
		ref tagSearchCardResult pSearchCardResult)
       {
           	//设置结果
            pSearchCardResult = new tagSearchCardResult(0);
	        byte cbResultCount = 0;

	        //构造扑克
            byte[] cbCardData = new byte[MAX_COUNT];
	        byte cbCardCount=cbHandCardCount;
	        Array.Copy(cbHandCardData,cbCardData,cbHandCardCount);

	        //排列扑克
	        SortCardList(ref cbCardData,cbCardCount);

	        //分析扑克
	        tagAnalyseResult AnalyseResult = new tagAnalyseResult(0);
	        AnalysebCardData( cbCardData,cbCardCount,ref AnalyseResult );

	        byte cbReferLogicValue = (byte)(cbReferCard==0?0:GetCardLogicValue(cbReferCard));
	        byte cbBlockIndex = (byte)(cbSameCardCount-1);
	        do
	        {
		        for( byte i = 0; i < AnalyseResult.cbBlockCount[cbBlockIndex]; i++ )
		        {
			        byte cbIndex = (byte)((AnalyseResult.cbBlockCount[cbBlockIndex]-i-1)*(cbBlockIndex+1));
			        if( GetCardLogicValue(AnalyseResult.cbCardData[cbBlockIndex,cbIndex]) > cbReferLogicValue )
			        {

				        //复制扑克
                        for (byte c = 0; c < cbSameCardCount;c++ )
                        {
                            pSearchCardResult.cbResultCard[cbResultCount][c] = AnalyseResult.cbCardData[cbBlockIndex,(cbIndex+c)];
                        }
				        pSearchCardResult.cbCardCount[cbResultCount] = cbSameCardCount;

				        cbResultCount++;
			        }
		        }

		        cbBlockIndex++;
	        }while( cbBlockIndex < AnalyseResult.cbBlockCount.Length );


		    pSearchCardResult.cbSearchCount = cbResultCount;
	        return cbResultCount;
       }
       #endregion

        #region 连牌搜索
        public byte SearchLineCardType( byte[] cbHandCardData, byte cbHandCardCount, byte cbReferCard, byte cbBlockCount, byte cbLineCount,
									     ref tagSearchCardResult pSearchCardResult )
        {

            pSearchCardResult = new tagSearchCardResult(0);
	        byte cbResultCount = 0;

	        //定义变量
	        byte cbLessLineCount = 0;
	        if( cbLineCount == 0 )
	        {
		        if( cbBlockCount == 1 )
			        cbLessLineCount = 5;
		        else if( cbBlockCount == 2 )
			        cbLessLineCount = 3;
		        else cbLessLineCount = 2;
	        }
	        else cbLessLineCount = cbLineCount;

	        byte cbReferIndex = 2;
	        if( cbReferCard != 0 )
	        {
		        cbReferIndex = (byte)(GetCardLogicValue(cbReferCard)-cbLessLineCount+1);
	        }
	        //超过A
	        if( cbReferIndex+cbLessLineCount > 14 ) return cbResultCount;

	        //长度判断
	        if( cbHandCardCount < cbLessLineCount*cbBlockCount ) return cbResultCount;

	        //构造扑克
            byte[] cbCardData = new byte[MAX_COUNT];
	        byte cbCardCount=cbHandCardCount;
	        Array.Copy(cbHandCardData,cbCardData,cbHandCardCount);

	        //排列扑克
	        SortCardList(ref cbCardData,cbCardCount);

	        //分析扑克
	        tagDistributing Distributing = new tagDistributing(0);
	        AnalysebDistributing(cbCardData,cbCardCount,ref Distributing);

	        //搜索顺子
	        byte cbTmpLinkCount = 0;
            byte cbValueIndex = 0;
	        for (cbValueIndex=cbReferIndex;cbValueIndex<13;cbValueIndex++)
	        {
		        //继续判断
		        if ( Distributing.cbDistributing[cbValueIndex][cbIndexCount]<cbBlockCount )
		        {
			        if( cbTmpLinkCount < cbLessLineCount )
			        {
				        cbTmpLinkCount=0;
				        continue;
			        }
			        else cbValueIndex--;
		        }
		        else 
		        {
			        cbTmpLinkCount++;
			        //寻找最长连
			        if( cbLineCount == 0 ) continue;
		        }

		        if( cbTmpLinkCount >= cbLessLineCount )
		        {

			        //复制扑克
			        byte cbCount = 0;
			        for( byte cbIndex = (byte)(cbValueIndex+1-cbTmpLinkCount); cbIndex <= cbValueIndex; cbIndex++ )
			        {
				        byte cbTmpCount = 0;
				        for (byte cbColorIndex=0;cbColorIndex<4;cbColorIndex++)
				        {
					        for( byte cbColorCount = 0; cbColorCount < Distributing.cbDistributing[cbIndex][3-cbColorIndex]; cbColorCount++ )
					        {
						        pSearchCardResult.cbResultCard[cbResultCount][cbCount++]=MakeCardData(cbIndex,(byte)(3-cbColorIndex));

						        if( ++cbTmpCount == cbBlockCount ) break;
					        }
					        if( cbTmpCount == cbBlockCount ) break;
				        }
			        }

			        //设置变量
			        pSearchCardResult.cbCardCount[cbResultCount] = cbCount;
			        cbResultCount++;

			        if( cbLineCount != 0 )
			        {
				        cbTmpLinkCount--;
			        }
			        else 
			        {
				        cbTmpLinkCount = 0;
			        }
		        }
	        }

	        //特殊顺子
	        if( cbTmpLinkCount >= cbLessLineCount-1 && cbValueIndex == 13 )
	        {
		        if( Distributing.cbDistributing[0][cbIndexCount] >= cbBlockCount ||
			        cbTmpLinkCount >= cbLessLineCount )
		        {


			        //复制扑克
			        byte cbCount = 0;
			        byte cbTmpCount = 0;
			        for( byte cbIndex = (byte)(cbValueIndex-cbTmpLinkCount); cbIndex < 13; cbIndex++ )
			        {
				        cbTmpCount = 0;
				        for (byte cbColorIndex=0;cbColorIndex<4;cbColorIndex++)
				        {
					        for( byte cbColorCount = 0; cbColorCount < Distributing.cbDistributing[cbIndex][3-cbColorIndex]; cbColorCount++ )
					        {
						        pSearchCardResult.cbResultCard[cbResultCount][cbCount++]=MakeCardData(cbIndex,(byte)(3-cbColorIndex));

						        if( ++cbTmpCount == cbBlockCount ) break;
					        }
					        if( cbTmpCount == cbBlockCount ) break;
				        }
			        }
			        //复制A
			        if( Distributing.cbDistributing[0][cbIndexCount] >= cbBlockCount )
			        {
				        cbTmpCount = 0;
				        for (byte cbColorIndex=0;cbColorIndex<4;cbColorIndex++)
				        {
                            for (byte cbColorCount = 0; cbColorCount < Distributing.cbDistributing[0][3 - cbColorIndex]; cbColorCount++)
					        {
						        pSearchCardResult.cbResultCard[cbResultCount][cbCount++]=MakeCardData((byte)0,(byte)(3-cbColorIndex));

						        if( ++cbTmpCount == cbBlockCount ) break;
					        }
					        if( cbTmpCount == cbBlockCount ) break;
				        }
			        }

			        //设置变量
			        pSearchCardResult.cbCardCount[cbResultCount] = cbCount;
			        cbResultCount++;
		        }
	        }

		    pSearchCardResult.cbSearchCount = cbResultCount;
	        return cbResultCount;
        }
        #endregion

        #region 带牌类型搜索
        byte SearchTakeCardType(byte[] cbHandCardData, byte cbHandCardCount, byte cbReferCard, byte cbSameCount, byte cbTakeCardCount, 
									        ref tagSearchCardResult pSearchCardResult )
        {
            pSearchCardResult = new tagSearchCardResult(0);
	        byte cbResultCount = 0;

	        //效验
	        if( cbSameCount != 3 && cbSameCount != 4 )
		        return cbResultCount;
	        if( cbTakeCardCount != 1 && cbTakeCardCount != 2 )
		        return cbResultCount;

	        //长度判断
	        if( cbSameCount == 4 && cbHandCardCount<cbSameCount+cbTakeCardCount*2 ||
		        cbHandCardCount < cbSameCount+cbTakeCardCount )
		        return cbResultCount;

	        //构造扑克
	        byte[] cbCardData = new byte[MAX_COUNT];
	        byte cbCardCount=cbHandCardCount;
	        Array.Copy(cbHandCardData,cbCardData,cbHandCardCount);

	        //排列扑克
	        SortCardList(ref cbCardData,cbCardCount);

	        //搜索同张
	        tagSearchCardResult SameCardResult = new tagSearchCardResult(0);
	        byte cbSameCardResultCount = SearchSameCard( cbCardData,cbCardCount,cbReferCard,cbSameCount,ref SameCardResult );

	        if( cbSameCardResultCount > 0 )
	        {
		        //分析扑克
		        tagAnalyseResult AnalyseResult = new tagAnalyseResult(0);
		        AnalysebCardData(cbCardData,cbCardCount,ref AnalyseResult);

		        //需要牌数
		        byte cbNeedCount = (byte)(cbSameCount+cbTakeCardCount);
		        if( cbSameCount == 4 ) cbNeedCount += cbTakeCardCount;

		        //提取带牌
		        for( byte i = 0; i < cbSameCardResultCount; i++ )
		        {
			        bool bMerge = false;

			        for( byte j = (byte)(cbTakeCardCount-1); j < AnalyseResult.cbBlockCount.Length; j++ )
			        {
				        for( byte k = 0; k < AnalyseResult.cbBlockCount[j]; k++ )
				        {
					        //从小到大
					        byte cbIndex = (byte)((AnalyseResult.cbBlockCount[j]-k-1)*(j+1));

					        //过滤相同牌
					        if( GetCardValue(SameCardResult.cbResultCard[i][0]) ==
						        GetCardValue(AnalyseResult.cbCardData[j,cbIndex]) )
						        continue;

					        //复制带牌
					        byte cbCount = SameCardResult.cbCardCount[i];
                            for (byte c = 0; c < cbTakeCardCount;c++ )
                            {
                                SameCardResult.cbResultCard[i][cbCount + c] = AnalyseResult.cbCardData[j, cbIndex + c];
                            }
					        SameCardResult.cbCardCount[i] += cbTakeCardCount;

					        if( SameCardResult.cbCardCount[i] < cbNeedCount ) continue;

					        //复制结果
                            Array.Copy(SameCardResult.cbResultCard[i], pSearchCardResult.cbResultCard[cbResultCount],
						                SameCardResult.cbCardCount[i] );
					        pSearchCardResult.cbCardCount[cbResultCount] = SameCardResult.cbCardCount[i];
					        cbResultCount++;

					        bMerge = true;

					        //下一组合
					        break;
				        }

				        if( bMerge ) break;
			        }
		        }
	        }

		    pSearchCardResult.cbSearchCount = cbResultCount;
	        return cbResultCount;
        }
        #endregion

        #region 搜索飞机
        byte SearchThreeTwoLine( byte[] cbHandCardData, byte cbHandCardCount,ref tagSearchCardResult pSearchCardResult )
        {
	        //设置结果
            pSearchCardResult = new tagSearchCardResult(0);

	        //变量定义
	        tagSearchCardResult tmpSearchResult = new tagSearchCardResult(0);
	        tagSearchCardResult tmpSingleWing = new tagSearchCardResult(0);
	        tagSearchCardResult tmpDoubleWing = new tagSearchCardResult(0);
	        byte cbTmpResultCount = 0;

	        //搜索连牌
	        cbTmpResultCount = SearchLineCardType( cbHandCardData,cbHandCardCount,0,3,0,ref tmpSearchResult );

	        if( cbTmpResultCount > 0 )
	        {
		        //提取带牌
		        for( byte i = 0; i < cbTmpResultCount; i++ )
		        {
			        //变量定义
			        byte[] cbTmpCardData = new byte[MAX_COUNT];
			        byte cbTmpCardCount = cbHandCardCount;

			        //不够牌
			        if( cbHandCardCount-tmpSearchResult.cbCardCount[i] < tmpSearchResult.cbCardCount[i]/3 )
			        {
				        byte cbNeedDelCount = 3;
				        while( cbHandCardCount+cbNeedDelCount-tmpSearchResult.cbCardCount[i] < (tmpSearchResult.cbCardCount[i]-cbNeedDelCount)/3 )
					        cbNeedDelCount += 3;
				        //不够连牌
				        if( (tmpSearchResult.cbCardCount[i]-cbNeedDelCount)/3 < 2 )
				        {
					        //废除连牌
					        continue;
				        }

				        //拆分连牌
				        RemoveCard( tmpSearchResult.cbResultCard[i],cbNeedDelCount,ref tmpSearchResult.cbResultCard[i],
					        tmpSearchResult.cbCardCount[i] );
				        tmpSearchResult.cbCardCount[i] -= cbNeedDelCount;
			        }


			        //删除连牌
			        Array.Copy( cbHandCardData,cbTmpCardData,cbHandCardCount );
			        RemoveCard( tmpSearchResult.cbResultCard[i],tmpSearchResult.cbCardCount[i],
				        ref cbTmpCardData,cbTmpCardCount );
			        cbTmpCardCount -= tmpSearchResult.cbCardCount[i];

			        //组合飞机
			        byte cbNeedCount = (byte)(tmpSearchResult.cbCardCount[i]/3);

			        byte cbResultCount = tmpSingleWing.cbSearchCount++;
			        Array.Copy( tmpSearchResult.cbResultCard[i],tmpSingleWing.cbResultCard[cbResultCount],
				            tmpSearchResult.cbCardCount[i] );
                    for(byte c = 0;c < cbNeedCount;c++)
                    {
                        tmpSingleWing.cbResultCard[cbResultCount][tmpSearchResult.cbCardCount[i] + c] = cbTmpCardData[cbTmpCardCount-cbNeedCount + c];
                    }
			        tmpSingleWing.cbCardCount[i] = (byte)(tmpSearchResult.cbCardCount[i]+cbNeedCount);

			        //不够带翅膀
			        if( cbTmpCardCount < tmpSearchResult.cbCardCount[i]/3*2 )
			        {
				        byte cbNeedDelCount = 3;
				        while( cbTmpCardCount+cbNeedDelCount-tmpSearchResult.cbCardCount[i] < (tmpSearchResult.cbCardCount[i]-cbNeedDelCount)/3*2 )
					        cbNeedDelCount += 3;
				        //不够连牌
				        if( (tmpSearchResult.cbCardCount[i]-cbNeedDelCount)/3 < 2 )
				        {
					        //废除连牌
					        continue;
				        }

				        //拆分连牌
				        RemoveCard( tmpSearchResult.cbResultCard[i],cbNeedDelCount,ref tmpSearchResult.cbResultCard[i],
					        tmpSearchResult.cbCardCount[i] );
				        tmpSearchResult.cbCardCount[i] -= cbNeedDelCount;

				        //重新删除连牌
				        Array.Copy( cbHandCardData,cbTmpCardData,cbHandCardCount );
				        RemoveCard( tmpSearchResult.cbResultCard[i],tmpSearchResult.cbCardCount[i],
					       ref cbTmpCardData,cbTmpCardCount ) ;
				        cbTmpCardCount = (byte)(cbHandCardCount-tmpSearchResult.cbCardCount[i]);
			        }

			        //分析牌
			        tagAnalyseResult  TmpResult = new tagAnalyseResult(0);
			        AnalysebCardData( cbTmpCardData,cbTmpCardCount,ref TmpResult );

			        //提取翅膀
			        byte[] cbDistillCard= new byte[MAX_COUNT];
			        byte cbDistillCount = 0;
			        byte cbLineCount = (byte)(tmpSearchResult.cbCardCount[i]/3);
			        for( byte j = 1; j < TmpResult.cbBlockCount.Length; j++ )
			        {
				        if( TmpResult.cbBlockCount[j] > 0 )
				        {
					        if( j+1 == 2 && TmpResult.cbBlockCount[j] >= cbLineCount )
					        {
						        byte cbTmpBlockCount = TmpResult.cbBlockCount[j];
                                cbDistillCount = (byte)((j + 1) * cbLineCount);
                                for (byte c = 0; c < cbDistillCount; c++)
                                {
                                    cbDistillCard[c] = TmpResult.cbCardData[j,(cbTmpBlockCount - cbLineCount) * (j + 1)];
                                }
						        
						        break;
					        }
					        else
					        {
						        for( byte k = 0; k < TmpResult.cbBlockCount[j]; k++ )
						        {
							        byte cbTmpBlockCount = TmpResult.cbBlockCount[j];
                                    cbDistillCard[cbDistillCount] = TmpResult.cbCardData[j,(cbTmpBlockCount-k-1)*(j+1)];
                                    cbDistillCard[cbDistillCount+1] = TmpResult.cbCardData[j,(cbTmpBlockCount-k-1)*(j+1)+1];
							        cbDistillCount += 2;

							        //提取完成
							        if( cbDistillCount == 2*cbLineCount ) break;
						        }
					        }
				        }

				        //提取完成
				        if( cbDistillCount == 2*cbLineCount ) break;
			        }

			        //提取完成
			        if( cbDistillCount == 2*cbLineCount )
			        {
				        //复制翅膀
				        cbResultCount = tmpDoubleWing.cbSearchCount++;
                        Array.Copy(tmpSearchResult.cbResultCard[i], tmpDoubleWing.cbResultCard[cbResultCount],
					                tmpSearchResult.cbCardCount[i] );

                        for (byte c = 0; c < cbDistillCount; c++)
                        {
                            tmpDoubleWing.cbResultCard[cbResultCount][tmpSearchResult.cbCardCount[i]+c] = cbDistillCard[c];
                        }
				        tmpDoubleWing.cbCardCount[i] = (byte)(tmpSearchResult.cbCardCount[i]+cbDistillCount);
			        }
		        }

		        //复制结果
		        for( byte i = 0; i < tmpDoubleWing.cbSearchCount; i++ )
		        {
			        byte cbResultCount = pSearchCardResult.cbSearchCount++;

                    Array.Copy(tmpDoubleWing.cbResultCard[i], pSearchCardResult.cbResultCard[cbResultCount],
				                tmpDoubleWing.cbCardCount[i] );
			        pSearchCardResult.cbCardCount[cbResultCount] = tmpDoubleWing.cbCardCount[i];
		        }
		        for( byte i = 0; i < tmpSingleWing.cbSearchCount; i++ )
		        {
			        byte cbResultCount = pSearchCardResult.cbSearchCount++;

                    Array.Copy(pSearchCardResult.cbResultCard[cbResultCount], tmpSingleWing.cbResultCard[i],
				            tmpSingleWing.cbCardCount[i] );
			        pSearchCardResult.cbCardCount[cbResultCount] = tmpSingleWing.cbCardCount[i];
		        }
	        }

	        return pSearchCardResult.cbSearchCount;
        }
        #endregion

    }
}
