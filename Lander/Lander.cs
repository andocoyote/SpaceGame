using System;
using SpaceGame.Models;

namespace SpaceGame.Lander
{
    internal class Lander
    {
        private const double TIME_MULTIPLIER = 3;   // Small amount of time passing during each cycle
        public double FuelFlowRate { get; private set; } = 0;   // Fuel flow rate as chosen by the pilot
        public double Velocity { get; private set; } = 0;       //CURRENT VELOCITY
        public double Altitude { get; private set; } = 1000;    //CURRENT ALTITUDE
        public double TotalFuel { get; private set; } = 1700;   //CURRENT AMOUNT OF FUEL
        public double LanderMass { get; private set; } = 900;   //TOTAL MASS OF LANDER
        public double MaxFuelRate { get; private set; } = 10;   //MAXIMUM FUEL CONSUMTION RATE
        public double MaxThrust { get; private set; } = 5000;   //MAXIMUM THRUST
        public double FreeFallAcceleration { get; private set; } = 1.62; // The affect planet gravity has on velocity
        public double TotalMass { get; private set; }      // Lander mass + fuel

        // Use this constructor when landing on a planet
        public Lander()
	    {
            TotalMass = LanderMass + TotalFuel;
        } 

        // Use this constructo when leaving a planet to dock at the ship
        public Lander(double f, double v, double a, double tf, double l, double mf, double mt, double ffa)
	    {
            FuelFlowRate = f;  //CHOSEN FUEL FLOW RATE
            Velocity = v;      //CURRENT VELOCITY
            Altitude = a;      //CURRENT ALTITUDE
            TotalFuel = tf;    //CURRENT AMOUNT OF FUEL
            LanderMass = l;    //MASS OF THE LANDER WITHOUT FUEL
            MaxFuelRate = mf;  //MAXIMUM FUEL CONSUMTION RATE
            MaxThrust = mt;	   //MAXIMUM THRUST
            FreeFallAcceleration = ffa; // The affect planet gravity has on velocity
            TotalMass = LanderMass + TotalFuel;
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

        public void DisplayValues()
        {
            Console.WriteLine($"The fuelflow is: {FuelFlowRate}");
            Console.WriteLine($"The velocity is: {Velocity}");
            Console.WriteLine($"The altitude is: {Altitude}");
            Console.WriteLine($"The total fuel is: {TotalFuel}");
            Console.WriteLine($"The mass of the lander is: {LanderMass}");
            Console.WriteLine($"The maximum fuel consumption rate is: {MaxFuelRate}");
            Console.WriteLine($"The maximum thrust of the lander's engines is: {MaxThrust}");
        }

        // Run the action of the lander landing on a planet
        public LanderState RunLandingCycle(double currentFlowRate)
        {
            double currentThrust = currentFlowRate * MaxThrust;    // Current thrust is current Flow Rate * Max Thrust

            // Calculate fuel used during this cycle
            TotalFuel = TotalFuel - (TIME_MULTIPLIER * currentFlowRate);

            // Calculate total mass of the lander during this cycle
            TotalMass = LanderMass + TotalFuel;

            // Use thrust, mass, time variable, and freeFallAcceleration to calculate velocity
            Velocity = (Velocity) - (TIME_MULTIPLIER * ((currentThrust / TotalMass) - FreeFallAcceleration));

            // Calculate altitude
            Altitude = Altitude - (TIME_MULTIPLIER * Velocity);

            // Fuel has fallen below zero
            if (TotalFuel < 0)
            {
                TotalFuel = 0;
                FuelFlowRate = 0;
                
                return LanderState.OutOfFuel;
            }

            // Player crashed into the planet (was going too fast)
            if (Altitude < 0 && Velocity > 2)
            {
                Altitude = 0;
                FuelFlowRate = 0;

                return LanderState.Crashed;
            }

            // Player landed successfully
            if (Altitude < 0 && Velocity <= 2)
            {
                Altitude = 0;
                FuelFlowRate = 0;

                return LanderState.Landed;
            }

            return LanderState.Flying;  // Still flying- haven't landed or crashed
        }
    }
}
