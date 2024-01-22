//using BeanSceneWebApp.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.DevTools.V116.Audits;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost("login/{username}")]
        public async Task<bool> login(string username, [FromBody] Order.API.Models.User user)
            
        {
            //[Authorize(Roles = "Administration")]
            
                var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, true, lockoutOnFailure: false);

            //_signInManager.

            

                if (result.Succeeded) {
                    var u = await _userManager.FindByEmailAsync(username);

                    var roles = await _userManager.GetRolesAsync(u);

                    if (roles.Contains("Administration") || roles.Contains("Employee")) return true;
                    else return false;
                }
                else { 
                    return false; 
                }

            
           
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
