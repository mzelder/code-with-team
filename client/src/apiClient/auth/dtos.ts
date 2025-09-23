export interface LoginRequestDto {
    username: string;
    password: string;
}

export interface RegisterRequestDto {
    username: string;
    password: string;
    confirmPassword: string;
}

export interface LoginResponseDto {
    message: string;
    success: boolean;
}

export interface RegisterResponseDto {
    message: string;
    success: boolean;
}

export interface LogoutResponseDto {
    message: string;
    success: boolean;
}

export interface ValidateCookiesResponseDto {
    isAuthenticated: boolean;
    user: string;
}