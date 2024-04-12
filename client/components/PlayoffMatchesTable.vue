<template>
  <DataTable
    :value="matches.filter((match) => match.is_playoffs === isPlayoffs)"
    :paginator="true"
    :rows="10"
    :rows-per-page-options="[10, 25, 50]"
    row-group-mode="subheader"
    group-rows-by="playoff_series_id"
    :filter="true"
  >
    <template #header>
      <div class="flex-center">
        <SeasonPicker @season-changed="season = $event" />
        <PlayoffToggle
          class="left-pad"
          @playoffs-changed="isPlayoffs = $event"
        />
      </div>
    </template>

    <template #groupheader="slotProps">
      <span>{{ convertStringToLabel(slotProps.data.playoff_series_id) }}</span>
    </template>

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
    <Column field="team1.name" header="Team 1">
      <template #body="slotProps">
        <span
          :class="{
            winner:
              slotProps?.data?.winner_team_id === slotProps?.data?.team1_id,
          }"
        >
          {{ slotProps?.data?.team1?.name }}
        </span>
      </template>
    </Column>
    <Column field="team2.name" header="Team 2">
      <template #body="slotProps">
        <span
          :class="{
            winner:
              slotProps?.data?.winner_team_id === slotProps?.data?.team2_id,
          }"
        >
          {{ slotProps?.data?.team2.name }}
        </span>
      </template>
    </Column>
  </DataTable>
</template>

<script setup>
const season = useSeason();
const isPlayoffs = ref(true);

const props = defineProps({
  matches: {
    type: Array,
    required: true,
  },
});

const convertStringToLabel = (str) => {
  const parts = str.split("-");
  const series_type = parts[1][0].toUpperCase() + parts[1].slice(1);
  return `${series_type}`;
};

</script>

<style scoped>
.winner {
  color: var(--primary-500);
}

.flex-center {
  display: flex;
  justify-content: center;
  align-items: center;
}

.left-pad {
  margin-left: 1rem;
  width: 107px;
}
</style>
