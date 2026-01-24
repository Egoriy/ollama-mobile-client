using Microsoft.Extensions.Logging;
using OllamaMobileClient.App.Presentation.Pages;
using OllamaMobileClient.App.Settings;
using OllamaMobileClient.App.ViewModels;
using OllamaMobileClient.Applications.Abstractions;
using OllamaMobileClient.Domain.Settings;
using OllamaMobileClient.Infrastructure.Backends.DirectOllama;
using OllamaMobileClient.Infrastructure.Settings;

namespace OllamaMobileClient.App
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; } = default!;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>();

            builder.Services.AddSingleton(new HttpClient
            {
                Timeout = Timeout.InfiniteTimeSpan
            });

            builder.Services.AddSingleton<IConnectionSettingsStore, PreferencesConnectionSettingsStore>();

            builder.Services.AddSingleton<IChatBackend, DirectOllamaBackend>();

            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsPage>();

            builder.Services.AddSingleton<MainPage>();

            var app = builder.Build();
            Services = app.Services;
            return app;
        }
    }
}
