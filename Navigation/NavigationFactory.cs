using Microsoft.Extensions.DependencyInjection;
using SpaceGame.Interfaces;

namespace SpaceGame.Navigation
{
    internal class NavigationFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // This factory is required because we have multiple implementations of IMap (e.g. SpaceMap, PlanetMap)
        // so we need a factory to return INavigation instances for each IMap implementation as needed
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

