namespace SpaceGame.Art
{
    internal static class SplashPage
    {
        public static void InitializeSplashPage(string[,] splashPage)
        {
            // Initialize all cells with spaces
            for (int i = 0; i < splashPage.GetLength(0); i++)
            {
                for (int j = 0; j < splashPage.GetLength(1); j++)
                {
                    splashPage[i, j] = " ";
                }
            }

            // Add spaceship
            string[] spaceship = {
                "            _",
                "           /^\\",
                "          /___\\",
                "         |=   =|",
                "         |     |",
                "         |     |",
                "         |     |",
                "         |_____|",
                "        /|_____|\\",
                "       /_|_____|_\\",
                "      //|   |   |\\\\",
                "     // |   |   | \\\\",
                "    //  |   |   |  \\\\",
                "   //   |   |   |   \\\\",
                "  //    |   |   |    \\\\",
                " //     |   |   |     \\\\",
                "//      |   |   |      \\\\",
                "        |   |   |",
                "        |   |   |",
                "        |___|___|",
            };

            int spaceshipRow = 20;
            foreach (string line in spaceship)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    splashPage[spaceshipRow, 40 + i] = line[i].ToString();
                }
                spaceshipRow++;
            }
        }

        public static void DisplaySplashPage(string[,] splashPage)
        {
            for (int i = 0; i < splashPage.GetLength(0); i++)
            {
                for (int j = 0; j < splashPage.GetLength(1); j++)
                {
                    Console.Write(splashPage[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
