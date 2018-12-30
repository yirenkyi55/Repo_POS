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
    public class PositionData : IPositionData
    {
        /// <summary>
        /// Insert a new position into the sql database
        /// </summary>
        /// <param name="position">
        /// The position model to be inserted
        /// </param>
        /// <returns>
        /// The newly inserted model with id
        /// </returns>
        public async Task<PositionModel> CreatePositionAsync(PositionModel position)
        {
            
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                //Create a dynamic parameter
                var param = new DynamicParameters();
                param.Add("@title",position.Title);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                //Execute the command
              await  connection.ExecuteAsync("spPosition_Insert", param, commandType: CommandType.StoredProcedure);

                //Get the model
                position.PositionId = param.Get<int>("@id");
                return position;
            }
        }
    }
}
