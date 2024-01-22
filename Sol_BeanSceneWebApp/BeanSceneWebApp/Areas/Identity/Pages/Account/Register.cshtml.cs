// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using BeanSceneWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using BeanSceneWebApp.Data;

namespace BeanSceneWebApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PersonService _personService;

        public RegisterModel( UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            PersonService personService)
         
        {
            _userManager = userManager;
            _personService = personService;
            _signInManager = signInManager;

        }

       
        [BindProperty]
        public InputModel Input { get; set; }

        public async Task OnGetAsync()
        {
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
           
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { Email=Input.Email,UserName=Input.Email};
                var result = await _userManager.CreateAsync(user, Input.Password);
               
                if (result.Succeeded)
                {
                 var person= await _personService.FindOrCreateAsync(Input.Email,Input.FirstName,Input.LastName,Input.Phone);
                    await _personService.SetUserIdAsync(person.Id, user.Id);

                    await _userManager.AddToRoleAsync(user, "Member");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var u = User;
                 return RedirectToAction("RedirectUser", "Home",new {area=""});

                  
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

    
     
    }



    public class InputModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
     
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
      
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
       
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long."/*, MinimumLength = 6*/)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
