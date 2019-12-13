using OrderCrateAPI.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCrateAPI.Contracts
{
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        Task<IEnumerable<Transaction>> GetAll();
        Task<IEnumerable<Transaction>> GetBusinessTransactionsByID(int BusinessID);
        Task<Transaction> GetBusinessTransactionByID(int BusinessID);
        Task<Transaction> GetByID(int TransactionID);
        new Task<Transaction> Create(Transaction transaction, int BusinessID);
    }
}