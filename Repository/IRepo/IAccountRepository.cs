using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllActiveAccounts();
        Task<List<Account>> GetAllActiveHosts();
        Task<Account> GetAccountByAccountId(string id);
        Account CheckLogin(string Email, string Password);
        void AddNew(Account account);
    }
}
