using POSLibrary.Abstract;
using POSLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace POSLibrary.DataAccess
{
    public class EmployeeData : IEmployeeData
    {

        /// <summary>
        /// Creates a new employee into sql database
        /// </summary>
        /// <param name="employee">
        /// The employee model to be created
        /// </param>
        /// <returns>
        /// The newly created employee
        /// </returns>
        public async Task<EmployeeModel> CreateEmployeeAsync(EmployeeModel employee)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var param = new DynamicParameters();
                param.Add("@FirstName", employee.FirstName);
                param.Add("@LastName", employee.LastName);
                param.Add("@Gender", employee.Gender);
                param.Add("@PictureName", employee.PictureName);
                param.Add("@PictureData", employee.PictureData);
                param.Add("@Phone", employee.Phone);
                param.Add("@Email", employee.Email);
                param.Add("@Address", employee.Address);
                param.Add("@PositionId", employee.Position.PositionId);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                //Execute the command
               await connection.ExecuteAsync("spEmployee_Insert", param, commandType: CommandType.StoredProcedure);

                //Retrieve the employee id
                employee.EmployeeId = param.Get<int>("@id");
                return employee;
            }
        }
    }
}
