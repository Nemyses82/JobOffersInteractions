namespace JobOffersInteractions.Models
{
    public record JobOffer
    {
        public int JobId { get; set; }

        public int? CompanyId { get; set; }

        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        public string Salary { get; set; }

        public string Genre { get; set; }
    }
}

