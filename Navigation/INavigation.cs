using SpaceGame.Models;

namespace SpaceGame.Navigation
{
    internal interface INavigation
    {
        State MoveDown();
        State MoveLeft();
        State MoveRight();
        State MoveUp();
        void DisplayMap();
    }
}