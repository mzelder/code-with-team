import { apiGet, apiPost, apiDelete } from "..";
import type { ApiResponseDto } from "../dtos";
import type { ChoosedOptionsDto, MatchmakingResponseDto, QueueTimeResponseDto, LobbyStatusDto } from "./dtos";

export async function getMatchmakingOptions(): Promise<MatchmakingResponseDto> {
    return apiGet("/api/Matchmaking/get-options");
}

export async function getMatchmakingChoosedOptions(): Promise<ChoosedOptionsDto> {
    return apiGet("/api/Matchmaking/get-choosed-options");
}

export async function startQueue(data: ChoosedOptionsDto): Promise<void> {
    return apiPost("/api/Matchmaking/start-queue", data);
}

export async function stopQueue(): Promise<ApiResponseDto> {
    return apiDelete("/api/Matchmaking/stop-queue");
}

export async function getCurrentTimeInQueue(): Promise<QueueTimeResponseDto> {
    return apiGet("/api/Matchmaking/queue-time");
}

export async function getLobbyStatus(): Promise<LobbyStatusDto> {
    return apiGet("/api/Matchmaking/wait-for-lobby");
}