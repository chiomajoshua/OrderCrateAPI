using OrderCrateAPI.Contracts;
using System.Collections.Generic;
using System.Linq;
using OrderCrateAPI.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderCrateAPI.Helpers;
using OrderCrateAPI.Models.DTOs;
using OrderCrateAPI.Models.ViewModels;

namespace OrderCrateAPI.Repository
{
    public class BusinessRepository : RepositoryBase<Business>, IBusinessRepository
    {
        private OrdercratedbContext _repositoryContext;
        public BusinessRepository(OrdercratedbContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<BusinessViewModel>> GetAll()
        {
            var businessViewModels = new List<BusinessViewModel>();
            var data = await _repositoryContext.Business.ToListAsync();

            businessViewModels.AddRange(data.Select(d => new BusinessViewModel
            {
                Name = d.Name,
                Description = d.Description,
                Email = d.Email,
                Industry = d.Industry,
                Phone = d.Phone
            }));

            return businessViewModels;
        }

        public async Task<BusinessDTO> GetByID(int BusinessID)
        {             
           var businessResponse = await _repositoryContext.Business.Where(a => a.ID == BusinessID).FirstOrDefaultAsync();

            var orderdetails = await _repositoryContext.Order.
                 Where(ord => ord.BusinessID == businessResponse.ID)
                 .ToListAsync();

            var orderDetailsDto = orderdetails.Select(a => new OrderDTO {
                ID = a.ID,
                Description = a.Description,
                Date = a.Date.ToString(),
                Discount = a.Discount,
                InvoiceNumber = a.InvoiceNumber,
                OrderPlatform = a.OrderPlatform,
                Status = a.Status
            }).ToList();

            var result = new BusinessDTO
            {
                Name = businessResponse.Name,
                Description = businessResponse.Description,
                Email = businessResponse.Email,
                Industry = businessResponse.Industry,
                Phone = businessResponse.Phone
            };            
            return result;
        }

        public async Task<IEnumerable<BusinessViewModel>> GetUserBusinessesByID(int UserID)
        {
            List<BusinessViewModel> businessViewModels = null;
            var data = await FindByCondition(business => business.User.ID.Equals(UserID));

            businessViewModels.AddRange(data.Select(d => new BusinessViewModel
            {
                Name = d.Name,
                Description = d.Description,
                Email = d.Email,
                Industry = d.Industry,
                Phone = d.Phone
            }));

            return businessViewModels;
        }

        public new async Task<Business> Create(BusinessViewModel business, int UserID)
        {
            //if (await _repositoryContext.Business.AnyAsync(x => x.Name == business.Name || x.Email == business.Email || x.Phone == business.Phone))
            //    throw new AppException("Business Name \"" + business.Name + " or Business Phone \"" + business.Phone + "or Business Email" + business.Email + "\" is already taken");

            var businessTocreate = new Business
            {
                Name = business.Name,
                Description = business.Description,
                Email = business.Email,
                Industry = business.Industry,
                Phone = business.Phone,
                UserID = UserID
            };
            _repositoryContext.Business.Add(businessTocreate);
            _repositoryContext.SaveChanges();

            return businessTocreate;
        }

        public async Task<BusinessViewModel> GetUserBusinessByID(int UserID)
        {
            var businessResponse = await _repositoryContext.Business.SingleAsync(p => p.User.ID == UserID);

            var result = new BusinessViewModel
            {
                Name = businessResponse.Name,
                Description = businessResponse.Description,
                Email = businessResponse.Email,
                Industry = businessResponse.Industry,
                Phone = businessResponse.Phone,
            };

            return result;
        }
        public async Task Update(Business businessParam, int UserID)
        {
            var business = await _repositoryContext.Business.SingleAsync(x => x.ID == businessParam.ID && x.User.ID == UserID);

            if (business.ToString() == null)
                throw new AppException("Business Account not found");

            if (businessParam.Name != business.Name)
            {
                // username has changed so check if the new username is already taken
                if (_repositoryContext.Business.Where(p => p.User.ID == UserID).Any(x => x.Name == businessParam.Name ||
                x.Phone == businessParam.Phone))
                    throw new AppException("Business Name " + businessParam.Name + "/ Business Phone " + businessParam.Phone + "/ Business Email " + businessParam.Email + " is already taken");
            }

            business.Name = businessParam.Name;
            business.Email = businessParam.Email;
            business.Phone = businessParam.Phone;
            business.Description = businessParam.Description;
            business.Industry = businessParam.Industry;

            _repositoryContext.Business.Update(business);
            _repositoryContext.SaveChanges();
        }

        public async Task<bool> CheckBusinessExistsPhone(string Phone)
        {
            var checkBusiness = await _repositoryContext.Business.FirstOrDefaultAsync(a => a.Phone == Phone);//.FirstOrDefaultAsync();

            if (checkBusiness == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckBusinessExistsName(string Name)
        {
            var checkBusiness = await _repositoryContext.Business.FirstOrDefaultAsync(a => a.Name == Name);//.FirstOrDefaultAsync();

            if (checkBusiness == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}