import { apiGet } from "..";
import type { ChatMessageDto, MeetingProposalDto } from "./dtos";

export async function getLobbyMessages(): Promise<ChatMessageDto[]> {
    return apiGet("/api/chat/get-messages");
}

export async function getMeetingProposals(): Promise<MeetingProposalDto[]> {
    return apiGet("/api/chat/get-meeting-proposals");
}