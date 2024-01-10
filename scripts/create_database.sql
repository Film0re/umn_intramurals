-- Enable the uuid-ossp extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
-- Create the users table
CREATE TABLE user_profiles (
                        id uuid not null REFERENCES auth.users on delete cascade,
                        username TEXT UNIQUE,
                        created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                        PRIMARY KEY (id)
    -- Add other user-related fields as needed
);

-- Create function to handle new user
CREATE OR REPLACE FUNCTION public.handle_new_user()
RETURNS TRIGGER
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public
AS $$
BEGIN
  -- Insert only the id into public.user_profiles
  INSERT INTO public.user_profiles (id)
  VALUES (NEW.id);
  RETURN NEW;
END;
$$;

-- Create trigger to execute the function after user creation
CREATE TRIGGER on_auth_user_created
AFTER INSERT ON auth.users
FOR EACH ROW
EXECUTE FUNCTION public.handle_new_user();



-- Create the teams table
CREATE TABLE teams (
                       team_id UUID DEFAULT uuid_generate_v4() PRIMARY KEY,
                       name TEXT NOT NULL,
                       season INT,
                       wins INT DEFAULT 0,
                       games_played INT DEFAULT 0,
                       rank INT DEFAULT 0,
                       created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                       CONSTRAINT unique_team_name_per_season UNIQUE (name, season)
    -- Add other team-related fields as needed
);


-- Create the players table
CREATE TABLE players (
                         player_id TEXT PRIMARY KEY,
                         name TEXT,
                         user_id UUID REFERENCES users(id),
                         team_id UUID REFERENCES teams(team_id),
                         created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    -- Add other player-related fields as needed
                         CONSTRAINT unique_user_team_pair UNIQUE (player_id, team_id)
);

-- Create the matches table
CREATE TABLE matches (
                         match_id TEXT PRIMARY KEY, 
                         game_version TEXT,
                         match_length INT,
                         is_playoffs BOOLEAN DEFAULT FALSE,
                         team1_id UUID REFERENCES teams(team_id),
                         team2_id UUID REFERENCES teams(team_id),
                         winner_team_id UUID REFERENCES teams(team_id),
                         created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    -- Add other match-related fields as needed
);


-- Create the match_data table
CREATE TABLE match_data (
                            match_id TEXT REFERENCES matches(match_id),
                            player_id TEXT REFERENCES players(player_id),
                            intramural_season INT,
                            assists INT,
                            baron_kills INT,
                            barracks_killed INT,
                            barracks_takedowns INT,
                            bounty_level INT,
                            champions_killed INT,
                            champion_mission_stat0 INT,
                            champion_mission_stat1 INT,
                            champion_mission_stat2 INT,
                            champion_mission_stat3 INT,
                            champion_transform INT,
                            consumable_purchased INT,
                            double_kills INT,
                            dragon_kills INT,
                            exp INT,
                            id INT,
                            friendly_dampen_lost INT,
                            friendly_hq_lost INT,
                            friendly_turret_lost INT,
                            game_ended_in_early_surrender BOOLEAN,
                            game_ended_in_surrender BOOLEAN,
                            gold_earned INT,
                            gold_spent INT,
                            hq_killed INT,
                            hq_takedowns INT,
                            individual_position TEXT,
                            item0 INT,
                            item1 INT,
                            item2 INT,
                            item3 INT,
                            item4 INT,
                            item5 INT,
                            item6 INT,
                            items_purchased INT,
                            keystone_id INT,
                            killing_sprees INT,
                            largest_critical_strike INT,
                            largest_killing_spree INT,
                            largest_multi_kill INT,
                            level INT,
                            longest_time_spent_living INT,
                            magic_damage_dealt_player INT,
                            magic_damage_dealt_to_champions INT,
                            magic_damage_taken INT,
                            minions_killed INT,
                            muted_all BOOLEAN,
                            name TEXT,
                            neutral_minions_killed INT,
                            neutral_minions_killed_enemy_jungle INT,
                            neutral_minions_killed_your_jungle INT,
                            node_capture INT,
                            node_capture_assist INT,
                            node_neutralize INT,
                            node_neutralize_assist INT,
                            num_deaths INT,
                            objectives_stolen INT,
                            objectives_stolen_assists INT,
                            penta_kills INT,
                            perk0 INT,
                            perk0_var1 INT,
                            perk0_var2 INT,
                            perk0_var3 INT,
                            perk1 INT,
                            perk1_var1 INT,
                            perk1_var2 INT,
                            perk1_var3 INT,
                            perk2 INT,
                            perk2_var1 INT,
                            perk2_var2 INT,
                            perk2_var3 INT,
                            perk3 INT,
                            perk3_var1 INT,
                            perk3_var2 INT,
                            perk3_var3 INT,
                            perk4 INT,
                            perk4_var1 INT,
                            perk4_var2 INT,
                            perk4_var3 INT,
                            perk5 INT,
                            perk5_var1 INT,
                            perk5_var2 INT,
                            perk5_var3 INT,
                            perk_primary_style INT,
                            perk_sub_style INT,
                            physical_damage_dealt_player INT,
                            physical_damage_dealt_to_champions INT,
                            physical_damage_taken INT,
                            ping INT,
                            players_i_muted INT,
                            players_that_muted_me INT,
                            player_position INT,
                            player_role INT,
                            player_score0 INT,
                            player_score1 INT,
                            player_score10 INT,
                            player_score11 INT,
                            player_score2 INT,
                            player_score3 INT,
                            player_score4 INT,
                            player_score5 INT,
                            player_score6 INT,
                            player_score7 INT,
                            player_score8 INT,
                            player_score9 INT,
                            quadra_kills INT,
                            sight_wards_bought_in_game INT,
                            skin TEXT,
                            spell1_cast INT,
                            spell2_cast INT,
                            spell3_cast INT,
                            spell4_cast INT,
                            stat_perk0 INT,
                            stat_perk1 INT,
                            stat_perk2 INT,
                            summon_spell1_cast INT,
                            summon_spell2_cast INT,
                            team INT,
                            team_early_surrendered BOOLEAN,
                            team_objective INT,
                            team_position TEXT,
                            time_ccing_others INT,
                            time_of_from_last_disconnect INT,
                            time_played INT,
                            time_spent_disconnected INT,
                            total_damage_dealt INT,
                            total_damage_dealt_to_buildings INT,
                            total_damage_dealt_to_champions INT,
                            total_damage_dealt_to_objectives INT,
                            total_damage_dealt_to_turrets INT,
                            total_damage_self_mitigated INT,
                            total_damage_shielded_on_teammates INT,
                            total_damage_taken INT,
                            total_heal INT,
                            total_heal_on_teammates INT,
                            total_time_crowd_control_dealt INT,
                            total_time_spent_dead INT,
                            total_units_healed INT,
                            triple_kills INT,
                            true_damage_dealt_player INT,
                            true_damage_dealt_to_champions INT,
                            true_damage_taken INT,
                            turrets_killed INT,
                            turret_takedowns INT,
                            unique_id TEXT,
                            unreal_kills INT,
                            victory_point_total INT,
                            vision_score INT,
                            vision_wards_bought_in_game INT,
                            wards_killed INT,
                            wards_placed INT,
                            wards_placed_detector INT,
                            was_afk BOOLEAN,
                            was_afk_after_failed_surrender BOOLEAN,
                            was_early_surrender_accomplice BOOLEAN,
                            was_leaver BOOLEAN,
                            was_surrender_due_to_afk BOOLEAN,
                            win BOOLEAN,
                            -- New fields from Fraxiinus update
                            all_in_pings INT,
                            assist_me_pings INT,
                            bait_pings INT,
                            basic_pings INT,
                            command_pings INT,
                            danger_pings INT,
                            enemy_missing_pings INT,
                            enemy_vision_pings INT,
                            get_back_pings INT,
                            need_vision_pings INT,
                            on_my_way_pings INT,
                            hold_pings INT,
                            push_pings INT,
                            retreat_pings INT,
                            puuid TEXT,
                            rift_herald_kills INT,
                            vision_wards_cleared INT,
                            horde_kills INT, -- No idea what this is
                            largest_ability_damage INT,
                            largest_attack_damage INT,
                            last_takedown_time INT,
                            -- I believe these are for arena games
                            player_augment_1 INT,
                            player_augment_2 INT,
                            player_augment_3 INT,
                            player_augment_4 INT,
                            player_subteam INT,
                            player_subteam_placement INT,

                            created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                            PRIMARY KEY (match_id, player_id)
);

-- Create team_players table
CREATE TABLE team_players (
                                team_id UUID REFERENCES teams(team_id),
                                player_id TEXT REFERENCES players(player_id),
                                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                                PRIMARY KEY (team_id, player_id)
);


CREATE OR REPLACE FUNCTION public.is_admin()
    RETURNS BOOLEAN AS $$
BEGIN
    RETURN (SELECT (raw_user_meta_data->>'is_admin')::BOOLEAN FROM auth.users WHERE id = auth.uid());
END;
$$ LANGUAGE plpgsql SECURITY DEFINER;


-- Grant EXECUTE privilege on the function to public
GRANT EXECUTE ON FUNCTION public.is_admin() TO public;

-- Policies to allow for public access of all tables

CREATE POLICY "Enable read access for all users" ON "public"."match_data"
AS PERMISSIVE FOR SELECT
TO anon, authenticated
USING (true);

CREATE POLICY "Enable read access for all users" ON "public"."teams"
AS PERMISSIVE FOR SELECT
TO anon, authenticated
USING (true);

CREATE POLICY "Enable read access for all users" ON "public"."players"
AS PERMISSIVE FOR SELECT
TO anon, authenticated
USING (true);

CREATE POLICY "Enable read access for all users" ON "public"."matches"
AS PERMISSIVE FOR SELECT
TO anon, authenticated
USING (true);

CREATE POLICY "Enable read access for all users" ON "public"."user_profiles"
AS PERMISSIVE FOR SELECT
TO anon, authenticated
USING (true);

CREATE POLICY "Enable read access for all users" ON "public"."team_players"
AS PERMISSIVE FOR SELECT
TO anon, authenticated
USING (true);

-- Create policy to allow admins to insert/update/delete teams
CREATE POLICY "Allow admins to insert/update/delete teams" ON "public"."teams"
FOR ALL
TO public
USING (public.is_admin());

-- Create policy to allow admins to insert/update/delete players
CREATE POLICY "Allow admins to insert/update/delete players" ON "public"."players"
FOR ALL
TO public
USING (public.is_admin());

-- Create policy to allow admins to insert/update/delete matches
CREATE POLICY "Allow admins to insert/update/delete matches" ON "public"."matches"
FOR ALL
TO public
USING (public.is_admin());

-- Create policy to allow admins to insert/update/delete match_data
CREATE POLICY "Allow admins to insert/update/delete match_data" ON "public"."match_data"
FOR ALL
TO public
USING (public.is_admin());

-- Create policy to allow admins to insert/update/delete user_profiles
CREATE POLICY "Allow admins to insert/update/delete user_profiles" ON "public"."user_profiles"
FOR ALL
TO public
USING (public.is_admin());



-- Grant SELECT privilege on the tables to public
GRANT SELECT ON TABLE public.teams TO public;
GRANT SELECT ON TABLE public.players TO public;
GRANT SELECT ON TABLE public.matches TO public;
GRANT SELECT ON TABLE public.match_data TO public;
GRANT SELECT ON TABLE public.user_profiles TO public;
GRANT SELECT ON TABLE public.team_players TO public;

-- Grant INSERT/UPDATE/DELETE privilege on the tables to authenticated users (row level security will prevent them from doing anything they shouldn't)
GRANT INSERT, UPDATE, DELETE ON TABLE public.teams TO authenticated;
GRANT INSERT, UPDATE, DELETE ON TABLE public.players TO authenticated;
GRANT INSERT, UPDATE, DELETE ON TABLE public.matches TO authenticated;
GRANT INSERT, UPDATE, DELETE ON TABLE public.match_data TO authenticated;
GRANT INSERT, UPDATE, DELETE ON TABLE public.user_profiles TO authenticated;


-- Grant usage on the sequences to public
-- GRANT USAGE ON SEQUENCE public.teams_team_id_seq TO authenticated;
-- GRANT USAGE ON SEQUENCE public.players_player_id_seq TO authenticated;
-- GRANT USAGE ON SEQUENCE public.matches_match_id_seq TO authenticated;
-- GRANT USAGE ON SEQUENCE public.match_data_match_id_seq TO authenticated;
-- GRANT USAGE ON SEQUENCE public.match_data_player_id_seq TO authenticated;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE matches TO service_role;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE match_data TO service_role;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE players TO service_role;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE teams TO service_role;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE user_profiles TO service_role;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE team_players TO service_role;

-- Storage Bucket policy

-- Allow admins to do anything
create policy "Allow admins to do anything" on storage.objects
for all
to public
using (public.is_admin());

-- Grant SELECT privilege on the tables to public
GRANT SELECT ON TABLE storage.objects TO public;

-- Grant select privilenge on storage.objects to postgres
GRANT SELECT ON TABLE storage.objects TO service_role;

-- Grant INSERT/UPDATE/DELETE privilege on the tables to authenticated users (row level security will prevent them from doing anything they shouldn't)
GRANT INSERT, UPDATE, DELETE ON TABLE storage.objects TO authenticated;

CREATE OR REPLACE FUNCTION public.update_win_loss()
RETURNS TRIGGER
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public
AS $$
BEGIN
    IF NOT NEW.is_playoffs THEN
        -- Update wins for the winning team
        UPDATE public.teams
        SET wins = wins + 1
        WHERE team_id = NEW.winner_team_id;

        -- Update games_played
        UPDATE public.teams
        SET games_played = games_played + 1
        WHERE team_id = NEW.team1_id OR team_id = NEW.team2_id;
    END IF;

    RETURN NEW;
END;
$$;

CREATE TRIGGER update_win_loss
AFTER INSERT ON public.matches
FOR EACH ROW
EXECUTE FUNCTION public.update_win_loss();
