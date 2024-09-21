// See https://aka.ms/new-console-template for more information


using System.Globalization;
using AutoForecast;
using Microsoft.ML;
using Microsoft.ML.Data;

internal static class Program
{
    public static void Main(string[] args)
    {
        // DD();

        // MLForecast.Instance.MLAutoForecast(args);
        // MLTest.ForecastBySSA();

        SupperForecast.Instance.StartAutoForecast();
        // SupperManualForecast();

        // ThreePermutationForecast.Instance.StartAutoForecast(LottoType.ThreePermutation);
        // ThreePermutationForecast.Instance.ViewHistoryForecast(LottoType.ThreePermutation);
    }
    
    private static void SupperManualForecast()
    {
        var redFirst = SupperTool.GetFirstRed([0,1,2,3,4,5,6], [1,2]).Amplitude(12, [0,1]);
        var redSecond = SupperTool.GetSecondRed([1,2,4,5, 0,3,6], [0,1,2]).Amplitude(23, [1,2]);
        var redThird = SupperTool.GetThirdRed([0,3,6,9], [0,1]).Amplitude(26, [0,1,2]);
        var redFourth = SupperTool.GetFourthRed([0,3,6,9], [0,1,2]).Amplitude(27, [0,2]);
        var redFifth = SupperTool.GetFifthRed([0,1,2,7,8,9], [0,1]).Amplitude(28, [0,1]);
        Console.WriteLine("第一位红色号码：" + redFirst.ToD2String());
        Console.WriteLine("第二位红色号码：" + redSecond.ToD2String());
        Console.WriteLine("第三位红色号码：" + redThird.ToD2String());
        Console.WriteLine("第四位红色号码：" + redFourth.ToD2String());
        Console.WriteLine("第五位红色号码：" + redFifth.ToD2String());

        var redResult = Tool.FindAllSortedCombinations(redFirst, redSecond, redThird, redFourth, redFifth);
        redResult = redResult
                .Sum12MantissaFilter([3,4,6,7,9])
                .Sum13MantissaFilter([3,4,5,6,7,8,9])
                .Sum14MantissaFilter([0,1,7,9])
                .Sum15MantissaFilter([0,1,2,3,4,5,6])
                .Sum23MantissaFilter([0,3,6,9])
                .Sum24MantissaFilter([3,4,6,7,9])
                // .Sum25MantissaFilter([7,8,9])
                .Sum34MantissaFilter([3,4,5,6])
                .Sum35MantissaFilter([3,4,5,6,7,8,9])
                .Sum45MantissaFilter([0,1,2,3,4,5,6])
                
                // .Sum12PathFilter([0,1])
                // .Sum13PathFilter([0,1])
                // .Sum14PathFilter([1,2])
                // .Sum15PathFilter([0,2])
                .Sum23PathFilter([0,2])
                // .Sum24PathFilter([0,1])
                // .Sum25PathFilter([0,2])
                .Sum34PathFilter([0,1])
                // .Sum35PathFilter([0,2])
                // .Sum45PathFilter([0,2])
                
                // .Span14MantissaFilter([0,3,6,9,2,5,8])
                // .Span15MantissaFilter([1,4,2,5, 0,3,6])
                // .Span13PathFilter([0,2])
                // .Span14PathFilter([1,2])
            ;

        Console.WriteLine($"红色号码共{redResult.Count}组：");
        redResult.ForEach(g => Console.WriteLine(g.ToD2String()));


        int[] blueFirst = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        int[] blueSecond = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
        blueFirst = blueFirst.Amplitude(3, [1,2]);
        blueSecond = blueSecond.Amplitude(11, [1,2]);
        var blueGroups = SupperTool.FindAllSortedCombinations(blueFirst, blueSecond);
        var blueResult = blueGroups
                .BlueFirstFilter([1,2])
                .BlueSecondFilter([0,1,2])
                // .BlueSumFilter([1,2, 0])
                // .BlueSumMantissaFilter([0,2])
                // .BlueSpanFilter([0,1])
                // .BlueSpanMantissaFilter([0,2])
            ;

        Console.WriteLine($"蓝色号码共{blueResult.Count}组：");
        blueResult.ForEach(g => Console.WriteLine(g.ToD2String()));
    }

    private static void UnionForecast()
    {
        // var redFirst = GetRed([1, 4, 7], [0, 1, 2]);
        // var redSecond = GetRed([1, 2, 3, 5, 6, 7], [0, 2]);
        // var redThird = GetRed([0, 1, 2, 3, 4, 5, 6, 7], [0]);
        // var redFourth = GetRed([1, 3, 5, 6, 9], [0, 1]);
        // var redFifth = GetRed([0, 3, 6, 9], [0, 1, 2]);
        // var redSixth = GetRed([3, 4, 5, 6, 7, 8, 9], [1, 2]);
        //
        // var redGroups = Tool.FindAllSortedCombinations(redFirst, redSecond, redThird, redFourth, redFifth, redSixth);
    }
}