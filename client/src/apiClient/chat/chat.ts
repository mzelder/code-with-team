import { apiGet } from "..";
import type { ChatMessageDto } from "./dtos";

export async function getLobbyMessages(): Promise<ChatMessageDto[]> {
    return apiGet("/api/chat/get-messages");
}