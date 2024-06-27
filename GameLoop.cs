namespace SpaceGame
{
    internal class GameLoop : IGameLoop
    {
        private INavigation _navigation;
        public GameLoop(INavigation navigation)
        {
            _navigation = navigation;
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
                        _navigation.MoveUp();
                        break;

                    case ConsoleKey.DownArrow:
                        _navigation.MoveDown();
                        break;

                    case ConsoleKey.LeftArrow:
                        _navigation.MoveLeft();
                        break;

                    case ConsoleKey.RightArrow:
                        _navigation.MoveRight();
                        break;

                    default:
                        Console.WriteLine($"Invalid command entered: {keyInfo.KeyChar}");
                        break;
                }
            }
        }
    }
}
