<template>
  <DataTable
    v-if="!isPlayoffs"
    :value="matches.filter((match) => match.is_playoffs === isPlayoffs)"
    :paginator="true"
    :rows="10"
    :rows-per-page-options="[10, 25, 50]"
    row-group-mode="subheader"
    group-rows-by="week"
    sort-order="0"
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

    <template #groupheader="{ data }">
      <span class="font-bold">Week {{ data.week === "Unknown" ? "Unknown" : data.week }}</span>
    </template>
    <Column field="week" header="Week">
      <template #body="{ data }">
        <span>{{
          data.week === "Unknown" ? "Week Unknown" : `Week ${data.week}`
        }}</span>
      </template>
    </Column>

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

  <PlayoffMatchesTable v-else :matches="matches" />
</template>

<script setup>
import Column from "primevue/column";

const client = useSupabaseClient();
const emit = defineEmits(["loaded"]);
const season = useSeason();
const isPlayoffs = usePlayoffs();

const props = defineProps({
  teamId: {
    type: String,
    required: false,
    default: null,
  },
});

// TODO: Filter only .rofl files
const getMatches = async () => {
  const { data, error } = await client
    .from("matches")
    .select(
      `*,
    team1: teams!matches_team1_id_fkey (
      name,
      season
    ),
    team2: teams!matches_team2_id_fkey (
      name,
      season
    ),
    match_data (
      *
    )
    `
    )
    .order("created_at", { ascending: false });

  if (error) {
    // eslint-disable-next-line no-console
    console.log(error);
    return [];
  } else {
    // If we're on a team page, filter the matches to only show the matches
    if (props.teamId) {
      return data.filter(
        (match) =>
          match.team1_id === props.teamId || match.team2_id === props.teamId
      );
    }

    // Ugly filtering to get only the matches for the selected season
    // Supabase doesn't support filtering on relationships yet >:(
    // TODO: Remove this once supabase supports filtering on relationships
    return data.filter(
      (match) =>
        match.team1.season === season.value ||
        match.team2.season === season.value
    );
  }
};

const { data: matches } = useAsyncData("matches", getMatches, {
  watch: [season],
});
emit("loaded");
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
  width: 107px;
  margin-left: 1rem;
}
</style>
