import type { LoginRequestDto, RegisterRequestDto, ValidateCookiesResponseDto } from "./dtos";
import type { ApiResponseDto } from "../dtos";
import { apiPost, apiGet } from "..";

export async function loginUser(data: LoginRequestDto): Promise<ApiResponseDto> {
    return apiPost("/api/Auth/login", data);
}

export async function registerUser(data: RegisterRequestDto): Promise<ApiResponseDto> {
    return apiPost("/api/Auth/register", data);
}

export async function logoutUser(): Promise<ApiResponseDto> {
    return apiPost("/api/Auth/logout");
}

export async function validateCookies(): Promise<ValidateCookiesResponseDto> {
    return apiGet("/api/Auth/check");
}

export async function handleGithubCallback(code: string | null): Promise<ApiResponseDto> {
    return apiPost("/api/Auth/github/callback", { code });
}