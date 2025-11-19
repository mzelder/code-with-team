import { formatMessageDate } from "../../apiClient/chat/formatDates";
import Button from "../shared/Button"

interface TeamCallButtonProps {
    onClick: () => void;
    meetingTime: string | null;
    meetingLink: string | null;
    isDisabled: boolean;
}

function TeamCallButton({
    onClick,
    meetingTime,
    meetingLink,
    isDisabled
}: TeamCallButtonProps) {
    const getText = (): string => {
        if (meetingLink) return "Join call";
        if (meetingTime) return `Team call - ${formatMessageDate(meetingTime)}`
        return "Team call - not scheduled";
    }
    
    return (
        <Button 
            className="flex-1 font-medium text-2xl" 
            text={getText()}
            onToggle={onClick}
            isDisabled={isDisabled}
            defaultBorderColor="white" 
            defaultTextColor="white"
        />
    );
}

export default TeamCallButton;