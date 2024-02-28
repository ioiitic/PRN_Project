using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace BirthDayPartyBooking.Pages
{
    public class HistoryBookingModel : PageModel
    {
        private readonly BirthdayPartyBookingContext _context;
        public List<OrderDetail> HistoryBooking { get; set; } = new List<OrderDetail>();
        public HistoryBookingModel(BirthdayPartyBookingContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            HistoryBooking = _context.OrderDetails.ToList();
        }
        
    }
}
