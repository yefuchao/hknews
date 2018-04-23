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
    public class StockQueries : BaseQueries, IStockQueries
    {
        private string _connectionString = string.Empty;

        public StockQueries(string constr) : base(constr)
        {
            _connectionString = constr;
        }

        public async Task<IEnumerable<StockItem>> GetDayStockAsync(DateTime date)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var sql = "select Id,Code,Stock_Name as Name ,Num as Amount,Rate from Records where Date = @date order by Code";

                //var queryDate = ConvertDateString(date);

                var result = await connection.QueryAsync<StockItem>(sql, new { date });

                return result;
            }
        }

        public async Task<IEnumerable<StockNameAndRate>> GetStockRateAsync(string code)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var sql = "select Date ,Rate from Records where Code = @code order by Date";

                return await connection.QueryAsync<StockNameAndRate>(sql, new { code });
            }
        }

        public async Task<IEnumerable<StockNameAndAmount>> GetStockAmountAsync(string code)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var sql = "select Date,Num as Amount from Records where Code = @code order by Date";

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

        public async Task<string> GetStockName(string code)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var sql = "select distinct Stock_Name from Records where Code = @code ";

                var result = await connection.QuerySingleAsync<string>(sql, new { code });

                return result;
            }
        }

        public async Task<IEnumerable<StockNameRateChart>> GetStockNameAmount(DateTime date)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var sql = "select Stock_Name as Name ,Rate as Value from Records where Date = @date order by Code";

                var result = await connection.QueryAsync<StockNameRateChart>(sql, new { date });

                return result;
            }
        }
    }
}
