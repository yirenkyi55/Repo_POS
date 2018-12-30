using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLibrary.Models
{
  public  class SupplierModel
    {
        /// <summary>
        /// Represents the Id of the supplier
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// Represents the name of the supplier
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the address of the supplier
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// Represents supplier's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Represents supplier's phone number
        /// </summary>
        public string CellPhoneNumber { get; set; }

        /// <summary>
        /// Represents the products supplied by the supplier
        /// </summary>
        public List<ProductModel> Products { get; set; }
    }
}
