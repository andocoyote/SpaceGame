using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;

namespace SpaceGame.Home
{
    internal class HomeLoop : IScenario
    {
        private ILogger _logger;
        private DomainModel _domainModel;

        public HomeLoop(
            DomainModel domainModel,
            ILogger logger)
        {
            _domainModel = domainModel;
            _logger = logger;
        }

        public DomainModel Run()
        {
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
                        Console.WriteLine("TODO: View inventory.");
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

            Console.WriteLine("Please choose from the following options:");
            Console.WriteLine("1. Fire-up the ship and launch to orbit");
            Console.WriteLine("2. View inventory");
            Console.WriteLine("0. Exit the game");
            Console.WriteLine("Enter your choice: ");

            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine($"Invalid selction. Please try again :");
            }

            return choice;
        }
    }
}