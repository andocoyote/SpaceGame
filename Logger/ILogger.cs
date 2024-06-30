namespace SpaceGame.Logger
{
    internal interface ILogger
    {
        void DebugPrint(string msg);
        void DebugPrintLine(string msg);
    }
}