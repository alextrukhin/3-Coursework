using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using PL.Models;
using System.Diagnostics;

namespace PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("db_type")))
            {
                HttpContext.Session.SetString("db_type", "json");
            }

            CategoryService categoryService = new(HttpContext.Session.GetString("db_type") ?? "json");

            return View(new IndexViewModel { DBTypes = new List<string>(categoryService.AvailableDBTypes), DBType = HttpContext.Session.GetString("db_type") });
        }

        [HttpPost]
        public IActionResult Index(string db_type)
        {
            HttpContext.Session.SetString("db_type", db_type ?? "json");
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Search([FromQuery(Name = "q")] string? query)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");
            return View(new SearchViewModel { Products = products.Search(query), Suppliers = suppliers.Search(query), Query = query ?? "" });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    public class IndexViewModel
    {
        public List<string> DBTypes { get; set; }
        public string? DBType { get; set; }
    }
    public class SearchViewModel
    {
        public List<Product> Products { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public string Query { get; set; }
    }
}
