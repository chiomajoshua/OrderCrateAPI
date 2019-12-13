using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCrateAPI.Contracts
{
    public interface IRepositoryWrapper
    {
        IBusinessRepository Business { get; }
        ICustomerRepository Customer { get; }
        IDeliveryRepository Delivery { get; }
        ILoginRepository Login { get; }
        IOrderRepository Order { get; }
        ITransactionRepository Transaction { get; }
        IUserRepository User { get; }
        void Save();
    }
}
