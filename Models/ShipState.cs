using SpaceGame.BaseClasses;

namespace SpaceGame.Models
{
    public enum ShipStateEnum
    {
        None,
        Landed,
        Docked,
        Flying,
        OutOfFuel,
        Crashed,
        Landing,
        Docking
    }

    public class ShipState : BaseState<ShipState, ShipStateEnum>
    {
        private ShipState(ShipStateEnum state) : base(state) { }

        public static readonly ShipState None = new ShipState(ShipStateEnum.None);
        public static readonly ShipState Landed = new ShipState(ShipStateEnum.Landed);
        public static readonly ShipState Docked = new ShipState(ShipStateEnum.Docked);
        public static readonly ShipState Flying = new ShipState(ShipStateEnum.Flying);
        public static readonly ShipState OutOfFuel = new ShipState(ShipStateEnum.OutOfFuel);
        public static readonly ShipState Crashed = new ShipState(ShipStateEnum.Crashed);
        public static readonly ShipState Landing = new ShipState(ShipStateEnum.Landing);
        public static readonly ShipState Docking = new ShipState(ShipStateEnum.Docking);
    }
}
