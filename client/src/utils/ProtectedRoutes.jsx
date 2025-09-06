import { Outlet, Navigate } from 'react-router-dom';
import { apiGet } from '../apiClient/apiClient';
import { useState, useEffect } from 'react';

 const ProtectedRoutes = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(null);
    
    useEffect(() => {
        const checkAuth = async () => {
            try {
                const data = await apiGet("/api/Auth/check");
                setIsAuthenticated(data?.isAuthenticated || false);
            } catch (error) {
                setIsAuthenticated(false);
            }
        };
        
        checkAuth();
    }, []);

    if (isAuthenticated === null) {
        return <div>Loading...</div>;
    }

    return isAuthenticated ? <Outlet /> : <Navigate to="/signin" />
 }

 export default ProtectedRoutes;