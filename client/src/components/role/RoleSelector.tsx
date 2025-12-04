import { useNavigate } from "react-router-dom";
import { Role } from "../../utils/RoleGuard";
import type { MentorDto } from "../../apiClient/mentor/dtos";
import { useEffect, useState } from "react";
import { getMentorStatus } from "../../apiClient/mentor/mentor";

function RoleSelector() {
    const navigate = useNavigate();
    const [mentor, setMentor] = useState<MentorDto | null>(null);
   
    useEffect(() => {
        const fetchMentor = async() => {
            try {
                const result = await getMentorStatus();
                setMentor(result);
            } catch (error) {
                setMentor(null);
            }
        }

        fetchMentor();
    }, []);
    
    const handlePlayerClick = () => {
        localStorage.setItem("userRole", Role.Player);
        navigate("/app");
    };

    const handleMentorClick = () => {
        if (!mentor?.appliedForMentor) {
            navigate("/mentor-form");
            return;
        }

        if (mentor.appliedForMentor && mentor.isMentor) {
            localStorage.setItem("userRole", Role.Mentor);
            navigate("/app");
        }
    };

    const getMentorButtonText = (): string => {
        if (!mentor) return "Become a Mentor";
        if (!mentor.appliedForMentor) return "Apply to Mentor";
        if (mentor.appliedForMentor && !mentor.isMentor) return "Application Under Review";
        return "Continue as Mentor";
    };

    const isMentorButtonDisabled = () => {
        return mentor?.appliedForMentor && !mentor.isMentor;
    };

    return (
        <div className="min-h-screen flex items-center justify-center p-8">
            <div className="w-full max-w-5xl">
                <h1 className="text-white mb-2 text-4xl md:text-5xl font-bold text-center">
                    Welcome! Choose Your Path
                </h1>
                <p className="text-white/90 text-center text-lg mb-12">
                    Select how you want to participate in Code with Team
                </p>
                
                <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                    {/* Player Card */}
                    <div 
                        onClick={() => handlePlayerClick()}
                        className="bg-white rounded-2xl p-10 text-center cursor-pointer transition-all duration-300 hover:-translate-y-2 hover:shadow-2xl active:translate-y-0 flex flex-col items-center group"
                    >
                        <div className="text-7xl mb-6 transition-transform duration-300 group-hover:scale-110">
                            🎮
                        </div>
                        <h2 className="text-gray-800 mb-4 text-2xl font-semibold">
                            Join a Project
                        </h2>
                        <p className="text-gray-600 mb-8 leading-relaxed flex-grow min-h-[3rem]">
                            Participate as a player and collaborate with your team on exciting projects
                        </p>
                    </div>

                    {/* Mentor Card */}
                    <div 
                        onClick={!isMentorButtonDisabled() ? handleMentorClick : undefined}
                        className={`bg-white rounded-2xl p-10 text-center transition-all duration-300 flex flex-col items-center group ${
                            isMentorButtonDisabled() 
                                ? 'opacity-60 cursor-not-allowed' 
                                : 'cursor-pointer hover:-translate-y-2 hover:shadow-2xl active:translate-y-0'
                        }`}
                    >
                        <div className={`text-7xl mb-6 transition-transform duration-300 ${!isMentorButtonDisabled() && 'group-hover:scale-110'}`}>
                            🎓
                        </div>
                        <h2 className="text-gray-800 mb-4 text-2xl font-semibold">
                            {getMentorButtonText()}
                        </h2>
                        <p className="text-gray-600 mb-8 leading-relaxed flex-grow min-h-[3rem]">
                            {isMentorButtonDisabled() 
                                ? "Your mentor application is being reviewed. We'll notify you once it's approved."
                                : "Guide and support teams with your experience and expertise"
                            }
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RoleSelector;