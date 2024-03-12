using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;

namespace BirthDayPartyBooking.Pages.Customer.OrderHistory
{
    public class DetailsModel : PageModel
    {
        private readonly BirthdayPartyBookingContext _context;

        public DetailsModel(BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.Guest)
                .Include(o => o.Place).FirstOrDefaultAsync(m => m.Id == id);
            OrderDetails = _context.OrderDetails
                .Where(o => o.OrderId == Order.Id)
                .Include(o => o.Service)
                .Include(o => o.Service.ServiceType).ToList();

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostPay()
        {
            return RedirectToPage("/Customer/Payment/PayBooking", new {id=Order.Id});
        }

        public async Task<IActionResult> OnPostCancel ()
        {
            Order = _context.Orders.FirstOrDefault(o => o.Id == Order.Id);
            Order.Status = 5;
            _context.Orders.Update(Order);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

    }
}
