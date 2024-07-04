using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpaceGame.Interfaces;
using SpaceGame.Lander;
using SpaceGame.Loggers;
using SpaceGame.Maps;
using SpaceGame.Models;
using SpaceGame.Navigation;
using SpaceGame.Space;
using SpaceGame.Vehicle;

namespace SpaceGame
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<ILogger, ConsoleLogger>();
            builder.Services.AddSingleton<IMap, SpaceMap>();
            builder.Services.AddSingleton<INavigation, Navigation.Navigation>();
            builder.Services.AddKeyedSingleton<IScenario, LanderLoop>("Lander");
            builder.Services.AddKeyedSingleton<IScenario, SpaceLoop>("Space");
            builder.Services.AddKeyedSingleton<IScenario, VehicleLoop>("Vehicle");
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
