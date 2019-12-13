using Microsoft.EntityFrameworkCore;
using OrderCrateAPI.Contracts;
using OrderCrateAPI.Entities;
using OrderCrateAPI.Helpers;
using OrderCrateAPI.Models.DTOs;
using OrderCrateAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCrateAPI.Repository
{
    public class LoginRepository : RepositoryBase<Login>, ILoginRepository
    {
        private readonly OrdercratedbContext _repositoryContext;
        public User User { get; }

        public LoginRepository(OrdercratedbContext repositoryContext, User user)
           : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
            User = user;
        }
        /// <summary>
        /// Check if username exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Login> CheckUsername(string email)
        {
            var result = await _repositoryContext.Login.SingleAsync(p => p.Username == email);
            return result ?? result;
        }
        /// <summary>
        /// Get All Login Details
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AdLoginViewModel>> GetAll()
        {
            var loginViewModels = new List<AdLoginViewModel>();

            var data = await _repositoryContext.Login.ToListAsync();

            loginViewModels.AddRange(data.Select(d => new AdLoginViewModel()
            {
                Status = d.Status,
                UserID = d.UserID,
                Username = d.Username
            }));

            return loginViewModels;
        }
        /// <summary>
        /// Get Login By ID
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public async Task<AdLoginViewModel> GetById(int loginId)
        {
            var loginResponse = await _repositoryContext.Login.FindAsync(loginId);

            var result = new AdLoginViewModel
            {
                Status = loginResponse.Status,
                UserID = loginResponse.UserID,
                Username = loginResponse.Username
            };
            return result;
        }

        /// <summary>
        /// User Authentication
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<LoginDto> Authenticate(string username, string password)
        {
           if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

           var login = await _repositoryContext.Login.SingleOrDefaultAsync(x => x.Username == username);
           if (login == null)
           {
               var loginError = new LoginDto()
               {
                   UserId = 0,
                   Username = null,
                   Status = false
               };
               return loginError;
           }            

           var result = VerifyPasswordHash(password, login.PasswordHash, login.PasswordSalt);
           if(result != true)
           {
               var loginError = new LoginDto()
               {
                   UserId = login.UserID,
                   Username = login.Username,
                   Status = false,
               };
               return loginError;
           }

           var userDetails = await _repositoryContext.User.FirstOrDefaultAsync(x => x.ID == login.UserID);
           var businessDetails = await _repositoryContext.Business.FirstOrDefaultAsync(x => x.UserID == userDetails.ID);
           var newLogin = new LoginDto()
           {
               UserId = login.UserID,
               Username = login.Username.Trim(),
               Status = login.Status,
               User = new UserDto()
               {
                   ID = userDetails.ID,
                   Lastname = userDetails.Lastname,
                   Firstname = userDetails.Firstname,
                   Birthdate = userDetails.Birthdate,
                   Gender = userDetails.Gender,
                   Email = userDetails.Email,
                   Date_Joined = userDetails.Date_Joined
               },
               Business = new BusinessDto()
               {
                   Id = businessDetails.ID,
                   Name = businessDetails.Description,
                   Email = businessDetails.Industry,
                   Description = businessDetails.Description,
                   Phone = businessDetails.Phone,
                   Industry = businessDetails.Industry
               }
           };
           return newLogin;           
        }

        /// <summary>
        /// Create Login Account
        /// </summary>
        /// <param name="login"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Login> Create(LoginViewModel login, int userId)
        {
            // validation
            if (string.IsNullOrWhiteSpace(login.Password))
                throw new AppException("Password is required");

            if (await _repositoryContext.Login.AnyAsync(x => x.Username == login.Username))
                throw new AppException("Email \"" + login.Username + "\" is already taken");

            CreatePasswordHash(login.Password, out var passwordHash, out var passwordSalt);

            var loginToCreate = new Login
            {
                Username = login.Username,
                UserID = userId,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };

            _repositoryContext.Login.Add(loginToCreate);
            _repositoryContext.SaveChanges();

            return loginToCreate;
        }

        /// <summary>
        /// Update Password
        /// </summary>
        /// <param name="loginParam"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task Update(Login loginParam, string password = null)
        {
            var login = await _repositoryContext.Login.FindAsync(loginParam.ID);

            if (login == null)
                throw new AppException("Login Account not found");

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                login.PasswordHash = passwordHash;
                login.PasswordSalt = passwordSalt;
            }
            _repositoryContext.Login.Update(login);
            _repositoryContext.SaveChanges();
        }


        #region privatehelpermethods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, IReadOnlyList<byte> storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (storedSalt == null) throw new ArgumentNullException(nameof(storedSalt));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Count != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));
            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).",
                    nameof(storedSalt));

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                {
                    return false;
                }
            }

            return true;

        }
        #endregion
    }
}