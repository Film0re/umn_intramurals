<template>
  <div>
    <h1>Matches</h1>
    <ul>
      <li v-for="match in matches" :key="match.id">
        <nuxt-link :to="'/matches/' + match.id">
          {{ match.name }}
        </nuxt-link>
      </li>
    </ul>
  </div>
</template>

<script>


</script>