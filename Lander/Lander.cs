using System;

namespace SpaceGame.Lander
{
    internal class Lander
    {
        private double _fuelFlow;       //CHOSEN FUEL FLOW RATE
        private double _velocity;       //CURRENT VELOCITY
        private double _altitude;       //CURRENT ALTITUDE
        private double _totalFuel;      //CURRENT AMOUNT OF FUEL
        private double _landerMass;     //TOTAL MASS OF LANDER 
        private double _maxFuelRate;    //MAXIMUM FUEL CONSUMTION RATE
        private double _maxThrust;      //MAXIMUM THRUST
        private double _freeFallAcceleration; // The affect planet gravity has on velocity

        public Lander()
	    {
		    _fuelFlow = 0;      //CHOSEN FUEL FLOW RATE
            _velocity = 0;      //CURRENT VELOCITY
            _altitude = 1000;   //CURRENT ALTITUDE
            _totalFuel = 1700;  //CURRENT AMOUNT OF FUEL
            _landerMass = 900;  //MASS OF THE LANDER WITHOUT FUEL
            _maxFuelRate = 10;  //MAXIMUM FUEL CONSUMTION RATE
            _maxThrust = 5000;	//MAXIMUM THRUST
            _freeFallAcceleration = 1.62; // The affect planet gravity has on velocity
        } 

        public Lander(double f, double v, double a, double tf, double l, double mf, double mt, double ffa)
	    {
		    _fuelFlow = f;      //CHOSEN FUEL FLOW RATE	
		    _velocity = v;      //CURRENT VELOCITY
            _altitude = a;      //CURRENT ALTITUDE
            _totalFuel = tf;    //CURRENT AMOUNT OF FUEL
            _landerMass = l;    //MASS OF THE LANDER WITHOUT FUEL
            _maxFuelRate = mf;  //MAXIMUM FUEL CONSUMTION RATE
            _maxThrust = mt;	//MAXIMUM THRUST
            _freeFallAcceleration = ffa; // The affect planet gravity has on velocity
        }

        // Change fuel flow rate
        public double ChangeFlow(double fuelFlow)
        {
            double currentFlowRate = 0;
            _fuelFlow = fuelFlow;

            //CALCULATE CURRENT FLOW RATE AS A PERCENTAGE OF THE MAX ALLOWED
            currentFlowRate = (fuelFlow / _maxFuelRate);

            return currentFlowRate;
        }

        public void DisplayValues()
        {
            Console.WriteLine($"The fuelflow is: {_fuelFlow}");
            Console.WriteLine($"The velocity is: {_velocity}");
            Console.WriteLine($"The altitude is: {_altitude}");
            Console.WriteLine($"The total fuel is: {_totalFuel}");
            Console.WriteLine($"The mass of the lander is: {_landerMass}");
            Console.WriteLine($"The maximum fuel consumption rate is: {_maxFuelRate}");
            Console.WriteLine($"The maximum thrust of the lander's engines is: {_maxThrust}");
        }

        public void ShowAltitude()
        {
            Console.WriteLine($"Your altitude is: {_altitude}");
        }

        public void ShowFuelFlow()
        {
            Console.WriteLine($"The fuel flow rate is: {_fuelFlow}");
        }

        public void ShowTotalFuel()
        {
            Console.WriteLine($"The total fuel is: {_totalFuel}");
        }

        public void ShowVelocity()
        {
            Console.WriteLine($"Your velocity is: {_velocity}");
        }

        // Run the action of the lander landing on a planet
        public LanderState RunLandingCycle(double currentFlowRate)
        {
            double currentThrust = currentFlowRate * _maxThrust;    // Current thrust is current Flow Rate * Max Thrust
            double totalMass;   // Mass of lander with fuel
            double t = 3;       // Small amount of time passing during each cycle
            totalMass = 0;

            // Calculate fuel used during this cycle
            _totalFuel = _totalFuel - (t * currentFlowRate);

            // Calculate total mass of the lander during this cycle
            totalMass = _landerMass + _totalFuel;

            // Use thrust, mass, time variable, and freeFallAcceleration to calculate velocity
            _velocity = (_velocity) - (t * ((currentThrust / totalMass) - _freeFallAcceleration));

            // Calculate altitude
            _altitude = _altitude - (t * _velocity);

            // Fuel has fallen below zero
            if (_totalFuel < 0)
            {
                _totalFuel = 0;
                _fuelFlow = 0;
                
                return LanderState.OutOfFuel;
            }

            // Player crashed into the planet (was going too fast)
            if ((_altitude < 0) && (_velocity > 2))
            {
                _altitude = 0;
                _fuelFlow = 0;

                return LanderState.Crashed;
            }

            // Player landed successfully
            if ((_altitude < 0) && (_velocity <= 2))
            {
                _altitude = 0;
                _fuelFlow = 0;

                return LanderState.Landed;
            }

            return LanderState.Flying;  // Still flying- haven't landed or crashed
        }
    }
}
