using SpaceGame.Interfaces;
using SpaceGame.Models;

namespace SpaceGame.Maps
{
    internal class PlanetMap : IMap
    {
        public PlanetMap()
        {

        }

        public (int, int) Position { get; set; }

        public void Build(int height, int width)
        {

        }

        public void Display()
        {

        }

        public GameState GetState()
        {
            return GameState.None;
        }
    }
}
