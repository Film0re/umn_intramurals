<template>
  <div>
    <div class="outer-container">
      <Card>
        <template #header>
          <h1 style="text-align: center">
            Login
          </h1>
        </template>

        <template #content>
          <form class="center">
            <span class="p-float-label">
              <InputText id="email" v-model="email" />
              <label for="email">Email</label>
            </span>

            <span class="p-float-label" style="margin-top: 2rem">
              <Password id="password" v-model="password" :feedback="false" />
              <label for="password">Password</label>
            </span>
            <p v-if="errorMsg" class="danger">
              {{ errorMsg }}
            </p>

            <Button label="Login" style="margin-top: 1rem" @click="login" />
          </form>
        </template>
      </Card>
    </div>
  </div>
</template>

<script setup>
// use seo meta to not show up in search engines
useSeoMeta({
  title: "Login",
  description: "Login to the admin dashboard",
  ogTitle: "Login",
  ogDescription: "Login to the admin dashboard",
  ogImage: "/club_logo.png",
  ogUrl: "https://uofm.lol/",
  twitterTitle: "Login",
  twitterDescription: "Login to the admin dashboard",
  twitterImage: "/club_logo.png",
  twitterCard: "summary",
});

const router = useRouter();
const client = useSupabaseClient();

const email = ref(null);
const password = ref(null);
const errorMsg = ref(null);

const login = async () => {
  const { error } = await client.auth.signInWithPassword({
    email: email.value,
    password: password.value,
  });

  if (error) {
    // eslint-disable-next-line no-console
    console.log(error);
    errorMsg.value = error.message;
  } else {
    router.push("/admin");
  }
};
</script>

<style scoped>
.danger {
  color: var(--red-500);
}
.outer-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100vh;
}

.center {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
</style>
