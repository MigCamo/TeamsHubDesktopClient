using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Windows;
using TeamsHubDesktopClient.Gateways.Provider;

namespace TeamsHubDesktopClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var logger = ServiceProvider.GetService<ILogger<App>>();
        logger.LogInformation("Application started.");

        base.OnStartup(e);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(dispose: true);
        });

        services.AddSingleton<UserIdentityManagerRESTProvider>();
        services.AddSingleton<FileManagementRESTProvider>();
        services.AddSingleton<ProjectManagementRESTProvider>();
        services.AddSingleton<StudentManagementRESTProvider>();
        services.AddSingleton<TaskManagementRESTProvider>();
        services.AddSingleton<UserIdentityManagerRESTProvider>();
        services.AddTransient<MainWindow>();
    }
}

