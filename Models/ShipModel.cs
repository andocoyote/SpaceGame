using SpaceGame.BaseClasses;

namespace SpaceGame.Models
{
    internal class ShipModel
    {
        public ShipState ShipState { get; set; } = SpaceGame.Models.ShipState.Landed;
        public double Velocity { get; set; } = 0;       // Current velocity
        public double StartingAltitude { get; set; } = 0; // The altitude at which the lander started its mission
        public double TargetAltitude { get; set; } = 0; // The altitude the lander is trying to reach
        public double DistanceFromTarget { get; set; } = 0; // How far the lander is from its target
        public double FuelFlowRate { get; set; } = 0;   // Current fuel flow rate
        public double Altitude { get; set; } = 0;    // Current altitude
        public double TotalFuel { get; set; } = 1700;   // Current amount of fuel
        public double LanderMass { get; set; } = 900;   // Mass of lander
        public double TotalMass { get; set; } = 2600;   // Mass of lander + fuel
        public double MaxFuelRate { get; set; } = 10;   // Maximum fuel amount
        public double MaxThrust { get; set; } = 5000;   // Maximum thrust level
        public double FreeFallAcceleration { get; set; } = 1.62;    // The affect planet gravity has on velocity

        public ShipModel()
        {
            this.ShipState = SpaceGame.Models.ShipState.Landed;
        }
    }
}
