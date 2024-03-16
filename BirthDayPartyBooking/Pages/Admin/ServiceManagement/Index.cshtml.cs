using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using Repository.IRepo;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BirthDayPartyBooking.Pages.Admin.ServiceManagement
{
    [Authorize(Roles = "Host")]
    public class IndexModel : PageModel
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly BirthdayPartyBookingContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(IServiceRepository serviceRepo, BirthdayPartyBookingContext context, IConfiguration configuration)
        {
            _serviceRepo = serviceRepo;
            _context = context;
            _configuration = configuration;
        }

        public PaginatedList<Service> Service { get; set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            string Id = HttpContext.Session.GetString("UserId");

            var pageSize = _configuration.GetValue("PageSize", 4);

            IQueryable<Service> serivces = _context.Services.Where(o => o.HostId.ToString() == Id);

            Service = await PaginatedList<Service>.CreateAsync(serivces.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
