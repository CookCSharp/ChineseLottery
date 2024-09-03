/*
 * Description: ForecastBase
 * Author: Chance.zheng
 * Creat Time: 2024/09/01 09:56:56 星期日
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System.Text.Json.Nodes;

namespace AutoForecast;

public abstract class ForecastBase
{
    private static readonly SemaphoreSlim SemaphoreSlimInstance = new SemaphoreSlim(0, 1);
    private static Dictionary<string, IList<int>> _historyData = new Dictionary<string, IList<int>>();
    protected int[] LatestLotto => _historyData.LastOrDefault().Value.ToArray();
    protected string NextPeriod => (int.Parse(_historyData.LastOrDefault().Key) + 1).ToString();


    public void StartAutoForecast(LottoType lottoType = LottoType.Supper)
    {
        ResolveJsonFile(lottoType); //.ConfigureAwait(false).GetAwaiter().GetResult();
        SemaphoreSlimInstance.Wait();

        Find(_historyData);
        Forecast();
    }

    public void ViewHistoryForecast(LottoType lottoType = LottoType.Supper)
    {
        ResolveJsonFile(lottoType);
        SemaphoreSlimInstance.Wait();

        HistoryForecast();
    }

    protected int[] UpdateHistoryData()
    {
        var lotto = _historyData.LastOrDefault().Value.ToArray();
        _historyData.Remove(_historyData.LastOrDefault().Key);
        _historyData = new Dictionary<string, IList<int>>(_historyData);

        return lotto;
    }
    
    protected void GetSingle(int index, out int[] mantissaValues, out int[] way012)
    {
        _historyData.AutoFind(g => g[index - 1] % 10 % 3, out var values1, out var periods1);
        mantissaValues = periods1.Where(x => x + values1.Count < _historyData.Count)
            .Select(x => _historyData.ElementAt(x + values1.Count).Value[index - 1] % 10 % 3)
            .Distinct()
            .ToMantissaValues()
            .ToArray();

        _historyData.AutoFind(g => g[index - 1] % 3, out var values2, out var periods2);
        way012 = periods2.Where(x => x + values2.Count < _historyData.Count)
            .Select(x => _historyData.ElementAt(x + values2.Count).Value[index - 1] % 3)
            .Distinct()
            .ToArray();
    }

    protected int[] GetSumMantissaValues(int first, int second)
    {
        _historyData.AutoFind(g => (g[first - 1] + g[second - 1]) % 10 % 3, out var values, out var periods);
        var mantissaValues = periods.Where(x => x + values.Count < _historyData.Count)
            .Select(x => (_historyData.ElementAt(x + values.Count).Value[first - 1] + _historyData.ElementAt(x + values.Count).Value[second - 1]) % 10 % 3)
            .Distinct()
            .ToMantissaValues()
            .ToArray();

        return mantissaValues;
    }

    protected int[] GetSumWay012(int first, int second)
    {
        _historyData.AutoFind(g => (g[first - 1] + g[second - 1]) % 3, out var values, out var periods);
        var way012 = periods.Where(x => x + values.Count < _historyData.Count)
            .Select(x => (_historyData.ElementAt(x + values.Count).Value[first - 1] + _historyData.ElementAt(x + values.Count).Value[second - 1]) % 3)
            .Distinct()
            .ToArray();

        return way012;
    }

    protected int[] GetSpanMantissaValues(int first, int second)
    {
        _historyData.AutoFind(g => (g[second - 1] - g[first - 1]) % 10 % 3, out var values, out var periods);
        var mantissaValues = periods.Where(x => x + values.Count < _historyData.Count)
            .Select(x => (_historyData.ElementAt(x + values.Count).Value[second - 1] - _historyData.ElementAt(x + values.Count).Value[first - 1]) % 10 % 3)
            .Distinct()
            .ToMantissaValues()
            .ToArray();

        return mantissaValues;
    }

    protected int[] GetSpanWay012(int first, int second)
    {
        _historyData.AutoFind(g => Math.Abs(g[second - 1] - g[first - 1]) % 3, out var values, out var periods);
        var way012 = periods.Where(x => x + values.Count < _historyData.Count)
            .Select(x => Math.Abs(_historyData.ElementAt(x + values.Count).Value[second - 1] - _historyData.ElementAt(x + values.Count).Value[first - 1]) % 3)
            .Distinct()
            .ToArray();

        return way012;
    }

    protected void GetSingleAmplitudeWay012(int index, out int[] way012)
    {
        IList<int> amplitudeValues = new List<int>();
        for (int i = 0; i < _historyData.Values.Count - 1; i++)
        {
            var amplitudeValue = Math.Abs(_historyData.Values.ElementAt(i + 1).ElementAt(index - 1) - _historyData.Values.ElementAt(i).ElementAt(index - 1));
            amplitudeValues.Add(amplitudeValue);
        }

        amplitudeValues.AutoFind(v => v % 3, out var values, out var periods);
        way012 = periods.Where(x => x + values.Count < amplitudeValues.Count)
            .Select(x => amplitudeValues.ElementAt(x + values.Count) % 3)
            .Distinct()
            .ToArray();
    }

    #region 适合排3和3D

    protected void GetSingleLessThanTen(int index, out int[] way012, out int[] size)
    {
        _historyData.AutoFind(g => g[index - 1] % 3, out var values1, out var periods1);
        way012 = periods1.Where(x => x + values1.Count < _historyData.Count)
            .Select(x => _historyData.ElementAt(x + values1.Count).Value[index - 1] % 3)
            .Distinct()
            .ToArray();

        _historyData.AutoFind(g => g[index - 1].ToLargeMediumSmallConverterValue(), out var values2, out var periods2);
        size = periods2.Where(x => x + values2.Count < _historyData.Count)
            .SelectMany(x => _historyData.ElementAt(x + values2.Count).Value[index - 1].ToLargeMediumSmallDescription().ToLargeMediumSmallValue())
            .Distinct()
            .ToArray();
    }

    protected int[] GetSum012Path()
    {
        _historyData.AutoFind(g => g.Sum() % 3, out var values, out var periods);
        var path012 = periods.Where(x => x + values.Count < _historyData.Count)
            .Select(x => _historyData.ElementAt(x + values.Count).Value.Sum() % 3)
            .Distinct()
            .ToArray();

        return path012;
    }

    protected int[] GetSumValueMantissa012Path()
    {
        _historyData.AutoFind(g => g.Sum() % 10 % 3, out var values, out var periods);
        var path012 = periods.Where(x => x + values.Count < _historyData.Count)
            .Select(x => _historyData.ElementAt(x + values.Count).Value.Sum() % 10 % 3)
            .Distinct()
            .ToArray();

        return path012;
    }

    protected int[] GetSumValueMantissaLargeMediumSmall()
    {
        _historyData.AutoFind(g => g.Sum() % 10.ToLargeMediumSmallConverterValue(), out var values, out var periods);
        var size = periods.Where(x => x + values.Count < _historyData.Count)
            .SelectMany(x => (_historyData.ElementAt(x + values.Count).Value.Sum() % 10).ToLargeMediumSmallDescription().ToLargeMediumSmallValue())
            .Distinct()
            .ToArray();

        return size;
    }

    protected int[] GetSpanValue012Path()
    {
        _historyData.AutoFind(g => (g.Max() - g.Min()) % 3, out var values, out var periods);
        var path012 = periods.Where(x => x + values.Count < _historyData.Count)
            .Select(x => (_historyData.ElementAt(x + values.Count).Value.Max() - _historyData.ElementAt(x + values.Count).Value.Min()) % 3)
            .Distinct()
            .ToArray();

        return path012;
    }

    protected int[] GetSpanValueLargeMediumSmallValue()
    {
        _historyData.AutoFind(g => (g.Max() - g.Min()).ToLargeMediumSmallConverterValue(), out var values, out var periods);
        var size = periods.Where(x => x + values.Count < _historyData.Count)
            .SelectMany(x => (_historyData.ElementAt(x + values.Count).Value.Max() - _historyData.ElementAt(x + values.Count).Value.Min()).ToLargeMediumSmallDescription().ToLargeMediumSmallValue())
            .Distinct()
            .ToArray();

        return size;
    }

    #endregion

    protected abstract void Forecast();

    protected abstract void Find(Dictionary<string, IList<int>> historyData);

    protected abstract void HistoryForecast();

    private async void ResolveJsonFile(LottoType lottoType = LottoType.Supper)
    {
        var historyData = new Dictionary<string, IList<int>>();
        await using var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + $"Assets\\History-{lottoType}.json", FileMode.Open);
        using var sw = new StreamReader(fs);
        var resultJson = await sw.ReadToEndAsync();
        var dataList = JsonNode.Parse(resultJson)!.AsArray();
        foreach (var t in dataList)
        {
            var key = t!["lotteryDrawNum"]!.ToString();
            var value = t["lotteryDrawResult"]!.ToString().Split(' ').Select(int.Parse).ToList();
            historyData.Add(key, value);
        }

        _historyData = new Dictionary<string, IList<int>>(historyData.Reverse());
        SemaphoreSlimInstance.Release();
    }
}