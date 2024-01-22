using BeanSceneWebApp.Data;
using BeanSceneWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeanSceneWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly PersonService _personService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public ProductController(ApplicationDbContext context, PersonService personService, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _personService = personService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var products = _context.Products.Include(p => p.Category);
            return View(await products.ToListAsync());
        }
    }
}
