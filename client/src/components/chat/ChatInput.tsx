import { useState } from "react";

function ChatInput() {
    const [message, setMessage] = useState("");

    const handleSend = () => {
        if (message.trim()) {
            // Handle sending message here
            console.log("Sending:", message);
            setMessage("");
        }
    };

    const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();
            handleSend();
        }
    };

    return (
        <div className="w-full p-4 border-t border-gray-700">
            <div className="flex items-center gap-2 bg-gray-800 rounded-lg p-2">
                <input
                    type="text"
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    onKeyDown={handleKeyPress}
                    placeholder="Type a message..."
                    className="flex-1 bg-transparent text-white placeholder-gray-400 outline-none px-2 py-2"
                />
                <button
                    onClick={handleSend}
                    disabled={!message.trim()}
                    className="bg-[#00D1FF] hover:bg-[#00B8E6] disabled:bg-gray-600 disabled:cursor-not-allowed text-white font-semibold rounded-lg px-4 py-2 transition-colors duration-200"
                >
                    <svg 
                        xmlns="http://www.w3.org/2000/svg" 
                        viewBox="0 0 24 24" 
                        fill="currentColor" 
                        className="w-5 h-5"
                    >
                        <path d="M3.478 2.405a.75.75 0 00-.926.94l2.432 7.905H13.5a.75.75 0 010 1.5H4.984l-2.432 7.905a.75.75 0 00.926.94 60.519 60.519 0 0018.445-8.986.75.75 0 000-1.218A60.517 60.517 0 003.478 2.405z" />
                    </svg>
                </button>
            </div>
        </div>
    );
}

export default ChatInput;