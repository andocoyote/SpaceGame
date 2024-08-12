using Microsoft.Extensions.Options;
using SpaceGame.BaseClasses;
using SpaceGame.Loggers;
using SpaceGame.Models;
using SpaceGame.Screen;
using System.Text.Json;

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
            // Set all of the default values for the Planet Map
            _mapObjectType = MapObjectType.Mountain;
            _homePositionObjectType = MapObjectType.LandingZone;
            _playerCharacter = '#';
            _objectCharacter = '^';
            _objectDescription = "An ordinary mountain";
            _startPositionCharacter = '@';
            _homePositionDescription = "Landing Zone";
        }

        // Navigation calls this with every arrow key.
        // ObjectMap calls this to set the initial player position.
        // Use this to update the domain model with the player position.
        public override void SetPlayerPosition((int, int) currentPlayerPosition)
        {
            base.SetPlayerPosition(currentPlayerPosition);

            List<string> ShipModel = new List<string>()
                {
                    "Ship Properties:",
                    $"Total Fuel: {_domainModel.ShipModel.TotalFuel}",
                    "Lander Properties:",
                    $"Total Fuel: {_domainModel.LanderModel.TotalFuel}",
                    $"Fuel Flow Rate: {_domainModel.LanderModel.FuelFlowRate}",
                    $"Maximum Fuel Consumption Rate: {_domainModel.LanderModel.MaxFuelRate}",
                    $"Maximum Engine Thrust: {_domainModel.LanderModel.MaxThrust}",
                    $"Lander Mass: {_domainModel.LanderModel.LanderMass}"
                };

            for (int i = 0; i < ShipModel.Count; i++)
            {
                _mapText[i] = ShipModel[i];
            }

            _domainModel.PlanetMapModel = GenerateModel();
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

        public PlanetMapModel GenerateModel()
        {
            PlanetMapModel model = new PlanetMapModel()
            {
                PlanetMapState = PlanetMapState.None, // TODO: set the actual state
                Position = this.Position,
                ObjectAtPosition = this.ObjectAtPosition
            };

            return model;
        }
        public string SerializeModel(PlanetMapModel model)
        {
            string modelSerialized = JsonSerializer.Serialize(model);

            return modelSerialized;
        }
        public PlanetMapModel? DeserializeModel(string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                return null;
            }

            PlanetMapModel? planetMapModelDeserialized = JsonSerializer.Deserialize<PlanetMapModel>(model);

            return planetMapModelDeserialized;
        }
    }
}
