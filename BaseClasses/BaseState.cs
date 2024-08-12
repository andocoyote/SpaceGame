namespace SpaceGame.BaseClasses
{
    public abstract class BaseState<T, TEnum> where T : BaseState<T, TEnum>
    {
        public TEnum State { get; }

        protected BaseState(TEnum state)
        {
            State = state;
        }

        public override string? ToString() => State?.ToString();
    }
}
