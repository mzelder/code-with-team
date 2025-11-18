import { apiGet } from "..";
import type { MeetingLinkDto, ScheduledMeetingDto } from "./dtos";

export async function getScheduledMeetingDate(): Promise<ScheduledMeetingDto> {
    return apiGet("/api/Meeting/get-scheduled");
}

export async function getMeetingLink(): Promise<MeetingLinkDto> {
    return apiGet("/api/Meeting/get-link");
}