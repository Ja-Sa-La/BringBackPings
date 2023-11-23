using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BringBackPings
{
    class Dataholder
    {
        public static List<Item> Items { get; private set; } = new List<Item>();
        private bool loaded = false;
        public async Task LoadData()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/items.json");
                    response.EnsureSuccessStatusCode(); // Ensure a successful response

                    string jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Items = JsonConvert.DeserializeObject<List<Item>>(jsonContent);
                }

                loaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        public int GetPrice(string itemName)
        {
            if (!loaded)
                LoadData();

            Item firstMatchingItem = Items.FirstOrDefault(item => item.name?.Equals(itemName, StringComparison.OrdinalIgnoreCase) == true);
            if (firstMatchingItem != null)
            {
                return firstMatchingItem.priceTotal;
            }
            return 0;
        }

        public class Item
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public bool active { get; set; }
            public bool inStore { get; set; }
            public int?[] from { get; set; }
            public int?[] to { get; set; }
            public string[] categories { get; set; }
            public int maxStacks { get; set; }
            public string requiredChampion { get; set; }
            public string requiredAlly { get; set; }
            public string requiredBuffCurrencyName { get; set; }
            public int requiredBuffCurrencyCost { get; set; }
            public int specialRecipe { get; set; }
            public bool isEnchantment { get; set; }
            public int price { get; set; }
            public int priceTotal { get; set; }
            public string iconPath { get; set; }
        }
    }
}
