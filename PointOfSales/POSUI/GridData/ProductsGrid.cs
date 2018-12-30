using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using POSLibrary.Models;
using POSLibrary.DataAccess;
using System.Data;

namespace POSUI.GridData
{

    public class ProductsGrid
    {
        private ProductData product = new ProductData();

        /// <summary>
        /// Gets all products records in a page format.
        /// </summary>
        /// <param name="pageNumber">The current page you want to return</param>
        /// <param name="pageSize">The total item in the page</param>    
        public async Task<IPagedList<ProductModel>> GetPagedProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await Task.Factory.StartNew(() =>
            {
                return product.GetAllRecords().OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            });
        }

        public async Task<IPagedList<ProductModel>> GetPagedProductsAsync(List<ProductModel> products, int pageNumber = 1, int pageSize = 10)
        {
            return await Task.Factory.StartNew(() =>
            {
                return products.OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            });
        }

        /// <summary>
        ///Populates and Formats the grid view control
        /// </summary>
        /// <param name="products">The list of products you want to populate</param>
        public DataTable FormatGridProduct(List<ProductModel> products)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Number",typeof(string));
            table.Columns.Add("Product",typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Unit Cost",typeof(string));
            table.Columns.Add("Unit Price", typeof(string));
            table.Columns.Add("Category",typeof(string));
            table.Columns.Add("In Stock",typeof(bool));
            table.Columns.Add("Expiry", typeof(string));

            foreach (var product in products)
            {
                table.Rows.Add(product.Id,product.Number,product.Name,product?.Description,product.UnitCostPrice.ToString("c2"),product.UnitSellingPrice.ToString("c2"),product.Category?.Name??"",product.Instock,product.ExpiryDate==null?"":product.ExpiryDate.Value.ToString("dd/MM/yyyy"));
            }
            return table;
        }
    }
}
