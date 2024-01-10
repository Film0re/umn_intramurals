<template>
  <div class="full-height">
    <DataTable :value="teams" :rows="10" :sortField="'rank'" :sortOrder="1">
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
const teams = ref([]);
const season = ref(5);
const seasons = ref([4, 5]);
const isPlayoffs = ref(false);

// Initial load
onMounted(async () => {
  teams.value = await getTeams();
  seasons.value = await getOptions();
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
    return data.map((team) => ({
      ...team,
      winPercentage: ((team.wins / team.games_played) * 100).toFixed(2),
    }));
  }
};

const getOptions = async () => {
  // Grab all teams for the selected season
  const { data, error } = 
  await client
  .from("teams")
  .select(`season`)
  .order("season", { ascending: false })

  if (error) {
    console.log(error);
  } else {
    // Ugly hack to get unique seasons but it works :\
    return [...new Set(data.map((season) => season.season))];
  }
};
</script>

<style scoped>
/* Add the following style to highlight the winning team with a green border */
.winner {
  border: 2px solid var(--pink-600);
}

.full-height {
  width: clamp(300px, 100%, 900px);
  margin: 0 auto;
  padding: 1rem;
  text-align: center;
  height: 100%;
}
</style>
