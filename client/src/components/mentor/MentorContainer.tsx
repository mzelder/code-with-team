import { useEffect, useState } from "react";
import Button from "../shared/Button";
import UserAvatar from "../shared/UserAvatar";
import type { LobbyStatusDto } from "../../apiClient/matchmaking/dtos";
import { getMentorTeams, submitMentorReview } from "../../apiClient/mentor/mentor";
import { toast } from "react-hot-toast";
import ReviewPanel from "./ReviewPanel";

export enum LobbyStatus {
    SchedulingMeeting,
    Working,
    Finished
}

const statusStyles: Record<LobbyStatus, { text: string; dot: string; label: string }> = {
    [LobbyStatus.SchedulingMeeting]: { text: "text-[#00D1FF]", dot: "bg-[#00D1FF]", label: "Scheduling first meeting" },
    [LobbyStatus.Working]: { text: "text-yellow-400", dot: "bg-yellow-400", label: "Working" },
    [LobbyStatus.Finished]: { text: "text-green-400", dot: "bg-green-400", label: "Finished" }
};

function getStatusStyle(status: LobbyStatus): { text: string; dot: string; label: string } {
    return statusStyles[status];
}

function parseLobbyStatus(raw?: string | LobbyStatus): LobbyStatus {
    if (typeof raw === "number") {
        return raw;
    }
    switch (raw) {
        case "Scheduling first meeting":
            return LobbyStatus.SchedulingMeeting;
        case "Working":
            return LobbyStatus.Working;
        case "Finished":
            return LobbyStatus.Finished;
        default:
            return LobbyStatus.SchedulingMeeting; // Fallback / unknown
    }
}

function MentorContainer() {
    const [mentorTeams, setMentorTeams] = useState<LobbyStatusDto[]>([]);
    const [showReviewPanel, setShowReviewPanel] = useState<boolean>(false);
    const [selectedTeamId, setSelectedTeamId] = useState<number | null>(null);

    useEffect(() => {
        fetchMentorTeams();
    }, []);

    useEffect(() => {
        const interval = setInterval(() => {
            fetchMentorTeams();
        }, 30_000);

        return () => clearInterval(interval);
    }, []);

    const fetchMentorTeams = async () => {
        try {
            const result = await getMentorTeams();
            setMentorTeams(result);
        } catch (error) {
            toast.error("Can't get mentor teams");
        }
    };

    const sendFeedback = async(feedback: string) => {
        if (selectedTeamId == null) {
            toast.error("No team selected for review");
            return;
        }

        const text = feedback.trim();
        if (!text) {
            toast.error("Feedback cannot be empty");
            return;
        }

        try {
            await submitMentorReview({ lobbyId: selectedTeamId, feedback: text });
            toast.success("Feedback sent");
            setShowReviewPanel(false);
        } catch (err) {
            console.error(err);
            toast.error("Failed to send feedback");
        }
    }

    return (
        <>
        <div className="p-6 text-white">
            <div className="overflow-x-auto border border-gray-700 rounded">
                <table className="min-w-full text-sm">
                    <thead className="bg-gray-800">
                        <tr>
                            <th className="p-3 text-left">Team</th>
                            <th className="p-3 text-left">Status</th>
                            <th className="p-3 text-left">Repo</th>
                            <th className="p-3 text-left">Review</th>
                        </tr>
                    </thead>
                    <tbody>
                        {mentorTeams.map(team => {
                            const lobbyStatus = parseLobbyStatus(team.status);
                            const styles = getStatusStyle(lobbyStatus);
                            const reviewDisabled = lobbyStatus !== LobbyStatus.Finished;
                            return (
                                <tr key={team.lobbyId} className="odd:bg-gray-900 even:bg-gray-850">
                                    <td className="p-3">
                                        <div className="flex -space-x-2">
                                            {team.members.slice(0, 4).map(m => (
                                                <div key={m.name} title={m.name}>
                                                    <UserAvatar userName={m.name} width={40} className="w-10 h-10" />
                                                </div>
                                            ))}
                                        </div>
                                    </td>
                                    <td className={`p-3 font-medium ${styles.text}`}>
                                        <span className={`inline-block w-2 h-2 rounded-full mr-2 ${styles.dot}`} />
                                        {styles.label}
                                    </td>
                                    <td className="p-3">
                                        {team.repositoryUrl ? (
                                            <Button
                                                text="Open Repo"
                                                onToggle={() => window.open(team.repositoryUrl, "_blank", "noopener")}
                                                defaultBorderColor="white"
                                                defaultTextColor="white"
                                            />
                                        ) : (
                                            <span className="text-gray-500">No repo</span>
                                        )}
                                    </td>
                                    <td className="p-3">
                                        <Button
                                            text="Review"
                                            isDisabled={reviewDisabled}
                                            isSelected={!reviewDisabled}
                                            onToggle={() => {
                                                setSelectedTeamId(team.lobbyId);
                                                setShowReviewPanel(true);
                                            }}
                                        />
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
            {mentorTeams.length === 0 && (
                <div className="mt-4 text-sm text-gray-400">No teams assigned yet.</div>
            )}
        </div>

        {showReviewPanel && (
            <ReviewPanel
                onClose={() => setShowReviewPanel(false)}
                onSendFeedback={(feedback) => sendFeedback(feedback)}
                aiReviewMarkdown={mentorTeams.find(t => t.lobbyId === selectedTeamId)?.aiSummary}
                title={selectedTeamId ? `Review for Lobby ${selectedTeamId}` : "Review Panel"}
            />
        )}
        </>
    );
}

export default MentorContainer;