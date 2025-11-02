import { useEffect, useState } from "react";
import Button from "../shared/Button";
import CheckBox from "../shared/CheckBox";
import UserAvatar from "../shared/UserAvatar";
import UserCard from "./UserCard";
import { useReadme } from "../../hooks/useReadme";
import Markdown from "react-markdown";
import remarkGfm from "remark-gfm"
import type { TaskProgressDto } from "../../apiClient/tasks/dtos";
import type { LobbyStatusDto } from "../../apiClient/matchmaking/dtos";
import ChatContainer from "../chat/ChatContainer";
import { getTaskProgress } from "../../apiClient/tasks/tasks";

interface LobbyComponentProps {
    lobbyData: LobbyStatusDto | null;
}

function LobbyComponent({ lobbyData }: LobbyComponentProps) {
    const [repoUrl, setRepoUrl] = useState<string | null>(null);
    const [tasks, setTasks] = useState<TaskProgressDto | null>(null);
    const [showChat, setShowChat] = useState<boolean>(false);
    const repoReadme = useReadme(repoUrl);
    
    useEffect(() => {
        setRepoUrl(lobbyData?.repositoryUrl ?? null);
        checkTasksProgress(); 

    }, [lobbyData]);
    
    const onClickRepositoryButton = () => {
        const repoUrl = lobbyData!.repositoryUrl;
        window.open(repoUrl, "_blank", "noopener,noreferrer");
    }

    const checkTasksProgress = async() => {
        try {
            const result = await getTaskProgress();
            if (result) setTasks(result);
        } catch (error) {
            console.log("Error while polling:", error);
        }
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
                            <ChatContainer />
                        </div>
                    </div>
                )}
            </div>
            
            <div className="flex flex-col items-center w-full h-full overflow-hidden">
                <div className="bg-white text-center px-5 py-6 w-full outline-4 outline-white items-center justify-center flex flex-shrink-0">
                    <h1 className="text-black text-4xl font-medium w-full">
                        Startup Product Landing Page
                    </h1>    
                </div>
                
                <div className="flex flex-col w-full flex-1 min-h-0 p-6 gap-6 overflow-y-auto">
                    <div className="flex flex-col items-start p-8 px-18 bg-[#374151] border-solid border-1 border-white rounded-md space-y-4 flex-1 min-h-0 overflow-y-auto">
                    <div className="prose prose-invert">
                        <Markdown remarkPlugins={[remarkGfm]}>{repoReadme}</Markdown>
                    </div>
                </div>
                    
                    <div className="grid grid-cols-3 gap-6 flex-shrink-0">
                        <div className="flex flex-col p-8 pt-3 bg-[#374151] border-solid border-1 border-white rounded-md text-white leading-relaxed text-xl">
                            <h1 className="text-center font-medium">Your First Steps</h1>
                            <div className="flex flex-row gap-3 items-center">
                                <CheckBox isChecked={tasks?.joinedVideoCall ?? false}/>
                                <p>Join a video call with your team</p>
                            </div>
                            <div className="flex flex-row gap-3 items-center">
                                <CheckBox isChecked={tasks?.visitedRepo ?? false}/>
                                <p>Visit the Github repository</p>
                            </div> 
                            <div className="flex flex-row gap-3 items-center">
                                <CheckBox isChecked={tasks?.createdIssues ?? false}/>
                                <p>Break down the tasks</p>
                            </div>
                            <div className="flex flex-row gap-3 items-center">
                                <CheckBox isChecked={tasks?.startedCoding ?? false}/>
                                <p>Start coding!</p>
                            </div>
                        </div>
                        
                        <div className="flex flex-col gap-6">
                            <Button className="flex-1 font-medium text-2xl" 
                                    text="Go to your repository"
                                    onToggle={() => onClickRepositoryButton()}
                                    isDisabled={!repoUrl} 
                                    isSelected={true}
                            />
                            <Button 
                                className="flex-1 font-medium text-2xl" 
                                text="Team call" 
                                defaultBorderColor="white" 
                                defaultTextColor="white"
                                icon={
                                    <svg width="38" height="39" viewBox="0 0 38 39" fill="none" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M26.3292 22.4941L27.0512 21.7768L25.3744 20.0921L24.6556 20.8094L26.3292 22.4941ZM29.4721 21.476L32.4979 23.1211L33.6299 21.0342L30.6058 19.3907L29.4721 21.476ZM33.079 26.3891L30.8306 28.6263L32.5042 30.3094L34.7525 28.0737L33.079 26.3891ZM29.4595 29.3451C27.1636 29.5605 21.2261 29.3689 14.7946 22.9754L13.1195 24.6585C20.1368 31.6362 26.8169 31.9782 29.6811 31.7106L29.4595 29.3451ZM14.7946 22.9754C8.66553 16.8796 7.64903 11.7543 7.52237 9.52973L5.15053 9.66431C5.30887 12.4636 6.56762 18.1446 13.1195 24.6585L14.7946 22.9754ZM16.9717 13.1904L17.4261 12.7376L15.7541 11.0545L15.2997 11.5057L16.9717 13.1904ZM17.7871 6.98214L15.7921 4.31581L13.8905 5.74081L15.8855 8.40556L17.7871 6.98214ZM9.0772 3.73473L6.59137 6.20473L8.26653 7.88939L10.7508 5.41939L9.0772 3.73473ZM16.1357 12.3481C15.2965 11.5057 15.2965 11.5057 15.2965 11.5089H15.2934L15.2886 11.5152C15.2134 11.5911 15.146 11.6744 15.0875 11.7638C15.002 11.8905 14.9086 12.0567 14.8294 12.2673C14.6367 12.8109 14.5887 13.3953 14.6901 13.9631C14.9023 15.3326 15.846 17.1424 18.2621 19.5459L19.9373 17.8612C17.6747 15.6129 17.1364 14.2449 17.0366 13.5989C16.9891 13.2917 17.0382 13.1397 17.0525 13.1049C17.062 13.0838 17.062 13.0806 17.0525 13.0954C17.0383 13.1172 17.0224 13.1379 17.0049 13.1571L16.9891 13.173L16.9733 13.1872L16.1357 12.3481ZM18.2621 19.5459C20.6799 21.9494 22.4991 22.8867 23.8703 23.0957C24.5717 23.2034 25.1369 23.1179 25.566 22.958C25.8064 22.8701 26.0309 22.7439 26.231 22.5843L26.3102 22.5131L26.3213 22.5036L26.326 22.4988L26.3276 22.4956C26.3276 22.4956 26.3292 22.4941 25.4916 21.6517C24.6525 20.8094 24.6572 20.8078 24.6572 20.8078L24.6604 20.8046L24.6635 20.8015L24.673 20.7936L24.6889 20.7777L24.749 20.7302C24.7638 20.7207 24.7601 20.7218 24.7379 20.7334C24.6984 20.7476 24.5432 20.7967 24.2313 20.7492C23.5758 20.6479 22.1983 20.1096 19.9373 17.8612L18.2621 19.5459ZM15.7921 4.31423C14.1771 2.16089 11.0041 1.81889 9.0772 3.73473L10.7508 5.41939C11.5931 4.58181 13.0878 4.66889 13.8905 5.74081L15.7921 4.31423ZM7.52395 9.53131C7.49228 8.98348 7.74403 8.41031 8.26653 7.89098L6.58978 6.20631C5.73953 7.05181 5.06978 8.24881 5.15053 9.66431L7.52395 9.53131ZM30.8306 28.6263C30.3968 29.0601 29.9281 29.304 29.461 29.3467L29.6811 31.7106C30.8449 31.6014 31.7965 31.0156 32.5058 30.311L30.8306 28.6263ZM17.4261 12.7376C18.9857 11.1875 19.1013 8.73806 17.7887 6.98373L15.8871 8.40714C16.5252 9.26056 16.4302 10.38 15.7525 11.0561L17.4261 12.7376ZM32.4994 23.1226C33.793 23.8256 33.9941 25.4818 33.0805 26.3906L34.7557 28.0737C36.8774 25.9631 36.2234 22.4434 33.6315 21.0358L32.4994 23.1226ZM27.0512 21.7784C27.6592 21.1736 28.6377 21.0247 29.4737 21.4776L30.6074 19.3923C28.891 18.4581 26.763 18.7162 25.376 20.0937L27.0512 21.7784Z" fill="white"/>
                                    </svg>
                                }
                            />
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