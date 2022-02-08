namespace JobOffersInteractions.Models
{
    public record Interaction(int UserId, int JobId, long EpochTime)
    {
        public int UserId { get; set; } = UserId;
        public int JobId { get; } = JobId;
        public long EpochTime { get; } = EpochTime;
    }
}
