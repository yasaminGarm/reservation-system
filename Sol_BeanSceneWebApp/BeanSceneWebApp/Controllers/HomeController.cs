using BeanSceneWebApp.Models;
using BeanSceneWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BeanSceneWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PersonService _personService;


        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager,UserManager<IdentityUser>userManager, PersonService personService)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _personService = personService;
        }

        public async Task<IActionResult> Index()
        {
            //Create roles
            foreach (var r in new[] { "Administration", "Employee", "Member" })
            {
                if (!await _roleManager.RoleExistsAsync(r))
                {
                    await _roleManager.CreateAsync(new IdentityRole(r));
                }
            }

            const string adminUsername = "admin@gmail.com";

            //Create default admin user
            var dbUser = await _personService.GetPersonByUserName(adminUsername);

            if(dbUser == null)
            {
                var user = new IdentityUser { Email = adminUsername, UserName = adminUsername };
                var result = await _userManager.CreateAsync(user, "Admin1234");
                var person = await _personService.FindOrCreateAsync("admin@gmail.com", "Admin", "Admin", "45678");
                await _personService.SetUserIdAsync(person.Id, user.Id);
                await _userManager.AddToRoleAsync(user, "Administration");
            }

          


            //return View();
            return RedirectUser();


           



        }
        public IActionResult RedirectUser()
        {
            if (User.IsInRole("Member"))
            {
                return RedirectToAction("Index", "Home", new { area = "Member" });
            }else  if (User.IsInRole("Employee"))
            {
                return RedirectToAction("Index", "Home", new { area = "Employee" });
            }
            else if (User.IsInRole("Administration"))
            {
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
            else
            {
                //return RedirectToAction("Index", "Home", new { area = "" });
                return View();

            }
          

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}