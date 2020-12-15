using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
using Models;

namespace ServiceLayer
{
    public class ErrorService
    {
        private IUnitOfWork Database { get; set; }

        public ErrorService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;  
        }

        public void AddErrors(Error error)
        {
            Database.Errors.Create(error);
        }
    }
}
