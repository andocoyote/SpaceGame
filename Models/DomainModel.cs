using SpaceGame.BaseClasses;
using SpaceGame.Maps;

namespace SpaceGame.Models
{
    internal class DomainModel
    {
        public GameState GameState { get; set; }
        public MapObject? MapObject { get; set; }
        public SpaceMapModel SpaceMapModel { get; set; } = new SpaceMapModel();
        public PlanetMapModel PlanetMapModel { get; set; } = new PlanetMapModel();
        public ShipModel ShipModel { get; set; } = new ShipModel();
        public LanderModel LanderModel { get; set; } = new LanderModel();
    }
}
