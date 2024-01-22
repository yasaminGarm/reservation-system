using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeanSceneWebApp.Areas.Administration.Controllers
{
    public class EmployeeController : AdministrationBaseController
    {

        private readonly UserManager<IdentityUser> _userManager;
        public EmployeeController(ApplicationDbContext context, UserManager<IdentityUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task <IActionResult> Index()
        {
            //var users=await _context.Users.ToListAsync();
            var m = new Models.Employee.Index
            {
                Users=await _context.Users.Select(u=>new Models.Employee.Index.UserInfo
                {
                    Id=u.Id,
                    Username=u.UserName,
                    IsAdmin=_context.UserRoles.Any(
                        ur=>
                        ur.UserId==u.Id 
                        &&
                        
                        ur.RoleId==_context.Roles.First(r=>r.Name=="Administration").Id)


                }).ToListAsync(),
            };
                return View(m);
        }



        [HttpGet]
        public IActionResult Create()
        {
            var m = new Models.Employee.Create { Password = "1" };
            return View(m);
        }



        [HttpPost]
        public async Task<IActionResult> Create(Models.Employee.Create m)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { Email = m.Email, UserName = m.Email };
                var result = await _userManager.CreateAsync(user, m.Password);
                if (result.Succeeded)
                {
                   
                   
                    
                    if(m.AssignAdminRole)
                    {
                        await _userManager.AddToRoleAsync(user, "Administration");

                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Employee");
                    }
                 
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
    }
}
