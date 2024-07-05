using SpaceGame.Interfaces;
using SpaceGame.Models;

namespace SpaceGame.Planet
{
    internal class PlanetLoop : IScenario
    {
        private DomainModel _domainModel;

        public PlanetLoop(
            DomainModel domainModel)
        {
            _domainModel = domainModel;
        }

        public DomainModel Run()
        {
            return _domainModel;
        }
    }
}
