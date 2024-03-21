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

namespace BirthDayPartyBooking.Pages.Admin.PlaceManagement
{
    [Authorize(Roles = "Host")]
    public class IndexModel : PageModel
    {
        private readonly IPlaceRepository _placeRepo;
        private readonly IConfiguration _configuration;

        public IndexModel(IPlaceRepository placeRepo, IConfiguration configuration)
        {
            _placeRepo = placeRepo;
            _configuration = configuration;
        }

        public PaginatedList<Place> Place { get; set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            string Id = HttpContext.Session.GetString("UserId");

            var pageSize = _configuration.GetValue("PageSize", 4);

            IQueryable<Place> places = _placeRepo.GetAllPlace(Id);

            Place = await PaginatedList<Place>.CreateAsync(places.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
