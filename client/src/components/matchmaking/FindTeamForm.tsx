import SelectButton from "./SelectButton";

function FindTeamForm() {
    
    return (
        <div className="flex flex-row w-full h-auto">
            <div className="h-auto w-auto p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]">
               <h1 className="text-white text-xl font-[Oswald]">Choose your playground</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    <SelectButton text="Test1"></SelectButton>
                    <SelectButton text="Test1"></SelectButton>
                    <SelectButton text="Test1"></SelectButton>
               </div>
            </div>
            <div className="flex justify-center items-center w-auto h-auto bg-[#00D1FF] border-solid border-x-10 border-white">
                <img src="./public/arrow.svg" alt="Arrow Icon" />
            </div>
            <div className="h-auto w-auto p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]">
               <h1 className="text-white text-xl font-[Oswald]">Choose your playground</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    <SelectButton text="Test1"></SelectButton>
                    <SelectButton text="Test1"></SelectButton>
                    <SelectButton text="Test1"></SelectButton>
               </div>
            </div>
            <div className="flex justify-center items-center w-auto h-auto bg-[#00D1FF] border-solid border-x-10 border-white">
                <img src="./public/arrow.svg" alt="Arrow Icon" />
            </div>
            <div className="h-auto w-auto p-5 bg-[#1F2937] justify-center items-center border-solid border-5 border-[#374151]">
               <h1 className="text-white text-xl font-[Oswald]">Choose your playground</h1> 
               <div className="flex flex-col h-auto space-y-3 justify-center py-5">
                    <SelectButton text="Test1"></SelectButton>
                    <SelectButton text="Test1"></SelectButton>
                    <SelectButton text="Test1"></SelectButton>
               </div>
            </div>
            <div className="flex justify-center items-center w-auto h-auto bg-[#00D1FF] border-solid border-x-10 border-white">
                <img src="./public/arrow.svg" alt="Arrow Icon" />
            </div>
            <div className="flex w-auto h-auto justify-center items-center bg-white px-5">
                <h1 className="text-xl text-[#00D1FF] font-[Oswald] font-bold">START</h1>
            </div>
        </div>
    );
}

export default FindTeamForm;