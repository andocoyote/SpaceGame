using SpaceGame.Logger;

namespace SpaceGame.Lander
{
    internal class LanderLoop : ILanderLoop
    {
        //ANDREW MIKESELL
        //CS-210-A
        //03-08-01

        private const double MAX_FUEL_FLOW_RATE = 10.0;

        private double fuelFlow;         // Current fuel flow rate
        private double velocity;         // Current velocity
        private double altitude;         // Current altitude
        private double totalFuel;        // Current amount of fuel
        private double landerMass;       // Mass of lander
        private double maxFuel;          // Maximum fuel amount
        private double maxThrust;        // Maximum thrust level
        private double currentFlowRate;  // Fuel flow rate/max fuel rate

        private Lander? ship;

        public void Run()
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
                    case 1: // Play using default values
                        ship = new Lander();

                        ship.DisplayValues();

                        Fly();

                        exit = false;
                        break;

                    case 2: // Get user-defined values for the lander properties
                        Console.WriteLine("Enter the maximum fuel consumption rate (Default is 10) : ");
                        while (!double.TryParse(Console.ReadLine(), out maxFuel))
                        {
                            Console.Write("Please enter an integer or decimal value for maximum fuel consumption rate: ");
                        }

                        Console.WriteLine($"Enter your starting throttle rate (0-{maxFuel} only): ");
                        while (!double.TryParse(Console.ReadLine(), out fuelFlow))
                        {
                            Console.Write("Please enter an integer or decimal value for fuel flow rate: ");
                        }

                        // Fuel flow rate cannot be larger than maximum fuel consumption rate
                        if (fuelFlow > maxFuel)
                        {
                            Console.WriteLine($"Fuel flow rate must be 0 through {maxFuel}!");
                            Console.WriteLine($"Setting fuel flow to {maxFuel}");
                            fuelFlow = maxFuel;
                        }

                        // Fuel flow rate cannot be less then zero
                        if (fuelFlow < 0)
                        {
                            Console.WriteLine($"Fuel flow rate must be 0 through {maxFuel}!");
                            Console.WriteLine($"Setting fuel flow to 0");
                            fuelFlow = 0;
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

                        // Build the lander
                        ship = new Lander(fuelFlow, velocity, altitude, totalFuel, landerMass, maxFuel, maxThrust);

                        ship.DisplayValues();

                        Fly();

                        exit = false;
                        break;

                    case 3: // User wants to exit the program
                        Console.WriteLine("Exiting lander program.");
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
        }

        private void Fly()
        {
            bool landOrCrash = false;
            string? flowrate = string.Empty;

            if (ship == null)
            {
                return;
            }

            do
            {
                Console.Write("Enter a new fuel flow rate or press enter to keep it unchanged: ");

                flowrate = Console.ReadLine();

                if (flowrate != "")
                {
                    while (!double.TryParse(flowrate, out fuelFlow))
                    {
                        Console.Write("Please enter an integer or decimal value for fuel flow rate: ");
                        flowrate = Console.ReadLine();
                    }
                }

                // Fuel flow rate cannot be greater than MAX_FUEL_FLOW_RATE
                if (fuelFlow > MAX_FUEL_FLOW_RATE)
                {
                    Console.WriteLine("Fuel flow rate must be 0 through 10.");
                    Console.WriteLine("Setting fuel flow to 10.");
                    fuelFlow = MAX_FUEL_FLOW_RATE;
                }

                // Fuel flow rate cannot be less than zero
                if (fuelFlow < 0)
                {
                    Console.WriteLine("Fuel flow rate must be 0 through 10.");
                    Console.WriteLine("Setting fuel flow to 0");
                    fuelFlow = 0;
                }

                // Calculate current flow rate as (fuel flow/max fuel rate)
                currentFlowRate = ship.ChangeFlow(fuelFlow);

                // Use the ship configuration to run one landing cycle
                landOrCrash = ship.RunLandingCycle(currentFlowRate);

                Console.Clear();

                ship.ShowAltitude();
                ship.ShowFuelFlow();
                ship.ShowVelocity();
                ship.ShowTotalFuel();

            } while (!landOrCrash); // While lander has not landed or crashed
        }

        private int UserMenu()
        {
            int choice;

            Console.WriteLine("Please choose from the following options:");
            Console.WriteLine("1. Start with the DEFAULT values");
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
            Console.WriteLine("Your mission is to land your ship on the planet.");
            Console.WriteLine("In order to accomplish this, your velocity");
            Console.WriteLine("when you land must be 2.0 or below.");
        }
    }
}
