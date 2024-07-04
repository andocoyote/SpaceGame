using Microsoft.Extensions.DependencyInjection;
using SpaceGame.Art;
using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;
using SpaceGame.Navigation;
using SpaceGame.Space;

namespace SpaceGame.GameLoop
{
    internal class GameLoop
    {
        private IScenario _spaceLoop;
        private IScenario _landerLoop;
        private IScenario _vehicleLoop;
        private ILogger _logger;
        private DomainModel _domainModel;

        public GameLoop(
            [FromKeyedServices("Space")] IScenario spaceLoop,
            [FromKeyedServices("Lander")] IScenario landerLoop,
            [FromKeyedServices("Vehicle")] IScenario vehicleloop,
            DomainModel domainModel,
            ILogger logger)
        {
            _spaceLoop = spaceLoop;
            _landerLoop = landerLoop;
            _vehicleLoop = vehicleloop;
            _domainModel = domainModel;
            _logger = logger;
        }

        public void Run()
        {
            _domainModel.GameState = GameState.EmtpySpace;
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
                    case GameState.EmtpySpace:
                        _domainModel = _spaceLoop.Run();
                        break;

                    case GameState.InitiateLanding:
                        _domainModel = _landerLoop.Run();
                        break;

                    case GameState.OverPlanet:
                        _domainModel = _spaceLoop.Run();
                        break;

                    case GameState.ExitGame:
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
            _logger.DebugPrintLine($"\tLanderState: {_domainModel.LanderProperties.LanderState}");
            _logger.DebugPrintLine($"\tAltitude: {_domainModel.LanderProperties.Altitude}");
            _logger.DebugPrintLine($"\tTotalFuel: {_domainModel.LanderProperties.TotalFuel}");
            _logger.DebugPrintLine($"\tFuelFlowRate: {_domainModel.LanderProperties.FuelFlowRate}");
            _logger.DebugPrintLine($"\tMaxFuelRate: {_domainModel.LanderProperties.MaxFuelRate}");
            _logger.DebugPrintLine($"\tLanderMass: {_domainModel.LanderProperties.LanderMass}");
            _logger.DebugPrintLine($"\tMaxThrust: {_domainModel.LanderProperties.MaxThrust}");
            _logger.DebugPrintLine($"\tFreeFallAcceleration: {_domainModel.LanderProperties.FreeFallAcceleration}");
        }
    }
}
