namespace SpaceGame
{
    internal interface INavigation
    {
        State MoveDown();
        State MoveLeft();
        State MoveRight();
        State MoveUp();
    }
}