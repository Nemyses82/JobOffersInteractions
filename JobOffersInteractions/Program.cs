using System;
using System.Collections.Generic;
using System.Linq;

namespace JobOffersInteractions
{
    public class Interaction
    {
        public int UserId { get; set; }
        public int RandomJobOfferId { get; set; }
    }

    // 1000 records of combined interaction data(after filtering by eventType and eventValueThreshold, if provided)
    //
    // 25 unique users with at least 2 interactions each

    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();

            const int numberOfInteractions = 1000;
            const int numberOfUniqueUsers = 25;
            const int numberOfMinimumInteractions = 2;

            var interactions = new List<Interaction>();

            while (interactions.Count < numberOfInteractions)
            {
                var randomUserId = random.Next(0, 1000);
                var randomJobOfferId = random.Next(30, 40);

                var grouped = interactions.GroupBy(a => a.UserId)
                    .Select(g => new { userId = g.Key, Count = g.Count() }).OrderBy(x => x.userId).ToList();

                // var itemInGroup = grouped.FirstOrDefault(x => x.userId == randomUserId);

                // fill interactions with 25 unique items
                if (interactions.Count < numberOfUniqueUsers && interactions.Count(x => x.UserId == randomUserId) == 0)
                {
                    interactions.Add(new Interaction
                    {
                        UserId = randomUserId,
                        RandomJobOfferId = randomJobOfferId
                    });
                    continue;
                }

                // ensure I have at least 25 unique items (regardless number of interactions)
                // if (interactions.Count >= numberOfUniqueUsers)
                // {
                //     // look up for already existing item and ensure they have at least 2 interactions
                //     if (itemInGroup != null && itemInGroup.Count < numberOfMinimumInteractions)
                //     {
                //         interactions.Add(new Interaction
                //         {
                //             UserId = randomUserId,
                //             RandomJobOfferId = randomJobOfferId
                //         });
                //         continue;
                //     }
                //     
                //     // ensure I have 25 unique items
                //     if (grouped.Count(x => x.Count >= numberOfMinimumInteractions) >= numberOfUniqueUsers)
                //     {

                        var allWithTwoInteractionGrouped = interactions
                            .GroupBy(a => a.UserId)
                            .Select(g => new { userId = g.Key, Count = g.Count() })
                            .Where(x => x.Count >= numberOfMinimumInteractions).ToList();

                        var allWithTwoInteraction = interactions.Where(x =>
                            allWithTwoInteractionGrouped.Select(y => y.userId).Contains(x.UserId)).ToList();

                        var allWithOneInteraction = interactions.Where(x =>
                            !allWithTwoInteractionGrouped.Select(y => y.userId).Contains(x.UserId)).ToList();

                        var oneInteractionMargin = allWithOneInteraction.Count * 2;

                        var marginForMinimumInteractions = numberOfInteractions - (allWithTwoInteraction.Count + oneInteractionMargin);

                        if (marginForMinimumInteractions == 0)
                        {
                            var itemInGroup = grouped.FirstOrDefault(x => x.userId == randomUserId && x.Count == 1);
                            if (itemInGroup != null)
                            {
                                interactions.Add(new Interaction
                                {
                                    UserId = randomUserId,
                                    RandomJobOfferId = randomJobOfferId
                                });
                            }
                            continue;
                        }

                        interactions.Add(new Interaction
                        {
                            UserId = randomUserId,
                            RandomJobOfferId = randomJobOfferId
                        });
                //     }
                //
                // }
            }

            //
            // Solution that works but I may encounter case when item has only one interaction
            //
            // while (interactions.Count < numberOfInteractions)
            // {
            //     var randomUserId = random.Next(10, 100);
            //     var randomJobOfferId = random.Next(30, 40);
            //
            //     var grouped = interactions.GroupBy(a => a.UserId)
            //         .Select(g => new { userId = g.Key, Count = g.Count() }).ToList();
            //
            //     var itemInGroup = grouped.FirstOrDefault(x => x.userId == randomUserId);
            //
            //     // fill interactions with 25 unique items
            //     if (interactions.Count < numberOfUniqueUsers && interactions.Count(x => x.UserId == randomUserId) == 0)
            //     {
            //         interactions.Add(new Interaction
            //         {
            //             UserId = randomUserId,
            //             RandomJobOfferId = randomJobOfferId
            //         });
            //         continue;
            //     }
            //
            //     // ensure I have at least 25 unique items (regardless number of interactions)
            //     if (interactions.Count >= numberOfUniqueUsers)
            //     {
            //         // look up for already existing item and ensure they have at least 2 interactions
            //         if (itemInGroup != null && itemInGroup.Count < numberOfMinimumInteractions)
            //         {
            //             interactions.Add(new Interaction

            //             {
            //                 UserId = randomUserId,
            //                 RandomJobOfferId = randomJobOfferId
            //             });
            //             continue;
            //         }
            //
            //         // ensure I have 25 unique items
            //         if (grouped.Count(x => x.Count >= numberOfMinimumInteractions) >= numberOfUniqueUsers)
            //         {
            //             interactions.Add(new Interaction
            //             {
            //                 UserId = randomUserId,
            //                 RandomJobOfferId = randomJobOfferId
            //             });
            //         }
            //
            //     }
            // }

            interactions = interactions.OrderBy(x => x.UserId).ToList();
            var groupedInteractions = interactions.GroupBy(a => a.UserId)
                .Select(g => new { userId = g.Key, Count = g.Count() }).OrderBy(x => x.userId).ToList();
            groupedInteractions.ForEach(x =>
                {
                    Console.WriteLine($"UserId: {x.userId} - Count: {x.Count}");
                });

            Console.ReadLine();
        }
    }
}
