﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Katalye.Data;
using Katalye.Data.Entities;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Katalye.Components.Queries
{
    public static class GetMinions
    {
        public class Query : IRequest<Result>, IPaginatedQuery
        {
            public int? Page { get; set; }
            public int? Size { get; set; }

            public IList<string> GrainSearch { get; set; }
        }

        public class Result : PagedResult<Minion>
        {
        }

        public class Minion
        {
            public string Id { get; set; }

            public IList<string> Os { get; set; }

            public IList<string> Master { get; set; }

            public IList<string> SaltMinionVersion { get; set; }

            public IList<string> IpV4Addresses { get; set; }

            public DateTimeOffset? LastAuthenticated { get; set; }

            public DateTimeOffset? LastSeen { get; set; }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly KatalyeContext _context;

            public Handler(KatalyeContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Query message, CancellationToken cancellationToken)
            {
                var valuePairs = (message.GrainSearch ?? new List<string>())
                                 .Select(x => x.Split(new[] {','}, 2))
                                 .Select(x => new
                                 {
                                     Key = x.First(),
                                     Value = x.Last()
                                 })
                                 .ToList();

                var expectedMatches = valuePairs.Count;

                var predicate = PredicateBuilder.New<MinionGrainValue>(false);
                foreach (var pair in valuePairs)
                {
                    predicate = predicate.Or(x => x.MinionGrain.Path == pair.Key && EF.Functions.ILike(x.Value, $"%{pair.Value}%"));
                }

                var query = _context.MinionGrainValues
                                    .Where(predicate)
                                    .Select(x => new
                                    {
                                        x.MinionGrain.Generation,
                                        x.MinionGrainId
                                    })
                                    .Distinct()
                                    .GroupBy(x => x.Generation)
                                    .Where(x => x.Count() == expectedMatches)
                                    .Select(x => x.Key);

                var result = await (from minion in _context.Minions
                                    let grains = _context.MinionGrains
                                                         .Where(x => x.MinionId == minion.Id && x.Generation == minion.GrainGeneration)
                                    from os in grains.Where(x => x.Path == "os").DefaultIfEmpty()
                                    from master in grains.Where(x => x.Path == "master").DefaultIfEmpty()
                                    from version in grains.Where(x => x.Path == "saltversioninfo").DefaultIfEmpty()
                                    from ipv4Addresses in grains.Where(x => x.Path == "ipv4").DefaultIfEmpty()
                                    where expectedMatches == 0 || query.Any(x => x == minion.GrainGeneration)
                                    orderby minion.MinionSlug
                                    select new Minion
                                    {
                                        Id = minion.MinionSlug,
                                        LastAuthenticated = minion.LastAuthentication,
                                        LastSeen = minion.LastSeen,
                                        IpV4Addresses = ipv4Addresses.Values,
                                        Master = master.Values,
                                        Os = os.Values,
                                        SaltMinionVersion = version.Values
                                    }).PageAsync(message, new Result());

                return result;
            }
        }
    }
}