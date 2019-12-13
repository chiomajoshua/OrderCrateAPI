using OrderCrateAPI.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCrateAPI.Contracts
{
    public interface IDeliveryRepository : IRepositoryBase<Delivery>
    {
        Task<IEnumerable<Delivery>> GetAll();
        Task<IEnumerable<Delivery>> GetBusinessDeliveriesByID(int BusinessID);
        Task<Delivery> GetBusinessDeliveryByID(int BusinessID);
        Task<Delivery> GetOrderDeliveryByID(int OrderID);
        Task<Delivery> GetByID(int DeliveryID);
        new Task<Delivery> Create(Delivery delivery, int BusinessID, int OrderID);
    }
}