using SpaceGame.Models;
using SpaceGame.Screen;

namespace SpaceGame.Lander
{
    internal class LanderAnimation : ILanderAnimation
    {
        private const int ANIMATION_HEIGHT = 50;
        private const int ANIMATION_WIDTH = 100;
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
        private DomainModel _domainModel;
        private IScreen _screen;

        public (int, int) Position { get; set; }
        public string[] AnimationText { get; set; } = new string[50];
        public int RowCount { get; private set; }

        public LanderAnimation(
            DomainModel domainModel,
            IScreen screen)
        {
            _domainModel = domainModel;
            _screen = screen;
            _landerColumn = ANIMATION_WIDTH / 2;
            RowCount = ANIMATION_HEIGHT;

            _animation = new char[ANIMATION_HEIGHT, ANIMATION_WIDTH];
        }
        public void Build()
        {
            int row;

            // TODO: Use IsInitialized property instead
            if (_animation == null) return;

            _screen.AddGraphics(_animation);
            _screen.AddText(AnimationText);

            // Initialize the animation:
            //  All rows except the bottom 5 will be empty space chars.
            //  The bottom 5 are terrain and converted from strings to char[]
            for (int i = 0; i < ANIMATION_HEIGHT - 5; i++)
            {
                for (int j = 0; j < ANIMATION_WIDTH; j++)
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
                    _animation[ANIMATION_HEIGHT - _terrains.Count + i, j] = _terrains[i][j];
                }
            }

            // Place the lander in the animation at the initial location (top or bottom)
            if (_domainModel.LanderProperties.LanderState == LanderState.Docked)
            {
                row = 0;
            }
            else
            {
                row = ANIMATION_HEIGHT - 1;
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
            else if (row >= ANIMATION_WIDTH - 1)
            {
                row = ANIMATION_WIDTH - 1;
            }

            // Put the character that was behind the lander back
            _animation[Position.Item1, Position.Item2] = _characterBehindLander;

            // Calculate the new position
            Position = (row, _landerColumn);

            // Capture the ASCII character that will be replaced by the lander so we can put it back
            _characterBehindLander = _animation[row, _landerColumn];

            // Move the lander to the new position
            _animation[Position.Item1, Position.Item2] = LANDER_CHARACTER;
        }

        public void Display()
        {
            _screen.Display();
        }
    }
}
