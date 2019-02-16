using System;
using System.Collections.Generic;
using System.Text;


namespace DragonLegend
{
    public partial class DragonLegend_
    {
        ////////////////////////////code by kingsley  6/27/2016 /////////////////////////////////////////////
        public const byte TURAN_TABLE_MAX   = 28;						//转盘索引
        public const byte TURAN_TABLE_JUMP   = 30;                      //转盘跳转

        //
        public const byte ANIMAL_TYPE_NULL = 0;                         //无
        public const byte ANIMAL_TYPE_RED = 1;                      //红色动物
        public const byte ANIMAL_TYPE_GREEN = 2;                        //绿色动物
        public const byte ANIMAL_TYPE_SLIVER = 3;                       //金龙
        public const byte ANIMAL_TYPE_GOLD = 4;							//金龙


        //动物索引
        public enum Animal_type
        {
            ANIMAL_REDRABBIT = 0,   //红兔子
            ANIMAL_REDMONKEY,   //红猴子
            ANIMAL_REDPANDA,    //红熊猫
            ANIMAL_REDELEPHANT, //红大象
            ANIMAL_REDLION,     //红狮子
            ANIMAL_GREENLION,   //绿狮子
            ANIMAL_GREENELEPHANT,//绿大象
            ANIMAL_GREENPANDA,  //绿熊猫
            ANIMAL_GREENMONKEY, //绿猴子
            ANIMAL_GREENRABBIT, //绿兔子
            ANIMAL_GOLDDRAGON,  //金龙
            ANIMAL_RED,         //红色动物
            ANIMAL_GREEN,       //绿色动物
            ANIMAL_MAX,         //
            ANIMAL_SLIVERDRAGON//银龙
        }

        //判断类别
        public int AnimalType(Animal_type nAnimal)
        {
            switch (nAnimal)
            {
                case Animal_type.ANIMAL_REDLION:
                case Animal_type.ANIMAL_REDELEPHANT:
                case Animal_type.ANIMAL_REDPANDA:
                case Animal_type.ANIMAL_REDMONKEY:
                case Animal_type.ANIMAL_REDRABBIT:
                    return ANIMAL_TYPE_RED;
                case Animal_type.ANIMAL_GREENLION:
                case Animal_type.ANIMAL_GREENELEPHANT:
                case Animal_type.ANIMAL_GREENPANDA:
                case Animal_type.ANIMAL_GREENMONKEY:
                case Animal_type.ANIMAL_GREENRABBIT:
                    return ANIMAL_TYPE_GREEN;
                case Animal_type.ANIMAL_GOLDDRAGON:
                    return ANIMAL_TYPE_GOLD;
                default:
                    return ANIMAL_TYPE_NULL;
            }
        }

        //转盘中动物
        public Animal_type TurnTableAnimal(int nTableIndex)
        {
            if (nTableIndex >= TURAN_TABLE_MAX || nTableIndex < 0)
            {               
                return Animal_type.ANIMAL_MAX;
            }

            Animal_type[] AnimalSpecies =
            {
            Animal_type.ANIMAL_GOLDDRAGON,Animal_type.ANIMAL_SLIVERDRAGON,Animal_type.ANIMAL_REDLION,Animal_type.ANIMAL_REDELEPHANT,
            Animal_type.ANIMAL_REDPANDA, Animal_type.ANIMAL_REDMONKEY, Animal_type.ANIMAL_REDRABBIT, Animal_type.ANIMAL_GOLDDRAGON,
            Animal_type.ANIMAL_GREENLION,Animal_type.ANIMAL_GREENELEPHANT,Animal_type.ANIMAL_GREENPANDA,Animal_type.ANIMAL_GREENMONKEY,
            Animal_type.ANIMAL_GREENRABBIT,Animal_type.ANIMAL_SLIVERDRAGON,Animal_type.ANIMAL_GOLDDRAGON,Animal_type.ANIMAL_SLIVERDRAGON,
            Animal_type.ANIMAL_REDLION,Animal_type.ANIMAL_REDELEPHANT,Animal_type.ANIMAL_REDPANDA,Animal_type.ANIMAL_REDMONKEY,
            Animal_type.ANIMAL_REDRABBIT,Animal_type.ANIMAL_GOLDDRAGON,Animal_type.ANIMAL_GREENLION,Animal_type.ANIMAL_GREENELEPHANT,
            Animal_type.ANIMAL_GREENPANDA,Animal_type.ANIMAL_GREENMONKEY,Animal_type.ANIMAL_GREENRABBIT,Animal_type.ANIMAL_SLIVERDRAGON
            };

            return AnimalSpecies[nTableIndex];
        }

        //转盘中动物类型
        public int TurnTableAnimalType(int nTableIndex)
        {
            if (nTableIndex >= TURAN_TABLE_MAX || nTableIndex < 0)
            {               
                return ANIMAL_TYPE_NULL;
            }

            return AnimalType(TurnTableAnimal(nTableIndex));
        }

        /////////////////////////////////////////////////////////////////////////////////////////    
    }
}
