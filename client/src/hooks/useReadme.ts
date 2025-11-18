import { useEffect, useState } from "react";

export function useReadme(
    repoUrl?: string | null,
    filename?: string | null
): string {
    const [readme, setReadme] = useState<string>("");

    useEffect(() => {
        const fetchReadme = async() => {
            try {
                if (!repoUrl || !filename) {
                    setReadme("");
                    return;
                }

                const match = repoUrl.match(/github\.com\/([^/]+)\/([^/]+)/);
                if (!match) return;

                const [_, owner, repo] = match;
                const response = await fetch(`https://raw.githubusercontent.com/${owner}/${repo}/main/${filename}`);
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