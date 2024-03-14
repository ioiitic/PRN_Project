using BusinessObject;
using DAO;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class OrderRepository : IOrderRepository
    {
        public void AddNew(Order order) => OrderDAO.Instance.AddNew(order);

        public bool CheckOrderExist(Order order, string Id) => OrderDAO.Instance.CheckOrderExist(order, Id);

        public Task<List<Order>> GetAllActiveorders() => OrderDAO.Instance.GetAllActiveorders();

        public Task<List<Order>> GetOrderByCustomerID(string id) => OrderDAO.Instance.GetOrderByCustomerID(id);

        public Task<List<Order>> GetOrderByHostID(string id) => OrderDAO.Instance.GetOrderByHostID(id);

        public Task<Order> GetOrderByOrderID(Guid id) => OrderDAO.Instance.GetOrderByOrderID(id);

        public Task Remove(Order order) => OrderDAO.Instance.Remove(order);
        public Task Update(Order order) => OrderDAO.Instance.Update(order);
    }
}
