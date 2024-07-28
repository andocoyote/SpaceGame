using Microsoft.Extensions.Options;
using SpaceGame.Interfaces;
using SpaceGame.Models;
using SpaceGame.Screen;

namespace SpaceGame.Lander
{
    internal class LanderAnimation : IAnimation
    {
        private int _animationHeight = 0;
        private int _animationWidth = 0;
        private const char LANDER_CHARACTER = '^';
        private char _characterBehindLander = ' ';
        private int _landerColumn;

        private List<string> _terrains = new List<string>()
        {
            @"_____________                                                                      _________________",
            @"             \_______                                                             /                 ",
            @"                    |                                                   _________/                  ",
            @"                     \_______                                          /                            ",
            @"                             \_________________________________________|                            "
        };

        private char[,]? _animation = null;
        private string[] _animationText { get; set; } = new string[50];
        private DomainModel _domainModel;
        private IScreen _screen;
        private IOptions<ScreenOptions>? _screenOptions;

        public (int, int) Position { get; set; }
        public int RowCount { get; private set; }

        public LanderAnimation(
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
                _landerColumn = _animationWidth / 2;
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

            // Place the lander in the animation at the initial location (top or bottom)
            if (_domainModel.LanderModel.LanderState == LanderState.Docked)
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

            // Put the character that was behind the lander back
            _animation[Position.Item1, Position.Item2] = _characterBehindLander;

            // Calculate the new position
            Position = (row, _landerColumn);

            // Capture the ASCII character that will be replaced by the lander so we can put it back
            _characterBehindLander = _animation[row, _landerColumn];

            // Move the lander to the new position
            _animation[Position.Item1, Position.Item2] = LANDER_CHARACTER;

            // Update the text
            List<string> LanderModel = new List<string>()
                {
                    $"Velocity: {_domainModel.LanderModel.Velocity}",
                    $"Altitude: {_domainModel.LanderModel.Altitude}",
                    $"Starting Altitude: {_domainModel.LanderModel.StartingAltitude}",
                    $"Target Altitude: {_domainModel.LanderModel.TargetAltitude}",
                    $"Distance from Target: {_domainModel.LanderModel.DistanceFromTarget}",
                    $"Total Fuel: {_domainModel.LanderModel.TotalFuel}",
                    $"Fuel Flow Rate: {_domainModel.LanderModel.FuelFlowRate}",
                    $"Maximum Fuel Consumption Rate: {_domainModel.LanderModel.MaxFuelRate}",
                    $"Maximum Engine Thrust: {_domainModel.LanderModel.MaxThrust}",
                    $"Lander Mass: {_domainModel.LanderModel.LanderMass}"
                };

            for (int i = 0; i < LanderModel.Count; i++)
            {
                _animationText[i] = LanderModel[i];
            }
        }

        public void Display()
        {
            _screen.Display();
        }
    }
}
