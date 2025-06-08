using FootballProject.Services;
using FootballProject.View;
using FootballProject.ViewModel;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;

namespace FootballProject
{
    /// <summary>
    /// The main entry point to configure and build the .NET MAUI application.
    /// Registers views, view models, services, fonts, and third-party libraries.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Configures and builds the MAUI application.
        /// </summary>
        /// <returns>A fully configured <see cref="MauiApp"/> instance.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Base MAUI app and Microcharts configuration
            builder
                .UseMauiApp<App>()
                .UseMicrocharts() // Enables charting support
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug(); // Enable debug logging in development
#endif

            // Register pages (Views) and their corresponding ViewModels
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
            builder.Services.AddSingleton<PlayerProfileViewModel>();

            // Register the user service implementation (used across the app)
            builder.Services.AddSingleton<IUser, WebService>();

            return builder.Build();
        }
    }
}
