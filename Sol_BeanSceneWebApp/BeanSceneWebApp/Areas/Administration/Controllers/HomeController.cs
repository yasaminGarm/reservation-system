using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace BeanSceneWebApp.Areas.Administration.Controllers
{
    public class HomeController : AdministrationBaseController
    {
        public HomeController(ApplicationDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
