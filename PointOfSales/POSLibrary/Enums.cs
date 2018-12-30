using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLibrary
{
    public enum Folders
    {
        Workspace,
        Products
    }


    public enum SearchProductBy
    {
        Number,
        Name,
        Category,
        ExpiryDate,
        InStock
    }

    public enum ProductInStock
    {
        InStock,
        OutOfStock
    }
}
