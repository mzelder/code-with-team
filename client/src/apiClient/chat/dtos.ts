export interface ChatMessageDto {
    username: string;
    message: string;
    createdAt: string;
}

export enum MeetingProposalStatus {
    Pending,
    Accepted,
    Rejected
}

export interface MeetingProposalDto {
    id: number;
    username: string;
    meetingTime: string;
    createdAt: string;
    status: MeetingProposalStatus;
    votes: MeetingVoteDto[];
}

export interface CreateMeetingProposalDto {
    username: string;
    meetingTime: string;
}

export interface MeetingVoteDto {
    username: string;
    isAccepted: boolean | null;
}