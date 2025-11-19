export interface Command {
    name: string;
    description: string[];
    args?: string[];
    validator?: (args: string[]) => { valid: boolean; error?: string };
    handler: (args: string[]) => void;
}