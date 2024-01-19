<template>
  <div class="opgg-match-card">
    <div class="opgg-match-card__teams">
      <div class="flex-column">
        <div class="flex-row">
          <h3>{{ match.team1.name }}</h3>
        </div>
        <MatchTeamCard :team="match.team1" :class="{ winner: match.team1.win }" />
      </div>
      <div class="flex-column">
        <h3>{{ match.team2.name }}</h3>
        <MatchTeamCard :team="match.team2" :class="{ winner: match.team2.win }" />
      </div>
    </div>
  </div>
</template>

<script setup>
const props = defineProps({
  match: {
    type: Object,
    required: true,
  },
});

</script>

<style scoped>
.opgg-match-card {
  display: flex;
  justify-content: space-around;
  flex-direction: column;
  align-items: center;
}

.opgg-match-card__teams {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  width: 100%;
}

.winner {
  border: 2px solid var(--primary-600);
}

.flex-row {
  display: flex;
  flex-direction: row;
  align-items: center;
}

.flex-column {
  display: flex;
  flex-direction: column;
  align-items: center;
}
</style>
