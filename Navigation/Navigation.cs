using Microsoft.Extensions.Options;
using SpaceGame.Interfaces;
using SpaceGame.Models;
using SpaceGame.Screen;

namespace SpaceGame.Navigation
{
    internal class Navigation : INavigation
    {
        private IMap? _map;
        private readonly int _mapHeight = 0;
        private readonly int _mapWidth = 0;
        private (int, int) _currentPlayerPosition = (0, 0);
        private DomainModel? _domainModel;
        private IOptions<ScreenOptions>? _screenOptions;

        public Navigation(
            IMap? map,
            DomainModel? domainModel,
            IOptions<ScreenOptions>? screenOptions)
        {
            _map = map;
            _domainModel = domainModel;
            _screenOptions = screenOptions;

            if (_screenOptions != null)
            {
                _mapHeight = _screenOptions.Value.GraphicsHeight;
                _mapWidth = _screenOptions.Value.GraphicsWidth;
            }

            Random rand = new Random();

            // Build the map and assign the player to a random location on the map
            if (_map != null)
            {
                _map.Build(_mapHeight, _mapWidth);
                _currentPlayerPosition = _map.Position;
            }
        }

        public GameState MoveUp()
        {
            if (_map == null || _domainModel == null) return GameState.None;

            // Calculate the map coordinates of the player's new position after moving
            _currentPlayerPosition = (
                _currentPlayerPosition.Item1 > 0 ? _currentPlayerPosition.Item1 - 1 : 0,
                _currentPlayerPosition.Item2);

            // Tell IMap to set the character at the new position
            _map.SetPlayerPosition(_currentPlayerPosition);
            _domainModel.MapObject = _map.ObjectAtPosition;

            // Ask the map what kind of object is at the player's position
            GameState state = _map.GetState();
            return state;
        }

        public GameState MoveDown()
        {
            if (_map == null || _domainModel == null) return GameState.None;

            // Calculate the map coordinates of the player's new position after moving
            _currentPlayerPosition = (
                _currentPlayerPosition.Item1 < _mapHeight - 1 ? _currentPlayerPosition.Item1 + 1 : _mapHeight - 1,
                _currentPlayerPosition.Item2);

            // Tell IMap to set the character at the new position
            _map.SetPlayerPosition(_currentPlayerPosition);
            _domainModel.MapObject = _map.ObjectAtPosition;

            // Ask the map what kind of object is at the player's position
            GameState state = _map.GetState();
            return state;
        }

        public GameState MoveLeft()
        {
            if (_map == null || _domainModel == null) return GameState.None;

            // Calculate the map coordinates of the player's new position after moving
            _currentPlayerPosition = (
                _currentPlayerPosition.Item1,
                _currentPlayerPosition.Item2 > 0 ? _currentPlayerPosition.Item2 - 1 : 0);

            // Tell IMap to set the character at the new position
            _map.SetPlayerPosition(_currentPlayerPosition);
            _domainModel.MapObject = _map.ObjectAtPosition;

            // Ask the map what kind of object is at the player's position
            GameState state = _map.GetState();
            return state;
        }

        public GameState MoveRight()
        {
            if (_map == null || _domainModel == null) return GameState.None;

            // Calculate the map coordinates of the player's new position after moving
            _currentPlayerPosition = (
                _currentPlayerPosition.Item1,
                _currentPlayerPosition.Item2 < _mapWidth - 1 ? _currentPlayerPosition.Item2 + 1 : _mapWidth - 1);

            // Tell IMap to set the character at the new position
            _map.SetPlayerPosition(_currentPlayerPosition);
            _domainModel.MapObject = _map.ObjectAtPosition;

            // Ask the map what kind of object is at the player's position
            GameState state = _map.GetState();
            return state;
        }

        public void DisplayMap()
        {
            _map?.Display();
        }
    }
}
