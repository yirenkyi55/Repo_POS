using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POSLibrary.Models;

namespace POSLibrary.Abstract
{
    public interface IProductData
    {
        Task<ProductModel> CreateProduct(ProductModel product);
        Task<bool> DeleteAllRecordsAsync(string procedureName = "spProduct_DeleteAll");
        Task<bool> DeleteRecordAsync(int recordId, string procedureName = "spProduct_Delete", string primaryColumn = "Id");
        Task<int> GenerateNextProductNumber();
        IEnumerable<ProductModel> GetAllRecords(string procedureName = "spProduct_SelectAll");
        Task<bool> UpdateProduct(ProductModel product);
        List<ProductModel> SearchProduct(List<ProductModel> products, SearchProductBy productBy, string item = null, DateTime? period1 = null, DateTime? period2 = null,ProductInStock inStock=0,CategoryModel category=null);
    }
}