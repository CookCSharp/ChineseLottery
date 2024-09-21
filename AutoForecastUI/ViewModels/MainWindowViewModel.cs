using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoForecast;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AutoForecastUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<string> _views;

    [ObservableProperty] private ContentControl? _view;

    [ObservableProperty] private int _selectedViewIndex;


    public MainWindowViewModel()
    {
        Views = ["Search", "Forecast", "LastPeriodResult"];
    }

    public void OnLoaded()
    {
    }

    [RelayCommand]
    private void SelectView()
    {
        View = (UserControl)Ioc.Default.GetService(Type.GetType($"AutoForecastUI.Views.{Views[SelectedViewIndex]}View")!)!;
    }
}