interface UserAvatarProps {
    width?: number;
    className?: string;
    finished?: boolean | null;
}

function UserAvatar({ width = 103, className = "h-auto", finished = null}: UserAvatarProps) {
    const getStrokeColor = () => {
        if (finished === true) return "#42CC67"; 
        if (finished === false) return "#C75151";
        return "transparent";
    };

    const strokeColor = getStrokeColor();
    
    return(
        <svg
            width={width}
            viewBox={`0, 0, 103, 103`}
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            className={className}
        >
            <circle cx="51.5" cy="51.5" r="51.5" fill="#374151" />
            <path d="M51.5 0C79.9427 0 103 23.0573 103 51.5C103 67.389 95.8024 81.595 84.4922 91.042V84.4922C84.4922 66.2711 69.7211 51.5 51.5 51.5C33.2789 51.5 18.5078 66.2711 18.5078 84.4922V91.042C7.19757 81.595 0 67.389 0 51.5C0 23.0573 23.0573 0 51.5 0Z" fill="white"/>
            <circle cx="51.5" cy="29.7734" r="15.2891" fill="#374151"/>
            <circle 
                cx="51.5" 
                cy="51.5" 
                r="49.5" 
                fill="none" 
                stroke={strokeColor}
                strokeWidth="5"
            />
        </svg>
    )
}

export default UserAvatar;