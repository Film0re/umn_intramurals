<template>
  <DataTable
    :value="matches"
    :paginator="true"
    :rows="10"
    :rows-per-page-options="[10, 25, 50]"
  >
    <Column field="match_id" header="Match ID">
      <template #body="slotProps">
        <Button
          type="button"
          @click="() => navigateTo(`/matches/${slotProps.data.match_id}`)"
        >
          View
        </Button>
      </template>
    </Column>
    <Column field="team1Name.name" header="Team 1" />
    <Column field="team2Name.name" header="Team 2" />
  </DataTable>
</template>

<script setup>
const client = useSupabaseClient();
const matches = ref([]);
const emit = defineEmits(["loaded"]);

onMounted(async () => {
  matches.value = await getMatches();
  emit("loaded");
});

// TODO: Filter only .rofl files
const getMatches = async () => {
  const { data, error } = await client.from("matches").select(`*,
  team1Name: teams!matches_team1_id_fkey (
    name
  ),
  team2Name: teams!matches_team2_id_fkey (
    name
  ),
  match_data (
    *
  )
  `);

  if (error) {
    // eslint-disable-next-line no-console
    console.log(error);
  } else {
    return data;
  }
};
</script>
