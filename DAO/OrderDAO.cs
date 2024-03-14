using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class OrderDAO
    {
        BirthdayPartyBookingContext myDB = new BirthdayPartyBookingContext();
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task<List<Order>> GetAllActiveorders()
        {
            List<Order> orders;
            try
            {
                orders = await myDB.Orders.AsNoTracking().Where(s => s.DeleteFlag == 0).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public async Task<List<Order>> GetOrderByHostID(string id)
        {
            List<Order> orders;
            try
            {
                orders = await myDB.Orders.AsNoTracking().Where(o => o.HostId.ToString() == id)
                                          .Include(o => o.Guest)
                                          .Include(o => o.Place).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public async Task<List<Order>> GetOrderByCustomerID(string id)
        {
            List<Order> orders;
            try
            {
                orders = await myDB.Orders.AsNoTracking().Where(o => o.GuestId.ToString() == id)
                                          .Include(o => o.Guest)
                                          .Include(o => o.Place).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public async Task<Order> GetOrderByOrderID(Guid id)
        {
            Order orders;
            try
            {
                orders = await myDB.Orders.Include(o => o.Guest)
                                          .Include(o => o.Place).AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public bool CheckOrderExist(Order order, string Id)
        {
            bool check = false;
            try
            {
                check = myDB.Orders.AsNoTracking().Any(o => o.Date == order.Date && o.HostId.ToString() == Id && o.PlaceId == order.PlaceId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return check;
        }

        public void AddNew(Order order)
        {
            try
            {
                myDB.Orders.Add(order);
                myDB.SaveChanges();
                myDB.Entry(order).State = EntityState.Detached;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task Update(Order order)
        {
            try
            {
                Task<Order> _order = GetOrderByOrderID(order.Id);
                if (_order != null)
                {
                    myDB.Entry<Order>(order).State = EntityState.Modified;
                    await myDB.SaveChangesAsync();
                    myDB.Entry(order).State = EntityState.Detached;
                }
                else
                {
                    throw new Exception("The order not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task Remove(Order order)
        {
            try
            {
                Task<Order> _order = GetOrderByOrderID(order.Id);
                if (_order != null)
                {
                    order.DeleteFlag = 1;
                    myDB.Entry<Order>(order).State = EntityState.Modified;
                    await myDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The order not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
