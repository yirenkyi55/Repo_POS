﻿using POSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POSLibrary.Abstract
{
    public interface IEmployeeData
    {
        Task<EmployeeModel> CreateEmployeeAsync(EmployeeModel employee);
    }
}
