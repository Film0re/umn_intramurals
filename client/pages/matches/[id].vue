<template>
  <div>
    <!-- <PlayerStatsCard v-for="player in match.match_data" :player="player" /> -->
        
        
    <MatchCard v-if="match" :match-data="match?.match_data" />
  </div>
</template>

<script setup>

const route = useRoute();
const client = useSupabaseClient();
const match = ref({});

const getMatch = async () => {
    const { data, error } = await client
        .from("matches")
        .select(
            `*,
            match_data(*)
        `
        )
        .eq("match_id", route.params.id)
        .single();

    if (error) {
        console.log(error);
    } else {
        console.log(data);
        return data;
    }
};

onMounted(async () => {
    match.value = await getMatch();

    console.log(match.value);
});




</script>