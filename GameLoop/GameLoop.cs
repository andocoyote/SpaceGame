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
            _domainModel.State = State.EmtpySpace;
            bool runGameLoop = true;

            SplashPage.DisplaySplashPage();

            Thread.Sleep(5000);

            // Examine the Domain Model State property and load the corresponding scenario
            while (runGameLoop)
            {
                switch (_domainModel.State)
                {
                    case State.None:
                    case State.EmtpySpace:
                        _domainModel = _spaceLoop.Run();
                        break;

                    case State.InitiateLanding:
                        _domainModel = _landerLoop.Run();
                        break;

                    case State.OverPlanet:
                        _domainModel = _spaceLoop.Run();
                        break;

                    case State.ExitGame:
                        runGameLoop = false;
                        break;

                    default:
                        Console.WriteLine($"Unhandled State: {_domainModel.State}. Exiting game loop.");
                        runGameLoop = false;
                        break;

                }
            }
        }
    }
}
