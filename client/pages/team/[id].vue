<template>
  <TeamDisplay v-if="team" :team="team" />
</template>

<script setup>
const route = useRoute();
const client = useSupabaseClient();

const getMatch = async () => {
  const { data, error } = await client
    .from("team_with_players_matches")
    .select()
    .eq("team_id", route.params.id)
    .single();

  if (error) {
    console.error(error);
    return null;
  }
  return data;
};

const { data: team } = useAsyncData("team", getMatch);
</script>
