using OrderCrateAPI.Contracts;
using Microsoft.EntityFrameworkCore;
using OrderCrateAPI;
using OrderCrateAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderCrateAPI.Entities;
using OrderCrateAPI.Models.ViewModels;

namespace OrderCrateAPI.Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private OrdercratedbContext _repositoryContext;
        public OrderRepository(OrdercratedbContext repositoryContext)
           : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public async Task<IEnumerable<OrderViewModel>> GetAll()
        {
            var orderViewModels = new List<OrderViewModel>();
            var data = await _repositoryContext.Order.ToListAsync();

            orderViewModels.AddRange(data.Select(d => new OrderViewModel()
            {
                BusinessID = d.BusinessID,
                InvoiceNumber = d.InvoiceNumber,
                CustomerID = d.CustomerID,
                Description = d.Description,
                Date = d.Date,
                Discount = d.Discount,
                OrderPlatform = d.OrderPlatform,
                Status = d.Status
            }));

            return orderViewModels;
        }

        public async Task<OrderViewModel> GetByID(int OrderID)
        {
            var orderResponse = await _repositoryContext.Order.FindAsync(OrderID);

            var result = new OrderViewModel
            {
                BusinessID = orderResponse.BusinessID,
                InvoiceNumber = orderResponse.InvoiceNumber,
                CustomerID = orderResponse.CustomerID,
                Description = orderResponse.Description,
                Date = orderResponse.Date,
                Discount = orderResponse.Discount,
                OrderPlatform = orderResponse.OrderPlatform,
                Status = orderResponse.Status
            };

            return result;
        }
        public async Task<IEnumerable<OrderViewModel>> GetBusinessOrdersByID(int BusinessID)
        {
            var orderViewModels = new List<OrderViewModel>();
            var data = await FindByCondition(order => order.Business.ID.Equals(BusinessID));

            orderViewModels.AddRange(data.Select(d => new OrderViewModel()
            {
                BusinessID = d.BusinessID,
                InvoiceNumber = d.InvoiceNumber,
                CustomerID = d.CustomerID,
                Description = d.Description,
                Date = d.Date,
                Discount = d.Discount,
                OrderPlatform = d.OrderPlatform,
                Status = d.Status
            }));
            return orderViewModels;
        }
        public async Task<OrderViewModel> GetBusinessOrderByID(int BusinessID)
        {
            var orderResponse = await _repositoryContext.Order.SingleAsync(p => p.Business.ID == BusinessID);

            var result = new OrderViewModel
            {
                BusinessID = orderResponse.BusinessID,
                InvoiceNumber = orderResponse.InvoiceNumber,
                CustomerID = orderResponse.CustomerID,
                Description = orderResponse.Description,
                Date = orderResponse.Date,
                Discount = orderResponse.Discount,
                OrderPlatform = orderResponse.OrderPlatform,
                Status = orderResponse.Status
            };

            return result;
        }

        public async Task<IEnumerable<OrderViewModel>> GetCustomerOrdersByID(int CustomerID)
        {
            var orderViewModels = new List<OrderViewModel>();
            var data = await FindByCondition(order => order.Customer.ID.Equals(CustomerID));

            orderViewModels.AddRange(data.Select(d => new OrderViewModel()
            {
                BusinessID = d.BusinessID,
                InvoiceNumber = d.InvoiceNumber,
                CustomerID = d.CustomerID,
                Description = d.Description,
                Date = d.Date,
                Discount = d.Discount,
                OrderPlatform = d.OrderPlatform,
                Status = d.Status
            }));
            return orderViewModels;
        }

        public async Task<OrderViewModel> GetCustomerOrderByID(int CustomerID)
        {
            var orderResponse = await _repositoryContext.Order.SingleAsync(p => p.Customer.ID == CustomerID);

            var result = new OrderViewModel
            {
                BusinessID = orderResponse.BusinessID,
                InvoiceNumber = orderResponse.InvoiceNumber,
                CustomerID = orderResponse.CustomerID,
                Description = orderResponse.Description,
                Date = orderResponse.Date,
                Discount = orderResponse.Discount,
                OrderPlatform = orderResponse.OrderPlatform,
                Status = orderResponse.Status
            };

            return result;
        }

        public async Task<Order> Create(OrderViewModel order, int BusinessID)
        {
            if (_repositoryContext.Order.Any(x => x.BusinessID == BusinessID && x.InvoiceNumber == order.InvoiceNumber))
                throw new AppException("Order With Invoice Number\"" + order.InvoiceNumber + "\" Found in Database");
            var orderToCreate = new Order
            {
                BusinessID = BusinessID,
                InvoiceNumber = order.InvoiceNumber,
                CustomerID = order.CustomerID,
                Description = order.Description,
                Date = order.Date,
                Discount = order.Discount,
                OrderPlatform = order.OrderPlatform,
                Status = order.Status
            };

            _repositoryContext.Order.Add(orderToCreate);
            _repositoryContext.SaveChanges();

            return orderToCreate;
        }

        public async Task<OrderViewModel> GetOrderByInvoiceNumber(string InvoiceNumber, int BusinessID)
        {
            var orderResponse = await _repositoryContext.Order.SingleAsync(p => p.InvoiceNumber == InvoiceNumber && p.BusinessID == BusinessID);

            var result = new OrderViewModel
            {
                BusinessID = orderResponse.BusinessID,
                InvoiceNumber = orderResponse.InvoiceNumber,
                CustomerID = orderResponse.CustomerID,
                Description = orderResponse.Description,
                Date = orderResponse.Date,
                Discount = orderResponse.Discount,
                OrderPlatform = orderResponse.OrderPlatform,
                Status = orderResponse.Status
            };

            return result;
        }
    }
}
