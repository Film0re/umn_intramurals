<template>
  <div v-if="matchData" class="chart-container">
    <h3>{{ chartTitle }}</h3>
    <Chart
      type="bar"
      :data="chartData"
      :options="chartOptions"
      :plugins="chartPlugins"
      class="chart"
    />
  </div>
</template>

<script setup>
import { computed, ref } from 'vue';
import Chart from 'primevue/chart';
import ChartDataLabels from 'chartjs-plugin-datalabels';

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

const chartPlugins = ref([ChartDataLabels]);

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

const formatNumber = (num) => {
  return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
};

const chartOptions = computed(() => ({
  indexAxis: 'y',
  responsive: true,
  maintainAspectRatio: false,
  animation: false,
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
            label += formatNumber(context.parsed.x);
          }
          return label;
        }
      }
    },
    datalabels: {
      anchor: 'end',
      align: 'right',
      offset: 4,
      color: '#F0F0F0',
      font: {
        weight: 'bold',
      },
      padding: 4,
      formatter: (value) => formatNumber(value)
    }
  },
  scales: {
    x: {
      beginAtZero: true,
      title: {
        display: true,
        text: props.chartTitle
      },
      ticks: {
        callback: function(value) {
          return formatNumber(value);
        }
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