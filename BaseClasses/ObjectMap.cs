using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;
using System.Text;

namespace SpaceGame.BaseClasses
{
    internal abstract class ObjectMap : IMap
    {
        private ILogger _logger;

        // Specify the min/max distances beween objects on the map
        protected const int MIN_OJBECT_DISTANCE = 20;
        protected const int MAX_OJBECT_DISTANCE = 50;
        protected char _playerCharacter = '%';
        protected const char _objectCharacter = '*';
        protected const char _vacantSpaceCharacter = ' ';

        protected int _width = 50;
        protected int _height = 100;
        protected char _charBehindPlayer = '\0';
        protected char[,]? _map = null;
        protected (int, int) _playerPosition;
        public (int, int) Position
        {
            get => _playerPosition;
            private set
            {
                if (_map != null)
                {
                    if (_charBehindPlayer != '\0')
                    {
                        // Replace the current player location with whatever was there before
                        _map[_playerPosition.Item1, _playerPosition.Item2] = _charBehindPlayer;
                    }

                    _playerPosition = value;

                    // Cache the thing currently at the location where the player will be
                    _charBehindPlayer = _map[_playerPosition.Item1, _playerPosition.Item2];
                    _map[_playerPosition.Item1, _playerPosition.Item2] = _playerCharacter;
                }
            }
        }

        public MapObject? ObjectAtPosition { get; private set; }
        public Dictionary<string, MapObject> ObjectDictionary { get; } = new Dictionary<string, MapObject>();

        public ObjectMap(ILogger logger)
        {
            _logger = logger;
        }

        // Create the map matrix with randomly placed objects as specified
        public void Build(int height, int width)
        {
            Random rand = new Random();

            _width = width;
            _height = height;
            _map = new char[height, width];

            // Populate the map with random objects:
            //  For each row, place objects at random intervals but not less than 2 spaces between them in any direction
            //  Fill the rest of the map with empty space
            for (int i = 0; i < _height; i++)
            {
                int starIndex = rand.Next(MAX_OJBECT_DISTANCE);
                string name = string.Empty;

                for (int j = 0; j < _width; j++)
                {
                    _logger.DebugPrintLine($"Current row: {i}");
                    _logger.DebugPrintLine($"Current index: {j}");
                    _logger.DebugPrintLine($"Next object will be at index {starIndex}");

                    if (j == starIndex)
                    {
                        name = DetermineObjectName(i, j);
                        MapObject mapObject = new MapObject(
                            name,
                            MapObjectType.Planet,
                            label: $"Planet {name}",
                            description: "An ordinary planet");

                        ObjectDictionary.Add(name, mapObject);

                        _logger.DebugPrintLine($"Placing object {name} at [{i}, {j}]");
                        _map[i, j] = _objectCharacter;
                        starIndex = j + rand.Next(MIN_OJBECT_DISTANCE, MAX_OJBECT_DISTANCE);
                    }
                    else
                    {
                        _map[i, j] = _vacantSpaceCharacter;
                    }
                }
            }
        }

        public void Display()
        {
            Console.Clear();

            if (_map == null)
            {
                _logger.DebugPrintLine("Map has not been built yet.");
                return;
            }

            _logger.DebugPrintLine("Map:");
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    Console.Write($"{_map[i, j]}");
                }
                Console.WriteLine();
            }

            foreach (MapObject mapObject in ObjectDictionary.Values)
            {
                _logger.DebugPrintLine($"MapObject: {mapObject.Label}");
            }
        }

        public void SetPlayerPosition((int, int) currentPlayerPosition)
        {
            Position = currentPlayerPosition;

            MapObject? mapObject;

            // Determine what the object name would be if there's an object there
            string label = DetermineObjectName(Position.Item1, Position.Item2);

            // See if the dictionary contains an object with that name (if so, there's an object there)
            ObjectDictionary.TryGetValue(label, out mapObject);
            ObjectAtPosition = mapObject;
        }

        private string DetermineObjectName(int row, int column)
        {
            StringBuilder objectName = new StringBuilder();

            // Either quadrant A or B
            if (row < _height/2)
            {
                if (column < _width/2)
                {
                    objectName.Append('A');
                }
                else
                {
                    objectName.Append('B');
                }
            }
            // Either quadrant C or D
            else
            {
                if (column < _width/2)
                {
                    objectName.Append('C');
                }
                else
                {
                    objectName.Append('D');
                }
            }

            objectName.Append("-" + row + "-" + column);
            return objectName.ToString();
        }

        // Calcute Game State so Navigation knows what scenario to call next
        public abstract GameState GetState();
    }
}
