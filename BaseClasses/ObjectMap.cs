using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;

namespace SpaceGame.BaseClasses
{
    internal abstract class ObjectMap : IMap
    {
        private ILogger _logger;

        protected const int MIN_OJBECT_DISTANCE = 20;
        protected const int MAX_OJBECT_DISTANCE = 50;
        protected char _playerCharacter = '%';
        protected const char _objectCharacter = '*';
        protected const char _vacantSpaceCharacter = ' ';

        protected int _width = 0;
        protected int _height = 0;
        protected char _charBehindPlayer = '\0';
        protected char[,]? _map = null;

        protected (int, int) _playerPosition;
        public (int, int) Position
        {
            get => _playerPosition;
            set
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

        public ObjectMap(ILogger logger)
        {
            _logger = logger;
        }

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

                for (int j = 0; j < _width; j++)
                {
                    _logger.DebugPrintLine($"Current row: {i}");
                    _logger.DebugPrintLine($"Current index: {j}");
                    _logger.DebugPrintLine($"Next object will be at index {starIndex}");

                    if (j == starIndex)
                    {
                        _logger.DebugPrintLine($"Placing object at [{i}, {j}]");
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
        }

        // Calcute Game State so Navigation knows what scenario to call next
        public abstract GameState GetState();
    }
}
