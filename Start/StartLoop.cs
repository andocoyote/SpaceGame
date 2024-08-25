using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;
using System.Xml.Linq;

namespace SpaceGame.Start
{
    internal class StartLoop : IScenario
    {
        private DomainModelService.DomainModelService _domainModelService;
        private ILogger _logger;
        private DomainModel _domainModel;
        private string? _playerName = string.Empty;

        public StartLoop(
            DomainModelService.DomainModelService domainModelService,
            DomainModel domainModel,
            ILogger logger)
        {
            _domainModelService = domainModelService;
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
                        exit = StartNewGame();

                        if (!string.IsNullOrEmpty(_playerName))
                        {
                            _domainModel.PlayerName = _playerName;
                            _domainModel.GameState = GameState.HomeScenario;
                            _domainModelService.SaveDomainModelAsync().GetAwaiter().GetResult();
                        }

                        break;

                    case 2:
                        exit = LoadExistingGame();
                        _domainModel.GameState = GameState.HomeScenario;
                        break;

                    case 3:
                        DeleteGame();
                        break;

                    case 4:
                        ListAllGames();
                        break;

                    default:
                        break;
                }
            } while (!exit);

            return _domainModel;
        }

        private bool StartNewGame()
        {
            bool exitStartLoop = false;

            // Get the player name from the user: assigns to _playerName
            GetPlayerName();

            // Check that the game does not already exist for this player
            if (!string.IsNullOrEmpty(_playerName))
            {
                bool modelExists = _domainModelService.ModelExists(_playerName);

                if (modelExists)
                {
                    Console.WriteLine($"A game already exists for {_playerName}.\n");
                }
                else
                {
                    exitStartLoop = true;
                }
            }

            return exitStartLoop;
        }

        private bool LoadExistingGame()
        {
            bool exitStartLoop = false;
            bool gotPlayerName = false;

            // Get the player name from the user: assigns to _playerName
            gotPlayerName = GetPlayerName();

            if (gotPlayerName && !string.IsNullOrEmpty(_playerName))
            {
                Console.WriteLine($"Loading game for {_playerName} ...");

                exitStartLoop = _domainModelService.LoadDomainModelAsync(_playerName).GetAwaiter().GetResult();

                if (!exitStartLoop )
                {
                    Console.WriteLine($"No game exists for {_playerName}.\n");
                }
            }

            return exitStartLoop;
        }

        private void DeleteGame()
        {
            bool gotPlayerName = false;
            bool deletedGame = false;

            // Get the player name from the user: assigns to _playerName

            gotPlayerName = GetPlayerName();

            if (gotPlayerName && !string.IsNullOrEmpty(_playerName))
            {
                Console.WriteLine($"Deleting game for {_playerName} ...");

                deletedGame = _domainModelService.DeleteSavedModel(_playerName);

                if (deletedGame)
                {
                    Console.WriteLine($"Game for {_playerName} has been deleted.\n");
                }
                else
                {
                    Console.WriteLine($"No game for {_playerName} exists.\n");
                }
            }
        }

        private void ListAllGames()
        {
            var savedGames = _domainModelService.GetAllGames();

            if (savedGames != null && savedGames.Count > 0)
            {
                Console.WriteLine("Games exist for the following players:");

                foreach (string name in savedGames)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("No saved games exist.\n");
            }
        }

        // Returns boolean indicating if we got the player name or not
        private bool GetPlayerName()
        {
            bool gotPlayerName = false;
            bool exitLoop = false;
            string? name = string.Empty;
            char selection;

            do
            {
                // Ask the user for the name of their character
                // and keep trying until they get it right
                Console.Write("Please enter the name of your space traveller (<Enter> to go back): ");
                name = Console.ReadLine();
                name = name?.Trim();

                // If name is null or empty, the player wants to go back
                if (string.IsNullOrEmpty(name))
                {
                    exitLoop = true;
                }
                else
                {
                    Console.WriteLine($"You entered: {name}");
                    Console.Write("Is this correct? (y/n): ");

                    while (!char.TryParse(Console.ReadLine(), out selection))
                    {

                    }

                    // If they entered the character name they want, move forward with the game
                    // Else, ask them to enter the character's name again
                    if (selection == 'y')
                    {
                        _playerName = name;
                        exitLoop = true;
                        gotPlayerName = true;
                    }
                    else
                    {
                        name = string.Empty;
                    }
                }
            } while (!exitLoop);

            return gotPlayerName;
        }

        private int UserMenu()
        {
            int choice;

            Console.WriteLine("Please choose from the following options:");
            Console.WriteLine("1. Start a new game");
            Console.WriteLine("2. Load an existing game");
            Console.WriteLine("3. Delete a game");
            Console.WriteLine("4. List all games");
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
