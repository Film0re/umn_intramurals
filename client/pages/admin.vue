<template>
  <div>
    Hello >:)

  </div>
</template>

<script setup>
const user = useSupabaseUser();

definePageMeta({
  middleware: ["admin-auth"],
});
</script>
