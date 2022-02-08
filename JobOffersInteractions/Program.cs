using System;
using System.Collections.Generic;
using System.Linq;
using JobOffersInteractions.Helpers;
using JobOffersInteractions.Models;

namespace JobOffersInteractions
{
    // 1000 records of combined interaction data(after filtering by eventType and eventValueThreshold, if provided)
    //
    // 25 unique users with at least 2 interactions each

    internal class Program
    {
        private static List<Interaction> _interactions = new();
        private static IEnumerable<JobOffer> _jobOffers = new List<JobOffer>();
        private static IEnumerable<JobSeeker> _jobSeekers = new List<JobSeeker>();

        private static void Main(string[] args)
        {
            var random = new Random();

            const int numberOfInteractions = 1000;
            const int numberOfUniqueUsers = 25;
            const int numberOfMinimumInteractions = 2;

            CreateDatasets();
            
            while (_interactions.Count < numberOfInteractions)
            {
                // var randomUserId = random.Next(0, 1000);
                var jobSeekerIds = _jobSeekers.Select(x => x.JobSeekerId).ToList();
                var randomUserIdIndex = random.Next(jobSeekerIds.Count);
                var randomUserId = jobSeekerIds[randomUserIdIndex];
                // var randomJobOfferId = random.Next(30, 40);
                var randomJobOfferIds = _jobOffers.Select(x => x.JobId).ToList();
                var randomJobOfferIdIndex = random.Next(randomJobOfferIds.Count);
                var randomJobOfferId = randomJobOfferIds[randomJobOfferIdIndex];

                var grouped = _interactions.GroupBy(a => a.UserId)
                    .Select(g => new { userId = g.Key, Count = g.Count() }).OrderBy(x => x.userId).ToList();

                if (_interactions.Count < numberOfUniqueUsers && _interactions.Count(x => x.UserId == randomUserId) == 0)
                {
                    _interactions.Add(new Interaction(randomUserId, randomJobOfferId,
                        new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()));
                    continue;
                }

                var allWithTwoInteractionGrouped = _interactions
                    .GroupBy(a => a.UserId)
                    .Select(g => new { userId = g.Key, Count = g.Count() })
                    .Where(x => x.Count >= numberOfMinimumInteractions).ToList();

                var allWithTwoInteraction = _interactions.Where(x =>
                    allWithTwoInteractionGrouped.Select(y => y.userId).Contains(x.UserId)).ToList();

                var allWithOneInteraction = _interactions.Where(x =>
                    !allWithTwoInteractionGrouped.Select(y => y.userId).Contains(x.UserId)).ToList();

                var oneInteractionMargin = allWithOneInteraction.Count * 2;

                var marginForMinimumInteractions = numberOfInteractions - (allWithTwoInteraction.Count + oneInteractionMargin);

                if (marginForMinimumInteractions == 0)
                {
                    var itemInGroup = grouped.FirstOrDefault(x => x.userId == randomUserId && x.Count == 1);
                    if (itemInGroup != null)
                    {
                        // _interactions.Add(new Interaction
                        // {
                        //     UserId = randomUserId,
                        //     RandomJobOfferId = randomJobOfferId
                        // });
                        _interactions.Add(new Interaction(randomUserId, randomJobOfferId,
                            new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()));
                    }
                    continue;
                }

                // _interactions.Add(new Interaction
                // {
                //     UserId = randomUserId,
                //     RandomJobOfferId = randomJobOfferId
                // });
                _interactions.Add(new Interaction(randomUserId, randomJobOfferId,
                    new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()));
            }

            _interactions = _interactions.OrderBy(x => x.UserId).ToList();
            var groupedInteractions = _interactions.GroupBy(a => a.UserId)
                .Select(g => new { userId = g.Key, Count = g.Count() }).OrderBy(x => x.userId).ToList();
            groupedInteractions.ForEach(x =>
                {
                    Console.WriteLine($"UserId: {x.userId} - Count: {x.Count}");
                });

            Console.ReadLine();
        }

        private static void CreateDatasets()
        {
            _jobSeekers = DataInitialiser.SetJobSeeker();
            _jobOffers = DataInitialiser.SetJobOffers();
        }
    }
}
