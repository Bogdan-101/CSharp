using System;
using System.Collections.Generic;
using Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    class ErrorRepository : IRepository<Error>
    {
        private string _connectionString;
        public ErrorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Create(Error item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("sp_LogError", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlParameter excepParam = new SqlParameter
                {
                    ParameterName = "@exception",
                    Value = item.Exception
                };
                command.Parameters.Add(excepParam);

                SqlParameter messParam = new SqlParameter
                {
                    ParameterName = "@message",
                    Value = item.Message
                };
                command.Parameters.Add(messParam);

                SqlParameter dateParam = new SqlParameter
                {
                    ParameterName = "@time",
                    Value = item.Time
                };
                command.Parameters.Add(dateParam);

                var result = command.ExecuteScalar();
            }
        }


        public void Delete(DateTime birthDay)
        {
            throw new NotImplementedException();
        }

        public Task<Error> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Error>> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Error item)
        {
            throw new NotImplementedException();
        }
    }
}
