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
