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
        private readonly BirthdayPartyBookingContext _context;

        public EditModel(BirthdayPartyBookingContext context)
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
            var statusDictionary = OrderStatus.StatusNames
                .Select((name, index) => new { Key = index, Value = name })
                .ToDictionary(item => item.Key, item => item.Value);
            ViewData["GuestId"] = new SelectList(_context.Accounts, "Id", "Email");
            ViewData["Status"] = new SelectList(statusDictionary, "Key", "Value");
            return Page();
        }
            
        public async Task<IActionResult> OnPostAccept()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order = await _context.Orders
                .Include(o => o.Guest)
                .Include(o => o.Place).FirstOrDefaultAsync(m => m.Id == Order.Id);
            Order.Status = 1;

            try
            {
                _context.Orders.Update(Order);
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

            return RedirectToPage(new { id = Order.Id });
        }
        public async Task<IActionResult> OnPostReject()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order = await _context.Orders
                .Include(o => o.Guest)
                .Include(o => o.Place).FirstOrDefaultAsync(m => m.Id == Order.Id);
            Order.Status = 2;

            try
            {
                _context.Orders.Update(Order);
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

            return RedirectToPage(new { id = Order.Id });
        }
        public async Task<IActionResult> OnPostClose()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order = await _context.Orders
                .Include(o => o.Guest)
                .Include(o => o.Place).FirstOrDefaultAsync(m => m.Id == Order.Id);
            Order.Status = 3;

            try
            {
                _context.Orders.Update(Order);
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

            return RedirectToPage(new { id = Order.Id });
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
