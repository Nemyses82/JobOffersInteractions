using System;
using CsvHelper.Configuration;

namespace JobOffersInteractions.Models
{
    public sealed class InteractionMap : ClassMap<Interaction>
    {
        public InteractionMap()
        {
            Map(m => m.JobId).Index(0).Name("ITEM_ID");
            Map(m => m.UserId).Index(1).Name("USER_ID");
            Map(m => m.EpochTime).Index(2).Name("TIMESTAMP");
        }
    }
}
