import { AVATAR_URL } from "../../constants";

interface UserAvatarProps {
    width?: number;
    className?: string;
    finished?: boolean | null;
    userName: string;
}

function UserAvatar({ 
    width = 103, 
    className = "h-auto", 
    finished = null,
    userName
}: UserAvatarProps) {
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
            <defs>
                <clipPath id={`avatar-clip-${width}`}>
                    <circle cx="51.5" cy="51.5" r="51.5" />
                </clipPath>
            </defs>
            <image
                x="0"
                y="0"
                width="103"
                height="103"
                href={AVATAR_URL + userName}
                clipPath={`url(#avatar-clip-${width})`}
                preserveAspectRatio="xMidYMid slice"
            />
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