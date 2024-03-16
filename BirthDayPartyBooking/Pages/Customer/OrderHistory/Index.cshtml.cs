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
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BirthDayPartyBooking.Pages.Customer.OrderHistory
{
    [Authorize(Roles = "Customer")]
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepo;
        private readonly BirthdayPartyBookingContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(IOrderRepository orderRepo, BirthdayPartyBookingContext context, IConfiguration configuration)
        {
            _orderRepo = orderRepo;
            _context = context;
            _configuration = configuration;
        }

        public PaginatedList<Order> Order { get;set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            string Id = HttpContext.Session.GetString("UserId");

            var pageSize = _configuration.GetValue("PageSize", 4);

            IQueryable<Order> orders = _context.Orders.Where(o => o.GuestId.ToString() == Id);

            Order = await PaginatedList<Order>.CreateAsync(orders.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
