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
    public class CreateModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public Account Customer { get; set; }
        [BindProperty]
        public Account Host { get; set; }
        [BindProperty]
        public IList<Service> Services { get; set; }
        [BindProperty]
        public Service Service { get; set; }

        public CreateModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["GuestId"] = new SelectList(_context.Accounts, "Id", "Name");
            ViewData["PlaceId"] = new SelectList(_context.Places.Select(p => new {
                Id = p.Id,
                NameAndDescription = p.Name + ", " + p.Address
            }), "Id", "NameAndDescription");

            Customer = _context.Accounts.FirstOrDefault(c => c.Role == 1);
            Host = _context.Accounts.FirstOrDefault(c => c.Role == 2);

            var servicesJson = HttpContext.Session.GetString("OrderDetails");
            Services = (servicesJson != null) ? JsonConvert.DeserializeObject<List<Service>>(servicesJson)
                : new List<Service>();

            var totalPrice = 0;

            foreach(Service service in Services)
            {
                var typeId = service.ServiceTypeId;
                service.ServiceType = _context.ServiceTypes.FirstOrDefault(t => t.Id == typeId);
                totalPrice += service.Price.Value;
            }
            Order = new Order();
            Order.TotalPrice = totalPrice;
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostFormOrder()
        {
            Customer = _context.Accounts.FirstOrDefault(c => c.Role == 1);
            Order.GuestId = Customer.Id;
            Order.Date = DateTime.Now;
            Order.DeleteFlag = 0;
            _context.Orders.Add(Order);
            _context.SaveChanges();
            var servicesJson = HttpContext.Session.GetString("OrderDetails");
            Services = (servicesJson != null) ? JsonConvert.DeserializeObject<List<Service>>(servicesJson)
                : new List<Service>();
            foreach (Service service in Services)
            {
                OrderDetail orderDetail = new OrderDetail();
                var date = _context.Orders.Max(o => o.Date);
                Order order = _context.Orders.FirstOrDefault(o => o.Date == date);
                orderDetail.OrderId = Order.Id;
                orderDetail.Price = service.Price;
                orderDetail.ServiceId = service.Id;
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
            }
            HttpContext.Session.Remove("BookingDetails");
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostFormDetail()
        {
            Service = _context.Services.FirstOrDefault(s => s.Id == Service.Id);

            var servicesJson = HttpContext.Session.GetString("OrderDetails");
            Services = (servicesJson != null) ? JsonConvert.DeserializeObject<List<Service>>(servicesJson)
                : new List<Service>();
            Services.Add(Service);

            HttpContext.Session.SetString("OrderDetails", JsonConvert.SerializeObject(Services));
            return RedirectToPage();
        }

        public IActionResult OnPostClearDetail()
        {
            HttpContext.Session.Remove("OrderDetails");
            return RedirectToPage();
        }
    }
}
