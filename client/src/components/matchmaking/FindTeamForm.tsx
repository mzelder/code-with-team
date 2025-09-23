import { useEffect, useState } from "react";
import clsx from "clsx";
import SelectButton from "./SelectButton";
import type { MatchmakingResponseDto } from "../../apiClient/matchmaking/dtos";
import { getMatchmakingOptions, startQueue } from "../../apiClient/matchmaking/matchmaking";
import toast from "react-hot-toast";

function FindTeamForm() {
    const [selectedCategory, setSelectedCategory] = useState<number | null>(null);
    const [selectedRole, setSelectedRole] = useState<number | null>(null);
    const [selectedTool, setSelectedTool] = useState<number[] | null>(null);
    const [metadata, setMetadata] = useState<MatchmakingResponseDto | null>(null);

    const handleToggleCategory = (value: number) => setSelectedCategory(value);
    const handleToggleRole = (value: number) => setSelectedRole(value);
    const handleToggleTool = (value: number) => {
        setSelectedTool((prev) => 
            prev === null ? [value] : prev.filter((v) => v !== value)
        );
    };
    const handleStart = (
        selectedCategory: number,
        selectedRole: number,
        selectedLanguages: number[]
    ) => {
        startQueue({
            categoryId: selectedCategory,
            roleId: selectedRole,
            programmingLanguageIds: selectedLanguages
        });
    };

    const filteredRoles = metadata?.roles.filter(role => { 
        const mactchkingRole = selectedCategory === role.categoryId;
        return mactchkingRole;
    });

    const filteredTools = metadata?.programmingLanguages.filter(programmingLanguage => {
        const matchingTool = selectedRole === programmingLanguage.id;
        return matchingTool;
    })

    useEffect(() => {
        const fetchMetadata = async () => {
            try {
                const data = await getMatchmakingOptions();
                setMetadata(data);
            } catch (error) {
                toast.error("Something went wrong. Please try again later.");
            }
        }

        fetchMetadata();
    }, []);

    return (
        <div className="flex flex-row w-full h-auto">
            <div className="h-auto w-auto min-w-[15vw] p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]">
               <h1 className="text-white text-xl font-[Oswald]">Choose your playground</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    {metadata?.categories?.map((category) => (
                        <SelectButton
                            key={category.id}
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
            <div className={clsx(selectedCategory ? "blur-none" : "blur-[2px]", "h-auto w-auto min-w-[15vw] p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]")}>
               <h1 className="text-white text-xl font-[Oswald]">Now, define your role</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    {filteredRoles?.map(role => (
                         <SelectButton
                            key={role.id}
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
            <div className={clsx(selectedRole ? "blur-none" : "blur-[2px]", "h-auto w-auto min-w-[15vw] p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]")}>
               <h1 className="text-white text-xl font-[Oswald]">Choose your tool</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    {filteredTools?.map(tool => (
                        <SelectButton
                            key={tool.id}
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
            <div className={clsx(selectedTool ? "blur-none" : "blur-[2px]", "flex w-auto min-w-[15vw] h-auto justify-center items-center bg-white px-5")}>
                <button 
                    style={{cursor: "pointer"}} 
                    disabled={!(selectedCategory && selectedRole && selectedTool?.length)}
                    onClick={() => handleStart(selectedCategory!, selectedRole!, selectedTool!)}
                >
                    <h1 className="text-3xl text-[#00D1FF] font-[Oswald] font-bold">START</h1>
                </button>
            </div>
        </div>
    );
}

export default FindTeamForm;