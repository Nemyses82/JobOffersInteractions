namespace JobOffersInteractions.Models
{
    public record JobSeeker(int JobSeekerId, string FirstName, string LastName, string Username, string Password)
    {
        public int JobSeekerId { get; set; } = JobSeekerId;
        public string FirstName { get; set; } = FirstName;
        public string LastName { get; set; } = LastName;
        public string Username { get; set; } = Username;
        public string Password { get; set; } = Password;
    }
}
