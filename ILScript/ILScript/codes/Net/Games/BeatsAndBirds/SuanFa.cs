namespace BeatsAndBirds
{

    //配置索引
    public enum emAreaIndex
    {
        ID_TU_ZI = 1,           //兔子
        ID_YAN_ZI = 2,          //燕子
        ID_GE_ZI = 3,           //鸽子
        ID_KONG_QUE = 4,        //孔雀
        ID_LAO_YING = 5,        //老鹰
        ID_SHI_ZI = 6,          //狮子
        ID_XIONG_MAO = 7,       //熊猫
        ID_HOU_ZI = 8,          //猴子
        ID_SHA_YU_SLIVER = 9,       //银鲨鱼
        ID_SHA_YU_GOLD = 10,        //金鲨鱼
        ID_T_CHI = 11,              //通吃
        ID_T_PEI = 12,              //通陪
        ID_FEI_QIN = 13,            //飞禽
        ID_ZOU_SHOU = 14            //走兽
    }

    //下注区域
    public enum emBetIndex
    {
        A_ID_TUZI = 1,          //兔子
        A_ID_YANZI = 2,         //燕子
        A_ID_GEZI = 3,          //鸽子
        A_ID_KONGQUE = 4,       //孔雀
        A_ID_LAOYING = 5,       //老鹰
        A_ID_SHIZI = 6,         //狮子
        A_ID_XIONGMAO = 7,      //熊猫
        A_ID_HOUZI = 8,         //猴子
        A_ID_SHAYU = 9,         //鲨鱼
        A_ID_FEIQIN = 10,       //飞禽
        A_ID_ZOUSHOU = 11       //走兽
    }

    /// <summary>
    /// 飞禽走兽逻辑接口
    /// </summary>
    public partial class BeatsAndBirds_
    {
        public const int KIND_ID = 8804;
        public const int GAME_PLAYER = 100;			//游戏人数
        public const string GAME_NAME = "飞禽走兽";


    }
}
