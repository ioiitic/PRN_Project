using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace BirthDayPartyBooking.Pages.Customer
{
    public class IndexModel : PageModel
    {
        private readonly BirthdayPartyBookingContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public IList<Account> Hosts { get;set; }

        public async Task OnGetAsync()
        {
            Hosts = await _context.Accounts.Where(a => a.Role == 2 && a.DeleteFlag == 0).ToListAsync();
        }
    }
}
