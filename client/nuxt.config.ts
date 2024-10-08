// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      supabaseUrl: process.env.NUXT_PUBLIC_SUPABASE_URL,
      supabaseKey: process.env.NUXT_PUBLIC_SUPABASE_KEY,
    },
  },

  modules: ['nuxt-primevue', '@nuxtjs/supabase', '@vueuse/nuxt', '@nuxt/image'],

  supabase: {
    url: process.env.NUXT_PUBLIC_SUPABASE_URL,
    key: process.env.NUXT_PUBLIC_SUPABASE_KEY,
    redirect: false,
  },

  image: {
    format: ['webp'],
  },

  css: ['primevue/resources/themes/lara-dark-pink/theme.css']
});