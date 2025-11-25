import { apiGet, apiPost } from "..";
import type { ApiResponseDto } from "../dtos";
import type { LobbyStatusDto } from "../matchmaking/dtos";
import type { MentorDto, MentorFormDto, MentorQuestionDto } from "./dtos";

export async function getMentorStatus(): Promise<MentorDto> {
    return apiGet("/api/Mentor/get-mentor");
}

export async function getQuestions(): Promise<MentorQuestionDto> {
    return apiGet("/api/Mentor/get-questions");
}

export async function submitMentorApplication(formData: MentorFormDto): Promise<ApiResponseDto> {
    return apiPost("/api/Mentor/submit-form", formData);
} 

export async function getMentorTeams(): Promise<LobbyStatusDto[]> {
    return apiGet("/api/Mentor/get-mentor-teams");
}