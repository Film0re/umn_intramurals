using Postgrest.Attributes;
using Postgrest.Models;
using umn_supabase;


namespace umn_supabase.Models
{
    [Table("match_data")]
    public class MatchStats : BaseModel
    {
        [PrimaryKey("match_id")]
        [Column("match_id")]
        public string match_id { get; set; }
        
        [PrimaryKey("player_id")]
        [Column("player_id")]
        public string player_id { get; set; }

        [Column("assists")]
        public int assists { get; init; }
        [Column("baron_kills")]
        public int baron_kills { get; init; }
        [Column("barracks_killed")]
        public int barracks_killed { get; init; }
        [Column("barracks_takedowns")]
        public int barracks_takedowns { get; init; }
        [Column("bounty_level")]
        public int bounty_level { get; init; }
        [Column("champions_killed")]
        public int champions_killed { get; init; }
        [Column("champion_mission_stat0")]
        public int champion_mission_stat0 { get; init; }
        [Column("champion_mission_stat1")]
        public int champion_mission_stat1 { get; init; }
        [Column("champion_mission_stat2")]
        public int champion_mission_stat2 { get; init; }
        [Column("champion_mission_stat3")]
        public int champion_mission_stat3 { get; init; }
        [Column("champion_transform")]
        public int champion_transform { get; init; }
        [Column("consumable_purchased")]
        public int consumable_purchased { get; init; }
        [Column("double_kills")]
        public int double_kills { get; init; }
        [Column("dragon_kills")]
        public int dragon_kills { get; init; }
        [Column("exp")]
        public int exp { get; init; }
        [Column("friendly_dampen_lost")]
        public int friendly_dampen_lost { get; init; }
        [Column("friendly_hq_lost")]
        public int friendly_hq_lost { get; init; }
        [Column("friendly_turret_lost")]
        public int friendly_turret_lost { get; init; }
        [Column("game_ended_in_early_surrender")]
        public bool game_ended_in_early_surrender { get; init; }
        [Column("game_ended_in_surrender")]
        public bool game_ended_in_surrender { get; init; }
        [Column("gold_earned")]
        public int gold_earned { get; init; }
        [Column("gold_spent")]
        public int gold_spent { get; init; }
        [Column("hq_killed")]
        public int hq_killed { get; init; }
        [Column("hq_takedowns")]
        public int hq_takedowns { get; init; }
        [Column("id")]
        public int id { get; init; }
        [Column("individual_position")]
        public string individual_position { get; init; }
        [Column("item0")]
        public int item0 { get; init; }
        [Column("item1")]
        public int item1 { get; init; }
        [Column("item2")]
        public int item2 { get; init; }
        [Column("item3")]
        public int item3 { get; init; }
        [Column("item4")]
        public int item4 { get; init; }
        [Column("item5")]
        public int item5 { get; init; }
        [Column("item6")]
        public int item6 { get; init; }
        [Column("items_purchased")]
        public int items_purchased { get; init; }
        [Column("keystone_id")]
        public int keystone_id { get; init; }
        [Column("killing_sprees")]
        public int killing_sprees { get; init; }
        [Column("largest_critical_strike")]
        public int largest_critical_strike { get; init; }
        [Column("largest_killing_spree")]
        public int largest_killing_spree { get; init; }
        [Column("largest_multi_kill")]
        public int largest_multi_kill { get; init; }
        [Column("level")]
        public int level { get; init; }
        [Column("longest_time_spent_living")]
        public int longest_time_spent_living { get; init; }
        [Column("magic_damage_dealt_player")]
        public int magic_damage_dealt_player { get; init; }
        [Column("magic_damage_dealt_to_champions")]
        public int magic_damage_dealt_to_champions { get; init; }
        [Column("magic_damage_taken")]
        public int magic_damage_taken { get; init; }
        [Column("minions_killed")]
        public int minions_killed { get; init; }
        [Column("muted_all")]
        public bool muted_all { get; init; }
        [Column("name")]
        public string name { get; init; }
        [Column("neutral_minions_killed")]
        public int neutral_minions_killed { get; init; }
        [Column("neutral_minions_killed_enemy_jungle")]
        public int neutral_minions_killed_enemy_jungle { get; init; }
        [Column("neutral_minions_killed_your_jungle")]
        public int neutral_minions_killed_your_jungle { get; init; }
        [Column("node_capture")]
        public int node_capture { get; init; }
        [Column("node_capture_assist")]
        public int node_capture_assist { get; init; }
        [Column("node_neutralize")]
        public int node_neutralize { get; init; }
        [Column("node_neutralize_assist")]
        public int node_neutralize_assist { get; init; }
        [Column("num_deaths")]
        public int num_deaths { get; init; }
        [Column("objectives_stolen")]
        public int objectives_stolen { get; init; }
        [Column("objectives_stolen_assists")]
        public int objectives_stolen_assists { get; init; }
        [Column("penta_kills")]
        public int penta_kills { get; init; }
        [Column("perk0")]
        public int perk0 { get; init; }
        [Column("perk0_var1")]
        public int perk0_var1 { get; init; }
        [Column("perk0_var2")]
        public int perk0_var2 { get; init; }
        [Column("perk0_var3")]
        public int perk0_var3 { get; init; }
        [Column("perk1")]
        public int perk1 { get; init; }
        [Column("perk1_var1")]
        public int perk1_var1 { get; init; }
        [Column("perk1_var2")]
        public int perk1_var2 { get; init; }
        [Column("perk1_var3")]
        public int perk1_var3 { get; init; }
        [Column("perk2")]
        public int perk2 { get; init; }
        [Column("perk2_var1")]
        public int perk2_var1 { get; init; }
        [Column("perk2_var2")]
        public int perk2_var2 { get; init; }
        [Column("perk2_var3")]
        public int perk2_var3 { get; init; }
        [Column("perk3")]
        public int perk3 { get; init; }
        [Column("perk3_var1")]
        public int perk3_var1 { get; init; }
        [Column("perk3_var2")]
        public int perk3_var2 { get; init; }
        [Column("perk3_var3")]
        public int perk3_var3 { get; init; }
        [Column("perk4")]
        public int perk4 { get; init; }
        [Column("perk4_var1")]
        public int perk4_var1 { get; init; }
        [Column("perk4_var2")]
        public int perk4_var2 { get; init; }
        [Column("perk4_var3")]
        public int perk4_var3 { get; init; }
        [Column("perk5")]
        public int perk5 { get; init; }
        [Column("perk5_var1")]
        public int perk5_var1 { get; init; }
        [Column("perk5_var2")]
        public int perk5_var2 { get; init; }
        [Column("perk5_var3")]
        public int perk5_var3 { get; init; }
        [Column("perk_primary_style")]
        public int perk_primary_style { get; init; }
        [Column("perk_sub_style")]
        public int perk_sub_style { get; init; }
        [Column("physical_damage_dealt_player")]
        public int physical_damage_dealt_player { get; init; }
        [Column("physical_damage_dealt_to_champions")]
        public int physical_damage_dealt_to_champions { get; init; }
        [Column("physical_damage_taken")]
        public int physical_damage_taken { get; init; }
        [Column("ping")]
        public int ping { get; init; }
        [Column("players_i_muted")]
        public int players_i_muted { get; init; }
        [Column("players_that_muted_me")]
        public int players_that_muted_me { get; init; }
        [Column("player_position")]
        public int player_position { get; init; }
        [Column("player_role")]
        public int player_role { get; init; }
        [Column("player_score0")]
        public int player_score0 { get; init; }
        [Column("player_score1")]
        public int player_score1 { get; init; }
        [Column("player_score10")]
        public int player_score10 { get; init; }
        [Column("player_score11")]
        public int player_score11 { get; init; }
        [Column("player_score2")]
        public int player_score2 { get; init; }
        [Column("player_score3")]
        public int player_score3 { get; init; }
        [Column("player_score4")]
        public int player_score4 { get; init; }
        [Column("player_score5")]
        public int player_score5 { get; init; }
        [Column("player_score6")]
        public int player_score6 { get; init; }
        [Column("player_score7")]
        public int player_score7 { get; init; }
        [Column("player_score8")]
        public int player_score8 { get; init; }
        [Column("player_score9")]
        public int player_score9 { get; init; }
        [Column("quadra_kills")]
        public int quadra_kills { get; init; }
        [Column("sight_wards_bought_in_game")]
        public int sight_wards_bought_in_game { get; init; }
        [Column("skin")]
        public string skin { get; init; }
        [Column("spell1_cast")]
        public int spell1_cast { get; init; }
        [Column("spell2_cast")]
        public int spell2_cast { get; init; }
        [Column("spell3_cast")]
        public int spell3_cast { get; init; }
        [Column("spell4_cast")]
        public int spell4_cast { get; init; }
        [Column("stat_perk0")]
        public int stat_perk0 { get; init; }
        [Column("stat_perk1")]
        public int stat_perk1 { get; init; }
        [Column("stat_perk2")]
        public int stat_perk2 { get; init; }
        [Column("summon_spell1_cast")]
        public int summon_spell1_cast { get; init; }
        [Column("summon_spell2_cast")]
        public int summon_spell2_cast { get; init; }
        [Column("team")]
        public int team { get; init; }
        [Column("team_early_surrendered")]
        public bool team_early_surrendered { get; init; }
        [Column("team_objective")]
        public int team_objective { get; init; }
        [Column("team_position")]
        public string team_position { get; init; }
        [Column("time_ccing_others")]
        public int time_ccing_others { get; init; }
        [Column("time_of_from_last_disconnect")]
        public int time_of_from_last_disconnect { get; init; }
        [Column("time_played")]
        public int time_played { get; init; }
        [Column("time_spent_disconnected")]
        public int time_spent_disconnected { get; init; }
        [Column("total_damage_dealt")]
        public int total_damage_dealt { get; init; }
        [Column("total_damage_dealt_to_buildings")]
        public int total_damage_dealt_to_buildings { get; init; }
        [Column("total_damage_dealt_to_champions")]
        public int total_damage_dealt_to_champions { get; init; }
        [Column("total_damage_dealt_to_objectives")]
        public int total_damage_dealt_to_objectives { get; init; }
        [Column("total_damage_dealt_to_turrets")]
        public int total_damage_dealt_to_turrets { get; init; }
        [Column("total_damage_self_mitigated")]
        
        public int total_damage_self_mitigated { get; init; }
        [Column("total_damage_shielded_on_teammates")]
        public int total_damage_shielded_on_teammates { get; init; }
        [Column("total_damage_taken")]
        public int total_damage_taken { get; init; }
        [Column("total_heal")]
        public int total_heal { get; init; }
        [Column("total_heal_on_teammates")]
        public int total_heal_on_teammates { get; init; }
        [Column("total_time_crowd_control_dealt")]
        public int total_time_crowd_control_dealt { get; init; }
        [Column("total_time_spent_dead")]
        public int total_time_spent_dead { get; init; }
        [Column("total_units_healed")]
        public int total_units_healed { get; init; }
        [Column("triple_kills")]
        public int triple_kills { get; init; }
        [Column("true_damage_dealt_player")]
        public int true_damage_dealt_player { get; init; }
        [Column("true_damage_dealt_to_champions")]
        public int true_damage_dealt_to_champions { get; init; }
        [Column("true_damage_taken")]
        public int true_damage_taken { get; init; }
        [Column("turrets_killed")]
        public int turrets_killed { get; init; }
        [Column("turret_takedowns")]
        public int turret_takedowns { get; init; }
        [Column("unique_id")]
        public string unique_id { get; init; }
        [Column("unreal_kills")]
        public int unreal_kills { get; init; }
        [Column("victory_point_total")]
        public int victory_point_total { get; init; }
        [Column("vision_score")]
        public int vision_score { get; init; }
        [Column("vision_wards_bought_in_game")]
        public int vision_wards_bought_in_game { get; init; }
        [Column("wards_killed")]
        public int wards_killed { get; init; }
        [Column("wards_placed")]
        public int wards_placed { get; init; }
        [Column("wards_placed_detector")]
        public int wards_placed_detector { get; init; }
        [Column("was_afk")]
        public bool was_afk { get; init; }
        [Column("was_afk_after_failed_surrender")]
        public bool was_afk_after_failed_surrender { get; init; }
        [Column("was_early_surrender_accomplice")]
        public bool was_early_surrender_accomplice { get; init; }
        [Column("was_leaver")]
        public bool was_leaver { get; init; }
        [Column("was_surrender_due_to_afk")]
        public bool was_surrender_due_to_afk { get; init; }
        [Column("win")]
        public bool win { get; init; }
        
        // New fields from Fraxiinus update
        [Column("all_in_pings")]
        public int all_in_pings { get; init; }

        [Column("assist_me_pings")]
        public int assist_me_pings { get; init; }

        [Column("bait_pings")]
        public int bait_pings { get; init; }

        [Column("basic_pings")]
        public int basic_pings { get; init; }

        [Column("command_pings")]
        public int command_pings { get; init; }

        [Column("danger_pings")]
        public int danger_pings { get; init; }

        [Column("enemy_missing_pings")]
        public int enemy_missing_pings { get; init; }

        [Column("enemy_vision_pings")]
        public int enemy_vision_pings { get; init; }

        [Column("get_back_pings")]
        public int get_back_pings { get; init; }

        [Column("hold_pings")]
        public int hold_pings { get; init; }

        [Column("horde_kills")]
        public int horde_kills { get; init; }

        [Column("largest_ability_damage")]
        public int largest_ability_damage { get; init; }

        [Column("largest_attack_damage")]
        public int largest_attack_damage { get; init; }

        [Column("last_takedown_time")]
        public int last_takedown_time { get; init; }

        [Column("need_vision_pings")]
        public int need_vision_pings { get; init; }

        [Column("on_my_way_pings")]
        public int on_my_way_pings { get; init; }

        [Column("player_augment_1")]
        public int player_augment1 { get; init; }

        [Column("player_augment_2")]
        public int player_augment2 { get; init; }

        [Column("player_augment_3")]
        public int player_augment3 { get; init; }

        [Column("player_augment_4")]
        public int player_augment4 { get; init; }

        [Column("player_subteam")]
        public int player_subteam { get; init; }

        [Column("player_subteam_placement")]
        public int player_subteam_placement { get; init; }

        [Column("push_pings")]
        public int push_pings { get; init; }

        [Column("puuid")]
        public string puuid { get; init; }

        [Column("retreat_pings")]
        public int retreat_pings { get; init; }

        [Column("rift_herald_kills")]
        public int rift_herald_kills { get; init; }

        [Column("vision_wards_cleared")]
        public int vision_wards_cleared { get; init; }

        
    }
}
