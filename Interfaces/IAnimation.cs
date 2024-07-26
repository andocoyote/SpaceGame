namespace SpaceGame.Interfaces
{
    internal interface IAnimation
    {
        (int, int) Position { get; set; }
        int RowCount { get; }

        void Build();
        void Display();
        void MoveToRow(int row);
    }
}