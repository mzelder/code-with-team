import { useEffect, useState, useRef, useMemo } from "react";
import ChatBubble from "./ChatBubble";
import ChatInput from "./ChatInput";
import MeetingProposalCard from "./MeetingProposalCard";
import { chatService } from "../../services/chatService";
import { type MeetingProposalDto, type ChatMessageDto } from "../../apiClient/chat/dtos";
import toast from "react-hot-toast";
import { getLobbyMessages, getMeetingProposals } from "../../apiClient/chat/chat";

interface ChatContainerProps {
    lobbyId: string;
    currentUser: string;
}

function ChatContainer({ lobbyId, currentUser }: ChatContainerProps) {
    const [messages, setMessages] = useState<ChatMessageDto[]>([]);
    const [meetingProposals, setMeetingProposals] = useState<MeetingProposalDto[]>([]);
    const messagesEndRef = useRef<HTMLDivElement>(null);
    
    const sortedItems = useMemo(() => {
        const allItems: (ChatMessageDto | MeetingProposalDto)[] = [
            ...messages,
            ...meetingProposals,
        ];
        
        return allItems.sort((a, b) => 
            new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
        );
    }, [messages, meetingProposals]);

    const isMessage = (item: ChatMessageDto | MeetingProposalDto): item is ChatMessageDto => {
        return 'message' in item;
    };

    useEffect(() => {
        const initChatAndProposals = async() => {
            try {
                const [chatHistory, meetingProposals] = await Promise.all([
                    getLobbyMessages(),
                    getMeetingProposals()
                ]);
                
                setMessages(chatHistory);
                setMeetingProposals(meetingProposals);
            } catch (error) {
                toast.error("Failed to load chat history");
            }
            
            await chatService.connect();
            await chatService.joinLobby(lobbyId);
        };

        const handleMessage = (message: ChatMessageDto) => {
            setMessages(prev => [...prev, message]);
        };
        const handleMeetingProposal = (proposal: MeetingProposalDto) => {
            setMeetingProposals(prev => [...prev, proposal]);
        }

        initChatAndProposals();
        chatService.onMessage(handleMessage);
        chatService.onMeetingProposal(handleMeetingProposal);

        return () => {
            chatService.removeMessageHandler(handleMessage);
            chatService.removeMeetingProposal(handleMeetingProposal);
            chatService.leaveLobby(lobbyId);
        };
    }, [lobbyId]);

    useEffect(() => {
        messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [sortedItems]);

    const handleSendMessage = async (message: string) => {
        const messageDto: ChatMessageDto = { 
            message: message, 
            username: currentUser,
            createdAt: ""
        };
        await chatService.sendMessage(messageDto, lobbyId);
    };

    const handleMeetingProposal = async (meetingTime: string) => {
        const proposalDto: MeetingProposalDto = {
            username: currentUser,
            meetingTime: meetingTime,
            createdAt: "",
            votes: []
        };
        await chatService.sendMeetingProposal(proposalDto, lobbyId);
    };

    const handleVote = (proposalId: string, accept: boolean) => {
        console.log("Vote clicked:", proposalId, accept);
    };

    return (
        <div className="w-full h-full flex flex-col">
            <div className="flex-1 overflow-auto gap-5 flex flex-col p-4">
                {sortedItems.map((item, index) => (
                    isMessage(item) 
                    ?
                        <ChatBubble
                            key={index}
                            userName={item.username}
                            date={item.createdAt}
                            message={item.message}
                            isCurrentUser={item.username === currentUser}
                        />
                    :
                        <MeetingProposalCard
                            proposal={item}
                            currentUser={currentUser}
                            onVote={handleVote}
                        />
                ))}
                <div ref={messagesEndRef} />
            </div>
            <ChatInput 
                onMessageSend={handleSendMessage}
                onBookMeeting={handleMeetingProposal} />
        </div>
    );
}

export default ChatContainer;