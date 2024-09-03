/*
 * Description: FindManual
 * Author: Chance.zheng
 * Creat Time: 2024/08/31 21:19:20 星期六
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System.Text.Json.Nodes;

namespace AutoForecast;

public static class FindManual
{
    private const int RegularLength = 10; //规律数组长度
    private const int MinPeriodCount = 3; //找到满足条件的最小周期数量
    private static readonly SemaphoreSlim SemaphoreSlimInstance = new SemaphoreSlim(0, 1);
    private static Dictionary<string, IList<int>> HistoryData = new Dictionary<string, IList<int>>();

    public static void Find(LottoType lottoType = LottoType.Supper)
    {
        ResolveJsonFile(lottoType);
        SemaphoreSlimInstance.Wait();
    }

    public static async void ResolveJsonFile(LottoType lottoType = LottoType.Supper)
    {
        await using var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"Assets\\History-{lottoType}.json", FileMode.Open);
        using var sw = new StreamReader(fs);
        var resultJson = await sw.ReadToEndAsync();
        var dataList = JsonNode.Parse(resultJson)!.AsArray();
        foreach (var t in dataList)
        {
            var key = t!["lotteryDrawNum"]!.ToString();
            var value = t["lotteryDrawResult"]!.ToString().Split(' ').Select(int.Parse).ToList();
            HistoryData.Add(key, value);
        }

        HistoryData = new Dictionary<string, IList<int>>(HistoryData.Reverse());
        SemaphoreSlimInstance.Release();
    }

    public static void Wait() => SemaphoreSlimInstance.Wait();
    
    private static void FindValue012Path(this Dictionary<string, IList<int>> historyData, int index, int[] values)
    {
        var path012 = historyData.Values.Select(group => group[index - 1] % 3).ToList();
        IsSubset(path012, values.ToList(), out var periodIndexes);

        PrintHelper.PrintResult($"满足第{index}位值012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{index}位下期值({historyData.ElementAt(periodIndexes.Last()).Key})012路为：" + string.Join(" ", periodIndexes.Where(x => x + values.Length < historyData.Count).Select(x => historyData.ElementAt(x + values.Length).Value[index - 1] % 3)));
    }

    private static void FindValueMantissa012Path(this Dictionary<string, IList<int>> historyData, int index, int[] values)
    {
        var path012 = historyData.Values.Select(group => group[index - 1] % 10 % 3).ToList();
        IsSubset(path012, values.ToList(), out var periodIndexes);

        PrintHelper.PrintResult($"满足第{index}位尾数012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{index}位下期尾数012路为：" + string.Join(" ", periodIndexes.Where(x => x + values.Length < historyData.Count).Select(x => historyData.ElementAt(x + values.Length).Value[index - 1] % 10 % 3)));
    }

    private static void FindSumValue012Path(this Dictionary<string, IList<int>> historyData, int first, int second, int[] values)
    {
        var path012 = historyData.Values.Select(group => (group[first - 1] + group[second - 1]) % 3).ToList();
        IsSubset(path012, values.ToList(), out var periodIndexes);

        PrintHelper.PrintResult($"满足第{first}、{second}位和值012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{first}、{second}位下期和值012路为：" + string.Join(" ", periodIndexes.Where(x => x + values.Length < historyData.Count).Select(x => (historyData.ElementAt(x + values.Length).Value[first - 1] + historyData.ElementAt(x + values.Length).Value[second - 1]) % 3)));
    }

    private static void FindSumValueMantissa012Path(this Dictionary<string, IList<int>> historyData, int first, int second, int[] values)
    {
        var path012 = historyData.Values.Select(group => (group[first - 1] + group[second - 1]) % 10 % 3).ToList();
        IsSubset(path012, values.ToList(), out var periodIndexes);

        PrintHelper.PrintResult($"满足第{first}、{second}位和值尾012路规律：{string.Join(" ", values)} 的有以下几期");
        PrintHelper.PrintResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        PrintHelper.PrintForecastResult($"预测第{first}、{second}位下期和值尾012路为：" + string.Join(" ", periodIndexes.Where(x => x + values.Length < historyData.Count).Select(x => (historyData.ElementAt(x + values.Length).Value[first - 1] + historyData.ElementAt(x + values.Length).Value[second - 1]) % 10 % 3)));
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

        return periods.Count >= MinPeriodCount;
    }
}