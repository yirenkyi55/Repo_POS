using POSLibrary.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLibrary.Models
{

    public class CategoryModel 
    {
       
        /// <summary>
        /// Represents the unique Id for a product category
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Represents a product category name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents list of products belonging to a category
        /// </summary>
        public List<ProductModel> Products { get; set; }
       
    }
}
