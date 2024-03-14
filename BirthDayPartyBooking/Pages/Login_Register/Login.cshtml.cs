using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using BusinessObject;
using System.ComponentModel.DataAnnotations;
using Repository.IRepo;

namespace BirthDayPartyBooking.Pages.Login_Register
{
    public class LoginPageModel : PageModel
    {
        private readonly IAccountRepository accountRepo;

        public LoginPageModel(IAccountRepository accountRepo)
        {
            this.accountRepo = accountRepo;
        }

        [BindProperty]
        [Required]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }


        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostLogin()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var account = accountRepo.CheckLogin(Email, Password);

            if (account != null)
            {
                HttpContext.Session.SetInt32("Role", account.Role.Value);
                HttpContext.Session.SetString("UserId", account.Id.ToString());
                if (account.Role.Value == 0)
                {
                    return RedirectToPage("/Admin/Accounts/Index");
                } else
                if (account.Role.Value == 1)
                {
                    return RedirectToPage("/Customer/Index");
                } else
                {
                    return RedirectToPage("/Admin/ServiceManagement/Index");
                }
            } else
            {
                ModelState.AddModelError("Password", "Invalid login attempt.");
                return Page();
            }

        }

    }
}
