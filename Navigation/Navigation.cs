using SpaceGame.Interfaces;
using SpaceGame.Models;

namespace SpaceGame.Navigation
{
    internal class Navigation : INavigation
    {
        private IMap? _map;
        private readonly int _mapHeight = 50;
        private readonly int _mapWidth = 100;
        private (int, int) _currentPlayerPosition = (0, 0);

        public Navigation(IMap? map)
        {
            _map = map;

            Random rand = new Random();

            // Build the map and assign the player to a random location on the map
            if (_map != null)
            {
                _currentPlayerPosition = (rand.Next(_mapHeight - 1), rand.Next(_mapWidth - 1));
                _map.Build(_mapHeight, _mapWidth);

                _map.Position = _currentPlayerPosition;
            }
        }

        public GameState MoveUp()
        {
            if (_map == null) return GameState.None;

            _currentPlayerPosition = (
                _currentPlayerPosition.Item1 > 0 ? _currentPlayerPosition.Item1 - 1 : 0,
                _currentPlayerPosition.Item2);

            _map.Position = _currentPlayerPosition;

            GameState state = _map.GetState();
            return state;
        }

        public GameState MoveDown()
        {
            if (_map == null) return GameState.None;

            _currentPlayerPosition = (
                _currentPlayerPosition.Item1 < _mapHeight - 1 ? _currentPlayerPosition.Item1 + 1 : _mapHeight - 1,
                _currentPlayerPosition.Item2);

            _map.Position = _currentPlayerPosition;

            GameState state = _map.GetState();
            return state;
        }

        public GameState MoveLeft()
        {
            if (_map == null) return GameState.None;

            _currentPlayerPosition = (
                _currentPlayerPosition.Item1,
                _currentPlayerPosition.Item2 > 0 ? _currentPlayerPosition.Item2 - 1 : 0);

            _map.Position = _currentPlayerPosition;

            GameState state = _map.GetState();
            return state;
        }

        public GameState MoveRight()
        {
            if (_map == null) return GameState.None;

            _currentPlayerPosition = (
                _currentPlayerPosition.Item1,
                _currentPlayerPosition.Item2 < _mapWidth - 1 ? _currentPlayerPosition.Item2 + 1 : _mapWidth - 1);

            _map.Position = _currentPlayerPosition;

            GameState state = _map.GetState();
            return state;
        }

        public void DisplayMap()
        {
            _map?.Display();
        }
    }
}
