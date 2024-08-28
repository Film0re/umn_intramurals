<template>
  <div class="bg-[#1c1c1c] min-h-screen p-4 md:p-8 lg:p-12">
    <div class="max-w-7xl mx-auto">
      <div class="bg-[#252525] p-4 md:p-6 lg:p-8 rounded-lg shadow-md">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-2xl md:text-3xl lg:text-4xl font-bold text-gray-200">
            Match Summary
          </h2>
        </div>
        <p class="text-center text-gray-400 mb-4 md:mb-6 lg:mb-8">
          Game Length: {{ formatDuration(matchInfo?.duration) }}
        </p>
        <div class="grid md:grid-cols-2 gap-4 md:gap-6 lg:gap-8">
          <div
            v-for="(team, index) in ['blueTeam', 'redTeam']"
            :key="team"
            :class="{
              'bg-blue-900': index === 0,
              'bg-red-900': index === 1,
              'ring-4 ring-yellow-600': matchInfo?.winner === team,
            }"
            class="p-4 md:p-6 rounded-md relative"
          >
            <h3
              class="text-xl md:text-2xl font-semibold mb-2 md:mb-4"
              :class="{
                'text-blue-200': index === 0,
                'text-red-200': index === 1,
              }"
            >
              {{ index === 0 ? "Blue Team" : "Red Team" }}
              <span
                v-if="matchInfo?.winner === team"
                class="ml-2 text-yellow-400"
              >
                (Winner)
              </span>
            </h3>
            <div
              v-for="player in matchInfo[team]"
              :key="player.summonerName"
              class="mb-2 md:mb-3 p-2 md:p-3 bg-[#333333] rounded shadow-sm"
            >
              <div class="flex items-center space-x-2 md:space-x-3">
                <img
                 :src="`/img/champion/${player.champion}.png`"
                  :alt="player.champion"
                  class="w-8 h-8 md:w-10 md:h-10 rounded-full"
                />
                <span class="font-medium text-gray-200 text-sm md:text-base">{{
                  player.summonerName
                }}</span>
                <span class="text-gray-400 text-sm md:text-base">{{
                  player.champion
                }}</span>
              </div>
              <div class="mt-1 md:mt-2 flex justify-between items-center">
                <span class="text-sm md:text-base text-gray-300">
                  {{ player.kills }}/{{ player.deaths }}/{{ player.assists }}
                  <span class="text-gray-400 ml-2">{{ player.cs }} CS</span>
                </span>
                <div class="flex space-x-1 md:space-x-2">
                  <img
                    v-for="item in player.items"
                    :key="item"
                    :src="`/img/item/${item}.png`"
                    alt="Item"
                    class="w-6 h-6 md:w-8 md:h-8 rounded"
                  />
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

const matchInfo = computed(() => {
  const { team1, team2 } = props.match;

  const transformTeam = (team) => {
    return team.players.map(player => ({
      winner: team.id === props.match.winner.id,
      summonerName: player.name,
      champion: player.skin,
      kills: player.champions_killed,
      deaths: player.num_deaths,
      assists: player.assists,
      cs: player.minions_killed,
      items: [player.item0, player.item1, player.item2, player.item3, player.item4, player.item5]
    }));
  };

  return {
    duration: props.match.duration,
    gameMode: props.match.gameMode,
    winner: props.match.winner.id === team1.id ? 'blueTeam' : 'redTeam',
    blueTeam: transformTeam(team1),
    redTeam: transformTeam(team2)
  };
});

// Mock data for the match
const matchInfo3 = ref({
  gameMode: "Ranked Solo",
  duration: 2100, // 35 minutes in seconds
  winner: "blueTeam", // 'blueTeam' or 'redTeam'
  blueTeam: [
    {
      summonerName: "Player1",
      champion: "Ahri",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 8,
      deaths: 3,
      assists: 12,
      cs: 180,
      items: Array(6).fill("/placeholder.svg?height=32&width=32"),
    },
    {
      summonerName: "Player2",
      champion: "Lee Sin",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 5,
      deaths: 4,
      assists: 15,
      cs: 150,
      items: Array(6).fill("/placeholder.svg?height=32&width=32"),
    },
    {
      summonerName: "Player3",
      champion: "Darius",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 6,
      deaths: 2,
      assists: 9,
      cs: 200,
      items: Array(6).fill("/placeholder.svg?height=32&width=32"),
    },
    {
      summonerName: "Player4",
      champion: "Jinx",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 12,
      deaths: 1,
      assists: 7,
      cs: 220,
      items: Array(6).fill("/placeholder.svg?height=32&width=32"),
    },
    {
      summonerName: "Player5",
      champion: "Thresh",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 1,
      deaths: 5,
      assists: 22,
      cs: 40,
      items: Array(6).fill("/placeholder.svg?height=32&width=32"),
    },
  ],
  redTeam: [
    {
      summonerName: "Player6",
      champion: "Zed",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 7,
      deaths: 6,
      assists: 4,
      cs: 170,
      items: Array(6).fill("/placeholder.svg?height=32&width=32"),
    },
    {
      summonerName: "Player7",
      champion: "Elise",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 4,
      deaths: 7,
      assists: 11,
      cs: 140,
      items: Array(6).fill("/placeholder.svg?height=32&width=32"),
    },
    {
      summonerName: "Player8",
      champion: "Malphite",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 2,
      deaths: 4,
      assists: 15,
      cs: 160,
      items: [6672,6672,6672,6672,6672,6672]
    },
    {
      summonerName: "Player9",
      champion: "Caitlyn",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 9,
      deaths: 5,
      assists: 6,
      cs: 210,
      items: Array(6).fill("/placeholder.svg?height=32&width=32"),
    },
    {
      summonerName: "Player10",
      champion: "Lulu",
      championIcon: "/placeholder.svg?height=40&width=40",
      kills: 0,
      deaths: 6,
      assists: 18,
      cs: 30,
      items: Array(6).fill("/placeholder.svg?height=32&width=32"),
    },
  ],
});

const formatDuration = (seconds) => {
  const minutes = Math.floor(seconds / 60);
  const remainingSeconds = seconds % 60;
  return `${minutes}m ${remainingSeconds}s`;
};
</script>
