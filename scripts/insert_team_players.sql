DO $$ 
DECLARE 
    teamName TEXT := 'TEAM_NAME';
    seasonNumber INT := 5;
    playerNames TEXT[] := ARRAY[''];
    teamId UUID;
BEGIN
    -- Attempt to retrieve team_id for the specified team name and season number
    SELECT team_id INTO teamId
    FROM teams
    WHERE name = teamName AND season = seasonNumber;

    -- If team_id is not found, insert a new team record
    IF teamId IS NULL THEN
        INSERT INTO teams (name, season) VALUES (teamName, seasonNumber)
        RETURNING team_id INTO teamId;
    END IF;

    -- Insert players into team_players table with the retrieved team_id
    INSERT INTO team_players (team_id, player_id)
    SELECT teamId, player_id
    FROM players
    WHERE name = ANY(playerNames)
    ON CONFLICT (team_id, player_id) DO NOTHING;
END $$;


