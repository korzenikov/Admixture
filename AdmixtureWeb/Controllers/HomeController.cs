using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdmixtureWeb.Models;
using Admixture;
using Newtonsoft.Json;

namespace AdmixtureWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Data"] = GetData();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetData()
        {
            var populations = Populations.getPopulations(',', @"D:\Populations\K36.csv");
            var samples = Populations.getPopulations(',', @"D:\Populations\K36samples.csv");
            var clusters = EthnoPlots.getEthnoPlot3DWithSamplesClusters(6, populations.Item2, samples.Item2);
            return JsonConvert.SerializeObject(clusters);
        }
    }
}
