namespace SpaceGame.Screen
{
    // Creating this as an interface in case I do want to swap it out for different implementations for whatever reason
    internal interface IScreen
    {
        void AddGraphics(char[,] array);
        void AddText(string[] text);
        void Display();
    }
}