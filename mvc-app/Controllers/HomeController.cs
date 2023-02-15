using Microsoft.AspNetCore.Mvc;

namespace WestcoastEducation.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View("Index");
    }
}
