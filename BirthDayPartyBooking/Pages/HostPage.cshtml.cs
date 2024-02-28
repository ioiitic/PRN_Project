using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace BirthDayPartyBooking.Pages
{
    public class HostPageModel : PageModel
    {
        private readonly BirthdayPartyBookingContext _context;
        public List<Service> HostPage { get; set; } = new List<Service>();

        public HostPageModel(BirthdayPartyBookingContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            HostPage = _context.Services.ToList();
        }
    }
}
