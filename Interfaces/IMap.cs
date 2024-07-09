using SpaceGame.BaseClasses;
using SpaceGame.Models;

namespace SpaceGame.Interfaces
{
    internal interface IMap
    {
        (int, int) Position { get; }
        MapObject? ObjectAtPosition { get; }
        Dictionary<string, MapObject> ObjectDictionary { get; }

        void Build(int height, int width);
        void Display();
        void SetPlayerPosition((int, int) currentPlayerPosition);

        GameState GetState();
    }
}