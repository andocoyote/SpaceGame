using SpaceGame.BaseClasses;

namespace SpaceGame.Models
{
    internal class PlanetMapModel
    {
        public PlanetMapState PlanetMapState { get; set; }
        public (int, int) Position { get; set; }
        public MapObject? ObjectAtPosition { get; set; }
    }
}
