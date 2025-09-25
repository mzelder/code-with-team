namespace api.Dtos.Matchmaking
{
    public class QueueTimeDto
    {
        public bool Success { get; set; }
        public TimeSpan QueueTime { get; set; } 
        public QueueTimeDto(bool Success, TimeSpan QueueTime)
        {
            this.Success = Success;
            this.QueueTime = QueueTime;
        }
    }
}
