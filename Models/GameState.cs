namespace SpaceGame.Models
{
    public enum GameState
    {
        None = 0,
        ExitGame = 1,
        EmtpySpace = 2,
        OverPlanet = 3,
        InitiateLanding = 4,
        OnLandingZone = 5,
        InitiateDocking = 6,
        InFight = 7,
        LanderCrashed = 8,
        EmtpyLand = 9,
        OverItem = 10,
        InspectItem = 11
    }
}
