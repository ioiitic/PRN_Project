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
    public class IndexModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public IndexModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public IList<Place> Place { get;set; }

        public async Task OnGetAsync()
        {
            string Id = HttpContext.Session.GetString("UserId");

            Place = await _context.Places.Where(p => p.HostId.ToString() == Id && p.DeleteFlag == 0)
                .Include(p => p.Host).ToListAsync();
        }
    }
}
