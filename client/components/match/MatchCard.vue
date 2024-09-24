<template>
  <div class="match-summary">
    <div class="container">
      <div class="summary-card">
        <div
          class="header"
          style="
            display: flex;
            justify-content: center;
            align-items: center;
            margin-bottom: 1rem;
          "
        >
          <h2 class="title">
            Match Summary
          </h2>
        </div>
        <p class="game-length">
          Game Length: {{ formatDuration(match.duration) }}
        </p>
        <div class="teams-container">
          <div
            v-for="(team, index) in [match.blueTeam, match.redTeam]"
            :key="team?.id"
            :class="[
              'team',
              index === 0 ? 'blue-team' : 'red-team',
              { winner: team?.win },
            ]"
          >
            <h3 class="team-title">
              {{ team?.name }}
              <span v-if="team.win" class="winner-label"> (Winner) </span>
            </h3>
            <div
              v-for="player in team?.players"
              :key="player?.summonerName"
              class="player-card"
            >
              <div class="player-info">
                <img
                  :src="`/img/champion/${player.champion}.png`"
                  :alt="player.champion"
                  class="champion-icon"
                >
                <span class="summoner-name">{{ player?.summonerName }}</span>
                <span class="champion-name">{{ player?.champion }}</span>
              </div>
              <div class="player-stats">
                <span class="kda">
                  {{ player.kills }}/{{ player?.deaths }}/{{ player.assists }}
                  <span class="cs">{{ player?.cs }} CS ({{(  player.cs / match.duration * 60  ).toFixed(1)}})</span> 
                </span>
                <div class="items">
                  <img
                    v-for="item in player.items || [0, 0, 0, 0, 0, 0]"
                    :key="item"
                    :src="`/img/item/${item}.png`"
                    alt="Item"
                    class="item-icon"
                    style="border: 1px solid gray; border-radius: 0.125rem"
                  >
                </div>
              </div>
            </div>
          </div>
        </div>
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

console.log(props.match);

const formatDuration = (seconds) => {
  const minutes = Math.floor(seconds / 60);
  const remainingSeconds = seconds % 60;
  return `${minutes}m ${remainingSeconds}s`;
};
</script>

<style scoped>
.match-summary {
  background-color: #1c1c1c;
  min-height: 100vh;
  padding: 1rem;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
}

.summary-card {
  background-color: #252525;
  padding: 1rem;
  border-radius: 0.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.title {
  font-size: 1.5rem;
  font-weight: bold;
  color: #e0e0e0;
}

.game-length {
  text-align: center;
  color: #a0a0a0;
  margin-bottom: 1rem;
}

.teams-container {
  display: grid;
  gap: 1rem;
}

.team {
  padding: 1rem;
  border-radius: 0.375rem;
  position: relative;
}

.blue-team {
  background-color: #2e3bcf;
}

.red-team {
  background-color: #ca3d3d;
}

.winner {
  box-shadow: 0 0 0 4px #ffd700;
}

.team-title {
  text-align: center;
  font-size: 1.25rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
  color: #e0e0e0;
}

.winner-label {
  margin-left: 0.5rem;
  color: #ffd700;
}

.player-card {
  background-color: #333333;
  margin-bottom: 0.5rem;
  padding: 0.5rem;
  border-radius: 0.25rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.player-info {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.champion-icon {
  width: 2rem;
  height: 2rem;
  border-radius: 50%;
}

.summoner-name {
  font-weight: 500;
  color: #e0e0e0;
}

.champion-name {
  color: #a0a0a0;
}

.player-stats {
  display: flex;
  align-items: center;
  margin-top: 0.25rem;
  padding-bottom: 0.25rem;
}

.kda {
  font-size: 0.875rem;
  color: #e0e0e0;
}

.cs {
  color: #a0a0a0;
  margin-left: 0.5rem;
}

.items {
  display: flex;
  margin-left: auto;
  margin-right: auto;
  gap: 0.25rem;
}

.item-icon {
  width: 1.5rem;
  height: 1.5rem;
  border-radius: 0.125rem;
}

@media (min-width: 768px) {
  .match-summary {
    padding: 2rem;
  }

  .summary-card {
    padding: 1.5rem;
  }

  .title {
    font-size: 1.875rem;
  }

  .teams-container {
    grid-template-columns: repeat(2, 1fr);
  }

  .team-title {
    font-size: 1.5rem;
  }

  .champion-icon {
    width: 2.5rem;
    height: 2.5rem;
  }

  .item-icon {
    width: 2rem;
    height: 2rem;
  }
}

@media (min-width: 1024px) {
  .match-summary {
    padding: 3rem;
  }

  .summary-card {
    padding: 2rem;
  }

  .title {
    font-size: 2.25rem;
  }
}
</style>
