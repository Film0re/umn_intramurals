<template>
  <div class="full-screen">
    <PageHeader />
    <NuxtPage />
    <PageFooter />
  </div>
</template>

<script setup>

useSeoMeta({
  title: 'UMN LoL Intramurals',
  description: 'View all the teams and their standings in the UMN LoL Intramurals',
  ogTitle: 'UMN LoL Intramurals',
  ogDescription: 'Check out the standings for the UMN LoL Intramurals!',
  ogImage: '/club_logo.png',
  ogUrl: 'https://uofm.lol/',
  twitterTitle: 'UMN LoL Intramurals',
  twitterDescription: 'Check out the standings for the UMN LoL Intramurals!',
  twitterImage: '/club_logo.png',
  twitterCard: 'summary'
})

useHead({
  htmlAttrs: {
    lang: 'en'
  },
  link: [
    {
      rel: 'icon',
      type: 'image/png',
      href: '/favicon.ico'
    },
  ],
  script: [
    {
      src: '/stats/script.js',
      'data-website-id': 'ecf1e18c-f544-4e38-89ee-7096336709d8',
      defer: true
    }
  ]
})
</script>

<style scoped>
.full-screen {
  display: flex;
  flex-direction: column;
  align-items: center;
  min-height: 98dvh;
  justify-content: space-between;
  margin: 0; /* Remove default margin */
  overflow: hidden; /* Prevents content from overflowing and triggering a scrollbar */
}

#app{
  min-height: 100dvh;
  margin: 0;
}

</style>
