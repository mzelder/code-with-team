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
    success: boolean;
}

export interface RegisterResponseDto {
    message: string;
    success: boolean;
}