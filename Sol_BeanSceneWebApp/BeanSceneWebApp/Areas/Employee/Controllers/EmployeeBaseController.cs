using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace BeanSceneWebApp.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class EmployeeBaseController : Controller
    {
        protected readonly ApplicationDbContext _context;
        public EmployeeBaseController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
