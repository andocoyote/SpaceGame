using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpaceGame.SpaceLoop;
using SpaceGame.Lander;
using SpaceGame.Logger;
using SpaceGame.Art;
using SpaceGame.Map;
using SpaceGame.Navigation;
using SpaceGame.Interfaces;
using SpaceGame.Models;

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
            builder.Services.AddSingleton<IScenario, LanderLoop>();
            builder.Services.AddSingleton<ISpaceLoop, SpaceLoop.SpaceLoop>();
            builder.Services.AddSingleton<DomainModel>();
            builder.Services.AddSingleton<GameLoop.GameLoop>();

            using IHost host = builder.Build();

            // Run the game
            GameLoop.GameLoop gameLoop = host.Services.GetRequiredService<GameLoop.GameLoop>();
            gameLoop.Run();

            await host.RunAsync();            
        }
    }
}
