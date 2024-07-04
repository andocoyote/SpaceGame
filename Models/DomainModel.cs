namespace SpaceGame.Models
{
    internal class DomainModel
    {
        public GameState GameState { get; set; }
        public LanderProperties LanderProperties { get; set; } = new LanderProperties();
    }
}
