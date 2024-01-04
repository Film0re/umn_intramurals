<template>
  <div>
    <div v-if="user">
        <Button label="Logout" @click="logout" />
    </div>
  </div>
</template>

<script setup>
import Button from "primevue/button";
const user = useSupabaseUser();
const client = useSupabaseClient();
const router = useRouter();

const logout = async () => {
  const { error } = await client.auth.signOut();
    if (error) {
        console.log(error);
        return;
    }

  router.push("/");
};

const login = async () => {
  router.push("/login");
};


</script>
