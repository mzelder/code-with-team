import type { Command } from "../../apiClient/chat/types";

interface CommandPanelProps {
    selectCommand: (name: string) => void;
    commands: Command[];
    selectedIndex: number;
}

function CommandPanel({
    selectCommand: onClick,
    commands,
    selectedIndex
} : CommandPanelProps) {
    return (
        <div className="absolute bottom-full left-0 right-0 bg-gray-800 border border-gray-700 rounded-t-lg shadow-lg mb-1">
            <div className="text-xs text-gray-400 px-3 py-2 border-b border-gray-700">
                Available Commands:
            </div>
            <div className="max-h-48 overflow-y-auto">
                {commands.map((cmd, index) => (
                    <div
                        key={cmd.name}
                        className={`px-3 py-2 hover:bg-gray-700 cursor-pointer transition-colors ${
                            index === selectedIndex && "bg-gray-700"
                        }`}
                        onClick={() => onClick(cmd.name)}
                    >
                        <span className="text-[#00D1FF] font-mono">{cmd.name}</span>
                        <div className="text-gray-400 text-sm ml-2">
                            {cmd.description.map((line, i) => (
                                <div key={i}>- {line}</div>
                            ))}
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default CommandPanel;