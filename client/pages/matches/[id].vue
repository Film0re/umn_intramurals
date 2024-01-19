<template>
  <div class="full-height">
    <MatchCard v-if="match" :match="formattedMatchData" />
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
    console.log(error);
  } else {
    return data;
  }
};

// Clean up the data since the way it gets returned from supabase is
// horrible to work with
const formattedMatchData = computed(() => {
  if (!match.value) return;

  const roleMap = {
    TOP: 0,
    JUNGLE: 1,
    MIDDLE: 2,
    BOTTOM: 3,
    UTILITY: 4,
  };

  const team1Players = match?.value?.match_data
    .filter((player) => player.team === 100)
    .sort((a, b) => roleMap[a.role] - roleMap[b.role]);

  const team2Players = match?.value?.match_data
    .filter((player) => player.team === 200)
    .sort((a, b) => roleMap[a.role] - roleMap[b.role]);

  const team1Gold = team1Players.reduce(
    (acc, player) => acc + player.gold_earned,
    0
  );
  const team2Gold = team2Players.reduce(
    (acc, player) => acc + player.gold_earned,
    0
  );

  const team1Kills = team1Players.reduce(
    (acc, player) => acc + player.champions_killed,
    0
  );
  const team2Kills = team2Players.reduce(
    (acc, player) => acc + player.champions_killed,
    0
  );

  if (match.value) {
    return {
      team1: {
        name: match.value.team1.name,
        players: team1Players,
        win: match.value.winner.team_id === match.value.team1.team_id,
        stats: {
          gold: team1Gold,
          kills: team1Kills,
        },
      },
      team2: {
        name: match.value.team2.name,
        players: team2Players,
        win: match.value.winner.team_id === match.value.team2.team_id,
        stats: {
          gold: team2Gold,
          kills: team2Kills,
        },
      },
    };
  }
});

const { data: match } = useAsyncData("match", getMatch);
</script>

<style scoped>
.full-height {
  margin-bottom: auto;
}
</style>
