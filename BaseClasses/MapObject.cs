namespace SpaceGame.BaseClasses
{
    public enum MapObjectType
    {
        None = 0,
        Planet = 1,
        HomePlanet = 2,
        LandingZone = 3,
        Mountain = 4
    }

    internal class MapObject
    {
        public string Name { get; set; }
        public MapObjectType Type { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool IsStartPosition { get; set; }

        public MapObject(string name, MapObjectType type, string label, string description, bool isStartPosition)
        {
            Name = name;
            Type = type;
            Label = label;
            Description = description;
            IsStartPosition = isStartPosition;
        }
    }
}
