import { useState } from "react";
import SideMenu from "./SideMenu";
import MatchmakingContainer from "../matchmaking/MatchmakingContainer";
import MentorContainer from "../mentor/MentorContainer";
import RoleSelector from "../role/RoleSelector";

function Dashboard() {
    const [userRole, setUserRole] = useState<string | null>(
        localStorage.getItem("userRole")
    );
    
    const handleRoleSelected = () => {
        setUserRole(localStorage.getItem("userRole"));
    };
    
    if (!userRole) {
        return <RoleSelector onRoleSelected={handleRoleSelected} />;
    }
    
    return (
        <div className="flex h-full">
            <SideMenu></SideMenu>

            <div className="flex-1 h-full">
                {userRole === "player" ? <MatchmakingContainer /> : <MentorContainer />}
            </div>
        </div>
    );
}

export default Dashboard;