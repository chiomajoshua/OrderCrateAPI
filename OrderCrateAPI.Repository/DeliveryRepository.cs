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

namespace OrderCrateAPI.Repository
{
    public class DeliveryRepository : RepositoryBase<Delivery>, IDeliveryRepository
    {
        private OrdercratedbContext _repositoryContext;
        public DeliveryRepository(OrdercratedbContext repositoryContext)
           : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<Delivery>> GetAll()
        {
            return await FindAllAsync();
        }

        public async Task<IEnumerable<Delivery>> GetBusinessDeliveriesByID(int BusinessID)
        {
            return await FindByCondition(delivery => delivery.Business.ID.Equals(BusinessID));
        }

        public async Task<Delivery> GetBusinessDeliveryByID(int BusinessID)
        {
            return await _repositoryContext.Delivery.SingleAsync(p => p.Business.ID == BusinessID);
        }
        public async Task<Delivery> GetByID(int DeliveryID)
        {
            return await _repositoryContext.Delivery.FindAsync(DeliveryID);
        }

        public async Task<Delivery> GetOrderDeliveryByID(int OrderID)
        {
            return await _repositoryContext.Delivery.SingleAsync(p => p.Order.ID == OrderID);
        }

        public async Task<Delivery> Create(Delivery delivery, int BusinessID, int OrderID)
        {
            if (_repositoryContext.Delivery.Any(x => x.BusinessID == BusinessID && x.Order.ID == OrderID))
                throw new AppException("Delivery With Invoice Number\"" + delivery.Order.InvoiceNumber + "\" Found in Database");


            _repositoryContext.Delivery.Add(delivery);
            _repositoryContext.SaveChanges();

            return delivery;
        }
    }
}
