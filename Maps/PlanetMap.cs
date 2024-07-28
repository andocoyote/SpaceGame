using Microsoft.Extensions.Options;
using SpaceGame.BaseClasses;
using SpaceGame.Loggers;
using SpaceGame.Models;
using SpaceGame.Screen;

namespace SpaceGame.Maps
{
    internal class PlanetMap : ObjectMap
    {
        public string GetMapType() => "PlanetMap";

        public PlanetMap(
            IScreen screen,
            DomainModel domainModel,
            ILogger logger,
            IOptions<ScreenOptions> screenOptions) : base(screen, domainModel, logger, screenOptions)
        {
            // Set all of the default values
            _mapObjectType = MapObjectType.Mountain;
            _homePositionObjectType = MapObjectType.LandingZone;
            _playerCharacter = '#';
            _objectCharacter = '^';
            _objectDescription = "An ordinary mountain";
            _startPositionCharacter = '@';
            _homePositionDescription = "Landing Zone";
        }

        // ScenarioLoop calls Navigation to move around which uses the appropriate map to update the map
        // Calcute Game State so Navigation knows what scenario to call next
        public override GameState GetState()
        {
            GameState state = GameState.None;

            if (ObjectAtPosition == null)
            {
                state = GameState.EmtpyLand;
            }
            else
            {
                switch (ObjectAtPosition.Type)
                {
                    case MapObjectType.Mountain:
                        state = GameState.OverItem;
                        break;

                    case MapObjectType.LandingZone:
                        state = GameState.OnLandingZone;
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
