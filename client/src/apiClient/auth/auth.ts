import type { LoginRequestDto, RegisterRequestDto, LoginResponseDto, RegisterResponseDto, LogoutResponseDto, ValidateCookiesResponseDto } from "./dtos";
import { apiPost, apiGet } from "../apiClient";

export async function loginUser(data: LoginRequestDto): Promise<LoginResponseDto> {
    return apiPost("/api/Auth/login", data);
}

export async function registerUser(data: RegisterRequestDto): Promise<RegisterResponseDto> {
    return apiPost("/api/Auth/register", data);
}

export async function logoutUser(): Promise<LogoutResponseDto> {
    return apiPost("/api/Auth/logout");
}

export async function validateCookies(): Promise<ValidateCookiesResponseDto> {
    return apiGet("/api/Auth/check");
}