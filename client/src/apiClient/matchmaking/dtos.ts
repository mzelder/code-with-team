export interface CategoryDto {
    id: number;
    name: string;
}

export interface RoleDto {
    id: number,
    name: string,
    categoryId: number
}

export interface ProgrammingLanguagesDto { 
    id: number,
    name: string,
    RoleId: number
}

export interface MetaDataDto {
    categories: CategoryDto[];
    roles: RoleDto[];
    programmingLanguages: ProgrammingLanguagesDto[];
}