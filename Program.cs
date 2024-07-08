using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpaceGame.Interfaces;
using SpaceGame.Lander;
using SpaceGame.Loggers;
using SpaceGame.Maps;
using SpaceGame.Models;
using SpaceGame.Navigation;
using SpaceGame.Planet;
using SpaceGame.Space;
using System.Globalization;

namespace SpaceGame
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<ILogger, ConsoleLogger>();
            builder.Services.AddKeyedSingleton<IMap, SpaceMap>("Space");
            builder.Services.AddKeyedSingleton<IMap, PlanetMap>("Planet");
            builder.Services.AddSingleton<INavigation, Navigation.Navigation>();
            builder.Services.AddSingleton<ILanderAnimation, LanderAnimation>();
            builder.Services.AddKeyedSingleton<IScenario, LanderLoop>("Lander");
            builder.Services.AddSingleton<DomainModel>();
            builder.Services.AddTransient<NavigationFactory>();

            // Since SpaceMap implements IMap, have to create the Navigation
            // instance with SpaceMap explicitly passed to the constructor
            builder.Services.AddSingleton<IScenario>(sp =>
            {
                var factory = sp.GetRequiredService<NavigationFactory>();
                var navigation = factory.CreateNavigation("SpaceMap");
                var domainModel = sp.GetRequiredService<DomainModel>();
                var logger = sp.GetRequiredService<ILogger>();

                return new SpaceLoop(navigation, domainModel, logger);
            });

            // Since PlanetMap implements IMap, have to create the Navigation
            // instance with PlanetMap explicitly passed to the constructor
            builder.Services.AddSingleton<IScenario>(sp =>
            {
                var factory = sp.GetRequiredService<NavigationFactory>();
                var navigation = factory.CreateNavigation("PlanetMap");
                var domainModel = sp.GetRequiredService<DomainModel>();
                var logger = sp.GetRequiredService<ILogger>();

                return new PlanetLoop(navigation, domainModel, logger);
            });

            // Have to request SpaceLoop and PlanetLoop since the both implement IScenario
            builder.Services.AddSingleton<GameLoop.GameLoop>(sp =>
            {
                var spaceLoop = sp.GetServices<IScenario>().OfType<SpaceLoop>().FirstOrDefault();
                var landerLoop = sp.GetKeyedService<IScenario>("Lander");
                var planetLoop = sp.GetServices<IScenario>().OfType<PlanetLoop>().FirstOrDefault();
                var domainModel = sp.GetRequiredService<DomainModel>();
                var logger = sp.GetRequiredService<ILogger>();

                return new GameLoop.GameLoop(spaceLoop, landerLoop, planetLoop, domainModel, logger);
            });

            using IHost host = builder.Build();

            // Run the game
            GameLoop.GameLoop gameLoop = host.Services.GetRequiredService<GameLoop.GameLoop>();
            gameLoop.Run();

            await host.RunAsync();            
        }
    }
}
