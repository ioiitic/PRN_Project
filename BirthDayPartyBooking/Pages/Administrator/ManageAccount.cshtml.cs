using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Repository.IRepo;

namespace BirthDayPartyBooking.Pages.Administrator
{
    public class ManageAccountModel : PageModel
    {
        private readonly IAccountRepository _accountRepo;

        public ManageAccountModel(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo; 
        }

        public IList<Account> Account { get;set; }

        public async Task OnGetAsync()
        {
            Account = await _accountRepo.GetAllAccounts();
        }
        public async Task<IActionResult> OnPostAsync(Guid itemId)
        {
            var item = _accountRepo.GetAccountByAccountId(itemId.ToString());
            if (item == null)
            {
                return NotFound();
            }

            item.DeleteFlag = item.DeleteFlag == 1 ? 0 : 1;
            try
            {
                await _accountRepo.Update(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            Account = await _accountRepo.GetAllAccounts();
            return Page();
        }
    }
}
