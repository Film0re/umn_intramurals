<template>
  <div class="opgg-team-card">
    <MatchPlayerStatsCard
      v-for="player in team.players"
      :key="player.puuid"
      :player="player"
    />
  </div>
</template>

<script setup>
const props = defineProps({
  team: {
    type: Object,
    required: true,
  },
});
</script>

<style scoped>
.opgg-team-card {
  display: flex;
  flex-direction: column;
  align-items: center;
}

/* Add any additional styling as needed */
</style>
