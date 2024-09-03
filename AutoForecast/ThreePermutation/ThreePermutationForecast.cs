/*
 * Description: ThreePermutationForecast
 * Author: Chance.zheng
 * Creat Time: 2024/09/02 17:23:38 星期一
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


namespace AutoForecast;

// https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=35&provinceId=0&pageSize=100&isVerify=1&pageNo=1

public class ThreePermutationForecast : ForecastBase
{
    private static readonly Lazy<ThreePermutationForecast> _lazy = new Lazy<ThreePermutationForecast>(() => new ThreePermutationForecast());
    public static ThreePermutationForecast Instance => _lazy.Value;

    static ThreePermutationForecast() { }
    private ThreePermutationForecast() { }

    protected override void Forecast()
    {
        // int[] first = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        // int[] second = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        // int[] third = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        GetSingleLessThanTen(1, out var way012, out var size);
        var first = ThreePermutationTool.GetSingle(way012, size);
        GetSingleLessThanTen(2, out way012, out size);
        var second = ThreePermutationTool.GetSingle(way012, size);
        GetSingleLessThanTen(3, out way012, out size);
        var third = ThreePermutationTool.GetSingle(way012, size);
        PrintHelper.PrintResult("第一位号码：" + first.ToD2String());
        PrintHelper.PrintResult("第二位号码：" + second.ToD2String());
        PrintHelper.PrintResult("第三位号码：" + third.ToD2String());

        var groups = Tool.FindAllSortedCombinations(first, second, third);
        var result = groups
            .SumValue012PathFilter(GetSum012Path())
            .SumValueMantissa012PathFilter(GetSumValueMantissa012Path())
            .SpanValue012PathFilter(GetSpanValue012Path())
            .SumValueMantissaLargeMediumSmallFilter(GetSumValueMantissaLargeMediumSmall())
            .SpanValueLargeMediumSmallFilter(GetSpanValueLargeMediumSmallValue())
            .Sum12Filter(GetSumWay012(1, 2))
            .Sum13Filter(GetSumWay012(1, 3))
            .Sum23Filter(GetSumWay012(2, 3))
            .Span12Filter(GetSpanWay012(1, 2))
            .Span13Filter(GetSpanWay012(1, 3))
            .Span23Filter(GetSpanWay012(2, 3));

        PrintHelper.PrintForecastResult($"预测{NextPeriod}期排列三号码共{result.Count}组：", isNewLine: false);
        PrintHelper.PrintForecastResult($"是否准确预测：{result.Any(g => g.SequenceEqual([2, 7, 5]))}", isNewLine: false);
        // result.ForEach(g => Console.WriteLine(g.ToD2String()));
    }

    protected override void Find(Dictionary<string, IList<int>> historyData)
    {
        historyData
            .FindValue012Path(1)
            .FindValueLargeMediumSmall(1)
            .FindValue012Path(2)
            .FindValueLargeMediumSmall(2)
            .FindValue012Path(3)
            .FindValueLargeMediumSmall(3)
            .FindSumValue012Path()
            .FindSumValueMantissa012Path()
            .FindSumValueMantissaLargeMediumSmall()
            .FindSpanValue012Path()
            .FindSpanValueLargeMediumSmall()
            .FindSumValue012Path(1, 2)
            .FindSumValue012Path(1, 3)
            .FindSumValue012Path(2, 3)
            .FindSpanValue012Path(1, 2)
            .FindSpanValue012Path(1, 3)
            .FindSpanValue012Path(2, 3);
    }

    protected override void HistoryForecast()
    {
        for (int i = 0; i < 20; i++)
        {
            var correctLotto = UpdateHistoryData();

            GetSingleLessThanTen(1, out var way012, out var size);
            var first = ThreePermutationTool.GetSingle(way012, size);
            GetSingleLessThanTen(2, out way012, out size);
            var second = ThreePermutationTool.GetSingle(way012, size);
            GetSingleLessThanTen(3, out way012, out size);
            var third = ThreePermutationTool.GetSingle(way012, size);
            PrintHelper.PrintResult("第一位号码：" + first.ToD2String());
            PrintHelper.PrintResult("第二位号码：" + second.ToD2String());
            PrintHelper.PrintResult("第三位号码：" + third.ToD2String());

            var groups = Tool.FindAllSortedCombinations(first, second, third);
            var result = groups
                .SumValue012PathFilter(GetSum012Path())
                .SumValueMantissa012PathFilter(GetSumValueMantissa012Path())
                .SpanValue012PathFilter(GetSpanValue012Path())
                .SumValueMantissaLargeMediumSmallFilter(GetSumValueMantissaLargeMediumSmall())
                .SpanValueLargeMediumSmallFilter(GetSpanValueLargeMediumSmallValue())
                .Sum12Filter(GetSumWay012(1, 2))
                .Sum13Filter(GetSumWay012(1, 3))
                .Sum23Filter(GetSumWay012(2, 3))
                .Span12Filter(GetSpanWay012(1, 2))
                .Span13Filter(GetSpanWay012(1, 3))
                .Span23Filter(GetSpanWay012(2, 3));

            PrintHelper.PrintForecastResult($"预测{NextPeriod}期排列三号码共{result.Count}组：", isNewLine: false);
            PrintHelper.PrintForecastResult($"是否准确预测({string.Join(" ", correctLotto)})：{result.Any(g => g.SequenceEqual(correctLotto))}", isNewLine: false);
        }
    }
}