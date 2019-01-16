using System;
using System.Collections.Generic;
using System.Text;


namespace JourneyToTheWest
{

    /// <summary>
    /// 水浒传逻辑接口
    /// </summary>
    public partial class JourneyToTheWest_
    {

        struct PayItem
        {
          public byte game_icon;      // 图标类型
          public int line;           // game_result_icons[3][5]中的行[3]
          public int row;            // game_result_icons[3][5]中的列[5]

          bool AndSelf(PayItem other)
          {
              return ((line < other.line) || ((line == other.line) && (row < other.row)));
          }
        }
        struct PayLineInfo
        {
            public int number;  // -1 不显示中奖线
            public long win_score;
            PayItem[] pay_line;
        };

        void AnalysisWinningLines(byte[][] game_result_icons, long bet_score, ref PayLineInfo[] output_result,ref int bonus_game_times, ref byte max_icon, ref int all_same_icon)
        {
            
        }
    }
}
