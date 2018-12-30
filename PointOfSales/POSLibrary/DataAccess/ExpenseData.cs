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
 
    public class ExpenseData : IExpenseData
    {
        /// <summary>
        /// Save expense record into a database asynchronously
        /// </summary>
        /// <param name="expense">
        /// The expense to save into the database
        /// </param>
        /// <returns>
        /// The expense of the newly inserted record
        /// </returns>
        public async Task<ExpenseModel> CreateExpenseAsync(ExpenseModel expense)
        {
            //Create a connection to database
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var param = new DynamicParameters();
                param.Add("@ExpenseName", expense.ExpenseName);
                param.Add("@ExpenseAmount", expense.ExpenseAmount);
                param.Add("@ExpenseDate", expense.ExpenseDate);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("spExpense_Insert", param, commandType: CommandType.StoredProcedure);
                expense.ExpenseId = param.Get<int>("@id");
                return expense;
            }
        }
    }
}
