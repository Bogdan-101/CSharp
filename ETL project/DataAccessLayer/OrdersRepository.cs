using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace DataAccessLayer
{
    internal class OrdersRepository : IRepository<Order>
    {
        private readonly string _connectionString;
        public OrdersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Order item)
        {
            throw new NotImplementedException();
        }

        public void Update(Order item)
        {
            throw new NotImplementedException();
        }

        public void Delete(DateTime birthDay)
        {
            throw new NotImplementedException();
        }

        public Order Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                try
                {
                    var command = new SqlCommand("sp_GetNOrders", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    var reader = command.ExecuteReader();
                    var order = new Order();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            order.OrderId = reader.GetInt32(0);
                            order.customer = reader.GetString(1);
                            order.shipName = reader.GetString(8);
                            order.shipAddress = reader.GetString(9);
                            order.shipCity = reader.GetString(10);
                            order.shipCountry = reader.GetString(13);
                            order.shippedDate = reader.GetDateTime(5);
                        }
                    }
                    reader.Close();

                    return order;
                }
                catch (Exception trouble)
                {
                    throw trouble;
                }
            }
        }


        public IEnumerable<Order> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var orders = new List<Order>();

                try
                {
                    var command = new SqlCommand("sp_GetNOrders", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    SqlParameter countParam = new SqlParameter
                    {
                        ParameterName = "@count",
                        Value = 10
                    };
                    command.Parameters.Add(countParam);

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var order = new Order();
                            order.OrderId = reader.GetInt32(0);
                            order.customer = reader.GetString(1);
                            order.shipName = reader.GetString(8);
                            order.shipAddress = reader.GetString(9);
                            order.shipCity = reader.GetString(10);
                            order.shipCountry = reader.GetString(13);
                            order.shippedDate = reader.GetDateTime(5);

                            orders.Add(order);
                        }
                    }
                    reader.Close();

                    return orders;
                }
                catch (Exception trouble)
                {
                    throw trouble;
                }
            }
        }
    }
}
