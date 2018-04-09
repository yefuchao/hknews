using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace API.Application.Queries
{
    public class StockQueries : IStockQueries
    {
        private string _connectionString = string.Empty;

        public StockQueries(string constr)
        {
            _connectionString = constr;
        }

        public async Task<IEnumerable<StockItem>> GetDayStockAsync(DateTime date)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "select Id,Code,Stock_Name as Name ,Num as Amount,Rate from Records where Date = @date order by Code";

                var queryDate = ConvertDateString(date);

                var result = await connection.QueryAsync<StockItem>(sql, new { date = queryDate });

                return result;
            }
        }

        public async Task<IEnumerable<StockNameAndRate>> GetStockRateAsync(string code)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "select Date ,Rate from Records where Code = @code order by Id";

                return await connection.QueryAsync<StockNameAndRate>(sql, new { code });
            }
        }

        public async Task<IEnumerable<StockNameAndAmount>> GetStockAmountAsync(string code)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = "select Date ,Num as Amount from Records where Code = @code order by Id";

                var result = await connection.QueryAsync<StockNameAndAmount>(sql, new { code });

                return result;
            }
        }

        private string ConvertDateString(DateTime date)
        {
            var year = date.Year.ToString();
            var month = date.Month > 9 ? date.Month.ToString() : "0" + date.Month.ToString();
            var day = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
            return day + "/" + month + "/" + year;
        }

    }
}
