using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Repository.IRepo;

namespace BirthDayPartyBooking.Pages.Admin.OrderManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderRepository orderRepo;
        private readonly IOrderDetailRepository orderDetailRepo;

        public DeleteModel(IOrderRepository orderRepo, IOrderDetailRepository orderDetailRepo)
        {
            this.orderRepo = orderRepo;
            this.orderDetailRepo = orderDetailRepo; 
        }

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await orderRepo.GetOrderByOrderID(id.Value);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await orderRepo.GetOrderByOrderID(id.Value);

            if (Order != null)
            {
                await orderRepo.Remove(Order);
            }

            return RedirectToPage("./Index");
        }
    }
}
