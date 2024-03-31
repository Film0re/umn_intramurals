<template>
  <div class="opgg-player-card">
    <Card class="opgg-card">
      <template #content>
        <div class="opgg-card-content">
          <div class="opgg-card-header">
            <img class="opgg-champion-icon" :src="`/img/champion/${player.skin}.png`">
            <div class="opgg-player-info">
              <div class="opgg-player-name">
                {{ player.name }}
              </div>
              <div class="opgg-kda">
                {{ player.champions_killed }}/{{ player.num_deaths }}/{{ player.assists }}
                <span class="opgg-kda-ratio" :style="{ color: getPlayerKDAColor() }">
                  {{ ((player.champions_killed + player.assists) / player.num_deaths).toFixed(2) }}
                </span>
                -
                <span class="opgg-gold-earned">
                  {{ formatGold(player.gold_earned) }} Gold
                </span>
              </div>
            </div>
          </div>
          <!--         
          <div class="opgg-spells">
            <img :src="`/img/spell/${player.perk_primary_style}.png`">
            <img :src="`/img/spell/${player.summoner_spell_2}.png`">
          </div> -->

          <div class="opgg-items">
            <div class="opgg-items">
              <div v-for="(row, index) in 2" :key="index" class="opgg-item-row">
                <img
                  v-for="(item, itemIndex) in [player.item0, player.item1, player.item2, player.item3, player.item4, player.item5, player.item6].slice(index * 3, (index + 1) * 3)"
                  :key="itemIndex" :src="`/img/item/${item}.png`" class="opgg-item-icon"
                  style="border: 1px solid grey;">
              </div>
            </div>
          </div>
        </div>
      </template>
    </Card>
  </div>
</template>

<script setup>
const props = defineProps({
  player: {
    type: Object,
    required: true,
  },
});

const formatGold = (gold) => {
  if (gold >= 1000) {
    return `${(gold / 1000).toFixed(1)}k`;
  }

  return gold;
};

const getKdaRatio = () => ((props.player.champions_killed + props.player.assists) / props.player.num_deaths);

const getPlayerKDAColor = () => {
  const kdaRatio = getKdaRatio();

  const colorMap = {
    0: 'var(--grey-600)',
    1: 'var(--grey-600)',
    3: 'var(--green-600)',
    4: 'var(--teal-600)',
    5: 'var(--orange-600)',
  };

  return colorMap[Math.floor(kdaRatio)];
};

</script>

<style scoped>
.opgg-player-card {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 0.5rem;
}

.opgg-card {
  width: 250px;
  /* Adjust the card width as needed */
  flex-direction: column;
  align-items: center;

}

.opgg-card-header {
  display: flex;
  align-items: center;
}


.opgg-champion-icon {
  height: 2rem;
  /* Reduce the height of the champion icon */
  width: 2rem;
  margin-right: 1rem;
}

.opgg-player-info {
  display: flex;
  flex-direction: column;
}

.opgg-player-name {
  font-size: 1rem;
  /* Reduce the font size of the player name */
  font-weight: bold;
}

.opgg-kda {
  font-size: 0.8rem;
  /* Reduce the font size of the KDA */
  color: #888;
}

.opgg-kda-ratio {
  color: #4CAF50;
  /* Green color for KDA ratio */
}

.opgg-spells {
  display: flex;
  margin-top: 0.5rem;
  /* Reduce the top margin for summoner spells */
}

.opgg-items {
  display: flex;
  margin-top: 0.5rem;
  /* Reduce the top margin for items */
}

.opgg-item-icon {
  height: 1.5rem;
  /* Reduce the height of item icons */
  width: 1.5rem;
  margin-right: 0.25rem;
  /* Reduce the right margin for item icons */
}
</style>
