using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace BirthDayPartyBooking.Pages.Customer
{
    public class BookingModel : PageModel
    {
        private readonly BirthdayPartyBookingContext _context;

        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public Account Customer { get; set; }
        [BindProperty]
        public Account Host { get; set; }
        [BindProperty]
        public IList<OrderDetail> OrderDetails { get; set; }
        [BindProperty]
        public OrderDetail OrderDetail { get; set; }
        [BindProperty]
        public int Number { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public BookingModel(BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Order = new Order();
            if (HttpContext.Session.GetString("OrderModelState") != null)
            {
                ModelState.AddModelError("Order", "Invalid!"); 
                HttpContext.Session.Remove("OrderModelState");
            }
            if (Id != null)
                HttpContext.Session.SetString("HostId", Id);
            else 
                Id = HttpContext.Session.GetString("HostId");

            string customerId = HttpContext.Session.GetString("UserId");

            Customer = _context.Accounts.FirstOrDefault(c => c.Id.ToString() == customerId);
            Host = _context.Accounts.FirstOrDefault(c => c.Id.ToString() == Id);
            ViewData["ServiceId"] = new SelectList(_context.Services.Where(p => p.HostId == Host.Id).OrderBy(s => s.ServiceTypeId).Select(p => new
            {
                p.Id,
                NameAndPrice = p.ServiceType.Name + " - " + p.Name + " - $" + p.Price.ToString()
            }), "Id", "NameAndPrice");
            ViewData["GuestId"] = new SelectList(_context.Accounts, "Id", "Name");
            ViewData["PlaceId"] = new SelectList(_context.Places.Where(p => p.HostId == Host.Id).Select(p => new
            {   
                p.Id,
                NameAndAddress = p.Name + ", " + p.Address + " - $" + p.Price
            }), "Id", "NameAndAddress");

            var orderDetailsJson = HttpContext.Session.GetString("OrderDetails");
            OrderDetails = (orderDetailsJson != null) ? JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailsJson)
                : new List<OrderDetail>();

            var totalPrice = (Order.Place==null)?0:Order.Place.Price;

            foreach(OrderDetail orderDetail in OrderDetails)
            {
                orderDetail.Service = _context.Services.FirstOrDefault(s => s.Id == orderDetail.ServiceId);
                var typeId = orderDetail.Service.ServiceTypeId;
                orderDetail.Service.ServiceType = _context.ServiceTypes.FirstOrDefault(t => t.Id == typeId);
                totalPrice += orderDetail.Price.Value;
            }

            OrderDetails = OrderDetails.OrderBy(o => o.Service.ServiceTypeId).ToList();

            Order.TotalPrice = totalPrice;
            return Page();
        }

        public IActionResult OnPostFormOrder()
        {
            if (Id != null)
                HttpContext.Session.SetString("HostId", Id);
            else
                Id = HttpContext.Session.GetString("HostId");

            var check = _context.Orders.Any(o => o.Date == Order.Date && o.HostId.ToString() == Id && o.PlaceId == Order.PlaceId);
            if (check)
            {
                HttpContext.Session.SetString("OrderModelState", "Invalid");
                return RedirectToPage();
            }

            Customer = _context.Accounts.FirstOrDefault(c => c.Role == 1);
            Order.GuestId = Customer.Id;
            Order.OrderDate = DateTime.Now;
            Order.HostId = Guid.Parse(Id);
            Order.Status = 0;
            Order.DeleteFlag = 0;
            _context.Orders.Add(Order);
            _context.SaveChanges();
            var orderDetailsJson = HttpContext.Session.GetString("OrderDetails");
            OrderDetails = (orderDetailsJson != null) ? JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailsJson)
                : new List<OrderDetail>();
            foreach (OrderDetail orderDetail in OrderDetails)
            {
                orderDetail.OrderId = Order.Id;
                orderDetail.Service = null;
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
            }
            HttpContext.Session.Remove("OrderDetails");
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostFormDetail()
        {
            OrderDetail.Service = _context.Services.FirstOrDefault(s => s.Id == OrderDetail.ServiceId);
            var orderDetailsJson = HttpContext.Session.GetString("OrderDetails");
            OrderDetail.Number = Number;
            OrderDetail.Price = OrderDetail.Service.Price * Number;
            OrderDetails = (orderDetailsJson != null) ? JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailsJson)
                : new List<OrderDetail>();

            var orderDetailToUpdate = OrderDetails.FirstOrDefault(od => od.Service.Id == OrderDetail.Service.Id);

            if (orderDetailToUpdate != null)
            {
                orderDetailToUpdate.Number += OrderDetail.Number;
                orderDetailToUpdate.Price = OrderDetail.Service.Price * orderDetailToUpdate.Number;
            }
            else
                OrderDetails.Add(OrderDetail);

            HttpContext.Session.SetString("OrderDetails", JsonConvert.SerializeObject(OrderDetails));
            return RedirectToPage();
        }

        public IActionResult OnPostClearDetail()
        {
            HttpContext.Session.Remove("OrderDetails");
            return RedirectToPage("/Customer/Booking");
        }

        public IActionResult OnPostBack()
        {
            HttpContext.Session.Remove("OrderDetails");
            return RedirectToPage("./Index");
        }
    }
}
