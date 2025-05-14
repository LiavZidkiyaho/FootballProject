using FootballProject.Services;
using FootballProject.View;
using FootballProject.ViewModel;
using Microcharts.Maui;
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
                .UseMicrocharts()
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

            builder.Services.AddSingleton<ClubPlayersSearch>();
            builder.Services.AddSingleton<ClubPlayersSearchViewModel>();

            builder.Services.AddSingleton<PlayerProfile>();
            builder.Services.AddSingleton <PlayerProfileViewModel>();

            builder.Services.AddSingleton<IUser, WebService>();
            return builder.Build();
        }
    }
}
