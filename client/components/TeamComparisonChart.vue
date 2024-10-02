<template>
  <div v-if="matchData" class="chart-container">
    <h3>{{ chartTitle }}</h3>
    <Chart
      type="bar"
      :data="chartData"
      :options="chartOptions"
      class="chart"
    />
  </div>
</template>

<script setup>
import { computed } from 'vue';
import Chart from 'primevue/chart';

const props = defineProps({
  matchData: {
    type: Object,
    required: true
  },
  statKey: {
    type: String,
    required: true
  },
  chartTitle: {
    type: String,
    required: true
  }
});

const chartData = computed(() => {
  if (!props.matchData) return {};

  const blueTeam = props.matchData.blueTeam.players;
  const redTeam = props.matchData.redTeam.players;

  return {
    labels: [
      ...blueTeam.map(p => p.summonerName),
      ...redTeam.map(p => p.summonerName)
    ],
    datasets: [{
      data: [
        ...blueTeam.map(p => p[props.statKey]),
        ...redTeam.map(p => p[props.statKey])
      ],
      backgroundColor: [
        ...blueTeam.map(() => 'rgba(54, 162, 235, 0.8)'),
        ...redTeam.map(() => 'rgba(255, 99, 132, 0.8)')
      ],
      borderColor: [
        ...blueTeam.map(() => 'rgb(54, 162, 235)'),
        ...redTeam.map(() => 'rgb(255, 99, 132)')
      ],
      borderWidth: 1
    }]
  };
});

const chartOptions = computed(() => ({
  indexAxis: 'y',
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      display: false
    },
    tooltip: {
      callbacks: {
        label: function(context) {
          let label = context.label || '';
          if (label) {
            label += ': ';
          }
          if (context.parsed.x !== null) {
            label += context.parsed.x;
          }
          return label;
        }
      }
    }
  },
  scales: {
    x: {
      beginAtZero: true,
      title: {
        display: true,
        text: props.chartTitle
      }
    },
    y: {
      title: {
        display: true,
        text: 'Players'
      },
      ticks: {
        autoSkip: false,
        callback: function(value) {
          return this.getLabelForValue(value);
        }
      }
    }
  }
}));
</script>

<style scoped>
.chart-container {
  width: 100%;
  height: 16rem;
  margin-bottom: 20px;
}

.chart {
  height: 100%;
  width: 100%;
}
</style>