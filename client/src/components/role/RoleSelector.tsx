enum Role {
    Player,
    Mentor
};

interface RoleSelectorProps {
    onRoleSelected: () => void;
}

function RoleSelector({ onRoleSelected }: RoleSelectorProps) {
    const handleRoleSelect = (role: Role) => {
        if (role === Role.Player) {
            localStorage.setItem("userRole", "player");
            onRoleSelected();
            return;
        } 

        if (role === Role.Mentor) {
            localStorage.setItem("userRole", "mentor");
            onRoleSelected();
            return;
        }
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
                        onClick={() => handleRoleSelect(Role.Player)}
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

                    <div 
                        onClick={() => handleRoleSelect(Role.Mentor)}
                        className="bg-white rounded-2xl p-10 text-center cursor-pointer transition-all duration-300 hover:-translate-y-2 hover:shadow-2xl active:translate-y-0 flex flex-col items-center group"
                    >
                        <div className="text-7xl mb-6 transition-transform duration-300 group-hover:scale-110">
                            🎓
                        </div>
                        <h2 className="text-gray-800 mb-4 text-2xl font-semibold">
                            Mentor a Team
                        </h2>
                        <p className="text-gray-600 mb-8 leading-relaxed flex-grow min-h-[3rem]">
                            Guide and support teams with your experience and expertise
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RoleSelector;