/*
 *Description: FilterHelper
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 14:45:53
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDLotto
{
    public class FilterHelper
    {
        /// <summary>
        /// 差5定胆计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByDistance5(IList<int[]> data, int[] values)
        {
            var res = SelectHelper.CaculateDistance5(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => res.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经差5定胆计算法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 上期号码中下期计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByPreLotto(IList<int[]> data, int[] values)
        {
            var res = SelectHelper.CaculateHundredsTensUnits(false);
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Count(n => res.Contains(n)) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经上期号码中下期计算法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 上期组三中下期计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByGroupThreePreLotto(IList<int[]> data)
        {
            var values = SelectHelper.CaculateDistance5(false).Distinct().OrderBy(n => n);
            var nums = data.Where(g =>
            {
                return values.Any(n => g.Contains(n));
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经上期组三中下期计算法过滤后余{0}组", nums.Count()));
            return nums;
        }


        /// <summary>
        /// 百位012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByHundreds012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[0] % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百位012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 十位012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByTens012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[1] % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经十位012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 个位012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByUnits012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[2] % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经个位012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 最大值012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByMax012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Max() % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经最大值012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 最小值012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByMin012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Min() % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经最小值012路过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 中间值012路过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">取值0、1、2</param>
        /// <returns></returns>
        public static IList<int[]> FilterByMiddle012Path(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => SelectHelper.CaculateMiddleNumber(g) % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经中间值012路过滤后余{0}组", nums.Count()));
            return nums;
        }


        /// <summary>
        /// 和值过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySum(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sum = g.Sum();
                var b1 = sum >= 0 && sum <= 14;
                var b2 = sum % 3 == 1 || sum % 3 == 2;
                return b1 && b2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 和值尾过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumMantissa(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Sum() % 10 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经和值尾过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 跨度过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySpan(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Max() - g.Min() == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经跨度过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 大小比过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByBigSmallRate(IList<int[]> data, double[] values)
        {
            var nums = data.Where(g =>
            {
                var maxCount = g.Count(n => n >= 5);
                var minCount = g.Count(n => n < 5);

                return values.Any(v => (double)maxCount / minCount == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经大小比过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 奇偶比过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByOddEvenRate(IList<int[]> data, double[] values)
        {
            var nums = data.Where(g =>
            {
                var oddCount = g.Count(n => n % 2 != 0);
                var evenCount = g.Count(n => n % 2 == 0);

                return values.Any(v => (double)oddCount / evenCount == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经奇偶比过滤后余{0}组", nums.Count()));
            return nums;
        }


        /// <summary>
        /// 百位值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByHundredsValue(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[0] == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百位值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 十位值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByTensValue(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[1] == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经十位值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 个位值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByUnitsValue(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[2] == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经个位值过滤后余{0}组", nums.Count()));
            return nums;
        }
    }
}
