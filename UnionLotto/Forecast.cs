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
            Data.PreRedBlueLotto = new int[7] { 2, 6, 10, 11, 17, 29, 15 }; //46期开奖结果

            var nums_PlusSubtract = SelectHelper.CalculatePlusAndSubtractLotto(false);
            var count_PlusSubtract = standardRedLotto.Count(n => nums_PlusSubtract.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("两两加减计算法得到结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_PlusSubtract, string.Join(" ", nums_PlusSubtract)));

            var nums_SumDivision = SelectHelper.CalculateSumDivisionLotto(false);
            var count_SumDivision = standardRedLotto.Count(n => nums_SumDivision.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("和值取商计算法得到结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_SumDivision, string.Join(" ", nums_SumDivision)));

            var nums_2 = SelectHelper.Calculate2911Lotto(2, false);
            var count_2 = standardRedLotto.Count(n => nums_2.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("2进制计算法得到结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_2, string.Join(" ", nums_2)));

            var nums_9 = SelectHelper.Calculate2911Lotto(9, false);
            var count_9 = standardRedLotto.Count(n => nums_9.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("9进制计算法得到结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_9, string.Join(" ", nums_9)));

            var nums_11 = SelectHelper.Calculate2911Lotto(11, false);
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

            var nums_Middle = SelectHelper.CalculateProbableMiddle(false);
            var count_Middle = standardRedLotto.Count(n => nums_Middle.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("中间数定胆计算法结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_Middle, string.Join(" ", nums_Middle)));

            var nums_GoldedCut = SelectHelper.CalculateProbableGoldedCut(false);
            var count_GoldedCut = standardRedLotto.Count(n => nums_GoldedCut.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("黄金分割定胆计算法结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_GoldedCut, string.Join(" ", nums_GoldedCut)));

            var nums_regular = SelectHelper.CalculateProbableRed(false);
            var count_regular = standardRedLotto.Count(n => nums_regular.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("规律计算定胆法结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_regular, string.Join(" ", nums_regular)));

            var nums_sumSubtract = SelectHelper.CalculateSumSubtract(false);
            var count_sumSubtract = standardRedLotto.Count(n => nums_sumSubtract.Contains(n));
            PrintHelper.PrintVerifyResult(string.Format("差值之和定胆法结果(命中{1}个)：{2}", Data.CurrentPeriod - 1, count_sumSubtract, string.Join(" ", nums_sumSubtract)));

            Console.WriteLine();
            Console.WriteLine();

            //依据各模型近期走势，推断下期号码
            //VerifyHelper.VerifyPastResults(isContainRecentLotto);
        }

        //44期
        //明暗点：75 / 75
        //明暗点尾数：0 2 3 5 7 8
        //45期
        //明暗点：91/82
        //明暗点尾数：1 2 3 4 6 7 8 9
        //46期
        //明暗点：89/89
        //明暗点尾数：3 4 8 9
        //47期
        //明暗点：95/95
        //明暗点尾数：0 4 5 9
        //48期
        //明暗点：107/107
        //明暗点尾数：0 2 3 5 7 8


        public static void GuessCurrentPeriodLotto()
        {
            Console.WriteLine("------------常规过滤结果------------");
            Console.WriteLine();

            //注意除法时的小数用double定义整形，以免出现错误
            var data = GetAllRedCombinations();
            PrintHelper.PrintForecastResult(string.Format("共{0}组初始数据进行过滤", data.Count));

            //data = new List<int[]>
            //{
            //    new int[] { 05, 06 ,13, 23, 24, 25},
            //    new int[] { 2, 6, 10, 11, 17, 29},
            //    new int[] { 7, 8, 21, 26, 29, 30},
            //    new int[] { 2, 9, 15, 19, 26, 28},
            //    new int[] { 12, 15, 17, 23,26, 32},
            //};

#if COMMON
            data = FilterHelper.FilterByOddEven(data, [3D / 3, 2D / 4, 4D / 2, 1D / 5, 5D / 1]);
            data = FilterHelper.FilterBySize(data, [3D / 3, 2D / 4, 4D / 2, 5D / 1, 1D / 5]);
            data = FilterHelper.FilterByPrimeComposite(data, [2D / 4, 1D / 5, 3D / 3, 4D / 2]);
            //data = FilterHelper.FilterBySumRegion(data, 2);

            data = FilterHelper.FilterByPlusAndSubtract(data, [2, 3, 4, 5]);
            data = FilterHelper.FilterBySumDivision(data, [1, 2, 3, 4]);
            data = FilterHelper.FilterByBinary(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByNoveary(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByUndecimal(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByEveryPeriodNum(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByEdge(data, [1, 2, 3, 4]);
            data = FilterHelper.FilterByPrimeNum(data, [1, 2, 3, 4]);
            data = FilterHelper.FilterByProbableMantissa(data, [0, 1, 2]);
            data = FilterHelper.FilterByProbableMiddle(data, [0, 1, 2]);
            data = FilterHelper.FilterByProbableGoldedCut(data, [0, 1, 2, 3]);
            data = FilterHelper.FilterByProbableRegular(data, [0, 1, 2]);
            data = FilterHelper.FilterBySumSubtract(data, [0, 1, 2]);
            data = FilterHelper.FilterByThreeDistance(data, [0, 1, 2, 3]);
            Console.WriteLine();
#else
            //data = FilterHelper.FilterBySize(data, [2D / 4, 3D / 3, 5D / 1]);
            //data = FilterHelper.FilterByOddEven(data, [3D / 3, 2D / 4, 4D / 2]);
            //data = FilterHelper.FilterByPrimeComposite(data, [1D / 5, 2D / 4, 3D / 3]);
            //Console.WriteLine();

            //data = FilterHelper.FilterByPlusAndSubtract(data, [2, 3, 4]);
            //data = FilterHelper.FilterBySumDivision(data, [1, 2, 3, 4]); //1
            //data = FilterHelper.FilterByBinary(data, [0, 1, 2, 3]); //3
            //data = FilterHelper.FilterByNoveary(data, [0, 1, 2, 3]);
            //data = FilterHelper.FilterByUndecimal(data, [1, 2, 3]);
            //data = FilterHelper.FilterByEveryPeriodNum(data, [1, 2, 3]);
            //data = FilterHelper.FilterByEdge(data, [1, 2, 3, 4]);
            //data = FilterHelper.FilterByPrimeNum(data, [1, 2, 3]);
            //data = FilterHelper.FilterByProbableMantissa(data, [0, 1, 2]);
            //data = FilterHelper.FilterByProbableMiddle(data, [0, 1, 2, 3]);
            //data = FilterHelper.FilterByProbableGoldedCut(data, [1, 2, 3]);
            //data = FilterHelper.FilterByProbableRegular(data, [0, 1, 2, 3]); //3
            //data = FilterHelper.FilterBySumSubtract(data, [0, 1, 2]);
            //data = FilterHelper.FilterByThreeDistance(data, [0, 1, 2, 3]);
            //Console.WriteLine();
#endif
            //data = FilterHelper.FilterByColdHotWarm(data, [6, 5]); 
            //data = FilterHelper.FilterByBigMiddleSmall(data, [6, 5]); 
            //data = FilterHelper.FilterBy012Path(data, [6, 5]); 
            //data = FilterHelper.FilterBySumRegion(data, 123); 
            //data = FilterHelper.FilterByAverage(data); 
            data = FilterHelper.FilterByMantissaSum(data); 
            data = FilterHelper.FilterBySumMantissa(data, [0]);
            //data = FilterHelper.FilterByAdjacentNumber(data, 0);
            //data = FilterHelper.FilterByACValue(data, [6, 9]);
            //Console.WriteLine();

            //data = FilterHelper.FilterBySumOf16(data); //质数，2路，质合合合...看趋势，质质合质与质质合合交叉进行
            //data = FilterHelper.FilterBySumOf25(data);  
            //data = FilterHelper.FilterBySumOf34(data);  
            data = FilterHelper.FilterBySpanOf16(data);
            //data = FilterHelper.FilterBySpanOf25(data); 
            //data = FilterHelper.FilterBySpanOf34(data); 
            //Console.WriteLine();

            /*************单个红球过滤*********************/


            //奇偶过滤
            data = FilterHelper.FilterByFirstOddEven(data, 0);
            data = FilterHelper.FilterBySecondOddEven(data, 0);
            //data = FilterHelper.FilterByThirdOddEven(data, 0);
            data = FilterHelper.FilterByFourthOddEven(data, 1);
            data = FilterHelper.FilterByFifthOddEven(data, 0);
            data = FilterHelper.FilterBySixthOddEven(data, 1);

            //质合过滤
            //data = FilterHelper.FilterByAnyPrimeComposite(data, 1, 0);
            data = FilterHelper.FilterByAnyPrimeComposite(data, 3, 0);
            data = FilterHelper.FilterByAnyPrimeComposite(data, 4, 0);
            //data = FilterHelper.FilterByAnyPrimeComposite(data, 6, 0); //不确定

            //012路过滤
            data = FilterHelper.FilterByFirst012Path(data, [0, 1]);
            data = FilterHelper.FilterBySecond012Path(data, [1, 2]);
            data = FilterHelper.FilterByThird012Path(data, [0, 2]);
            data = FilterHelper.FilterByFourth012Path(data, [1, 2]);
            data = FilterHelper.FilterByFifth012Path(data, [0, 1]);
            data = FilterHelper.FilterBySixth012Path(data, [1, 2]);
            Console.WriteLine();

            //升平降过滤
            data = FilterHelper.FilterByAnyValue(data, 1, 12, -1);
            data = FilterHelper.FilterByAnyValue(data, 2, 15, -1);
            //data = FilterHelper.FilterByAnyValue(data, 3, 17, -1); //不确定
            //data = FilterHelper.FilterByAnyValue(data, 4, 23, -1); //不确定
            data = FilterHelper.FilterByAnyValue(data, 5, 26, -11);
            data = FilterHelper.FilterByAnyValue(data, 6, 32, -10);

            //data = FilterHelper.FilterByPassword(data, [42]);
            //data = FilterHelper.FilterByCompositeNumber(data);

            SelectHelper.CalculateLightAndShade(data);

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
