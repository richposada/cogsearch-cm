using Microsoft.AspNetCore.Mvc;
using CognitiveSearch.UI.Models;
using System.Collections.Generic;

namespace CognitiveSearch.UI.Controllers
{
    public class RequestsController : Controller
    {
        public IActionResult Index()
        {
            //get existing FOIA Requests
            FOIARequestsViewModel model = new FOIARequestsViewModel();
            FOIARequest fr = new FOIARequest(); 
            model.FOIARequests.Add(fr);
            return View(model);
        }
    }
}
