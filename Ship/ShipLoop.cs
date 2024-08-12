using Microsoft.Extensions.DependencyInjection;
using SpaceGame.BaseClasses;
using SpaceGame.Interfaces;
using SpaceGame.Models;

namespace SpaceGame.Ship
{
    internal class ShipLoop : IScenario
    {
        //ANDREW MIKESELL
        //CS-210-A
        //03-08-01

        private const double MAX_FUEL_FLOW_RATE = 10.0;

        private double fuelFlowRate;        // Current fuel flow rate
        private double currentFlowRate;     // Fuel flow rate/max fuel rate

        private Ship? _ship;
        private DomainModel _domainModel;
        private IAnimation _shipAnimation;
        private ShipState _shipState = ShipState.None;

        public ShipLoop(
            DomainModel domainModel,
            [FromKeyedServices("Ship")] IAnimation shipAnimation)
        {
            _domainModel = domainModel;
            _shipAnimation = shipAnimation;

            _shipAnimation.Build();
        }

        public DomainModel Run()
        {
            int selection;
            bool exit = false;

            Console.Clear();

            UpdateShipAnimation();
            _shipAnimation.Display();

            InstructUser();

            do
            {
                selection = UserMenu();

                switch (selection)
                {
                    case 1: 
                        // If ship is in orbit in space, start with default values
                        if (_domainModel.ShipModel.ShipState == ShipState.InOrbit)
                        {
                            _ship = new Ship();
                            _ship.ShipState = ShipState.Landing;

                            exit = Fly();
                            SetDomainModelShipModel();
                        }
                        // If ship is on a planet, start with previous lander properties from Domain Model
                        else
                        {
                            _ship = new Ship(
                                _domainModel.ShipModel.FuelFlowRate,
                                _domainModel.ShipModel.Altitude,
                                _domainModel.ShipModel.TotalFuel,
                                _domainModel.ShipModel.LanderMass,
                                _domainModel.ShipModel.MaxFuelRate,
                                _domainModel.ShipModel.MaxThrust,
                                _domainModel.ShipModel.FreeFallAcceleration,
                                _domainModel.ShipModel.ShipState);

                            _ship.ShipState = ShipState.Docking;

                            exit = Fly();
                            SetDomainModelShipModel();
                        }
                        
                        break;

                    case 2: // User wants to exit the program
                        Console.WriteLine("Exiting lander program.");
                        _domainModel.GameState = GameState.SpaceScenario;
                        _domainModel.SpaceMapModel.SpaceMapState = SpaceMapState.OverPlanet;

                        exit = true;
                        break;

                    case 3: // User wants to display the instructions
                        InstructUser();
                        exit = false;
                        break;

                    default:
                        Console.WriteLine($"You entered '{selection}' which is not a valid menu choice.");
                        exit = false;
                        break;
                }
            } while (!exit);

            return _domainModel;
        }

        // Use the Ship properties to calculate the motion of the ship
        // Run the ShipAnimation and move the player charactor
        private bool Fly()
        {
            bool landOrCrash = false;
            string? flowrate = string.Empty;

            if (_ship == null)
            {
                // TODO: this needs to return a code indicating bad state (along with crashed, landed, etc.
                return false;
            }

            do
            {
                Console.Write("Enter a new fuel flow rate or press enter to keep it unchanged: ");

                flowrate = Console.ReadLine();

                if (flowrate != "")
                {
                    while (!double.TryParse(flowrate, out fuelFlowRate))
                    {
                        Console.Write("Please enter an integer or decimal value for fuel flow rate: ");
                        flowrate = Console.ReadLine();
                    }
                }

                // Fuel flow rate cannot be greater than MAX_FUEL_FLOW_RATE
                if (fuelFlowRate > MAX_FUEL_FLOW_RATE)
                {
                    Console.WriteLine("Fuel flow rate must be 0 through 10.");
                    Console.WriteLine("Setting fuel flow to 10.");
                    fuelFlowRate = MAX_FUEL_FLOW_RATE;
                }

                // Fuel flow rate cannot be less than zero
                if (fuelFlowRate < 0)
                {
                    Console.WriteLine("Fuel flow rate must be 0 through 10.");
                    Console.WriteLine("Setting fuel flow to 0");
                    fuelFlowRate = 0;
                }

                // Calculate current flow rate as (fuel flow/max fuel rate)
                currentFlowRate = _ship.ChangeFlow(fuelFlowRate);

                Console.Clear();

                // Use the ship configuration to run one landing or docking cycle
                if (_ship.ShipState == ShipState.Landing)
                {
                    _shipState = _ship.RunLandingCycle(currentFlowRate);
                }
                else if (_ship.ShipState == ShipState.Docking)
                {
                    _shipState = _ship.RunDockingCycle(currentFlowRate);
                }

                SetDomainModelShipModel();
                UpdateShipAnimation();
                _shipAnimation.Display();
                landOrCrash = ProcessCurrentState();

            } while (!landOrCrash); // While lander has not landed or crashed

            return landOrCrash;
        }

        private bool ProcessCurrentState()
        {
            bool exitShipLoop = false;

            switch (_shipState)
            {
                case ShipState { } state when state == ShipState.Flying:
                    break;
                case ShipState { } state when state == ShipState.Landed:
                    Console.WriteLine($"You've landed successfully!");
                    SetDomainModelShipModel();
                    _domainModel.GameState = GameState.HomeScenario;
                    exitShipLoop = true;
                    break;
                case ShipState { } state when state == ShipState.OutOfFuel:
                    Console.WriteLine($"You ran out of fuel and will crash into the planet.");
                    _domainModel.GameState = GameState.ShipCrashed;
                    exitShipLoop = true;
                    break;
                case ShipState { } state when state == ShipState.Crashed:
                    Console.WriteLine($"Your ship has smashed into the planet!");
                    _domainModel.GameState = GameState.ShipCrashed;
                    exitShipLoop = true;
                    break;
                case ShipState { } state when state == ShipState.InOrbit:
                    Console.WriteLine($"You've reached orbit successfully!");
                    SetDomainModelShipModel();
                    _domainModel.GameState = GameState.SpaceScenario;
                    _domainModel.SpaceMapModel.SpaceMapState = SpaceMapState.OverHomePlanet;
                    exitShipLoop = true;
                    break;
                default:
                    break;
            }

            return exitShipLoop;
        }

        private void UpdateShipAnimation()
        {
            if (_ship == null) return;
            if (_shipAnimation == null) return;

            // Calculate the row to which to move
            double totalDistance = Math.Abs(_ship.StartingAltitude - _ship.TargetAltitude);
            int metersPerRow = (int)(totalDistance / _shipAnimation.RowCount);
            int row = (int)(_ship.DistanceFromTarget / metersPerRow);

            if (_ship.ShipState == ShipState.Landing)
            {
                row = Math.Abs(row - _shipAnimation.RowCount) - 1;
            }

            // Move the ship
            _shipAnimation.MoveToRow(row);
        }

        // Keep track of the current ship properties for use in the Domain Model
        private void SetDomainModelShipModel()
        {
            if (_ship == null)
            {
                return;
            }

            _domainModel.ShipModel = _ship.GenerateModel();
        }

        private int UserMenu()
        {
            int choice;

            Console.WriteLine("Please choose from the following options:");
            Console.WriteLine("1. Fire-up the ship");
            Console.WriteLine("2. Abort the operation");
            Console.WriteLine("3. Display user instructions");
            Console.WriteLine("Enter your choice: ");

            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine($"Invalid selction. Please try again :");
            }

            return choice;
        }

        private void InstructUser()
        {
            if (_domainModel.PlanetMapModel.PlanetMapState == PlanetMapState.InitiateHomePlanetLanding)
            {
                Console.WriteLine("You will attempt to land your ship on the planet.");
            }
            else
            {
                Console.WriteLine("You will attempt to launch your ship to orbit.");
            }
            
            Console.WriteLine("In order to accomplish this, your velocity");
            Console.WriteLine("when you land must be 2.0 or below.");
            Console.WriteLine("Be careful- if your velocity is too high, it will be impossible to slow down in time.");
        }
    }
}
