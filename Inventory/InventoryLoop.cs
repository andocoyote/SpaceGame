using SpaceGame.BaseClasses;
using SpaceGame.DomainModelService;
using SpaceGame.Interfaces;
using SpaceGame.Loggers;
using SpaceGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpaceGame.Inventory
{
    internal class InventoryLoop : IScenario
    {
        private DomainModelService.DomainModelService _domainModelService;
        private ILogger _logger;
        private DomainModel _domainModel;
        private Store _store;

        public InventoryLoop(
            DomainModelService.DomainModelService domainModelService,
            DomainModel domainModel,
            Store store,
            ILogger logger)
        {
            _domainModelService = domainModelService;
            _domainModel = domainModel;
            _store = store;
            _logger = logger;
        }

        public DomainModel Run()
        {
            int selection;
            bool exit = false;

            Console.Clear();

            do
            {
                selection = UserMenu();

                switch (selection)
                {
                    case 0:
                        exit = true;
                        break;

                    case 1:
                        DisplayInventory();
                        break;

                    case 2:
                        DisplayStore();
                        break;

                    case 3:
                        // Buy items
                        bool itemPurchased = BuyItem();

                        if (!itemPurchased)
                        {
                            Console.WriteLine("Failed to purchase the item. Check your available funds.");
                        }
                        else
                        {
                            Console.WriteLine("Item purchased.");
                            _domainModelService.SaveDomainModelAsync().GetAwaiter().GetResult();
                        }
                        
                        break;

                    case 4:
                        // Sell items
                        bool itemSold = SellItem();

                        if (!itemSold)
                        {
                            Console.WriteLine("Failed to sell the item. Check your available inventory.");
                        }
                        else
                        {
                            Console.WriteLine("Item sold.");
                            _domainModelService.SaveDomainModelAsync().GetAwaiter().GetResult();
                        }

                        break;

                    default:
                        break;
                }
            } while (!exit);

            return _domainModel;
        }

        private void DisplayInventory()
        {
            if (_domainModel == null ||
                _domainModel.InventoryModel == null ||
                _domainModel.InventoryModel?.PlayerInventory == null ||
                _domainModel.InventoryModel?.ShipInventory == null ||
                _domainModel.InventoryModel?.LanderInventory == null)
            {
                return;
            }

            Console.WriteLine($"Health: {_domainModel.InventoryModel.Health}");
            Console.WriteLine($"Money: {_domainModel.InventoryModel.Money}");
            Console.WriteLine($"Fuel: {_domainModel.InventoryModel.Fuel}");

            foreach (InventoryItem item in _domainModel.InventoryModel.PlayerInventory)
            {
                Console.WriteLine(item.Name);
            }

            foreach (InventoryItem item in _domainModel.InventoryModel.ShipInventory)
            {
                Console.WriteLine(item.Name);
            }

            foreach (InventoryItem item in _domainModel.InventoryModel.LanderInventory)
            {
                Console.WriteLine(item.Name);
            }
        }

        private void DisplayStore()
        {
            _store.ListItems();
        }

        private bool BuyItem()
        {
            if (_domainModel == null ||
                _domainModel.InventoryModel == null ||
                _domainModel.InventoryModel?.PlayerInventory == null ||
                _domainModel.InventoryModel?.ShipInventory == null ||
                _domainModel.InventoryModel?.LanderInventory == null)
            {
                return false;
            }

            bool itemPurchased = false;
            int choice;
            InventoryItem? item = null;

            // Determine which item the player wants to buy
            do
            {
                Console.Write("Enter the item number you want to buy (0 to leave): ");

                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.Write($"Invalid selction. Please try again: ");
                }

                if (choice == 0)
                {
                    return false;
                }

                item = _store.GetItem(choice);

            } while (item == null);

            // Check if got enough money to purchase
            if (_domainModel.InventoryModel.Money >= item.Value)
            {
                switch (item.ItemCategory)
                {
                    case ItemCategory { } none when none == ItemCategory.None:
                        // TODO: what to do with 'other' items?
                        break;
                    case ItemCategory { } playerWeapon when playerWeapon == ItemCategory.PlayerWeapon:
                    case ItemCategory { } playerArmor when playerArmor == ItemCategory.PlayerArmor:
                        _domainModel.InventoryModel.PlayerInventory.Add(item);
                        break;
                    case ItemCategory { } shipWeapon when shipWeapon == ItemCategory.ShipWeapon:
                    case ItemCategory { } shipArmor when shipArmor == ItemCategory.ShipArmor:
                        _domainModel.InventoryModel.ShipInventory.Add(item);
                        break;
                    case ItemCategory { } landerWeapon when landerWeapon == ItemCategory.LanderWeapon:
                    case ItemCategory { } landerArmor when landerArmor == ItemCategory.LanderArmor:
                        _domainModel.InventoryModel.LanderInventory.Add(item);
                        break;
                    default:
                        break;
                }

                // Update available funds
                _domainModel.InventoryModel.Money -= item.Value;
                itemPurchased = true;
            }

            return itemPurchased;
        }

        private bool SellItem()
        {
            if (_domainModel == null ||
                _domainModel.InventoryModel == null ||
                _domainModel.InventoryModel?.PlayerInventory == null ||
                _domainModel.InventoryModel?.ShipInventory == null ||
                _domainModel.InventoryModel?.LanderInventory == null)
            {
                return false;
            }

            bool itemSold = false;
            string? choice;
            InventoryItem? item = null;

            // Determine which item the player wants to sell
            Console.Write("Enter the name of the item you want to sell (0 to leave): ");
            choice = Console.ReadLine();

            while (string.IsNullOrEmpty(choice))
            {
                Console.Write($"Invalid selction. Please try again: ");
            }

            if (choice == "0")
            {
                return false;
            }

            // See if the player owns the item


            // Update available funds
            if (item?.Name != null)
            {
                // Delete the item
                _domainModel.InventoryModel.Money += item.Value;
                itemSold = true;
            }

            return itemSold;
        }

        private int UserMenu()
        {
            int choice;

            Console.WriteLine("Please choose from the following options:");
            Console.WriteLine("1. View inventory");
            Console.WriteLine("2. View store");
            Console.WriteLine("3. Buy items");
            Console.WriteLine("4. Sell items");
            Console.WriteLine("0. Exit inventory");
            Console.WriteLine("Enter your choice: ");

            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.Write($"Invalid selction. Please try again: ");
            }

            return choice;
        }
    }
}
