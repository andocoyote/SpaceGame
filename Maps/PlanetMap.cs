using SpaceGame.BaseClasses;
using SpaceGame.Loggers;
using SpaceGame.Models;

namespace SpaceGame.Maps
{
    internal class PlanetMap : ObjectMap
    {
        public string GetMapType() => "PlanetMap";

        public PlanetMap(ILogger logger) : base(logger)
        {
            _playerCharacter = '#';
        }

        // ScenarioLoop calls Navigation to move around which uses the appropriate map to update the map
        // Calcute Game State so Navigation knows what scenario to call next
        public override GameState GetState()
        {
            GameState state = GameState.None;

            switch (_charBehindPlayer)
            {
                case _vacantSpaceCharacter:
                    state = GameState.EmtpyLand;
                    break;

                case _objectCharacter:
                    state = GameState.OnLandingZone;
                    break;

                default:
                    state = GameState.None;
                    break;
            }

            return state;
        }
    }
}
