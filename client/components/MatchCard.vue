<template>
  <div class="opgg-match-card">
    <TeamCard :players="team100" />
    <TeamCard :players="team200" />
  </div>
</template>

<script setup>
const props = defineProps({
  matchData: {
    type: Array,
    required: true,
  },
});

// Map the role to a number so we can sort by role
const roleMap = {
  TOP: 0,
  JUNGLE: 1,
  MIDDLE: 2,
  BOTTOM: 3,
  UTILITY: 4,
};

// Assuming matchData is an array of player objects
const team100 = computed(() =>
  props?.matchData
    ?.filter((player) => player.team === 100)
    ?.sort((a, b) => roleMap[a.team_position] - roleMap[b.team_position])
);
const team200 = computed(() =>
  props?.matchData
    ?.filter((player) => player.team === 200)
    ?.sort((a, b) => roleMap[a.team_position] - roleMap[b.team_position])
);
</script>

<style scoped>
.opgg-match-card {
  display: flex;
  justify-content: space-around;
}

/* Add any additional styling as needed */
</style>
