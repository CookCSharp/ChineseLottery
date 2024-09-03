// See https://aka.ms/new-console-template for more information


using AutoForecast;

internal static class Program
{
    public static void Main(string[] args)
    {
        // SupperForecast.Instance.StartAutoForecast();
        
        // ThreePermutationForecast.Instance.StartAutoForecast(LottoType.ThreePermutation);
        ThreePermutationForecast.Instance.ViewHistoryForecast(LottoType.ThreePermutation);
    }
    
    private static void SupperManualForecast()
    {
        var redFirst = SupperTool.GetFirstRed([2, 5, 8, 1, 4, 7, 0, 3, 6, 9], [1]);
        var redSecond = SupperTool.GetSecondRed([0, 3, 6, 9, 2, 5, 8, 1, 4, 7], [1]);
        var redThird = SupperTool.GetThirdRed([0, 3, 6, 9], [0, 2]);
        var redFourth = SupperTool.GetFourthRed([2, 5, 8], [0, 1]);
        var redFifth = SupperTool.GetFifthRed([0, 3, 6, 9], [1]);
        Console.WriteLine("第一位红色号码：" + redFirst.ToD2String());
        Console.WriteLine("第二位红色号码：" + redSecond.ToD2String());
        Console.WriteLine("第三位红色号码：" + redThird.ToD2String());
        Console.WriteLine("第四位红色号码：" + redFourth.ToD2String());
        Console.WriteLine("第五位红色号码：" + redFifth.ToD2String());

        var redGroups = Tool.FindAllSortedCombinations(redFirst, redSecond, redThird, redFourth, redFifth);
        var redResult = redGroups
            .Sum12MantissaFilter([2, 5, 8])
            .Sum13MantissaFilter([0, 3, 6, 9])
            .Sum14MantissaFilter([0, 3, 6, 9, 2, 5, 8])
            .Sum15MantissaFilter([0, 3, 6, 9])
            .Sum23MantissaFilter([0, 3, 6, 9])
            .Sum24MantissaFilter([0, 3, 6, 9, 2, 5, 8])
            //.Sum25MantissaFilter([1, 4, 7])
            .Sum34MantissaFilter([1, 4, 7, 2, 5, 8])
            //.Sum35MantissaFilter([0, 3, 6, 9, 1, 4, 7])
            .Sum45MantissaFilter([0, 3, 6, 9, 1, 4, 7])

            // .Difference15Filter([0, 2])
            // .Difference23Filter([1, 2])
            // .Difference45Filter([0, 2])
            .Span45MantissaFilter([0, 3, 6, 9, 2, 5, 8, 1, 4, 7]);

        Console.WriteLine($"红色号码共{redResult.Count}组：");
        redResult.ForEach(g => Console.WriteLine(g.ToD2String()));


        int[] blueFirst = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        int[] blueSecond = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
        var blueGroups = SupperTool.FindAllSortedCombinations(blueFirst, blueSecond);
        var blueResult = blueGroups
            .BlueFirstFilter([1])
            .BlueSecondFilter([1])
            // .BlueSumFilter([1])
            .BlueSumMantissaFilter([1, 2]);
        // .BlueDifferenceFilter([0, 2])
        // .BlueDifferenceMantissaFilter([0, 2]);

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