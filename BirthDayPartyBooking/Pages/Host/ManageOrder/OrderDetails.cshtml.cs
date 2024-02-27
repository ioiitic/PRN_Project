using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;

namespace BirthDayPartyBooking.Pages.Host.ManageOrder
{
    public class OrderDetailsModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public OrderDetailsModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public IList<OrderDetail> OrderDetail { get;set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Service).Where(m => m.OrderId == id).ToListAsync();

            if (OrderDetail == null)
            {
                return NotFound();
            }
            return Page();

        }
    }
}
