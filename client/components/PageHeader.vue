<template>
  <header class="header">
    <a href="/" class="logo-link">
      <img src="/club_logo.png" alt="Club Logo" class="logo">
    </a>
    <div v-if="user" class="logout-container">
      <Button label="Logout" @click="logout" />
    </div>
  </header>
</template>

<script setup>
import Button from "primevue/button";
const user = useSupabaseUser();
const client = useSupabaseClient();
const router = useRouter();

const logout = async () => {
  const { error } = await client.auth.signOut();
  if (error) {
    // eslint-disable-next-line no-console
    console.log(error);
    return;
  }

  router.push("/");
};
</script>
<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  background-color: var(--background-color);
  border-radius: var(--border-radius);
  width: 100%;
}

.logo-link {
  display: flex;
  align-items: center;
  text-decoration: none;
}

.logo {
  height: 2.5rem;
  width: auto;
  margin-right: 0.5rem;
}

</style>
