<template>
  <div>
    <DataTable
      :value="matches"
      :paginator="true"
      :rows="10"
      :rowsPerPageOptions="[10, 25, 50]"
    >
      <Column field="match_id" header="Match ID"></Column>
      <Column field="game_version" header="Match Date"></Column>
      <Column field="game_mode" header="Game Mode"></Column>
    </DataTable>

    <div>
      <Button type="button" @click="open()">Choose files</button>
      <Button type="button" :disabled="!files" @click="reset()">Reset</button>

      <Button type="button" :disabled="!files" @click="() => upload()">Upload</button>

      <template v-if="files">
        <p>
          You have selected:
          <b>{{
            `${files.length} ${files.length === 1 ? "file" : "files"}`
          }}</b>
        </p>
        <li v-for="file of files" :key="file.name">
          {{ file.name }}
        </li>
      </template>
    </div>

    <Toast />
</div>
</template>

<script setup>
definePageMeta({
  middleware: ["admin-auth"],
});

const user = useSupabaseUser();
const client = useSupabaseClient();
const router = useRouter();
const matches = ref([]);
const toast = useToast();

onMounted(() => {
  getMatches();
  getTeams();
});


const getTeams = async () => {
  const { data, error } = await 
  client.from("teams")
  .select(`
  *,
  team_players (
    player: players (
      name,
      match_data: match_data (
        champions_killed,
        bait_pings
      )
    )
  )
  `)
  .eq("season", 5);

  if (error) {
    console.log(error);
  } else {
    console.log(data);
  }
};


// TODO: Filter only .rofl files
const getMatches = async () => {
  const { data, error } = await 
  client.from("matches")
  .select("*");

  if (error) {
    console.log(error);
  } else {
    matches.value = data;
  }
};

const { files, open, reset, onChange } = useFileDialog();


// Iterate over files and upload them
// to the "Replays" bucket
// Using promise.all would be faster but would also make it harder to
// figure out which file failed to upload
const upload = async () => {
  for (const file of files.value) {
    const { data, error } = await client.storage
      .from("Replays")
      .upload(file.name, file);

    if (error) {
      toast.add({
        severity: "error",
        summary: "Error",
        detail: error.message,
        life: 3000,
      });

    } else {
      toast.add({
        severity: "success",
        summary: "Success",
        detail: "File uploaded",
        life: 3000,
      });
    }
  }
};
</script>
