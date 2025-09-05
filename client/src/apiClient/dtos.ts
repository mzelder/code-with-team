export interface LoginDto {
    username: string;
    password: string;
}

export interface RegisterDto {
    username: string;
    password: string;
    confirmPassword: string;
}

export interface LoginResponseDto {
    message: string;
}

export interface RegisterResponseDto {
    message: string;
}