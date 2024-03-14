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

namespace BirthDayPartyBooking.Pages.Admin.ServiceManagement
{
    public class IndexModel : PageModel
    {
        private readonly IServiceRepository serviceRepo;

        public IndexModel(IServiceRepository serviceRepo)
        {
            this.serviceRepo = serviceRepo;
        }

        public IList<Service> Service { get;set; }

        public async Task OnGetAsync()
        {
            string Id = HttpContext.Session.GetString("UserId");

            Service = await serviceRepo.GetAllServicesByHostID(Id);
        }
    }
}
