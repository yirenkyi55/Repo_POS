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
    public class SupplierData : ISupplierData
    {
        /// <summary>
        /// Saves a supplier record into a database asynchronously
        /// </summary>
        /// <param name="supplier">
        /// The supplier to be saved into the database
        /// </param>
        /// <returns>
        /// The supplier of the newly inserted record
        /// </returns>
        public async Task<SupplierModel> CreateSupplierAsync(SupplierModel supplier)
        {

            //Create a connection to database
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
                {
                    var param = new DynamicParameters();
                    param.Add("@Name", supplier.Name);
                    param.Add("@Address", supplier.Address);
                    param.Add("@Email", supplier.Email);
                    param.Add("@CellPhoneNumber", supplier.CellPhoneNumber);
                    param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("spSuppliers_Insert", param, commandType: CommandType.StoredProcedure);
                    supplier.SupplierId= param.Get<int>("@id");

                //Insert supplier_products records
                foreach (ProductModel product in supplier.Products)
                {
                     param = new DynamicParameters();
                    param.Add("@SupplierId", supplier.SupplierId);
                    param.Add("@ProductId", product.Id);                 
                    param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await connection.ExecuteAsync("spSupplierProducts_Insert", param, commandType: CommandType.StoredProcedure);
                  
                }
                    return supplier;
                }
            
        }
    }
}
