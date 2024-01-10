<template>
  <div class="full-screen">
    <Header />
    <NuxtPage />
    <PageFooter />
  </div>
</template>

<script setup></script>

<style scoped>
.full-screen {
  display: flex;
  flex-direction: column;
  align-items: center;
  height: 98dvh;
  justify-content: space-between;
  margin: 0; /* Remove default margin */
  overflow: hidden; /* Prevents content from overflowing and triggering a scrollbar */
}

#app{
  height: 100dvh;
  margin: 0;
}

</style>
