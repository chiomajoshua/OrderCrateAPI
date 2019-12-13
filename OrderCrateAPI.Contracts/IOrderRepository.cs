using OrderCrateAPI.Entities;
using OrderCrateAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCrateAPI.Contracts
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<IEnumerable<OrderViewModel>> GetAll();
        Task<IEnumerable<OrderViewModel>> GetBusinessOrdersByID(int BusinessID);
        Task<IEnumerable<OrderViewModel>> GetCustomerOrdersByID(int CustomerID);
        Task<OrderViewModel> GetCustomerOrderByID(int CustomerID);
        Task<OrderViewModel> GetOrderByInvoiceNumber(string InvoiceNumber, int BusinessID);
        Task<OrderViewModel> GetBusinessOrderByID(int BusinessID);
        Task<OrderViewModel> GetByID(int OrderID);
        new Task<Order> Create(OrderViewModel orderViewModel, int BusinessID);
    }
}
