using SpaceGame.Interfaces;
using SpaceGame.Logger;
using SpaceGame.Models;
using SpaceGame.Navigation;

namespace SpaceGame.SpaceLoop
{
    internal class SpaceLoop : ISpaceLoop
    {
        private INavigation _navigation;
        private IScenario _landerLoop;
        private ILogger _logger;
        private State _currentState;
        private DomainModel _domainModel;

        public SpaceLoop(
            INavigation navigation,
            IScenario landerLoop,
            DomainModel domainModel,
            ILogger logger)
        {
            _navigation = navigation;
            _landerLoop = landerLoop;
            _domainModel = domainModel;
            _logger = logger;
        }

        public DomainModel Run()
        {
            bool runSpaceLoop = true;

            _navigation.DisplayMap();

            while (runSpaceLoop)
            {
                Console.WriteLine("Use arrow keys to fly (ESC to quit).");
                var keyInfo = Console.ReadKey(intercept: true); // Read a key without displaying it

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        _domainModel.State = State.ExitGame;
                        runSpaceLoop = false;
                        break;

                    case ConsoleKey.UpArrow:
                        _currentState = _navigation.MoveUp();
                        _navigation.DisplayMap();
                        runSpaceLoop = ProcessCurrentState();
                        break;

                    case ConsoleKey.DownArrow:
                        _currentState = _navigation.MoveDown();
                        _navigation.DisplayMap();
                        runSpaceLoop = ProcessCurrentState();
                        break;

                    case ConsoleKey.LeftArrow:
                        _currentState = _navigation.MoveLeft();
                        _navigation.DisplayMap();
                        runSpaceLoop = ProcessCurrentState();
                        break;

                    case ConsoleKey.RightArrow:
                        _currentState = _navigation.MoveRight();
                        _navigation.DisplayMap();
                        runSpaceLoop = ProcessCurrentState();
                        break;

                    default:
                        Console.WriteLine($"Invalid command entered: {keyInfo.KeyChar}");
                        break;
                }
            }

            return _domainModel;
        }

        private bool ProcessCurrentState()
        {
            bool continueScenario = false;
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
                        _domainModel.State = State.InitiateLanding;
                        continueScenario = false;
                        //_landerLoop.Run();
                    }

                    //_navigation.DisplayMap();
                    
                    break;

                default:
                    break;
            }

            return continueScenario;
        }
    }
}
