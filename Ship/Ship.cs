using SpaceGame.Models;

namespace SpaceGame.Ship
{
    internal class Ship
    {
        private const int TIME_MULTIPLIER = 3;   // Small amount of time passing during each cycle
        private const int MAX_COMPLETION_VELOCITY = 2;  // The max velocity acceptable for landing and docking
        public ShipState ShipState { get; set; } = ShipState.Docked;
        public double FuelFlowRate { get; private set; } = 0;   // Fuel flow rate as chosen by the pilot
        public double Velocity { get; private set; } = 0;       // Current velocity
        public double Altitude { get; private set; } = 1000;    // Current altitude
        public double TotalFuel { get; private set; } = 1700;   // Current amount of fuel
        public double LanderMass { get; private set; } = 900;   // Mass of lander without fuel
        public double MaxFuelRate { get; private set; } = 10;   // Maximum fuel consumption rate
        public double MaxThrust { get; private set; } = 5000;   // Maximum thrust level
        public double FreeFallAcceleration { get; private set; } = 1.62; // The affect planet gravity has on velocity
        public double TotalMass { get; private set; }           // Lander mass + fuel
        public double DistanceFromTarget { get; private set; } = 0; // How far we have to travel
        public double StartingAltitude { get; private set; } = 0;
        public double TargetAltitude { get; private set; } = 0;

        private bool _altimeterIsSet = false;

        // Use this constructor when landing on a planet
        public Ship()
	    {
            TotalMass = LanderMass + TotalFuel;
            TargetAltitude = 0;
            StartingAltitude = 1000;
            TargetAltitude = 0;
            DistanceFromTarget = Math.Abs(TargetAltitude - StartingAltitude);
        } 

        // Use this constructor when landing or leaving a planet to dock at the ship
        public Ship(
            double fuelFlowRate,
            double altitude,
            double totalFuel,
            double landerMass,
            double maxFuelRate,
            double maxThrust,
            double freeFallAcceleration,
            ShipState shipState)
	    {
            FuelFlowRate = fuelFlowRate;    // Fuel flow rate as chosen by the pilot
            Altitude = altitude;            // Current altitude
            TotalFuel = totalFuel;          // Current amount of fuel
            LanderMass = landerMass;        // Mass of lander without fuel
            MaxFuelRate = maxFuelRate;      // Maximum fuel consumption rate
            MaxThrust = maxThrust;	        // Maximum thrust level
            FreeFallAcceleration = freeFallAcceleration; // The affect planet gravity has on velocity
            TotalMass = LanderMass + TotalFuel;

            // If the ship is docked, we need to get to the surface of the planet (altitude == 0)
            // Else, we need to get back to the ship (altitude == 1000)
            TargetAltitude = shipState == ShipState.Docked ? 0 : 1000;
            DistanceFromTarget = Math.Abs(TargetAltitude - StartingAltitude);
        }

        // Change fuel flow rate
        public double ChangeFlow(double fuelFlow)
        {
            double currentFlowRate = 0;
            FuelFlowRate = fuelFlow;

            //CALCULATE CURRENT FLOW RATE AS A PERCENTAGE OF THE MAX ALLOWED
            currentFlowRate = (fuelFlow / MaxFuelRate);

            return currentFlowRate;
        }

        // Always set your altimeter before flying
        public void SetAltimeter()
        {
            StartingAltitude = Altitude;
            DistanceFromTarget = Math.Abs(TargetAltitude - StartingAltitude);
            _altimeterIsSet = true;
        }

        public void DisplayValues()
        {
            Console.WriteLine($"The fuelflow is: {FuelFlowRate}");
            Console.WriteLine($"The velocity is: {Velocity}");
            Console.WriteLine($"The altitude is: {Altitude}");
            Console.WriteLine($"The target altitude is: {TargetAltitude}");
            Console.WriteLine($"The distance from target is: {DistanceFromTarget}");
            Console.WriteLine($"The total fuel is: {TotalFuel}");
            Console.WriteLine($"The mass of the lander is: {LanderMass}");
            Console.WriteLine($"The maximum fuel consumption rate is: {MaxFuelRate}");
            Console.WriteLine($"The maximum thrust of the lander's engines is: {MaxThrust}");
        }

        // Run the action of the ship landing on a planet
        public ShipState RunLandingCycle(double currentFlowRate)
        {
            double currentThrust = currentFlowRate * MaxThrust;    // Current thrust is current Flow Rate * Max Thrust
            if (!_altimeterIsSet)
            {
                SetAltimeter();
            }

            // Calculate fuel used during this cycle
            TotalFuel = TotalFuel - (TIME_MULTIPLIER * currentFlowRate);

            // Calculate total mass of the lander during this cycle
            TotalMass = LanderMass + TotalFuel;

            // Use thrust, mass, time variable, and freeFallAcceleration to calculate velocity
            Velocity = Velocity - (TIME_MULTIPLIER * ((currentThrust / TotalMass) - FreeFallAcceleration));

            // Calculate altitude
            Altitude = Altitude - (TIME_MULTIPLIER * Velocity);

            // Track how far we have yet to go
            //DistanceFromTarget -= Math.Abs(DistanceFromTarget - Altitude);
            DistanceFromTarget = Altitude - TargetAltitude;

            // Fuel has fallen below zero
            if (TotalFuel < 0)
            {
                TotalFuel = 0;
                FuelFlowRate = 0;
                _altimeterIsSet = false;

                this.ShipState = ShipState.OutOfFuel;
            }
            // Player crashed into the planet (was going too fast)
            else if (DistanceFromTarget <= 0 && Velocity > MAX_COMPLETION_VELOCITY)
            {
                Altitude = 0;
                FuelFlowRate = 0;
                _altimeterIsSet = false;

                this.ShipState = ShipState.Crashed;
            }
            // Player landed successfully
            else if (DistanceFromTarget <= 0 && Velocity <= MAX_COMPLETION_VELOCITY)
            {
                Velocity = 0;
                Altitude = 0;
                StartingAltitude = 0;
                DistanceFromTarget = 0;
                FuelFlowRate = 0;
                _altimeterIsSet = false;

                this.ShipState = ShipState.Landed;
            }
            // Still flying- haven't landed, docked, or crashed
            else
            {
                //this.LanderState = LanderState.Flying;
            }

            return this.ShipState;
        }

        // Run the action of the lander landing on a planet
        public ShipState RunDockingCycle(double currentFlowRate)
        {
            double currentThrust = currentFlowRate * MaxThrust;    // Current thrust is current Flow Rate * Max Thrust
            if (!_altimeterIsSet)
            {
                SetAltimeter();
            }

            // Calculate fuel used during this cycle
            TotalFuel = TotalFuel - (TIME_MULTIPLIER * currentFlowRate);

            // Calculate total mass of the lander during this cycle
            TotalMass = LanderMass + TotalFuel;

            // Use thrust, mass, time variable, and freeFallAcceleration to calculate velocity
            Velocity += (TIME_MULTIPLIER * ((currentThrust / TotalMass) - FreeFallAcceleration));

            // Calculate altitude
            Altitude += (TIME_MULTIPLIER * Velocity);

            // Track how far we have yet to go
            DistanceFromTarget = TargetAltitude - Altitude;

            // Fuel has fallen below zero
            if (TotalFuel < 0)
            {
                TotalFuel = 0;
                FuelFlowRate = 0;
                _altimeterIsSet = false;

                this.ShipState = ShipState.OutOfFuel;
            }
            // Player crashed into the planet (was going too fast)
            else if (DistanceFromTarget <= 0 && Velocity > MAX_COMPLETION_VELOCITY)
            {
                Altitude = 0;
                FuelFlowRate = 0;
                _altimeterIsSet = false;

                this.ShipState = ShipState.Crashed;
            }
            // Player docked successfully
            else if (DistanceFromTarget <= 0 && Velocity <= MAX_COMPLETION_VELOCITY)
            {
                Velocity = 0;
                Altitude = 1000;
                StartingAltitude = 0;
                DistanceFromTarget = 0;
                FuelFlowRate = 0;
                _altimeterIsSet = false;
                this.ShipState = ShipState.Docked;
            }
            // Still flying- haven't landed, docked, or crashed
            else
            {
                //this.LanderState = LanderState.Flying;
            }

            return this.ShipState;
        }
    }
}
