using SpaceGame.BaseClasses;

namespace SpaceGame.Models
{
    public enum LanderStateEnum
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

    public class LanderState : BaseState<LanderState, LanderStateEnum>
    {
        private LanderState(LanderStateEnum state) : base(state) { }

        public static readonly LanderState None = new LanderState(LanderStateEnum.None);
        public static readonly LanderState Landed = new LanderState(LanderStateEnum.Landed);
        public static readonly LanderState Docked = new LanderState(LanderStateEnum.Docked);
        public static readonly LanderState Flying = new LanderState(LanderStateEnum.Flying);
        public static readonly LanderState OutOfFuel = new LanderState(LanderStateEnum.OutOfFuel);
        public static readonly LanderState Crashed = new LanderState(LanderStateEnum.Crashed);
        public static readonly LanderState Landing = new LanderState(LanderStateEnum.Landing);
        public static readonly LanderState Docking = new LanderState(LanderStateEnum.Docked);
    }
    }
