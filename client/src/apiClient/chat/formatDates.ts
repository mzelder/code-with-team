export const formatToLocalTime = (utcTime: string): Date => {
    return utcTime.endsWith('Z') ? new Date(utcTime) : new Date(utcTime + 'Z');
}

export const formatProposalDate = (isoDate: string, shortWeekday: boolean = false): string => {
    const date = formatToLocalTime(isoDate);
    const options: Intl.DateTimeFormatOptions = { 
        weekday: shortWeekday ? 'short' : 'long', 
        day: 'numeric', 
        month: 'long', 
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false,
    };
    return date.toLocaleDateString('en-PL', options);
};

export const formatMessageDate = (isoDate: string): string => {
    const d = formatToLocalTime(isoDate);
    const now = new Date();
    const isToday = d.toDateString() === now.toDateString();
    
    const hours = String(d.getHours()).padStart(2, '0');
    const minutes = String(d.getMinutes()).padStart(2, '0');
    
    if (isToday) {
        return `${hours}:${minutes}`;
    }
    
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    return `${day}.${month} ${hours}:${minutes}`;
}


export const parseBookDateToDate = (dateString: string): Date => {
    const parts = dateString.split(":");
    const year = new Date().getFullYear();
    const month = parseInt(parts[0]) - 1;
    const day = parseInt(parts[1]);
    const hour = parseInt(parts[2]);
    const minute = parseInt(parts[3]);
    
    return new Date(year, month, day, hour, minute);
};