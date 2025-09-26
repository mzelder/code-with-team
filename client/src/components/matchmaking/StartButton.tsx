import type React from "react";

interface StartButtonProps {
    options: [number | null, number | null, number[] | null] 
    isSearching: boolean,
    timeInQueue: string,
    onToggle: () => void;
}

const StartButton: React.FC<StartButtonProps> = ({
    options,
    isSearching,
    timeInQueue,
    onToggle
}) => {
    const isDisabled = options[0] == null || options[1] == null || !options[2] == null;

    return (
        <button
            style={{
                cursor: "pointer",
            }}
            onClick={onToggle}
            disabled={isDisabled}
        >
            <h1 className="text-3xl text-[#00D1FF] font-[Oswald] font-bold">{isSearching ? "Stop Searching" : "Find Your Team"}</h1>
            {isSearching && <p>{timeInQueue}</p>}
        </button>
    )
}

export default StartButton;





