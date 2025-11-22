import { Role } from "../../utils/RoleGuard";
import MatchmakingContainer from "../matchmaking/MatchmakingContainer";
import MentorContainer from "../mentor/MentorContainer";
import SideMenu from "./SideMenu";

function Dashboard() {
    const role = localStorage.getItem("userRole"); 
    
    return (
        <div className="flex h-full">
            <SideMenu></SideMenu>

            <div className="flex-1 h-full">
                {role === Role.Player ? <MatchmakingContainer /> : <MentorContainer />}
            </div>
        </div>
    );
}

export default Dashboard;