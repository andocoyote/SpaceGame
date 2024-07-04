namespace SpaceGame.Loggers
{
    internal interface ILogger
    {
        void DebugPrint(string msg);
        void DebugPrintLine(string msg);
    }
}