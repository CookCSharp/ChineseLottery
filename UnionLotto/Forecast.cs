/*
 *Description: Forecast
 *Author: Chance.zheng
 *Creat Time: 2024/4/10 14:50:49
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnionLotto.Tool;

namespace UnionLotto
{
    public class Forecast
    {
        /// <summary>
        /// 等开奖后验证上一期的猜测
        /// </summary>
        /// <param name="isVerifyAll">是否验证包含最近一次开奖结果</param>
        /// <remarks>
        /// 主要是验证模型命中概率在开奖号码中的表现，以此来推断下一期的号码
        /// </remarks>
        public static void VerifyLastPeriodGuess(bool isContainRecentLotto)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(string.Format("{0}期开奖结果：{1}", Data.CurrentPeriod - 1, string.Join(" ", Data.PreRedBlueLotto)));
            Console.WriteLine(string.Format("{0}期猜测验证结果如下：", Data.CurrentPeriod - 1));
            Console.WriteLine();

            var standardRedLotto = Data.PreRedBlueLotto.Take(6).ToList();
            Data.PreRedBlueLotto = new int[7] { 2, 9, 12, 22, 25, 33, 16 }; //41期开奖结果

            var nums_PlusSubtract = SelectHelper.CalculatePlusAndSubtractLotto(false);
            var count_PlusSubtract = standardRedLotto.Count(n => nums_PlusSubtract.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("两两加减计算法得到结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_PlusSubtract, string.Join(" ", nums_PlusSubtract)));

            var nums_SumDivision = SelectHelper.CalculateSumDivisionLotto(false);
            var count_SumDivision = standardRedLotto.Count(n => nums_SumDivision.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("和值取商计算法得到结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_SumDivision, string.Join(" ", nums_SumDivision)));

            var nums_9 = SelectHelper.Calculate9And11Lotto(9, false);
            var count_9 = standardRedLotto.Count(n => nums_9.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("9进制计算法得到结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_9, string.Join(" ", nums_9)));

            var nums_11 = SelectHelper.Calculate9And11Lotto(11, false);
            var count_11 = standardRedLotto.Count(n => nums_11.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("11进制计算法得到结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_11, string.Join(" ", nums_11)));

            var nums_EveryHave = Data.EveryHaveNums;
            var count_EveryHave = standardRedLotto.Count(n => nums_EveryHave.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("每期必有号码计算法结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_EveryHave, string.Join(" ", nums_EveryHave)));

            var nums_Prime = Data.AllPrimeNums;
            var count_Prime = standardRedLotto.Count(n => nums_Prime.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("质数计算法结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_Prime, string.Join(" ", nums_Prime)));

            var nums_Mantissa = SelectHelper.CalculateProbableMantissa(false);
            var count_Mantissa = nums_Mantissa.Count(n => standardRedLotto.Any(m => m % 10 == n));
            PrintHelper.PrintVerifyResult(string.Format("尾数定胆计算法结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_Mantissa, string.Join(" ", nums_Mantissa)));

            var nums_Middle = SelectHelper.CalulateProbableMiddle(false);
            var count_Middle = standardRedLotto.Count(n => nums_Middle.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("中间数定胆计算法结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_Middle, string.Join(" ", nums_Middle)));

            var nums_GoldedCut = SelectHelper.CalulateProbableGoldedCut(false);
            var count_GoldedCut = standardRedLotto.Count(n => nums_GoldedCut.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("黄金分割定胆计算法结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_GoldedCut, string.Join(" ", nums_GoldedCut)));

            Console.WriteLine();
            Console.WriteLine();

            //依据各模型近期走势，推断下期号码
            //VerifyHelper.VerifyPastResults(isContainRecentLotto);
        }

        public static void GuessCurrentPeriodLotto()
        {
            //注意除法时的小数用double定义整形，以免出现错误
            var data = GetAllRedCombinations();
            PrintHelper.PrintForecastResult(string.Format("共{0}组初始数据进行过滤", data.Count));

            //data = new List<int[]>
            //{
            //    //new int[] { 1, 2, 3, 4, 5, 26 },
            //    //new int[] { 1, 2, 3, 4,26,27 },
            //    //new int[] { 1, 2, 3, 216,17,116  },
            //    //new int[] { 2, 6, 12, 29, 30, 31},
            //    new int[] { 2, 9, 12, 22, 32, 33},
            //};
            //2, 9, 12, 22, 25, 33, 16

            data = FilterHelper.FilterByOddEven(data, [4D / 2]);
            data = FilterHelper.FilterBySize(data, [4D / 2]);
            data = FilterHelper.FilterByPrimeComposite(data, [2D / 4, 3D / 3]);
            data = FilterHelper.FilterBySumMantissa(data, [6, 8]);
            data = FilterHelper.FilterBySumRegion(data, 123);
            data = FilterHelper.FilterBySumOfHeadAndTail(data, 37);
            data = FilterHelper.FilterBySpanOfHeadAndTail(data, 24, 27, 30); //24, 27, 30
            data = FilterHelper.FilterByAdjacentNumber(data, 2); //无连号或2个2连号
            data = FilterHelper.FilterByACValue(data, [6, 7, 8]); //6, 7, 8
            Console.WriteLine();

            data = FilterHelper.FilterByPlusAndSubtract(data, [2, 3, 4]);
            data = FilterHelper.FilterBySumDivision(data, [1, 2, 3, 4]);
            data = FilterHelper.FilterByNoveary(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByUndecimal(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByEveryPeriodNum(data, [1, 2]);
            data = FilterHelper.FilterByPrimeNum(data, [1, 2, 4]);
            data = FilterHelper.FilterByProbableMantissa(data, [0, 1, 2]);
            data = FilterHelper.FilterByProbableMiddle(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByProbableGoldedCut(data, [0, 1, 3]);
            Console.WriteLine();

            data = FilterHelper.FilterByFirstOddEven(data, 1);
            data = FilterHelper.FilterBySixthOddEven(data, 1);
            Console.WriteLine();

            //明暗点：113/95
            //明暗点尾数：0 1 3 4 5 6 8 9

            data.ToList().ForEach(group =>
            {
                PrintHelper.PrintForecastResult(group.ToD2String());
            });
        }

        private static IList<int[]> GetAllRedCombinations()
        {
            int N = 33, K = 6;
            var combinationCount = CommonHelper.CalculateCombinations(N, K); //1107568
            var combinations = CommonHelper.GenerateCombinations(N, K);

            return combinations;
        }

        public static void GuessCommonPeriodLotto()
        {
            //注意除法时的小数用double定义整形，以免出现错误
            var data = GetAllRedCombinations();
            PrintHelper.PrintForecastResult(string.Format("共{0}组初始数据进行过滤", data.Count));

            data = FilterHelper.FilterByOddEven(data, [2D/4, 3D/3, 4D / 2]);
            data = FilterHelper.FilterBySize(data, [2D/4, 3D/3, 4D / 2, 5D/1]);
            data = FilterHelper.FilterByPrimeComposite(data, [1D/5, 2D / 4, 3D / 3]);
            data = FilterHelper.FilterBySumMantissa(data, [8, 9]);
            data = FilterHelper.FilterBySumRegion(data, 2);
            data = FilterHelper.FilterBySumOfHeadAndTail(data, 37);
            data = FilterHelper.FilterBySpanOfHeadAndTail(data, 24, 27, 30); //24, 27, 30
            data = FilterHelper.FilterByAdjacentNumber(data, 0); //无连号或2个2连号
            data = FilterHelper.FilterByACValue(data, [6, 7, 8]); //6, 7, 8
            Console.WriteLine();

            data = FilterHelper.FilterByPlusAndSubtract(data, [2, 3, 4]);
            data = FilterHelper.FilterBySumDivision(data, [1, 2, 3, 4]);
            data = FilterHelper.FilterByNoveary(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByUndecimal(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByEveryPeriodNum(data, [1, 2]);
            data = FilterHelper.FilterByPrimeNum(data, [1, 2, 4]);
            data = FilterHelper.FilterByProbableMantissa(data, [0, 1, 2]);
            data = FilterHelper.FilterByProbableMiddle(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByProbableGoldedCut(data, [0, 1, 3]);
            Console.WriteLine();

            //data = FilterHelper.FilterByFirstOddEven(data, 1);
            //data = FilterHelper.FilterBySixthOddEven(data, 1);
            //Console.WriteLine();

            //data.ToList().ForEach(group =>
            //{
            //    PrintHelper.PrintForecastResult(group.ToD2String());
            //});
        }
    }
}
