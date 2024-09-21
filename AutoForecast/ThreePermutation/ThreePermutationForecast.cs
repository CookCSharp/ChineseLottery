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

        GetSingleLessThanTen(1, out var path012, out var size);
        var first = ThreePermutationTool.GetSingle(path012, size);
        GetSingleLessThanTen(2, out path012, out size);
        var second = ThreePermutationTool.GetSingle(path012, size);
        GetSingleLessThanTen(3, out path012, out size);
        var third = ThreePermutationTool.GetSingle(path012, size);
        PrintHelper.PrintResult("第一位号码：" + first.ToD2String());
        PrintHelper.PrintResult("第二位号码：" + second.ToD2String());
        PrintHelper.PrintResult("第三位号码：" + third.ToD2String());

        var groups = Tool.FindAllSortedCombinations(first, second, third);
        var result = groups
                .SumValue012PathFilter(GetSum012Path())
                .SumValueMantissa012PathFilter(GetSumValueMantissa012Path())
                .SpanValue012PathFilter(GetSpanValue012Path())
                .SumValueMantissaLargeMediumSmallFilter(GetSumValueMantissaLargeMediumSmall())
                .SpanValueLargeMediumSmallFilter(GetSpanValueLargeMediumSmall())
                .Sum12PathFilter(GetSum012Path(1, 2))
                .Sum13PathFilter(GetSum012Path(1, 3))
                .Sum23PathFilter(GetSum012Path(2, 3))
                .Span12PathFilter(GetSpan012Path(1, 2))
                .Span13PathFilter(GetSpan012Path(1, 3))
                .Span23PathFilter(GetSpan012Path(2, 3))
            ;

        PrintHelper.PrintForecastResult($"预测{NextPeriod}期排列三号码共{result.Count}组：", isNewLine: false);
        // PrintHelper.PrintForecastResult($"是否准确预测：{result.Any(g => g.SequenceEqual([4, 3, 0]))}");
        if (result.Count <= 5) result.ForEach(g => Console.WriteLine(g.ToD2String()));
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
            .FindSpanValue012Path(2, 3)
            ;

        int index = 3;
        int[] values = [2, 0, 0, 0, 1, 2, 2, 0];
        historyData.ManualFindValue012Path(index, values, out var periods);
        var enumerable1 = periods.Select(x => $"{historyData.ElementAt(x).Key}期").ToArray();
        var enumerable2 = periods.Where(x => x + values.Length < historyData.Count).Select(x => historyData.ElementAt(x + values.Length).Value[index - 1] % 3).ToArray();
    }

    protected override void HistoryValidation()
    {
        for (int i = 0; i < 20; i++)
        {
            var correctLotto = UpdateHistoryData();

            GetSingleLessThanTen(1, out var path012, out var size);
            var first = ThreePermutationTool.GetSingle(path012, size);
            GetSingleLessThanTen(2, out path012, out size);
            var second = ThreePermutationTool.GetSingle(path012, size);
            GetSingleLessThanTen(3, out path012, out size);
            var third = ThreePermutationTool.GetSingle(path012, size);
            PrintHelper.PrintResult("第一位号码：" + first.ToD2String());
            PrintHelper.PrintResult("第二位号码：" + second.ToD2String());
            PrintHelper.PrintResult("第三位号码：" + third.ToD2String());

            var groups = Tool.FindAllSortedCombinations(first, second, third);
            var result = groups
                .SumValue012PathFilter(GetSum012Path())
                .SumValueMantissa012PathFilter(GetSumValueMantissa012Path())
                .SpanValue012PathFilter(GetSpanValue012Path())
                .SumValueMantissaLargeMediumSmallFilter(GetSumValueMantissaLargeMediumSmall())
                .SpanValueLargeMediumSmallFilter(GetSpanValueLargeMediumSmall())
                .Sum12PathFilter(GetSum012Path(1, 2))
                .Sum13PathFilter(GetSum012Path(1, 3))
                .Sum23PathFilter(GetSum012Path(2, 3))
                .Span12PathFilter(GetSpan012Path(1, 2))
                .Span13PathFilter(GetSpan012Path(1, 3))
                .Span23PathFilter(GetSpan012Path(2, 3));

            PrintHelper.PrintForecastResult($"预测{NextPeriod}期排列三号码共{result.Count}组：", isNewLine: false);
            PrintHelper.PrintForecastResult($"是否准确预测({string.Join(" ", correctLotto)})：{result.Any(g => g.SequenceEqual(correctLotto))}");
        }
    }

    public void ManualForecast()
    {
        int[] first = [0, 1, 2];
        int[] second = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        int[] third = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        var groups = Tool.FindAllSortedCombinations(first, second, third);
        var result = groups
            .SumValue012PathFilter(GetSum012Path())
            .SumValueMantissa012PathFilter(GetSumValueMantissa012Path())
            .SpanValue012PathFilter(GetSpanValue012Path())
            .SumValueMantissaLargeMediumSmallFilter(GetSumValueMantissaLargeMediumSmall())
            .SpanValueLargeMediumSmallFilter(GetSpanValueLargeMediumSmall())
            .Sum12PathFilter(GetSum012Path(1, 2))
            .Sum13PathFilter(GetSum012Path(1, 3))
            .Sum23PathFilter(GetSum012Path(2, 3))
            .Span12PathFilter(GetSpan012Path(1, 2))
            .Span13PathFilter(GetSpan012Path(1, 3))
            .Span23PathFilter(GetSpan012Path(2, 3));

        PrintHelper.PrintForecastResult($"预测{NextPeriod}期排列三号码共{result.Count}组：", isNewLine: false);
        // result.ForEach(g => Console.WriteLine(g.ToD2String()));
    }
}