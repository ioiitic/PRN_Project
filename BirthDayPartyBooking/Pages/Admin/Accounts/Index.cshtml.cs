using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // Add this to use HttpContext.Session
using BusinessObject;

namespace BirthDayPartyBooking.Pages.Admin.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public IndexModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public IList<Account> Account { get; set; }

        public async Task<IActionResult> OnGetAsync() // Change the return type to IActionResult
        {
            // Get the role from the session
            var role = HttpContext.Session.GetInt32("Role");

            // Check the role
            if (role == null || role != 0) 
            {
                // If the role is not set in the session, or it's not admin, return an error
                return Unauthorized();
            }

            Account = await _context.Accounts.Where(s => s.Role !=0).ToListAsync();

            return Page();
        }
    }
}
