<template>
  <DataTable
    :value="matches"
    :paginator="true"
    :rows="10"
    :rows-per-page-options="[10, 25, 50]"
  >
    <Column field="match_id" header="Match ID">
      <template #body="slotProps">
        <NuxtLink
          :to="{
            name: 'matches-id',
            params: { id: slotProps.data.match_id },
          }"
        >
        View
        </NuxtLink>
      </template>
    </Column>
    <Column field="team1Name.name" header="Team 1" />
    <Column field="team2Name.name" header="Team 2" />
  </DataTable>
</template>

<script setup>
const client = useSupabaseClient();
const emit = defineEmits(["loaded"]);

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

const { data : matches } = useAsyncData("matches", getMatches);
emit("loaded");
</script>
