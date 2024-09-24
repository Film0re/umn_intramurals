<template>
  <div class="full-height">
    <!-- <pre>
      {{ match }}
    </pre> -->
    <MatchCard v-if="formattedMatchData" :match="formattedMatchData" />
  </div>
</template>

<script setup>
const route = useRoute();
const client = useSupabaseClient();

const getMatch = async () => {
  const { data, error } = await client
    .from("matches")
    .select(
      `*,
        team1:teams!matches_team1_id_fkey(*, team_players(players(name, player_id))),
        team2:teams!matches_team2_id_fkey(*, team_players(players(name,player_id))),
        winner:teams!matches_winner_team_id_fkey(*),
        match_data(*)
        `
    )
    .eq("match_id", route.params.id)
    .single();

  if (error) {
    console.error(error);
    return null;
  }
  return data;
};

const { data: match } = useAsyncData("match", getMatch);

const formattedMatchData = computed(() => {
  if (!match.value) return null;

  const roleMap = {
    TOP: 0,
    JUNGLE: 1,
    MIDDLE: 2,
    BOTTOM: 3,
    UTILITY: 4,
  };

  const formatTeam = (teamData, teamId) => {
    const players = match.value.match_data
      .filter((player) => player.team === teamId)
      .sort(
        (a, b) =>
          roleMap[a.individual_position] - roleMap[b.individual_position]
      )
      .map((player) => ({
        summonerName: player.name,
        player_id: player.player_id,
        champion: player.skin,
        kills: player.champions_killed,
        deaths: player.num_deaths,
        assists: player.assists,
        cs: player.minions_killed + player.neutral_minions_killed,
        killparticipation: player.kill_participation,
        items: [
          player.item0,
          player.item1,
          player.item2,
          player.item3,
          player.item4,
          player.item5,
        ],
      }));

    const totalKills = players.reduce((acc, player) => acc + player.kills, 0);
    const totalGold = players.reduce(
      (acc, player) => acc + player.gold_earned,
      0
    );

    return {
      id: teamData.team_id,
      name: teamData.name,
      players,
      stats: {
        kills: totalKills,
        gold: totalGold,
      },
    };
  };
  console.log("team1", match.value.team1);

  const team1 = formatTeam(match.value.team1, 100);
  const team2 = formatTeam(match.value.team2, 200);

  // Loop through match.value.team1/2.team_players and create an object with player_id as the key
  // and the player name as the value

  const playerNames = {};

  match.value.team1.team_players.forEach((player) => {
    playerNames[player.players.player_id] = player.players.name;
  });

  match.value.team2.team_players.forEach((player) => {
    playerNames[player.players.player_id] = player.players.name;
  });


  // Update the name with the one from the db if known
  // will not work for subs :\
  // Damn you riot for breaking my code >:(
  team1.players.forEach((player) => {
    player.summonerName = playerNames[player.player_id] ?? player.summonerName;
  });

  team2.players.forEach((player) => {
    player.summonerName = playerNames[player.player_id] ?? player.summonerName;
  });


  return {
    duration: match.value.match_length,
    gameMode: match.value.queue_id, // Assuming queue_id represents the game mode
    winner: match.value.winner.team_id,
    blueTeam: {
      ...team1,
      win: match.value.winner.team_id === team1.id,
    },
    redTeam: {
      ...team2,
      win: match.value.winner.team_id === team2.id,
    },
  };
});
</script>

<style scoped>
.full-height {
  margin-bottom: auto;
}
</style>
