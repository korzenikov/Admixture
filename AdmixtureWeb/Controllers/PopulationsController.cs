using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Admixture;
using AdmixtureWeb.Models;

namespace AdmixtureWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulationsController : ControllerBase
    {
        private readonly Repository repository;

        public PopulationsController()
        {
            repository = new Repository();
        }
        
        [Route("{id}")]
        public IActionResult Get(int id, int k)
        {
            var calculator = repository.GetCalculator(id);
            if (calculator == null)
            {
                return NotFound();
            }

            var populations = Populations.getPopulations(',', @"D:\Populations\" + calculator.FileName);

            var results = EthnoPlots.getEthnoPlot3D(k == 0 ? 6 : k, populations.Item2);
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

            return Ok(clusters);
        }
    }
}