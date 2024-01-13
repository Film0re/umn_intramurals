import os
from dotenv import load_dotenv
import csv
from supabase import create_client
import sys

# Load environment variables from .env
load_dotenv()

# Function to read teams, players, and season from a text file
def read_teams_players(file_path):
    with open(file_path, 'r') as file:
        # Read the first line to get the season information
        season_line = file.readline().strip()
        season_key, season_value = season_line.split('=')
        season = season_value.strip()

        # Continue reading the rest of the file
        reader = csv.reader(file)
        teams_players = [row for row in reader]

    return season, teams_players

# Function to connect to Supabase
def connect_to_supabase():
    supabase_url = os.environ.get("SUPABASE_URL")
    supabase_key = os.environ.get("SUPABASE_KEY")

    supabase_client = create_client(supabase_url, supabase_key)
    return supabase_client

# Function to get team ID based on team name
def get_team_id(supabase_client, team_name):
    result = supabase_client.table('teams').select('team_id').eq('name', team_name).limit(1).execute()
    
    if result.data:
        return result.data[0]['team_id']
    else:
        raise ValueError(f"Team with name '{team_name}' not found.")

# Function to get player ID based on player name
def get_player_id(supabase_client, player_name):
    result = supabase_client.table('players').select('player_id').eq('name', player_name).limit(1).execute()

    if result.data:
        return result.data[0]['player_id']

    else:
        raise ValueError(f"Player with name '{player_name}' not found.")

# Function to insert team_players into Supabase
def insert_team_players(supabase_client, season, teams_players_data):
    success_count = 0
    total_count = len(teams_players_data)

    with open('insert_log.txt', 'w') as log_file:
        for team_player in teams_players_data:
            team_name = team_player[0]
            player_name = team_player[1]
            
            try:
                print("Team name: ", team_name)
                team_id = get_team_id(supabase_client, team_name)

                print("Team ID: ", team_id)
                player_id = get_player_id(supabase_client, player_name)

                supabase_client.table('team_players').insert({
                    'team_id': team_id,
                    'player_id': player_id,
                    'intramural_season': season  # Use the extracted season value
                }).execute()

                success_count += 1
                log_file.write(f"Successfully inserted team_id={team_id}, player_id={player_id}\n")
            except Exception as e:
                log_file.write(f"Failed to insert team_name={team_name}, player_name={player_name}: {str(e)}\n")
                print(f"Failed to insert team_name={team_name}, player_name={player_name}: {str(e)}\n")

        success_rate = (success_count / total_count) * 100
        log_file.write(f"\nSuccess rate: {success_rate}%\n")

def main():
    if len(sys.argv) != 3:
        print("Usage: python insert_team_players.py <file_path> <season>")
        sys.exit(1)

    file_path = sys.argv[1]
    season = sys.argv[2]
    teams_players_data = read_teams_players(file_path)
    supabase_client = connect_to_supabase()
    insert_team_players(supabase_client, season, teams_players_data)

if __name__ == '__main__':
    main()
