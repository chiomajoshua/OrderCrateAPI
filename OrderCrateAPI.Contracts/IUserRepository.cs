using OrderCrateAPI.Entities;
using OrderCrateAPI.Models.DTOs;
using OrderCrateAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCrateAPI.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<FullProfileDTO> GetFullProfile(int UserID);
        Task<IEnumerable<UserViewModel>> GetAll();
        Task<bool> CheckUserExists(string Username);
        Task<IEnumerable<UserViewModel>> GetAllWithSearchString(string SearchString);
        Task<UserViewModel> GetById(int UserID);
        Task<UserViewModel> GetByUsernameString(string SearchString);
        new Task<User> Create(UserViewModel userViewModel);

        //void Update(User login, string Firstname = null, string Lastname = null, string Gender = null, string Email = null);
        new void Update(User user);
    }
}