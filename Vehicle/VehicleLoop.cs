using SpaceGame.Interfaces;
using SpaceGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Vehicle
{
    internal class VehicleLoop : IScenario
    {
        private DomainModel _domainModel;

        public VehicleLoop(
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
