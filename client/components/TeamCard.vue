<template>
  <div class="opgg-team-card">
    <PlayerStatsCard v-for="player in players" :key="player.puuid" :player="player" />
  </div>
</template>

<script setup>
const props = defineProps({
  players: {
    type: Array,
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
