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

        public Lander()
	    {
		    _fuelFlow = 0;      //CHOSEN FUEL FLOW RATE
            _velocity = 0;      //CURRENT VELOCITY
            _altitude = 1000;   //CURRENT ALTITUDE
            _totalFuel = 1700;  //CURRENT AMOUNT OF FUEL
            _landerMass = 900;  //MASS OF THE LANDER WITHOUT FUEL
            _maxFuelRate = 10;  //MAXIMUM FUEL CONSUMTION RATE
            _maxThrust = 5000;	//MAXIMUM THRUST
	    } 

        public Lander(double f, double v, double a, double tf, double l, double mf, double mt)
	    {

		    _fuelFlow = f;      //CHOSEN FUEL FLOW RATE	
		    _velocity = v;      //CURRENT VELOCITY
            _altitude = a;      //CURRENT ALTITUDE
            _totalFuel = tf;    //CURRENT AMOUNT OF FUEL
            _landerMass = l;    //MASS OF THE LANDER WITHOUT FUEL
            _maxFuelRate = mf;  //MAXIMUM FUEL CONSUMTION RATE
            _maxThrust = mt;	//MAXIMUM THRUST

	    }   //END OF USER DEFINED VALUES CONSTRUCTOR

        //PERIODICALLY ASKS USER FOR FULE FLOW CHANGE
        public double ChangeFlow(double fuelFlow)
        {
            double currentFlowRate = 0;  //CALCULATED AS FUEL FLOW / MAX FUEL RATE

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

        //DISPLAY CURRENT ALTITUDE
        public void ShowAltitude()
        {
            Console.WriteLine($"Your altitude is: {_altitude}");
        }

        //DISPLAY CURRENT FUEL FLOW RATE
        public void ShowFuelFlow(double fuelFlow)
        {
            Console.WriteLine($"The fuel flow rate is: {_fuelFlow}");
        }

        //DISPLAY CURRENT AMOUNT OF FUEL
        public void ShowTotalFuel()
        {
            Console.WriteLine($"The total fuel is: {_totalFuel}");
        }

        //DISPLAY CURRENT VELOCITY
        public void ShowVelocity()
        {
            Console.WriteLine($"Your velocity is: {_velocity}");
        }

        //SIMULATE ACTION OF THE LUNAR LANDER
        public bool RunLandingCycle(double currentFlowRate)
        {
            double currentThrust = 0;   //CURRENT THRUST IS CURRENT FLOW RATE * MAX THRUST
            double totalMass;           //MASS OF LANDER AND FUEL
            double t = 0.1;             //SMALL AMOUNT OF TIME VARIABLE

            //CALCULATE CURRENT THRUST
            currentThrust = (currentFlowRate * _maxThrust);

            //FOR LOOP SIMULATES THE PASSAGE OF A SMALL AMOUNT OF TIME
            for (int i = 0; i < 30; ++i)
            {

                totalMass = 0;  //INITITIALIZE TOTAL MASS TO ZERO

                //CALCULATE TOTAL AMOUNT OF FUEL
                _totalFuel = _totalFuel - (t * currentFlowRate);

                //CALCULATE COMBINED MASS OF LANDER AND FUEL
                totalMass = _landerMass + _totalFuel;

                //CALCULATE CURRENT VELOCITY
                _velocity = (_velocity) - (t * ((currentThrust / totalMass) - 1.62));

                //CALCULATE CURRENT ALTITUDE
                _altitude = _altitude - (t * _velocity);

                //IF TOTAL FUEL DROPS BELOW ZERO
                if (_totalFuel < 0)
                {
                    _totalFuel = 0;  //SET TOTAL FUEL TO ZERO
                    _fuelFlow = 0;   //SET FUEL FLOW RATE TO ZERO

                    Console.WriteLine($"OUT OF FUEL!!!");
                    Console.WriteLine($"GAME OVER!");
                    return true;
                }

                //IF PLAYER LANDS SUCCESSFULLY
                if ((_altitude < 0) && (_velocity <= 2))
                {
                    _altitude = 0;   //SET ALTITUDE TO ZERO
                    _fuelFlow = 0;   //SET FUEL FLOW RATE TO ZERO

                    Console.WriteLine($"YOU LANDED SUCCESSFULLY!!! CONGRADULATIONS!!!");
                    return true;
                }

                //IF PLAYER CRASHES THE LANDER
                if ((_altitude < 0) && (_velocity > 2))
                {
                    _altitude = 0;   //SET ALTITUDE TO ZERO
                    _fuelFlow = 0;   //SET FUEL FLOW RATE TO ZERO

                    Console.WriteLine($"YOUR LANDER HAS SMASHED INTO THE Planet!!!");
                    Console.WriteLine($"GAME OVER!");
                    return true;
                }

            }

            return false;  //DON'T LEAVE GAME PLAY LOOP IN MAIN
        }
    }
}
