import { apiGet, apiPost } from "..";
import type { ApiResponseDto } from "../dtos";
import type { TaskProgressDto } from "./dtos";
import type { TaskNames } from "../../components/lobby/LobbyComponent";

export async function getTaskProgress(): Promise<TaskProgressDto[]> {
    return apiGet("/api/TaskProgress/get-tasks");
}

export async function updateUserTask(userTask: TaskNames): Promise<ApiResponseDto> {
    return apiPost("/api/TaskProgress/update-user-task", userTask);
}

export async function finishWork(): Promise<ApiResponseDto> {
    return apiPost("/api/TaskProgress/finish-work");
}