namespace api.Utils
{
    public class MeetingTimeHelper : IMeetingTimeHelper
    {
        public bool IsValidMeetingTime(string proposedTime)
        {
            var now = DateTime.UtcNow;
            var proposed = ParseMeetingTime(proposedTime);
            var maxTime = now.AddDays(7);
            return proposed > now && proposed <= maxTime;
        }

        public DateTime ParseMeetingTime(string meetingTimeString)
        {
            return DateTime.Parse(meetingTimeString, null, 
                System.Globalization.DateTimeStyles.RoundtripKind);
        }
    }
}
