const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function apiPost(endpoit: string, data?: any) {
    const response = await fetch(`${API_BASE_URL}${endpoit}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: data ? JSON.stringify(data) : undefined,
        credentials: "include"
    });

    if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "API error");
    }

    return response.json();
}

export async function apiGet(endpoint: string) {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
        method: "GET",
        credentials: "include"
    });

    if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "API error");
    }

    return response.json();
}