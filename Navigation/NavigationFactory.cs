using Microsoft.Extensions.DependencyInjection;
using SpaceGame.Interfaces;
using SpaceGame.Models;

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
                "SpaceMap" => new Navigation(
                    _serviceProvider.GetKeyedService<IMap>("Space"),
                    _serviceProvider.GetService<DomainModel>()),
                "PlanetMap" => new Navigation(
                    _serviceProvider.GetKeyedService<IMap>("Planet"),
                    _serviceProvider.GetService<DomainModel>()),
                _ => throw new ArgumentException($"Unknown map type: {mapType}")
            };
        }
    }
}

