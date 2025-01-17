﻿using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.WebSockets;

namespace UnionLotto
{
    /*
     * 双色球：
     * 
     * 双色球是采用了数字密码组合4种形式中的最高级也是最复杂的密码组合（超大值＋移位＋替代）
     * 超大值：什么是超大值？就是每期我们买的彩票后，中心数据库的电脑开奖前自动统计出的风险最高的，也就是彩民买的最多的几个号。 
     * 移位：什么是移位？移位就是我们说的与上期同尾，比如这期开的21下期变11了。这就是移位红球1-33，篮球1-16
     * 替代：什么是替代？替代就是运用了合数的基础理论1变成了6，0变成了5。最简单的比方就是用隐形眼镜替换了有架眼镜，同样是眼镜。
     * 1＝6，2＝7，3＝8，4＝9，5＝0。  
     * 
     * 
     *    9进制           10进制
     * 01 10 19 28;     01 12 23；
     * 02 11 20 29;     02 13 24；
     * 03 12 21 30;     03 14 25；
     * 04 13 22 31;     04 15 26；
     * 05 14 23 32;     05 16 27；
     * 06 15 24 33;     06 17 28；
     * 07 16 25;        07 18 29；
     * 08 17 26;        08 19 30；
     * 09 18 27         09 20 31；
     *                  10 21 32；
     *                  11 22 33
     *
     *
     * 合数替代关系：
     * 可以看成n*9或者个位与十位相加所得
     * (0*9)    1合6,       2合7,       3合8,     4合9,     5合0,     6合1,     7合2,     8合3,     9合4,
     * (1*9) 10=1合6,    11=2合7,    12=3合8,  13=4合9,  14=5合0,  15=6合1,  16=7合2,  17=8合3,  18=9合4, 
     * (2*9) 19=10=1合6, 20=2合7,    21=3合8,  22=4合9,  23=5合0,  24=6合1,  25=7合2,  26=8合3,  27=9合4, 
     * (3*9) 28=10=1合6, 29=11=2合7, 30=3合8,  31=4合9,  32=5合0,  33=6合1
     * 1+10+19+28=58     
     * 2+11+20+29=62
     * 3+12+21+30=66
     * 4+13+22+31=70
     * 5+14+23+32=74
     * 6+15+24+22=67
     * 7+16+25=48
     * 8+17+26=51
     * 9+18+27=54
     * 
     * 
     * 04 05 13 14 20 21 27 28 29，每期开奖红色号码中必有1个在此9个号中出现
     * 
     * 
     * 红色球第3位，尾数定胆轨迹：
     * 047，158，269，370，481，592，603，714，825，936
     * 
     * 
     * 合数单：1,3,5,7,9,10,12,14,16
     * 合数双：2,4,6,8,11,13,15
     * 
     * 
     * 金：09 10 21 22 33；
     * 木：03 04 15 16 27 28；
     * 水：01 12 13 24 25；
     * 火：06 07 18 19 30 31；
     * 土：02 05 08 11 14 17 20 23 26 29 32
     * 
     * 
     * 0路   1路   2路   
     * 03    01    02   
     * 06    04    05
     * 09    07    08
     * 12    10    11
     * 15    13    14
     * 18    16    17
     * 21    19    20
     * 24    22    23
     * 27    25    26
     * 30    28    29
     * 33    31    32
     * 
     * 
     * 红球号码的划分：
     * 大号区间：17-33
     * 小号区间：01-16
     * 蓝球号码的划分：
     * 大号区间：09-16
     * 小号区间：01-08 
     */

    internal class Program
    {
        private static bool _isVerifyHistory = false; //是否进行历史数据验证，还是预测数据
        private static bool _isContainRecentLotto = true;  //是否验证包含最近一次开奖结果，false，为了推断下一期的号码

        static void Main(string[] args)
        {
            if (_isVerifyHistory)
            {
                if (_isContainRecentLotto)
                    VerifyHelper.VerifyPastResults(true);
                else
                    Forecast.VerifyLastPeriodGuess(false);
            }
            else
            {
                //CommonHelper.GetSumOfFirstAndSixth();
                //CommonHelper.GetSubtractOfFirstAndSixth();

                //PrintHelper.PrintForeword();

                {
                    //FindHelper.FindOddEven(1, [0, 0, 0, 0, 0, 1, 0, 0]);
                    //FindHelper.FindPrimeComposite(1, [1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0]);
                    //FindHelper.Find012Path(1, [0, 1, 2, 1, 1, 2, 2]);

                    //FindHelper.FindOddEven(2, [1, 0, 0, 0, 0, 0, 0, 1, 1,1]);
                    //FindHelper.FindPrimeComposite(2, [1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0]);
                    //FindHelper.Find012Path(2, [0, 0, 0, 1, 2, 1, 1]);

                    //FindHelper.FindOddEven(3, [1, 0, 0, 0, 0, 0, 0, 1, 1,1]);
                    //FindHelper.FindPrimeComposite(3, [0,0,1, 1, 1, 1, 0, 0, 0, 1]);
                    //FindHelper.Find012Path(3, [2, 1, 1, 2, 0, 2, 0]);

                    //FindHelper.FindOddEven(4, [0, 0, 0, 1, 1, 1, 0, 1, 1]);
                    //FindHelper.FindPrimeComposite(4, [0, 0, 0, 0, 1, 1, 0, 1, 1, 0]);
                    //FindHelper.Find012Path(4, [2, 1, 2, 0, 1, 1, 1]);

                    //FindHelper.FindOddEven(5, [1, 1, 0, 1, 0, 0, 1, 1, 0, 0]);
                    //FindHelper.FindPrimeComposite(5, [0, 0, 0, 0, 1, 1, 0, 1, 1, 0]);
                    //FindHelper.Find012Path(5, [2, 2, 1, 2, 2, 1, 1, 1]);

                    //FindHelper.FindOddEven(6, [0, 1, 0, 1, 0, 0, 0]);
                    //FindHelper.FindPrimeComposite(6, [1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0]);
                    //FindHelper.Find012Path(6, [0, 1, 0, 0, 2, 2, 1]);

                    //FindHelper.Find012Path(7, [0, 0, 1, 0, 0, 2]);
                }

                //SelectHelper.CalculatePlusAndSubtractLotto();
                //SelectHelper.CalculateSumDivisionLotto();
                //SelectHelper.Calculate9And11Lotto();
                //SelectHelper.CalculateProbableMantissa();
                //SelectHelper.CalculateProbableMiddle();
                //SelectHelper.CalculateProbableGoldedCut();
                //SelectHelper.CalculateProbableRed();
                //SelectHelper.CalculateSumSubtract();
                //SelectHelper.CalculateByRetoreValue(9);

                //SelectHelper.CaculateFirstLotto();
                //SelectHelper.CalculatePassword();

                //SelectHelper.CalculateColdHotWarm();

                //SelectHelper.CalculateExcludeBlueLotto();
                //SelectHelper.CalculateBlueWithRedSubtract();

                Forecast.GuessCurrentPeriodLotto();
            }
        }
    }
}
