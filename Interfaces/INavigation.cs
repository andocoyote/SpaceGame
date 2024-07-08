using SpaceGame.Models;

namespace SpaceGame.Interfaces
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