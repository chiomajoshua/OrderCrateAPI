using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderCrateAPI.Contracts;
using OrderCrateAPI.Models.DTOs;
using OrderCrateAPI.Models.ViewModels;

namespace OrderCrateAPI.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private ILoggerManager _logger;
        private ResponseDataDTO response;

        public LoginController(ILoggerManager logger, IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
        }
        // GET: api/Login
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _repoWrapper.Login.GetAll();
                response = new ResponseDataDTO
                {
                    ResponseObject = result,
                    ResponseCode = 200,
                    RecordCount = result.Count(),
                    RespMessage = "Success"
                };
                _logger.LogInfo($"Returned all Users from database.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ResponseDataDTO
                {
                    ResponseCode = 400,
                    ResponseObject = null,
                    RecordCount = 0,
                    RespMessage = ex.Message
                };
                _logger.LogError($"Some error in the Login:GetAll method: {ex}");
                return BadRequest(response);
            }
        }

        [HttpGet("{id}", Name = "GetLoginById")]
        public async Task<IActionResult> GetLoginByID(int id)
        {
           try
            {
                var result = await _repoWrapper.Login.GetByID(id);

                if (result == null)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = result,
                        ResponseCode = 404,
                        RecordCount = 0,
                        RespMessage = "User Not Found"
                    };
                    _logger.LogError($"user with id: {id}, hasn't been found in db.");
                    return NotFound(response);
                }
                else
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = result,
                        ResponseCode = 200,
                        RecordCount = 1,
                        RespMessage = "Success"
                    };
                    _logger.LogInfo($"Returned user with id: {id}");
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response = new ResponseDataDTO
                {
                    ResponseCode = 400,
                    ResponseObject = null,
                    RecordCount = 0,
                    RespMessage = ex.Message
                };
                _logger.LogError($"Something went wrong inside GetLoginByID action: {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(LoginViewModel loginViewModel)
        {
           try
            {
                if (ModelState.IsValid)
                {                   
                   var result = await _repoWrapper.Login.Authenticate(loginViewModel.Username, loginViewModel.Password);
                    if (!string.IsNullOrEmpty(result.Username))
                    {
                        if (result.User == null)
                        {
                            response = new ResponseDataDTO
                            {
                                ResponseObject = null,
                                ResponseCode = 404,
                                RecordCount = 0,
                                RespMessage = "Sorry, Password Provided For " + loginViewModel.Username + " Is Incorrect"
                            };
                            _logger.LogError($"user with id: {loginViewModel.Username}, has incorrect password");
                            return NotFound(response);
                        }
                        else
                        if (result.Status == false)
                        {
                            response = new ResponseDataDTO
                            {
                                ResponseObject = null,
                                ResponseCode = 404,
                                RecordCount = 0,
                                RespMessage = "Sorry, Account: " + loginViewModel.Username + " Is Disabled"
                            };
                            _logger.LogError($"user with id: {loginViewModel.Username}, is disabled.");
                            return NotFound(response);
                        }
                        else
                        {
                            response = new ResponseDataDTO
                            {
                                ResponseObject = result,
                                ResponseCode = 200,
                                RecordCount = 1,
                                RespMessage = "Success"
                            };
                            _logger.LogInfo($"Returned user with id: {loginViewModel.Username}");
                            return Ok(response);
                        }
                    }
                    else
                    {
                        response = new ResponseDataDTO
                        {
                            ResponseObject = null,
                            ResponseCode = 204,
                            RecordCount = 0,
                            RespMessage = "Email: " + loginViewModel.Username + " Is Not Registered On Order Crate."
                        };
                        _logger.LogError($"user with id: {loginViewModel.Username}, does not exist.");
                        return NotFound(response);
                    }                   
                }                
                return Ok(null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Authenticate action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Login
        [HttpPost ("CreateLogin")]
        public async Task<IActionResult> CreateLogin([FromBody] LoginViewModel loginViewModel, int UserID)
        {
           try
            {
                if (ModelState.IsValid)
                {
                    var result = await _repoWrapper.Login.Create(loginViewModel, UserID);
                    if (result.Status != true)
                    {
                        response = new ResponseDataDTO
                        {
                            ResponseObject = result,
                            ResponseCode = 404,
                            RecordCount = 0,
                            RespMessage = "Cannot Create Login Credentials"
                        };
                        _logger.LogError($"Cannot Create Login Credentials for User: {loginViewModel.Username}.");
                        return NotFound(response);
                    }
                    else
                    {
                        response = new ResponseDataDTO
                        {
                            ResponseObject = result,
                            ResponseCode = 201,
                            RecordCount = 0,
                            RespMessage = "Success"
                        };
                        _logger.LogInfo($"Created Login Credentials for User: {loginViewModel.Username}");
                        return Ok(response);
                    }
                }
                return Ok(null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateLogin action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Login/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
