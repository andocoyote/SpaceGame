using SpaceGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpaceGame.Inventory
{
    internal class Inventory
    {
        // TODO: this file needs to be persisted somewhere so it's not blown away on 'clean build'
        private const string INVENTORY_FILENAME = "inventory.json";
        private const int STARTING_HEALTH = 1000;
        private const int STARTING_MONEY = 5000;
        private const int STARTING_FUEL = 15000;

        private InventoryModel? _inventoryModel;

        public Inventory()
        {
            // Determine if there's an InventoryModel already saved to disk
            bool inventoryExists = File.Exists(INVENTORY_FILENAME);

            // If an InventoryModel exists, load it
            if (inventoryExists)
            {
                LoadInventory().GetAwaiter().GetResult();
            }
            // Else, create a new InventoryModel with default values
            else
            {
                _inventoryModel = new InventoryModel();

                _inventoryModel.Health = STARTING_HEALTH;
                _inventoryModel.Money = STARTING_MONEY;
                _inventoryModel.Fuel = STARTING_FUEL;
                _inventoryModel.Items = new Dictionary<string, int>();
            }
        }

        public void DisplayInventory()
        {
            if (_inventoryModel == null || _inventoryModel.Items == null) return;

            Console.WriteLine($"Health: {_inventoryModel.Health}");
            Console.WriteLine($"Money: {_inventoryModel.Money}");
            Console.WriteLine($"Fuel: {_inventoryModel.Fuel}");

            foreach (KeyValuePair<string, int> item in _inventoryModel.Items)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        public void SaveInventory()
        {
            if (_inventoryModel == null) return;

            string serializedInventory = SerializeModel(_inventoryModel);

            using (StreamWriter writer = new StreamWriter(INVENTORY_FILENAME, append:false))
            {
                writer.WriteLine(serializedInventory);
            }
        }

        public async Task LoadInventory()
        {
            using Task<string> readTask = File.ReadAllTextAsync(INVENTORY_FILENAME);

            string lines = await readTask;

            _inventoryModel = DeserializeModel(lines);
        }

        public string SerializeModel(InventoryModel model)
        {
            string modelSerialized = JsonSerializer.Serialize(model);

            return modelSerialized;
        }
        public InventoryModel? DeserializeModel(string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                return null;
            }

            InventoryModel? inventoryModelDeserialized = JsonSerializer.Deserialize<InventoryModel>(model);

            return inventoryModelDeserialized;
        }
    }
}
