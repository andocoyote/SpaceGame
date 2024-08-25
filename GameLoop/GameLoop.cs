using Microsoft.Extensions.DependencyInjection;
using SpaceGame.Art;
using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;

namespace SpaceGame.GameLoop
{
    // Main Game Loop object
    internal class GameLoop
    {
        private IScenario? _startLoop;
        private IScenario? _homeLoop;
        private IScenario? _shipLoop;
        private IScenario? _spaceLoop;
        private IScenario? _landerLoop;
        private IScenario? _planetLoop;
        private ILogger _logger;
        private DomainModel _domainModel;

        // GameLoop will run scenarios based on properties stored in the DomainModel
        public GameLoop(
            [FromKeyedServices("Start")] IScenario? startLoop,
            [FromKeyedServices("Home")] IScenario? homeLoop,
            [FromKeyedServices("Ship")] IScenario? shipLoop,
            IScenario? spaceLoop,
            [FromKeyedServices("Lander")] IScenario? landerLoop,
            IScenario? planetloop,
            DomainModel domainModel,
            ILogger logger)
        {
            _startLoop = startLoop;
            _homeLoop = homeLoop;
            _shipLoop = shipLoop;
            _spaceLoop = spaceLoop;
            _landerLoop = landerLoop;
            _planetLoop = planetloop;
            _domainModel = domainModel;
            _logger = logger;
        }

        // Run a scenario for the Domain Model State property
        public void Run()
        {
            if (_startLoop == null ||
                _homeLoop == null ||
                _shipLoop == null ||
                _spaceLoop == null ||
                _landerLoop == null ||
                _planetLoop == null)
            {
                return;
            }

            _domainModel.GameState = GameState.StartGame;
            bool runGameLoop = true;

            SplashPage.DisplaySplashPage();

            Thread.Sleep(5000);

            // Examine the Domain Model State property and load the corresponding scenario
            while (runGameLoop)
            {
                DumpDomainModel();

                switch (_domainModel.GameState)
                {
                    case GameState.None:
                    case GameState.StartGame:
                        _domainModel = _startLoop.Run();
                        break;

                    case GameState.HomeScenario:
                        _domainModel = _homeLoop.Run();
                        break;

                    case GameState.ShipScenario:
                        _domainModel = _shipLoop.Run();
                        break;

                    case GameState.SpaceScenario:
                        _domainModel = _spaceLoop.Run();
                        break;

                    case GameState.LanderScenario:
                        _domainModel = _landerLoop.Run();
                        break;

                    case GameState.PlanetScenario:
                        _domainModel = _planetLoop.Run();
                        break;

                    case GameState.LanderCrashed:
                    case GameState.ShipCrashed:
                        Console.WriteLine("You've crashed the lander.");
                        runGameLoop = false;
                        break;

                    case GameState.ExitGame:
                        Console.WriteLine("See you at the next mission.");
                        runGameLoop = false;
                        break;

                    default:
                        Console.WriteLine($"Unhandled State: {_domainModel.GameState}. Exiting game loop.");
                        runGameLoop = false;
                        break;

                }
            }
        }

        private void DumpDomainModel()
        {
            _logger.DebugPrintLine("Printing GameState:");
            _logger.DebugPrintLine($"\tGameState: {_domainModel.GameState}");
            _logger.DebugPrintLine("Printing Lander Properties:");
            _logger.DebugPrintLine($"\tLanderState: {_domainModel.LanderModel.LanderState}");
            _logger.DebugPrintLine($"\tAltitude: {_domainModel.LanderModel.Altitude}");
            _logger.DebugPrintLine($"\tTotalFuel: {_domainModel.LanderModel.TotalFuel}");
            _logger.DebugPrintLine($"\tFuelFlowRate: {_domainModel.LanderModel.FuelFlowRate}");
            _logger.DebugPrintLine($"\tMaxFuelRate: {_domainModel.LanderModel.MaxFuelRate}");
            _logger.DebugPrintLine($"\tLanderMass: {_domainModel.LanderModel.LanderMass}");
            _logger.DebugPrintLine($"\tMaxThrust: {_domainModel.LanderModel.MaxThrust}");
            _logger.DebugPrintLine($"\tFreeFallAcceleration: {_domainModel.LanderModel.FreeFallAcceleration}");
        }
    }
}
