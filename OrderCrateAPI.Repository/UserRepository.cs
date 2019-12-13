using OrderCrateAPI.Contracts;
using Microsoft.EntityFrameworkCore;
using OrderCrateAPI;
using OrderCrateAPI.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderCrateAPI.Entities;
using OrderCrateAPI.Models.ViewModels;
using OrderCrateAPI.Models.DTOs;

namespace OrderCrateAPI.Repository
{
   public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private OrdercratedbContext _repositoryContext;
        public UserRepository(OrdercratedbContext repositoryContext)
           : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public async Task<IEnumerable<UserViewModel>> GetAll()
        {
            try
            {
               var userViewModels = new List<UserViewModel>();
                var data = await _repositoryContext.User.ToListAsync();

                userViewModels.AddRange(data.Select(d => new UserViewModel()
                {
                    ID = d.ID,
                    Lastname = d.Lastname,
                    Firstname = d.Firstname,
                    Birthdate = d.Birthdate,
                    Date_Joined = d.Date_Joined,
                    Email = d.Email,
                    Gender = d.Gender

                }));

                return userViewModels;
            }
            catch(Exception ce)
            {
                throw new Exception(ce.Message);
            }
        }
        public async Task<UserViewModel> GetById(int UserID)
        {
           var userResponse = await _repositoryContext.User.Where(a => a.ID == UserID).FirstOrDefaultAsync();

            //var businessdetails = await _repositoryContext.Business.FirstOrDefaultAsync(x => x.UserID == userdetails.ID);

            var result = new UserViewModel
            {
                ID = userResponse.ID,
                Lastname = userResponse.Lastname,
                Firstname = userResponse.Firstname,
                Birthdate = userResponse.Birthdate,
                Date_Joined = userResponse.Date_Joined,
                Email = userResponse.Email,
                Gender = userResponse.Gender
            };

            return result;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllWithSearchString(string SearchString)
        {
            List<UserViewModel> userViewModels = null;
            var data = await _repositoryContext.User.Where(a => a.Lastname == SearchString || a.Firstname == SearchString 
                || a.Email == SearchString || a.Birthdate.ToString() == SearchString || a.Gender == SearchString).ToListAsync();

            userViewModels.AddRange(data.Select(d => new UserViewModel
            {
                ID = d.ID,
                Lastname = d.Lastname,
                Firstname = d.Firstname,
                Birthdate = d.Birthdate,
                Date_Joined = d.Date_Joined,
                Email = d.Email,
                Gender = d.Gender
            }));
            return userViewModels;
        }

        public async Task<UserViewModel> GetByUsernameString(string SearchString)
        {
            try
            {
                var userResponse = await _repositoryContext.User.Where(a => a.Email == SearchString).FirstOrDefaultAsync();
                var result = new UserViewModel();

                if (userResponse == null)
                {
                    return result;
                }
                else
                {
                    result = new UserViewModel
                    {
                        ID = userResponse.ID,
                        Lastname = userResponse.Lastname,
                        Firstname = userResponse.Firstname,
                        Birthdate = userResponse.Birthdate,
                        Date_Joined = userResponse.Date_Joined,
                        Email = userResponse.Email,
                        Gender = userResponse.Gender
                    };
                    return result;
                }
            }
            catch(Exception ce)
            {
                throw new Exception(ce.Message);
            }
        }

        public new async Task Update(User userParam)
        {
            var user = await _repositoryContext.User.FindAsync(userParam.ID);

            if (user == null)
                throw new AppException("User Account not found");

            if (userParam.Email != user.Email)
            {
                // Email has changed so check if the new Email is already taken
                if (await _repositoryContext.User.AnyAsync(x => x.Email == userParam.Email))
                    throw new AppException("Email " + userParam.Email + " is already taken");
            }

            user.Lastname = userParam.Lastname;
            user.Firstname = userParam.Firstname;
            user.Gender = userParam.Gender;
            user.Birthdate = userParam.Birthdate;
            user.Email = userParam.Email;

            _repositoryContext.User.Update(user);
            _repositoryContext.SaveChanges();
        }

       public new async Task<User> Create(UserViewModel user)
        {
            var userToCreate = new User
            {
                Lastname = user.Lastname,
                Firstname = user.Firstname,
                Gender = user.Gender,
                Birthdate = user.Birthdate,
                Email = user.Email,
                Date_Joined = user.Date_Joined              
            };
           
            await _repositoryContext.User.AddAsync(userToCreate);
            await _repositoryContext.SaveChangesAsync();

            return userToCreate;
       }

        public async Task<bool> CheckUserExists(string Username)
        {
           var checkUser = await _repositoryContext.User.Where(a => a.Email == Username).FirstOrDefaultAsync();

            if(checkUser == null)
            {
                return false;
            }
            else            
            {
                return true;
            }
        }

        /// <summary>
        /// Returns the Full User Profile
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<FullProfileDTO> GetFullProfile(int UserID)
        {
            var loginResponse = await _repositoryContext.Login.FirstOrDefaultAsync(a => a.UserID == UserID);
            var userResponse = await _repositoryContext.User.FirstOrDefaultAsync(a => a.ID == UserID);
            var businessDetails = await _repositoryContext.Business.FirstOrDefaultAsync(x => x.UserID == userResponse.ID);


            #region GettingOrdersAndMapping
            var orderDetails = await _repositoryContext.Order
                .Where(x => x.BusinessID == businessDetails.ID)
                .OrderByDescending(m => m.Date)
                .Take(20).AsNoTracking()
                .ToListAsync();
            var orderListDto = new List<OrderDTO>();
            orderListDto.AddRange(orderDetails.Select(order => new OrderDTO
            {
                ID = order.ID,
                Description = order.Description,
                Date = order.Date.ToString(),
                InvoiceNumber = order.InvoiceNumber,
                OrderPlatform = order.OrderPlatform,
                Discount = order.Discount,
                Status = order.Status
            }));
            #endregion

            #region GettingTransactionsAndMapping
            var transactionDetails = await _repositoryContext.Transaction.Where(x => x.BusinessID == businessDetails.ID)
                                                                         .OrderByDescending(m => m.Date)
                                                                         .Take(20).AsNoTracking()
                                                                         .ToListAsync();
            var transactionListDto = new List<TransactionDTO>();
            transactionListDto.AddRange(transactionDetails.Select(trans => new TransactionDTO
            {
                ID = trans.ID,
                Amount = trans.Amount,
                Date = trans.Date.ToString(),
                DebitCredit = trans.DebitCredit,
                Description = trans.Description,
            }));
            #endregion


            var result = new FullProfileDTO
            {
                UserID = loginResponse.UserID,
                Username = loginResponse.Username,
                Status = loginResponse.Status,
                User = new UserBusinessDTO()
                {
                    ID = userResponse.ID,
                    Lastname = userResponse.Lastname,
                    Firstname = userResponse.Firstname,
                    Birthdate = userResponse.Birthdate,
                    Date_Joined = userResponse.Date_Joined,
                    Email = userResponse.Email,
                    Gender = userResponse.Gender,
                    Business = new BusinessOrderTransactionDTO()
                    {
                        ID = businessDetails.ID,
                        Description = businessDetails.Description,
                        Email = businessDetails.Email,
                        Industry = businessDetails.Industry,
                        Name = businessDetails.Name,
                        Phone = businessDetails.Phone,
                        Order = orderListDto,
                        Transaction = transactionListDto
                    }
                }                 
            };
            return result;
        }
    }
}
