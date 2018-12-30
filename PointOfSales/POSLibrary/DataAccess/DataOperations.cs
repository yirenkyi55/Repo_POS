using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
namespace POSLibrary.DataAccess
{
    public abstract class DataOperations<T> where T : class
    {
        /// <summary>
        /// Deletes a record from the database
        /// </summary>
        /// <param name="recordId"> The recordId you want to delete</param>
        /// <param name="procedureName"> 
        /// The procedure name you want to use in delete action
        /// </param>
        /// <param name="primaryColumn">
        /// The name of the primaryColumn you want to delete from
        /// </param>
        /// <returns> A boolean value indicating success/failure.</returns>
        public virtual async Task<bool> DeleteRecordAsync(int recordId, string procedureName,string primaryColumn="Id")
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var param = new DynamicParameters();
                param.Add("@"+primaryColumn, recordId);
                var result = await connection.ExecuteAsync(procedureName, param, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        /// <summary>
        /// Deletes all records from the database
        /// </summary>
        /// <param name="procedureName"> The procedure name you want to use</param>
       
        public virtual async Task<bool> DeleteAllRecordsAsync(string procedureName)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var result = await connection.ExecuteAsync(procedureName, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        /// <summary>
        /// Gets all records from the database
        /// </summary>
        /// <param name="procedureName">The procedure name you want to use</param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAllRecords(string procedureName)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var result = connection.Query<T>(procedureName, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

        public virtual IEnumerable<T> SearchForRecord(IEnumerable<T> allRecords, string itemToSearch, Func<T,bool> searchPattern1, Func<T,bool> searchPattern2)
        {
            var results = allRecords.Where(searchPattern1);
            if (results.Count()>0)
            {
                return results;
            }
            else
            {
                //Search for a new pattern
                results = allRecords.Where(searchPattern2);
                return results;
            }
        }
    }
}
