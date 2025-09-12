import { useState } from "react";
import SelectButton from "./SelectButton";

function FindTeamForm() {
    const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
    const [selectedRole, setSelectedRole] = useState<string | null>(null);
    const [selectedTool, setSelectedTool] = useState<string[]>([]);

    const handleToggleCategory = (value: string) => setSelectedCategory(value);
    const handleToggleRole = (value: string) => setSelectedRole(value);
    const handleToggleTool = (value: string) => {
        setSelectedTool((prev) => 
            prev.includes(value) ? prev.filter((v) => v !== value) : [...prev, value]
        );
    };

    return (
        <div className="flex flex-row w-full h-auto">
            <div className="h-auto w-auto min-w-[15vw] p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]">
               <h1 className="text-white text-xl font-[Oswald]">Choose your playground</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    {["Test1", "Test2", "Test3"].map((label) => (
                        <SelectButton
                            key={label}
                            text={label}
                            isSelected={selectedCategory === label}
                            onToggle={() => handleToggleCategory(label)}
                        />
                    ))}
               </div>
            </div>
            <div className="flex justify-center items-center w-auto h-auto bg-[#00D1FF] border-solid border-x-10 border-white">
                <img src="/arrow.svg" alt="Arrow Icon" />
            </div>
            <div className="h-auto w-auto min-w-[15vw] p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]">
               <h1 className="text-white text-xl font-[Oswald]">Now, define your role</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    {["Test1", "Test2", "Test3"].map((label) => (
                        <SelectButton
                            key={label}
                            text={label}
                            isSelected={selectedRole === label}
                            onToggle={() => handleToggleRole(label)}
                        />
                    ))}
               </div>
            </div>
            <div className="flex justify-center items-center w-auto h-auto bg-[#00D1FF] border-solid border-x-10 border-white">
                <img src="./public/arrow.svg" alt="Arrow Icon" />
            </div>
            <div className="h-auto w-auto min-w-[15vw] p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]">
               <h1 className="text-white text-xl font-[Oswald]">Choose your tool</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    {["Test1", "Test2", "Test3"].map((label) => (
                        <SelectButton
                            key={label}
                            text={label}
                            isSelected={selectedTool.includes(label)}
                            onToggle={() => handleToggleTool(label)}
                        />
                    ))}
               </div>
            </div>
            <div className="flex justify-center items-center w-auto h-auto bg-[#00D1FF] border-solid border-x-10 border-white">
                <img src="./public/arrow.svg" alt="Arrow Icon" />
            </div>
            <div className="flex w-auto min-w-[15vw] h-auto justify-center items-center bg-white px-5">
                <h1 className="text-3xl text-[#00D1FF] font-[Oswald] font-bold">START</h1>
            </div>
        </div>
    );
}

export default FindTeamForm;