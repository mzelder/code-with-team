import { apiGet } from "../apiClient";
import type { MetadataDto } from "./dtos";

export async function getMetadata(): Promise<MetadataDto> {
    return apiGet("/api/Matchmaking/metadata");
}