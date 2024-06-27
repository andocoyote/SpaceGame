using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SpaceGame
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<IMap, Map>();
            builder.Services.AddSingleton<INavigation, Navigation>();
            builder.Services.AddSingleton<IGameLoop, GameLoop>();

            using IHost host = builder.Build();

            // Run the game
            IGameLoop gameLoop = host.Services.GetRequiredService<IGameLoop>();
            gameLoop.Run();

            await host.RunAsync();            
        }
    }
}
