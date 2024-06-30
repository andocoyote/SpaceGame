using SpaceGame.Lander;
using SpaceGame.Logger;
using SpaceGame.Navigation;
using System.Runtime.ExceptionServices;

namespace SpaceGame.GameLoop
{
    internal class GameLoop : IGameLoop
    {
        private INavigation _navigation;
        private ILanderLoop _landerLoop;
        private ILogger _logger;
        private State _currentState;

        public GameLoop(
            INavigation navigation,
            ILanderLoop landerLoop,
            ILogger logger)
        {
            _navigation = navigation;
            _landerLoop = landerLoop;
            _logger = logger;
        }

        public void Run()
        {
            bool runGameLoop = true;

            while (runGameLoop)
            {
                Console.WriteLine("Use arrow keys to fly.");
                var keyInfo = Console.ReadKey(intercept: true); // Read a key without displaying it

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        runGameLoop = false;
                        break;

                    case ConsoleKey.UpArrow:
                        _currentState = _navigation.MoveUp();
                        ProcessCurrentState();
                        break;

                    case ConsoleKey.DownArrow:
                        _currentState = _navigation.MoveDown();
                        ProcessCurrentState();
                        break;

                    case ConsoleKey.LeftArrow:
                        _currentState = _navigation.MoveLeft();
                        ProcessCurrentState();
                        break;

                    case ConsoleKey.RightArrow:
                        _currentState = _navigation.MoveRight();
                        ProcessCurrentState();
                        break;

                    default:
                        Console.WriteLine($"Invalid command entered: {keyInfo.KeyChar}");
                        break;
                }
            }
        }

        private void ProcessCurrentState()
        {
            _logger.DebugPrintLine($"Current State: {_currentState}");

            char selection;

            switch ( _currentState )
            {
                case State.EmtpySpace:
                    break;
                case State.OverPlanet:
                    Console.Write("You are over a planet. Want to descend (y/n) :");
                    
                    while (!char.TryParse(Console.ReadLine(), out selection))
                    {

                    }

                    if (selection == 'y')
                    {
                        _landerLoop.Run();
                    }
                    
                    break;
                default:
                    break;
            }
        }
    }
}
