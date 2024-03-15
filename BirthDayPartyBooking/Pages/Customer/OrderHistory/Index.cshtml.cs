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
        private readonly BirthdayPartyBookingContext _context;

        public IndexModel(IOrderRepository orderRepo, BirthdayPartyBookingContext context)
        {
            this.orderRepo = orderRepo;
            _context = context;
        }

        public PaginatedList<Order> Order { get;set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            string Id = HttpContext.Session.GetString("UserId");

            var pageSize = 4;

            IQueryable<Order> orders = _context.Orders.Where(o => o.GuestId.ToString() == Id);
            var orderList = await orderRepo.GetOrderByCustomerID(Id);

            var orderQuery = orderList.AsQueryable();

            Order = await PaginatedList<Order>.CreateAsync(orders.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
