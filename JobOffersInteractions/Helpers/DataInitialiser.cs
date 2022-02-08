using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JobOffersInteractions.Models;

namespace JobOffersInteractions.Helpers
{
    internal static class DataInitialiser
    {
        public static IEnumerable<JobSeeker> SetJobSeeker()
        {
            var jobSeekers = new List<JobSeeker>
            {
                new(1, "", "", "", ""),
                new(2, "", "", "", ""),
                new(3, "", "", "", ""),
                new(4, "", "", "", ""),
                new(5, "", "", "", "")
            };

            return jobSeekers;
        }

        public static IEnumerable<JobOffer> SetJobOffers()
        {
            var jobOffers = new List<JobOffer>();

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Dataset\JobOffersIds.txt");

            foreach (var line in File.ReadLines(path))
            {
                jobOffers.Add(new JobOffer{JobId = Convert.ToInt32(line)});
            }
            

            return jobOffers;
        }
    }
}
