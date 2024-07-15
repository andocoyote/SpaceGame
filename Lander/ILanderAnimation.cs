namespace SpaceGame.Lander
{
    internal interface ILanderAnimation
    {
        (int, int) Position { get; set; }
        public string[] AnimationText { get; set; }
        int RowCount { get; }

        void Build();
        void Display();
        void MoveToRow(int row);
    }
}