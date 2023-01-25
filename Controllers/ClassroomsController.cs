using Microsoft.AspNetCore.Mvc;

namespace WestcoastEducationRESTDel1.api.Controllers
{
    [ApiController]
    [Route("api/v1/classrooms")]
    public class ClassroomsController : ControllerBase
    {
        //ActionResult används för att få tillgång till statuskoder
        public ActionResult List()
        {
            return Ok(new { message = "Lista kurser fungerar" });
        }
    }
}