using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdmixtureWeb.Models;
using Admixture;
using Newtonsoft.Json;

namespace AdmixtureWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly double[] testPersonK15 = new double[]
        {
            15.80,
            18.44,
            31.46,
            24.02,
            0.68 ,
            2.52 ,
            0.0  ,
            0.46 ,
            2.16 ,
            0.0  ,
            3.65 ,
            0.14 ,
            0.0  ,
            0.66 ,
            0.0  ,
        };

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
            var result = Populations.getPopulations(',', @"D:\Populations\K15.csv");
            var populations = result.Item2;
            //var clusters = EthnoPlots.getEthnoPlot3DClusters(6, populations);
            var clusters = EthnoPlots.getEthnoPlot3DWithSamplesClusters(6, populations, new[] { testPersonK15 });
            return JsonConvert.SerializeObject(clusters);
        }
    }
}
