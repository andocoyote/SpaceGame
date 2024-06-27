namespace SpaceGame
{
    internal class Navigation : INavigation
    {
        private IMap _map;
        private readonly int _mapHeight = 50;
        private readonly int _mapWidth = 100;
        private (int, int) _currentShipPosition = (0, 0);

        public Navigation(IMap map)
        {
            _map = map;

            Random rand = new Random();

            // Build the map and assign the ship to a random location on the map
            _currentShipPosition = (rand.Next(_mapHeight - 1), rand.Next(_mapWidth - 1));
            _map.Build(_mapHeight, _mapWidth);
            _map.ShipPosition = _currentShipPosition;
            _map.Display();
        }

        public State MoveUp()
        {
            _currentShipPosition = (
                _currentShipPosition.Item1 > 0 ? _currentShipPosition.Item1 - 1 : 0,
                _currentShipPosition.Item2);

            _map.ShipPosition = _currentShipPosition;
            _map.Display();

            State state = _map.GetState();
            return state;
        }

        public State MoveDown()
        {
            _currentShipPosition = (
                _currentShipPosition.Item1 < _mapHeight - 1 ? _currentShipPosition.Item1 + 1 : _mapHeight - 1,
                _currentShipPosition.Item2);

            _map.ShipPosition = _currentShipPosition;
            _map.Display();

            State state = _map.GetState();
            return state;
        }

        public State MoveLeft()
        {
            _currentShipPosition = (
                _currentShipPosition.Item1,
                _currentShipPosition.Item2 > 0 ? _currentShipPosition.Item2 - 1 : 0);

            _map.ShipPosition = _currentShipPosition;
            _map.Display();

            State state = _map.GetState();
            return state;
        }

        public State MoveRight()
        {
            _currentShipPosition = (
                _currentShipPosition.Item1,
                _currentShipPosition.Item2 < _mapWidth - 1 ? _currentShipPosition.Item2 + 1 : _mapWidth - 1);

            _map.ShipPosition = _currentShipPosition;
            _map.Display();

            State state = _map.GetState();
            return state;
        }
    }
}
