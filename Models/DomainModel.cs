using SpaceGame.BaseClasses;
using SpaceGame.Maps;

namespace SpaceGame.Models
{
    internal class DomainModel
    {
        public string PlayerName { get; set; } = string.Empty;
        public GameState GameState { get; set; } = GameState.None;
        public MapObject? MapObject { get; set; } = null;
        public SpaceMapModel SpaceMapModel { get; set; } = new SpaceMapModel();
        public PlanetMapModel PlanetMapModel { get; set; } = new PlanetMapModel();
        public ShipModel ShipModel { get; set; } = new ShipModel();
        public LanderModel LanderModel { get; set; } = new LanderModel();
    }
}
