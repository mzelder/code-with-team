namespace api.Dtos.Matchmaking
{
    public class QueueTimeDto
    {
        public bool Success { get; set; }
        public string QueueTime { get; set; } 
        public QueueTimeDto(bool Success, string QueueTime)
        {
            this.Success = Success;
            this.QueueTime = QueueTime;
        }
    }
}
