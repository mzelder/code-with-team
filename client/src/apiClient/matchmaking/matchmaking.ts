import { apiGet, apiPost } from "../apiClient";
import type { MetadataRequestDto, MetadataResponseDto } from "./dtos";

export async function getMetadata(): Promise<MetadataResponseDto> {
    return apiGet("/api/Matchmaking/metadata");
}

export async function saveMetadata(data: MetadataRequestDto): Promise<void> {
    return apiPost("/api/Matchmaking/sendmetadata", data);
}