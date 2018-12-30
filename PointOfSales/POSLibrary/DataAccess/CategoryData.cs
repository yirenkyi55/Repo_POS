using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSLibrary.Abstract;
using POSLibrary.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace POSLibrary.DataAccess
{

    public class CategoryData : DataOperations<CategoryModel>, ICategoryData
    {

        /// <summary>
        /// Creates a new category model in a sql database
        /// </summary>
        /// <param name="category">
        /// The category model to be created
        /// </param>
        /// <returns>
        /// The newly category id
        /// </returns>
        public async Task<CategoryModel> CreateCategoryAsync(CategoryModel category)
        {
            //Create a connection to the database
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                //Create a dapper dynamic parameters
                var param = new DynamicParameters();

                //Add models to parameters
                param.Add("@Name", category.Name);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                //Execute the commad
                await connection.ExecuteAsync("spCategory_Insert", param, commandType: CommandType.StoredProcedure);

                //Get the category id
                category.Id = param.Get<int>("@id");

                return category;
            }
        }


        public override IEnumerable<CategoryModel> GetAllRecords(string procedureName = "spCategories_SelectAll")
        {
            return base.GetAllRecords(procedureName).OrderBy(c=>c.Name);
        }

        /// <summary>
        /// Search for a specific category by providing a categoryName.
        /// </summary>

        public override IEnumerable<CategoryModel> SearchForRecord(IEnumerable<CategoryModel> allRecords, string itemToSearch, Func<CategoryModel, bool> searchPattern1 = null, Func<CategoryModel, bool> searchPattern2 = null)
        {
            if (searchPattern1 == null)
                searchPattern1 = cat => cat.Name.ToLower().StartsWith(itemToSearch.ToLower());
            if (searchPattern2 == null)
                searchPattern2 = cat => cat.Name.ToLower().Contains(itemToSearch.ToLower());
            return base.SearchForRecord(allRecords, itemToSearch, searchPattern1, searchPattern2);
        }


        /// <summary>
        /// Deletes a category from the database
        /// </summary>
        /// <param name="CategoryId"> The categoryId you want to delete</param>

        public override Task<bool> DeleteRecordAsync(int recordId, string procedureName = "spCategory_Delete", string primaryColumn = "Id")
        {
            return base.DeleteRecordAsync(recordId, procedureName, primaryColumn);
        }

        /// <summary>
        /// Deletes all categories from the database
        /// </summary>
        /// <param name="procedureName">The procedure name used in the delete process</param>
        /// <returns></returns>    

        public override Task<bool> DeleteAllRecordsAsync(string procedureName = "spCategory_DeleteAll")
        {
            return base.DeleteAllRecordsAsync(procedureName);
        }

        /// <summary>
        /// Update a category record
        /// </summary>
        /// <param name="category">The category you want to update</param>
        /// <returns>A result indicating success/failure.</returns>
        public async Task<bool> UpdateCategoryAsync(CategoryModel category)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var param = new DynamicParameters();
                param.Add("@Id", category.Id);
                param.Add("@Name", category.Name);
                var result = await connection.ExecuteAsync("spCategory_Update", param, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }
    }
}
