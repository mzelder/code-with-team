const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

type HttpMethod = "GET" | "POST" | "PUT" | "DELETE";

export async function apiRequest(method: HttpMethod, endpoit: string, data?: any) {
    const options: RequestInit = {
        method,
        headers: { "Content-Type": "application/json" },
        credentials: "include"
    };

    if (data) options.body = JSON.stringify(data);
    
    const response = await fetch(`${API_BASE_URL}${endpoit}`, options);

    if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "API error");
    }

    return response.json();
}

export const apiGet = async(endpoint: string) => await apiRequest("GET", endpoint);
export const apiPost = async(endpoint: string, data?: any) => await apiRequest("POST", endpoint, data);
export const apiPut = async(endpoint: string, data?: any) => await apiRequest("PUT", endpoint, data);
export const apiDelete = async(endpoint: string, data?: any) => await apiRequest("DELETE", endpoint, data);
