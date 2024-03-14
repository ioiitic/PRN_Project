﻿using System;
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
    public class DetailsModel : PageModel
    {
        private readonly IServiceRepository serviceRepo;

        public DetailsModel(IServiceRepository serviceRepo)
        {
            this.serviceRepo = serviceRepo; 
        }

        public Service Service { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            string Id = HttpContext.Session.GetString("UserId");

            Service = await serviceRepo.GetServiceByServiceIDAndHostID(id.Value, Id);

            if (Service == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
