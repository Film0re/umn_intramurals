export default defineNuxtRouteMiddleware(async () => {
    const user = useSupabaseUser();
    const supabase = useSupabaseClient();

    // Call RPC to check if user is admin
    const { data, error } = await supabase.rpc('is_admin');
    if (error) {
        console.error(error);
    }

    // If the user is not logged in, redirect to home page
    if (data !== true) {
        return navigateTo('/');
    }
});