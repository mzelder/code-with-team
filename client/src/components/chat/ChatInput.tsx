import { useEffect, useState } from "react";
import type { Command } from "../../apiClient/chat/types";
import CommandPanel from "./CommandPanel";
import { validateBookDate } from "../../apiClient/chat/validators";
import DateTimePicker from "./DateTimePicker";
import toast from "react-hot-toast";

interface ChatInputProps {
    onMessageSend: (message: string) => void;
    onBookMeeting: (time: string) => void;
}

function ChatInput({ onMessageSend, onBookMeeting }: ChatInputProps) {
    const [message, setMessage] = useState<string>("");
    const [showCommands, setShowCommands] = useState<boolean>(false);
    const [showDatePicker, setshowDatePicker] = useState<boolean>(false);
    const [selectedIndex, setSelectedIndex] = useState<number>(0);

    const commands: Command[] = [
        { 
            name: "/book", 
            description: [
                "Book meeting.",
                "Format: MM:DD:HH:MM (e.g., 12:25:14:30).",
                "Leave blank to open date picker."
            ],
            args: ["MM:DD:HH:MM"],
            validator: validateBookDate,
            handler: (args: string[]) => {
                onBookMeeting(args[0]);
            }
        }
    ];

    const executeCommand = (command: Command, args: string[]): boolean => {
        if (!command.validator) {
            command.handler(args);
            return true;
        }
       
        const validation = command.validator(args);
        if (!validation.valid) {
            toast.error(validation.error || "Invalid command");
            return false;
        }
    
        command.handler(args);
        return true;
    };  

    const findCommand = (message: string): Command | null => {
        const commandName = message.trim().split(" ")[0];
        return commands.find(cmd => cmd.name === commandName) || null;
    };

    const getCommandArgs = (message: string): string[] => {
        const parts = message.trim().split(" ");
        return parts.slice(1);
    };
    
    const resetCommandState = () => {
        setShowCommands(false);
        setSelectedIndex(0);
    };
    
    const handleSubmit = () => {
        if (!message.trim()) return;
        
        const command = findCommand(message);

        if (!command) {
            onMessageSend(message);
            setMessage("");
            resetCommandState();
            return;
        }

        const args = getCommandArgs(message);
        if (args.length === 0) {
            setshowDatePicker(true);
            setMessage(prev => prev.trim());
            return;
        }

        const success = executeCommand(command, args);
        if (success) {
            setMessage("");
            resetCommandState();
        }
    };

    const handleDateSelect = (dateTime: string) => {
        setshowDatePicker(false);
        setMessage(prev => `${prev} ${dateTime}`);
    };

    const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
        switch (e.key) {
            case "Enter":
                e.preventDefault();
                if (showCommands) {
                    selectCommand(commands[selectedIndex].name);
                    resetCommandState();
                } else {
                    handleSubmit();
                }
                break;
                
            case "ArrowUp":
                if (showCommands) {
                    e.preventDefault();
                    setSelectedIndex(prev =>
                        prev > 0 ? prev - 1 : commands.length - 1
                    );
                }
                break;
                
            case "ArrowDown":
                if (showCommands) {
                    e.preventDefault();
                    setSelectedIndex(prev =>
                        prev < commands.length - 1 ? prev + 1 : 0
                    );
                }
                break;
        }
    };

    const selectCommand = (commandName: string) => {
        setMessage(commandName);
        resetCommandState();
    };

    useEffect(() => {
        const startsWithSlash = message.startsWith("/");
        const isCompleteCommand = commands.some(cmd =>
            message.startsWith(cmd.name)
        );
        
        if (!startsWithSlash || isCompleteCommand) {
            resetCommandState();
        } else {
            setShowCommands(true);
        }

    }, [message]);

    return (
        <div className="relative">
            {showCommands && (
                <CommandPanel 
                    selectCommand={selectCommand}
                    commands={commands}
                    selectedIndex={selectedIndex} />
            )}

            {showDatePicker && (
                <DateTimePicker 
                    onDateSelect={handleDateSelect}
                    onClose={() => setshowDatePicker(false)}
                />
            )}
            
            {/* Input Panel */}
            <div className="w-full p-4 border-t border-gray-700">
                <div className="flex items-center gap-2 bg-gray-800 rounded-lg p-2">
                    <input
                        type="text"
                        value={message}
                        onChange={(e) => setMessage(e.target.value)}
                        onKeyDown={handleKeyPress}
                        placeholder="Type a message or / for commands..."
                        className="flex-1 bg-transparent text-white placeholder-gray-400 outline-none px-2 py-2"
                    />
                    <button
                        onClick={handleSubmit}
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
        </div>
    );
}

export default ChatInput;