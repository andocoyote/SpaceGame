using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.BaseClasses
{
    public enum MapObjectType
    {
        None = 0,
        Planet = 1,
        HomePlanet = 2,
        LandingZone = 3
    }

    internal class MapObject
    {
        public string Name { get; private set; }
        public MapObjectType Type { get; private set; }
        public string Label { get; private set; }
        public string Description { get; private set; }

        public MapObject(string name, MapObjectType type, string label, string description)
        {
            Name = name;
            Type = type;
            Label = label;
            Description = description;
        }
    }
}
