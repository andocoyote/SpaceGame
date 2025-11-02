using SpaceGame.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Models
{
    internal class InventoryModel
    {
        private const int STARTING_HEALTH = 1000;
        private const int STARTING_MONEY = 5000;
        private const int STARTING_FUEL = 15000;

        public int Health { get; set; } = STARTING_HEALTH;
        public int Money { get; set; } = STARTING_MONEY;
        public int Fuel { get; set; } = STARTING_FUEL;

        // InventoryItems and count of each item
        public List<InventoryItem>? PlayerInventory { get; set; } = new();
        public List<InventoryItem>? ShipInventory { get; set; } = new();
        public List<InventoryItem>? LanderInventory { get; set; } = new();
    }
}
