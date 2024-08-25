using SpaceGame.BaseClasses;

namespace SpaceGame.Models
{
    public enum PlanetMapStateEnum
    {
        None,
        ExitGame,
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

    public class PlanetMapState : BaseState<PlanetMapState, PlanetMapStateEnum>
    {
        private PlanetMapState(PlanetMapStateEnum state) : base(state) { }

        public static readonly PlanetMapState None = new PlanetMapState(PlanetMapStateEnum.None);
        public static readonly PlanetMapState ExitGame = new PlanetMapState(PlanetMapStateEnum.ExitGame);
        public static readonly PlanetMapState InitiateHomePlanetLanding = new PlanetMapState(PlanetMapStateEnum.InitiateHomePlanetLanding);
        public static readonly PlanetMapState ShipCrashed = new PlanetMapState(PlanetMapStateEnum.ShipCrashed);
        public static readonly PlanetMapState OverPlanet = new PlanetMapState(PlanetMapStateEnum.OverPlanet);
        public static readonly PlanetMapState InitiatePlanetLanding = new PlanetMapState(PlanetMapStateEnum.InitiatePlanetLanding);
        public static readonly PlanetMapState OnLandingZone = new PlanetMapState(PlanetMapStateEnum.OnLandingZone);
        public static readonly PlanetMapState InitiateDocking = new PlanetMapState(PlanetMapStateEnum.InitiateDocking);
        public static readonly PlanetMapState InFight = new PlanetMapState(PlanetMapStateEnum.InFight);
        public static readonly PlanetMapState LanderCrashed = new PlanetMapState(PlanetMapStateEnum.LanderCrashed);
        public static readonly PlanetMapState EmtpyLand = new PlanetMapState(PlanetMapStateEnum.EmtpyLand);
        public static readonly PlanetMapState OverItem = new PlanetMapState(PlanetMapStateEnum.OverItem);
        public static readonly PlanetMapState InspectItem = new PlanetMapState(PlanetMapStateEnum.InspectItem);
    }
}
