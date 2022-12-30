using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestApp.Models;
using TestApp.Models.Dtos;
using TestApp.Services;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICheckIin _check;

        public HomeController(ILogger<HomeController> logger, ICheckIin check)
        {
            _logger = logger;
            _check = check;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ClientVm request)
        {
            await _check.SearchIin(request);
            return View(new ClientVm(){IsAvailable = request.IsAvailable});
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}