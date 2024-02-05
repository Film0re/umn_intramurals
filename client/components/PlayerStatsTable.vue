<template>
  <DataTable
    :value="player_averages"
    :paginator="true"
    :rows="10"
    :rows-per-page-options="[10, 25, 50]"
  >
    <template #header>
      <div style="display: flex; justify-content: center; align-items: center">
        <SeasonPicker @season-changed="season = $event" />

        <div class="flex-column">
          <label for="options">Options</label>
          <MultiSelect
            v-model="selectedOptions"
            :options="options.stat_options"
            :option-label="convertFieldNameToLabel"
            :placeholder="`Select ${options.stat_options.length} options`"
            :max-selected-labels="3"
            :filter="true"
            :show-select-all="true"
            :on-change="getTeams"
          />
        </div>
      </div>
    </template>

    <Column field="name" header="Name" />

    <Column
      v-for="option in selectedOptions"
      :key="option"
      :field="`averages.${option}`"
      :header="convertFieldNameToLabel(option)"
      sortable
    />
  </DataTable>
</template>

<script setup>
const client = useSupabaseClient();
const teams = ref([]);
const team_players = ref([]);
const season = ref(6);

const emit = defineEmits(["loaded"]);

const selectedOptions = ref([
  "champions_killed",
  "num_deaths",
  "assists",
  "gold_earned",
]);
const stats_to_not_average = ref([
  "match_id",
  "match_data",
  "team",
  "name",
  "double_kills",
  "triple_kills",
  "quadra_kills",
  "penta_kills",
  "unreal_kills",
  "largest_killing_spree",
  "largest_multi_kill",
  "largest_critical_strike",
]);

const convertFieldNameToLabel = (fieldName) => {
  const label = fieldName
    .split("_")
    .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
    .join(" ");

  return label;
};

// Function that will query the database for all of the fields in match_data table
const getOptions = async () => {
  const { data, error } = await client.from("options").select("*").single();

  if (error) {
    // eslint-disable-next-line no-console
    console.log(error);
  } else {
    return data;
  }
};

// Possibly the worlds ugliest function but it works
// and will continue to work even as new fields are added
const getTeams = async () => {
  const { data, error } = await client
    .from("teams")
    .select(
      `
      *,
      team_players (
        player: players (
          name,
          match_data: match_data (
            *
          )
        )
      )
  `
    )
    .eq("season", season.value);

  if (error) {
    // eslint-disable-next-line no-console
    console.log(error);
  } else {
    teams.value = data;

    team_players.value = data.map((team) => convertTeamToPlayerArray(team));

    // Extract player names and averages
    const playerAverages = team_players.value.reduce((result, team) => {
      team.forEach((player) => {
        const playerName = player.name;
        const playerAverage = {};

        // Calculate averages for each player
        Object.keys(player).forEach((key) => {
          if (key === "match_data") {
            // Calculate averages for match_data fields
            const matches = player[key].length;
            player[key].forEach((match) => {
              Object.keys(match).forEach((stat) => {
                if (
                  typeof match[stat] === "number" &&
                  !stats_to_not_average.value.includes(stat)
                ) {
                  playerAverage[stat] =
                    (playerAverage[stat] || 0) + match[stat] / matches;
                  // round to 2 decimal places
                  playerAverage[stat] =
                    Math.round(playerAverage[stat] * 100) / 100;
                } else if (stats_to_not_average.value.includes(stat)) {
                  playerAverage[stat] = match[stat];
                }
              });
            });
          } else if (typeof player[key] === "number") {
            // Include non-match_data numeric values
            playerAverage[key] = player[key];
          }
        });

        // Add player name, team, and averages to the result array
        result.push({
          name: playerName,
          team: player.team,
          averages: playerAverage,
        });
      });

      return result;
    }, []);

    return playerAverages;
  }
};

const convertTeamToPlayerArray = (team) =>
  team.team_players.map((player) => ({
    ...player.player,
    team: team.name,
  }));

// Get player averages for the selected season, and watch for changes to the season
const { data: player_averages } = await useAsyncData(
  "player_averages",
  getTeams,
  {
    watch: season,
  }
);
const { data: options } = await useAsyncData("options", getOptions);

// Emit event to parent component to let it know that the data has loaded

emit("loaded");
</script>

<style scoped>
.flex-column {
  display: flex;
  flex-direction: column;
  padding-left: 5px;
}
</style>
