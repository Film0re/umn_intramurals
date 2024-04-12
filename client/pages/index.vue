<template>
  <div class="full-height">
    <TabView>
      <TabPanel header="Standings">
        <DataTable
          v-if="teams.length > 0"
          :value="teams"
          :rows="10"
          :sort-field="season === 5 ? 'rank' : 'winPercentage'"
          :sort-order="season === 5 ? 1 : -1"
        >
          <template #header>
            <SeasonPicker @season-changed="season = $event" />
          </template>

          <!-- <Column field="rank" header="Rank" :sortable="false" /> -->
          <Column field="name" header="Team">
            <template #body="slotProps">
              <span>
                {{ slotProps.data.name }} 
              </span>

              <span v-if="slotProps.data.rank === 1" class="winner">
                🏆
              </span>
            </template>
          </Column>
          <Column field="wins" header="Wins" />
          <Column field="games_played" header="Games Played" />
          <Column field="winPercentage" header="Win Percentage" />
        </DataTable>
        <ProgressBar v-else mode="indeterminate" style="height: 6px" />
      </TabPanel>
      <TabPanel header="Matches">
        <MatchesTable />
      </TabPanel>

      <!-- <TabPanel header="Playoffs">

      </TabPanel> -->
    </TabView>
  </div>
</template>

<script setup>
const client = useSupabaseClient();
const season = useSeason();
const isPlayoffs = ref(false);

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
    // eslint-disable-next-line no-console
    console.log(error);
  } else {
    return data.map((team) => ({
      ...team,
      winPercentage: ((team.wins / team.games_played) * 100).toFixed(2),
    }));
  }
};

const { data: teams } = useAsyncData("teams", getTeams, {
  watch: season,
});
</script>

<style scoped>
/* Add the following style to highlight the winning team with a green border */
/* .winner {
  border: 2px solid var(--primary-500);
  border-radius: 5px;
  padding: 0.5rem 0.5rem;
} */

.full-height {
  width: clamp(300px, 100%, 900px);
  margin: 0 auto;
  padding: 1rem;
  text-align: center;
  height: 100%;
  margin-bottom: auto;
}
</style>
