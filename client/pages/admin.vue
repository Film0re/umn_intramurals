<template>
  <div class="full-height">
    <TabView v-show="playersTableLoaded">
      <TabPanel header="Player Stats">
        <PlayerStatsTable @loaded="() => (playersTableLoaded = true)" />
      </TabPanel>

      <TabPanel header="Matches">
        <MatchesTable @loaded="() => (matchesTableLoaded = true)" />
      </TabPanel>

      <TabPanel header="Upload">
        <UploadMatch />
      </TabPanel>
    </TabView>

    <ProgressBar
      v-show="!playersTableLoaded"
      mode="indeterminate"
      style="height: 6px"
    />
  </div>
</template>

<script setup>
definePageMeta({
  middleware: ["admin-auth"],
});

const playersTableLoaded = ref(true);
const matchesTableLoaded = ref(false);
</script>

<style scoped>
.full-height {
  width: clamp(300px, 100%, 1000px);
  margin: 0 auto;
  padding: 1rem;
  text-align: center;
  height: 100%;
  margin-bottom: auto;
}
</style>
