/*
 * Description: SupperForecast
 * Author: Chance.zheng
 * Creat Time: 2024/09/01 10:01:35 星期日
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


namespace AutoForecast;

// https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=85&provinceId=0&pageSize=100&isVerify=1&pageNo=1

public class SupperForecast : ForecastBase
{
    public static SupperForecast Instance { get; } = new SupperForecast();

    static SupperForecast() { }

    private SupperForecast() { }


    protected override void Forecast()
    {
        GetSingle(1, out var mantissaValues, out var way012);
        var redFirst = SupperTool.GetFirstRed(mantissaValues, way012);
        GetSingle(2, out mantissaValues, out way012);
        var redSecond = SupperTool.GetSecondRed(mantissaValues, way012);
        GetSingle(3, out mantissaValues, out way012);
        var redThird = SupperTool.GetThirdRed(mantissaValues, way012);
        GetSingle(4, out mantissaValues, out way012);
        var redFourth = SupperTool.GetFourthRed(mantissaValues, way012);
        GetSingle(5, out mantissaValues, out way012);
        var redFifth = SupperTool.GetFifthRed(mantissaValues, way012);
        PrintHelper.PrintResult("第一位红色号码：" + redFirst.ToD2String());
        PrintHelper.PrintResult("第二位红色号码：" + redSecond.ToD2String());
        PrintHelper.PrintResult("第三位红色号码：" + redThird.ToD2String());
        PrintHelper.PrintResult("第四位红色号码：" + redFourth.ToD2String());
        PrintHelper.PrintResult("第五位红色号码：" + redFifth.ToD2String());
        Console.WriteLine(Environment.NewLine);

        var redGroups = Tool.FindAllSortedCombinations(redFirst, redSecond, redThird, redFourth, redFifth);
        var redResult = redGroups
            .Sum12MantissaFilter(GetSumMantissaValues(1, 2))
            .Sum13MantissaFilter(GetSumMantissaValues(1, 3))
            .Sum14MantissaFilter(GetSumMantissaValues(1, 4))
            .Sum15MantissaFilter(GetSumMantissaValues(1, 5))
            .Sum23MantissaFilter(GetSumMantissaValues(2, 3))
            .Sum24MantissaFilter(GetSumMantissaValues(2, 4))
            .Sum25MantissaFilter(GetSumMantissaValues(2, 5))
            .Sum34MantissaFilter(GetSumMantissaValues(3, 4))
            .Sum35MantissaFilter(GetSumMantissaValues(3, 5))
            .Sum45MantissaFilter(GetSumMantissaValues(4, 5))
            .Sum12Filter(GetSumWay012(1, 2))
            .Sum13Filter(GetSumWay012(1, 3))
            .Sum14Filter(GetSumWay012(1, 4))
            .Sum15Filter(GetSumWay012(1, 5))
            .Sum23Filter(GetSumWay012(2, 3))
            .Sum24Filter(GetSumWay012(2, 4))
            .Sum25Filter(GetSumWay012(2, 5))
            .Sum34Filter(GetSumWay012(3, 4))
            .Sum35Filter(GetSumWay012(3, 5))
            .Sum45Filter(GetSumWay012(4, 5))
            .Span12MantissaFilter(GetSpanMantissaValues(1, 2))
            .Span13MantissaFilter(GetSpanMantissaValues(1, 3))
            .Span14MantissaFilter(GetSpanMantissaValues(1, 4))
            .Span15MantissaFilter(GetSpanMantissaValues(1, 5))
            // .Span23MantissaFilter(GetSpanMantissaValues(2, 3))
            // .Span24MantissaFilter(GetSpanMantissaValues(2, 4))
            // .Span25MantissaFilter(GetSpanMantissaValues(2, 5))
            // .Span34MantissaFilter(GetSpanMantissaValues(3, 4))
            //.Span35MantissaFilter(GetSpanMantissaValues(3, 5))
            .Span45MantissaFilter(GetSpanMantissaValues(4, 5));

        PrintHelper.PrintForecastResult($"预测红色号码共{redResult.Count}组：", isNewLine: false);
        redResult.ForEach(g => Console.WriteLine(g.ToD2String()));
        Console.WriteLine(Environment.NewLine);

        int[] blueFirst = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        int[] blueSecond = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

        GetSingleAmplitudeWay012(6, out int[] blueFirstWay012);
        blueFirst = blueFirst.BlueAmplitudeFilter(LatestLotto[5], blueFirstWay012);
        GetSingleAmplitudeWay012(7, out int[] blueSecondWay012);
        blueSecond = blueSecond.BlueAmplitudeFilter(LatestLotto[6], blueSecondWay012);

        GetSingle(6, out _, out var firstBlueWay012);
        GetSingle(7, out _, out var secondBlueWay012);
        var blueGroups = SupperTool.FindAllSortedCombinations(blueFirst, blueSecond);
        var blueResult = blueGroups
            .BlueFirstFilter(firstBlueWay012)
            .BlueSecondFilter(secondBlueWay012)
            .BlueSumFilter(GetSumWay012(6, 7))
            .BlueSumMantissaFilter(GetSumMantissaValues(6, 7))
            .BlueSpanFilter(GetSpanWay012(6, 7))
            .BlueSpanMantissaFilter(GetSpanMantissaValues(6, 7));

        PrintHelper.PrintForecastResult($"预测蓝色号码共{blueResult.Count}组：", isNewLine: false);
        blueResult.ForEach(g => Console.WriteLine(g.ToD2String()));
    }

    protected override void Find(Dictionary<string, IList<int>> historyData)
    {
        historyData
            .FindValue012Path(1)
            .FindValueMantissa012Path(1)
            .FindValue012Path(2)
            .FindValueMantissa012Path(2)
            .FindValue012Path(3)
            .FindValueMantissa012Path(3)
            .FindValue012Path(4)
            .FindValueMantissa012Path(4)
            .FindValue012Path(5)
            .FindValueMantissa012Path(5)
            .FindSumValue012Path(1, 2)
            .FindSumValueMantissa012Path(1, 2)
            .FindSumValue012Path(1, 3)
            .FindSumValueMantissa012Path(1, 3)
            .FindSumValue012Path(1, 4)
            .FindSumValueMantissa012Path(1, 4)
            .FindSumValue012Path(1, 5)
            .FindSumValueMantissa012Path(1, 5)
            .FindSumValue012Path(2, 3)
            .FindSumValueMantissa012Path(2, 3)
            .FindSumValue012Path(2, 4)
            .FindSumValueMantissa012Path(2, 4)
            .FindSumValue012Path(2, 5)
            .FindSumValueMantissa012Path(2, 5)
            .FindSumValue012Path(3, 4)
            .FindSumValueMantissa012Path(3, 4)
            .FindSumValue012Path(3, 5)
            .FindSumValueMantissa012Path(3, 5)
            .FindSumValue012Path(4, 5)
            .FindSumValueMantissa012Path(4, 5)
            .FindSpanValue012Path(1, 2)
            .FindSpanValueMantissa012Path(1, 2)
            .FindSpanValue012Path(1, 3)
            .FindSpanValueMantissa012Path(1, 3)
            .FindSpanValue012Path(1, 4)
            .FindSpanValueMantissa012Path(1, 4)
            .FindSpanValue012Path(1, 5)
            .FindSpanValueMantissa012Path(1, 5)
            .FindSpanValue012Path(2, 3)
            .FindSpanValueMantissa012Path(2, 3)
            .FindSpanValue012Path(2, 4)
            .FindSpanValueMantissa012Path(2, 4)
            .FindSpanValue012Path(2, 5)
            .FindSpanValueMantissa012Path(2, 5)
            .FindSpanValue012Path(3, 4)
            .FindSpanValueMantissa012Path(3, 4)
            .FindSpanValue012Path(3, 5)
            .FindSpanValueMantissa012Path(3, 5)
            .FindSpanValue012Path(4, 5)
            .FindSpanValueMantissa012Path(4, 5);

        historyData
            .FindValue012Path(6)
            .FindAmplitudeValue012Path(6)
            .FindValue012Path(7)
            .FindAmplitudeValue012Path(7)
            .FindSumValue012Path(6, 7)
            .FindSumValueMantissa012Path(6, 7)
            .FindSpanValue012Path(6, 7)
            .FindSpanValueMantissa012Path(6, 7);
    }

    protected override void HistoryForecast()
    {
        
    }
}