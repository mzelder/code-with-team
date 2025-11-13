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
            // Expected format: MM:DD:hh:mm
            var parts = meetingTimeString.Split(':');

            if (parts.Length != 4)
            {
                throw new FormatException("Invalid meeting time format. Expected MM:DD:hh:mm");
            }

            if (!int.TryParse(parts[0], out int month) ||
                !int.TryParse(parts[1], out int day) ||
                !int.TryParse(parts[2], out int hour) ||
                !int.TryParse(parts[3], out int minute))
            {
                throw new FormatException("Invalid meeting time format. All parts must be numbers.");
            }

            int year = DateTime.UtcNow.Year;

            return new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc);
        }
    }
}
