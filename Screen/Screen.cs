using Microsoft.Extensions.Options;

namespace SpaceGame.Screen
{
    internal class Screen : IScreen
    {
        // Height and width of the entire display
        protected int SCREEN_HEIGHT;
        protected int SCREEN_WIDTH;

        // Height and width of the array portion (left two-thirds)
        protected int GRAPHICS_HEIGHT;
        protected int GRAPHICS_WIDTH;

        // Height and width of the text portion (right third)
        protected int TEXT_HEIGHT;
        protected int TEXT_WIDTH;

        protected char[,]? _graphics = null;
        protected string[]? _text;
        protected IOptions<ScreenOptions> _screenOptions;

        // TODO: is char[] more efficient to write to console than string for this?
        protected List<char[]> _aggregatedDisplay;

        public Screen(
            IOptions<ScreenOptions> screenOptions)
        {
            _screenOptions = screenOptions;

            SCREEN_HEIGHT = _screenOptions.Value.ScreenHeight;
            SCREEN_WIDTH = _screenOptions.Value.ScreenWidth;
            GRAPHICS_HEIGHT = _screenOptions.Value.GraphicsHeight;
            GRAPHICS_WIDTH = _screenOptions.Value.GraphicsWidth;
            TEXT_HEIGHT = _screenOptions.Value.TextHeight;
            TEXT_WIDTH = _screenOptions.Value.TextWidth;

            _aggregatedDisplay = new List<char[]>(SCREEN_HEIGHT);

            // Create a char array to store the row characters
            for (int i = 0; i < SCREEN_HEIGHT; i++)
            {
                _aggregatedDisplay.Add(new char[SCREEN_WIDTH]);
            }
        }

        // Use this method to update the array contained in the left two-thirds of the screen
        public void AddGraphics(char[,] graphics)
        {
            _graphics = graphics;
        }

        // Use this method to update the text contained in the right third of the screen
        public void AddText(string[] text)
        {
            _text = text;
        }

        // Aggregate and display the screen (array + text)
        public void Display()
        {
            if (_graphics == null)
            {
                return;
            }

            Console.Clear();

            CreateGraphicsDisplay();
            CreateTextDisplay();

            // Converting the char array to strings is costly, so only do it displaying
            for (int i = 0; i < _aggregatedDisplay.Count; i++)
            {
                // Write the row as a single string
                Console.WriteLine(_aggregatedDisplay[i]);
            }
        }

        // Adds the graphics to the text portion (right-most third) of the screen
        private void CreateGraphicsDisplay()
        {
            if (_graphics == null) return;

            // Converting the char array to strings is costly, so only do it displaying
            for (int i = 0; i < GRAPHICS_HEIGHT; i++)
            {
                // TODO: Only need to do this for the left two-thirds of the screen, not SCREEN_WIDTH
                for (int j = 0; j < GRAPHICS_WIDTH; j++)
                {
                    _aggregatedDisplay[i][j] = _graphics[i, j];

                }
            }
        }

        // Adds the text to the text portion (right-most third) of the screen
        private void CreateTextDisplay()
        {
            if (_text == null) return;

            // Converting the char array to strings is costly, so only do it displaying
            for (int i = 0; i < TEXT_HEIGHT; i++)
            {
                if (_text[i] != null && _text[i].Length > 0)
                {
                    // Start from the end of the graphics section and go to the end of the text section
                    for (int j = GRAPHICS_WIDTH; j < GRAPHICS_WIDTH + _text[i].Length; j++)
                    {
                        _aggregatedDisplay[i][j] = _text[i][j- GRAPHICS_WIDTH];
                    }
                }
            }
        }
    }
}
