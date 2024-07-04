namespace SpaceGame.Interfaces
{
    internal interface IMap
    {
        (int, int) Position { get; set; }

        void Build(int height, int width);
        void Display();

        State GetState();
    }
}