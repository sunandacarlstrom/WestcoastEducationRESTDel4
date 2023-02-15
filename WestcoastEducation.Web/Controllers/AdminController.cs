using Microsoft.AspNetCore.Mvc;

namespace WestcoastEducation.Web.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View("Index");
    }
}
