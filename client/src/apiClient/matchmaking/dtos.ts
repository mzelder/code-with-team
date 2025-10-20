export interface CategoryResponseDto {
    id: number;
    name: string;
}

export interface RoleResponseDto {
    id: number;
    name: string;
    categoryId: number;
}

export interface ProgrammingLanguagesResponseDto { 
    id: number;
    name: string;
    RoleId: number;
}

export interface MatchmakingResponseDto {
    categories: CategoryResponseDto[];
    roles: RoleResponseDto[];
    programmingLanguages: ProgrammingLanguagesResponseDto[];
}

export interface ChoosedOptionsDto {
    categoryId: number;
    roleId: number;
    programmingLanguageIds: number[];
}

export interface QueueTimeResponseDto {
    success: boolean;
    queueTime: string;
}

export interface LobbyMemberDto {
    name: string;
    category: string;
    role: string;
}

export interface LobbyStatusDto {
    found: boolean;
    lobbyId: number;
    members: LobbyMemberDto[];
}