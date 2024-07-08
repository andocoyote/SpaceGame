using SpaceGame.BaseClasses;
using SpaceGame.Loggers;
using SpaceGame.Models;

namespace SpaceGame.Maps
{
    internal class SpaceMap : ObjectMap
    {
        public string GetMapType() => "SpaceMap";

        public SpaceMap(ILogger logger) : base(logger)
        {
            ;
        }

        public override GameState GetState()
        {
            GameState state = GameState.None;

            switch (_charBehindPlayer)
            {
                case _vacantSpaceCharacter:
                    state = GameState.EmtpySpace;
                    break;

                case _objectCharacter:
                    state = GameState.OverPlanet;
                    break;

                default:
                    state = GameState.None;
                    break;
            }

            return state;
        }
    }
}
