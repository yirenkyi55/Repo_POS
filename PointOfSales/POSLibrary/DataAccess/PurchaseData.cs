using POSLibrary.Abstract;
using POSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace POSLibrary.DataAccess
{
    public class PurchaseData : IPurchaseData
    {
        /// <summary>
        /// Inserts a new purchase model into the sql database
        /// </summary>
        /// <param name="purchase">
        /// The purchase model to be saved into the database
        /// </param>
        /// <returns>
        /// The purchase model that has been saved
        /// </returns>
        public async Task<PurchaseModel> CreatePurchase(PurchaseModel purchase)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {

                var param = new DynamicParameters();
                param.Add("@ProductId",purchase.Product.Id);
                param.Add("@DatePurchased", purchase.DatePurchased);
                param.Add("@Quantity", purchase.Quantity);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync("spPurchase_Insert", param, commandType: CommandType.StoredProcedure);
                purchase.PurchaseId = param.Get<int>("@id");
                return purchase;
            }
        }
    }
}
