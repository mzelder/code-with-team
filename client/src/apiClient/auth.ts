import type { LoginDto, RegisterDto, LoginResponseDto, RegisterResponseDto } from "./dtos";
import { apiPost } from "./apiClient";

export async function loginUser(data: LoginDto): Promise<LoginResponseDto> {
    return apiPost("/api/Auth/login", data);
}

export async function registerUser(data: RegisterDto): Promise<RegisterResponseDto> {
    return apiPost("/api/Auth/register", data);
}