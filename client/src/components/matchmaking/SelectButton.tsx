import type React from "react";
import { useState } from "react";

interface SelectButtonProps {
    defaultBgColor?: string;
    defaultTextColor?: string;
    defaultBorderColor?: string;
    selectedColor?: string;
    text: string; 
}

const SelectButton: React.FC<SelectButtonProps> = ({
    defaultBgColor = "#374151",
    defaultTextColor = "#9CA3AF",
    defaultBorderColor = "black",
    selectedColor = "#00D1FF",
    text,
}) => {
    const [isSelected, setIsSelected] = useState(true);

    return (
        <button
            className="rounded border-solid border-1 w-full h-10"
            style={{
                backgroundColor: defaultBgColor,
                color: isSelected ? defaultTextColor : selectedColor,
                borderColor: isSelected ? defaultBorderColor : selectedColor
            }}
            onClick={() => setIsSelected(!isSelected)}
        >
            {text}
        </button>
    )
}

export default SelectButton;