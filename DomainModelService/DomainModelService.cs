using SpaceGame.Loggers;
using SpaceGame.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpaceGame.DomainModelService
{
    internal class DomainModelService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters =
            {
                new ShipStateConverter(),
                new LanderStateConverter(),
                new SpaceMapStateConverter(),
                new PlaneMapStateConverter()
            }
        };

        private const string DOMAIN_MODEL_FILENAME = "space_game_model.json";
        private const string APP_NAME = "SpaceGame";
        private const string GAMES_DIRECTORY = "Games";

        private string _playerName = string.Empty;

        // Something like: C:\Users\{UserName}\AppData\Local\SpaceGame\{PlayerName}\space_game_model.json
        string _domainModelFileDirectoy = string.Empty;
        string _domainModelFilePath = string.Empty;
        string _gamesDirectory = string.Empty;

        private DomainModel? _domainModel;
        private ILogger _logger;

        public DomainModelService(
            DomainModel domainModel,
            ILogger logger)
        {
            _domainModel = domainModel;
            _logger = logger;

            // Create the directory that contains the saved games
            _gamesDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                APP_NAME,
                GAMES_DIRECTORY);

            if (!Directory.Exists(_gamesDirectory))
            {
                Directory.CreateDirectory(_gamesDirectory);
            }
        }

        /// <summary>
        /// Saves the Domain Model to disk
        /// </summary>
        public async Task SaveDomainModelAsync()
        {
            if (_domainModel == null) return;

            // If the file path hasn't been set yet, this is the first time saving the model
            if (string.IsNullOrEmpty(_domainModelFilePath))
            {
                _playerName = _domainModel.PlayerName;

                // Create the directory
                _domainModelFileDirectoy = Path.Combine(
                    _gamesDirectory,
                    _playerName);

                Directory.CreateDirectory(_domainModelFileDirectoy);

                // Create the full file path
                _domainModelFilePath = Path.Combine(
                    _domainModelFileDirectoy,
                    DOMAIN_MODEL_FILENAME);
            }

            string serializedDomainModel = SerializeModel(_domainModel);

            using (StreamWriter writer = new StreamWriter(_domainModelFilePath, append: false))
            {
                await writer.WriteLineAsync(serializedDomainModel);
            }
        }

        /// <summary>
        /// Loads an existing Domain Model file for an existing player
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoadDomainModelAsync(string playerName)
        {
            _playerName = playerName;

            if (string.IsNullOrEmpty(_domainModelFilePath))
            {
                // Create the full path to the domain model file
                _domainModelFileDirectoy = Path.Combine(
                    _gamesDirectory,
                    _playerName,
                    DOMAIN_MODEL_FILENAME);
            }

            if (!File.Exists(_domainModelFileDirectoy))
            {
                return false;
            }

            using Task<string> readTask = File.ReadAllTextAsync(_domainModelFileDirectoy);
            string lines = await readTask;

            _domainModel = DeserializeModel(lines);

            return true;
        }

        public bool DeleteSavedModel(string playerName)
        {
            bool modelDeleted = false;

            string dirToDelete = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    _gamesDirectory,
                    playerName);

            if (Directory.Exists(dirToDelete))
            {
                Directory.Delete(dirToDelete, true);
                modelDeleted = true;
            }

            return modelDeleted;
        }

        public List<string> GetAllGames()
        {
            List<string> playerNames = new();

            var savedGames = Directory.EnumerateDirectories(_gamesDirectory);

            // Get only the end of the path which is the name of the player
            foreach (string name in savedGames)
            {
                var dirs = name.Split('\\');

                playerNames.Add(dirs[dirs.Length - 1]);
            }

            return playerNames;
        }

        public bool ModelExists(string playerName)
        {
            bool modelExists = false;

            if (!string.IsNullOrEmpty(playerName))
            {
                // Create the full path to the domain model file
                _domainModelFileDirectoy = Path.Combine(
                    _gamesDirectory,
                    playerName,
                    DOMAIN_MODEL_FILENAME);

                modelExists = File.Exists(_domainModelFileDirectoy);
            }

            return modelExists;
        }

        public string SerializeModel(DomainModel model)
        {
            string modelSerialized = JsonSerializer.Serialize(model, options);

            return modelSerialized;
        }

        public DomainModel? DeserializeModel(string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                return null;
            }

            DomainModel? domainModelDeserialized = JsonSerializer.Deserialize<DomainModel>(model, options);

            return domainModelDeserialized;
        }
    }

    public class ShipStateConverter : JsonConverter<ShipState>
    {
        // Serialize the static fields manually
        public override void Write(Utf8JsonWriter writer, ShipState value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("State", value.ToString());
            writer.WriteEndObject();
        }

        // Deserialize static fields (re-initialize them if necessary)
        public override ShipState? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Ensure we are at the start of the "ShipState" object in the JSON
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            // Local variables to hold the deserialized values
            ShipState stateName = ShipState.None;

            // Read through the JSON object
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    // End of the object
                    break;
                }

                // Get the property name
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString() ?? string.Empty;

                    // Move to the property value
                    reader.Read();

                    if (propertyName == "State")
                    {
                        string propertyValue = reader.GetString() ?? string.Empty;

                        switch (propertyName)
                        {
                            case nameof(ShipState.None):
                                stateName = ShipState.None;
                                break;
                            case nameof(ShipState.Landed):
                                stateName = ShipState.Landed;
                                break;
                            case nameof(ShipState.InOrbit):
                                stateName = ShipState.InOrbit;
                                break;
                            case nameof(ShipState.Flying):
                                stateName = ShipState.Flying;
                                break;
                            case nameof(ShipState.OutOfFuel):
                                stateName = ShipState.OutOfFuel;
                                break;
                            case nameof(ShipState.Crashed):
                                stateName = ShipState.Crashed;
                                break;
                            case nameof(ShipState.Landing):
                                stateName = ShipState.Landing;
                                break;
                            case nameof(ShipState.Docking):
                                stateName = ShipState.Docking;
                                break;
                            default:
                                stateName = ShipState.None;
                                break;
                        }
                    }
                }
            }

            return stateName;
        }
    }

    public class LanderStateConverter : JsonConverter<LanderState>
    {
        // Serialize the static fields manually
        public override void Write(Utf8JsonWriter writer, LanderState value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("State", value.ToString());
            writer.WriteEndObject();
        }

        // Deserialize static fields (re-initialize them if necessary)
        public override LanderState? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Ensure we are at the start of the "LanderState" object in the JSON
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            // Local variables to hold the deserialized values
            LanderState stateName = LanderState.None;

            // Read through the JSON object
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    // End of the object
                    break;
                }

                // Get the property name
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString() ?? string.Empty;

                    // Move to the property value
                    reader.Read();

                    if (propertyName == "State")
                    {
                        string propertyValue = reader.GetString() ?? string.Empty;

                        switch (propertyName)
                        {
                            case nameof(LanderState.None):
                                stateName = LanderState.None;
                                break;
                            case nameof(LanderState.Landed):
                                stateName = LanderState.Landed;
                                break;
                            case nameof(LanderState.Docked):
                                stateName = LanderState.Docked;
                                break;
                            case nameof(LanderState.Flying):
                                stateName = LanderState.Flying;
                                break;
                            case nameof(LanderState.OutOfFuel):
                                stateName = LanderState.OutOfFuel;
                                break;
                            case nameof(LanderState.Crashed):
                                stateName = LanderState.Crashed;
                                break;
                            case nameof(LanderState.Landing):
                                stateName = LanderState.Landing;
                                break;
                            case nameof(LanderState.Docking):
                                stateName = LanderState.Docking;
                                break;
                            default:
                                stateName = LanderState.None;
                                break;
                        }
                    }
                }
            }

            return stateName;
        }
    }

    public class SpaceMapStateConverter : JsonConverter<SpaceMapState>
    {
        // Serialize the static fields manually
        public override void Write(Utf8JsonWriter writer, SpaceMapState value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("State", value.ToString());
            writer.WriteEndObject();
        }

        // Deserialize static fields (re-initialize them if necessary)
        public override SpaceMapState? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Ensure we are at the start of the "SpaceMapState" object in the JSON
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            // Local variables to hold the deserialized values
            SpaceMapState stateName = SpaceMapState.None;

            // Read through the JSON object
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    // End of the object
                    break;
                }

                // Get the property name
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString() ?? string.Empty;

                    // Move to the property value
                    reader.Read();

                    if (propertyName == "State")
                    {
                        string propertyValue = reader.GetString() ?? string.Empty;

                        switch (propertyValue)
                        {
                            case nameof(SpaceMapState.None):
                                stateName = SpaceMapState.None;
                                break;
                            case nameof(SpaceMapState.ExitGame):
                                stateName = SpaceMapState.ExitGame;
                                break;
                            case nameof(SpaceMapState.EmtpySpace):
                                stateName = SpaceMapState.EmtpySpace;
                                break;
                            case nameof(SpaceMapState.OverHomePlanet):
                                stateName = SpaceMapState.OverHomePlanet;
                                break;
                            case nameof(SpaceMapState.OnHomePlanet):
                                stateName = SpaceMapState.OnHomePlanet;
                                break;
                            case nameof(SpaceMapState.InitiateHomePlanetLanding):
                                stateName = SpaceMapState.InitiateHomePlanetLanding;
                                break;
                            case nameof(SpaceMapState.ShipCrashed):
                                stateName = SpaceMapState.ShipCrashed;
                                break;
                            case nameof(SpaceMapState.OverPlanet):
                                stateName = SpaceMapState.OverPlanet;
                                break;
                            case nameof(SpaceMapState.InitiatePlanetLanding):
                                stateName = SpaceMapState.InitiatePlanetLanding;
                                break;
                            default:
                                stateName = SpaceMapState.None;
                                break;
                        }
                    }
                }
            }

            return stateName;
        }
    }
    public class PlaneMapStateConverter : JsonConverter<PlanetMapState>
    {
        // Serialize the static fields manually
        public override void Write(Utf8JsonWriter writer, PlanetMapState value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("State", value.ToString());
            writer.WriteEndObject();
        }

        // Deserialize static fields (re-initialize them if necessary)
        public override PlanetMapState? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Ensure we are at the start of the "PlanetMapState" object in the JSON
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            // Local variables to hold the deserialized values
            PlanetMapState stateName = PlanetMapState.None;

            // Read through the JSON object
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    // End of the object
                    break;
                }

                // Get the property name
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString() ?? string.Empty;

                    // Move to the property value
                    reader.Read();

                    if (propertyName == "State")
                    {
                        string propertyValue = reader.GetString() ?? string.Empty;

                        switch (propertyName)
                        {
                            case nameof(PlanetMapState.None):
                                stateName = PlanetMapState.None;
                                break;
                            case nameof(PlanetMapState.ExitGame):
                                stateName = PlanetMapState.ExitGame;
                                break;
                            case nameof(PlanetMapState.InitiateHomePlanetLanding):
                                stateName = PlanetMapState.InitiateHomePlanetLanding;
                                break;
                            case nameof(PlanetMapState.ShipCrashed):
                                stateName = PlanetMapState.ShipCrashed;
                                break;
                            case nameof(PlanetMapState.OverPlanet):
                                stateName = PlanetMapState.OverPlanet;
                                break;
                            case nameof(PlanetMapState.InitiatePlanetLanding):
                                stateName = PlanetMapState.InitiatePlanetLanding;
                                break;
                            case nameof(PlanetMapState.OnLandingZone):
                                stateName = PlanetMapState.OnLandingZone;
                                break;
                            case nameof(PlanetMapState.InitiateDocking):
                                stateName = PlanetMapState.InitiateDocking;
                                break;
                            case nameof(PlanetMapState.InFight):
                                stateName = PlanetMapState.InFight;
                                break;
                            case nameof(PlanetMapState.LanderCrashed):
                                stateName = PlanetMapState.LanderCrashed;
                                break;
                            case nameof(PlanetMapState.EmtpyLand):
                                stateName = PlanetMapState.EmtpyLand;
                                break;
                            case nameof(PlanetMapState.OverItem):
                                stateName = PlanetMapState.OverItem;
                                break;
                            case nameof(PlanetMapState.InspectItem):
                                stateName = PlanetMapState.InspectItem;
                                break;
                            default:
                                stateName = PlanetMapState.None;
                                break;
                        }
                    }
                }
            }

            return stateName;
        }
    }
}
