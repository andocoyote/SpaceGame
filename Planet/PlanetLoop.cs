using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;

namespace SpaceGame.Planet
{
    internal class PlanetLoop : IScenario
    {
        private INavigation _navigation;
        private ILogger _logger;
        private DomainModel _domainModel;

        public PlanetLoop(
            INavigation navigation,
            DomainModel domainModel,
            ILogger logger)
        {
            _navigation = navigation;
            _domainModel = domainModel;
            _logger = logger;
        }

        // Move the player around the planet map and update the display as the user moves
        public DomainModel Run()
        {
            bool runPlanetLoop = ProcessCurrentState();

            while (runPlanetLoop)
            {
                Console.WriteLine("Use arrow keys to driver around.");
                var keyInfo = Console.ReadKey(intercept: true); // Read a key without displaying it

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        _domainModel.GameState = _navigation.MoveUp();
                        runPlanetLoop = ProcessCurrentState();
                        break;

                    case ConsoleKey.DownArrow:
                        _domainModel.GameState = _navigation.MoveDown();
                        runPlanetLoop = ProcessCurrentState();
                        break;

                    case ConsoleKey.LeftArrow:
                        _domainModel.GameState = _navigation.MoveLeft();
                        runPlanetLoop = ProcessCurrentState();
                        break;

                    case ConsoleKey.RightArrow:
                        _domainModel.GameState = _navigation.MoveRight();
                        runPlanetLoop = ProcessCurrentState();
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
            bool continueScenario = true;
            _logger.DebugPrintLine($"Current State: {_domainModel.GameState}");

            char selection;

            switch (_domainModel.GameState)
            {
                case GameState.EmtpyLand:
                    _navigation.DisplayMap();
                    break;

                case GameState.OnLandingZone:
                    _navigation.DisplayMap();
                    Console.Write("You are on the landing zone. Want to ascend? (y/n) :");

                    while (!char.TryParse(Console.ReadLine(), out selection))
                    {

                    }

                    if (selection == 'y')
                    {
                        _domainModel.GameState = GameState.InitiateDocking;
                        continueScenario = false;
                    }

                    break;

                default:
                    break;
            }

            return continueScenario;
        }
    }
}
