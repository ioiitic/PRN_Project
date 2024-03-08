using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using BusinessObject.Enum;

namespace BirthDayPartyBooking.Pages.Admin.OrderManagement
{
    public class EditModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public EditModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

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
            var statusDictionary = OrderStatus.StatusNames
                .Select((name, index) => new { Key = index, Value = name })
                .ToDictionary(item => item.Key, item => item.Value);
            ViewData["GuestId"] = new SelectList(_context.Accounts, "Id", "Email");
            ViewData["Status"] = new SelectList(statusDictionary, "Key", "Value");
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

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.Id))
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

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
