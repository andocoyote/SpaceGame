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
        private INavigation _navigation;
        private IScenario _landerLoop;
        private ILogger _logger;
        private DomainModel _domainModel;
        private State _currentState;

        public GameLoop(
            ISpaceLoop spaceLoop,
            INavigation navigation,
            IScenario landerLoop,
            DomainModel domainModel,
            ILogger logger)
        {
            _spaceLoop = spaceLoop;
            _navigation = navigation;
            _landerLoop = landerLoop;
            _domainModel = domainModel;
            _logger = logger;
        }

        public void Run()
        {
            _domainModel.State = State.EmtpySpace;

            bool runGameLoop = true;

            while (runGameLoop)
            {
                _domainModel = _spaceLoop.Run();

                if (_domainModel.State == State.ExitGame)
                {
                    break;
                }

                if (_domainModel.State == State.InitiateLanding)
                {
                    _domainModel = _landerLoop.Run();
                }

                if (_domainModel.State == State.OverPlanet)
                {
                    _domainModel = _spaceLoop.Run();
                }
            }
        }

        private void ProcessCurrentState()
        {
            _logger.DebugPrintLine($"Current State: {_currentState}");

            char selection;

            switch (_currentState)
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

                    _navigation.DisplayMap();

                    break;

                default:
                    break;
            }
        }
    }
}
