export const GITHUB_OAUTH_CONFIG = {
    client_id: import.meta.env.VITE_GITHUB_CLIENT_ID,
    redirect_uri: `${window.location.origin}/callback/auth`,
    scope: "user:email",
    response_type: "code"
};