using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Admixture;
using System.Collections.Generic;

namespace AdmixtureWeb.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulationsController : ControllerBase
    {
        public IEnumerable<Cluster> Get()
        {
            var populations = Populations.getPopulations(',', @"D:\Populations\K15.csv");

            var results = EthnoPlots.getEthnoPlot3D(6, populations.Item2);
            var clusters = results.GroupBy(i => i.Item5).Select(
                g =>
                    new Cluster
                    {
                        Index = g.Key,
                        Label = g.Select(i => i.Item1),
                        X = g.Select(i => i.Item2),
                        Y = g.Select(i => i.Item3),
                        Z = g.Select(i => i.Item4),
                    }
            );

            return clusters;
        }
    }
}