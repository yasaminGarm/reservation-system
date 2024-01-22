using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace BeanSceneWebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class AdministrationBaseController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public AdministrationBaseController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
