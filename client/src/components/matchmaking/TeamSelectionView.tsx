import { useEffect, useState } from "react";
import clsx from "clsx";
import SelectButton from "./SelectButton";
import { type MatchmakingResponseDto } from "../../apiClient/matchmaking/dtos";
import { getCurrentTimeInQueue, getMatchmakingOptions, getMatchmakingChoosedOptions, startQueue, stopQueue } from "../../apiClient/matchmaking/matchmaking";
import toast from "react-hot-toast";
import StartButton from "./StartButton";

interface TeamSelectionViewProps {
    onQueueStateChange: (isQueuing: boolean) => void;
}

function TeamSelectionView({ onQueueStateChange }: TeamSelectionViewProps) {
    const [selectedCategory, setSelectedCategory] = useState<number | null>(null);
    const [selectedRole, setSelectedRole] = useState<number | null>(null);
    const [selectedTool, setSelectedTool] = useState<number[] | null>(null);
    const [options, setOptions] = useState<MatchmakingResponseDto | null>(null);
    const [searching, setSearching] = useState<boolean>(false);
    const [timeInQueue, setTimeInQueue] = useState<[number, number]>([0, 0]);

    useEffect(() => {
        onQueueStateChange(searching);
    }, [searching, onQueueStateChange]);

    const handleToggleCategory = (value: number) => setSelectedCategory(value);
    const handleToggleRole = (value: number) => setSelectedRole(value);
    const handleToggleTool = (value: number) => {
        setSelectedTool((prev) => 
            prev === null ? [value] : prev.filter((v) => v !== value)
        );
    };
    const handleQueueToggle = async (
        selectedCategory: number,
        selectedRole: number,
        selectedLanguages: number[]
    ) => {
        if (searching) {
            await stopQueue();
            setSearching(false);
            setTimeInQueue([0, 0]);
        } else {
            await startQueue({
                categoryId: selectedCategory,
                roleId: selectedRole,
                programmingLanguageIds: selectedLanguages
            });
            fetchTime();
        }
    };

    const fetchTime = async () => {
            try {
                const time = await getCurrentTimeInQueue();
                setSearching(time.success);
                setTimeInQueue(formatTime(time.queueTime));
            } catch (error) {
                console.log("not in queue"); // to delete
            }
        }
        
    const fetchOptions = async () => {
        try {
            const data = await getMatchmakingOptions();
            setOptions(data);
        } catch (error) {
            toast.error("Something went wrong. Please try again later.");
        }
    }

    const fetchChoosedOptions = async () => {
        try {
            const choosedOptions = await getMatchmakingChoosedOptions();
            setSelectedCategory(choosedOptions.categoryId);
            setSelectedRole(choosedOptions.roleId);
            setSelectedTool(choosedOptions.programmingLanguageIds);
        } catch (error) {
            console.log("null's for choosedOptions");
        }
    }
    
    const formatTime = (time: string): [number, number] => {
        const splittedTime = time.split(":");
        const minutes = parseInt(splittedTime[1]);
        const seconds = Math.floor(parseFloat(splittedTime[2]));
        return [minutes, seconds];
    }

    const filteredRoles = options?.roles.filter(role => { 
        const mactchkingRole = selectedCategory === role.categoryId;
        return mactchkingRole;
    });

    const filteredTools = options?.programmingLanguages.filter(programmingLanguage => {
        const matchingTool = selectedRole === programmingLanguage.id;
        return matchingTool;
    })

    useEffect(() => {
        fetchOptions();
        fetchTime();
        fetchChoosedOptions();
    }, []);
        
    useEffect(() => {
        if (!searching) return;
        
        const interval = setInterval(() => {
            setTimeInQueue(([minutes, seconds]) => {
                let newSeconds = seconds + 1;
                let newMinutes = minutes;
                
                if (newSeconds >= 60) {
                    newSeconds = 0;
                    newMinutes++;
                }

                return [newMinutes, newSeconds]
            });
        }, 1000);
        return () => clearInterval(interval);
    }, [searching]);

    return (
        <div className="flex flex-row w-full h-auto">
            <div className="h-auto flex-1 p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]">
               <h1 className="text-white text-xl font-[Oswald]">Choose your playground</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    {options?.categories?.map((category) => (
                        <SelectButton
                            key={category.id}
                            isSearching = {searching}
                            text={category.name}
                            isSelected={selectedCategory === category.id}
                            onToggle={() => {handleToggleCategory(category.id)}}
                        />
                    ))}
               </div>
            </div>
            <div className="flex justify-center items-center w-auto h-auto bg-[#00D1FF] border-solid border-x-10 border-white">
                <img src="/arrow.svg" alt="Arrow Icon" />
            </div>
            <div className={clsx(selectedCategory ? "blur-none" : "blur-[2px]", "h-auto flex-1 p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]")}>
               <h1 className="text-white text-xl font-[Oswald]">Now, define your role</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    {filteredRoles?.map(role => (
                         <SelectButton
                            key={role.id}
                            isSearching = {searching}
                            text={role.name}
                            isSelected={selectedRole === role.id}
                            onToggle={() => {
                                handleToggleRole(role.id);
                                setSelectedTool(null);
                            }}
                        />
                    ))}
               </div>
            </div>
            <div className="flex justify-center items-center w-auto h-auto bg-[#00D1FF] border-solid border-x-10 border-white">
                <img src="/arrow.svg" alt="Arrow Icon" />
            </div>
            <div className={clsx(selectedRole ? "blur-none" : "blur-[2px]", "h-auto flex-1 p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]")}>
               <h1 className="text-white text-xl font-[Oswald]">Choose your tool</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    {filteredTools?.map(tool => (
                        <SelectButton
                            key={tool.id}
                            isSearching = {searching}
                            text={tool.name}
                            isSelected={selectedTool?.includes(tool.id) ?? false}
                            onToggle={() => handleToggleTool(tool.id)}
                        />
                    ))}
               </div>
            </div>
            <div className="flex justify-center items-center w-auto h-auto bg-[#00D1FF] border-solid border-l-10 border-white">
                <img src="/arrow.svg" alt="Arrow Icon" />
            </div>
            <div className={clsx(selectedTool ? "blur-none" : "blur-[2px]", "flex flex-1 h-auto justify-center items-center bg-white px-5")}>
                <StartButton
                    options={[selectedCategory, selectedRole, selectedTool]}
                    isSearching={searching}
                    timeInQueue={`${timeInQueue[0]} min ${timeInQueue[1]} sec`}
                    onToggle={() => handleQueueToggle(selectedCategory!, selectedRole!, selectedTool!)}
                />
            </div>
        </div>
    );
}

export default TeamSelectionView;