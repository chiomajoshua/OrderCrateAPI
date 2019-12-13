using OrderCrateAPI.Entities;
using OrderCrateAPI.Models.DTOs;
using OrderCrateAPI.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCrateAPI.Contracts
{
    public interface ILoginRepository : IRepositoryBase<Login>
    {
        Task<IEnumerable<AdLoginViewModel>> GetAll();
        Task<AdLoginViewModel> GetById(int loginId);
        Task<Login> Create(LoginViewModel login, int userId);
        Task<LoginDto> Authenticate(string username, string password);
        Task Update(Login login, string password =null);
        Task<Login> CheckUsername(string email);
    }
}