<template>
  <div>
    <Button type="button" @click="open()">
      Choose files
    </Button>
    <Button type="button" :disabled="!files" @click="reset()">
      Reset
    </Button>

    <Button type="button" :disabled="!files" @click="() => upload()">
      Upload
    </Button>

    <template v-if="files">
      <p>
        You have selected:
        <b>{{ `${files.length} ${files.length === 1 ? "file" : "files"}` }}</b>
      </p>
      <li v-for="file of files" :key="file.name">
        {{ file.name }}
      </li>
    </template>

    <Toast />
  </div>
</template>

<script setup>
const { files, open, reset, onChange } = useFileDialog();
const toast = useToast();
const client = useSupabaseClient();

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
