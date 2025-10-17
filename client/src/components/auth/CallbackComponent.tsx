import { useEffect } from "react";
import toast from "react-hot-toast";
import { useNavigate, useSearchParams } from "react-router-dom";
import { handleGithubCallback } from "../../apiClient/auth/auth";


function CallbackComponent() {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    
    useEffect(() => {
        const processCallback = async () => {
            const code = searchParams.get("code");
            const error = searchParams.get("error");

            if (error) {
                toast.error("Oauth Error");
                navigate("/signin", { replace: true });
            } 

            try {
                const response = await handleGithubCallback(code);
                toast.success(response.message);
                navigate("/app", { replace: true });
            } catch (e) {
                toast.error("Something went wrong: " + e);
                navigate("/signin", { replace: true });
            }
        }
        
        processCallback();
    }, [searchParams, navigate]);
    

    return <div className="text-white">Loading...</div>;
}

export default CallbackComponent;