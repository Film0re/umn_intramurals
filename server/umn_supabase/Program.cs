using System.Text.Json;
using Fraxiinus.Rofl.Extract.Data;
using Fraxiinus.Rofl.Extract.Data.Models.Rofl;
using Postgrest;
using Postgrest.Exceptions;
using umn_supabase;
using umn_supabase.Models;

// Create a new class called "Program" in the namespace "umn_supabase"

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var url = APIKeysLocal.SupabaseUrl;
        var key = APIKeysLocal.SupabaseKey;
        replayFolder = APIKeysLocal.replayFolder;
        intramural_season = APIKeysLocal.season;
        
        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true
        };
        
        supabase = new Supabase.Client(url, key, options);
        await supabase.InitializeAsync();
        
        Console.WriteLine("Connected to Supabase!");
        
        // ASk user if they want to upload replays or get player info
        Console.WriteLine("Would you like to upload replays or get player info?");
        Console.WriteLine("1. Loop through replays in folder and upload to Supabase");
        Console.WriteLine("2. Get player info from Supabase");
        Console.WriteLine("3. Retrieve replays from Supabase and upload matches/match_data to Supabase");
        var input = Console.ReadLine();
        
        switch (input)
        {
            case "1":
                // Grab all the files in the replay folder
                var files = Directory.GetFiles(replayFolder);
                
                // Loop through each file in the folder iter
                var tasks = new List<Task>();
                foreach (var file in files)
                {
                    await processMatch(file);
                }
                break;
            case "2":
                await saveUsers();
                break;
            case "3":
                await getReplaysAndProcess();
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
        return 0;
    }

    public static async Task getReplaysAndProcess()
    {
        // List all replays in the queue folder
        var replays = await supabase.Storage.From("Replays").List("queue");

        foreach (var replay in replays)
        {
            Console.WriteLine(replay.Name);
        }

        // Task array to store all the tasks
        var tasks = new List<Task>();

        // Loop through each replay and download it
        foreach (var replay in replays)
        {
            // Check if the replay has already been downloaded
            if (File.Exists(replayFolder + replay.Name))
            {
                Console.WriteLine("Replay already downloaded");
                continue;
            }

            // Check if the replay ends with .rofl
            if (!replay.Name.EndsWith(".rofl"))
            {
                Console.WriteLine("Replay does not end with .rofl");
                continue;
            }

            var replayName = replay.Name;
            var replayId = replay.Id;
            var replayBucketId = replay.BucketId;

            Console.WriteLine("Downloading replay: " + replayName);

            // Download replay
            var replayBytes = await supabase.Storage.From("Replays").Download("queue/"+replayName, (bytes, progress) =>
            {
                Console.WriteLine($"Downloaded {progress}%");
            });

            // Save replay to folder
            File.WriteAllBytes(replayFolder + replayName, replayBytes);

            // Process the match
            var processResult = await processMatch(replayFolder + replayName);

            if (processResult)
            {
                // Move the successfully processed replay to the "processed" folder
                await supabase.Storage.From("Replays").Move("queue/" + replayName, "processed/" + replayName);
            }
        }

        // Wait for all tasks to finish
        await Task.WhenAll(tasks);
    }


    public static async Task<bool> processMatch(string fileName)
    { 
        var replay = await RoflReader.LoadAsync(fileName, loadAll: true);

        Console.WriteLine("Processing match: " + fileName);

        Metadata? metadata = replay.Metadata;

        if (metadata == null)
        {
            Console.WriteLine("No metadata found");
        }
        
        
        PlayerStats[] playerStats = metadata.PlayerStatistics;
        
        Player[] players = new Player[playerStats.Length];
        MatchStats[] matchStats = new MatchStats[playerStats.Length];
        Match match = GetMatch(metadata, replay.PayloadHeader);
        var teams = await GetTeams(metadata);
        Console.WriteLine("teams: " + teams.Item1 + " " + teams.Item2 + " " + teams.Item3);

        // Loop through each player in the match
        // Getting the player's account information from Riot
        // Creating a new Player object
        // Creating a new MatchStats object
        // Adding the player to the players array
        // Adding the match stats to the match stats 
        for (int i = 0; i < playerStats.Length; i++)
        {
            Player player = new Player
            {
                player_id = playerStats[i].PUUID,
                user_id = null,
                name = playerStats[i].Name,
            };

            MatchStats matchStat = GetPlayerMatchStats(playerStats[i], replay.PayloadHeader);
        
            matchStat.player_id = player.player_id;
            matchStat.match_id = match.match_id;
            
            // Get team ids and winner id
            // If the match is invalid, then skip it
            
            
            // Set the match team ids and winner id
            
            if (teams.Item1 != Guid.Empty && teams.Item2 != Guid.Empty && teams.Item3 != Guid.Empty)
            {
                match.team1_id = teams.Item1;
                match.team2_id = teams.Item2;
                match.winner_team_id = teams.Item3;
            }
            else
            {
                Console.WriteLine("Invalid");
            }


            players[i] = player;
            matchStats[i] = matchStat;
        }

        Console.WriteLine("Yipee");
        
        // Upload match to Supabase
        var matchResponse = await supabase
            .From<Match>()
            .Upsert(match);

        // Loop through each player in array and upload if not null
        foreach (Player player in players)
        {
            if (player == null)
            {
                continue;
            }

            try
            {
                var playerResponse = await supabase
                    .From<Player>()
                    .Insert(player);
            }
            catch (PostgrestException e)
            {
                // Ignore duplicate key exception
                Console.WriteLine( e.Message);
            }
           
        }
        

        // Loop through each match stat in array and upload if not null
        foreach (MatchStats matchStat in matchStats)
        {
            if (matchStat == null)
            {
                continue;
            }

            try
            {
                var matchStatsResponse = await supabase
                    .From<MatchStats>()
                    .Upsert(matchStat);
            }
            catch (PostgrestException e)
            {
                // Ignore duplicate key exception
            }
            
        }
        
        return true;
        Console.WriteLine("Finished processing match: " + fileName);
    }
    
    // Get teams from match and winner team id
    // Grabs the team id from the first player in the list
    // Grabs the second team id from the last player in the list
    
    // Returns a tuple of team1_id, team2_id, winner_team_id
    public static async Task<(Guid, Guid, Guid)> GetTeams(Metadata metadata)
    {
        PlayerStats[] playerStats = metadata.PlayerStatistics;
        
        // IF the lenght of player stats is less than 10, then the match is invalid
        if (playerStats.Length < 10)
        {
            Console.WriteLine("Invalid match");
            return (Guid.Empty, Guid.Empty, Guid.Empty);
        }
        
        // Array to hold player string puuids
        string [] team_puuids = new string[10];
        int winning_team = 0;
        
        // Loop through each player in the match
        // Add the player's puuid to the team_puuids array
        for (int i = 0; i < playerStats.Length; i++)
        {
            team_puuids[i] = playerStats[i].PUUID;
            
            if (playerStats[i].Win == "Win" && winning_team == 0)
            {
                winning_team = i < 5 ? 1 : 2;
            }
        }
        
        
        
        // Grab the players team id for each player from supabase
        // If the player is not in the database, then do nothing
        
        // Loop through each player in the match
        // If the player is on team 1, add their team id to the team1_ids array
        // If the player is on team 2, add their team id to the team2_ids array
        Guid [] team1_ids = new Guid[5];
        Guid [] team2_ids = new Guid[5];
        Guid winning_team_id = Guid.Empty;
        
        for (int i = 0; i < playerStats.Length; i++)
        {
            // Get the player from the database
            // Get the players team for season from the team_players table
            var player1 = await supabase.From<TeamPlayer>()
                .Select("*, teams(season), players(name)")
                .Filter("player_id", Constants.Operator.Equals, team_puuids[i])
                .Filter("intramural_season", Constants.Operator.Equals, intramural_season)
                .Single();


            // If the player is null, then they are not in the database
            // Do nothing
            if (player1 == null)
            {
                continue;
            }
            
            // Add the player's team id to the team1_ids array
            if (i < 5)
            {
                team1_ids[i] = player1.team_id;
                
                // If the player is on the winning team, then set the winning team id
                if (winning_team == 1 && player1.team_id != Guid.Empty)
                {
                    winning_team_id = player1.team_id;
                }
            }
            // Add the player's team id to the team2_ids array
            else
            {
                team2_ids[i - 5] = player1.team_id;
                
                // If the player is on the winning team, then set the winning team id
                if (winning_team == 2 && player1.team_id != Guid.Empty)
                {
                    winning_team_id = player1.team_id;
                }
            }
        }
        
        
        // team1_id is the id that appears the most in the team1_ids array
        Guid team1_id = team1_ids.GroupBy(x => x)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
        Guid team2_id = team2_ids.GroupBy(x => x)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
        
        if(team1_id == Guid.Empty || team2_id == Guid.Empty)
        {
            Console.WriteLine("Invalid match");
            return (Guid.Empty, Guid.Empty, Guid.Empty);
        }

        return (team1_id, team2_id, winning_team_id);
    }

    // Saves all users puuid:riot_id to a json file from the replay folder
    public static async Task saveUsers()
    {
        var files = Directory.GetFiles(replayFolder);
        var users = new Dictionary<string, string>();
        
        foreach (var file in files)
        {
            var replay = await RoflReader.LoadAsync(file, loadAll: true);
            var metadata = replay.Metadata;
            var playerStats = metadata.PlayerStatistics;
            
            foreach (var player in playerStats)
            {
                var puuid = player.PUUID;
                var name = player.Name;

                try
                {
                    users.Add(puuid, name);
                }
                catch (ArgumentException)
                {
                  // Ignore duplicate key exception
                }
                
            }
        }
        
        var json = JsonSerializer.Serialize(users);
        // pretty print json
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var prettyJson = JsonSerializer.Serialize(users, options);
        
        File.WriteAllText(@"users.json", prettyJson);
    }
    
    public static Match GetMatch(Metadata metadata, PayloadHeader payloadHeader)
    {
        Match match = new Match
        {
            match_id = payloadHeader.GameId.ToString(),
            match_length = (int) metadata.GameLength / 1000,
            game_version = metadata.GameVersion,
            team1_id = null,
            team2_id = null,
            season = intramural_season, // Set the season, used for filtering in the database
            winner_team_id = null
        };

        return match;
    }
    
    // Returns a new MatchStats object from a RoflParticipant object
    // It will not contain the puuid
    public static MatchStats GetPlayerMatchStats(PlayerStats playerStats, PayloadHeader payloadHeader)
    {
        MatchStats matchStats = new MatchStats
        {
            match_id = payloadHeader.GameId.ToString(),
            assists = int.TryParse(playerStats.Assists, out int assists) ? assists : 0,
            baron_kills = int.TryParse(playerStats.BaronKills, out int baron_kills) ? baron_kills : 0,
            barracks_killed = int.TryParse(playerStats.BarracksKilled, out int barracks_killed) ? barracks_killed : 0,
            barracks_takedowns = int.TryParse(playerStats.BarracksTakedowns, out int barracks_takedowns) ? barracks_takedowns : 0,
            bounty_level = int.TryParse(playerStats.BountyLevel, out int bounty_level) ? bounty_level : 0,
            champions_killed = int.TryParse(playerStats.ChampionsKilled, out int champions_killed) ? champions_killed : 0,
            champion_mission_stat0 = int.TryParse(playerStats.ChampionMissionStat0, out int champion_mission_stat0) ? champion_mission_stat0 : 0,
            champion_mission_stat1 = int.TryParse(playerStats.ChampionMissionStat1, out int champion_mission_stat1) ? champion_mission_stat1 : 0,
            champion_mission_stat2 = int.TryParse(playerStats.ChampionMissionStat2, out int champion_mission_stat2) ? champion_mission_stat2 : 0,
            champion_mission_stat3 = int.TryParse(playerStats.ChampionMissionStat3, out int champion_mission_stat3) ? champion_mission_stat3 : 0,
            champion_transform = int.TryParse(playerStats.ChampionTransform, out int champion_transform) ? champion_transform : 0,
            consumable_purchased = int.TryParse(playerStats.ConsumablesPurchased, out int consumable_purchased) ? consumable_purchased : 0,
            double_kills = int.TryParse(playerStats.DoubleKills, out int double_kills) ? double_kills : 0,
            dragon_kills = int.TryParse(playerStats.DragonKills, out int dragon_kills) ? dragon_kills : 0,
            exp = int.TryParse(playerStats.Exp, out int exp) ? exp : 0,
            friendly_dampen_lost = int.TryParse(playerStats.FriendlyDampenLost, out int friendly_dampen_lost) ? friendly_dampen_lost : 0,
            friendly_hq_lost = int.TryParse(playerStats.FriendlyHQLost, out int friendly_hq_lost) ? friendly_hq_lost : 0,
            friendly_turret_lost = int.TryParse(playerStats.FriendlyTurretLost, out int friendly_turret_lost) ? friendly_turret_lost : 0,
            game_ended_in_early_surrender = bool.TryParse(playerStats.GameEndedInEarlySurrender, out bool game_ended_in_early_surrender) ? game_ended_in_early_surrender : false,
            game_ended_in_surrender = bool.TryParse(playerStats.GameEndedInSurrender, out bool game_ended_in_surrender) ? game_ended_in_surrender : false,
            gold_earned = int.TryParse(playerStats.GoldEarned, out int gold_earned) ? gold_earned : 0,
            gold_spent = int.TryParse(playerStats.GoldSpent, out int gold_spent) ? gold_spent : 0,
            hq_killed = int.TryParse(playerStats.HQKilled, out int hq_killed) ? hq_killed : 0,
            hq_takedowns = int.TryParse(playerStats.HQTakedowns, out int hq_takedowns) ? hq_takedowns : 0,
            id = int.TryParse(playerStats.Id, out int id) ? id : 0,
            individual_position = playerStats.IndividualPosition,
            item0 = int.TryParse(playerStats.Item0, out int item0) ? item0 : 0,
            item1 = int.TryParse(playerStats.Item1, out int item1) ? item1 : 0,
            item2 = int.TryParse(playerStats.Item2, out int item2) ? item2 : 0,
            item3 = int.TryParse(playerStats.Item3, out int item3) ? item3 : 0,
            item4 = int.TryParse(playerStats.Item4, out int item4) ? item4 : 0,
            item5 = int.TryParse(playerStats.Item5, out int item5) ? item5 : 0,
            item6 = int.TryParse(playerStats.Item6, out int item6) ? item6 : 0,
            items_purchased = int.TryParse(playerStats.ItemsPurchased, out int items_purchased) ? items_purchased : 0,
            keystone_id = int.TryParse(playerStats.KeystoneId, out int keystone_id) ? keystone_id : 0,
            killing_sprees = int.TryParse(playerStats.KillingSprees, out int killing_sprees) ? killing_sprees : 0,
            largest_critical_strike = int.TryParse(playerStats.LargestCriticalStrike, out int largest_critical_strike) ? largest_critical_strike : 0,
            largest_killing_spree = int.TryParse(playerStats.LargestKillingSpree, out int largest_killing_spree) ? largest_killing_spree : 0,
            largest_multi_kill = int.TryParse(playerStats.LargestMultiKill, out int largest_multi_kill) ? largest_multi_kill : 0,
            level = int.TryParse(playerStats.Level, out int level) ? level : 0,
            longest_time_spent_living = int.TryParse(playerStats.LongestTimeSpentLiving, out int longest_time_spent_living) ? longest_time_spent_living : 0,
            magic_damage_dealt_player = int.TryParse(playerStats.MagicDamageDealtPlayer, out int magic_damage_dealt_player) ? magic_damage_dealt_player : 0,
            magic_damage_dealt_to_champions = int.TryParse(playerStats.MagicDamageDealtToChampions, out int magic_damage_dealt_to_champions) ? magic_damage_dealt_to_champions : 0,
            magic_damage_taken = int.TryParse(playerStats.MagicDamageTaken, out int magic_damage_taken) ? magic_damage_taken : 0,
            minions_killed = int.TryParse(playerStats.MinionsKilled, out int minions_killed) ? minions_killed : 0,
            muted_all = bool.TryParse(playerStats.MutedAll, out bool muted_all) ? muted_all : false,
            name = playerStats.Name,
            neutral_minions_killed = int.TryParse(playerStats.NeutralMinionsKilled, out int neutral_minions_killed) ? neutral_minions_killed : 0,
            neutral_minions_killed_enemy_jungle = int.TryParse(playerStats.NeutralMinionsKilledEnemyJungle, out int neutral_minions_killed_enemy_jungle) ? neutral_minions_killed_enemy_jungle : 0,
            neutral_minions_killed_your_jungle = int.TryParse(playerStats.NeutralMinionsKilledYourJungle, out int neutral_minions_killed_your_jungle) ? neutral_minions_killed_your_jungle : 0,
            node_capture = int.TryParse(playerStats.NodeCapture, out int node_capture) ? node_capture : 0,
            node_capture_assist = int.TryParse(playerStats.NodeCaptureAssist, out int node_capture_assist) ? node_capture_assist : 0,
            node_neutralize = int.TryParse(playerStats.NodeNeutralize, out int node_neutralize) ? node_neutralize : 0,
            node_neutralize_assist = int.TryParse(playerStats.NodeNeutralizeAssist, out int node_neutralize_assist) ? node_neutralize_assist : 0,
            num_deaths = int.TryParse(playerStats.NumDeaths, out int num_deaths) ? num_deaths : 0,
            objectives_stolen = int.TryParse(playerStats.ObjectivesStolen, out int objectives_stolen) ? objectives_stolen : 0,
            objectives_stolen_assists = int.TryParse(playerStats.ObjectivesStolenAssists, out int objectives_stolen_assists) ? objectives_stolen_assists : 0,
            penta_kills = int.TryParse(playerStats.PentaKills, out int penta_kills) ? penta_kills : 0,
            perk0 = int.TryParse(playerStats.Perk0, out int perk0) ? perk0 : 0,
            perk0_var1 = int.TryParse(playerStats.Perk0Var1, out int perk0_var1) ? perk0_var1 : 0,
            perk0_var2 = int.TryParse(playerStats.Perk0Var2, out int perk0_var2) ? perk0_var2 : 0,
            perk0_var3 = int.TryParse(playerStats.Perk0Var3, out int perk0_var3) ? perk0_var3 : 0,
            perk1 = int.TryParse(playerStats.Perk1, out int perk1) ? perk1 : 0,
            perk1_var1 = int.TryParse(playerStats.Perk1Var1, out int perk1_var1) ? perk1_var1 : 0,
            perk1_var2 = int.TryParse(playerStats.Perk1Var2, out int perk1_var2) ? perk1_var2 : 0,
            perk1_var3 = int.TryParse(playerStats.Perk1Var3, out int perk1_var3) ? perk1_var3 : 0,
            perk2 = int.TryParse(playerStats.Perk2, out int perk2) ? perk2 : 0,
            perk2_var1 = int.TryParse(playerStats.Perk2Var1, out int perk2_var1) ? perk2_var1 : 0,
            perk2_var2 = int.TryParse(playerStats.Perk2Var2, out int perk2_var2) ? perk2_var2 : 0,
            perk2_var3 = int.TryParse(playerStats.Perk2Var3, out int perk2_var3) ? perk2_var3 : 0,
            perk3 = int.TryParse(playerStats.Perk3, out int perk3) ? perk3 : 0,
            perk3_var1 = int.TryParse(playerStats.Perk3Var1, out int perk3_var1) ? perk3_var1 : 0,
            perk3_var2 = int.TryParse(playerStats.Perk3Var2, out int perk3_var2) ? perk3_var2 : 0,
            perk3_var3 = int.TryParse(playerStats.Perk3Var3, out int perk3_var3) ? perk3_var3 : 0,
            perk4 = int.TryParse(playerStats.Perk4, out int perk4) ? perk4 : 0,
            perk4_var1 = int.TryParse(playerStats.Perk4Var1, out int perk4_var1) ? perk4_var1 : 0,
            perk4_var2 = int.TryParse(playerStats.Perk4Var2, out int perk4_var2) ? perk4_var2 : 0,
            perk4_var3 = int.TryParse(playerStats.Perk4Var3, out int perk4_var3) ? perk4_var3 : 0,
            perk5 = int.TryParse(playerStats.Perk5, out int perk5) ? perk5 : 0,
            perk5_var1 = int.TryParse(playerStats.Perk5Var1, out int perk5_var1) ? perk5_var1 : 0,
            perk5_var2 = int.TryParse(playerStats.Perk5Var2, out int perk5_var2) ? perk5_var2 : 0,
            perk5_var3 = int.TryParse(playerStats.Perk5Var3, out int perk5_var3) ? perk5_var3 : 0,
            perk_primary_style = int.TryParse(playerStats.PerkPrimaryStyle, out int perk_primary_style) ? perk_primary_style : 0,
            perk_sub_style = int.TryParse(playerStats.PerkSubStyle, out int perk_sub_style) ? perk_sub_style : 0,
            physical_damage_dealt_player = int.TryParse(playerStats.PhysicalDamageDealtPlayer, out int physical_damage_dealt_player) ? physical_damage_dealt_player : 0,
            physical_damage_dealt_to_champions = int.TryParse(playerStats.PhysicalDamageDealtToChampions, out int physical_damage_dealt_to_champions) ? physical_damage_dealt_to_champions : 0,
            physical_damage_taken = int.TryParse(playerStats.PhysicalDamageTaken, out int physical_damage_taken) ? physical_damage_taken : 0,
            ping = int.TryParse(playerStats.Ping, out int ping) ? ping : 0,
            players_i_muted = int.TryParse(playerStats.PlayersIMuted, out int players_i_muted) ? players_i_muted : 0,
            players_that_muted_me = int.TryParse(playerStats.PlayersThatMutedMe, out int players_that_muted_me) ? players_that_muted_me : 0,
            player_position = int.TryParse(playerStats.PlayerPosition, out int player_position) ? player_position : 0,
            player_role = int.TryParse(playerStats.PlayerRole, out int player_role) ? player_role : 0,
            player_score0 = int.TryParse(playerStats.PlayerScore0, out int player_score0) ? player_score0 : 0,
            player_score1 = int.TryParse(playerStats.PlayerScore1, out int player_score1) ? player_score1 : 0,
            player_score10 = int.TryParse(playerStats.PlayerScore10, out int player_score10) ? player_score10 : 0,
            player_score11 = int.TryParse(playerStats.PlayerScore11, out int player_score11) ? player_score11 : 0,
            player_score2 = int.TryParse(playerStats.PlayerScore2, out int player_score2) ? player_score2 : 0,
            player_score3 = int.TryParse(playerStats.PlayerScore3, out int player_score3) ? player_score3 : 0,
            player_score4 = int.TryParse(playerStats.PlayerScore4, out int player_score4) ? player_score4 : 0,
            player_score5 = int.TryParse(playerStats.PlayerScore5, out int player_score5) ? player_score5 : 0,
            player_score6 = int.TryParse(playerStats.PlayerScore6, out int player_score6) ? player_score6 : 0,
            player_score7 = int.TryParse(playerStats.PlayerScore7, out int player_score7) ? player_score7 : 0,
            player_score8 = int.TryParse(playerStats.PlayerScore8, out int player_score8) ? player_score8 : 0,
            player_score9 = int.TryParse(playerStats.PlayerScore9, out int player_score9) ? player_score9 : 0,
            quadra_kills = int.TryParse(playerStats.QuadraKills, out int quadra_kills) ? quadra_kills : 0,
            sight_wards_bought_in_game = int.TryParse(playerStats.SightWardsBoughtInGame, out int sight_wards_bought_in_game) ? sight_wards_bought_in_game : 0,
            skin = playerStats.Skin,
            spell1_cast = int.TryParse(playerStats.Spell1Cast, out int spell1_cast) ? spell1_cast : 0,
            spell2_cast = int.TryParse(playerStats.Spell2Cast, out int spell2_cast) ? spell2_cast : 0,
            spell3_cast = int.TryParse(playerStats.Spell3Cast, out int spell3_cast) ? spell3_cast : 0,
            spell4_cast = int.TryParse(playerStats.Spell4Cast, out int spell4_cast) ? spell4_cast : 0,
            stat_perk0 = int.TryParse(playerStats.StatPerk0, out int stat_perk0) ? stat_perk0 : 0,
            stat_perk1 = int.TryParse(playerStats.StatPerk1, out int stat_perk1) ? stat_perk1 : 0,
            stat_perk2 = int.TryParse(playerStats.StatPerk2, out int stat_perk2) ? stat_perk2 : 0,
            summon_spell1_cast = int.TryParse(playerStats.SummonSpell1Cast, out int summon_spell1_cast) ? summon_spell1_cast : 0,
            summon_spell2_cast = int.TryParse(playerStats.SummonSpell2Cast, out int summon_spell2_cast) ? summon_spell2_cast : 0,
            team = int.TryParse(playerStats.Team, out int team) ? team : 0,
            team_early_surrendered = bool.TryParse(playerStats.TeamEarlySurrendered, out bool team_early_surrendered) ? team_early_surrendered : false,
            team_objective = int.TryParse(playerStats.TeamObjective, out int team_objective) ? team_objective : 0,
            team_position = playerStats.TeamPosition,
            time_ccing_others = int.TryParse(playerStats.TimeCCingOthers, out int time_ccing_others) ? time_ccing_others : 0,
            time_of_from_last_disconnect = int.TryParse(playerStats.TimeOfFromLastDisconnect, out int time_of_from_last_disconnect) ? time_of_from_last_disconnect : 0,
            time_played = int.TryParse(playerStats.TimePlayed, out int time_played) ? time_played : 0,
            time_spent_disconnected = int.TryParse(playerStats.TimeSpentDisconnected, out int time_spent_disconnected) ? time_spent_disconnected : 0,
            total_damage_dealt = int.TryParse(playerStats.TotalDamageDealt, out int total_damage_dealt) ? total_damage_dealt : 0,
            total_damage_dealt_to_buildings = int.TryParse(playerStats.TotalDamageDealtToBuildings, out int total_damage_dealt_to_buildings) ? total_damage_dealt_to_buildings : 0,
            total_damage_dealt_to_champions = int.TryParse(playerStats.TotalDamageDealtToChampions, out int total_damage_dealt_to_champions) ? total_damage_dealt_to_champions : 0,
            total_damage_dealt_to_objectives = int.TryParse(playerStats.TotalDamageDealtToObjectives, out int total_damage_dealt_to_objectives) ? total_damage_dealt_to_objectives : 0,
            total_damage_dealt_to_turrets = int.TryParse(playerStats.TotalDamageDealtToTurrets, out int total_damage_dealt_to_turrets) ? total_damage_dealt_to_turrets : 0,
            total_damage_self_mitigated = int.TryParse(playerStats.TotalDamageSelfMitigated, out int total_damage_self_mitigated) ? total_damage_self_mitigated : 0,
            total_damage_shielded_on_teammates = int.TryParse(playerStats.TotalDamageShieldedOnTeammates, out int total_damage_shielded_on_teammates) ? total_damage_shielded_on_teammates : 0,
            total_damage_taken = int.TryParse(playerStats.TotalDamageTaken, out int total_damage_taken) ? total_damage_taken : 0,
            total_heal = int.TryParse(playerStats.TotalHeal, out int total_heal) ? total_heal : 0,
            total_heal_on_teammates = int.TryParse(playerStats.TotalHealOnTeammates, out int total_heal_on_teammates) ? total_heal_on_teammates : 0,
            total_time_crowd_control_dealt = int.TryParse(playerStats.TotalTimeCrowdControlDealt, out int total_time_crowd_control_dealt) ? total_time_crowd_control_dealt : 0,
            total_time_spent_dead = int.TryParse(playerStats.TotalTimeSpentDead, out int total_time_spent_dead) ? total_time_spent_dead : 0,
            total_units_healed = int.TryParse(playerStats.TotalUnitsHealed, out int total_units_healed) ? total_units_healed : 0,
            triple_kills = int.TryParse(playerStats.TripleKills, out int triple_kills) ? triple_kills : 0,
            true_damage_dealt_player = int.TryParse(playerStats.TrueDamageDealtPlayer, out int true_damage_dealt_player) ? true_damage_dealt_player : 0,
            true_damage_dealt_to_champions = int.TryParse(playerStats.TrueDamageDealtToChampions, out int true_damage_dealt_to_champions) ? true_damage_dealt_to_champions : 0,
            true_damage_taken = int.TryParse(playerStats.TrueDamageTaken, out int true_damage_taken) ? true_damage_taken : 0,
            turrets_killed = int.TryParse(playerStats.TurretsKilled, out int turrets_killed) ? turrets_killed : 0,
            turret_takedowns = int.TryParse(playerStats.TurretTakedowns, out int turret_takedowns) ? turret_takedowns : 0,
            unique_id = playerStats.UniqueId,
            unreal_kills = int.TryParse(playerStats.UnrealKills, out int unreal_kills) ? unreal_kills : 0,
            victory_point_total = int.TryParse(playerStats.VictoryPointTotal, out int victory_point_total) ? victory_point_total : 0,
            vision_score = int.TryParse(playerStats.VisionScore, out int vision_score) ? vision_score : 0,
            vision_wards_bought_in_game = int.TryParse(playerStats.VisionWardsBoughtInGame, out int vision_wards_bought_in_game) ? vision_wards_bought_in_game : 0, 
            wards_killed = int.TryParse(playerStats.WardKilled, out int wards_killed) ? wards_killed : 0,
            wards_placed = int.TryParse(playerStats.WardPlaced, out int wards_placed) ? wards_placed : 0,
            wards_placed_detector = int.TryParse(playerStats.WardPlacedDetector, out int wards_placed_detector) ? wards_placed_detector : 0,
            was_afk = bool.TryParse(playerStats.WasAfk, out bool was_afk) ? was_afk : false,
            was_afk_after_failed_surrender = bool.TryParse(playerStats.WasAfkAfterFailedSurrender, out bool was_afk_after_failed_surrender) ? was_afk_after_failed_surrender : false,
            was_leaver = bool.TryParse(playerStats.WasLeaver, out bool was_leaver) ? was_leaver : false,
            win = (playerStats.Win == "Win") ? true : ((playerStats.Win == "Fail") ? false : false),

            // New stats from latest Fraxiinus update
            all_in_pings = int.TryParse(playerStats.AllInPings, out int all_in_pings) ? all_in_pings : 0,
            assist_me_pings = int.TryParse(playerStats.AssistMePings, out int assist_me_pings) ? assist_me_pings : 0,
            bait_pings = int.TryParse(playerStats.BaitPings, out int bait_pings) ? bait_pings : 0,
            basic_pings = int.TryParse(playerStats.BasicPings, out int basic_pings) ? basic_pings : 0,
            command_pings = int.TryParse(playerStats.CommandPings, out int command_pings) ? command_pings : 0,
            danger_pings = int.TryParse(playerStats.DangerPings, out int danger_pings) ? danger_pings : 0,
            enemy_missing_pings = int.TryParse(playerStats.EnemyMissingPings, out int enemy_missing_pings) ? enemy_missing_pings : 0,
            enemy_vision_pings = int.TryParse(playerStats.EnemyVisionPings, out int enemy_vision_pings) ? enemy_vision_pings : 0,
            get_back_pings = int.TryParse(playerStats.GetBackPings, out int get_back_pings) ? get_back_pings : 0,
            hold_pings = int.TryParse(playerStats.HoldPings, out int hold_pings) ? hold_pings : 0,
            horde_kills = int.TryParse(playerStats.HordeKills, out int horde_kills) ? horde_kills : 0,
            largest_ability_damage = int.TryParse(playerStats.LargestAbilityDamage, out int largest_ability_damage) ? largest_ability_damage : 0,
            largest_attack_damage = int.TryParse(playerStats.LargestAttackDamage, out int largest_attack_damage) ? largest_attack_damage : 0,
            last_takedown_time = int.TryParse(playerStats.LastTakedownTime, out int last_takedown_time) ? last_takedown_time : 0,
            need_vision_pings = int.TryParse(playerStats.NeedVisionPings, out int need_vision_pings) ? need_vision_pings : 0,
            on_my_way_pings = int.TryParse(playerStats.OnMyWayPings, out int on_my_way_pings) ? on_my_way_pings : 0,
            player_augment1 = int.TryParse(playerStats.PlayerAugment1, out int player_augment1) ? player_augment1 : 0,
            player_augment2 = int.TryParse(playerStats.PlayerAugment2, out int player_augment2) ? player_augment2 : 0,
            player_augment3 = int.TryParse(playerStats.PlayerAugment3, out int player_augment3) ? player_augment3 : 0,
            player_augment4 = int.TryParse(playerStats.PlayerAugment4, out int player_augment4) ? player_augment4 : 0,
            player_subteam = int.TryParse(playerStats.PlayerSubteam, out int player_subteam) ? player_subteam : 0,
            player_subteam_placement = int.TryParse(playerStats.PlayerSubteamPlacement, out int player_subteam_placement) ? player_subteam_placement : 0,
            puuid = playerStats.PUUID,
            rift_herald_kills = int.TryParse(playerStats.RiftHeraldKills, out int rift_herald_kills) ? rift_herald_kills : 0,
            vision_wards_cleared = int.TryParse(playerStats.VisionClearedPings, out int vision_cleared_pings) ? vision_cleared_pings : 0
        };
        
        return matchStats;
    }
    
    // Create a public variable called "supabase" of type Supabase.Client
    public static Supabase.Client supabase;
    
    // Replays folder location
    public static string replayFolder = "";

    public static int intramural_season; // No default value, must be set before running the program to ensure the correct season is set
}