using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;

namespace BirthDayPartyBooking.Pages.Admin.ServiceManagement
{
    public class DeleteModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public DeleteModel(BusinessObject.BirthdayPartyBookingContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service = await _context.Services.FindAsync(id);

            if (Service != null)
            {
                Service.DeleteFlag = 1;
                _context.Services.Update(Service);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
