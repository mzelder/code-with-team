import type React from "react";

interface SelectButtonProps {
    defaultBgColor?: string;
    defaultTextColor?: string;
    defaultBorderColor?: string;
    selectedColor?: string;
    text: string; 
    isSelected?: boolean,
    isDisabled?: boolean,
    onToggle?: () => void;
    className?: string;
    icon?: React.ReactNode;
}

function Button({
    defaultBgColor = "#374151",
    defaultTextColor = "#9CA3AF",
    defaultBorderColor = "black",
    selectedColor = "#00D1FF",
    text,
    isSelected = false,
    isDisabled = false,
    onToggle,
    className = "",
    icon
}: SelectButtonProps) {
    return (
        <button
            className={`rounded border-solid border-1 h-10 flex items-center justify-center gap-3 ${className || 'w-full'}`}
            style={{
                cursor: isDisabled ? "auto" : "pointer",
                backgroundColor: defaultBgColor,
                color: isSelected ? selectedColor : defaultTextColor,
                borderColor: isSelected ? selectedColor : defaultBorderColor
            }}
            onClick={onToggle}
            disabled={isDisabled}
        >
            {icon && <span className="flex items-center justify-center">{icon}</span>}
            {text}
        </button>
    )
}

export default Button;