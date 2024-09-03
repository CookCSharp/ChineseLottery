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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDLotto
{
    public class FilterHelper
    {
        public static IList<int[]> FilterByAllTwoPowerful(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                return Data.AllTwoLottos.Any(two => g.Count(n => two.Contains(n)) >= 2);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经万能2码过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 差5定胆计算法过滤
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByDistance5(IList<int[]> data, int[] values)
        {
            var res = SelectHelper.CalculateDistance5(false);
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
            var res = SelectHelper.CalculateHundredsTensUnits(false);
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
            var values = SelectHelper.CalculateDistance5(false).Distinct().OrderBy(n => n);
            var nums = data.Where(g =>
            {
                return values.Any(n => g.Contains(n));
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经上期组三中下期计算法过滤后余{0}组", nums.Count()));
            return nums;
        }

        public static IList<int[]> FilterByCurrentPeriodMantissa(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                return g.All(n => Data.CurrentPeriod % 10 != n);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经当期期数尾数杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }

        public static IList<int[]> FilterByRelation(IList<int[]> data)
        {
            var relation_number = SelectHelper.CalculateRelationNumbers(Data.PreLotto);
            var nums = data.Where(g =>
            {
                var relation_count = relation_number.Count(n => g.Contains(n));
                return relation_count != 2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经上期开奖号对应号杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }


        public static IList<int[]> FilterByPreLottoPower()
        {
            var nums = SelectHelper.CalculateIncludeNumbers(Data.PreLotto);

            PrintHelper.PrintForecastResult(string.Format("经上期开奖号平方杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }

        public static IList<int[]> FilterByPreLottoSum(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                return g.Sum() != Data.PreLotto.Sum();
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经上期开奖号和值杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 上期开奖号跨度与和值尾组合
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanSumMantissa(IList<int[]> data)
        {
            var relation_number = SelectHelper.CalculateSpanSumMantissaNumbers(Data.PreLotto);
            var nums = data.Where(g =>
            {
                var relation_count = relation_number.Count(n => g.Contains(n));
                return relation_count != 2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经跨度和值尾组合杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }

        public static IList<int[]> FilterByCurrentPeriodTwoMantissa(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var twoMantissa = Data.CurrentPeriod % 100;
                var numbers = new int[2] { twoMantissa / 10, twoMantissa % 10 };
                return g.Count(n => numbers.Contains(n)) < 2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经当期期数后两位组合杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }

        public static IList<int[]> FilterByCurrentPeriodAndSumMantissa(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var numbers = new int[2] { Data.CurrentPeriod % 10, Data.PreLotto.Sum() % 10 };
                return g.Distinct().Count(n => numbers.Contains(n)) < 2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经当期期数尾与上期和值尾组合杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }

        public static IList<int[]> FilterByMoreCompostion(IList<int[]> data)
        {
            var lastPeriodSpan = Data.PreLotto.Max() - Data.PreLotto.Min();
            //var equal1 = lastPeriodSpan != nextPeriodLotto.Sum() && lastPeriodSpan != nextPeriodLotto.Sum() % 10; //89%
            //var equal2 = lastPeriodLotto.Sum() % 10 + 4 != nextPeriodLotto[2]; //百位、十位、个位 皆为94%
            //var equal3 = lastPeriodSpan != nextPeriodLotto[2]; //88%
            //var equal4 = lastPeriodLotto[1] != nextPeriodLotto[0]; //90%
            //var equal5 = (lastPeriodLotto.Sum() % 10 + lastPeriodSpan) % 10 != nextPeriodLotto[1]; //90%
            //var equal6 = Data.CurrentPeriod % 10 + 4 != nextPeriodLotto[0]; //百位、十位、个位 皆为94%
            //var equal7 = lastPeriodLotto[0] != nextPeriodLotto[2]; //88%
            //var equal8 = lastPeriodLotto.Sum() % 10 - 3 != nextPeriodLotto[1]; //百位、十位、个位 皆为93%
            //var equal9 = lastPeriodLotto[2] != nextPeriodLotto[0]; //百位、十位92%
            //var equal10 = Data.CurrentPeriod % 10 != nextPeriodLotto[0]; //90%
            //var equal11 = (Data.CurrentPeriod % 10 * 3 + 3) % 10 != nextPeriodLotto[0]; //90%

            var nums = data.Where(g =>
            {
                var equal1 = lastPeriodSpan != g.Sum() && lastPeriodSpan != g.Sum() % 10;
                var equal2 = Data.PreLotto.Sum() % 10 + 4 != g[0] && Data.PreLotto.Sum() % 10 + 4 != g[1] && Data.PreLotto.Sum() % 10 + 4 != g[2];
                var equal3 = Data.PreLotto[1] != g[0];
                var equal4 = Data.CurrentPeriod % 10 + 4 != g[0] && Data.CurrentPeriod % 10 + 4 != g[1] && Data.CurrentPeriod % 10 + 4 != g[2];
                var equal5 = Data.PreLotto.Sum() % 10 - 3 != g[0] && Data.PreLotto.Sum() % 10 - 3 != g[1] && Data.PreLotto.Sum() % 10 - 3 != g[2];
                var equal6 = Data.PreLotto[2] != g[0] && Data.PreLotto[2] != g[1];
                var equal7 = Data.CurrentPeriod % 10 != g[0];

                return equal1 && equal2 && equal3 && equal4 && equal5 && equal6 && equal7;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经各种组合杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }

        public static IList<int[]> FilterByLastTwoPeriodTens(IList<int[]> data)
        {
            var numbers = new int[2] { 6, 4 };
            var nums = data.Where(g =>
            {
                return g.Count(n => numbers.Contains(n)) < 2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经上两期十位组合杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 上期试机号跨度与和值尾组合
        /// 上期开奖号与试机号跨度组合
        /// 上期开奖号与试机号和值尾组合
        /// 上期开奖号和值尾与试机号跨度组合
        /// 上期开奖号跨度与试机号和值尾组合
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterByOtherComposition(IList<int[]> data)
        {
            var relation_number = new List<int[]>() { new int[2] { 3, 8 }, new int[2] { 3, 2 }, new int[2] { 8, 8 }, new int[2] { 2, 8 } };
            var nums = data.Where(g =>
            {
                return relation_number.All(r => r.Count(n => g.Contains(n)) != 2);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经跨度或值尾其它组合杀号法过滤后余{0}组", nums.Count()));
            return nums;
        }

        public static IList<int[]> FilterBySpecific(IList<int[]> data)
        {
            var relation_number = new int[3] { 2, 0, 1 };
            var nums = data.Where(g =>
            {
                return relation_number.Count(n => g.Contains(n)) >= 2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经特定方法过滤后余{0}组", nums.Count()));
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
                return values.Any(v => SelectHelper.CalculateMiddleNumber(g) % 3 == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经中间值012路过滤后余{0}组", nums.Count()));
            return nums;
        }


        /// <summary>
        /// 百位过滤
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小号012，中号3456，大号789</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterByHundreds(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var value = g[0];
                var b1 = value % 3 == 1 || value % 3 == 2;
                var b2 = value <= 6;
                return b1 && b2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百位过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 十位过滤
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小号012，中号3456，大号789</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterByTens(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var value = g[1];
                var b1 = value % 3 == 0 || value % 3 == 1;
                var b2 = value <= 6 && value >= 0;
                return b1 && b2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经十位过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 个位过滤
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小号012，中号3456，大号789</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterByUnits(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var value = g[2];
                var b1 = value % 3 == 0 || value % 3 == 1;
                //var b2 = value <= 2 || value >= 7;
                return b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经个位过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 和值过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小号0-8，中号9-17，大号18-27</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySum(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sum = g.Sum();
                var b1 = sum % 3 == 0 || sum % 3 == 0;
                var b2 = sum <= 17;
                return b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 和值尾过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <remarks>012为小，3456为中，789为大</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySumMantissa(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sumMantissa = g.Sum() % 10;
                var b1 = sumMantissa % 3 == 1 || sumMantissa % 3 == 2;
                var b2 = sumMantissa <= 6;
                return b1 && b2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经和值尾过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 跨度过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <remarks>012为小，3456为中，789为大</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySpan(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var span = g.Max() - g.Min();
                var b1 = span % 3 == 0 || span % 3 == 1;
                //var b2 = span >= 3;
                return b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经跨度过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 百十位和值过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小：0-6，中：7-12，大：13-18</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySumOf12(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sum = g[0] + g[1];
                var b1 = sum % 3 == 0 || sum % 3 == 2;
                var b2 = sum <= 12;
                return b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百十位和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 百个位和值过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小：0-6，中：7-12，大：13-18</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySumOf13(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sum = g[0] + g[2];
                var b1 = sum % 3 == 0 || sum % 3 == 2;
                var b2 = sum <= 12;
                return b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百个位和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 十个位和值过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小：0-6，中：7-12，大：13-18</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySumOf23(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var sum = g[1] + g[2];
                var b1 = sum % 3 == 2 || sum % 3 == 2;
                var b2 = sum <= 12;
                return b2 && b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经十个位和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 百十位差值过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小：0-2，中：3-6，大：7-9</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf12(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var span = Math.Abs(g[0] - g[1]);
                var b1 = span % 3 == 1 || span % 3 == 2;
                var b2 = span <= 2 || span >= 7;
                return b2;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百十位差值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 百个位差值过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小：0-2，中：3-6，大：7-9</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf13(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var span = Math.Abs(g[0] - g[2]);
                var b1 = span % 3 == 0 || span % 3 == 2;
                var b2 = span <= 6;
                return b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百个位差值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 十个位差值过滤。注意：每次需要修改条件
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>小：0-2，中：3-6，大：7-9</remarks>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf23(IList<int[]> data)
        {
            var nums = data.Where(g =>
            {
                var span = Math.Abs(g[1] - g[2]);
                var b1 = span % 3 == 0 || span % 3 == 2;
                var b2 = span <= 2 || span >= 7;
                return b1;
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经十个位差值过滤后余{0}组", nums.Count()));
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

        /// <summary>
        /// 和值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumValue(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Sum() == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 和值尾过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumMantissaValue(IList<int[]> data, int[] values)
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
        /// <returns></returns>
        public static IList<int[]> FilterBySpanValue(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g.Max() - g.Min() == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经跨度过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 百十和值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumOf12Value(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[0] + g[1] == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百十和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 百个和值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumOf13Value(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[0] + g[2] == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百个和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 十个和值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySumOf23Value(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => g[1] + g[2] == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经十个和值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 百十差值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf12Value(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => Math.Abs(g[0] - g[1]) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百十差值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 百个差值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf13Value(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => Math.Abs(g[0] - g[2]) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经百个差值过滤后余{0}组", nums.Count()));
            return nums;
        }

        /// <summary>
        /// 十个差值过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int[]> FilterBySpanOf23Value(IList<int[]> data, int[] values)
        {
            var nums = data.Where(g =>
            {
                return values.Any(v => Math.Abs(g[1] - g[2]) == v);
            }).ToList();

            PrintHelper.PrintForecastResult(string.Format("经十个差值过滤后余{0}组", nums.Count()));
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


        private static bool IsPrime(int value)
        {
            if (value == 0)
            {
                return false;
            }
            else if (value >= 1 && value <= 2)
            {
                return true;
            }
            else
            {
                return Enumerable.Range(2, (int)Math.Sqrt(value)).Any(v => value % v != 0);
            }
        }
    }
}