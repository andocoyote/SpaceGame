using SpaceGame.Models;
using System.Text;

namespace SpaceGame.Lander
{
    internal class LanderAnimation : ILanderAnimation
    {
        public (int, int) Position { get; set; }
        public int RowCount { get; private set; }

        private StringBuilder[] _animation =
            {
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"                                                                        "),
                new StringBuilder(@"_____                                                      _____________"),
                new StringBuilder(@"     \__                                                  /             "),
                new StringBuilder(@"        |                                             ___/              "),
                new StringBuilder(@"         \_______                                    /                  "),
                new StringBuilder(@"                 \__________________________________|                   "),
            };

        private const char LANDER_CHARACTER = '^';
        private char _characterBehindLander;
        private int _landerColumn;
        private DomainModel _domainModel;

        public LanderAnimation(DomainModel domainModel)
        {
            _domainModel = domainModel;
            _landerColumn = _animation[0].Length / 2;
            RowCount = _animation.Length;
        }
        public void Build()
        {
            int row;

            // Place the lander in the animation at the initial location (top or bottom)
            if (_domainModel.LanderProperties.LanderState == LanderState.Docked)
            {
                row = 0;
            }
            else
            {
                row = _animation.Length - 1;
            }

            MoveToRow(row);
        }

        public void MoveToRow(int row)
        {
            // Set the row to 0 or length-1, as needed
            if (row < 0)
            {
                row = 0;
            }
            else if (row >= _animation.Length - 1)
            {
                row = _animation.Length - 1;
            }

            // Put the character that was behind the lander back
            _animation[Position.Item1][Position.Item2] = _characterBehindLander;

            // Calculate the new position
            Position = (row, _landerColumn);

            // Capture the ASCII character that will be replaced by the lander so we can put it back
            _characterBehindLander = _animation[row][_landerColumn];

            // Move the lander to the new position
            _animation[Position.Item1][Position.Item2] = LANDER_CHARACTER;
        }

        public void Display()
        {
            foreach (var row in _animation)
            {
                Console.WriteLine(row);
            }
        }
    }
}
