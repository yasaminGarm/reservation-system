using Microsoft.AspNetCore.Mvc;

namespace BeanSceneWebApp.Areas.Administration.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
