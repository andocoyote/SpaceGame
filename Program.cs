using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpaceGame.GameLoop;
using SpaceGame.Lander;
using SpaceGame.Logger;
using SpaceGame.Art;
using SpaceGame.Map;
using SpaceGame.Navigation;

namespace SpaceGame
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<ILogger, ConsoleLogger>();
            builder.Services.AddSingleton<IMap, Map.Map>();
            builder.Services.AddSingleton<INavigation, Navigation.Navigation>();
            builder.Services.AddSingleton<ILanderLoop, LanderLoop>();
            builder.Services.AddSingleton<IGameLoop, GameLoop.GameLoop>();

            using IHost host = builder.Build();

            string[,] splashPage = new string[50, 100];
            SplashPage.InitializeSplashPage(splashPage);
            SplashPage.DisplaySplashPage(splashPage);

            Thread.Sleep(3000);

            // Run the game
            IGameLoop gameLoop = host.Services.GetRequiredService<IGameLoop>();
            gameLoop.Run();

            await host.RunAsync();            
        }
    }
}
