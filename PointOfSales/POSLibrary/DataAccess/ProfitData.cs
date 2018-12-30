using Dapper;
using POSLibrary.Abstract;
using POSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLibrary.DataAccess
{
    public class ProfitData : IProfitData
    {

        /// <summary>
        /// Saves profit record into the sql database
        /// </summary>
        /// <param name="profit">
        /// The profit model to be saved into the database
        /// </param>
        /// <returns>
        /// The newly created profit model..
        /// </returns>
        public async Task<ProfitModel> CreateProfitAsync(ProfitModel profit)
        {
            //Create a connection to database
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var param = new DynamicParameters();
                param.Add("@ProfitName", profit.ProfitName);
                param.Add("@ProfitAmount", profit.ProfitAmount);
                param.Add("@ProfitDate", profit.ProfitDate);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("spProfit_Insert", param, commandType: CommandType.StoredProcedure);
                profit.ProfitId = param.Get<int>("@id");
                return profit;
            }
        }
    }
}
