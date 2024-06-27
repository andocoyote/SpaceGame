namespace SpaceGame
{
    internal interface IMap
    {
        (int, int) ShipPosition { get; set; }

        void Build(int height, int width);
        void Display();
    }
}