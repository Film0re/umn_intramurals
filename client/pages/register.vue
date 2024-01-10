<template>
  <div>
    <label for="email">Email</label>
    <InputText id="email" v-model="email" />

    <label for="password">Password</label>
    <Password id="password" v-model="password" toggle-mask />

    <Button label="Register" @click="register" />
    <p v-if="errorMsg" class="danger">{{ errorMsg }}</p>
  </div>
</template>

<script setup>
const client = useSupabaseClient();
const email = ref("");
const password = ref(null);
const errorMsg = ref(null);
const successMsg = ref(null);

const register = async () => {
  const { data, error } = await client.auth.signUp({
    email: email.value,
    password: password.value,
  });

  console.log(data);

  if (error) {
    console.log(error);
    errorMsg.value = error.message;
    console.log("error: ", error.value);
  } else {
    successMsg.value = "Success! Check your email to confirm your account.";
    errorMsg.value = null;
  }
};
</script>

<style scoped>
.danger {
  color: var(--red-500);
}
</style>
