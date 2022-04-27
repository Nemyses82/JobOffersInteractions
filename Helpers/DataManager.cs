using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Bogus;
using Bogus.DataSets;
using CsvHelper;
using JobOffersInteractions.Models;

namespace JobOffersInteractions.Helpers
{
    internal static class DataManager
    {
        public static IEnumerable<JobSeeker> SetJobSeeker()
        {
            var userIds = 1;
            var testUsers = new Faker<JobSeeker>()
                //Optional: Call for objects that have complex initialization
                .CustomInstantiator(f => new JobSeeker(userIds++))

                //Use an enum outside scope.
                .RuleFor(u => u.Gender, f => f.PickRandom<Name.Gender>())

                //Basic rules using built-in generators
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Username, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")

                .RuleFor(u => u.Password, f => f.Internet.Password(5, true))

                .RuleFor(u => u.CartId, f => Guid.NewGuid())
                .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
                .FinishWith((f, u) =>
                {
                    Console.WriteLine("User Created! Id={0}", u.JobSeekerId);
                });

            var jobSeekers = Enumerable.Range(1, 1000).Select(x => testUsers.Generate()).ToList();

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

            // string[] keywords = { "senior", "developer", ".net", "c#", "architect", "asp.net", "java", "javascript", "software", "engineer", "agile", "tester", "test" };
            string[] keywords = { ".net", "c#", "asp.net", "java", "javascript"};
            // string[] keywords = { "tester", "test" };
            // string[] keywords = { "architect" };

            jobs = jobs.Where(x => ContainsAny(x.JobTitle.ToLower(), keywords.ToList())).ToList();

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

        public static bool ContainsAny(string stringToTest, List<string> substrings)
        {
            if (string.IsNullOrEmpty(stringToTest) || substrings == null)
                return false;

            return substrings.Any(stringToTest.Contains);
        }
    }
}
