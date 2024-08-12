using SpaceGame.BaseClasses;

namespace SpaceGame.Models
{
    internal class SpaceMapModel
    {
        public SpaceMapState SpaceMapState { get; set; } = SpaceMapState.None;
        public (int, int) Position { get; set; }
        public MapObject? ObjectAtPosition { get; set; }
    }
}
