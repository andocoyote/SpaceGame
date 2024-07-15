using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;

namespace SpaceGame.Lander
{
    internal class LanderLoop : IScenario
    {
        //ANDREW MIKESELL
        //CS-210-A
        //03-08-01

        private const double MAX_FUEL_FLOW_RATE = 10.0;

        private double fuelFlowRate;        // Current fuel flow rate
        private double currentFlowRate;     // Fuel flow rate/max fuel rate

        private Lander? _lander;
        private DomainModel _domainModel;
        private ILanderAnimation _landerAnimation;
        private LanderState _landerState = LanderState.None;

        public LanderLoop(
            DomainModel domainModel,
            ILanderAnimation landerAnimation)
        {
            _domainModel = domainModel;
            _landerAnimation = landerAnimation;

            _landerAnimation.Build();
        }

        public DomainModel Run()
        {
            int selection;
            bool exit = false;

            Console.Clear();
            InstructUser();

            do
            {
                selection = UserMenu();

                switch (selection)
                {
                    case 1: 
                        // If lander is docked at the ship in space, start with default values
                        if (_domainModel.LanderProperties.LanderState == LanderState.Docked)
                        {
                            _lander = new Lander();
                            _lander.LanderState = LanderState.Landing;

                            _lander.DisplayValues();

                            exit = Fly();
                        }
                        // If lander is on a planet, start with previous lander properties from Domain Model
                        else
                        {
                            _lander = new Lander(
                                _domainModel.LanderProperties.FuelFlowRate,
                                _domainModel.LanderProperties.Altitude,
                                _domainModel.LanderProperties.TotalFuel,
                                _domainModel.LanderProperties.LanderMass,
                                _domainModel.LanderProperties.MaxFuelRate,
                                _domainModel.LanderProperties.MaxThrust,
                                _domainModel.LanderProperties.FreeFallAcceleration,
                                _domainModel.LanderProperties.LanderState);

                            _lander.LanderState = LanderState.Docking;

                            _lander.DisplayValues();

                            exit = Fly();
                        }
                        
                        break;

                    case 2: // User wants to exit the program
                        Console.WriteLine("Exiting lander program.");
                        _domainModel.GameState = GameState.OverPlanet;
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

        // Use the Lander properties to calculate the motion of the lander
        // Run the LanderAnimation and move the player charactor
        private bool Fly()
        {
            bool landOrCrash = false;
            string? flowrate = string.Empty;

            if (_lander == null)
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
                currentFlowRate = _lander.ChangeFlow(fuelFlowRate);

                Console.Clear();

                // Use the ship configuration to run one landing or docking cycle
                if (_lander.LanderState == LanderState.Landing)
                {
                    _landerState = _lander.RunLandingCycle(currentFlowRate);
                }
                else if (_lander.LanderState == LanderState.Docking)
                {
                    _landerState = _lander.RunDockingCycle(currentFlowRate);
                }

                List<string> landerProperties = new List<string>()
                {
                    $"Velocity: {_lander.Velocity}",
                    $"Altitude: {_lander.Altitude}",
                    $"Starting Altitude: {_lander.StartingAltitude}",
                    $"Target Altitude: {_lander.TargetAltitude}",
                    $"Distance from Target: {_lander.DistanceFromTarget}",
                    $"Total Fuel: {_lander.TotalFuel}",
                    $"Fuel Flow Rate: {_lander.FuelFlowRate}"
                };

                for (int i = 0; i < landerProperties.Count; i++)
                {
                    _landerAnimation.AnimationText[i] = landerProperties[i];
                }

                UpdateLanderAnimation();
                _landerAnimation.Display();
                landOrCrash = ProcessCurrentState();

                /*Console.WriteLine($"Your velocity is: {_lander.Velocity}");
                Console.WriteLine($"Your altitude is: {_lander.Altitude}");
                Console.WriteLine($"Your target altitude is: {_lander.TargetAltitude}");
                Console.WriteLine($"Your starting altitude was: {_lander.StartingAltitude}");
                Console.WriteLine($"Your distance from target is: {_lander.DistanceFromTarget}");
                Console.WriteLine($"The total fuel is: {_lander.TotalFuel}");
                Console.WriteLine($"The fuel flow rate is: {_lander.FuelFlowRate}");*/

            } while (!landOrCrash); // While lander has not landed or crashed

            return landOrCrash;
        }

        private bool ProcessCurrentState()
        {
            bool exitLanderLoop = false;

            switch (_landerState)
            {
                case LanderState.Flying:
                    break;
                case LanderState.Landed:
                    Console.WriteLine($"You've landed successfully!");
                    SetDomainModelLanderProperties();
                    _domainModel.GameState = GameState.OnLandingZone;
                    exitLanderLoop = true;
                    break;
                case LanderState.OutOfFuel:
                    Console.WriteLine($"You ran out of fuel and will crash into the planet.");
                    _domainModel.GameState = GameState.LanderCrashed;
                    exitLanderLoop = true;
                    break;
                case LanderState.Crashed:
                    Console.WriteLine($"Your lander has smashed into the planet!");
                    _domainModel.GameState = GameState.LanderCrashed;
                    exitLanderLoop = true;
                    break;
                case LanderState.Docked:
                    Console.WriteLine($"You've docked successfully!");
                    SetDomainModelLanderProperties();
                    _domainModel.GameState = GameState.OverPlanet;
                    exitLanderLoop = true;
                    break;
                default:
                    break;
            }

            return exitLanderLoop;
        }

        private void UpdateLanderAnimation()
        {
            if (_lander == null) return;
            if (_landerAnimation == null) return;

            // Calculate the row to which to move
            double totalDistance = Math.Abs(_lander.StartingAltitude - _lander.TargetAltitude);
            int metersPerRow = (int)(totalDistance / _landerAnimation.RowCount);
            int row = (int)(_lander.DistanceFromTarget / metersPerRow);

            if (_lander.LanderState == LanderState.Landing)
            {
                row = Math.Abs(row - _landerAnimation.RowCount) - 1;
            }

            // Move the lander
            _landerAnimation.MoveToRow(row);
        }

        // Keep track of the current lander properties for use in the Domain Model
        private void SetDomainModelLanderProperties()
        {
            if (_lander == null)
            {
                return;
            }

            _domainModel.LanderProperties.LanderState = _landerState;
            _domainModel.LanderProperties.FuelFlowRate = _lander.FuelFlowRate;
            _domainModel.LanderProperties.Altitude = _lander.Altitude;
            _domainModel.LanderProperties.TotalFuel = _lander.TotalFuel;
            _domainModel.LanderProperties.LanderMass = _lander.LanderMass;
            _domainModel.LanderProperties.TotalMass = _lander.TotalMass;
            _domainModel.LanderProperties.MaxFuelRate = _lander.MaxFuelRate;
            _domainModel.LanderProperties.MaxThrust = _lander.MaxThrust;
            _domainModel.LanderProperties.FreeFallAcceleration = _lander.FreeFallAcceleration;
    }

        private int UserMenu()
        {
            int choice;

            Console.WriteLine("Please choose from the following options:");
            Console.WriteLine("1. Fire-up the lander");
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
            if (_domainModel.GameState == GameState.InitiateLanding)
            {
                Console.WriteLine("You will attempt to land your ship on the planet.");
            }
            else
            {
                Console.WriteLine("You will attempt to dock your lander to the ship.");
            }
            
            Console.WriteLine("In order to accomplish this, your velocity");
            Console.WriteLine("when you land must be 2.0 or below.");
            Console.WriteLine("Be careful- if your velocity is too high, it will be impossible to slow down in time.");
        }
    }
}
