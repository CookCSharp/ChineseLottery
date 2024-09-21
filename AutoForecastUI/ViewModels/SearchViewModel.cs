/*
 * Description: SearchViewModel
 * Author: Chance.zheng
 * Creat Time: 2024/09/18 09:38:57 星期三
 * Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoForecast;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AutoForecastUI.ViewModels;

public partial class SearchViewModel : ViewModelBase
{
    private readonly string[] Singles = ["First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh"];
    private readonly string[] Combinations = ["12", "13", "14", "15", "23", "24", "25", "34", "35", "45"];

    [ObservableProperty] private int _minPeriodCount = 3;
    
    [ObservableProperty] private ObservableCollection<HistoryFeature> _singleFeatures;

    [ObservableProperty] private ObservableCollection<HistoryFeature> _sumFeatures;

    [ObservableProperty] private string? _result;

    private Dictionary<string, IList<int>> _historyData = new();
    private HistoryFeature _feature = new();

    public SearchViewModel()
    {
        _singleFeatures = new ObservableCollection<HistoryFeature>();
        _sumFeatures = new ObservableCollection<HistoryFeature>();

        Array.ForEach(Singles, s => _singleFeatures.Add(new HistoryFeature { Name = s }));
        Array.ForEach(Combinations, s => _sumFeatures.Add(new HistoryFeature { Name = s }));

        WeakReferenceMessenger.Default.Register<string>(this, (o, m) => { Result = m; });
        WeakReferenceMessenger.Default.Register<HistoryFeature>(this, (o, f) => { _feature = f; });
    }
    
    public async void OnLoaded()
    {
        await UpdateAsync(new CancellationToken());
    }

    [RelayCommand]
    private void Sure()
    {
        FindExtensions.SetMinPeriodCount(MinPeriodCount);
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task QueryAsync(CancellationToken token)
    {
        using var cts = new CancellationTokenSource();
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, token);
        linkedTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

        try
        {
            await GetPrint(_feature.Name, linkedTokenSource.Token).ConfigureAwait(false);
            
            //await Task.Delay(10000, linkedTokenSource.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            UpdateCommand.Cancel();
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception)
        {
            //ignore
        }
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task UpdateAsync(CancellationToken token)
    {
        //由于当前只有一项，故只需要在此获取即可
        _historyData = await SupperForecast.Instance.GetHistoryData();
    }

    private async Task GetPrint(string name, CancellationToken token)
    {
        await Task.Run(() =>
        {
            // 创建一个StringWriter来捕获输出
            using var stringWriter = new StringWriter();
            // 保存原始的 Console 输出流
            var originalConsoleOut = Console.Out;
            // 重定向 Console 输出流到 StringWriter
            Console.SetOut(stringWriter);

            if (_feature.IsSelected)
                ChooseMethod(name);

            // 恢复原始的 Console 输出流
            Console.SetOut(originalConsoleOut);
            // 获取捕获到的输出
            var output = stringWriter.ToString();
            output = _feature.IsSelected ? output : "Nothing is selected";
            WeakReferenceMessenger.Default.Send(output);
        }, token);
    }

    private void ChooseMethod(string name)
    {
        switch (name)
        {
            case "First" or "Second" or "Third" or "Fourth" or "Fifth" or "Sixth" or "Seventh":
                var index = GetSingle(name);
                _historyData.FindValue012Path(index);
                _historyData.FindValueMantissa012Path(index);
                _historyData.FindValueMantissaLargeMediumSmall(index);
                _historyData.FindAmplitudeValue012Path(index);
                break;
            case "12" or "13" or "14" or "15" or "23" or "24" or "25" or "34" or "35" or "45":
                var tuple = GetTuple(name);
                var first = tuple.Item1;
                var second = tuple.Item2;
                _historyData.FindSumValue012Path(first, second);
                _historyData.FindSumValueMantissa012Path(first, second);
                _historyData.FindSumValueMantissaLargeMediumSmall(first, second);
                break;
        }

        int GetSingle(string str) => str switch
        {
            "First" => 1,
            "Second" => 2,
            "Third" => 3,
            "Fourth" => 4,
            "Fifth" => 5,
            "Sixth" => 6,
            "Seventh" => 7,
            _ => 0
        };

        (int, int) GetTuple(string str) => str switch
        {
            "12" => (1, 2),
            "13" => (1, 3),
            "14" => (1, 4),
            "15" => (1, 5),
            "23" => (2, 3),
            "24" => (2, 4),
            "25" => (2, 5),
            "34" => (3, 4),
            "35" => (3, 5),
            "45" => (4, 5),
            _ => (0, 0),
        };
    }
}

public partial class HistoryFeature : ObservableObject
{
    [ObservableProperty] private string _name = string.Empty;

    [ObservableProperty] private bool _isSelected;

    partial void OnIsSelectedChanged(bool oldValue, bool newValue)
    {
        WeakReferenceMessenger.Default.Send(this);
    }
}