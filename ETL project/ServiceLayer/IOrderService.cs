using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace ServiceLayer
{
    interface IOrderService
    {
        Order GetInfo(int? id);
    }
}
