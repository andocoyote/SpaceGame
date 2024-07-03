using SpaceGame.Art;
using SpaceGame.Interfaces;
using SpaceGame.Logger;
using SpaceGame.Models;
using SpaceGame.Navigation;
using SpaceGame.SpaceLoop;

namespace SpaceGame.GameLoop
{
    internal class GameLoop
    {
        private ISpaceLoop _spaceLoop;
        private IScenario _landerLoop;
        private ILogger _logger;
        private DomainModel _domainModel;

        public GameLoop(
            ISpaceLoop spaceLoop,
            IScenario landerLoop,
            DomainModel domainModel,
            ILogger logger)
        {
            _spaceLoop = spaceLoop;
            _landerLoop = landerLoop;
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
