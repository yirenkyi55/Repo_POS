using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLibrary.Models
{
  public  class PurchaseModel
    {
        /// <summary>
        /// Represents the Id of the purchase made
        /// </summary>
        public int PurchaseId { get; set; }


        /// <summary>
        /// Represents product of purchase item
        /// </summary>
        public ProductModel Product { get; set; }

        /// <summary>
        /// Represents the date of purchase item
        /// </summary>
        public DateTime DatePurchased { get; set; }

        /// <summary>
        /// Represents the quantity of purchase item
        /// </summary>
        public int Quantity { get; set; }
    }
}
