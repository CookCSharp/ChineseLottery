/*
 *Description: Forecast
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 10:25:24
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


namespace ThreeDLotto;

public class Forecast
{
    public static void GuessCurrentPeriodLotto()
    {
        //注意除法时的小数用double定义整形，以免出现错误
        var data = Common.GetAllCombinations(10, 3);
        PrintHelper.PrintForecastResult(string.Format("共{0}组初始数据进行过滤", data.Count));

        //data = new List<int[]>()
        //{
        //    new int[] { 3,9,7 }
        //};

        //data = FilterHelper.FilterByCurrentPeriodMantissa(data);
        //data = FilterHelper.FilterByRelation(data);

        //data = FilterHelper.FilterByPreLottoPower();
        //data = FilterHelper.FilterByPreLottoSum(data);
        //data = FilterHelper.FilterBySpanSumMantissa(data);
        //data = FilterHelper.FilterByCurrentPeriodTwoMantissa(data);
        //data = FilterHelper.FilterByCurrentPeriodAndSumMantissa(data);
        //data = FilterHelper.FilterByMoreComposition(data);
        //Console.WriteLine();

        //上期开组三时使用
        //data = FilterHelper.FilterByGroupThreePreLotto(data);

        //data = FilterHelper.FilterByDistance5(data, [1, 2]);
        //data = FilterHelper.FilterByPreLotto(data, [0, 1]);

        //data = FilterHelper.FilterByLastTwoPeriodTens(data);
        //data = FilterHelper.FilterByOtherComposition(data);
        //data = FilterHelper.FilterBySpecific(data);

        //data = FilterHelper.FilterByHundreds012Path(data, [1, 2]);
        //data = FilterHelper.FilterByTens012Path(data, [0, 2]);
        //data = FilterHelper.FilterByUnits012Path(data, [0, 1]);
        //data = FilterHelper.FilterByHundreds(data);
        //data = FilterHelper.FilterByTens(data);
        //data = FilterHelper.FilterByUnits(data);
        //Console.WriteLine();

        // data = FilterHelper.FilterByHundredsValue(data, [3,7,8]);
        // data = FilterHelper.FilterByTensValue(data, [2,7,8]);
        // data = FilterHelper.FilterByUnitsValue(data, [5,6,7,8,9,0]);
        // Console.WriteLine();

        //data = FilterHelper.FilterByUnitsValue(data, [6]);

        //data = FilterHelper.FilterBySum(data);
        //data = FilterHelper.FilterBySumMantissa(data);
        //data = FilterHelper.FilterBySpan(data);
        //data = FilterHelper.FilterBySumValue(data, [6,16,19,5,8,11,18,20,24]); //6,16,19,5,8,11,18,20,24
        // data = FilterHelper.FilterBySpanValue(data, [3,7,0,5,8]);
        // data = FilterHelper.FilterBySumMantissaValue(data, [6,0]);
        //
        // Console.WriteLine();

        //data = FilterHelper.FilterBySumOf12(data);
        //data = FilterHelper.FilterBySumOf13(data);
        //data = FilterHelper.FilterBySumOf23(data);
        //data = FilterHelper.FilterBySumOf12Value(data, []);
        //data = FilterHelper.FilterBySumOf13Value(data, []);
        //data = FilterHelper.FilterBySumOf23Value(data, [5, 6, 8, 9, 11, 13, 14, 16, 17]);
        //Console.WriteLine();

        //data = FilterHelper.FilterBySpanOf12(data);
        //data = FilterHelper.FilterBySpanOf13(data);
        //data = FilterHelper.FilterBySpanOf23(data);
        //data = FilterHelper.FilterBySpanOf12Value(data, [3,4,5,6,7,8,9]);
        //data = FilterHelper.FilterBySpanOf13Value(data, [0,1,3,4,6,7,9]);
        //data = FilterHelper.FilterBySpanOf23Value(data, [1,2,4,5,7,8]);
        //Console.WriteLine();


        // // 3D
        // // data = FilterHelper.FilterByHundredsValue(data, [1,7]);
        // data = FilterHelper.FilterByTensValue(data, [2,5,8,0,3,6,9]);
        // data = FilterHelper.FilterByUnitsValue(data, [0,3,6,9]);
        // Console.WriteLine();
        // data = FilterHelper.FilterBySumValue(data, [18,20,24]); //18,20,21,23,24,26,27
        // data = FilterHelper.FilterBySpanValue(data, [0,1,2,8,9]);
        // data = FilterHelper.FilterBySumMantissaValue(data, [1,2,7,8,9]);


        // 排3
        // data = FilterHelper.FilterByHundredsValue(data, [0,2,8,9]);
        // data = FilterHelper.FilterByTensValue(data, [0,1,2]);
        // data = FilterHelper.FilterByUnitsValue(data, [6,7,8,9]);
        // Console.WriteLine();
        data = FilterHelper.FilterBySum(data);
        // data = FilterHelper.FilterBySumValue(data,[1,4,5,8,16,0,9,15]);
        // data = FilterHelper.FilterBySpanValue(data, [0,1,2,7,8,9]);
        data = FilterHelper.FilterBySumMantissaValue(data, [0,1,3,4,6,7,9]);

        data = FilterHelper.FilterBySumOf12(data);
        data = FilterHelper.FilterBySumOf13(data);
        data = FilterHelper.FilterBySumOf23(data); //\\
        // data = FilterHelper.FilterBySpanOf12(data);
        data = FilterHelper.FilterBySpanOf13(data);
        data = FilterHelper.FilterBySpanOf23(data);

        data.ToList().ForEach(group => PrintHelper.PrintForecastResult(string.Join(" ", group)));
    }
}

public class Common
{
    /// <summary>
    /// 从N个元素中选取K个元素进行的排列
    /// </summary>
    /// <param name="n"></param>
    /// <param name="k"></param>
    /// <returns>排列方式集合</returns>
    public static IList<int[]> GetAllCombinations(int n, int k)
    {
        var combinations = new List<int[]>();
        var currentCombination = new int[k];
        RecursiveLoop(combinations, currentCombination, 0, n, k);
        return combinations;
    }

    private static void RecursiveLoop(List<int[]> combinations, int[] currentCombination, int start, int n, int k)
    {
        if (k == 0)
        {
            var newCombination = new int[currentCombination.Length];
            Array.Copy(currentCombination, newCombination, currentCombination.Length);
            combinations.Add(newCombination);
            return;
        }

        for (var i = start; i < n; i++)
        {
            currentCombination[currentCombination.Length - k] = Data.AllLotto[i];
            RecursiveLoop(combinations, currentCombination, 0, n, k - 1);
        }
    }
}