namespace SpaceGame.Models
{
    public enum GameState
    {
        None,
        ExitGame,
        EmtpySpace,
        OverHomePlanet,
        OnHomePlanet,
        InitiateHomePlanetLanding,
        ShipCrashed,
        OverPlanet,
        InitiatePlanetLanding,
        OnLandingZone,
        InitiateDocking,
        InFight,
        LanderCrashed,
        EmtpyLand,
        OverItem,
        InspectItem
    }
}
