﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Paycompute.Entity;

namespace Paycompute.Services
{
    public interface IEmployeeService
    {
        Task CreateAsync(Employee newEmployee);
        Employee GetById(int employeeId);
        Task UpdateAsync(Employee employee);
        Task UpdateAsync(int id);
        Task Delete(int id);
        decimal UnionFees(int id);
        decimal StudentLoadRepaymentAmount(int id, decimal totalAmount);
        IEnumerable<Employee> GetAll();
    }
}
