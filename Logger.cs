using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    internal class Logger : ILogger
    {
        private const bool _debugPrint = true;

        public void DebugPrint(string msg)
        {
            if (_debugPrint)
            {
#pragma warning disable CS0162 // Unreachable code detected
                Console.Write(msg);
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        public void DebugPrintLine(string msg)
        {
            if (_debugPrint)
            {
#pragma warning disable CS0162 // Unreachable code detected
                Console.WriteLine(msg);
#pragma warning restore CS0162 // Unreachable code detected
            }
        }
    }
}
