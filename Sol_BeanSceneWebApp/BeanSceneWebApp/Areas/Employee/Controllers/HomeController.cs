using BeanSceneWebApp.Areas.Administration.Controllers;
using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace BeanSceneWebApp.Areas.Employee.Controllers
{
    public class HomeController : EmployeeBaseController
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
