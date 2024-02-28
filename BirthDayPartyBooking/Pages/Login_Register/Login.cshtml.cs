using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using BusinessObject;

namespace BirthDayPartyBooking.Pages.Login_Register
{
    public class LoginPageModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly BirthdayPartyBookingContext _context;

        public LoginPageModel(IConfiguration configuration, BirthdayPartyBookingContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnPostLogin()
        {

            var account = _context.Accounts.Where(s => s.Email == Email && s.Password == Password && s.DeleteFlag ==0).FirstOrDefault();

            if (account != null)
            {
                HttpContext.Session.SetInt32("Role", account.Role.Value);
                if (account.Role.Value == 0)
                {
                    return RedirectToPage("/Admin/Accounts/Index");
                }

            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        public IActionResult OnPostRegister()
        {
            // Handle registration
            return RedirectToPage("/Login_Register/Register");
        }
    }
}
