using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;

namespace BirthDayPartyBooking.Pages.Admin
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
            return Page();
        }

        [BindProperty]
        public ServiceType ServiceType { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ServiceTypes.Add(ServiceType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
