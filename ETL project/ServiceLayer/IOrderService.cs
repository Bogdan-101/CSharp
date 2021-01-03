using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace ServiceLayer
{
    interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrders();
    }
}
