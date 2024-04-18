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

        public static void GuessCommonPeriodLotto()
        {
            Console.WriteLine("------------常规过滤结果------------");
            Console.WriteLine();

            //注意除法时的小数用double定义整形，以免出现错误
            var data = GetAllRedCombinations();
            PrintHelper.PrintForecastResult(string.Format("共{0}组初始数据进行过滤", data.Count));

            //data = new List<int[]>
            //{
            //    new int[] { 05, 06 ,13, 23, 24, 25},
            //};

#if COMMON
            data = FilterHelper.FilterByOddEven(data, [2D / 4, 3D / 3, 4D / 2, 1D / 5, 5D / 1]); //2D/4,3D/3,4D/2,1D/5,5D/1
            data = FilterHelper.FilterBySize(data, [2D / 4, 3D / 3, 4D / 2, 5D / 1]); //2D/4,3D/3,4D/2,5D/1
            data = FilterHelper.FilterByPrimeComposite(data, [1D / 5, 2D / 4, 3D / 3, 4D / 2]); //1D/5,2D/4,3D/3,4D/2
            data = FilterHelper.FilterBySumMantissa(data, [8, 6, 9]);
            data = FilterHelper.FilterByMantissaSum(data, [20, 23, 25, 28]);
            data = FilterHelper.FilterBySumRegion(data, 123);
            data = FilterHelper.FilterBySumOfHeadAndTail(data, 123);
            data = FilterHelper.FilterBySpanOfHeadAndTail(data, 20, 23, 24, 25, 27, 28, 29);
            data = FilterHelper.FilterByACValue(data, [8, 9, 6, 7]);
            data = FilterHelper.FilterByAdjacentNumber(data, 22); //2个2连或1个3连
            data = FilterHelper.FilterBy012Path(data, [6, 5]);
            data = FilterHelper.FilterByBigMiddleSmall(data, [6, 5]);
            Console.WriteLine();

            data = FilterHelper.FilterByPlusAndSubtract(data, [2, 4, 3, 5]);
            data = FilterHelper.FilterBySumDivision(data, [3, 4, 1, 2]);
            data = FilterHelper.FilterByNoveary(data, [1, 2, 3, 0]);
            data = FilterHelper.FilterByUndecimal(data, [2, 0, 1, 3]);
            data = FilterHelper.FilterByEveryPeriodNum(data, [2, 1, 0, 3]);
            data = FilterHelper.FilterByPrimeNum(data, [3, 1, 2, 4]);
            data = FilterHelper.FilterByProbableMantissa(data, [0, 2, 1]);
            data = FilterHelper.FilterByProbableMiddle(data, [0, 2, 1]);
            data = FilterHelper.FilterByProbableGoldedCut(data, [1, 0, 2, 3, 4]);
            Console.WriteLine();
#else
            data = FilterHelper.FilterByOddEven(data, [2D / 4, 3D / 3, 4D / 2, 1D / 5, 5D / 1]); //2D/4,3D/3,4D/2,1D/5,5D/1
            data = FilterHelper.FilterBySize(data, [2D / 4, 3D / 3, 4D / 2, 5D / 1]); //2D/4,3D/3,4D/2,5D/1
            data = FilterHelper.FilterByPrimeComposite(data, [1D / 5, 2D / 4, 3D / 3, 4D / 2]); //1D/5,2D/4,3D/3,4D/2
            data = FilterHelper.FilterBySumMantissa(data, [8, 6, 9]);
            data = FilterHelper.FilterBySumRegion(data, 123);
            data = FilterHelper.FilterBySumOfHeadAndTail(data, 123);
            data = FilterHelper.FilterBySpanOfHeadAndTail(data, 20, 23, 24, 25, 27, 28, 29);
            data = FilterHelper.FilterByACValue(data, [8, 9, 6, 7]);
            data = FilterHelper.FilterByAdjacentNumber(data, 3); //2个2连或1个3连
            data = FilterHelper.FilterBy012Path(data, [6, 5]);
            data = FilterHelper.FilterByBigMiddleSmall(data, [6, 5]);
            Console.WriteLine();

            data = FilterHelper.FilterByPlusAndSubtract(data, [2, 4, 3, 5]);
            data = FilterHelper.FilterBySumDivision(data, [3, 4, 1]);
            data = FilterHelper.FilterByNoveary(data, [1, 2, 3]);
            data = FilterHelper.FilterByUndecimal(data, [2, 0]);
            data = FilterHelper.FilterByEveryPeriodNum(data, [2, 1, 0]);
            data = FilterHelper.FilterByPrimeNum(data, [3, 1]);
            data = FilterHelper.FilterByProbableMantissa(data, [0, 2]);
            data = FilterHelper.FilterByProbableMiddle(data, [0, 2]);
            data = FilterHelper.FilterByProbableGoldedCut(data, [1, 0, 2, 3]);
            Console.WriteLine();
#endif

            data = FilterHelper.FilterByFirstValue(data);
            data = FilterHelper.FilterBySixthValue(data);
            Console.WriteLine();

            data.ToList().ForEach(group =>
            {
                PrintHelper.PrintForecastResult(group.ToD2String());
            });
        }

        public static void GuessCurrentPeriodLotto()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("------------精简过滤结果------------");
            Console.WriteLine();

            //注意除法时的小数用double定义整形，以免出现错误
            var data = GetAllRedCombinations();
            PrintHelper.PrintForecastResult(string.Format("共{0}组初始数据进行过滤", data.Count));

            //data = new List<int[]>
            //{
            //    new int[] { 2, 4, 5, 24, 26, 32},
            //    new int[] { 22, 24, 25, 27, 26, 32},
            //};

            data = FilterHelper.FilterByOddEven(data, [4D / 2, 2D / 4]);
            data = FilterHelper.FilterBySize(data, [3D / 3, 2D / 4]);
            data = FilterHelper.FilterByPrimeComposite(data, [2D / 4, 3D / 3]);
            data = FilterHelper.FilterBySumMantissa(data, [8, 6, 9]);
            data = FilterHelper.FilterBySumRegion(data, 123);
            data = FilterHelper.FilterBySumOfHeadAndTail(data, 123);
            data = FilterHelper.FilterBySpanOfHeadAndTail(data, 29, 23);
            data = FilterHelper.FilterByACValue(data, [8, 9, 6, 7]);
            data = FilterHelper.FilterByAdjacentNumber(data, 3); //2个2连或1个3连
            data = FilterHelper.FilterBy012Path(data, [6, 5]);
            data = FilterHelper.FilterByBigMiddleSmall(data, [6, 5]);
            Console.WriteLine();

            data = FilterHelper.FilterByPlusAndSubtract(data, [2, 4, 3, 5]);
            data = FilterHelper.FilterBySumDivision(data, [3, 4, 1]);
            data = FilterHelper.FilterByNoveary(data, [1, 2, 3]);
            data = FilterHelper.FilterByUndecimal(data, [2, 0]);
            data = FilterHelper.FilterByEveryPeriodNum(data, [2, 1, 0]);
            data = FilterHelper.FilterByPrimeNum(data, [3, 1]);
            data = FilterHelper.FilterByProbableMantissa(data, [0, 2]);
            data = FilterHelper.FilterByProbableMiddle(data, [0, 2]);
            data = FilterHelper.FilterByProbableGoldedCut(data, [1, 0, 2, 3]);
            Console.WriteLine();

            data = FilterHelper.FilterByFirstValue(data);
            data = FilterHelper.FilterBySixthValue(data);
            Console.WriteLine();

            //明暗点：121/76
            //明暗点尾数：1 2 4 6 7 9
            //蓝号开出同尾数

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
    }
}
