using Microsoft.Extensions.Options;
using SpaceGame.BaseClasses;
using SpaceGame.Loggers;
using SpaceGame.Models;
using SpaceGame.Screen;

namespace SpaceGame.Maps
{
    internal class SpaceMap : ObjectMap
    {
        public string GetMapType() => "SpaceMap";

        public SpaceMap(
            IScreen screen,
            ILogger logger,
            IOptions<ScreenOptions> screenOptions) : base(screen, logger, screenOptions)
        {
            _mapObjectType = MapObjectType.Planet;
            _homePositionObjectType = MapObjectType.HomePlanet;
            _playerCharacter = '%';
            _objectCharacter = '*';
            _objectDescription = "An ordinary planet";
            _startPositionCharacter = '@';
            _homePositionDescription = "Home Planet";
        }

        // ScenarioLoop calls Navigation to move around which uses the appropriate map to update the map
        // Calcute Game State so Navigation knows what scenario to call next
        public override GameState GetState()
        {
            GameState state = GameState.None;

            if (ObjectAtPosition == null)
            {
                state = GameState.EmtpySpace;
            }
            else
            {
                switch (ObjectAtPosition.Type)
                {
                    case MapObjectType.Planet:
                        state = GameState.OverPlanet;
                        break;

                    case MapObjectType.HomePlanet:
                        state = GameState.OverHomePlanet;
                        break;

                    default:
                        state = GameState.None;
                        break;
                }
            }

            return state;
        }
    }
}
