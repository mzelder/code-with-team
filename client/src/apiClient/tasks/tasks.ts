import { apiGet, apiPost } from "..";
import type { ApiResponseDto } from "../dtos";
import type { TaskProgressDto } from "./dtos";

export async function getTaskProgress(): Promise<TaskProgressDto[]> {
    return apiGet("/api/TaskProgress/get-tasks");
}

export async function updateAttendInMeetingTask(): Promise<ApiResponseDto> {
    return apiPost("/api/TaskProgress/attend-meeting");
}