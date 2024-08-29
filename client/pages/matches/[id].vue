<template>
  <div class="full-height">
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
        team1:teams!matches_team1_id_fkey(*),
        team2:teams!matches_team2_id_fkey(*),
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
      .sort((a, b) => roleMap[a.individual_position] - roleMap[b.individual_position])
      .map(player => ({
        summonerName: player.name,
        champion: player.skin,
        kills: player.champions_killed,
        deaths: player.num_deaths,
        assists: player.assists,
        cs: player.minions_killed + player.neutral_minions_killed,
        killparticipation: player.kill_participation,
        items: [player.item0, player.item1, player.item2, player.item3, player.item4, player.item5]
      }));

    const totalKills = players.reduce((acc, player) => acc + player.kills, 0);
    const totalGold = players.reduce((acc, player) => acc + player.gold_earned, 0);

    return {
      id: teamData.team_id,
      name: teamData.name,
      players,
      stats: {
        kills: totalKills,
        gold: totalGold,
      }
    };
  };

  const team1 = formatTeam(match.value.team1, 100);
  const team2 = formatTeam(match.value.team2, 200);

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
    }
  };
});
</script>

<style scoped>
.full-height {
  margin-bottom: auto;
}
</style>
