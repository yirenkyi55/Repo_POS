using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLibrary.Models
{
     public  class ExpenseModel
    {
        /// <summary>
        /// Represents the unique id of expenses
        /// </summary>
        public int ExpenseId { get; set; }

        /// <summary>
        /// Represents the name of expenses
        /// </summary>
        public string ExpenseName { get; set; }

        /// <summary>
        /// Represents the amount spent
        /// </summary>
        public decimal ExpenseAmount { get; set; }

        /// <summary>
        /// Represents the Date of expenditure
        /// </summary>
        public DateTime ExpenseDate { get; set; }
    }
}
