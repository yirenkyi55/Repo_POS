using Dapper;
using POSLibrary.Abstract;
using POSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
namespace POSLibrary.DataAccess
{

    public class ProductData : DataOperations<ProductModel>, IProductData
    {

        /// <summary>
        /// Saves a product record into a database asynchronously
        /// </summary>
        /// <param name="product">
        /// The product to be saved into the database
        /// </param>
        /// <returns>
        /// The product of the newly inserted record
        /// </returns>
        public async Task<ProductModel> CreateProduct(ProductModel product)
        {

            //Create a connection to database
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var param = new DynamicParameters();
                param.Add("@Number", product.Number);
                param.Add("@Name", product.Name);
                param.Add("@Description", product.Description);
                param.Add("@Quantity", product.Quantity);
                param.Add("@UnitSellingPrice", product.UnitSellingPrice);
                param.Add("@UnitCostPrice", product.UnitCostPrice);
                param.Add("@ExpiryDate", product.ExpiryDate);
                param.Add("@Instock", product.Instock);
                param.Add("@CategoryId", product.Category.Id);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync("spProduct_Insert", param, commandType: CommandType.StoredProcedure);
                product.Id = param.Get<int>("@id");
                return product;
            }

        }


        /// <summary>
        /// Deletes all products from the database
        /// </summary>
        /// <returns>
        /// A boolean indicates success or failure
        /// </returns>
        public override Task<bool> DeleteAllRecordsAsync(string procedureName = "spProduct_DeleteAll")
        {
            return base.DeleteAllRecordsAsync(procedureName);
        }


        /// <summary>
        /// Deletes a product from the database
        /// </summary>
        /// <param name="product">
        /// The product you wish to delete
        /// </param>
        /// <returns>
        /// A boolean indicates whether the delete was successful or not
        /// </returns>

        public override Task<bool> DeleteRecordAsync(int recordId, string procedureName = "spProduct_Delete", string primaryColumn = "Id")
        {
            return base.DeleteRecordAsync(recordId, procedureName, primaryColumn);
        }


        /// <summary>
        /// Generates the next product number for a product
        /// </summary>
        /// <returns>
        /// an integer value indicates the next product 
        /// number from the database
        /// </returns>
        public async Task<int> GenerateNextProductNumber()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                //Retrieve next product number from database
                string productNumber = await connection.ExecuteScalarAsync<string>("spProduct_FindNextId", commandType: CommandType.StoredProcedure);

                //Try convert product number into integer
                int myProductId = 0;
                bool result = int.TryParse(productNumber, out myProductId);

                return myProductId += 9000;
            }
        }

        /// <summary>
        /// Retrieves all product records from the database
        /// </summary>
        /// <returns>
        /// returns list of products
        /// </returns>
        public override IEnumerable<ProductModel> GetAllRecords(string procedureName = "spProduct_SelectAll")
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var query = connection.Query<ProductModel, CategoryModel, ProductModel>("spProduct_SelectAll",
                    (product, category) =>
                    {
                        product.Category = category;
                        return product;
                    }, splitOn: "Id", commandType: CommandType.StoredProcedure);
                return query;
            }
        }

        /// <summary>
        /// Searches the all products by the type you want to search.
        /// </summary>
        /// <param name="products">The list of products you want to search</param>
        /// <param name="productBy">An enum of what you want to search by</param>
        /// <param name="item">The item to search for</param>
        /// <param name="period1">Search by date period</param>
        /// <param name="period2">Search by date period</param>
        /// <returns></returns>
        public List<ProductModel> SearchProduct(List<ProductModel> products, SearchProductBy productBy, string item = null, DateTime? period1 = null, DateTime? period2 = null, ProductInStock inStock = 0, CategoryModel category = null)
        {
            if (productBy == SearchProductBy.Name)
            {
                //Search by product name
                //Find all products that contains the search results
                return products.Where(p => p.Name.ToLower().Contains(item.ToLower())).ToList();
            }
            else if (productBy == SearchProductBy.Number)
            {
                //Search by product number
                return products.Where(p => p.Number.ToLower() == item.ToLower()).ToList();
            }
            else if (productBy == SearchProductBy.InStock)
            {
                //Search by Instock
                if (inStock == ProductInStock.InStock)
                {
                    return products.Where(p => p.Instock == true).ToList();
                }
                else
                {
                    return products.Where(p => p.Instock == false).ToList();
                }
            }
            else if (productBy == SearchProductBy.Category)
            {
                //Search by category name
                return products.Where(p => p.Category.Name.ToLower()==category.Name.ToLower()).ToList();
            }else if (productBy == SearchProductBy.ExpiryDate)
            {
                //Search by expiry date
                return products.Where(p => p.ExpiryDate >= period1 && p.ExpiryDate <= period2).ToList();
            }
            return new List<ProductModel>();
        }

        /// <summary>
        /// Update a product in the database
        /// </summary>
        /// <param name="product">
        /// The product you want to update
        /// </param>
        /// <returns>
        /// Returns a boolean value idicating success.
        /// </returns>
        public async Task<bool> UpdateProduct(ProductModel product)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString()))
            {
                var param = new DynamicParameters();
                param.Add("@Id", product.Id);
                param.Add("@Number", product.Number);
                param.Add("@Name", product.Name);
                param.Add("@Quantity", product.Quantity);
                param.Add("@UnitCostPrice", product.UnitCostPrice);
                param.Add("@UnitSellingPrice", product.UnitSellingPrice);
                param.Add("@Description", product.Description);
                param.Add("@UnitCostPrice", product.UnitCostPrice);
                param.Add("@ExpiryDate", product.ExpiryDate);
                param.Add("@Instock", product.Instock);
                param.Add("@CategoryId", product.Category?.Id);
                int result = await connection.ExecuteAsync("spProduct_Update", param, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }
    }
}
