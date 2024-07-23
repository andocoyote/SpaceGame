using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SpaceGame.Interfaces;
using SpaceGame.Models;
using SpaceGame.Screen;

namespace SpaceGame.Navigation
{
    internal class NavigationFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<ScreenOptions> _screenOptions;

        public NavigationFactory(
            IServiceProvider serviceProvider,
            IOptions<ScreenOptions> screenOptions)
        {
            _serviceProvider = serviceProvider;
            _screenOptions = screenOptions;
        }

        // This factory is required because we have multiple implementations of IMap (e.g. SpaceMap, PlanetMap)
        // so we need a factory to return INavigation instances for each IMap implementation as needed
        public INavigation CreateNavigation(string mapType)
        {
            return mapType switch
            {
                "SpaceMap" => new Navigation(
                    _serviceProvider.GetKeyedService<IMap>("Space"),
                    _serviceProvider.GetService<DomainModel>(),
                    _screenOptions),
                "PlanetMap" => new Navigation(
                    _serviceProvider.GetKeyedService<IMap>("Planet"),
                    _serviceProvider.GetService<DomainModel>(),
                    _screenOptions),
                _ => throw new ArgumentException($"Unknown map type: {mapType}")
            };
        }
    }
}

