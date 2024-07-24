using Microsoft.Extensions.Options;
using SpaceGame.BaseClasses;
using SpaceGame.Loggers;
using SpaceGame.Models;
using SpaceGame.Screen;

namespace SpaceGame.Maps
{
    internal class SpaceMap : ObjectMap
    {
        private DomainModel _domainModel;

        public string GetMapType() => "SpaceMap";

        public SpaceMap(
            IScreen screen,
            DomainModel domainModel,
            ILogger logger,
            IOptions<ScreenOptions> screenOptions) : base(screen, logger, screenOptions)
        {
            _domainModel = domainModel;

            _mapObjectType = MapObjectType.Planet;
            _homePositionObjectType = MapObjectType.HomePlanet;
            _playerCharacter = '%';
            _objectCharacter = '*';
            _objectDescription = "An ordinary planet";
            _startPositionCharacter = '@';
            _homePositionDescription = "Home Planet";
        }

        public override void SetPlayerPosition((int, int) currentPlayerPosition)
        {
            base.SetPlayerPosition(currentPlayerPosition);

            List<string> shipProperties = new List<string>()
                {
                    "Ship Properties:",
                    $"Total Fuel: {_domainModel.ShipProperties.TotalFuel}",
                    "Lander Properties:",
                    $"Total Fuel: {_domainModel.LanderProperties.TotalFuel}",
                    $"Fuel Flow Rate: {_domainModel.LanderProperties.FuelFlowRate}",
                    $"Maximum Fuel Consumption Rate: {_domainModel.LanderProperties.MaxFuelRate}",
                    $"Maximum Engine Thrust: {_domainModel.LanderProperties.MaxThrust}",
                    $"Lander Mass: {_domainModel.LanderProperties.LanderMass}"
                };

            for (int i = 0; i < shipProperties.Count; i++)
            {
                _mapText[i] = shipProperties[i];
            }
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
