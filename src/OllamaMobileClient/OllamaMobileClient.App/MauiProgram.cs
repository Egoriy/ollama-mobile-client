using Microsoft.Extensions.Logging;
using OllamaMobileClient.Applications.Abstractions;
using OllamaMobileClient.Infrastructure.Backends.DirectOllama;
using OllamaMobileClient.Infrastructure.Settings;

namespace OllamaMobileClient.App
{
    public static class MauiProgram
    {

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>();

            // Настройки (позже вынесем в экран Settings + SecureStorage)
            var cfg = new OllamaConnection
            {
                BaseUrl = "http://192.168.1.241:11434",
                Model = "qwen3:8b"
            };

            builder.Services.AddSingleton(cfg);
            builder.Services.AddSingleton(new HttpClient());

            builder.Services.AddSingleton<IChatBackend, DirectOllamaBackend>();
            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}
