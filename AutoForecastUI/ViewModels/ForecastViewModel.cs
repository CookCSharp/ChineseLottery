/*
 * Description: ForecastViewModel
 * Author: Chance.zheng
 * Creat Time: 2024/09/21 14:33:11 星期六
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.ObjectModel;
using System.IO;
using AutoForecast;
using CommunityToolkit.Mvvm.Input;

namespace AutoForecastUI.ViewModels;

public partial class ForecastViewModel : ViewModelBase
{
    public ObservableCollection<ForecastFeature> ForecastFeatures { get; set; } = new();

    public ObservableCollection<ForecastFeature> LastPeriodFeatures { get; set; } = new();

    public string? LastLotto { get; set; }

    private readonly string[] LottoSequence =
    {
        "第一位", "第二位", "第三位", "第四位", "第五位",
        "12位和", "13位和", "14位和", "15位和", "23位和", "24位和", "25位和", "34位和", "35位和", "45位和"
    };

    public ForecastViewModel()
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

        int[] nums = [01, 07, 08, 18, 28, 04, 12];
        LastLotto = nums.ToD2String();
    }

    public void OnLoaded()
    {
        // if (ForecastFeatures.Count > 0) return;
    }

    [RelayCommand]
    private void Save()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Supper.txt");
        using var sw = new StreamWriter(path);
        foreach (var feature in ForecastFeatures)
        {
            sw.WriteLine("{0}    路数: {1}    尾数值: {2}    振幅: {3}    其它: {4}", feature.LottoFeature, feature.Path012, feature.MantissaValue, feature.AmplitudePath012, feature.Other);
        }

        sw.Flush();
    }
}

public class ForecastFeature
{
    public string? LottoFeature { get; set; }

    public string? Path012 { get; set; }

    public string? MantissaValue { get; set; }

    public string? AmplitudePath012 { get; set; }

    public string? Other { get; set; }
}