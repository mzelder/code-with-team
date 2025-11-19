export const validateBookDate = (args: string[]): { valid: boolean; error?: string } => {
    const hasValidPadding = (timeParts: string[]): boolean => {
        return timeParts.every(part => part.length === 2);
    };

    const hasValidPartCount = (timeParts: string[]): boolean => {
        return timeParts.length === 4;
    };

    const hasArguments = (args: string[]): boolean => {
        return args.length !== 0;
    }

    const hasValidRanges = (
        year: number,
        month: number, 
        day: number, 
        hour: number, 
        minute: number
    ): { valid: boolean; error?: string } => {
        // month - 1-12, day - 1-31 (depends), hours - 0 - 23, minutes 0-59
        if (month < 1 || month > 12) {
            return { valid: false, error: "Month must be between 01-12" };
        }

        const maxDays = daysInMonth(year, month);
        if (day < 1 || day > maxDays) {
            return { valid: false, error: `Day must be between 01-${maxDays} for month ${String(month).padStart(2, '0')}` };
        }

        if (hour < 0 || hour > 23) {
            return { valid: false, error: "Hour must be between 00-23"};
        }

        if (minute < 0 || minute > 59) {
            return { valid: false, error: "Minute must be between 00-59"};
        }
        
        return { valid: true };
    }

    const daysInMonth = (year: number, month: number): number => {
        return new Date(year, month, 0).getDate();    
    };

    const isWithinOneWeek = (proposedDate: Date, now: Date): boolean => {
        const oneWeekFromNow = new Date(now);
        oneWeekFromNow.setDate(now.getDate() + 7);
        return proposedDate <= oneWeekFromNow;
    }
    
    if (!hasArguments(args)) {
        return { valid: true};
    }
    
    const time = args[0];
    const timeParts = time.split(":");

    if (!hasValidPartCount(timeParts)) {
        return { valid: false, error: "Input should be in this format: MM:DD:HH:MM" };
    }
    
    if (!hasValidPadding(timeParts)) {
        return { valid: false, error: "Each element should have 2 digits. Example: 01:02:03:04" }
    }

    const now = new Date();
    const year = now.getFullYear();
    const [month, day, hour, minute] = timeParts.map(Number);

    const rangeValidation = hasValidRanges(year, month, day, hour, minute);
    if (!rangeValidation.valid) {
        return rangeValidation;
    }
    
    const proposedDate = new Date(year, month - 1, day, hour, minute);
    
    if (proposedDate <= now) {
        return { valid: false, error: "Cannot propose a meeting in the past" };
    }

    if (!isWithinOneWeek(proposedDate, now)) {
        return { valid: false, error: "Meeting must be scheduled within one week from now"};
    }

    return { valid: true };
};