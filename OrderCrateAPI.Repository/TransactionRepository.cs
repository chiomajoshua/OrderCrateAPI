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
    
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        private OrdercratedbContext _repositoryContext;
        public TransactionRepository(OrdercratedbContext repositoryContext)
           : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return await FindAllAsync();
        }
        public async Task<Transaction> GetByID(int TransactionID)
        {
            return await _repositoryContext.Transaction.FindAsync(TransactionID);
        }
        public async Task<IEnumerable<Transaction>> GetBusinessTransactionsByID(int BusinessID)
        {
            return await FindByCondition(transaction => transaction.Business.ID.Equals(BusinessID));
        }
        public async Task<Transaction> GetBusinessTransactionByID(int BusinessID)
        {
            return await _repositoryContext.Transaction.SingleAsync(p => p.Business.ID == BusinessID);
        }

        public async Task<Transaction> Create(Transaction transaction, int BusinessID)
        {
            if (_repositoryContext.Transaction.Any(x => x.BusinessID == BusinessID && x.ID == transaction.ID))
                throw new AppException("Transaction With ID\"" + transaction.ID + "\" Found in Database");


            _repositoryContext.Transaction.Add(transaction);
            _repositoryContext.SaveChanges();

            return transaction;
        }
    }
}
