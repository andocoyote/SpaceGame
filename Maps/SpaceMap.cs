using Microsoft.Extensions.Options;
using SpaceGame.BaseClasses;
using SpaceGame.Loggers;
using SpaceGame.Models;
using SpaceGame.Screen;
using System.Text.Json;

namespace SpaceGame.Maps
{
    internal class SpaceMap : ObjectMap
    {
        public string GetMapType() => "SpaceMap";

        public SpaceMap(
            IScreen screen,
            DomainModel domainModel,
            ILogger logger,
            IOptions<ScreenOptions> screenOptions) : base(screen, domainModel, logger, screenOptions)
        {
            // Set all of the default values for the Space Map
            _mapObjectType = MapObjectType.Planet;
            _homePositionObjectType = MapObjectType.HomePlanet;
            _playerCharacter = '%';
            _objectCharacter = '*';
            _objectDescription = "An ordinary planet";
            _startPositionCharacter = '@';
            _homePositionDescription = "Home Planet";
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

            _domainModel.SpaceMapModel = GenerateModel();
        }

        // ScenarioLoop calls Navigation to move around which uses the appropriate map to update the map
        // Calcute Game State so Navigation knows what scenario to call next
        public override GameState GetState()
        {
            GameState state = GameState.None;
            SpaceMapState spaceMapState = SpaceMapState.None;

            if (ObjectAtPosition == null)
            {
                state = GameState.EmtpySpace;
                spaceMapState = SpaceMapState.EmtpySpace;
            }
            else
            {
                switch (ObjectAtPosition.Type)
                {
                    case MapObjectType.Planet:
                        state = GameState.OverPlanet;
                        spaceMapState = SpaceMapState.OverPlanet;
                        break;

                    case MapObjectType.HomePlanet:
                        state = GameState.OverHomePlanet;
                        spaceMapState = SpaceMapState.OverHomePlanet;
                        break;

                    default:
                        state = GameState.None;
                        spaceMapState = SpaceMapState.None;
                        break;
                }
            }

            return state;
        }

        public SpaceMapModel GenerateModel()
        {
            SpaceMapModel model = new SpaceMapModel()
            {
                SpaceMapState = SpaceMapState.None, // TODO: set the actual state
                Position = this.Position,
                ObjectAtPosition = this.ObjectAtPosition
            };

            return model;
        }
        public string SerializeModel(SpaceMapModel model)
        {
            string modelSerialized = JsonSerializer.Serialize(model);

            return modelSerialized;
        }
        public SpaceMapModel? DeserializeModel(string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                return null;
            }

            SpaceMapModel? spaceMapModelDeserialized = JsonSerializer.Deserialize<SpaceMapModel>(model);

            return spaceMapModelDeserialized;
        }
    }
}
