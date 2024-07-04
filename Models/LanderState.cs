namespace SpaceGame.Models
{
    public enum LanderState
    {
        None = 0,
        Landed = 1,
        Docked = 2,
        Flying = 3,
        OutOfFuel = 4,
        Crashed = 5
    }

    public class LanderProperties
    {
        public LanderState LanderState { get; set; }
        public double FuelFlowRate { get; set; }        // Current fuel flow rate
        public double Altitude { get; set; }            // Current altitude
        public double TotalFuel { get; set; }           // Current amount of fuel
        public double LanderMass { get; set; }          // Mass of lander
        public double TotalMass { get; set; }           // Mass of lander + fuel
        public double MaxFuelRate { get; set; }         // Maximum fuel amount
        public double MaxThrust { get; set; }           // Maximum thrust level
        public double FreeFallAcceleration { get; set; }// The affect planet gravity has on velocity
    }
}
