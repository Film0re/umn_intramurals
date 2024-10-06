<template>
  <div v-if="matchData" class="dashboard-container">
    <!-- <h2>{{ matchData.blueTeam.name }} vs {{ matchData.redTeam.name }} graphs </h2> -->
    <h2>Graphs</h2>
    <div class="grid-container">
      <div v-for="(stat, index) in stats" :key="index" class="stat-chart">
        <TeamComparisonChart
          :match-data="matchData"
          :stat-key="stat"
          :chart-title="statTitles[stat]"
        />
      </div>
    </div>
  </div>
</template>

<script setup>
const props = defineProps({
  matchData: Object,
});

const stats = [
  "kills",
  "total_damage_dealt_to_champions",
  "total_damage_taken",
  "gold_earned",
  "wards_placed",
  "cs",
];

const statTitles = {
  kills: "Kills",
  deaths: "Deaths",
  assists: "Assists",
  gold_earned: "Gold Earned",
  cs: "CS",
  total_damage_dealt_to_champions: "Total Damage Dealt to Champions",
  total_damage_taken: "Total Damage Taken",
  wards_placed: "Wards Placed",
};
</script>

<style scoped>
.dashboard-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
}

.grid-container {
  display: grid;
  grid-template-columns: repeat(2, 1fr); /* Default to 2 columns */
  grid-template-rows: repeat(3, auto); /* 3 rows by default */
  gap: 1rem;
  width: 100%;
}

.stat-chart {
  width: 100%;
  margin-bottom: 1rem;
}

/* Media query for small screens: switch to 1 column layout */
@media (max-width: 768px) {
  .grid-container {
    grid-template-columns: 1fr; /* 1 column */
    grid-template-rows: repeat(6, auto); /* 6 rows for each stat */
  }
}
</style>
