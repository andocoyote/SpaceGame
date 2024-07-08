using Microsoft.Extensions.DependencyInjection;
using SpaceGame.Interfaces;
using SpaceGame.Maps;

namespace SpaceGame.Navigation
{
    internal class NavigationFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public INavigation CreateNavigation(string mapType)
        {
            return mapType switch
            {
                "SpaceMap" => new Navigation(_serviceProvider.GetKeyedService<IMap>("Space")),
                "PlanetMap" => new Navigation(_serviceProvider.GetKeyedService<IMap>("Planet")),
                _ => throw new ArgumentException($"Unknown map type: {mapType}")
            };
        }
    }
}

