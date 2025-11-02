// import { useEffect } from "react";
// import * as signalR from "@microsoft/signalr";
import ChatBubble from "./ChatBubble";
import ChatInput from "./ChatInput";

function ChatContainer() {
    return (
        <div className="w-full h-full flex flex-col">
            <div className="flex-1 overflow-auto gap-5 flex flex-col overflow-auto">
                <ChatBubble 
                    userName="mzelder" 
                    date="12:15"
                    message="Hejka!HejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejka Jak sie macie dzisiaj? W ogole taka sytuacja
                    "
                />
                <ChatBubble 
                    userName="mzelder" 
                    date="12:15"
                    message="Hejka!HejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejka Jak sie macie dzisiaj? W ogole taka sytuacja
                    "
                />
                <ChatBubble 
                    userName="mzelder" 
                    date="12:15"
                    message="Hejka!HejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejka Jak sie macie dzisiaj? W ogole taka sytuacja
                    "
                />
                

                <ChatBubble 
                    userName="Codewithteam1" 
                    date="12:15"
                    message="Hejka!HejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejkaHejka Jak sie macie dzisiaj? W ogole taka sytuacja
                    "
                    isCurrentUser={true}
                />
            </div>
            
            <ChatInput />
        </div>
    );
}

export default ChatContainer;