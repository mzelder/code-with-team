import * as signalR from "@microsoft/signalr";
import type { ChatMessageDto, CreateMeetingProposalDto, MeetingProposalDto, MeetingVoteDto } from "../apiClient/chat/dtos";

class ChatService {
    private chatUrl : string = `${import.meta.env.VITE_API_BASE_URL}/hubs/chat`;
    private connection: signalR.HubConnection | null = null;
    
    private messageHandlers: ((message: ChatMessageDto) => void)[] = [];
    private proposalHandlers: ((proposal: MeetingProposalDto) => void)[] = [];

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
        
        this.connection.on("Error", (error: string) => {
            console.error(error);
        });
        
        this.connection.on("ReceiveMessage", (message: ChatMessageDto) => {
            this.messageHandlers.forEach(handler => handler(message));
        });

        this.connection.on("ReceiveMeetingProposal", (proposal: MeetingProposalDto) => {
            this.proposalHandlers.forEach(handler => handler(proposal));
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

    async sendMessage(message: ChatMessageDto, lobbyId: string) {
        if (this.connection?.state === signalR.HubConnectionState.Connected) {
            await this.connection.invoke("SendMessage", message, lobbyId);
        }
    }

    async sendMeetingProposal(proposal: CreateMeetingProposalDto, lobbyId: string) {
        if (this.connection?.state === signalR.HubConnectionState.Connected) {
            await this.connection.invoke("SendMeetingProposal", proposal, lobbyId);
        }
    }

    async sendVote(vote: MeetingVoteDto, lobbyId: string, proposalId: number) {
        if (this.connection?.state === signalR.HubConnectionState.Connected) {
            await this.connection.invoke("SendVote", vote, lobbyId, proposalId);
        }
    }

    onMessage(handler: (message: ChatMessageDto) => void) {
        this.messageHandlers.push(handler);
    }

    removeMessageHandler(handler: (message: ChatMessageDto) => void) {
        this.messageHandlers = this.messageHandlers.filter(h => h !== handler);
    }

    onMeetingProposal(handler: (proposal: MeetingProposalDto) => void) {
        this.proposalHandlers.push(handler);
    }

    removeMeetingProposalHandler(handler: (proposal: MeetingProposalDto) => void) {
        this.proposalHandlers = this.proposalHandlers.filter(h => h !== handler);
    }
      
    async disconnect() {
        if (this.connection) {
            await this.connection.stop();
            this.messageHandlers = [];
            this.proposalHandlers = [];
        }
    }
}

export const chatService = new ChatService();