using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;

namespace BirthDayPartyBooking.Pages.Admin.PlaceManagement
{
    public class DetailsModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public DetailsModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

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
    }
}
