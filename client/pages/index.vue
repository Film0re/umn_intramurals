<template>
  <div>
    <DataTable :value="matches" :rows="10" :sortField="'rank'" :sortOrder="1">
      <template #header="slotProps">
        <div>
          Regular Season
          <div>
            <span>
              <Dropdown
                :options="seasons"
                v-model="season"
                @change="season = $event.value"
              />
            </span>
          </div>
        </div>
      </template>
      <Column field="rank" header="Rank" :sortable="false"></Column>
      <Column field="name" header="Name"></Column>
      <Column field="wins" header="Wins"></Column>
      <Column field="games_played" header="Games Played"></Column>

      <Column field="winPercentage" header="Win Percentage"></Column>
    </DataTable>

 
  </div>
</template>

<script setup>



const client = useSupabaseClient();
const matches = ref([]);
const season = ref(5);
const seasons = ref([4, 5]);
const isPlayoffs = ref(false);

// Initial load
onMounted(async () => {
  await getTeams();
  await getOptions();
});

// Watch for changes
watch(season, async () => {
  await getTeams();
});

const getTeams = async () => {
  // Grab all teams for the selected season
  const { data, error } = await client
    .from("teams")
    .select(
      `*
    `
    )
    .eq("season", season.value);

  if (error) {
    console.log(error);
  } else {
    matches.value = data.map((team) => ({
      ...team,
      winPercentage: ((team.wins / team.games_played) * 100).toFixed(2),
    }));
    console.log(matches.value);
  }
};

const getOptions = async () => {
  // Grab all teams for the selected season
  const { data, error } = await client.from("teams").select(`season`);

  if (error) {
    console.log(error);
  } else {
    // Ugly hack to get unique seasons but it works :\
    seasons.value = [...new Set(data.map((season) => season.season))];

    console.log(seasons.value);
  }
};
</script>

<style scoped>
/* Add the following style to highlight the winning team with a green border */
.winner {
  border: 2px solid var(--pink-600);
}
</style>
