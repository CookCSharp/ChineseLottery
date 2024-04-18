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
        /// 尾数和值过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mantissas"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByMantissaSum(IList<int[]> data, int[] sums)
        {
            var nums = data.Where(g =>
            {
                var mantissaSum = g.Select(n => n % 10).Sum();

                return sums.All(sum => mantissaSum != sum) &&
                       mantissaSum > 14 && //&& mantissaSum % 2 == 1
                       (mantissaSum % 3 == 0 || mantissaSum % 3 == 1);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经尾数和值过滤后余{0}组", nums.Count()));
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
                switch ( region )
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
                        b = (g.Sum() >= 78 && g.Sum() <= 96) ||
                            (g.Sum() >= 117 && g.Sum() <= 136);
                        var b1 = (g.Sum() & 1) == 0;
                        var b2 = g.Sum() % 3 == 0;
                        var b3 = (g.Sum() & 1) == 0 && g.Sum() % 3 == 0;
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
        /// 首尾和过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">
        /// 取值1、2、3，分别表示大、中、小，7-24为小；25-43为中；44-61为大。
        /// 取值0时表示具体值，取值12，13，23表示联合区间段；取值123表示具体修改
        /// </param>
        /// <remarks> 
        /// 首尾和最小值：1+6=7；首尾和最大值：28+33=61
        /// </remarks> 
        /// <returns></returns>
        public static IList<int[]> FilterBySumOfHeadAndTail(IList<int[]> data, int value)
        {
            var nums = data.Where(g =>
            {
                var b = false;
                var sum = g[0] + g[^1];
                switch ( value )
                {
                    case 0:
                        b = sum == value;
                        break;
                    case 1:
                        b = sum >= 7 && sum <= 24;
                        break;
                    case 2:
                        b = sum >= 25 && sum <= 43;
                        break;
                    case 3:
                        b = sum >= 44 && sum <= 61;
                        break;
                    case 12:
                        b = sum <= 43;
                        break;
                    case 13:
                        b = (sum >= 7 && sum <= 24) || (sum >= 44 && sum <= 61);
                        break;
                    case 23:
                        b = sum >= 25;
                        break;
                    case 123:
                        b = (sum >= 7 && sum <= 31) || sum == 33 || sum == 36;
                        //b = sum < 44;
                        break;
                    default:
                        break;
                }
                return b;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经首尾和过滤后余{0}组", nums.Count()));
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
        public static IList<int[]> FilterBySpanOfHeadAndTail(IList<int[]> data, params int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[^1] - g[0] == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经跨度过滤后余{0}组", nums.Count()));
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
                if ( start < g.Length )
                {
                    if ( k < g.Length )
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
                for ( int i = 0; i < g.Length - 1; i++ )
                {
                    if ( g[i + 1] - g[i] == 1 )
                    {
                        consecutiveCount++;
                    }
                }
                return consecutiveCount == 0;
            }).ToList();

            var nums2 = data.Where(g =>
            {
                int consecutiveCount = 0;
                for ( int i = 0; i < g.Length - 1; i++ )
                {
                    if ( g[i + 1] - g[i] == 1 )
                    {
                        consecutiveCount++;
                    }
                }
                return consecutiveCount > 0;
            }).ToList();

            var nums22 = data.Where(g =>
            {
                int consecutiveCount = 0;
                if ( (g[1] - g[0] == 1 && g[3] - g[2] == 1 && g[2] - g[1] > 1 && g[4] - g[3] > 1 && g[5] - g[4] > 1) ||
                     (g[1] - g[0] == 1 && g[4] - g[3] == 1 && g[2] - g[1] > 1 && g[3] - g[2] > 1 && g[5] - g[4] > 1) ||
                     (g[1] - g[0] == 1 && g[5] - g[4] == 1 && g[2] - g[1] > 1 && g[3] - g[2] > 1 && g[4] - g[3] > 1) ||
                     (g[2] - g[1] == 1 && g[4] - g[3] == 1 && g[1] - g[0] > 1 && g[3] - g[2] > 1 && g[5] - g[4] > 1) ||
                     (g[2] - g[1] == 1 && g[5] - g[4] == 1 && g[1] - g[0] > 1 && g[3] - g[2] > 1 && g[4] - g[3] > 1) ||
                     (g[3] - g[2] == 1 && g[5] - g[4] == 1 && g[1] - g[0] > 1 && g[2] - g[1] > 1 && g[4] - g[3] > 1) )
                {
                    consecutiveCount++;
                }
                return consecutiveCount > 0;
            }).ToList();

            var nums3 = data.Where(g =>
            {
                int consecutiveCount = 0;
                for ( int i = 0; i < g.Length - 2; i++ )
                {
                    if ( g[i + 1] - g[i] == 1 && g[i + 2] - g[i + 1] == 1 )
                    {
                        consecutiveCount++;
                    }
                }
                return consecutiveCount > 0;
            }).ToList();

            var nums4 = data.Where(g =>
            {
                int consecutiveCount = 0;
                for ( int i = 0; i < g.Length - 3; i++ )
                {
                    if ( g[i + 1] - g[i] == 1 && g[i + 2] - g[i + 1] == 1 && g[i + 3] - g[i + 2] == 1 )
                    {
                        consecutiveCount++;
                    }
                }
                return consecutiveCount > 0;
            }).ToList();

            switch ( value )
            {
                case 0:
                    nums = nums0;
                    break;
                case 2:
                    nums = nums2.Except(nums3).ToList();
                    break;
                case 3:
                    nums = nums3.Except(nums4).ToList(); ;
                    break;
                case 4:
                    nums = nums4;
                    break;
                case 22:
                    nums = nums22;
                    break;
                default:

                    break;
            }

            PrintHelper.PrintForecastResult(string.Format("经{0}连号法过滤后余{1}组", value, nums.Count()));
            return nums;
        }

        /// <summary>
        /// 012路过滤
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

                var b = values.All(v => zer != v && one != v && two != v);

                var b1 = zer == 0 && one == 2 && two == 4;

                var b2 = zer == 3 && one == 3 && two == 0;
                var b3 = zer == 0 && one == 3 && two == 3;
                var b4 = zer == 3 && one == 0 && two == 3;

                return b && !b1 && !b2 && !b3 && !b4;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 大中小过滤。
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

                var b = values.All(v => big != v && middle != v && small != v);

                var b1 = big == 3 && middle == 0 && small == 3;
                var b2 = big == 3 && middle == 3 && small == 0;
                var b3 = big == 0 && middle == 3 && small == 3;
                var b4 = big == 0 && middle == 2 && small == 4;
                var b5 = big == 1 && middle == 1 && small == 4;

                return b && !b1 && !b2 && !b3 && !b4 && b5;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经大中小过滤后余{0}组", nums.Count()));
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
        /// <param name="values">命中个数</param>
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

        #endregion

        #region 红色球单个过滤

        /// <summary>
        /// 第1个红色球大小过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByFirstValue(IList<int[]> data, int value = 17)
        {
            var nums = data.Where(g =>
            {
                return g[0] < value;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第1个红色球大小过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第1个红色球奇偶过滤。
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
        /// 第2个红色球奇偶过滤。
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
        /// 第6个红色球大小过滤。注意：需要每次修改判断条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySixthValue(IList<int[]> data, int value = 16)
        {
            var nums = data.Where(g =>
            {
                return g[^1] > value;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经第6个红色球大小过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 第6个红色球奇偶过滤。
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

        #endregion




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
