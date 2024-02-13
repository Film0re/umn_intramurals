CREATE VIEW player_stats_view AS
SELECT
  players.name,
  team_players.player_id,
  teams.season,
  AVG(match_data.assists) AS average_assists,
  AVG(match_data.champions_killed) as average_kills,
  AVG(match_data.num_deaths) as average_deaths,
  AVG(match_data.champions_killed) + AVG(match_data.assists) / AVG(match_data.num_deaths) as average_kda,
  SUM(quadra_kills) as total_quada_kills,
  SUM(penta_kills) as total_penta_kills,
  SUM(triple_kills) as total_triple_kills,
  SUM(dragon_kills) as total_dragon_kills,
  SUM(baron_kills) as total_baron_kills,
  SUM(spell1_cast) as total_q_cast,
  SUM(spell2_cast) as total_w_cast,
  SUM(spell3_cast) as total_e_cast,
  SUM(spell4_cast) as total_r_cast,
  max(largest_critical_strike) as largest_critical_strike,
  SUM(time_ccing_others) as total_time_ccing_others,
  max(largest_ability_damage) as largest_ability_damage,
  max(longest_time_spent_living) as longest_time_spent_living,
  SUM(all_in_pings) as total_all_in_pings,
  SUM(assist_me_pings) as total_assist_me_pings,
  SUM(bait_pings) as total_bait_pings,
  SUM(basic_pings) as total_basic_pings,
  SUM(command_pings) as total_command_pings,
  SUM(danger_pings) as total_danger_pings,
  SUM(enemy_missing_pings) as total_enemy_missing_pings,
  SUM(enemy_vision_pings) as total_enemy_vision_pings,
  SUM(get_back_pings) as total_get_back_pings,
  SUM(need_vision_pings) as total_need_vision_pings,
  SUM(on_my_way_pings) as total_on_my_way_pings,
  SUM(hold_pings) as total_hold_pings,
  SUM(push_pings) as total_push_pings,
  SUM(retreat_pings) as total_retreat_pings,
  -- Calculate total pings
  SUM(all_in_pings + assist_me_pings + bait_pings + basic_pings + command_pings +
  danger_pings + enemy_missing_pings + enemy_vision_pings + get_back_pings +
  need_vision_pings + on_my_way_pings + hold_pings + push_pings + retreat_pings) as total_pings
FROM
  team_players
  INNER JOIN players ON players.player_id = team_players.player_id
  INNER JOIN teams ON teams.team_id = team_players.team_id
  INNER JOIN match_data ON players.player_id = match_data.player_id
  INNER JOIN matches ON match_data.match_id = matches.match_id
WHERE
  team_players.intramural_season = matches.season
GROUP BY
  players.name,
  team_players.player_id,
  teams.season;
