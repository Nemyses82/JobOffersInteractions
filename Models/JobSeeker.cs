using System;
using Bogus.DataSets;

namespace JobOffersInteractions.Models
{
    public record JobSeeker(int JobSeekerId)
    {
        public int JobSeekerId { get; set; } = JobSeekerId;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string SomethingUnique { get; set; }
        public Guid CartId { get; set; }
        public string FullName { get; set; }
        public Name.Gender Gender { get; set; }
    }
}
