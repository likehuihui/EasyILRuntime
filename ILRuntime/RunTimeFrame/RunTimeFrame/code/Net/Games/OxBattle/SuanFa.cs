using System;
using System.Collections.Generic;
using System.Text;


namespace OxBattle
{
    public partial class OxBattle_
    {
        ////////////////////////////code by kingsley  10/9/2016 /////////////////////////////////////////////
        public const byte CARD_COUNT = 54;						//扑克数目

        public const byte ST_VALUE = 1;                      //数值排序
        public const byte ST_NEW = 3;                      //数值排序
        public const byte ST_LOGIC = 2;                      //数值排序

        //
        public const byte LOGIC_MASK_COLOR = 0xF0;                       //花色掩码
        public const byte LOGIC_MASK_VALUE = 0x0F;                      //数值掩码

        public const byte OX_CARD_NUM = 5;						//扑克数目
        //牌型
        public enum emCardType
        {
            CT_ERROR = 0,								//错误类型
            CT_POINT = 1,								//点数类型
            CT_SPECIAL_NIU1 = 3,								//特殊类型
            CT_SPECIAL_NIU2 = 4,								//特殊类型
            CT_SPECIAL_NIU3 = 5,								//特殊类型
            CT_SPECIAL_NIU4 = 6,								//特殊类型
            CT_SPECIAL_NIU5 = 7,								//特殊类型
            CT_SPECIAL_NIU6 = 8,								//特殊类型
            CT_SPECIAL_NIU7 = 9,								//特殊类型
            CT_SPECIAL_NIU8 = 10,								//特殊类型
            CT_SPECIAL_NIU9 = 11,								//特殊类型
            CT_SPECIAL_NIUNIU = 12,								//特殊类型
            CT_SPECIAL_NIUNIUXW = 13,								//特殊类型
            CT_SPECIAL_NIUNIUDW = 14,								//特殊类型
            CT_SPECIAL_NIUYING = 15,								//特殊类型
            CT_SPECIAL_NIUKING = 16,								//特殊类型
            CT_SPECIAL_BOMEBOME = 17								//特殊类型

        };

        public  byte[] m_cbCardListData = new byte[CARD_COUNT]     
        {
		    0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,	//方块 A - K
		    0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1A,0x1B,0x1C,0x1D,	//梅花 A - K
		    0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,	//红桃 A - K
		    0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3A,0x3B,0x3C,0x3D,	//黑桃 A - K
		    0x41,0x42,
        };


       
        /////////////////////////////////////////////////////////////////////////////////////////    

        //类型函数


        #region 类型函数

        //获取数值
        public byte GetCardValue(byte cbCardData)
        {
            return (byte)(cbCardData & LOGIC_MASK_VALUE);
        }


        public byte GetCardNewValue(byte cbCardData)
        {
	        //扑克属性
	        byte cbCardColor=GetCardColor(cbCardData);
	        byte cbCardValue=GetCardValue(cbCardData);

	        //转换数值
	        if (cbCardColor==0x04) return (byte)(cbCardValue+13+2);
	        return cbCardValue;

        }
        //逻辑大小
        public byte GetCardLogicValue(byte cbCardData)
        {
	        byte cbValue=GetCardValue(cbCardData);

	        //获取花色
	        byte cbColor=GetCardColor(cbCardData);

	        if(cbValue>10)
	        {
		        cbValue = 10;

	        }
	        if(cbColor==0x4)
	        {
		        return 11;
	        }
	        return cbValue;
        }

        //获取花色
        public byte GetCardColor(byte cbCardData)
        {
            return (byte)((cbCardData & LOGIC_MASK_COLOR) >> 4);
        }

        #endregion

        #region 控制函数
         /// <summary>
        /// 排列扑克
        /// </summary>
        /// <param name="cbCardData"></param>
        public void SortCardList(ref byte[] cbCardData, byte cbCardCount, byte cbSortType)
        {
            //转换数值
            byte[] cbLogicValue = new byte[cbCardCount];

            for (byte i = 0; i < cbCardCount; i++)
            {
                if (cbSortType == ST_VALUE)
                    cbLogicValue[i] = GetCardValue(cbCardData[i]);
                else if (cbSortType == ST_NEW)
                    cbLogicValue[i] = GetCardNewValue(cbCardData[i]);
                else 
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


        public emCardType RetType(int itype)
        {
            itype = itype % 10;
            switch (itype)
            {
                case 0:
                    return emCardType.CT_SPECIAL_NIUNIU;
                case 1:
                    return emCardType.CT_SPECIAL_NIU1;
                    break;
                case 2:
                    return emCardType.CT_SPECIAL_NIU2;
                    break;
                case 3:
                    return emCardType.CT_SPECIAL_NIU3;
                    break;
                case 4:
                    return emCardType.CT_SPECIAL_NIU4;
                    break;
                case 5:
                    return emCardType.CT_SPECIAL_NIU5;
                    break;
                case 6:
                    return emCardType.CT_SPECIAL_NIU6;
                    break;
                case 7:
                    return emCardType.CT_SPECIAL_NIU7;
                    break;
                case 8:
                    return emCardType.CT_SPECIAL_NIU8;
                    break;
                case 9:
                    return emCardType.CT_SPECIAL_NIU9;
                    break;
                default:
                    return emCardType.CT_POINT;
                    break;
            }

        }

    public int CompareCard( byte[] cbFirstCardData, byte cbFirstCardCount, byte[] cbNextCardData, byte cbNextCardCount,ref byte Multiple)
    {
	    //合法判断
	   
	    if (!(5==cbFirstCardCount && 5==cbNextCardCount)) return 0;

        byte[]  outcard =new byte[OX_CARD_NUM];
	    //获取牌型
	    emCardType cbFirstCardType=GetCardType(cbFirstCardData, cbFirstCardCount,ref outcard,0);
	    emCardType cbNextCardType=GetCardType(cbNextCardData, cbNextCardCount,ref outcard,0);

	    //牌型比较
	    if (cbFirstCardType != cbNextCardType) 
	    {
		    if (cbNextCardType > cbFirstCardType)
		    {
			    if((byte)cbNextCardType>=12)
			    {
				    Multiple = 10;

			    }else
			    {
				    Multiple = (byte)(cbNextCardType-2);

			    }
			    return 1;

		    }
		    else
		    {
			    if((byte)cbFirstCardType>=12)
			    {
				    Multiple = 10;

			    }else
			    {
				    Multiple = (byte)(cbFirstCardType-2);

			    }
			    return -1;
		    }
	    }

	    //特殊牌型判断
	    if (emCardType.CT_POINT!=cbFirstCardType && cbFirstCardType==cbNextCardType)
	    {
		    //排序扑克
		    byte[] cbFirstCardDataTmp =new byte[OX_CARD_NUM];
            byte[] cbNextCardDataTmp =new byte[OX_CARD_NUM];
               
            Array.Copy(cbFirstCardData,cbFirstCardDataTmp,cbFirstCardCount);	
            Array.Copy(cbNextCardData,cbNextCardDataTmp,cbNextCardCount);	

	
		    SortCardList(ref cbFirstCardDataTmp,cbFirstCardCount,ST_NEW);
		    SortCardList(ref cbNextCardDataTmp,cbNextCardCount,ST_NEW);

		    if((byte)cbFirstCardType>=12)
		    {
			    Multiple = 10;

		    }else
		    {
			    Multiple = (byte)(cbFirstCardType-2);

		    }
		    byte firstValue = GetCardNewValue(cbFirstCardDataTmp[0]);
		    byte NextValue = GetCardNewValue(cbNextCardDataTmp[0]);

		    byte firstColor = GetCardColor(cbFirstCardDataTmp[0]);

		    byte NextColor = GetCardColor(cbNextCardDataTmp[0]);


		    if(firstValue<NextValue)
		    {
			    return 1;
		    }else
		    {
			    if(firstValue==NextValue)
			    {
				    if(firstColor<NextColor)
				    {
					    return 1;

				    }else
				    {
					    return -1;
				    }
			    }
			    return -1;
		    }

	    }

	    //排序扑克
	   

        byte[] cbFirstCardDataTmp1 =new byte[OX_CARD_NUM];
        byte[] cbNextCardDataTmp1 =new byte[OX_CARD_NUM];


         Array.Copy(cbFirstCardData,cbFirstCardDataTmp1 , cbFirstCardCount);
         Array.Copy(cbNextCardData,cbNextCardDataTmp1 , cbNextCardCount);

	
	    SortCardList(ref cbFirstCardDataTmp1,cbFirstCardCount,ST_NEW);
	    SortCardList(ref cbNextCardDataTmp1,cbNextCardCount,ST_NEW);

	    byte firstValue1 = GetCardNewValue(cbFirstCardDataTmp1[0]);
	    byte NextValue1 = GetCardNewValue(cbNextCardDataTmp1[0]);
	    byte firstColor1 = GetCardColor(cbFirstCardDataTmp1[0]);
	    byte NextColor1 = GetCardColor(cbNextCardDataTmp1[0]);

	    if(firstValue1<NextValue1)
	    {
		    return 1;
	    }else
	    {
		    if(firstValue1==NextValue1)
		    {
			    if(firstColor1<NextColor1)
			    {
				    return 1;

			    }else
			    {
				    return -1;
			    }
		    }
		    return -1;
	    }

    }



        //获取牌型
    public emCardType GetCardType( byte[] cbCardData, byte cbCardCount,ref byte[] bcOutCadData , byte cbOutCadCount=0)
{
	//合法判断
	
	if (5!=cbCardCount) return emCardType.CT_ERROR;

	//排序扑克
	byte[] cbCardDataSort = new byte[OX_CARD_NUM];
    Array.Copy(cbCardData,cbCardDataSort,cbCardCount);
	
	SortCardList(ref cbCardDataSort,cbCardCount,ST_NEW);

	if(cbOutCadCount != 0)
	{
        Array.Copy(cbCardDataSort,bcOutCadData,cbOutCadCount);		
	}

	int igetW= 0;


	if(GetCardNewValue(cbCardDataSort[0])==GetCardNewValue(cbCardDataSort[cbCardCount-2]))
	{
		if(cbOutCadCount != 0)
		{
            Array.Copy(cbCardDataSort, bcOutCadData, cbOutCadCount);	
		}
		return emCardType.CT_SPECIAL_BOMEBOME;
	}else
	{
		if(GetCardNewValue(cbCardDataSort[1])==GetCardNewValue(cbCardDataSort[cbCardCount-1]))
		{
			if(cbOutCadCount != 0)
		    {
                Array.Copy(cbCardDataSort, bcOutCadData, cbOutCadCount);		
		    }
			return emCardType.CT_SPECIAL_BOMEBOME;
		}

	}
	if(GetCardColor(cbCardDataSort[0])==0x04&&GetCardColor(cbCardDataSort[1])==0x04)
	{
		if(GetCardNewValue(cbCardDataSort[2])==GetCardNewValue(cbCardDataSort[3]))
		{
			if(cbOutCadCount != 0)
			{
				bcOutCadData[0] = cbCardDataSort[2];
				bcOutCadData[1] = cbCardDataSort[3];
				bcOutCadData[2] = cbCardDataSort[0];
				bcOutCadData[3] = cbCardDataSort[1];
				bcOutCadData[4] = cbCardDataSort[4];

			}
			return emCardType.CT_SPECIAL_BOMEBOME;
		}else
		{
			if(GetCardNewValue(cbCardDataSort[3])==GetCardNewValue(cbCardDataSort[4]))
			{
				if(cbOutCadCount != 0)
				{
					bcOutCadData[0] = cbCardDataSort[0];
					bcOutCadData[1] = cbCardDataSort[1];
					bcOutCadData[2] = cbCardDataSort[3];
					bcOutCadData[3] = cbCardDataSort[4];
					bcOutCadData[4] = cbCardDataSort[2];

				}
				return emCardType.CT_SPECIAL_BOMEBOME;
			}

		}	

	}
	if(GetCardColor(cbCardDataSort[0])==0x04)
	{
		if(GetCardNewValue(cbCardDataSort[1])==GetCardNewValue(cbCardDataSort[3]))
		{
			if(cbOutCadCount != 0)
			{
				bcOutCadData[0] = cbCardDataSort[1];
				bcOutCadData[1] = cbCardDataSort[2];
				bcOutCadData[2] = cbCardDataSort[3];
				bcOutCadData[3] = cbCardDataSort[0];
				bcOutCadData[4] = cbCardDataSort[4];

			}
			return emCardType.CT_SPECIAL_BOMEBOME;
		}else
		{
			if(GetCardNewValue(cbCardDataSort[2])==GetCardNewValue(cbCardDataSort[4]))
			{
				if(cbOutCadCount != 0)
				{
					bcOutCadData[0] = cbCardDataSort[2];
					bcOutCadData[1] = cbCardDataSort[3];
					bcOutCadData[2] = cbCardDataSort[4];
					bcOutCadData[3] = cbCardDataSort[0];
					bcOutCadData[4] = cbCardDataSort[1];

				}
				return emCardType.CT_SPECIAL_BOMEBOME;
			}

		}	

	}
	bool blBig = true;
	int iCount = 0;
	int iLogicValue = 0;
	int iValueTen = 0;
	for (int i = 0;i<cbCardCount;i++)
	{
		byte bcGetValue = GetCardLogicValue(cbCardDataSort[i]);
		if(bcGetValue!=10&&bcGetValue!=11)
		{

			blBig = false;
			break;

		}else
		{
			if(GetCardNewValue(cbCardDataSort[i])==10)
			{
				iValueTen++;
			}
		}
		iCount++;
	}
	if(blBig)
	{
		if(cbOutCadCount != 0)
		{
            Array.Copy(cbCardDataSort, bcOutCadData, cbOutCadCount);			
		}
		if(iValueTen==0)
			return emCardType.CT_SPECIAL_NIUKING;
		else
		{
			if(iValueTen==1)
			{
				return emCardType.CT_SPECIAL_NIUYING;
			}
		}
	}

	int n = 0;

	byte[] bcMakeMax =new byte[OX_CARD_NUM];
	
	int iBigValue = 0;
	byte[] iSingleA =new byte[2];
	int iIndex = 0;
	bcMakeMax[0]= cbCardDataSort[n];

	int iGetTenCount = 0;

	for (int iten = 0;iten<cbCardCount;iten++)
	{
		if(GetCardLogicValue(cbCardDataSort[iten])==10||GetCardLogicValue(cbCardDataSort[iten])==11)
		{
			iGetTenCount++;

		}
	}
	if( iGetTenCount>=3)
	{
		if(GetCardColor(cbCardDataSort[0])==0x04&&GetCardColor(cbCardDataSort[1])==0x04)
		{
			if(cbOutCadCount != 0)
			{
				bcOutCadData[0] = cbCardDataSort[0];
				bcOutCadData[1] = cbCardDataSort[3];
				bcOutCadData[2] = cbCardDataSort[4];
				bcOutCadData[3] = cbCardDataSort[1];
				bcOutCadData[4] = cbCardDataSort[2];

			}
			return emCardType.CT_SPECIAL_NIUNIUDW;

		}
		if(GetCardColor(cbCardDataSort[0])==0x04)
		{
			//大小王与最小的组合成牛 
			if(cbOutCadCount != 0)
			{
				bcOutCadData[0] = cbCardDataSort[0];
				bcOutCadData[1] = cbCardDataSort[3];
				bcOutCadData[2] = cbCardDataSort[4];
				bcOutCadData[3] = cbCardDataSort[1];
				bcOutCadData[4] = cbCardDataSort[2];
			}
			if(cbCardDataSort[0]==0x42)
				return emCardType.CT_SPECIAL_NIUNIUDW;
			else
				return emCardType.CT_SPECIAL_NIUNIUXW;
		}else
		{
			return RetType(GetCardLogicValue(cbCardDataSort[3])+GetCardLogicValue(cbCardDataSort[4]));
		}

	}
	if(iGetTenCount==2||(iGetTenCount==1&&GetCardColor(cbCardDataSort[0])==0x04))
	{

		if(GetCardColor(cbCardDataSort[0])==0x04&&GetCardColor(cbCardDataSort[1])==0x04)
		{
			if(cbOutCadCount != 0)
			{
				bcOutCadData[0] = cbCardDataSort[0];
				bcOutCadData[1] = cbCardDataSort[3];
				bcOutCadData[2] = cbCardDataSort[4];
				bcOutCadData[3] = cbCardDataSort[1];
				bcOutCadData[4] = cbCardDataSort[2];
			}
			return emCardType.CT_SPECIAL_NIUNIUDW;
		}else
		{
			//如果有一张王 其他任意三张组合为10则是牛牛
			if(GetCardColor(cbCardDataSort[0])==0x04)
			{

				for ( n=1;n<cbCardCount;n++)
				{
					for (int j = 1;j<cbCardCount;j++)
					{
						if(j != n)
						{
							for (int w = 1;w<cbCardCount;w++)
							{
								if(w != n&&w!=j)
								{
									//如果剩余的四张中任意三张能组合位10的整数倍

									if((GetCardLogicValue(cbCardDataSort[n])+GetCardLogicValue(cbCardDataSort[j])+GetCardLogicValue(cbCardDataSort[w]))%10==0)
									{

										int add = 0;
										for (int y = 1;y<cbCardCount;y++)
										{
											if(y != n&&y!=j&&y!=w)
											{
												iSingleA[add] =cbCardDataSort[y]; 
												add++;

											}

										}
										if(cbOutCadCount != 0)
										{
											bcOutCadData[0] = cbCardDataSort[n];
											bcOutCadData[1] = cbCardDataSort[j];
											bcOutCadData[2] = cbCardDataSort[w];
											bcOutCadData[3] = cbCardDataSort[0];
											bcOutCadData[4] = iSingleA[0];
										}
										if(cbCardDataSort[0]==0x42)
											return emCardType.CT_SPECIAL_NIUNIUDW;
										else
											return emCardType.CT_SPECIAL_NIUNIUXW;


									}
								}
							}
						}
					}
				}
				//如果有一张王 其他任意三张组合不为10则 取两张点数最大的组合
				byte[] bcTmp = new byte[4];
				int iBig = 0;
				int ins = 0;
				for ( ins = 1;ins<cbCardCount;ins++)
				{
					for (int j = 1;j<cbCardCount;j++)
					{
						if(ins != j)
						{
							byte bclogic = (byte)((GetCardLogicValue(cbCardDataSort[ins])+GetCardLogicValue(cbCardDataSort[j]))%10);
							if(bclogic>iBig)
							{
								iBig = bclogic;
								int add = 0;
								bcTmp[0]=cbCardDataSort[ins];
								bcTmp[1]=cbCardDataSort[j];
								for (int y = 1;y<cbCardCount;y++)
								{
									if(y != ins&&y!=j)
									{
										iSingleA[add] =cbCardDataSort[y]; 
										add++;
									}

								}
								bcTmp[2]=iSingleA[0];
								bcTmp[3]=iSingleA[1];

							}

						}
					}   
				}

				if(cbOutCadCount != 0)
				{
					bcOutCadData[0] = cbCardDataSort[0];
					bcOutCadData[1] = bcTmp[2];
					bcOutCadData[2] = bcTmp[3];
					bcOutCadData[3] = bcTmp[0];
					bcOutCadData[4] = bcTmp[1];
				}
				if(iGetTenCount==1&&GetCardColor(cbCardDataSort[0])==0x04)
				{
					//下面还能组合 有两张为 10 也可以组合成牛牛

				}else
				{
					//如果没有则比较 完与最小组合最大点数和组合
					return RetType(GetCardLogicValue(bcTmp[0])+GetCardLogicValue(bcTmp[1]));
				}


			}else
			{
				if((GetCardLogicValue(cbCardDataSort[2])+GetCardLogicValue(cbCardDataSort[3])+GetCardLogicValue(cbCardDataSort[4]))%10==0)
				{
					if(cbOutCadCount != 0)
					{
						bcOutCadData[0] = cbCardDataSort[2];
						bcOutCadData[1] = cbCardDataSort[3];
						bcOutCadData[2] = cbCardDataSort[4];
						bcOutCadData[3] = cbCardDataSort[0];
						bcOutCadData[4] = cbCardDataSort[1];
					}
					return emCardType.CT_SPECIAL_NIUNIU;
				}else
				{
					for ( n= 2;n<cbCardCount;n++)
					{
						for (int j = 2;j<cbCardCount;j++)
						{
							if(j != n)
							{
								if((GetCardLogicValue(cbCardDataSort[n])+GetCardLogicValue(cbCardDataSort[j]))%10==0)
								{
									int add = 0;
									for (int y = 2;y<cbCardCount;y++)
									{
										if(y != n&&y!=j)
										{
											iSingleA[add] =cbCardDataSort[y]; 
											add++;

										}
									}
									if(iBigValue<=iSingleA[0]%10)
									{
										iBigValue = GetCardLogicValue(iSingleA[0])%10;
										if(cbOutCadCount != 0)
										{
											bcOutCadData[0]= cbCardDataSort[0];
											bcOutCadData[1]= cbCardDataSort[n]; 
											bcOutCadData[2]= cbCardDataSort[j]; 
											bcOutCadData[3]= cbCardDataSort[1];
											bcOutCadData[4]= iSingleA[0]; 

										}

										if(iBigValue==0)
										{

											return emCardType.CT_SPECIAL_NIUNIU;
										}
									}

								}
							}
						}
					}
					if(iBigValue != 0)
					{
						return RetType(iBigValue);
					}
				}
			}

		}

		iGetTenCount = 1;

	}
	//4个组合
	if(iGetTenCount==1)
	{
		if(GetCardColor(cbCardDataSort[0])==0x04)
		{
			for ( n= 1;n<cbCardCount;n++)
			{
				for (int j = 1;j<cbCardCount;j++)
				{
					if(j != n)
					{
						//任意两张组合成牛
						if((GetCardLogicValue(cbCardDataSort[n])+GetCardLogicValue(cbCardDataSort[j]))%10==0)
						{
							int add = 0;
							for (int y = 1;y<cbCardCount;y++)
							{
								if(y != n&&y!=j)
								{
									iSingleA[add] =cbCardDataSort[y]; 
									add++;

								}

							}

							if(cbOutCadCount != 0)
							{
								bcOutCadData[0] = cbCardDataSort[0];
								bcOutCadData[1] = iSingleA[0];
								bcOutCadData[2] = iSingleA[1];
								bcOutCadData[3] = cbCardDataSort[n];
								bcOutCadData[4] = cbCardDataSort[j];
							}
							if(cbCardDataSort[0]==0x42)
								return emCardType.CT_SPECIAL_NIUNIUDW;
							else
								return emCardType.CT_SPECIAL_NIUNIUXW;

						}
					}

				}
			}

			//取4张中组合最大的点数

			byte[] bcTmp =new byte[4];
			int iBig = 0;
			int inr = 0;
			for ( inr = 1;inr<cbCardCount;inr++)
			{
				for (int j = 1;j<cbCardCount;j++)
				{
					if(inr != j)
					{
						byte bclogic = (byte)((GetCardLogicValue(cbCardDataSort[inr])+GetCardLogicValue(cbCardDataSort[j]))%10);
						if(bclogic>iBig)
						{
							iBig = bclogic;
							int add = 0;
							bcTmp[0]=cbCardDataSort[inr];
							bcTmp[1]=cbCardDataSort[j];
							for (int y = 1;y<cbCardCount;y++)
							{
								if(y != inr&&y!=j)
								{
									iSingleA[add] =cbCardDataSort[y]; 
									add++;
								}

							}
							bcTmp[2]=iSingleA[0];
							bcTmp[3]=iSingleA[1];

						}

					}
				}   
			}

			if(cbOutCadCount != 0)
			{
				bcOutCadData[0] = cbCardDataSort[0];
				bcOutCadData[1] = bcTmp[2];
				bcOutCadData[2] = bcTmp[3];
				bcOutCadData[3] = bcTmp[0];
				bcOutCadData[4] = bcTmp[1];
			}
			return RetType(GetCardLogicValue(bcTmp[0])+GetCardLogicValue(bcTmp[1]));

		}
		//取4张中任两张组合为10 然后求另外两张的组合看是否是组合中最大
		for ( n= 1;n<cbCardCount;n++)
		{
			for (int j = 1;j<cbCardCount;j++)
			{
				if(j != n)
				{
					if((GetCardLogicValue(cbCardDataSort[n])+GetCardLogicValue(cbCardDataSort[j]))%10==0)
					{
						int add = 0;
						for (int y = 1;y<cbCardCount;y++)
						{
							if(y != n&&y!=j)
							{
								iSingleA[add] =cbCardDataSort[y]; 
								add++;

							}

						}
						if(iBigValue<=(GetCardLogicValue(iSingleA[0])+GetCardLogicValue(iSingleA[1]))%10)
						{
							iBigValue = GetCardLogicValue(iSingleA[0])+GetCardLogicValue(iSingleA[1])%10;
							bcMakeMax[0]= cbCardDataSort[0];
							bcMakeMax[1]= cbCardDataSort[j];
							bcMakeMax[2]= cbCardDataSort[n]; 
							bcMakeMax[3]= iSingleA[0]; 
							bcMakeMax[4]= iSingleA[1]; 

							if(cbOutCadCount != 0)
							{								
                                 Array.Copy(bcMakeMax,bcOutCadData,cbOutCadCount);	
							}
							if(iBigValue==0)
							{

								return emCardType.CT_SPECIAL_NIUNIU;
							}
						}

					}
				}
			}
		}
		if(iBigValue!= 0)
		{
			return RetType(iBigValue);
		}else
		{
			//如果组合不成功
			iGetTenCount = 0;
		}

	}
	if(iGetTenCount==0)
	{
		//5个组合
		for ( n= 0;n<cbCardCount;n++)
		{
			for (int j = 0;j<cbCardCount;j++)
			{
				if(j != n)
				{
					for (int w = 0;w<cbCardCount;w++)
					{
						if(w != n&&w!=j)
						{
							int valueAdd = GetCardLogicValue(cbCardDataSort[n]);
							valueAdd += GetCardLogicValue(cbCardDataSort[j]);
							valueAdd += GetCardLogicValue(cbCardDataSort[w]);

							if(valueAdd%10==0)
							{
								int add = 0;
								for (int y = 0;y<cbCardCount;y++)
								{
									if(y != n&&y!=j&&y!=w)
									{
										iSingleA[add] =cbCardDataSort[y]; 
										add++;

									}

								}
								if(iBigValue<=(GetCardLogicValue(iSingleA[0])+GetCardLogicValue(iSingleA[1]))%10)
								{
									iBigValue = GetCardLogicValue(iSingleA[0])+GetCardLogicValue(iSingleA[1])%10;
									bcMakeMax[0]= cbCardDataSort[n];
									bcMakeMax[1]= cbCardDataSort[j];
									bcMakeMax[2]= cbCardDataSort[w]; 
									bcMakeMax[3]= iSingleA[0]; 
									bcMakeMax[4]= iSingleA[1]; 

									if(cbOutCadCount != 0)
									{										
                                         Array.Copy(bcMakeMax,bcOutCadData,cbOutCadCount);	
									}
									if(iBigValue==0)
									{

										return emCardType.CT_SPECIAL_NIUNIU;
									}
								}

							}

						}
					}
				}
			}		
		}
		if(iBigValue!=0)
		{
			return RetType(iBigValue);
		}	
		else
		{
			return emCardType.CT_POINT;
		}

	}

	return emCardType.CT_POINT;


    }

        #endregion
    }
}
