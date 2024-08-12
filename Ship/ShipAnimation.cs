using Microsoft.Extensions.Options;
using SpaceGame.Interfaces;
using SpaceGame.Models;
using SpaceGame.Screen;

namespace SpaceGame.Ship
{
    internal class ShipAnimation : IAnimation
    {
        private int _animationHeight = 0;
        private int _animationWidth = 0;
        private const char SHIP_CHARACTER = '^';
        private char _characterBehindShip = ' ';
        private int _shipColumn;

        private List<string> _terrains = new List<string>()
        {
            @"______________                                                                    __________________",
            @"|     |      |_______                                                             |                |",
            @"|     |      |      |                                                   __________|                |",
            @"|     |      |      |_________                                          |         |       _____    |",
            @"|     |      |      |        |__________________________________________|         |       |   |    |"
        };

        private char[,]? _animation = null;
        private string[] _animationText { get; set; } = new string[50];
        private DomainModel _domainModel;
        private IScreen _screen;
        private IOptions<ScreenOptions>? _screenOptions;

        public (int, int) Position { get; set; }
        public int RowCount { get; private set; }

        public ShipAnimation(
            DomainModel domainModel,
            IScreen screen,
            IOptions<ScreenOptions>? screenOptions)
        {
            _domainModel = domainModel;
            _screen = screen;
            _screenOptions = screenOptions;

            if (_screenOptions != null)
            {
                _animationHeight = _screenOptions.Value.GraphicsHeight;
                _animationWidth = _screenOptions.Value.GraphicsWidth;
                _shipColumn = _animationWidth / 2;
                RowCount = _animationHeight;
            }

            _animation = new char[_animationHeight, _animationWidth];
        }
        public void Build()
        {
            int row;

            // TODO: Use IsInitialized property instead
            if (_animation == null) return;

            _screen.AddGraphics(_animation);
            _screen.AddText(_animationText);

            // Initialize the animation:
            //  All rows except the bottom 5 will be empty space chars.
            //  The bottom 5 are terrain and converted from strings to char[]
            for (int i = 0; i < _animationHeight - 5; i++)
            {
                for (int j = 0; j < _animationWidth; j++)
                {
                    _animation[i, j] = ' ';
                }
            }

            // Add the terrain
            for (int i = 0; i < _terrains.Count; i++)
            {
                for (int j = 0; j < _terrains[i].Length; j++)
                {
                    // This weird calculation is because I want the terrains listed in the list in the order
                    // I've specified above so I can easily picture and edit the terrain as desired
                    _animation[_animationHeight - _terrains.Count + i, j] = _terrains[i][j];
                }
            }

            // Place the ship in the animation at the initial location (top or bottom)
            if (_domainModel.ShipModel.ShipState == SpaceGame.Models.ShipState.Docked)
            {
                row = 0;
            }
            else
            {
                row = _animationHeight - 1;
            }

            MoveToRow(row);
        }

        public void MoveToRow(int row)
        {
            if (_animation == null) return;

            // Set the row to 0 or length-1, as needed
            if (row < 0)
            {
                row = 0;
            }
            else if (row >= _animationHeight - 1)
            {
                row = _animationHeight - 1;
            }

            // Put the character that was behind the ship back
            _animation[Position.Item1, Position.Item2] = _characterBehindShip;

            // Calculate the new position
            Position = (row, _shipColumn);

            // Capture the ASCII character that will be replaced by the lander so we can put it back
            _characterBehindShip = _animation[row, _shipColumn];

            // Move the ship to the new position
            _animation[Position.Item1, Position.Item2] = SHIP_CHARACTER;

            // Update the text
            List<string> ShipModel = new List<string>()
                {
                    $"Velocity: {_domainModel.ShipModel.Velocity}",
                    $"Altitude: {_domainModel.ShipModel.Altitude}",
                    $"Starting Altitude: {_domainModel.ShipModel.StartingAltitude}",
                    $"Target Altitude: {_domainModel.ShipModel.TargetAltitude}",
                    $"Distance from Target: {_domainModel.ShipModel.DistanceFromTarget}",
                    $"Total Fuel: {_domainModel.ShipModel.TotalFuel}",
                    $"Fuel Flow Rate: {_domainModel.ShipModel.FuelFlowRate}",
                    $"Maximum Fuel Consumption Rate: {_domainModel.ShipModel.MaxFuelRate}",
                    $"Maximum Engine Thrust: {_domainModel.ShipModel.MaxThrust}",
                    $"Lander Mass: {_domainModel.ShipModel.LanderMass}"
                };

            for (int i = 0; i < ShipModel.Count; i++)
            {
                _animationText[i] = ShipModel[i];
            }
        }

        public void Display()
        {
            _screen.Display();
        }
    }
}
