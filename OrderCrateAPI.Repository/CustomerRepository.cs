using OrderCrateAPI.Contracts;
using System.Collections.Generic;
using System.Linq;
using OrderCrateAPI.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderCrateAPI;
using OrderCrateAPI.Helpers;
using OrderCrateAPI.Models.ViewModels;

namespace OrderCrateAPI.Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        private OrdercratedbContext _repositoryContext;
        public CustomerRepository(OrdercratedbContext repositoryContext)
           : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public async Task<IEnumerable<CustomerViewModel>> GetAll()
        {
            var result = new List<CustomerViewModel>();
            var data = await _repositoryContext.Customer.ToListAsync();

            result.AddRange(data.Select(d => new CustomerViewModel()
            {
                Title = d.Title,
                Name = d.Name,
                Platform = d.Platform,
                Email = d.Email,
                Phone = d.Phone,
                Address = d.Address
            }));

            return result;
        }
        public async Task<CustomerViewModel> GetById(int CustomerID)
        {
            var customerResponse = await _repositoryContext.Customer.FindAsync(CustomerID);

            var result = new CustomerViewModel
            {
                Title = customerResponse.Title,
                Name = customerResponse.Name,
                Platform = customerResponse.Platform,
                Email = customerResponse.Email,
                Phone = customerResponse.Phone,
                Address = customerResponse.Address
            };

            return result;
        }
        public async Task<IEnumerable<CustomerViewModel>> GetAllCustomers(int BusinessID)
        {
            var result = new List<CustomerViewModel>();
            var data = await FindByCondition(customer => customer.Business.ID.Equals(BusinessID));

            result.AddRange(data.Select(d => new CustomerViewModel()
            {
                Title = d.Title,
                Name = d.Name,
                Platform = d.Platform,
                Email = d.Email,
                Phone = d.Phone,
                Address = d.Address
            }));

            return result;
        }

        public async Task<Customer> Create(CustomerViewModel customer, int BusinessID)
        {
            if (_repositoryContext.Customer.Any(x => x.Phone == customer.Phone && x.Business.ID == BusinessID))
                throw new AppException("Customer With \"" + customer.Phone + "\" Found in Database");

            var customerToCreate = new Customer
            {
                Title = customer.Title,
                Name = customer.Name,
                Platform = customer.Platform,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address
            };
           await _repositoryContext.Customer.AddAsync(customerToCreate);
           await _repositoryContext.SaveChangesAsync();

            return customerToCreate;
        }

        public async Task Update(Customer customerParam, int BusinessID)
        {
            var customer = await _repositoryContext.Customer.SingleAsync(x => x.ID == customerParam.ID && x.Business.ID == BusinessID);

            if (customer.ToString() == null)
                throw new AppException("Customer Account not found");

            if (customerParam.Phone != customer.Phone)
            {
                // username has changed so check if the new username is already taken
                if (_repositoryContext.Customer.Where(p => p.Business.ID == BusinessID).Any(x => x.Phone == customerParam.Phone))
                    throw new AppException("Customer Phone " + customerParam.Phone + " is already taken");
            }

            customer.Title = customerParam.Title;
            customer.Name = customerParam.Name;
            customer.Address = customerParam.Address;
            customer.Email = customerParam.Email;
            customer.Phone = customerParam.Phone;
            customer.Platform = customerParam.Platform;

            _repositoryContext.Customer.Update(customer);
            _repositoryContext.SaveChanges();
        }
    }
}
