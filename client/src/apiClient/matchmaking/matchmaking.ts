import { apiGet, apiPost } from "../apiClient";
import type { MatchmakingRequestDto, MatchmakingResponseDto } from "./dtos";

export async function getMatchmakingOptions(): Promise<MatchmakingResponseDto> {
    return apiGet("/api/Matchmaking/get-options");
}

export async function startQueue(data: MatchmakingRequestDto): Promise<void> {
    return apiPost("/api/Matchmaking/start-queue", data);
}