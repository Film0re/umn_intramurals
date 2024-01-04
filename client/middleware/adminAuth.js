export default defineNuxtRouteMiddleware(() => {
    const user = useSupabaseUser();
    const router = useRouter();
    // If the user is not logged in, redirect to home page
    if (!user) {
        return navigateTo('/');
    }

    // If the user is logged in but not an admin, redirect to home page
    if (user && user.user_metadata && !user.user_metadata.is_admin) {
        return navigateTo('/');
    }

    // If the user does not have a user_metadata object or is_admin property, redirect to home page
    if (user && (!user.user_metadata || !user.user_metadata.is_admin)) {
        return navigateTo('/');
    }

});