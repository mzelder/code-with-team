export interface ChatMessageDto {
    username: string;
    message: string;
    createdAt: string;
}

export interface MeetingProposalDto {
    username: string;
    meetingTime: string;
    createdAt: string;
    votes: MeetingVoteDto[];
}

export interface MeetingVoteDto {
    username: string;
    isAccepted: boolean | null;
}