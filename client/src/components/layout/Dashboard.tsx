import { Outlet } from "react-router-dom";
import SideMenu from "./SideMenu";

function Dashboard() {
    return (
        <div className="flex h-screen">
            <SideMenu></SideMenu>

            <div className="flex-1 p-6">
                <Outlet />
            </div>
            
        </div>
    );
}

export default Dashboard;