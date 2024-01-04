<template>
  <div>
    <div class="outer-container">
      <Card>
        <template #header style="text-align: center">
          <h1 style="text-align: center">Login</h1>
        </template>

        <template #content>
          <div class="center">
            <span class="p-float-label">
              <InputText id="email" v-model="email" />
              <label for="email">Email</label>
            </span>

            <span class="p-float-label" style="margin-top: 2rem">
              <Password id="password" v-model="password" />
              <label for="password">Password</label>
            </span>
            <p v-if="errorMsg" class="danger">{{ errorMsg }}</p>

            <Button label="Login" @click="login" style="margin-top: 1rem" />
          </div>
        </template>
      </Card>
    </div>
  </div>
</template>

<script setup>
import Card from "primevue/card";
import InputText from "primevue/inputtext";
import Password from "primevue/password";
import Button from "primevue/button";

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
    errorMsg.value = error.message;
  } else {
    router.push("/profile");
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
