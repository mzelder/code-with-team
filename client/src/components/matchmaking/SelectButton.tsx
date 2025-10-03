import type React from "react";

interface SelectButtonProps {
    defaultBgColor?: string;
    defaultTextColor?: string;
    defaultBorderColor?: string;
    selectedColor?: string;
    text: string; 
    isSelected: boolean,
    isSearching: boolean,
    onToggle: () => void;
}

const SelectButton: React.FC<SelectButtonProps> = ({
    defaultBgColor = "#374151",
    defaultTextColor = "#9CA3AF",
    defaultBorderColor = "black",
    selectedColor = "#00D1FF",
    text,
    isSelected,
    isSearching,
    onToggle,
}) => {
    return (
        <button
            className="rounded border-solid border-1 w-full h-10"
            style={{
                cursor: isSearching ? "auto" : "pointer",
                backgroundColor: defaultBgColor,
                color: isSelected ? selectedColor : defaultTextColor,
                borderColor: isSelected ? selectedColor : defaultBorderColor
            }}
            onClick={onToggle}
            disabled={isSearching}
        >
            {text}
        </button>
    )
}

export default SelectButton;