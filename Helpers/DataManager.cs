using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using JobOffersInteractions.Models;

namespace JobOffersInteractions.Helpers
{
    internal static class DataManager
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

        // public static IEnumerable<JobOffer> SetJobOffers()
        // {
        //     var jobOffers = new List<JobOffer>();
        //
        //     var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Dataset\JobOffersIds.txt");
        //
        //     foreach (var line in File.ReadLines(path))
        //     {
        //         jobOffers.Add(new JobOffer{JobId = Convert.ToInt32(line)});
        //     }
        //     
        //     return jobOffers;
        // }

        public static void GenerateCsvInteractions(List<Interaction> interactions)
        {
            var currentDomainBaseDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName;

            // var runTimeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var outputDirectory = Path.Combine(currentDomainBaseDirectory, "Output");

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            using var writer = new StreamWriter(Path.Combine(outputDirectory, @"Interactions.csv"), false);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<InteractionMap>();
            csv.WriteRecords(interactions);
        }

        public static IEnumerable<JobOffer> RetrieveJobOffers()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Dataset\JobOffers.csv");
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<JobOfferMap>();
            var jobs = csv.GetRecords<JobOffer>().ToList();

            return jobs;

            // var runTimeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // var outputDirectory = Path.Combine(runTimeDirectory, "Output");
            //
            // if (!Directory.Exists(outputDirectory))
            //     Directory.CreateDirectory(outputDirectory);
            //
            // using var writer = new StreamWriter(Path.Combine(outputDirectory, @"Interactions.csv"), false);
            // using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            // csv.Context.RegisterClassMap<InteractionMap>();
            // csv.WriteRecords(interactions);
        }
    }
}
