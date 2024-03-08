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

namespace BirthDayPartyBooking.Pages.Admin.ServiceManagement
{
    public class EditModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public EditModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Service Service { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            string Id = HttpContext.Session.GetString("UserId");

            Service = await _context.Services.Where(s => s.HostId.ToString() == Id && s.DeleteFlag == 0)
                .Include(s => s.Host)
                .Include(s => s.ServiceType).FirstOrDefaultAsync(m => m.Id == id);

            if (Service == null)
            {
                return NotFound();
            }
           ViewData["ServiceTypeId"] = new SelectList(_context.ServiceTypes, "Id", "Name");
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

            _context.Attach(Service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(Service.Id))
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

        private bool ServiceExists(Guid id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
