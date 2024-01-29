<template>
  <div>
    <span>
      Season:
      <Dropdown v-model="season" :options="options || []" @change="onChange" />
    </span>
  </div>
</template>

<script setup>
const client = useSupabaseClient();
const season = ref(6);

const emit = defineEmits(["seasonChanged"]);

const getSeasons = async () => {
  // Grab all teams for the selected season
  const { data, error } = await client
    .from("teams")
    .select(`season`)
    .order("season", { ascending: false });

  if (error) {
    // eslint-disable-next-line no-console
    console.log(error);
  } else {
    // Filter out duplicate seasons and return
    const local_seasons = [...new Set(data.map((team) => team.season))];

    season.value = local_seasons[0];

    return local_seasons;
  }
};

const onChange = (e) => {
  season.value = e.value;
  console.log("season changed", e.value);
  emit("seasonChanged", e.value);
};

emit("seasonChanged", season.value);

const { data: options } = useAsyncData("seasons", getSeasons);
</script>
