using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.Queries
{
    public class BaseQueries
    {
        private readonly string _conn;

        public BaseQueries(string conn)
        {
            _conn = conn;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_conn);
        }


    }
}
