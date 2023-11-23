using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BringBackPings;

internal class Livedata
{
    public Gamedata gamedata = new();
    public List<ChampionData> LiveDataStorage = new();
    public List<ChampionData> LiveDataStorageChaos = new();
    public List<ChampionData> LiveDataStorageOrder = new();



    public async Task<bool> UpdateData()
    {
        try
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            var client = new HttpClient(clientHandler);
            var response = await client.GetAsync("https://127.0.0.1:2999/liveclientdata/playerlist");
            var responseBody2 = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            LiveDataStorage = JsonConvert.DeserializeObject<List<ChampionData>>(responseBody2);
            // Extract players from Team Chaos
            LiveDataStorageChaos = LiveDataStorage.Where(player => player.Team == "CHAOS").ToList();
            // Extract players from Team Order
            LiveDataStorageOrder = LiveDataStorage.Where(player => player.Team == "ORDER").ToList();
            response = await client.GetAsync("https://127.0.0.1:2999/liveclientdata/gamestats");
            responseBody2 = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            gamedata = JsonConvert.DeserializeObject<Gamedata>(responseBody2);
            return true;
        }
        catch (Exception a)
        {
            throw;
        }
    }


    private static string RemoveDiacritics(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var normalizedString = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }


    public class Gamedata
    {
        public string gameMode { get; set; }
        public float gameTime { get; set; }
        public string mapName { get; set; }
        public int mapNumber { get; set; }
        public string mapTerrain { get; set; }

        public string GetFormattedGameTime()
        {
            var timeSpan = TimeSpan.FromSeconds(gameTime);
            return $"{(int)timeSpan.TotalMinutes:D2}:{timeSpan.Seconds:D2}";
        }
    }

    public class Item
    {
        public bool CanUse { get; set; }
        public bool Consumable { get; set; }
        public int Count { get; set; }
        public string DisplayName { get; set; }
        public int ItemID { get; set; }
        public int Price { get; set; }
        public string RawDescription { get; set; }
        public string RawDisplayName { get; set; }
        public int Slot { get; set; }
    }

    public class KeystoneRune
    {
        public string DisplayName { get; set; }
        public int ID { get; set; }
        public string RawDescription { get; set; }
        public string RawDisplayName { get; set; }
    }

    public class PrimaryRuneTree
    {
        public string DisplayName { get; set; }
        public int ID { get; set; }
        public string RawDescription { get; set; }
        public string RawDisplayName { get; set; }
    }

    public class SecondaryRuneTree
    {
        public string DisplayName { get; set; }
        public int ID { get; set; }
        public string RawDescription { get; set; }
        public string RawDisplayName { get; set; }
    }

    public class Runes
    {
        public KeystoneRune Keystone { get; set; }
        public PrimaryRuneTree PrimaryRuneTree { get; set; }
        public SecondaryRuneTree SecondaryRuneTree { get; set; }
    }

    public class SummonerSpell
    {
        public string DisplayName { get; set; }
        public string RawDescription { get; set; }
        public string RawDisplayName { get; set; }
        public double Cooldown
        {
            get { return Cooldowns.GetCooldown(DisplayName.Replace(" ", "")); }
        }
        public string CD { get; set; } = "set CD";
    }

    public class SummonerSpells
    {
        public SummonerSpell SummonerSpellOne { get; set; }
        public SummonerSpell SummonerSpellTwo { get; set; }
    }

    public class Scores
    {
        public int Assists { get; set; }
        public int CreepScore { get; set; }
        public int Deaths { get; set; }
        public int Kills { get; set; }
        public double WardScore { get; set; }
    }

    public class ChampionData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Dataholder dataholder = new Dataholder();

        private double RespawnTimer;
        public string ChampionName { get; set; }
        public bool IsBot { get; set; }
        public bool IsDead { get; set; }
        public List<Item> Items { get; set; }

        public int TotalItemCost
        {
            get
            {
                return Items.Sum(item => dataholder.GetPrice(item.DisplayName) * item.Count);
            }
        }

        public int Level { get; set; }
        public string Position { get; set; }
        public string RawChampionName { get; set; }

        public double respawnTimer
        {
            get => Math.Round(RespawnTimer, 1);
            set => RespawnTimer = Math.Round(value, 1);
        }

        public Runes Runes { get; set; }
        public Scores Scores { get; set; }
        public int SkinID { get; set; }
        public string SummonerName { get; set; }
        public SummonerSpells SummonerSpells { get; set; }
        public string Team { get; set; }
        public string P { get; set; } = "Ping";
        public string Q { get; set; } = "Ping";
        public string W { get; set; } = "Ping";
        public string E { get; set; } = "Ping";
        public string R { get; set; } = "Ping";

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Cooldowns
    {
        private static Dictionary<string, double> cooldowns = new Dictionary<string, double>
        {
            { "Teleport", 360 },
            { "Flash", 300 },
            { "Clarity", 240 },
            { "Heal", 240 },
            { "UnleashedTeleport", 240 },
            { "Cleanse", 210 },
            { "Exhaust", 210 },
            { "Ghost", 210 },
            { "Barrier", 180 },
            { "Ignite", 180 },
            { "Smite", 90 },
            { "UnleashedSmite", 90 }
        };

        public static double GetCooldown(string displayName)
        {
            // Check if the display name is in the dictionary
            if (cooldowns.ContainsKey(displayName))
            {
                // Return the cooldown based on the display name
                return cooldowns[displayName];
            }
            else
            {
                // Return a default cooldown value if the display name is not found
                return 0; // You can return a default value or handle it in another way
            }
        }
    }


}