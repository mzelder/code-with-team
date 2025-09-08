import { Outlet, Navigate } from 'react-router-dom';
import { validateCookies } from '../apiClient/auth';
import { useState, useEffect } from 'react';

 const ProtectedRoutes = () => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);
    
    useEffect(() => {
        const checkAuth = async () => {
            try {
                const result = await validateCookies();
                setIsAuthenticated(result?.isAuthenticated || false);
            } catch (error) {
                setIsAuthenticated(false);
            }
        };
        
        checkAuth();
    }, []);

    if (isAuthenticated === null) {
        return <div className="text-white">Loading...</div>;
    }

    return isAuthenticated ? <Outlet /> : <Navigate to="/signin" />
 }

 export default ProtectedRoutes;