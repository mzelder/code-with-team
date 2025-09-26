export interface LoginRequestDto {
    username: string;
    password: string;
}

export interface RegisterRequestDto {
    username: string;
    password: string;
    confirmPassword: string;
}

export interface ValidateCookiesResponseDto {
    isAuthenticated: boolean;
    user: string;
}