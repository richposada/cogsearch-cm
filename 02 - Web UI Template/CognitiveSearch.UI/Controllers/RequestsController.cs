using Microsoft.AspNetCore.Mvc;

namespace CognitiveSearch.UI.Controllers
{
    public class RequestsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
