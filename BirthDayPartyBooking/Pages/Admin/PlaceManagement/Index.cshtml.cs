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

namespace BirthDayPartyBooking.Pages.Admin.PlaceManagement
{
    public class IndexModel : PageModel
    {
        private readonly IPlaceRepository placeRepo;

        public IndexModel(IPlaceRepository placeRepo)
        {
            this.placeRepo = placeRepo;
        }

        public IList<Place> Place { get;set; }

        public async Task OnGetAsync()
        {
            string Id = HttpContext.Session.GetString("UserId");

            Place = await placeRepo.GetAllPlaceByHostID(Id);
        }
    }
}
