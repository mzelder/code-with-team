export const OAUTH_PROD_CONFIG = {
    client_id: import.meta.env.VITE_GITHUB_CLIENT_ID,
    redirect_uri: `${window.location.origin}/callback/auth`,
    scope: "user:email",
    response_type: "code"
};

export const OAUTH_MOCK_CONFIG = {
    client_id: "test-client-id",
    redirect_uri: `${window.location.origin}/callback/auth`,
    scope: "user:email",
    response_type: "code"
}

export const OAUTH_CONFIG = import.meta.env.VITE_MOCK_AUTH === "true"
    ? OAUTH_MOCK_CONFIG
    : OAUTH_PROD_CONFIG;

export const MOCK_URL = "https://oauth-mock.mock.beeceptor.com/oauth/authorize";
export const PROD_URL = "https://github.com/login/oauth/authorize";

export const OAUTH_URL = import.meta.env.VITE_MOCK_AUTH === "true"
    ? MOCK_URL
    : PROD_URL;

export const AVATAR_URL = "https://avatars.githubusercontent.com/";