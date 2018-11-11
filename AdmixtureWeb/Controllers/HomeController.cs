using System.Diagnostics;
using System.Linq;
using AdmixtureWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdmixtureWeb.Controllers
{
    public class HomeController : Controller
    {
        private Repository repository;

        public HomeController()
        {
            repository = new Repository();
        }

        public IActionResult Index(int id, int k)
        {
            if (k < 1 || k > 10)
            {
                k = 6;
            }
            ViewBag.Calculators = new SelectList(repository.GetCalculators(), nameof(Calculator.Id), nameof(Calculator.Name), id);
            ViewBag.Clusters = Enumerable.Range(1, 10).Select(i => new SelectListItem(i.ToString(), i.ToString(), i == k)).ToArray();
                    
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
    }
}
