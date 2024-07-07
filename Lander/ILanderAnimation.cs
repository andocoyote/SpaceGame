namespace SpaceGame.Lander
{
    internal interface ILanderAnimation
    {
        (int, int) Position { get; set; }
        int RowCount { get; }

        void Build();
        void Display();
        void MoveToRow(int row);
    }
}