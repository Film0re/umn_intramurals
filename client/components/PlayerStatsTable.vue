<template>
  <pre>
    <!-- {{ player_averages }} -->
  </pre>
  <DataTable
    :value="player_averages"
    :paginator="true"
    :rows="10"
    :rows-per-page-options="[10, 25, 50]"
  >
    <template #header>
      <div style="display: flex; justify-content: center; align-items: center">
        <SeasonPicker @season-changed="season = $event" />

        <!-- {{ player_averages }} -->
        <div class="flex-row">
          <label for="options" style="padding-left: 10px;">Options: </label>
          <MultiSelect
            v-model="selectedOptions"
            :options="player_averages ? Object.keys(player_averages[0]) : []"
            :option-label="convertFieldNameToLabel"
            :placeholder="`Select ${player_averages ? Object.keys(player_averages[0]).length : 0} options`"
            :max-selected-labels="0"
            :filter="true"
            :show-select-all="true"
            :on-change="getPlayersStats"
          />
        </div>
      </div>
    </template>

    <Column field="name" header="Name" />

    <Column
      v-for="option in selectedOptions"
      :key="option"
      :field="option"
      :header="convertFieldNameToLabel(option)"
      sortable
    />
  </DataTable>
</template>

<script setup>
const client = useSupabaseClient();
const season = useSeason();

const emit = defineEmits(["loaded"]);

const selectedOptions = ref(["average_kda", "average_kills", "average_deaths", 
]);

const convertFieldNameToLabel = (fieldName) => {
  const label = fieldName
    .split("_")
    .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
    .join(" ");

  return label;
};

// Possibly the worlds ugliest function but it works
// and will continue to work even as new fields are added
const getPlayersStats = async () => {
  const { data, error } = await client
    .from("player_stats_view")
    .select("*")
    .eq("season", season.value);

  if (error) {
    // eslint-disable-next-line no-console
    console.log(error);
  } else {
    return data;
  }
};
// Get player averages for the selected season, and watch for changes to the season
const { data: player_averages } = await useAsyncData(
  "player_averages",
  getPlayersStats,
  {
    watch: season,
  }
);

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
