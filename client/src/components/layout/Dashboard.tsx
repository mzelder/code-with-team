import { Outlet } from "react-router-dom";
import SideMenu from "./SideMenu";

function Dashboard() {
    return (
        <div className="flex h-full">
            <SideMenu></SideMenu>

            <div className="flex-1 h-full">
                <Outlet />
            </div>
            
        </div>
    );
}

export default Dashboard;