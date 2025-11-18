import { useEffect, useState } from "react";
import Button from "../shared/Button";
import CheckBox from "../shared/CheckBox";
import UserAvatar from "../shared/UserAvatar";
import UserCard from "./UserCard";
import { useReadme } from "../../hooks/useReadme";
import Markdown from "react-markdown";
import remarkGfm from "remark-gfm";
import type { TaskProgressDto } from "../../apiClient/tasks/dtos";
import type { LobbyStatusDto } from "../../apiClient/matchmaking/dtos";
import ChatContainer from "../chat/ChatContainer";
import { getTaskProgress, updateAttendInMeetingTask } from "../../apiClient/tasks/tasks";
import useCurrentUser from "../../hooks/useCurrentUser";
import { getMeetingLink, getScheduledMeetingDate } from "../../apiClient/meeting/meeting";
import TeamCallButton from "../meeting/TeamCallButton";
import { isMeetingLinkAccessible } from "../../apiClient/matchmaking/validators";

interface LobbyComponentProps {
    lobbyData: LobbyStatusDto | null;
}

function LobbyComponent({ lobbyData }: LobbyComponentProps) {
    const [repoUrl, setRepoUrl] = useState<string | null>(null);
    const [tasks, setTasks] = useState<TaskProgressDto[] | null>(null);
    const [showChat, setShowChat] = useState<boolean>(false);
    const [meetingStartAt, setMeetingStartAt] = useState<string | null>(null);
    const [meetingLink, setMeetingLink] = useState<string | null>(null);
    const repoReadme = useReadme(repoUrl, "README.md");
    const whatToDoReadme = useReadme(repoUrl, "WHATTODO.md");
    const currentUser = useCurrentUser();

    useEffect(() => {
        fetchAll();
        setRepoUrl(lobbyData?.repositoryUrl ?? null);
    }, [lobbyData]);

    useEffect(() => {
        if (!meetingStartAt) return;

        const checkAndFetchLink = async() => {
            const isAccessible = isMeetingLinkAccessible(meetingStartAt);
            
            if (!isAccessible && meetingLink) {
                setMeetingStartAt(null);
                setMeetingLink(null);
                return;
            } 

            if (isAccessible && !meetingLink) {
                await fetchMeetingLink();
                return;
            }
        }

        checkAndFetchLink();
        const interval = setInterval(checkAndFetchLink, 60_000);
        return () => clearInterval(interval);
    }, [meetingStartAt, meetingLink])

    const fetchAll = async() => { 
        const fetchedTasks = await getTaskProgress();
        setTasks(fetchedTasks);

        const meeting = await getScheduledMeetingDate();
        setMeetingStartAt(meeting.scheduledDateTime);
    }

    const fetchMeetingLink = async() => {
        const fetchedLink = await getMeetingLink();
        setMeetingLink(fetchedLink.meetingLink);
    }

    const fetchUpdateTask = async() => {
        const response = await updateAttendInMeetingTask();
        if (response.success) {
            updateTaskCompletion("Attend your scheduled team meeting");
        }
    };

    const onClickRepositoryButton = () => {
        if (repoUrl) {
            window.open(repoUrl, "_blank", "noopener,noreferrer");
        }
    };

    const onClickTeamCallButton = () => {
        if (meetingLink) {
            fetchUpdateTask();
            window.open(meetingLink, "_blank", "noopener,noreferrer");
        }
    };

    const updateTaskCompletion = (taskName: string) => {
        setTasks(prev => 
            prev?.map(task => 
                task.name === taskName
                    ? { ...task, isCompleted: true }
                    : task
            ) ?? null
        );
    };
    
    return (
        <div className="flex flex-row w-full h-screen overflow-hidden">
            <div className="flex flex-col p-4 items-center h-full w-1/4 justify-between border-solid border-r-4 border-[#374151] overflow-hidden">
                {!showChat ? (
                    // When chat is closed
                    <> 
                        <div className="flex flex-col overflow-y-auto w-full">
                            {lobbyData?.members.map((user, index) => (
                                <UserCard 
                                    key={index}
                                    userName={user.name}
                                    userRole={user.role}
                                />
                            ))}
                        </div>
                        <Button 
                            className="flex-shrink-0 w-full"
                            defaultBorderColor="white"
                            defaultTextColor="white"
                            text="Chat"
                            onToggle={() => setShowChat(!showChat)}
                        />
                    </>
                ) : (
                    // When chat is open
                    <div className="flex flex-col gap-4 w-full h-full overflow-hidden">
                        <Button 
                            className="w-full flex-shrink-0"
                            defaultBorderColor="white"
                            defaultTextColor="white"
                            text="Go back to team"
                            onToggle={() => setShowChat(!showChat)}
                        />
                        <div className="flex flex-row relative justify-center flex-shrink-0">
                            {lobbyData?.members.map((user, index) => (
                                <div 
                                    key={index}
                                    className="relative"
                                    style={{ 
                                        marginLeft: index === 0 ? '0' : '-30px',
                                        zIndex: index 
                                    }}
                                >
                                    <UserAvatar width={70} userName={user.name} />
                                </div>
                            ))}
                        </div>

                        <div className="flex-1 min-h-0 overflow-hidden">
                            <ChatContainer 
                                lobbyId={lobbyData!.lobbyId.toString()} 
                                currentUser={currentUser ?? ""}
                            />
                        </div>
                    </div>
                )}
            </div>
            
            <div className="flex flex-col items-center w-full h-full overflow-hidden">
                <div className="flex flex-col w-full flex-1 min-h-0 p-6 gap-6 overflow-hidden">
                    <div className="flex flex-row w-full gap-6 flex-1 min-h-0">
                        <div className="flex flex-col p-8 px-18 bg-[#374151] border-solid border-1 border-white rounded-md space-y-4 flex-1 min-h-0 overflow-hidden">
                            <div className="prose prose-invert max-w-none h-full overflow-y-auto no-scrollbar">
                                <Markdown remarkPlugins={[remarkGfm]}>{whatToDoReadme}</Markdown>
                            </div>
                        </div>
                        <div className="flex flex-col p-8 px-18 bg-[#374151] border-solid border-1 border-white rounded-md space-y-4 flex-1 min-h-0 overflow-hidden">
                            <div className="prose prose-invert max-w-none h-full overflow-y-auto no-scrollbar">
                                <Markdown remarkPlugins={[remarkGfm]}>{repoReadme}</Markdown>
                            </div>
                        </div>
                    </div>
                    
                    <div className="grid grid-cols-3 gap-6 flex-none">
                        <div className="flex flex-col p-8 pt-3 bg-[#374151] border-solid border-1 border-white rounded-md text-white leading-relaxed text-xl">
                            <h1 className="text-center font-medium">Your First Steps</h1>
                            {tasks?.map((task, index) => (
                                <div key={index} className="flex flex-row gap-3 items-center">
                                    <CheckBox isChecked={task.isCompleted}/>
                                    <p>{task.name}</p>
                                </div>
                            ))}
                        </div>
                        
                        <div className="flex flex-col gap-6">
                            <Button className="flex-1 font-medium text-2xl" 
                                    text="Go to your repository"
                                    onToggle={() => onClickRepositoryButton()}
                                    isDisabled={!repoUrl} 
                                    isSelected={true}
                            />
                            <TeamCallButton 
                                meetingTime={meetingStartAt} 
                                onClick={() => onClickTeamCallButton()}
                                isDisabled={!meetingLink}
                                meetingLink={meetingLink}/>
                        </div>

                        <div className="flex flex-col gap-3">
                            <div className="flex flex-row gap-2 justify-center mt-6">
                                {lobbyData?.members.map((user, index) => (
                                    <UserAvatar 
                                        key={index}
                                        width={60}
                                        userName={user.name}    
                                        finished={false}
                                    />
                                ))}
                            </div>
                            <Button className="flex-1 font-medium text-2xl" text="Finish" defaultBorderColor="white" defaultTextColor="white"/>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    ); 
}

export default LobbyComponent;