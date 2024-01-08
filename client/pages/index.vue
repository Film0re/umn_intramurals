<template>
  <div>
    <DataTable
      :value="matches"
      :paginator="true"
      :rows="10"
      :rowsPerPageOptions="[10, 25, 50]"
      :sortField="'winPercentage'"
      :sortOrder="-1"
    >
    <template #header="slotProps">
      <div>
        Season
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

      <Column field="team_name" header="Name"></Column>
      <Column field="wins" header="Wins"></Column>
      <Column field="losses" header="Losses"></Column>
      <Column field="winPercentage" header="Win Percentage"></Column>
    </DataTable>
  </div>
</template>

<script setup>

const client = useSupabaseClient();
const matches = ref([]);
const season = ref(4);
const seasons = ref([4,5]);
const isPlayoffs = ref(false);  

// Initial load
onMounted(async () => {
  await getTeams();
});

// Watch for changes
watch(season, async () => {
  await getTeams();
});

const getTeams = async () => {
  // Grab all teams for the selected season
  const { data, error } = await client
    .from("teams")
    .select(`*`)
    .eq("season", season.value)

  if (error) {
    console.log(error);
  } else {
    matches.value = data.map(team => ({
      ...team,
      winPercentage: ((team.wins / (team.wins + team.losses)) * 100).toFixed(0)
    }));
    console.log(matches.value);
  }
};



</script>