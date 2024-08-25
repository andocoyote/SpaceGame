using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Models
{
    internal class InventoryModel
    {
        public int Health { get; set; }
        public int Money { get; set; }
        public int Fuel { get; set; }
        public Dictionary<string, int>? Items { get; set; }
    }
}
