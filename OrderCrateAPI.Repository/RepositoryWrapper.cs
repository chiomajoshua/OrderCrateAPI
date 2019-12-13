using OrderCrateAPI.Contracts;
using OrderCrateAPI.Entities;
using OrderCrateAPI;
using System;

namespace OrderCrateAPI.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private OrdercratedbContext _repoContext;
        private IBusinessRepository _business;
        private ICustomerRepository _customer;
        private IDeliveryRepository _delivery;
        private ILoginRepository _login;
        private IOrderRepository _order;
        private ITransactionRepository _transaction;
        private IUserRepository _user;        

        public IBusinessRepository Business {
            get
            {
                if (_business == null)
                {
                    _business = new BusinessRepository(_repoContext);
                }

                return _business;
            }

        }
        public ICustomerRepository Customer {
            get
            {
                if (_customer == null)
                {
                    _customer = new CustomerRepository(_repoContext);
                }

                return _customer;
            }
        }
        public IDeliveryRepository Delivery {
            get
            {
                if (_delivery == null)
                {
                    _delivery = new DeliveryRepository(_repoContext);
                }

                return _delivery;
            }
        }
        public ILoginRepository Login {
            get
            {
                if (_login == null)
                {
                    _login = new LoginRepository(_repoContext);
                }

                return _login;
            }
        }
        public IOrderRepository Order {
            get
            {
                if (_order == null)
                {
                    _order = new OrderRepository(_repoContext);
                }

                return _order;
            }
        }
        public ITransactionRepository Transaction {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new TransactionRepository(_repoContext);
                }

                return _transaction;
            }
        }
        public IUserRepository User {
            get
            {
                try
                {
                    if (_user == null)
                    {
                        _user = new UserRepository(_repoContext);
                    }

                    return _user;
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }            
            }
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }

        public RepositoryWrapper(OrdercratedbContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
    }
}
