using SpaceGame.BaseClasses;
using SpaceGame.Maps;

namespace SpaceGame.Models
{
    internal class DomainModel
    {
        public GameState GameState { get; set; }
        public MapObject? MapObject { get; set; }
        public Dictionary<string, MapObject> PlanetsDictionary { get; set;  } = new Dictionary<string, MapObject>();
        public Dictionary<string, Dictionary<string, MapObject>> PlanetObjectsDictionary { get; set; } = new Dictionary<string, Dictionary<string, MapObject>>();
        public ShipModel ShipModel { get; set; } = new ShipModel();
        public LanderModel LanderModel { get; set; } = new LanderModel();
    }
}
