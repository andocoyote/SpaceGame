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

namespace SpaceGame
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            // Maps and animation
            builder.Services.AddKeyedSingleton<IMap, SpaceMap>("Space");
            builder.Services.AddKeyedSingleton<IMap, PlanetMap>("Planet");
            builder.Services.AddSingleton<ILanderAnimation, LanderAnimation>();

            // Navigation
            builder.Services.AddTransient<NavigationFactory>();
            builder.Services.AddSingleton<INavigation, Navigation.Navigation>();;

            // Scenarios
            builder.Services.AddKeyedSingleton<IScenario, LanderLoop>("Lander");

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

            // Models
            builder.Services.AddSingleton<DomainModel>();

            // Loggers
            builder.Services.AddSingleton<ILogger, ConsoleLogger>();

            // Game Loop
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
