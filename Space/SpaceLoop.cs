﻿using SpaceGame.BaseClasses;
using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;

namespace SpaceGame.Space
{
    internal class SpaceLoop : IScenario
    {
        private INavigation _navigation;
        private ILogger _logger;
        private DomainModel _domainModel;

        public SpaceLoop(
            INavigation navigation,
            DomainModel domainModel,
            ILogger logger)
        {
            _navigation = navigation;
            _domainModel = domainModel;
            _logger = logger;
        }

        // Move the player around the space map and update the display as the user moves
        public DomainModel Run()
        {
            bool runSpaceLoop = ProcessCurrentState();

            while (runSpaceLoop)
            {
                Console.WriteLine("Use arrow keys to fly (ESC to quit).");
                var keyInfo = Console.ReadKey(intercept: true); // Read a key without displaying it

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        _domainModel.GameState = GameState.ExitGame;
                        runSpaceLoop = false;
                        break;

                    case ConsoleKey.UpArrow:
                        _domainModel.GameState = _navigation.MoveUp();
                        runSpaceLoop = ProcessCurrentState();
                        break;

                    case ConsoleKey.DownArrow:
                        _domainModel.GameState = _navigation.MoveDown();
                        runSpaceLoop = ProcessCurrentState();
                        break;

                    case ConsoleKey.LeftArrow:
                        _domainModel.GameState = _navigation.MoveLeft();
                        runSpaceLoop = ProcessCurrentState();
                        break;

                    case ConsoleKey.RightArrow:
                        _domainModel.GameState = _navigation.MoveRight();
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
            bool continueScenario = true;
            _logger.DebugPrintLine($"Current State: {_domainModel.GameState}");

            char selection;

            switch (_domainModel.SpaceMapModel.SpaceMapState)
            {
                case SpaceMapState { } state when state == SpaceMapState.EmtpySpace:
                    _navigation.DisplayMap();
                    break;

                case SpaceMapState { } state when state == SpaceMapState.OverPlanet:
                    _navigation.DisplayMap();
                    Console.Write($"You are over {_domainModel?.MapObject?.Label}. Want to descend? (y/n) :");
                    
                    while (!char.TryParse(Console.ReadLine(), out selection))
                    {

                    }

                    if (selection == 'y')
                    {
                        if (_domainModel != null)
                        {
                            _domainModel.GameState = GameState.LanderScenario;
                            _domainModel.PlanetMapModel.PlanetMapState = PlanetMapState.InitiatePlanetLanding;
                        }
                        
                        continueScenario = false;
                    }
                    
                    break;

                case SpaceMapState { } state when state == SpaceMapState.OverHomePlanet:
                    _navigation.DisplayMap();
                    MapObject? homePlanet = _domainModel?.SpaceMapModel?.ObjectAtPosition;

                    Console.Write($"You are over {homePlanet?.Label}. Want to descend? (y/n) :");

                    while (!char.TryParse(Console.ReadLine(), out selection))
                    {

                    }

                    if (selection == 'y')
                    {
                        Console.WriteLine("Refuel, etc. on home planet.");
                        if (_domainModel != null)
                        {
                            _domainModel.GameState = GameState.ShipScenario;
                            _domainModel.PlanetMapModel.PlanetMapState = PlanetMapState.InitiateHomePlanetLanding;
                        }

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
