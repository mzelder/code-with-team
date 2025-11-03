import { useEffect, useState, useRef } from "react";
import ChatBubble from "./ChatBubble";
import ChatInput from "./ChatInput";
import { chatService } from "../../services/chatService";
import type { ChatMessageDto } from "../../apiClient/chat/dtos";
import toast from "react-hot-toast";
import { getLobbyMessages } from "../../apiClient/chat/chat";

interface ChatContainerProps {
    lobbyId: string;
    currentUser: string;
}

function ChatContainer({ lobbyId, currentUser }: ChatContainerProps) {
    const [messages, setMessages] = useState<ChatMessageDto[]>([]);
    const messagesEndRef = useRef<HTMLDivElement>(null);
    
    useEffect(() => {
        const initChat = async() => {
            try {
                const chatHistory = await getLobbyMessages();
                setMessages(chatHistory);
            } catch (error) {
                toast.error("Failed to load history chat :(");
            }
            
            await chatService.connect();
            await chatService.joinLobby(lobbyId);
        };

        const handleMessage = (message: ChatMessageDto) => {
            setMessages(prev => [...prev, message]);
        };

        initChat();
        chatService.onMessage(handleMessage);

        return () => {
            chatService.removeMessageHandler(handleMessage);
            chatService.leaveLobby(lobbyId);
        };
    }, [lobbyId]);

    useEffect(() => {
        messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [messages]);

    const handleSendMessage = async (message: string) => {
        await chatService.sendMessage(lobbyId, currentUser, message);
    };

    return (
        <div className="w-full h-full flex flex-col">
            <div className="flex-1 overflow-auto gap-5 flex flex-col p-4">
                {messages.map((msg, index) => (
                    <ChatBubble
                        key={index}
                        userName={msg.username}
                        date={msg.date}
                        message={msg.message}
                        isCurrentUser={msg.username === currentUser}
                    />
                ))}
                <div ref={messagesEndRef} />
            </div>
            <ChatInput onSend={handleSendMessage} />
        </div>
    );
}

export default ChatContainer;