/*
 *Description: FilterHelper
 *Author: Chance.zheng
 *Creat Time: 2024/4/7 13:59:32
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionLotto
{
    public class FilterHelper
    {
        //private const int MinSum = 6;  // 1 + 2 + 3 = 6;
        //private const int MaxSum = 96; // 31 + 32 + 33 = 6;
        //private static int[] AllLotto = new int[33] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 };


        ////比如38期，明暗点 101/92 合11，小2
        //private const int M1 = 9; //第一个合数
        //private const int M2 = 2; //第二个合数
        //private const int N = 15; //临界值

        //private const string Str1 = "15,17,24";  //后3位
        //private const string Str2 = "15,17,33";

        ///// <summary>
        ///// 获取对应合数的组合数，在确定胆号之后可用此方法
        ///// </summary>
        ///// <param name="value">合数值</param>
        //private static IEnumerable<string> GetCompositeNumber()
        //{
        //    var sums = Enumerable.Range(9, 88); //96 - 9 + 1
        //    var resultNumers = sums.Where(n =>
        //    {
        //        var res = n % 9 == 0 ? 9 : n % 9;
        //        return res == M1;
        //    });

        //    var str_RedLottos = new List<string>();
        //    for (int i = 1; i < N; i++)
        //    {
        //        for (int j = i + 1; j < N; j++)
        //        {
        //            for (int k = j + 1; k < N; k++)
        //            {
        //                var sum = i + j + k;
        //                var res = sum % 9 == 0 ? 9 : sum % 9;
        //                if (res == M1)
        //                {
        //                    str_RedLottos.Add(i.ToString("D2") + "," + j.ToString("D2") + "," + k.ToString("D2") + "," + Str1);
        //                    str_RedLottos.Add(i.ToString("D2") + "," + j.ToString("D2") + "," + k.ToString("D2") + "," + Str2);
        //                }
        //            }
        //        }
        //    }

        //    return str_RedLottos;
        //}


        ///// <summary>
        ///// 胆号尾数过滤
        ///// </summary>
        ///// <param name="values">胆号尾数集合</param>
        //private static IEnumerable<string> FilterByBileMantissa(IEnumerable<string> redLottos, int[] values)
        //{
        //    var correctLottos = redLottos.Where(s =>
        //    {
        //        return s.Split(',').Where(n =>
        //        {
        //            if (int.Parse(n) % 10 == values[0] || int.Parse(n) % 10 == values[1])
        //                return true;
        //            else
        //                return false;
        //        }).Count() > 0;
        //    });

        //    return correctLottos;
        //}

        ///// <summary>
        ///// 胆号尾数过滤
        ///// </summary>
        ///// <param name="values">胆号尾数集合</param>
        //private static IEnumerable<int[]> FilterByBileMantissa(IEnumerable<int[]> redLottos, int[] values)
        //{
        //    var correctLottos = redLottos.Where(s =>
        //    {
        //        return s.Where(n =>
        //        {
        //            if (n % 10 == values[0] || n % 10 == values[1])
        //                return true;
        //            else
        //                return false;
        //        }).Count() > 0;
        //    });

        //    return correctLottos;
        //}

        ///// <summary>
        ///// 合数值过滤
        ///// </summary>
        ///// <param name="redLottos"></param>
        ///// <returns></returns>
        ///// <example>
        ///// 比如38期，明暗点 101/92 合11，小2，比如开暗92，需满足前3合9，后3合2
        ///// </example>
        //private static IEnumerable<int[]> FilterByCompositeNumberValue(IEnumerable<int[]> redLottos)
        //{
        //    return redLottos.Where(nums =>
        //    {
        //        var cn1 = nums[0] + nums[1] + nums[2];
        //        var cn2 = nums[3] + nums[4] + nums[5];
        //        var cn1Res = cn1 % 9 == 0 ? 9 : cn1 % 9;
        //        var cn2Res = cn2 % 9 == 0 ? 9 : cn2 % 9;
        //        if (nums[0] == 2 && nums[1] == 10 && nums[2] == 15
        //        && nums[3] == 17 && nums[4] == 19 && nums[5] == 20)
        //        {

        //        }
        //        return cn1Res == M1 && cn2Res == M2;
        //    });
        //}

        ///// <summary>
        ///// 合数尾数过滤
        ///// </summary>
        ///// <param name="values">合数尾数集合</param>
        ///// <example>
        ///// 比如38期，明暗点 101/92 合11，小2，合数尾0,1,2,9
        ///// </example>
        //private static IEnumerable<string> FilterByCompositeNumberMantissa(IEnumerable<string> redLottos, int[] values)
        //{
        //    var correctLottos = redLottos.Where(s =>
        //    {
        //        var res = new List<int>();
        //        var nums = s.Split(',').Select(s => int.Parse(s)).ToArray();
        //        for (int i = 0; i < nums.Length; i++)
        //        {
        //            for (int j = i + 1; j < nums.Length; j++)
        //            {
        //                res.Add((nums[i] + nums[j]) % 10);
        //            }
        //        }

        //        return values.ToArray().All(n => res.Contains(n));
        //    });

        //    return correctLottos;
        //}

        ///// <summary>
        ///// 合数尾数过滤
        ///// </summary>
        ///// <param name="values">合数尾数集合</param>
        ///// <example>
        ///// 比如38期，明暗点 101/92 合11，小2，合数尾0,1,2,9
        ///// </example>
        //private static IEnumerable<int[]> FilterByCompositeNumberMantissa(IEnumerable<int[]> redLottos, int[] values)
        //{
        //    var correctLottos = redLottos.Where(nums =>
        //    {
        //        var res = new List<int>();
        //        for (int i = 0; i < nums.Length; i++)
        //        {
        //            for (int j = i + 1; j < nums.Length; j++)
        //            {
        //                res.Add((nums[i] + nums[j]) % 10);
        //            }
        //        }

        //        return values.ToArray().All(n => res.Contains(n));
        //    });

        //    return correctLottos;
        //}


        ///// <summary>
        ///// 大范围过滤
        ///// 1.取两两相加的结果数，然后取4-6个作为胆码，再与剩下的0-2个可能号码进行组合，形成一组号码
        ///// 2.再进行过滤
        ///// </summary>
        //private static void CalculateAllRange()
        //{
        //    var combinations = new List<int[]>();
        //    var data = new int[19] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 15, 16, 17, 18, 19, 20, 26 };
        //    //出4个
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        for (int j = i + 1; j < data.Length; j++)
        //        {
        //            for (int k = j + 1; k < data.Length; k++)
        //            {
        //                for (int l = k + 1; l < data.Length; l++)
        //                {
        //                    var combination = new int[4] { data[i], data[j], data[k], data[l] };
        //                    combinations.Add(combination);
        //                }
        //            }
        //        }
        //    }

        //    //出5个
        //    //for (int i = 0; i < data.Length; i++)
        //    //{
        //    //    for (int j = i + 1; j < data.Length; j++)
        //    //    {
        //    //        for (int k = j + 1; k < data.Length; k++)
        //    //        {
        //    //            for (int l = k + 1; l < data.Length; l++)
        //    //            {
        //    //                for(int m = l + 1; m < data.Length; m++)
        //    //                {
        //    //                    var combination = new int[5] { data[i], data[j], data[k], data[l], data[m] };
        //    //                    combinations.Add(combination);
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    //所有过滤后的号码集合
        //    var redLottos = new List<int[]>();
        //    foreach (var standard in combinations)
        //    {
        //        var remainRedLotto = new List<int[]>();
        //        var remain = AllLotto.Except(standard).ToArray();

        //        //再出2个
        //        for (int i = 0; i < remain.Length; i++)
        //        {
        //            for (int j = i + 1; j < remain.Length; j++)
        //            {
        //                var ls = new List<int>();
        //                ls.AddRange(standard);
        //                ls.Add(remain[i]);
        //                ls.Add(remain[j]);
        //                ls.Sort();
        //                remainRedLotto.Add(ls.ToArray());
        //            }
        //        }

        //        //再出1个
        //        //for (int i = 0; i < remain.Length; i++)
        //        //{
        //        //    var ls = new List<int>();
        //        //    ls.AddRange(standard);
        //        //    ls.Add(remain[i]);
        //        //    ls.Sort();
        //        //    remainRedLotto.Add(ls.ToArray());
        //        //}

        //        //remainRedLotto = FilterByBileMantissa(remainRedLotto, new int[] { 9, 2 }).ToList();
        //        //remainRedLotto = FilterByCompositeNumberMantissa(remainRedLotto, new int[] { 0, 1, 2, 9 }).ToList();
        //        //remainRedLotto = FilterByCompositeNumberValue(remainRedLotto).ToList();

        //        redLottos.AddRange(remainRedLotto);
        //    }

        //    //包含15，17
        //    //redLottos = redLottos.Where(nums => nums.Contains(15) && nums.Contains(17)).ToList();

        //    //每期大概率必有的
        //    var verifyNums = new int[9] { 4, 5, 13, 14, 20, 21, 27, 28, 29 };
        //    redLottos = redLottos.Where(nums => nums.Any(n => verifyNums.Contains(n))).ToList();

        //    //预测号码结果，以组为单位
        //    var resultGroup = redLottos.Select(nums => string.Join(" ", nums)).Distinct();
        //    //预测号码
        //    var resultNumber = redLottos.SelectMany(nums => nums.ToList()).Distinct().OrderBy(x => x);
        //}


        public static void Filter()
        {
            //var redLottos = GetCompositeNumber();

            //redLottos = FilterByBileMantissa(redLottos, new int[] { 9, 2 });
            //redLottos = FilterByCompositeNumberMantissa(redLottos, new int[] { 0, 1, 2, 9 });

            //redLottos = redLottos.Select(s => s.Replace(",", " "));
        }


        /// <summary>
        /// 奇偶比过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByOddEven(IList<int[]> data, double[] values)
        {
            var nums = data.Where(g =>
            {
                var oddCount = g.Count(n => (n & 1) == 1);
                var evenCount = g.Count(n => (n & 1) == 0);

                return values.Any(v => (double)oddCount / evenCount == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经奇偶比过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 质合比过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByPrimeComposite(IList<int[]> data, double v)
        {
            var nums = data.Where(g =>
            {
                var primeCount = g.Count(n => Data.AllPrimeNums.Contains(n));
                var compositeCount = g.Count() - primeCount;

                return (double)primeCount / compositeCount == v;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经质合比过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 和值尾过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mantissas"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumMantissa(IList<int[]> data, int[] mantissas)
        {
            var nums = data.Where(g =>
            {
                return mantissas.Any(n => g.Sum() % 10 == n);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经和尾过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 和值过滤。和值条件每次需要临时调整
        /// </summary>
        /// <param name="data"></param>
        /// <param name="section"></param>
        /// <remarks>
        /// 和值最小值：1+2+3+4+5+6=21；和值最大值：28+29+30+31+32+33=183；
        /// </remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySum(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                return g.Sum() < 79 || g.Sum() > 125;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 首尾和过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <remarks>
        /// 首尾和最小值：1+6=7；首尾和最大值：28+33=61
        /// </remarks> 
        /// <returns></returns>
        public static IList<int[]> FilterBySumOfHeadAndTail(IList<int[]> data, int value)
        {
            var nums = data.Where(g =>
            {
                return g[0] + g[^1] < value;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经首尾和过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 加减计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByPlusAndSubtract(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.CalculatePlusAndSubtractLotto(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经加减计算法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 和值取商计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumDivision(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.CalculateSumDivisionLotto(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经和值取商计算法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 9进制计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByNoveary(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.Calculate9And11Lotto(9, false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经9进制计算法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 11进制计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByUndecimal(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.Calculate9And11Lotto(11, false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经11进制计算法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 每期必有计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByEveryPeriodNum(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => Data.EveryHaveNums.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经每期必有计算法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 质数计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByPrimeNum(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => Data.AllPrimeNums.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经质数计算法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 尾数定胆法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="canContain"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByProbableMantissa(IList<int[]> data, bool canContain)
        {
            var result = SelectHelper.CalulateProbableMantissa(false);
            var nums = data.Where(g =>
            {
                var has = g.Any(n => result.Contains(n % 10));
                return canContain ? has : !has;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经尾数定胆法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 中间数定胆法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="canContain"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByProbableMiddle(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.CalulateProbableMiddle(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经中间数定胆法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 黄金分割定胆法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="canContain"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByProbableGoldedCut(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.CalulateProbableGoldedCut(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经黄金分割定胆法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 连号法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">连号数量</param>
        /// <returns></returns>
        public static IList<int[]> FilterByAdjacent(IList<int[]> data, int value)
        {
            var result = SelectHelper.CalulateProbableGoldedCut(false);
            var nums = new List<int[]>();
            var nums2 = data.Where(g =>
            {
                int consecutiveCount = 0;
                for (int i = 0; i < g.Length - 1; i++)
                {
                    if (g[i + 1] - g[i] == 1)
                    {
                        consecutiveCount++;
                    }
                }
                return consecutiveCount > 0;
            }).ToList();

            var nums3 = data.Where(g =>
            {
                int consecutiveCount = 0;
                for (int i = 0; i < g.Length - 2; i++)
                {
                    if (g[i + 1] - g[i] == 1 && g[i + 2] - g[i + 1] == 1)
                    {
                        consecutiveCount++;
                    }
                }
                return consecutiveCount > 0;
            }).ToList();

            var nums4 = data.Where(g =>
            {
                int consecutiveCount = 0;
                for (int i = 0; i < g.Length - 3; i++)
                {
                    if (g[i + 1] - g[i] == 1 && g[i + 2] - g[i + 1] == 1 && g[i + 3] - g[i + 2] == 1)
                    {
                        consecutiveCount++;
                    }
                }
                return consecutiveCount > 0;
            }).ToList();

            switch (value)
            {
                case 4:
                    nums = nums4;
                    break;
                case 3:
                    nums = nums3.Except(nums4).ToList(); ;
                    break;
                case 2:
                default:
                    nums = nums2.Except(nums3).ToList();
                    break;
            }

            PrintHelper.PrintForecastResult(string.Format("经连号法过滤后余{0}组", nums.Count()));
            return nums;
        }
    }
}
