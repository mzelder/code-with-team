import * as signalR from "@microsoft/signalr";
import type { ChatMessageDto } from "../apiClient/chat/dtos";

class ChatService {
    private chatUrl : string = `${import.meta.env.VITE_API_BASE_URL}/hubs/chat`;
    private connection: signalR.HubConnection | null = null;
    private messageHandlers: ((message: ChatMessageDto) => void)[] = [];

    async connect() {
        if (this.connection?.state === signalR.HubConnectionState.Connected) {
            return;
        }

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.chatUrl, {
                withCredentials: true
            })
            .withAutomaticReconnect()
            .build();
        
        this.connection.on("ReceiveMessage", (username: string, message: string, date: string) => {
            const chatMessage: ChatMessageDto = { username, message, date };
            this.messageHandlers.forEach(handler => handler(chatMessage));
        });

        try {
            await this.connection.start();
        } catch (e) {
            console.log("Error: " + e);
        }
    }

    async joinLobby(lobbyId: string) {
        if (this.connection?.state === signalR.HubConnectionState.Connected) {
            await this.connection.invoke("JoinLobby", lobbyId);
        }
    }

    async leaveLobby(lobbyId: string) {
        if (this.connection?.state === signalR.HubConnectionState.Connected) {
            await this.connection.invoke("LeaveLobby", lobbyId);
        }
    }

    async sendMessage(lobbyId: string, userName: string, message: string) {
        if (this.connection?.state === signalR.HubConnectionState.Connected) {
            await this.connection.invoke("SendMessage", lobbyId, userName, message);
        }
    }

    onMessage(handler: (message: ChatMessageDto) => void) {
        this.messageHandlers.push(handler);
    }

    removeMessageHandler(handler: (message: ChatMessageDto) => void) {
        this.messageHandlers = this.messageHandlers.filter(h => h !== handler);
    }
    
    async disconnect() {
        if (this.connection) {
            await this.connection.stop();
            this.messageHandlers = [];
        }
    }
}

export const chatService = new ChatService();