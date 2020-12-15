using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace DataAccessLayer
{
    public interface IUnitOfWork
    {
        IRepository<Order> Orders { get; }
        IRepository<Error> Errors { get; }
    }
}
