<template>
  <div>
    <pre>{{ user }}</pre>

  </div>
</template>

<script setup>
const user = useSupabaseUser();
const client = useSupabaseClient();
const router = useRouter();

</script>
