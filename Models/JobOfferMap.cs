using System;
using CsvHelper.Configuration;

namespace JobOffersInteractions.Models
{
    public sealed class JobOfferMap : ClassMap<JobOffer>
    {
        public JobOfferMap()
        {
            Map(m => m.JobId).Name("ITEM_ID");
            Map(m => m.JobTitle).Name("JOB_TITLE");
            Map(m => m.Genre).Name("GENRE");
        }
    }
}
