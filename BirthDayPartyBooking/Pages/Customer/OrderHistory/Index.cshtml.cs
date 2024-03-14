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

namespace BirthDayPartyBooking.Pages.Customer.OrderHistory
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository orderRepo;

        public IndexModel(IOrderRepository orderRepo)
        {
            this.orderRepo = orderRepo;
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            string Id = HttpContext.Session.GetString("UserId");

            Order = await orderRepo.GetOrderByCustomerID(Id);
        }
    }
}
