using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Repository.IRepo;
using BusinessObject;

namespace BirthDayPartyBooking.Pages.Administrator
{
    public class ReportStatisticModel : PageModel
    {
        public readonly IOrderRepository orderRepo;

        public ReportStatisticModel(IOrderRepository repo)
        {
            orderRepo = repo;
        }

        [BindProperty]
        public List<Order> orders { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public async Task OnGetAsync()
        {
            orders = await orderRepo.GetOrderForReport();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                orders = await orderRepo.GetOrderByDate(StartDate, EndDate);
            }

            return Page();
        }
    }
}
