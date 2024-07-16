namespace SpaceGame.Models
{
    public class LanderProperties
    {
        public LanderState LanderState { get; set; } = LanderState.Docked;
        public double FuelFlowRate { get; set; } = 0;   // Current fuel flow rate
        public double Altitude { get; set; } = 1000;    // Current altitude
        public double TotalFuel { get; set; } = 1700;   // Current amount of fuel
        public double LanderMass { get; set; } = 900;   // Mass of lander
        public double TotalMass { get; set; } = 2600;   // Mass of lander + fuel
        public double MaxFuelRate { get; set; } = 10;   // Maximum fuel amount
        public double MaxThrust { get; set; } = 5000;   // Maximum thrust level
        public double FreeFallAcceleration { get; set; } = 1.62;    // The affect planet gravity has on velocity
    }
}
