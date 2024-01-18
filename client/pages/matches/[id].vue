<template>
  <div class="full-height">
    <MatchCard v-if="match" :match-data="match?.match_data" />
  </div>
</template>

<script setup>
const route = useRoute();
const client = useSupabaseClient();

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
    return data;
  }
};


const { data: match } = useAsyncData("matches", getMatch)
</script>

<style scoped>
.full-height {
  margin-bottom: auto;
}

</style>