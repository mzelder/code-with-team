import { useEffect, useState } from "react";
import { Navigate, Outlet } from "react-router-dom";
import { getMentorStatus } from "../apiClient/mentor/mentor";

export enum Role {
    Player = "player",
    Mentor = "mentor"
}

const RoleGuard = () => {
    const [isRoleSelected, setIsRoleSelected] = useState<boolean | null>(null);

    useEffect(() => {
        const role = localStorage.getItem("userRole");
        
        if (role === Role.Player) {
            setIsRoleSelected(true);
            return;
        }

        if (role === Role.Mentor) {
            fetchMentor();
            return;
        }

        localStorage.removeItem("userRole");
        setIsRoleSelected(false);
    }, []);

    const fetchMentor = async() => {
        try {
            const result = await getMentorStatus();
            setIsRoleSelected(result.isMentor);
        } catch (error) {
            setIsRoleSelected(false);
        }
    }
    
    if (isRoleSelected === null) {
        return <div className="text-white">Loading...</div>;
    }

    return isRoleSelected ? <Outlet /> : <Navigate to="/choose-role" />;
};

export default RoleGuard;