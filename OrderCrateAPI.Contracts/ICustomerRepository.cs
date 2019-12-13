using OrderCrateAPI.Entities;
using OrderCrateAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCrateAPI.Contracts
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<IEnumerable<CustomerViewModel>> GetAllCustomers(int BusinessID);
        Task<IEnumerable<CustomerViewModel>> GetAll();
        Task<CustomerViewModel> GetById(int CustomerID);
        Task<Customer> Create(CustomerViewModel customer, int BusinessID);
        new void Update(Customer customer);

    }
}
