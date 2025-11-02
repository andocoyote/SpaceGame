using Microsoft.Extensions.Options;
using SpaceGame.BaseClasses;
using SpaceGame.DomainModelService;
using SpaceGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpaceGame.Inventory
{
    internal class Store
    {
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters =
            {
                new ItemCategoryConverter()
            }
        };

        private List<InventoryItem> Items;

        public Store()
        {
            string readJson = File.ReadAllText("Inventory\\merchandise.json");
            Items = JsonSerializer.Deserialize<List<InventoryItem>>(readJson, options) ?? new List<InventoryItem>();
        }

        public void ListItems()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Console.WriteLine($"[{i + 1}]: {Items[i].Name}");
                Console.WriteLine($"\t{Items[i].Description}");
                Console.WriteLine($"\tDamage: {Items[i].Damage} Protection: {Items[i].Protection} Value: {Items[i].Value}");
                Console.WriteLine();
            }
        }

        public InventoryItem? GetItem(int itemNum)
        {
            InventoryItem? item = null;

            if (Items.Count >= itemNum)
            {
                item = Items[itemNum - 1];
            }

            return item;
        }
    }

    public class ItemCategoryConverter : JsonConverter<ItemCategory>
    {
        // Serialize the static fields manually
        public override void Write(Utf8JsonWriter writer, ItemCategory value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("State", value.ToString());
            writer.WriteEndObject();
        }

        // Deserialize static fields (re-initialize them if necessary)
        public override ItemCategory? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Ensure we are at the start of the "PlanetMapState" object in the JSON
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            // Local variables to hold the deserialized values
            ItemCategory stateName = ItemCategory.None;

            // Read through the JSON object
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    // End of the object
                    break;
                }

                // Get the property name
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString() ?? string.Empty;

                    // Move to the property value
                    reader.Read();

                    if (propertyName == "State")
                    {
                        string propertyValue = reader.GetString() ?? string.Empty;

                        switch (propertyValue)
                        {
                            case nameof(ItemCategory.None):
                                stateName = ItemCategory.None;
                                break;
                            case nameof(ItemCategory.PlayerWeapon):
                                stateName = ItemCategory.PlayerWeapon;
                                break;
                            case nameof(ItemCategory.PlayerArmor):
                                stateName = ItemCategory.PlayerArmor;
                                break;
                            case nameof(ItemCategory.ShipWeapon):
                                stateName = ItemCategory.ShipWeapon;
                                break;
                            case nameof(ItemCategory.ShipArmor):
                                stateName = ItemCategory.ShipArmor;
                                break;
                            case nameof(ItemCategory.LanderWeapon):
                                stateName = ItemCategory.LanderWeapon;
                                break;
                            case nameof(ItemCategory.LanderArmor):
                                stateName = ItemCategory.LanderArmor;
                                break;
                            default:
                                stateName = ItemCategory.None;
                                break;
                        }
                    }
                }
            }

            return stateName;
        }
    }
}
