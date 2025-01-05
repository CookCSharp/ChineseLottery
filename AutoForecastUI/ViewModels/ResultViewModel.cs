/*
 * Description: ResultViewModel
 * Author: Chance.zheng
 * Creat Time: 2024/09/23 10:15:43 星期一
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoForecast;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tensorflow;

namespace AutoForecastUI.ViewModels;

public partial class ResultViewModel : ViewModelBase
{
    [ObservableProperty] private string? _result;
    [ObservableProperty] private LottoType _selectedLottoType;
    [ObservableProperty] private ObservableCollection<LottoType> _lottoTypes = new(Enum.GetValues<LottoType>());

    public ObservableCollection<ForecastFeature> ForecastFeatures { get; set; } = new();

    private void ResolveData()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Supper.txt");
        using var sr = new StreamReader(path);
        while (!sr.EndOfStream)
        {
            var strs = sr.ReadLine()?.Split([' ', ':'])!;
            ForecastFeatures.Add(new ForecastFeature
            {
                LottoFeature = strs[0],
                Path012 = strs[6],
                MantissaValue = strs[12],
                AmplitudePath012 = strs[18],
                Other = strs[24],
            });
        }
    }
    
    private async Task BeginSupperForecast()
    {
        var latestLotto = await SupperForecast.Instance.GetLatestLottoAsync();
        var redFirst = SupperTool.GetFirstRed([0,3,6,9,1,4,7], [2]).Amplitude(latestLotto[0], [0]);
        var redSecond = SupperTool.GetSecondRed([7,8,9], [0,1,2]).Amplitude(latestLotto[1], [0,1]); //9
        var redThird = SupperTool.GetThirdRed([0,3,6,9,1,4,7], [0,1,2]).Amplitude(latestLotto[2], [2]); //>10
        var redFourth = SupperTool.GetFourthRed([1,4,7,2,5,8], [0]).Amplitude(latestLotto[3], [2]);
        var redFifth = SupperTool.GetFifthRed([2,5,8], [1,2]).Amplitude(latestLotto[4], [0,2]);
        
        redSecond = redSecond.Where(n => n > redFirst.Min()).ToArray();
        redThird = redThird.Where(n => n > redSecond.Min()).ToArray();
        redFourth = redFourth.Where(n => n > redThird.Min()).ToArray();
        redFifth = redFifth.Where(n => n > redFourth.Min()).ToArray();
        
        redFourth = redFourth.Where(n => n < redFifth.Max()).ToArray();
        redThird = redThird.Where(n => n < redFourth.Max()).ToArray();
        redSecond = redSecond.Where(n => n < redThird.Max()).ToArray();
        redFirst = redFirst.Where(n => n < redSecond.Max()).ToArray();
        
        Console.WriteLine($"First: {redFirst.ToD2String()}");
        Console.WriteLine($"Second: {redSecond.ToD2String()}");
        Console.WriteLine($"Third: {redThird.ToD2String()}");
        Console.WriteLine($"Fourth: {redFourth.ToD2String()}");
        Console.WriteLine($"Fifth: {redFifth.ToD2String()}");
        
        var redResult = Tool.FindAllSortedCombinations(redFirst, redSecond, redThird, redFourth, redFifth);
        redResult = redResult
                .Sum12MantissaFilter([0,3,6,9,1,4,7])
                .Sum13MantissaFilter([0,3,6,9,2,5,8])
                .Sum14MantissaFilter([0,1,2,7,8,9]) 
                // .Sum15MantissaFilter([0,1,2,7,8,9]) 
                .Sum23MantissaFilter([0,3,6,9,2,5,8])
                .Sum24MantissaFilter([0,3,6,9,2,5,8])
                .Sum25MantissaFilter([0,3,6,9,2,5,8])
                .Sum34MantissaFilter([7,8,9])
                // .Sum35MantissaFilter([0,1,2,3,4,5,6])
                // .Sum45MantissaFilter([0,1,2,7,8,9,4,6])
                
                // .Sum12PathFilter([0,2])
                // .Sum13PathFilter([0,2])
                // .Sum14PathFilter([0,2])
                // .Sum15PathFilter([0, 1])
                // .Sum23PathFilter([0,1])
                // .Sum24PathFilter([0,1])
                // .Sum25PathFilter([0,2])
                
                // .Sum34PathFilter([1,2])
                // .Sum35PathFilter([1,2])
                // .Sum45PathFilter([0])
                
                // .Sum23Filter(SupperTool.ConditionType.MoreThan, 24)
                // .Sum34Filter(SupperTool.ConditionType.LessThan, 45)
                // .Sum25Filter(SupperTool.ConditionType.LessThan, 45)
                
                // .Span12MantissaFilter([0,1,2,3,4,5,6])
                // .Span13MantissaFilter([3,4,5,6,7,8,9])
                // .Span14MantissaFilter([0,3,6,9])
                // .Span15MantissaFilter([0,1,2,3,4,5,6])
                // .Span23MantissaFilter([0,3,6,9,1,4,7])
                // .Span24MantissaFilter([0,3,6,9,2,5,8])
                //.Span25MantissaFilter([0,1,2,7,8,9])
                // .Span34MantissaFilter([0,3,6,9,2,5,8,4,7])
                // .Span35MantissaFilter([0,3,6,9,1,4,7,2])
                // .Span45MantissaFilter([0,1,2,7,8,9])
            ;
        
        Console.WriteLine($"Red groups: {redResult.Count}");
        redResult.ForEach(g => Console.WriteLine(g.ToD2String()));
        
        
        int[] blueFirst = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        int[] blueSecond = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
        
        blueFirst = blueFirst.Amplitude(latestLotto[5], [1,2]);
        blueSecond = blueSecond.Amplitude(latestLotto[6], [1,2]);
        
        var blueGroups = SupperTool.FindAllSortedCombinations(blueFirst, blueSecond);
        var blueResult = blueGroups
                .BlueFirstFilter([0])
                // .BlueSecondFilter([1,2])
                // .BlueSumFilter([0,1])
                // .BlueSumMantissaFilter([0])
                // .BlueSpanFilter([2])
                // .BlueSpanMantissaFilter(GetSpanMantissaValues(6, 7))
                // .BlueSumMantissaLargeMediumSmallFilter(GetSumValueMantissaLargeMediumSmall())
                // .BlueSpanMantissaLargeMediumSmallFilter(GetSpanValueLargeMediumSmall())
                
                // .Where(g => new[]{06,07,09,10,11,12}.Contains(g[1]))
                // .Where(g => g.Max()-g.Min() == 03 || g.Max()-g.Min() == 11)
                // .Where(g => g.Sum() == 19)
                // .ToList()
            ;
        Console.WriteLine($"Blue group: {blueResult.Count}"); 
        blueResult.ForEach(g => Console.WriteLine(g.ToD2String()));
        
        Console.WriteLine(); 
        Console.WriteLine(); 
        Console.WriteLine();
        Console.WriteLine("大乐透"); 
        redResult.ForEach(gr =>
        {
            blueResult.ForEach(gb =>
            {
                Console.Write(gr.ToD2String());
                Console.Write(" " + gb.ToD2String());
                Console.Write(Environment.NewLine);
            });
        });
    }
    
    private async Task BeginThreeDForecast()
    {
        var latestLotto = await SupperForecast.Instance.GetLatestLottoAsync(LottoType.ThreeD);
    }
    
    private void BeginSevenPermutationForecast()
    {
        // int[] first = [9, 7,8];
        // int[] second = [4,8, 3];
        // int[] third = [0, 3, 2];
        // int[] fourth = [1];
        // int[] fifth = [1,4, 0,6,7];
        // int[] sixth = [3,6,9, 0];
        
        int[] first = [9];
        int[] second = [4,8];
        int[] third = [0, 3];
        int[] fourth = [1];
        int[] fifth = [1,4];
        int[] sixth = [3,6,9];
        var redResult = Tool.FindAllSortedCombinations(first, second, third, fourth, fifth, sixth, true);
        
        redResult = redResult
                .Sum12MantissaFilter([1,4,7,2,5,8])
                .Sum13MantissaFilter([0,3,6,9,2,5,8])
                .Sum14MantissaFilter([0,3,6,9,2,5,8]) //0369
                .Sum15MantissaFilter([0,3,6,9,1,4,7])
                
                .Sum16MantissaFilter([0,3,6,9,2,5,8])
                .Sum23MantissaFilter([0,1,2,7,8,9,5,6]) //02路
                //.Sum24MantissaFilter([1,4,7,2,5,8])
                .Sum25MantissaFilter([0,1,2,7,8,9]) //0289
                
                .Sum26MantissaFilter([0,1,2,7,8,9])
                //.Sum34MantissaFilter([0,3,6,9,2,5,8])
                //.Sum35MantissaFilter([0,3,6,9,2,5,8]) //0289
                .Sum36MantissaFilter([0,1,2,7,8,9])
                
                //.Sum45MantissaFilter([0,3,6,9,1,4,7])
                //.Sum46MantissaFilter([0,1,2,3,4,5,6])
                //.Sum56MantissaFilter([1,4,7,2,5,8])  //1278
                
                // .Sum46PathFilter([0,2])
                // .Sum14Filter(SupperTool.ConditionType.LessThan, 6)
                // .Sum14Filter(SupperTool.ConditionType.MoreThan, 13)
                // .Sum26Filter(SupperTool.ConditionType.LessThan, 6)
                // .Sum26Filter(SupperTool.ConditionType.MoreThan, 13)
            ;
        
        Console.WriteLine($"共{redResult.Count}组：");
        redResult.ForEach(g => Console.WriteLine(g.ToD2String()));
    }
    
    [RelayCommand]
    private async Task GetResultAsync()
    {
        Result = string.Empty;
        await Task.Run(async () =>
        {
            await using var stringWriter = new StringWriter();
            var originalConsoleOut = Console.Out;
            Console.SetOut(stringWriter);
            
            switch (SelectedLottoType)
            {
                case LottoType.Supper:
                    await BeginSupperForecast();
                    break;
                case LottoType.Union:
                    break;
                case LottoType.ThreeD:
                    await BeginThreeDForecast();
                    break;
                case LottoType.ThreePermutation:
                    break;
                case LottoType.SevenPermutation:
                    BeginSevenPermutationForecast();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Console.SetOut(originalConsoleOut);
            Result = stringWriter.ToString();
        });
    }
}