using Microsoft.Extensions.DependencyInjection;
using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;

namespace SpaceGame.Home
{
    internal class HomeLoop : IScenario
    {
        private IScenario? _inventoryLoop;
        private DomainModelService.DomainModelService _domainModelService;
        private ILogger _logger;
        private DomainModel _domainModel;

        public HomeLoop(
            [FromKeyedServices("Inventory")] IScenario? inventoryLoop,
            DomainModelService.DomainModelService domainModelService,
            DomainModel domainModel,
            ILogger logger)
        {
            _inventoryLoop = inventoryLoop;
            _domainModelService = domainModelService;
            _domainModel = domainModel;
            _logger = logger;
        }

        public DomainModel Run()
        {
            if (_inventoryLoop == null) return _domainModel;

            int selection;
            bool exit = false;

            Console.Clear();

            do
            {
                selection = UserMenu();

                switch (selection)
                {
                    case 0:
                        _domainModel.GameState = GameState.ExitGame;
                        exit = true;
                        break;

                    case 1:
                        _domainModel.GameState = GameState.ShipScenario;
                        exit = true;
                        break;

                    case 2:
                        _inventoryLoop.Run();
                        break;

                    case 3:
                        Console.WriteLine("Saving game ...");
                        _domainModelService.SaveDomainModelAsync().GetAwaiter().GetResult();
                        bool modelExists = _domainModelService.ModelExists(_domainModel.PlayerName);

                        if (modelExists)
                        {
                            Console.WriteLine($"Saved game for {_domainModel.PlayerName}.\n");
                        }
                        else
                        {
                            Console.WriteLine($"Error saving game for {_domainModel.PlayerName}.\n");
                        }

                        break;

                    default:
                        break;
                }
            } while (!exit);

            return _domainModel;
        }

        private int UserMenu()
        {
            int choice;

            Console.WriteLine("You are currently on your home planet.");
            Console.WriteLine("This is where you can fuel-up your ship, and buy, sell, and drop items.");
            Console.WriteLine("This is also the only location where you can save your progress.");
            Console.WriteLine("If you die or quit somewhere else in the Universe, your progress will not be saved.");
            Console.WriteLine();

            Console.WriteLine("Please choose from the following options:");
            Console.WriteLine("1. Fire-up the ship and launch to orbit");
            Console.WriteLine("2. Access inventory");
            Console.WriteLine("3. Save the game");
            Console.WriteLine("0. Exit the game");
            Console.WriteLine("Enter your choice: ");

            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.Write($"Invalid selction. Please try again: ");
            }

            return choice;
        }
    }
}