using System;
using System.Collections.Generic;
using DataAccessLayer;
using Models;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class OrderService : IOrderService
    {
        public IUnitOfWork Database { get; set; }

        public OrderService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetListOfOrders()
        {
            return await Database.Orders.GetAll();
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await Task.Run(async () =>
            {
                var orders = await GetListOfOrders();
                return orders;
            });
        }
    }
}
