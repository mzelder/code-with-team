import type { LoginDto, RegisterDto } from "./dtos";
import { apiPost } from "./apiClient";

export async function loginUser(data: LoginDto): Promise<any> {
    return apiPost("/api/Auth/login", data);
}

export async function registerUser(data: RegisterDto): Promise<any> {
    return apiPost("/api/Auth/register", data);
}