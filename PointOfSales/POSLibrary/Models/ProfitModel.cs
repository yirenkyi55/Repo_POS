using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLibrary.Models
{
  public  class ProfitModel
    {
        /// <summary>
        /// Represents the unique Id of the profit
        /// </summary>
        public int ProfitId { get; set; }

        /// <summary>
        /// Represents the name of the profit
        /// </summary>
        public string ProfitName { get; set; }

        /// <summary>
        /// Represents the profit amount
        /// </summary>
        public decimal ProfitAmount { get; set; }

        /// <summary>
        /// Represents the date of profit
        /// </summary>
        public DateTime ProfitDate { get; set; }
    }
}
