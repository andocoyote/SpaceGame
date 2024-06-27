﻿namespace SpaceGame
{
    internal class Map : IMap
    {
        private const bool _debugPrint = false;
        private const int _minStarDistance = 20;
        private const int _maxStarDistance = 50;
        private int _width = 0;
        private int _height = 0;
        private char _charBehindShip = '\0';
        private const char _shipCharacter = '%';
        private char[,]? _map = null;

        private (int, int) _shipPosition;
        public (int, int) ShipPosition
        {
            get => _shipPosition;
            set
            {
                if (_map != null)
                {
                    if (_charBehindShip != '\0')
                    {
                        // Replace the current ship location with whatever was there before
                        _map[_shipPosition.Item1, _shipPosition.Item2] = _charBehindShip;
                    }

                    _shipPosition = value;

                    // Cache the thing currently at the location where the ship will be
                    _charBehindShip = _map[_shipPosition.Item1, _shipPosition.Item2];
                    _map[_shipPosition.Item1, _shipPosition.Item2] = _shipCharacter;
                }
            }
        }

        public void Build(int height, int width)
        {
            Random rand = new Random();

            _width = width;
            _height = height;
            _map = new char[height, width];

            // Populate the map with random stars:
            //  For each row, place stars at random intervals but not less than 2 spaces between them in any direction
            //  Fill the rest of the map with empty space
            for (int i = 0; i < _height; i++)
            {
                int starIndex = rand.Next(_maxStarDistance);

                for (int j = 0; j < _width; j++)
                {
                    DebugPrintLine($"Current row: {i}");
                    DebugPrintLine($"Current index: {j}");
                    DebugPrintLine($"Next star will be at index {starIndex}");

                    if (j == starIndex)
                    {
                        DebugPrintLine($"Placing star at [{i}, {j}]");
                        _map[i, j] = '*';
                        starIndex = j + rand.Next(_minStarDistance, _maxStarDistance);
                    }
                    else
                    {
                        _map[i, j] = ' ';
                    }
                }
            }
        }

        public void Display()
        {
            if (_map == null)
            {
                DebugPrintLine("Map has not been built yet.");
                return;
            }

            DebugPrintLine("Map:");
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    Console.Write($"{_map[i, j]}");
                }
                Console.WriteLine();
            }
        }

        private void DebugPrint(string msg)
        {
            if (_debugPrint)
            {
#pragma warning disable CS0162 // Unreachable code detected
                Console.Write(msg);
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        private void DebugPrintLine(string msg)
        {
            if (_debugPrint)
            {
#pragma warning disable CS0162 // Unreachable code detected
                Console.WriteLine(msg);
#pragma warning restore CS0162 // Unreachable code detected
            }
        }
    }
}
