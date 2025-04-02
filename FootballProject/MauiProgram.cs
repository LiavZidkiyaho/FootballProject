﻿using FootballProject.Services;
using FootballProject.View;
using FootballProject.ViewModel;
using Microsoft.Extensions.Logging;

namespace FootballProject
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<SignUp>();
            builder.Services.AddSingleton<SignUpViewModel>();

            builder.Services.AddSingleton<LogIn>();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddSingleton<ViewUsers>();
            builder.Services.AddSingleton<ViewUsersViewModel>();

            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomePageViewModel>();

            builder.Services.AddSingleton<FinancePage>();
            builder.Services.AddSingleton<FinancePageViewModel>();

            builder.Services.AddSingleton<PlayersSearch>();
            builder.Services.AddSingleton<PlayersSearchViewModel>();

            builder.Services.AddSingleton<UserService>();
            return builder.Build();
        }
    }
}
