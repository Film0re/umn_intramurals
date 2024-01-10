<template>
  <div
    style="
      width: clamp(300px, 100%, 1000px);
      margin: 0 auto;
      padding: 1rem;
      text-align: center;
    "
  >
    <TabView>
      <TabPanel header="Player Stats">
        <DataTable
          :value="player_averages"
          :paginator="true"
          :rows="10"
          :rowsPerPageOptions="[10, 25, 50]"
        >
          <template #header>
            <div
              style="
                display: flex;
                justify-content: center;
                align-items: center;
              "
            >
              <div class="flex-column">
                <label for="season">Season</label>
                <Dropdown
                  v-model="season"
                  :options="seasons"
                  :placeholder="`Select Season`"
                  :onChange="getTeams"
                ></Dropdown>
              </div>

              <div class="flex-column">
                <label for="options">Options</label>
                <MultiSelect
                  v-model="selectedOptions"
                  :options="options"
                  :optionLabel="convertFieldNameToLabel"
                  :placeholder="`Select ${options.length} options`"
                  :maxSelectedLabels="3"
                  :filter="true"
                  :showSelectAll="true"
                  :onChange="getTeams"
                ></MultiSelect>
              </div>
            </div>
          </template>

          <Column field="name" header="Name"></Column>

          <Column
            v-for="option in selectedOptions"
            :key="option"
            :field="`averages.${option}`"
            :header="convertFieldNameToLabel(option)"
            sortable
          ></Column>
        </DataTable>
      </TabPanel>

      <TabPanel header="Matches">
        <DataTable
          :value="matches"
          :paginator="true"
          :rows="10"
          :rowsPerPageOptions="[10, 25, 50]"
        >
          <Column field="match_id" header="Match ID"></Column>
          <Column field="game_version" header="Match Date"></Column>
          <Column field="game_mode" header="Game Mode"></Column>
        </DataTable>
      </TabPanel>

      <TabPanel header="Upload">
        <div>
          <Button type="button" @click="open()">Choose files</Button>
          <Button type="button" :disabled="!files" @click="reset()"
            >Reset</Button
          >

          <Button type="button" :disabled="!files" @click="() => upload()"
            >Upload</Button
          >

          <template v-if="files">
            <p>
              You have selected:
              <b>{{
                `${files.length} ${files.length === 1 ? "file" : "files"}`
              }}</b>
            </p>
            <li v-for="file of files" :key="file.name">
              {{ file.name }}
            </li>
          </template>
        </div>
      </TabPanel>
    </TabView>
    <Toast />
  </div>
</template>

<script setup>
definePageMeta({
  middleware: ["admin-auth"],
});

const client = useSupabaseClient();
const matches = ref([]);
const toast = useToast();

const teams = ref([]);
const team_players = ref([]);
const player_averages = ref([]);

const options = ref([
  "champions_killed",
  "num_deaths",
  "assists",
  "gold_earned",
]);
const selectedOptions = ref([
  "champions_killed",
  "num_deaths",
  "assists",
  "gold_earned",
]);

const season = ref(5);
const seasons = ref([]);

onMounted(() => {
  getMatches();
  getTeams();
  getOptions();
});

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

//Function that will query the database for all of the fields in match_data table
const getOptions = async () => {
  const { data, error } = await client.from("options").select("*").single();

  if (error) {
    console.log(error);
  } else {
    options.value = data.stat_options;
    stats_to_not_average.value = data.stats_to_not_average;
    seasons.value = data.seasons;

    console.log(options.value);
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
    console.log(error);
  } else {
    teams.value = data;

    team_players.value = data.map((team) => {
      return convertTeamToPlayerArray(team);
    });

    console.log("HI");
    console.log(team_players.value);
    console.log(teams.value);

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

    console.log(playerAverages);

    player_averages.value = playerAverages;
  }
};

// TODO: Filter only .rofl files
const getMatches = async () => {
  const { data, error } = await client.from("matches").select("*");

  if (error) {
    console.log(error);
  } else {
    matches.value = data;
  }
};

const { files, open, reset, onChange } = useFileDialog();

// Iterate over files and upload them
// to the "Replays" bucket
// Using promise.all would be faster but would also make it harder to
// figure out which file failed to upload
const upload = async () => {
  for (const file of files.value) {
    const { data, error } = await client.storage
      .from("Replays")
      .upload(file.name, file);

    if (error) {
      toast.add({
        severity: "error",
        summary: "Error",
        detail: error.message,
        life: 3000,
      });
    } else {
      toast.add({
        severity: "success",
        summary: "Success",
        detail: "File uploaded",
        life: 3000,
      });
    }
  }
};

const convertTeamToPlayerArray = (team) => {
  return team.team_players.map((player) => {
    return {
      ...player.player,
      team: team.name,
    };
  });
};
</script>

<style scoped>
.flex-column {
  display: flex;
  flex-direction: column;
  padding-left: 5px;
}
</style>
