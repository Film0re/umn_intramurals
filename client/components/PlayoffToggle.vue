<template>
  <div>
    <span>
      <ToggleButton v-model="isPlayoffs" on-label="Playoffs" off-label="Regular" />
    </span>
  </div>
</template>

<script setup>
const isPlayoffs = usePlayoffs();
</script>
