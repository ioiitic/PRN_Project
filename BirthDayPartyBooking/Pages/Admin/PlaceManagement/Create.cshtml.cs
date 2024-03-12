using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Microsoft.AspNetCore.Http;

namespace BirthDayPartyBooking.Pages.Admin.PlaceManagement
{
    public class CreateModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public CreateModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["HostId"] = new SelectList(_context.Accounts, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public Place Place { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Place.HostId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Place.DeleteFlag = 0;
            _context.Places.Add(Place);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
