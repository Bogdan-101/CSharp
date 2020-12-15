using System;
using Models;

namespace DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        public string _connectionString;
        private OrdersRepository ordersRepository;
        private ErrorRepository errorsRepository;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (ordersRepository == null)
                    ordersRepository = new OrdersRepository(_connectionString);
                return ordersRepository;
            }
        }

        public IRepository<Error> Errors
        {
            get
            {
                if (errorsRepository == null)
                    errorsRepository = new ErrorRepository(_connectionString);
                return errorsRepository;
            }
        }
    }
}
