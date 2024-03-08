using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BirthDayPartyBooking.Pages.Login_Register
{
    public class RegisterModel : PageModel
    {
        private readonly BusinessObject.BirthdayPartyBookingContext _context;

        public RegisterModel(BusinessObject.BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Accounts.Add(Account);
            _context.SaveChanges();

            return RedirectToPage("./Login");
        }
    }
}
