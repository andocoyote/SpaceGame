namespace SpaceGame
{
    internal class GameLoop : IGameLoop
    {
        private INavigation _navigation;
        private ILogger _logger;
        private State _currentState;

        public GameLoop(INavigation navigation, ILogger logger)
        {
            _navigation = navigation;
            _logger = logger;   
        }

        public void Run()
        {
            bool runGameLoop = true;

            while (runGameLoop)
            {
                var keyInfo = Console.ReadKey(intercept: true); // Read a key without displaying it

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        runGameLoop = false;
                        break;

                    case ConsoleKey.UpArrow:
                        _currentState = _navigation.MoveUp();
                        _logger.DebugPrintLine($"Current State: {_currentState}");
                        break;

                    case ConsoleKey.DownArrow:
                        _currentState = _navigation.MoveDown();
                        _logger.DebugPrintLine($"Current State: {_currentState}");
                        break;

                    case ConsoleKey.LeftArrow:
                        _currentState = _navigation.MoveLeft();
                        _logger.DebugPrintLine($"Current State: {_currentState}");
                        break;

                    case ConsoleKey.RightArrow:
                        _currentState = _navigation.MoveRight();
                        _logger.DebugPrintLine($"Current State: {_currentState}");
                        break;

                    default:
                        Console.WriteLine($"Invalid command entered: {keyInfo.KeyChar}");
                        break;
                }
            }
        }
    }
}
