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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnionLotto
{
    public class FilterHelper
    {
        #region 常规过滤

        /// <summary>
        /// 大小比过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySize(IList<int[]> data, double[] values)
        {
            var nums = data.Where(g =>
            {
                var bigCount = g.Count(n => n > 16);
                var smallCount = g.Count(n => n <= 16);

                return values.Any(v => (double)bigCount / smallCount == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经大小比过滤后余{0}组", nums.Count()));
            return nums;
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
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByPrimeComposite(IList<int[]> data, double[] values)
        {
            var nums = data.Where(g =>
            {
                var primeCount = g.Count(n => Data.AllPrimeNums.Contains(n));
                var compositeCount = g.Count() - primeCount;

                return values.Any(v => (double)primeCount / compositeCount == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经质合比过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 和值尾数过滤
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

            PrintHelper.PrintForecastResult(string.Format("经和值尾数过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 连号法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">连号数</param>
        /// <returns></returns>
        public static IList<int[]> FilterByAdjacentNumber(IList<int[]> data, int value)
        {
            var nums = new List<int[]>();

            var nums0 = data.Where(g =>
            {
                int consecutiveCount = 0;
                for (int i = 0; i < g.Length - 1; i++)
                {
                    if (g[i + 1] - g[i] == 1)
                    {
                        consecutiveCount++;
                    }
                }
                return consecutiveCount == 0;
            }).ToList();

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

            var nums22 = data.Where(g =>
            {
                int consecutiveCount = 0;
                if ((g[1] - g[0] == 1 && g[3] - g[2] == 1 && g[2] - g[1] > 1 && g[4] - g[3] > 1 && g[5] - g[4] > 1) ||
                     (g[1] - g[0] == 1 && g[4] - g[3] == 1 && g[2] - g[1] > 1 && g[3] - g[2] > 1 && g[5] - g[4] > 1) ||
                     (g[1] - g[0] == 1 && g[5] - g[4] == 1 && g[2] - g[1] > 1 && g[3] - g[2] > 1 && g[4] - g[3] > 1) ||
                     (g[2] - g[1] == 1 && g[4] - g[3] == 1 && g[1] - g[0] > 1 && g[3] - g[2] > 1 && g[5] - g[4] > 1) ||
                     (g[2] - g[1] == 1 && g[5] - g[4] == 1 && g[1] - g[0] > 1 && g[3] - g[2] > 1 && g[4] - g[3] > 1) ||
                     (g[3] - g[2] == 1 && g[5] - g[4] == 1 && g[1] - g[0] > 1 && g[2] - g[1] > 1 && g[4] - g[3] > 1))
                {
                    consecutiveCount++;
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
                case 0:
                    nums = nums0;
                    break;
                case 2:
                    nums = nums2.Except(nums22).Except(nums3).ToList();
                    break;
                case 3:
                    nums = nums3.Except(nums4).ToList();
                    break;
                case 4:
                    nums = nums4;
                    break;
                case 22:
                    nums = nums22.Except(nums4).ToList();
                    break;
                default:

                    break;
            }

            PrintHelper.PrintForecastResult(string.Format("经{0}连号法过滤后余{1}组", value, nums.Count()));
            return nums;
        }

        /// <summary>
        /// AC值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByACValue(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                var ls = new List<int>();
                Get(ls, g, 0, 1);
                var acValue = ls.Distinct().Count() - (6 - 1);
                return values.Contains(acValue);
            }).ToList();

            void Get(List<int> ls, int[] g, int start, int k)
            {
                if (start < g.Length)
                {
                    if (k < g.Length)
                    {
                        ls.Add(Math.Abs(g[start] - g[k]));
                        Get(ls, g, start, k + 1);
                    }
                    else
                    {
                        Get(ls, g, start + 1, start + 2);
                    }
                }
            }

            PrintHelper.PrintForecastResult(string.Format("经AC值过滤后余{0}组", nums.Count()));
            return nums;
        }

        #endregion

        #region 常规过滤，每次需要修改判断条件

        /// <summary>
        /// 冷热温过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <remarks>
        /// 10期以内，出现0，1次为冷，出现2次为温，出现3次及以上为热
        /// </remarks>
        /// <returns></returns>
        public static IList<int[]> FilterByColdHotWarm(IList<int[]> data, int[] values)
        {
            var cold_hot_warm = SelectHelper.CalculateColdHotWarm(false);
            var nums = data.Where(g =>
            {
                var cold = cold_hot_warm.Item1;
                var hot = cold_hot_warm.Item2;
                var warm = cold_hot_warm.Item3;

                var coldCount = g.Count(n => cold.Contains(n));
                var hotCount = g.Count(n => hot.Contains(n));
                var warmCount = g.Count(n => warm.Contains(n));

                var b1 = values.All(v => coldCount != v && hotCount != v && warmCount != v);

                var b2 = coldCount == 3 && hotCount == 3 && warmCount == 0;
                var b3 = coldCount == 3 && hotCount == 0 && warmCount == 3;
                var b4 = coldCount == 0 && hotCount == 3 && warmCount == 3;

                var b5 = coldCount == 0 && hotCount == 2 && warmCount == 4;
                var b6 = coldCount == 0 && hotCount == 4 && warmCount == 2;
                var b7 = coldCount == 2 && hotCount == 0 && warmCount == 4;
                var b8 = coldCount == 2 && hotCount == 4 && warmCount == 0;
                var b9 = coldCount == 4 && hotCount == 0 && warmCount == 2;
                var b10 = coldCount == 4 && hotCount == 2 && warmCount == 0;

                //return b1 && !b5 && !b6 && !b7 && !b8 && !b9 && !b10;

                return b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经冷热温过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 大中小过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <remarks>
        /// 1-11为小；12-22为中；23-33为大
        /// </remarks>
        /// <returns></returns>
        public static IList<int[]> FilterByBigMiddleSmall(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                var big = g.Count(n => n >= 23);
                var middle = g.Count(n => n >= 12 && n <= 22);
                var small = g.Count(n => n <= 11);

                var b1 = values.All(v => big != v && middle != v && small != v);

                var b2 = big == 3 && middle == 0 && small == 3;
                var b3 = big == 3 && middle == 3 && small == 0;
                var b4 = big == 0 && middle == 3 && small == 3;

                var b5 = big == 0 && middle == 2 && small == 4;

                var b6 = big == 1 && middle == 1 && small == 4;
                var b7 = big == 1 && middle == 4 && small == 1;
                var b8 = big == 4 && middle == 1 && small == 1;

                //return b1 && !b5 && !b6 && !b7 && !b8;

                return b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经大中小过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 012路过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBy012Path(IList<int[]> data, int[] values)
        {
            //var zer = new int[11] { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33 };
            //var one = new int[11] { 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31 };
            //var two = new int[11] { 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32 };

            var nums = data.Where(g =>
            {
                var zer = g.Count(n => n % 3 == 0);
                var one = g.Count(n => n % 3 == 1);
                var two = g.Count(n => n % 3 == 2);

                var b1 = values.All(v => zer != v && one != v && two != v);

                var b2 = zer == 3 && one == 0 && two == 3;
                var b3 = zer == 3 && one == 3 && two == 0;
                var b4 = zer == 0 && one == 3 && two == 3;

                return b1 && !b2 && !b3 && !b4;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 和值区间过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="region">
        /// 区间，取值1，2，3，分别表示Sum大于78，78小于等于Sum且小于等于127，Sum大于127；
        /// 取值12、13、23表示联合区间段；取值123表示具体修改
        /// </param>
        /// <remarks>
        /// 和值最小值：1+2+3+4+5+6=21；和值最大值：28+29+30+31+32+33=183；
        /// </remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySumRegion(IList<int[]> data, int region)
        {
            var nums = data.Where(g =>
            {
                bool b = false;
                switch (region)
                {
                    case 1:
                        b = g.Sum() < 78;
                        break;
                    case 2:
                        b = g.Sum() >= 78 && g.Sum() <= 127;
                        break;
                    case 3:
                        b = g.Sum() > 125;
                        break;
                    case 12:
                        b = g.Sum() <= 125;
                        break;
                    case 13:
                        b = g.Sum() < 78 || g.Sum() > 127;
                        break;
                    case 23:
                        b = g.Sum() >= 78 && g.Sum() <= 183;
                        break;
                    case 123:
                        var b1 = g.Sum() >= 49 && g.Sum() <= 148;
                        var b2 = g.Sum() % 3 == 0 || g.Sum() % 3 == 2;
                        b = b1 && b2;
                        break;
                    default:
                        break;
                }
                return b;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经和值区间过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 平均值过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByAverage(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sum = (int)Math.Round(g.Sum() / 6D, MidpointRounding.AwayFromZero);

                var b1 = sum >= 7 && sum <= 29;
                var b2 = sum % 3 == 0 || sum % 3 == 2;
                var b3 = Enumerable.Range(2, (int)Math.Sqrt(sum)).Any(v => sum % v == 0);
                var b = b1 && b2 && b3;

                return b;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经平均值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 尾数和值过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByMantissaSum(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var mantissaSum = g.Select(n => n % 10).Sum();

                //mantissaSum >= 12 && mantissaSum <= 40 &&
                return mantissaSum >= 12 && mantissaSum <= 36 && mantissaSum % 2 == 0;
                       //(mantissaSum % 3 == 1 || mantissaSum % 3 == 2);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经尾数和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 首尾和过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// 取值1、2、3，分别表示大、中、小，7-24为小；25-43为中；44-61为大。
        /// 取值0时表示具体值，取值12，13，23表示联合区间段；取值123表示具体修改
        /// <remarks> 
        /// 首尾和最小值：1+6=7；首尾和最大值：28+33=61
        /// </remarks> 
        /// <returns></returns>
        public static IList<int[]> FilterBySumOf16(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sum = g[0] + g[^1];

                //var b1 = sum >= 15 && sum <= 55;
                var b1 = sum >= 15 && sum <= 46;
                var b2 = sum % 3 == 0 || sum % 3 == 2;
                var b3 = Enumerable.Range(2, (int)Math.Sqrt(sum)).Any(v => sum % v == 0);
                var b = b1 && b2;

                return b;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经首尾和过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 红2与红5之和过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumOf25(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sum = g[1] + g[4];

                var b1 = sum >= 15 && sum <= 58;
                var b2 = sum % 3 == 1 || sum % 3 == 2;
                var b = b1 && b2;

                return b;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经红2红5之和过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 红3与红4之和过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumOf34(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sum = g[2] + g[3];

                var b1 = sum >= 11 && sum <= 58;
                var b2 = sum % 3 == 0 || sum % 3 == 2;
                var b = b1 && b2;

                return b;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经红3红4之和过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 跨度过滤，即首尾差值。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <remarks>
        /// 跨度最小值：6-1=5；跨度最大值：33-1=32
        /// </remarks> 
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf16(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var span = g[^1] - g[0];
                return (span >= 11 && span <= 32) && (span % 3 == 0 || span % 3 == 1);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经跨度过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 红2与红5之差过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf25(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var span = g[4] - g[1];
                return (span >= 5 && span <= 30) && (span % 3 == 0 || span % 3 == 2);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经红2红5之差过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 红3与红4之差过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf34(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var span = g[3] - g[2];
                return (span >= 1 && span <= 21) && (span % 3 == 0 || span % 3 == 1);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经红3红4之差过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 红2与红4之差过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf24(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var span = g[3] - g[1];
                return (span >= 2 && span <= 26) && (span % 3 == 0 || span % 3 == 1);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经红2红4之差过滤后余{0}组", nums.Count()));
            return nums;
        }

        #endregion

        #region 自研方法过滤

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
        /// 2进制计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByBinary(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.Calculate2911Lotto(2, false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经2进制计算法过滤后余{0}组", nums.Count()));
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
            var result = SelectHelper.Calculate2911Lotto(9, false);
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
            var result = SelectHelper.Calculate2911Lotto(11, false);
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
        /// 边缘码计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByEdge(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => Data.EdgeLotto.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经边缘码计算法过滤后余{0}组", nums.Count()));
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
        /// <param name="values">命中几个胆尾，取值0或1或2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByProbableMantissa(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.CalculateProbableMantissa(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => result.Count(n => g.Any(m => m % 10 == n)) == v);
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
            var result = SelectHelper.CalculateProbableMiddle(false);
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
        /// <param name="values">命中个数</param>
        /// <returns></returns>
        public static IList<int[]> FilterByProbableGoldedCut(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.CalculateProbableGoldedCut(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经黄金分割定胆法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 规律计算定胆法过滤
        /// </summary>
        /// <remarks>
        /// 注意：每次需要手动添加前两期的1号红球相加之和
        /// </remarks>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByProbableRegular(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.CalculateProbableRed(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经规律计算定胆法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 差值之和定胆法过滤
        /// </summary>
        /// <remarks>
        /// 和值减5，十位个位相加确定下一期首号，该种情况要么长时间不出现，要么连续两三期出现或隔一期就会出现
        /// </remarks>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumSubtract(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.CalculateSumSubtract(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经差值之和定胆法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 3隔点定胆法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByThreeDistance(IList<int[]> data, int[] values)
        {
            var result = SelectHelper.CalculatePassword(false);
            var index = result.IndexOf(333);
            result = result.Skip(index + 1).ToList();
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => result.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经3隔点定胆法过滤后余{0}组", nums.Count()));
            return nums;
        }

        #endregion

        #region 红色球单个过滤

        /// <summary>
        /// 第N个红色球大小过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index">表示第几个红球</param>
        /// <param name="value"></param>
        /// <param name="compare">小于0表示小于，等于0表示相等，大于0表示大于</param>
        /// <returns></returns>
        public static IList<int[]> FilterByAnyValue(IList<int[]> data, int index, int value, int compare)
        {
            var nums = data.Where(g =>
            {
                var b = false;
                switch (compare)
                {
                    case -1:
                        b = g[index - 1] < value;
                        break;
                    case 0:
                        b = g[index - 1] == value;
                        break;
                    case 1:
                        b = g[index - 1] > value;
                        break;
                    case 10:
                        b = g[index - 1] >= value;
                        break;
                    case -10:
                        b = g[index - 1] <= value;
                        break;
                    case -11:
                        b = g[index - 1] != value;
                        break;
                    default:
                        break;
                }
                return b;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第{0}个红色球大小过滤后余{1}组", index, nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第N个红球质合过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index">第几个红球</param>
        /// <param name="value">0、1，0表示合数，1表示质数</param>
        /// <returns></returns>
        public static IList<int[]> FilterByAnyPrimeComposite(IList<int[]> data, int index, int value)
        {
            var nums = data.Where(g =>
            {
                var number = g[index - 1];
                var isPrime = true;

                for (int i = 2; i <= Math.Sqrt(number); i++)
                {
                    if (number % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                return value == 1 ? isPrime : !isPrime;

            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第{0}个红球质合过滤后余{1}组", index, nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第1个红色球奇偶过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0或1，0表示偶数，1表示奇数</param>
        /// <returns></returns>
        public static IList<int[]> FilterByFirstOddEven(IList<int[]> data, int value)
        {
            var nums = data.Where(g =>
            {
                return (g[0] & 1) == value;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第1个红色球奇偶过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第1个红色球012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByFirst012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[0] % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第1个红色球012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第2个红色球奇偶过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0或1，0表示偶数，1表示奇数</param>
        /// <returns></returns>
        public static IList<int[]> FilterBySecondOddEven(IList<int[]> data, int value)
        {
            var nums = data.Where(g =>
            {
                return (g[1] & 1) == value;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第2个红色球奇偶过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第2个红色球012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterBySecond012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[1] % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第2个红色球012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第3个红色球奇偶过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0或1，0表示偶数，1表示奇数</param>
        /// <returns></returns>
        public static IList<int[]> FilterByThirdOddEven(IList<int[]> data, int value)
        {
            var nums = data.Where(g =>
            {
                return (g[2] & 1) == value;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第3个红色球奇偶过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第3个红色球012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByThird012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[2] % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第3个红色球012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第4个红色球奇偶过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0或1，0表示偶数，1表示奇数</param>
        /// <returns></returns>
        public static IList<int[]> FilterByFourthOddEven(IList<int[]> data, int value)
        {
            var nums = data.Where(g =>
            {
                return (g[3] & 1) == value;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第4个红色球奇偶过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第4个红色球012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByFourth012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[3] % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第4个红色球012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第5个红色球奇偶过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0或1，0表示偶数，1表示奇数</param>
        /// <returns></returns>
        public static IList<int[]> FilterByFifthOddEven(IList<int[]> data, int value)
        {
            var nums = data.Where(g =>
            {
                return (g[4] & 1) == value;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第5个红色球奇偶过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第5个红色球012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByFifth012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[4] % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第5个红色球012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第6个红色球奇偶过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0或1，0表示偶数，1表示奇数</param>
        /// <returns></returns>
        public static IList<int[]> FilterBySixthOddEven(IList<int[]> data, int value)
        {
            var nums = data.Where(g =>
            {
                return (g[^1] & 1) == value;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第6个红色球奇偶过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第6个红色球012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterBySixth012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[5] % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第6个红色球012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        #endregion

        /// <summary>
        /// 红色球金木水火土过滤
        /// </summary>
        /// <remarks>
        /// 不建议使用此过滤，个人感觉不准
        /// </remarks>
        /// <param name="data"></param>
        /// <param name="indexLotto">表示第几号球</param>
        /// <param name="value">取值1、2、3、4、5，分别表示金、木、水、火、土</param>
        /// <returns></returns>
        public static IList<int[]> FilterByGoldWoodWaterFireEarth(IList<int[]> data, int indexLotto, int value)
        {
            var nums = data.Where(g =>
            {
                var b = false;
                switch (value)
                {
                    case 1:
                        b = Data.GoldLotto.Contains(g[indexLotto - 1]);
                        break;
                    case 2:
                        b = Data.WoodLotto.Contains(g[indexLotto - 1]);
                        break;
                    case 3:
                        b = Data.WaterLotto.Contains(g[indexLotto - 1]);
                        break;
                    case 4:
                        b = Data.FireLotto.Contains(g[indexLotto - 1]);
                        break;
                    case 5:
                        b = Data.EarthLotto.Contains(g[indexLotto - 1]);
                        break;
                    default:
                        break;
                }

                return b;
            }).ToList();

            string five = value switch
            {
                1 => "金",
                2 => "木",
                3 => "水",
                4 => "火",
                5 => "土",
                _ => throw new NotImplementedException(),
            };

            PrintHelper.PrintForecastResult(string.Format("经第{0}个红色球五行“{1}”过滤后余{2}组", indexLotto, five, nums.Count()));
            return nums;
        }

        /// <summary>
        /// 明暗点过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByPassword(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                var num123 = g[0] + g[1] + g[2];
                var num456 = g[3] + g[4] + g[5];

                var composite123 = Get(num123);
                var composite456 = Get(num456);

                return values.Any(v => v / 10 == composite123 && v % 10 == composite456);

            }).ToList();

            int Get(int num)
            {
                var composite = num / 10 + num % 10;
                if (composite >= 10)
                {
                    composite = composite / 10 + composite % 10;
                    Get(composite);
                }

                return composite;
            }

            PrintHelper.PrintForecastResult(string.Format("经明暗点法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 合数点过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByCompositeNumber(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var composite05 = g.Count(n => Data.Composite05.Contains(n));
                var composite16 = g.Count(n => Data.Composite16.Contains(n));
                var composite27 = g.Count(n => Data.Composite27.Contains(n));
                var composite38 = g.Count(n => Data.Composite38.Contains(n));
                var composite49 = g.Count(n => Data.Composite49.Contains(n));

                //return composite05 == 1 && composite16 == 1 && composite49 == 4; //没有
                //return composite05 == 1 && composite27 == 1 && composite49 == 4; //没有
                return composite05 == 1 && composite38 == 1 && composite49 == 4; //没有
                //return composite05 == 1 && composite16 == 3 && composite49 == 2; //没有

                //return composite05 == 2 && composite49 == 4; //没有
                //return composite16 == 2 && composite49 == 4; //没有
                //return composite27 == 2 && composite49 == 4; //没有
                //return composite38 == 2 && composite49 == 4; //没有

                //return composite05 == 1 && composite16 == 2 && composite27 == 1 && composite49 == 2; //没有
                //return composite05 == 1 && composite16 == 1 && composite27 == 2 && composite49 == 2; 

                //return composite05 == 1 && composite16 == 2 && composite38 == 1 && composite49 == 2; 
                //return composite05 == 1 && composite16 == 1 && composite38 == 2 && composite49 == 2; 
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经合数点过滤后余{0}组", nums.Count()));
            return nums;
        }

        /*
         * 
         * 
         * 篮球过滤
         * 
         * 
         */

        public static void FilterByRedSubtract(IList<int[]> data)
        {

        }
    }
}
