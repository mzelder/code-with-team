export const isMeetingLinkAccessible = (meetingTime: string): boolean => {
    const now = new Date();
    const meetingDate = new Date(meetingTime+'Z');
    const timeUntilMeeting = meetingDate.getTime() - now.getTime();
    const minutesUntilMeeting = Math.floor(timeUntilMeeting / 60_000);

    return minutesUntilMeeting <= 5 && minutesUntilMeeting >= -30;
}