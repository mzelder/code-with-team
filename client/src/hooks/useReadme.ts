import { useEffect, useState } from "react";

export function useReadme(repoUrl: string | null) {
    const [readme, setReadme] = useState<string>("");

    useEffect(() => {
        const fetchReadme = async() => {
            try {
                if (!repoUrl) {
                    setReadme("");
                    return;
                }

                const match = repoUrl.match(/github\.com\/([^/]+)\/([^/]+)/);
                if (!match) return;

                const [_, owner, repo] = match;
                const response = await fetch(`https://raw.githubusercontent.com/${owner}/${repo}/main/README.md`);
                if (response.ok) setReadme(await response.text());
                else setReadme("Could not load README. Try refresh page.");
            } 
            catch {
                setReadme("Could not load README. Try refresh page.");
            }
        }
        
        fetchReadme();
    }, [repoUrl]);

    return readme;
}