using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    public class SchoolController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }
    }
}