using OrderCrateAPI.Entities;
using OrderCrateAPI.Models.DTOs;
using OrderCrateAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCrateAPI.Contracts
{
    public interface IBusinessRepository : IRepositoryBase<Business>
    {
        Task<IEnumerable<BusinessViewModel>> GetAll();
        Task<bool> CheckBusinessExistsPhone(string Phone);
        Task<bool> CheckBusinessExistsName(string Name);
        Task<BusinessDTO> GetByID(int BusinessID);
        Task<IEnumerable<BusinessViewModel>> GetUserBusinessesByID(int UserID);
        Task<BusinessViewModel> GetUserBusinessByID(int UserID);
        new Task<Business> Create(BusinessViewModel business, int UserID);
        new void Update(Business business);
    }
}
