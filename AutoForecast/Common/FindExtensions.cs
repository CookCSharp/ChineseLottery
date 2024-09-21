/*
 * Description: FindExtensions
 * Author: Chance.zheng
 * Creat Time: 2024/09/01 10:11:32 星期日
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


namespace AutoForecast;

public static class FindExtensions
{
    private const int RegularLength = 15; //规律数组长度
    // private const int MinPeriodCount = 3; //找到满足条件的最小周期数量

    private static int MinPeriodCount = 3; //找到满足条件的最小周期数量 

    public static void SetMinPeriodCount(int period) => MinPeriodCount = period;

    public static Dictionary<string, IList<int>> FindValue012Path(this Dictionary<string, IList<int>> historyData, int index)
    {
        historyData.AutoFind(g => g[index - 1] % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足第{index}位值012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{index}位下期值012路为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => historyData.ElementAt(x + values.Count).Value[index - 1] % 3)));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindValueMantissa012Path(this Dictionary<string, IList<int>> historyData, int index)
    {
        historyData.AutoFind(g => g[index - 1] % 10 % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足第{index}位尾数012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{index}位下期尾数012路为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => historyData.ElementAt(x + values.Count).Value[index - 1] % 10 % 3)));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindValueMantissaLargeMediumSmall(this Dictionary<string, IList<int>> historyData, int index)
    {
        historyData.AutoFind(g => (g[index - 1] % 10).ToLargeMediumSmallConverterValue(), out var values, out var periods);

        PrintHelper.PrintResult($"满足第{index}位尾数大中小规律：{string.Join(" ", values.ToLargeMediumSmallDescription())} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{index}位下期尾数大中小为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => (historyData.ElementAt(x + values.Count).Value[index - 1] % 10).ToLargeMediumSmallDescription())));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSumValue012Path(this Dictionary<string, IList<int>> historyData, int first, int second)
    {
        historyData.AutoFind(g => (g[first - 1] + g[second - 1]) % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足第{first}、{second}位和值012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{first}、{second}位下期和值012路为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => (historyData.ElementAt(x + values.Count).Value[first - 1] + historyData.ElementAt(x + values.Count).Value[second - 1]) % 3)));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSumValueMantissa012Path(this Dictionary<string, IList<int>> historyData, int first, int second)
    {
        historyData.AutoFind(g => (g[first - 1] + g[second - 1]) % 10 % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足第{first}、{second}位和值尾012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{first}、{second}位下期和值尾012路为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => (historyData.ElementAt(x + values.Count).Value[first - 1] + historyData.ElementAt(x + values.Count).Value[second - 1]) % 10 % 3)));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSumValueMantissaLargeMediumSmall(this Dictionary<string, IList<int>> historyData, int first, int second)
    {
        historyData.AutoFind(g => ((g[first - 1] + g[second - 1]) % 10).ToLargeMediumSmallConverterValue(), out var values, out var periods);

        PrintHelper.PrintResult($"满足{first}、{second}和值尾大中小规律：{string.Join(" ", values.ToLargeMediumSmallDescription())} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测{first}、{second}和值尾下期值大中小为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => ((historyData.ElementAt(x + values.Count).Value[first - 1] + historyData.ElementAt(x + values.Count).Value[second - 1]) % 10).ToLargeMediumSmallDescription())));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSpanValue012Path(this Dictionary<string, IList<int>> historyData, int first, int second)
    {
        historyData.AutoFind(g => Math.Abs(g[second - 1] - g[first - 1]) % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足第{first}、{second}位差值012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{first}、{second}位下期差值012路为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => Math.Abs(historyData.ElementAt(x + values.Count).Value[second - 1] - historyData.ElementAt(x + values.Count).Value[first - 1]) % 3)));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSpanValueMantissa012Path(this Dictionary<string, IList<int>> historyData, int first, int second)
    {
        historyData.AutoFind(g => Math.Abs(g[second - 1] - g[first - 1]) % 10 % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足第{first}、{second}位差值尾012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{first}、{second}位下期差值尾012路为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => Math.Abs(historyData.ElementAt(x + values.Count).Value[second - 1] - historyData.ElementAt(x + values.Count).Value[first - 1]) % 10 % 3)));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSpanValueMantissaLargeMediumSmall(this Dictionary<string, IList<int>> historyData, int first, int second)
    {
        historyData.AutoFind(g => (Math.Abs(g[second - 1] - g[first - 1]) % 10).ToLargeMediumSmallConverterValue(), out var values, out var periods);

        PrintHelper.PrintResult($"满足{first}、{second}差值尾大中小规律：{string.Join(" ", values.ToLargeMediumSmallDescription())} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测{first}、{second}差值尾下期值大中小为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => (Math.Abs(historyData.ElementAt(x + values.Count).Value[second - 1] - historyData.ElementAt(x + values.Count).Value[first - 1]) % 10).ToLargeMediumSmallDescription())));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindAmplitudeValue012Path(this Dictionary<string, IList<int>> historyData, int index)
    {
        IList<int> amplitudeValues = new List<int>();
        for (int i = 0; i < historyData.Values.Count - 1; i++)
        {
            var amplitudeValue = Math.Abs(historyData.Values.ElementAt(i + 1).ElementAt(index - 1) - historyData.Values.ElementAt(i).ElementAt(index - 1));
            amplitudeValues.Add(amplitudeValue);
        }

        amplitudeValues.AutoFind(v => v % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足第{index}位振幅012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x + 1).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{index}位下期振幅012路为：" + string.Join(" ", periods.Where(x => x + values.Count < amplitudeValues.Count).Select(x => amplitudeValues.ElementAt(x + values.Count) % 3)));

        return historyData;
    }


    #region 适合排列三和3D

    public static Dictionary<string, IList<int>> FindValueLargeMediumSmall(this Dictionary<string, IList<int>> historyData, int index)
    {
        historyData.AutoFind(g => g[index - 1].ToLargeMediumSmallConverterValue(), out var values, out var periods);

        PrintHelper.PrintResult($"满足第{index}位值大中小规律：{string.Join(" ", values.ToLargeMediumSmallDescription())} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{index}位下期值大中小为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => historyData.ElementAt(x + values.Count).Value[index - 1].ToLargeMediumSmallDescription())));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSumValueMantissaLargeMediumSmall(this Dictionary<string, IList<int>> historyData)
    {
        historyData.AutoFind(g => (g.Sum() % 10).ToLargeMediumSmallConverterValue(), out var values, out var periods);

        PrintHelper.PrintResult($"满足和值尾大中小规律：{string.Join(" ", values.ToLargeMediumSmallDescription())} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult("预测和值尾下期值大中小为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => (historyData.ElementAt(x + values.Count).Value.Sum() % 10).ToLargeMediumSmallDescription())));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSpanValueLargeMediumSmall(this Dictionary<string, IList<int>> historyData)
    {
        historyData.AutoFind(g => (g.Max() - g.Min()).ToLargeMediumSmallConverterValue(), out var values, out var periods);

        PrintHelper.PrintResult($"满足跨度大中小规律：{string.Join(" ", values.ToLargeMediumSmallDescription())} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult("预测跨度下期值大中小为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => (historyData.ElementAt(x + values.Count).Value.Max() - historyData.ElementAt(x + values.Count).Value.Min()).ToLargeMediumSmallDescription())));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSumValue012Path(this Dictionary<string, IList<int>> historyData)
    {
        historyData.AutoFind(g => g.Sum() % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足和值012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult("预测下期和值012路为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => historyData.ElementAt(x + values.Count).Value.Sum() % 3)));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSumValueMantissa012Path(this Dictionary<string, IList<int>> historyData)
    {
        historyData.AutoFind(g => g.Sum() % 10 % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足和值尾012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult("预测下期和值尾012路为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => historyData.ElementAt(x + values.Count).Value.Sum() % 10 % 3)));

        return historyData;
    }

    public static Dictionary<string, IList<int>> FindSpanValue012Path(this Dictionary<string, IList<int>> historyData)
    {
        historyData.AutoFind(g => (g.Max() - g.Min()) % 3, out var values, out var periods);

        PrintHelper.PrintResult($"满足跨度012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periods.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult("预测下期跨度012路为：" + string.Join(" ", periods.Where(x => x + values.Count < historyData.Count).Select(x => (historyData.ElementAt(x + values.Count).Value.Max() - historyData.ElementAt(x + values.Count).Value.Min()) % 3)));

        return historyData;
    }

    #endregion

    public static void AutoFind(this Dictionary<string, IList<int>> historyData, Func<IList<int>, int> func, out List<int> values, out List<int> periods)
    {
        var path012 = historyData.Values.Select(func).ToList();

        var data = historyData.Values.Reverse().ToList();
        values = new List<int>();
        periods = new List<int>();
        var length = RegularLength;
        var find = false;
        while (!find)
        {
            values = data.Take(length).Select(func).Reverse().ToList();
            find = IsSubset(path012, values, out periods);
            length -= 1;

            // if (length <= 0) break;
        }
    }

    public static void AutoFind(this IList<int> historyData, Func<int, int> func, out List<int> values, out List<int> periods)
    {
        var path012 = historyData.Select(func).ToList();

        var data = historyData.Reverse().ToList();
        values = new List<int>();
        periods = new List<int>();
        var length = RegularLength;
        var find = false;
        while (!find)
        {
            values = data.Take(length).Select(func).Reverse().ToList();
            find = IsSubset(path012, values, out periods);
            length -= 1;
        }
    }

    public static void ManualFindValue012Path(this Dictionary<string, IList<int>> historyData, int index, int[] values, out List<int> periods)
    {
        var path012 = historyData.Values.Select(g => g[index - 1] % 3).ToList();
        IsSubset(path012, values.ToList(), out periods);
    }

    private static bool IsSubset(List<int> list1, List<int> list2, out List<int> periods)
    {
        periods = new List<int>();

        for (var i = 0; i <= list1.Count - list2.Count; i++)
        {
            if (list1.Skip(i).Take(list2.Count).SequenceEqual(list2))
            {
                periods.Add(i);
            }
        }

        // 除去最近一期规律
        return periods.Count - 1 >= MinPeriodCount;
    }

    private static List<string> ToLargeMediumSmallDescription(this IEnumerable<int> values)
    {
        return values.Select(v =>
        {
            if (v == -1)
                return "小";
            if (v == 0)
                return "中";
            return "大";
        }).ToList();
    }
}