using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllActiveorders();
        Task<List<Order>> GetOrderByHostID(string id);
        Task<List<Order>> GetOrderByCustomerID(string id);

        Task<Order> GetOrderByOrderID(Guid id);
        bool CheckOrderExist(Order order, string Id);
        void AddNew(Order order);
        Task Update(Order order);
        Task Remove(Order order);
        
        

    }
}
