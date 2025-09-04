import type { LoginDto } from "./dtos";
import { apiPost } from "./apiClient";

export async function loginUser(data: LoginDto): Promise<any> {
    return apiPost("/api/Auth/login", data);
}