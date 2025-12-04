import Button from "../shared/Button";

interface FinishButtonProps {
    onClick: () => void;
    isFinished: boolean;
    didWholeTeamFinished: boolean;
    isDisabled: boolean;
    mentorFeedback: string | null;
}

function FinishButton({
    onClick,
    isFinished,
    didWholeTeamFinished,
    isDisabled,
    mentorFeedback
}: FinishButtonProps) {
    const getText = (): string => {
        if (!isFinished) return "Finish";
        if(!didWholeTeamFinished) return "Waiting for your team";
        if (!mentorFeedback) return "Mentor is reviewing your work";
        return "View mentor feedback";
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
    )
}

export default FinishButton;