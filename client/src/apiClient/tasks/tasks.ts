import { apiGet } from "..";
import type { TaskProgressDto } from "./dtos";

export async function getTaskProgress(): Promise<TaskProgressDto[]> {
    return apiGet("/api/TaskProgress/get-tasks");
}