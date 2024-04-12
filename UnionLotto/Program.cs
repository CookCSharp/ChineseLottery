using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using UnionLotto.Tool;

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
     * 
     * 
     * 04 05 13 14 20 21 27 28 29，每期开奖红色号码中必有1个在此9个号中出现
     * 
     * 
     * 红色球第3位，尾数定胆轨迹：
     * 047，158，269，370，481，592，603，714，825，936
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
                //PrintHelper.PrintForeword();

                //SelectHelper.CalculatePlusAndSubtractLotto();
                //SelectHelper.CalculateSumDivisionLotto();
                //SelectHelper.Calculate9And11Lotto();
                //SelectHelper.CalulateProbableMantissa();
                //SelectHelper.CalulateProbableMiddle();
                //SelectHelper.CalulateProbableGoldedCut();
                //SelectHelper.CalulatePassword();

                //SelectHelper.CalculateBlueLotto();

                Forecast.GuessCurrentPeriodLotto();
            }
        }

        /// <summary>
        /// 39期预测
        /// 012路   012122
        /// 奇偶比  4:2或2:4或3:3
        /// 大小比  4:2或2:4或3:3
        /// 和尾    4
        /// </summary>
        private static void ForecastRed()
        {
            var zer = new int[11] { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33 };
            var one = new int[11] { 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31 };
            var two = new int[11] { 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32 };

            var allRedLottos = Enumerable.Range(1, 33).ToList();
            var guess = new int[6] { 0, 1, 2, 1, 2, 2 };

            var reds = new List<List<int>>();
            for (int i = 0; i < guess.Length; i++)
            {
                var nums = new List<int>();
                allRedLottos.ForEach(x =>
                {
                    if (x % 3 == guess[i])
                        nums.Add(x);
                });
                reds.Add(nums);
            }
            //3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33
            //1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31
            //2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32
            //1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31
            //2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32
            //2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32


            //满足012路的有3003组
            //var sums = new List<int>();
            var sumsGroup = new List<List<int>>();
            for (int i = 0; i < reds[0].Count; i++)
            {
                for (int j = 0; j < reds[1].Count; j++)
                {
                    if (reds[0][i] >= reds[1][j])
                        continue;

                    for (int k = 0; k < reds[2].Count; k++)
                    {
                        if (reds[1][j] >= reds[2][k])
                            continue;

                        for (int h = 0; h < reds[3].Count; h++)
                        {
                            if (reds[2][k] >= reds[3][h])
                                continue;

                            for (int l = 0; l < reds[4].Count; l++)
                            {
                                if (reds[3][h] >= reds[4][l])
                                    continue;

                                for (int m = 0; m < reds[5].Count; m++)
                                {
                                    if (reds[4][l] >= reds[5][m])
                                        continue;

                                    var nums = new List<int>();
                                    nums.Add(reds[0][i]);
                                    nums.Add(reds[1][j]);
                                    nums.Add(reds[2][k]);
                                    nums.Add(reds[3][h]);
                                    nums.Add(reds[4][l]);
                                    nums.Add(reds[5][m]);
                                    sumsGroup.Add(nums);
                                    //sumsGroup.Add(string.Join(" ", nums));
                                    //sums.Add(nums.Sum());
                                }
                            }
                        }
                    }
                }
            }

            //满足奇偶比的有2716组
            var odd_even = sumsGroup.Where(g =>
            {
                var odd = new List<int>();
                var even = new List<int>();
                for (int i = 0; i < g.Count; i++)
                {
                    if ((g[i] & 1) == 0)
                        odd.Add(i);
                    else
                        even.Add(i);
                }

                if (even.Count == 0) return false;
                var value = (double)odd.Count / even.Count;
                if (value == 2 / 4 || value == 4 / 2 || value == 3 / 3)
                    return true;
                else
                    return false;
            }).ToList();

            //满足大小比的有2387组
            var big_small = odd_even.Where(g =>
            {
                var big = new List<int>();
                var small = new List<int>();
                for (int i = 0; i < g.Count; i++)
                {
                    if (g[i] > 16)
                        big.Add(i);
                    else
                        small.Add(i);
                }

                if (small.Count == 0) return false;
                var value = (double)big.Count / small.Count;
                if (value == 2 / 4 || value == 4 / 2 || value == 3 / 3)
                    return true;
                else
                    return false;
            }).ToList();

            //满足和尾与和值范围的有145组
            var sumMantissa = big_small.Where(g =>
            {
                return g.Sum() >= 79 && g.Sum() <= 125 && g.Sum() % 10 == 4;
            }).ToList();

            //满足黄金分割点的有145组
            var goldenCutNums = SelectHelper.CalulateProbableGoldedCut(false);
            var goldenCut = sumMantissa.Where(g =>
            {
                return g.Any(n => goldenCutNums.Contains(n));
            }).ToList();

            //满足9进制的有137组
            var nineNums = SelectHelper.Calculate9And11Lotto(9, false);
            var nine = goldenCut.Where(g =>
            {
                return g.Any(n => nineNums.Contains(n));
            }).ToList();

            //满足11进制的有117组
            var elevenNums = SelectHelper.Calculate9And11Lotto(11, false);
            var eleven = nine.Where(g =>
            {
                return g.Any(n => elevenNums.Contains(n));
            }).ToList();

            //满足每期必有号的有95组
            var every = eleven.Where(g =>
            {
                return g.Any(n => Data.EveryHaveNums.Contains(n));
            }).ToList();

            //满足加减法的有44组
            var plus_subtract_Nums = SelectHelper.CalculatePlusAndSubtractLotto(false);
            var plus_subtract = every.Where(g =>
            {
                return g.Count(n => plus_subtract_Nums.Contains(n)) >= 3;
            }).ToList();

            //满足和值取商的有41组
            var sum_division_Nums = SelectHelper.CalculateSumDivisionLotto(false);
            var sum_division = plus_subtract.Where(g =>
            {
                return g.Count(n => sum_division_Nums.Contains(n)) >= 2;
            }).ToList();

            //满足每期必有号的有41组
            var prime = sum_division.Where(g =>
            {
                return g.Count(n => Data.AllPrimeNums.Contains(n)) >= 1;
            }).ToList();

            //明暗点：108/90 0189
            //var sums = prime.Select(g => g.Sum());
            var brightNumers = new int[4] { 0, 1, 8, 9 };
            var bright = prime.Where(g =>
            {
                var num123 = (g[0] + g[1] + g[2]) % 10;
                var num456 = (g[3] + g[4] + g[5]) % 10;
                return brightNumers.Contains(num123) || brightNumers.Contains(num456);
            }).ToList();

            var forecast_result = bright.Select(g => string.Join(" ", g));
        }
    }
}
