using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Microsoft.AspNetCore.Http;

namespace BirthDayPartyBooking.Pages.Admin.ServiceManagement
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
        ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Service Service { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Service.HostId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Service.DeleteFlag = 0;
            _context.Services.Add(Service);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
