import UserAvatar from "../shared/UserAvatar";

interface ChatBubbleProps {
    userName: string;
    date: string;
    message: string;
    isCurrentUser?: boolean
}

function ChatBubble({
    userName,
    date,
    message,
    isCurrentUser = false
}: ChatBubbleProps) {
    if (isCurrentUser) {
        return (
            <div className="flex items-start gap-2 justify-end">
                <div className="flex flex-col gap-1 max-w-xs items-end">
                    <div className="flex items-center gap-2">
                        <time className="text-xs opacity-50 text-white">{date}</time>
                        <span className="text-sm font-semibold text-white">{userName}</span>
                    </div>
                    <div className="bg-[#00D1FF] text-black rounded-lg px-4 py-2 break-all w-fit">
                        {message}
                    </div>
                </div>
                <div className="w-10 h-10 rounded-full overflow-hidden flex-shrink-0">
                    <UserAvatar width={50} userName={userName} /> 
                </div>
            </div>
        );
    }
    
    return (
        <div className="flex items-start gap-2">
            <div className="w-10 h-10 rounded-full overflow-hidden flex-shrink-0">
                <UserAvatar width={50} userName={userName}/>
            </div>
            <div className="flex flex-col gap-1 max-w-xs">
                <div className="flex items-center gap-2">
                    <span className="text-sm font-semibold text-white">{userName}</span>
                    <time className="text-xs opacity-50 text-white">{date}</time>
                </div>
                <div className="bg-gray-700 text-white rounded-lg px-4 py-2 break-all w-fit">
                    {message}
                </div>
            </div>
        </div>
    );
}

export default ChatBubble;