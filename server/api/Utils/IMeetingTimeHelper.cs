namespace api.Utils
{
    public interface IMeetingTimeHelper
    {
        bool IsValidMeetingTime(string proposedTime);
        DateTime ParseMeetingTime(string meetingTimeString);
    }
}