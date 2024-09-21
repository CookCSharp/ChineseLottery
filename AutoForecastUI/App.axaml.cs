using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AutoForecastUI.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using HotAvalonia;
using Microsoft.Extensions.DependencyInjection;

namespace AutoForecastUI;

public partial class App : Application
{
    private void ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddScoped<SearchView>();
        services.AddScoped<LastPeriodResultView>();
        services.AddScoped<ForecastView>();
        
        Ioc.Default.ConfigureServices(services.BuildServiceProvider());
    }
    
    public override void Initialize()
    {
        this.EnableHotReload();
        ConfigureServices();
        
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}