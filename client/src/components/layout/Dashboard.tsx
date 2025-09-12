import { FindTeamForm } from "../matchmaking";
import SideMenu from "./SideMenu";

function Dashboard() {
    return (
        <div className="flex h-screen">
            <SideMenu></SideMenu>

            <div className="flex-1 p-6">
                <FindTeamForm></FindTeamForm>     
            </div>
            
        </div>
    );
}

export default Dashboard;