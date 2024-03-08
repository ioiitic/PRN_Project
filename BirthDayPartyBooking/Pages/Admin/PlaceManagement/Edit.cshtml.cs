using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;

namespace BirthDayPartyBooking.Pages.Admin.PlaceManagement
{
    public class EditModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public EditModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Place Place { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            string Id = HttpContext.Session.GetString("UserId");

            Place = await _context.Places.Where(p => p.HostId.ToString() == Id)
                .Include(p => p.Host).FirstOrDefaultAsync(m => m.Id == id);

            if (Place == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Place).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(Place.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PlaceExists(Guid id)
        {
            return _context.Places.Any(e => e.Id == id);
        }
    }
}
