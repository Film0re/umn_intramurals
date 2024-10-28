<template>
  <div class="team-container">
    <Card>
      <template #header>
        <div class="team-header">
          <h2 class="team-name">
            {{ team.name }}
          </h2>
          <div class="team-stats">
            <p class="season">
              Season {{ team.season }}
            </p>
            <p class="record">
              {{ team.wins }} W - {{ team.games_played - team.wins }} L
              <span class="win-rate">
                ({{ calculateWinRate }}% WR)
              </span>
            </p>
          </div>
        </div>
      </template>
      <template #content>
        <div class="players-list">
          <div 
            v-for="player in team.players" 
            :key="player.player_id"
            class="player-row"
          >
            <span class="player-name">{{ player.name }}</span>
            
            <a
              v-if="player.name && player.tag_line"
              :href="getOpGGLink(player.name, player.tag_line)"
              target="_blank"
              rel="noopener noreferrer"
              class="opgg-link"
            >
              op.gg
            </a>
          </div>
        </div>
      </template>

      <template #footer>
        Most op.gg links will be broken as I do not have up-to-date riot-ids and tag-lines for the players. This data must be updated manually :(
      </template>
    </Card>
  </div>
</template>

<script setup>

const props = defineProps({
    team: {
        type: Object,
        required: true
    }
});

const calculateWinRate = computed(() => ((props.team.wins / props.team.games_played) * 100).toFixed(1));

const getOpGGLink = (name, tagLine) => 
    `https://op.gg/summoners/na/${encodeURIComponent(name)}-${encodeURIComponent(tagLine)}`;
</script>

<style scoped>
.team-container {
  width: 100%;
  max-width: 40rem;
  margin: 2rem auto 0;
}
.team-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
}
.team-name {
  font-size: 1.875rem;
  font-weight: bold;
  color: var(--text-color);
}
.team-stats {
  text-align: right;
}
.season {
  font-size: 1.125rem;
  font-weight: 600;
  color: var(--text-color);
}
.record {
  color: var(--text-color-secondary);
}
.win-rate {
  margin-left: 0.5rem;
  font-size: 0.875rem;
}
.players-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
.player-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem;
  background-color: var(--surface-card);
  border-radius: 0.5rem;
  transition: background-color 0.2s;
}
.player-row:hover {
  background-color: var(--surface-hover);
}
.player-name {
  font-weight: 500;
  color: var(--text-color);
}
.opgg-link {
  color: var(--primary-color);
  text-decoration: none;
  transition: color 0.2s;
}
.opgg-link:hover {
  color: var(--primary-color-text);
}
</style>