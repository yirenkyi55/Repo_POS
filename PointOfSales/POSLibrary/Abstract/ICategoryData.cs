using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POSLibrary.Models;

namespace POSLibrary.Abstract
{
    public interface ICategoryData
    {
        Task<CategoryModel> CreateCategoryAsync(CategoryModel category);
        Task<bool> DeleteAllRecordsAsync(string procedureName = "spCategory_DeleteAll");
        Task<bool> DeleteRecordAsync(int recordId, string procedureName = "spCategory_Delete", string primaryColumn = "Id");
        IEnumerable<CategoryModel> GetAllRecords(string procedureName = "spCategories_SelectAll");
        IEnumerable<CategoryModel> SearchForRecord(IEnumerable<CategoryModel> allRecords, string itemToSearch, Func<CategoryModel, bool> searchPattern1 = null, Func<CategoryModel, bool> searchPattern2 = null);
        Task<bool> UpdateCategoryAsync(CategoryModel category);
    }
}