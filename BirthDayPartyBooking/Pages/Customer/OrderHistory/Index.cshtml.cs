using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;

namespace BirthDayPartyBooking.Pages.Customer.OrderHistory
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public IndexModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            string Id = HttpContext.Session.GetString("UserId");

            Order = await _context.Orders
                .Where(o => o.GuestId.ToString() == Id)
                .Include(o => o.Guest)
                .Include(o => o.Place).ToListAsync();
        }
    }
}
