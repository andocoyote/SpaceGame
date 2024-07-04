using SpaceGame.Models;

namespace SpaceGame.Navigation
{
    internal interface INavigation
    {
        GameState MoveDown();
        GameState MoveLeft();
        GameState MoveRight();
        GameState MoveUp();
        void DisplayMap();
    }
}