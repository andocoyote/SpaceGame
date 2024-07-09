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
