import { apiGet } from "../apiClient";
import type { MetaDataDto } from "./dtos";

export async function getMetadata(): Promise<MetaDataDto> {
    return apiGet("/api/Matchmaking/metadata");
}