using POSLibrary.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLibrary.Models
{
     public  class ProductModel 
    {
        /// <summary>
        /// Represents the product Id which is 
        /// auto-increment
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Represents the product number which is unique 
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Represents the name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the description of the product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents the quantity of the product
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Represents the selling price for an item
        /// </summary>
        public decimal UnitSellingPrice { get; set; }

        /// <summary>
        /// Represents the unit cost price for an item
        /// </summary>
        public decimal UnitCostPrice { get; set; }

       
        /// <summary>
        /// Represents the expirydate for the item if possible
        /// </summary>
        public DateTime? ExpiryDate { get; set; }  

        /// <summary>
        /// Represents the Category in which a product belongs
        /// </summary>
        public CategoryModel Category { get; set; }

       
        /// <summary>
        /// Represents the availability of the item
        /// </summary>
        public bool Instock { get; set; }
    }
}
