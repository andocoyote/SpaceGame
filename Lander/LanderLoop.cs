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
        private double velocity;            // Current velocity
        private double altitude;            // Current altitude
        private double totalFuel;           // Current amount of fuel
        private double landerMass;          // Mass of lander
        private double maxFuel;             // Maximum fuel amount
        private double maxThrust;           // Maximum thrust level
        private double currentFlowRate;     // Fuel flow rate/max fuel rate
        private double freeFallAcceleration;// The affect planet gravity has on velocity

        private Lander? _lander;
        private DomainModel _domainModel;
        private LanderState _landerState = LanderState.None;

        public LanderLoop(
            DomainModel domainModel)
        {
            _domainModel = domainModel;
        }

        public DomainModel Run()
        {
            int selection;
            bool exit = false;
            bool landOrCrash = false;

            Console.Clear();
            InstructUser();

            do
            {
                selection = UserMenu();

                switch (selection)
                {
                    case 1: // Play using default values
                        _lander = new Lander();

                        _lander.DisplayValues();

                        landOrCrash = Fly();

                        exit = false;
                        break;

                    case 2: // Get user-defined values for the lander properties
                        Console.WriteLine("Enter the maximum fuel consumption rate (Default is 10) : ");
                        while (!double.TryParse(Console.ReadLine(), out maxFuel))
                        {
                            Console.Write("Please enter an integer or decimal value for maximum fuel consumption rate: ");
                        }

                        Console.WriteLine($"Enter your starting throttle rate (0-{maxFuel} only): ");
                        while (!double.TryParse(Console.ReadLine(), out fuelFlowRate))
                        {
                            Console.Write("Please enter an integer or decimal value for fuel flow rate: ");
                        }

                        // Fuel flow rate cannot be larger than maximum fuel consumption rate
                        if (fuelFlowRate > maxFuel)
                        {
                            Console.WriteLine($"Fuel flow rate must be 0 through {maxFuel}!");
                            Console.WriteLine($"Setting fuel flow to {maxFuel}");
                            fuelFlowRate = maxFuel;
                        }

                        // Fuel flow rate cannot be less then zero
                        if (fuelFlowRate < 0)
                        {
                            Console.WriteLine($"Fuel flow rate must be 0 through {maxFuel}!");
                            Console.WriteLine($"Setting fuel flow to 0");
                            fuelFlowRate = 0;
                        }

                        // Get starting velocity
                        Console.WriteLine("Enter the current speed of lander (Default is 0 meters per second) : ");
                        while (!double.TryParse(Console.ReadLine(), out velocity))
                        {
                            Console.Write("Please enter an integer or decimal value for velocity: ");
                        }

                        // Get starting altitude
                        Console.WriteLine("Enter the altitude of the lander (Default is 1000 meters) : ");
                        while (!double.TryParse(Console.ReadLine(), out altitude))
                        {
                            Console.Write("Please enter an integer or decimal value for altitude: ");
                        }

                        // Get total amount of fuel on board
                        Console.WriteLine("Enter the amount of fuel on board (Default is 1700 kgs) : ");
                        while (!double.TryParse(Console.ReadLine(), out totalFuel))
                        {
                            Console.Write("Please enter an integer or decimal value for total fuel: ");
                        }

                        // Get lander mass
                        Console.WriteLine("Enter the mass of the lander when all fuel");
                        Console.WriteLine("has been lost (Default 900 kgs) : ");
                        while (!double.TryParse(Console.ReadLine(), out landerMass))
                        {
                            Console.Write("Please enter an integer or decimal value for lander mass: ");
                        }

                        // Get max allowed thrust of lander engines
                        Console.WriteLine("Enter the maximum thrust of the lander's engines (Default is 5000) : ");
                        while (!double.TryParse(Console.ReadLine(), out maxThrust))
                        {
                            Console.Write("Please enter an integer or decimal value for fuel flow rate: ");
                        }

                        // Get free-fall acceleration
                        Console.WriteLine("Enter the value of free-fall acceleration for the planet (Default is 1.62) : ");
                        while (!double.TryParse(Console.ReadLine(), out freeFallAcceleration))
                        {
                            Console.Write("Please enter an integer or decimal value for free-fall acceleration: ");
                        }

                        // Build the lander
                        _lander = new Lander(fuelFlowRate, velocity, altitude, totalFuel, landerMass, maxFuel, maxThrust, freeFallAcceleration);

                        _lander.DisplayValues();

                        landOrCrash = Fly();

                        exit = false;
                        break;

                    case 3: // User wants to exit the program
                        Console.WriteLine("Exiting lander program.");
                        _domainModel.GameState = GameState.OverPlanet;
                        exit = true;
                        break;

                    case 4: // User wants to display the instructions
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

                // Use the ship configuration to run one landing cycle
                _landerState = _lander.RunLandingCycle(currentFlowRate);

                landOrCrash = ProcessCurrentState();

                Console.WriteLine($"Your altitude is: {_lander.Altitude}");
                Console.WriteLine($"The fuel flow rate is: {_lander.FuelFlowRate}");
                Console.WriteLine($"The total fuel is: {_lander.TotalFuel}");
                Console.WriteLine($"Your velocity is: {_lander.Velocity}");

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
                    exitLanderLoop = true;
                    break;
                case LanderState.OutOfFuel:
                    Console.WriteLine($"You ran out of fuel and will crash into the planet.");
                    exitLanderLoop = true;
                    break;
                case LanderState.Crashed:
                    Console.WriteLine($"Your lander has smashed into the planet!");
                    exitLanderLoop = true;
                    break;
                default:
                    break;
            }

            return exitLanderLoop;
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
            Console.WriteLine("1. Start with the default values");
            Console.WriteLine("2. Enter your own start values");
            Console.WriteLine("3. Abort the landing");
            Console.WriteLine("4. Display user instructions");
            Console.WriteLine("Enter your choice: ");

            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine($"Invalid selction. Please try again :");
            }

            return choice;
        }

        private void InstructUser()
        {
            Console.WriteLine("You will attempt to land your ship on the planet.");
            Console.WriteLine("In order to accomplish this, your velocity");
            Console.WriteLine("when you land must be 2.0 or below.");
            Console.WriteLine("Be careful- if you descend too fast, it will be impossible to slow down in time.");
        }
    }
}
