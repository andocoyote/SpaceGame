using SpaceGame.BaseClasses;

namespace SpaceGame.Models
{
    public enum SpaceMapStateEnum
    {
        None,
        ExitGame,
        EmtpySpace,
        OverHomePlanet,
        OnHomePlanet,
        InitiateHomePlanetLanding,
        ShipCrashed,
        OverPlanet,
        InitiatePlanetLanding
    }
    public class SpaceMapState : BaseState<SpaceMapState, SpaceMapStateEnum>
    {
        private SpaceMapState(SpaceMapStateEnum state) : base(state) { }

        public static readonly SpaceMapState None = new SpaceMapState(SpaceMapStateEnum.None);
        public static readonly SpaceMapState ExitGame = new SpaceMapState(SpaceMapStateEnum.ExitGame);
        public static readonly SpaceMapState EmtpySpace = new SpaceMapState(SpaceMapStateEnum.EmtpySpace);
        public static readonly SpaceMapState OverHomePlanet = new SpaceMapState(SpaceMapStateEnum.OverHomePlanet);
        public static readonly SpaceMapState OnHomePlanet = new SpaceMapState(SpaceMapStateEnum.OnHomePlanet);
        public static readonly SpaceMapState InitiateHomePlanetLanding = new SpaceMapState(SpaceMapStateEnum.InitiateHomePlanetLanding);
        public static readonly SpaceMapState ShipCrashed = new SpaceMapState(SpaceMapStateEnum.ShipCrashed);
        public static readonly SpaceMapState OverPlanet = new SpaceMapState(SpaceMapStateEnum.OverPlanet);
        public static readonly SpaceMapState InitiatePlanetLanding = new SpaceMapState(SpaceMapStateEnum.InitiatePlanetLanding);
    }
}
