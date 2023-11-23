using System;

namespace BringBackPings;

internal class MessageGenerator
{
    public string PingMSGCreator(pingData PingData, double time)
    {
        try
        {
            TimeSpan timeSpan;
            switch (PingData.ColumnHeader)
            {
                case "Champion":
                    return $"{PingData.champdata.SummonerName} - {PingData.champdata.ChampionName}";
                case "Summoner Name":
                    return $"{PingData.champdata.ChampionName} - {PingData.champdata.SummonerName}";
                case "Level":
                    return $"{PingData.champdata.ChampionName} - Level {PingData.CellValue}";
                case "Total inventory":
                    return
                        $"{PingData.champdata.ChampionName} - Inventory worth {PingData.champdata.TotalItemCost} Gold";
                case "Summoner Spell D":
                    return
                        $"{PingData.champdata.ChampionName} - {PingData.champdata.SummonerSpells.SummonerSpellOne.DisplayName}";
                case "Summoner Spell F":
                    return
                        $"{PingData.champdata.ChampionName} - {PingData.champdata.SummonerSpells.SummonerSpellTwo.DisplayName}";
                case "Dead":
                    return
                        $"{PingData.champdata.ChampionName} - {(Convert.ToBoolean(PingData.CellValue) ? "Dead" : "Alive")}";
                case "set CD F":
                    timeSpan = TimeSpan.FromSeconds(time + PingData.champdata.SummonerSpells.SummonerSpellTwo.Cooldown);
                    return
                        $"{PingData.champdata.ChampionName} - {PingData.champdata.SummonerSpells.SummonerSpellTwo.DisplayName} is on cooldown until {(int)timeSpan.TotalMinutes:D2}:{timeSpan.Seconds:D2}";
                case "set CD D":
                    timeSpan = TimeSpan.FromSeconds(time + PingData.champdata.SummonerSpells.SummonerSpellOne.Cooldown);
                    return
                        $"{PingData.champdata.ChampionName} - {PingData.champdata.SummonerSpells.SummonerSpellOne.DisplayName} is on cooldown until {(int)timeSpan.TotalMinutes:D2}:{timeSpan.Seconds:D2}";
                case "Respawn time":
                    return
                        $"{PingData.champdata.ChampionName} -  {(PingData.champdata.respawnTimer > 0.1 ? "Respawning in " + PingData.champdata.respawnTimer + " Seconds" : "Alive")}";
                case "Passive":
                    return $"{PingData.champdata.ChampionName} - {PingData.ColumnHeader}";
                case "Q Ability":
                    return $"{PingData.champdata.ChampionName} - {PingData.ColumnHeader}";
                case "W Ability":
                    return $"{PingData.champdata.ChampionName} - {PingData.ColumnHeader}";
                case "E Ability":
                    return $"{PingData.champdata.ChampionName} - {PingData.ColumnHeader}";
                case "R Ability":
                    return $"{PingData.champdata.ChampionName} - {PingData.ColumnHeader}";
                default:
                    return "Unknown column";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public class pingData
    {
        public Livedata.ChampionData champdata { get; set; }
        public string ColumnHeader { get; set; }
        public object CellValue { get; set; }
    }

}