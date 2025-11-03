import { useEffect, useState } from "react";
import { validateCookies } from "../apiClient/auth/auth";

function useCurrentUser() {
    const [username, setUsername] = useState<string | null>(null);

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await validateCookies();
                if (response.isAuthenticated) {
                    setUsername(response.user);
                }
            } catch (error) {
                console.error("Failed to fetch user: " + error);
                setUsername(null);
            }
        };

        fetchUser();
    }, []);
    return username;
}

export default useCurrentUser;