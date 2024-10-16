﻿using MapApp.Services;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MapApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSerilog(
                new LoggerConfiguration()
                    .WriteTo.File(
                        Path.Combine(
                            FileSystem.AppDataDirectory, "logs", "logs.txt"),
                        rollingInterval: RollingInterval.Day)
                    .CreateLogger());

            builder.Services.AddLogging(logging =>
            {
                logging.AddSerilog(dispose: true);
            });

            builder.Services.AddSingleton<IWindowSizeChangedService, WindowSizeChangedService>();
            builder.Services.AddMauiBlazorWebView();
#if DEBUG   
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
