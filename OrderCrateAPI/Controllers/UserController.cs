using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderCrateAPI.Contracts;
using OrderCrateAPI.Entities;
using OrderCrateAPI.Models.DTOs;
using OrderCrateAPI.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderCrateAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private ILoggerManager _logger;

        public UserController(ILoggerManager logger, IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.User.GetAll();
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

               _logger.LogError($"Some error in the Users:GetAll method: {ex}");
                return BadRequest(response);
            }
        }
        [HttpGet("GetAllWithSearchString/{SearchString}", Name = "GetAllWithSearchString")]
        public async Task<IActionResult> GetAllWithSearchString(string SearchString)
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.User.GetAllWithSearchString(SearchString);
                response = new ResponseDataDTO
                {
                    ResponseObject = result,
                    ResponseCode = 200,
                    RecordCount = result.Count(),
                    RespMessage = "Success"
                };
                _logger.LogInfo($"Returned all Users For SearchString: {SearchString} from database.");
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
                _logger.LogError($"Some error in the Users:GetAll method: {ex}");
                return BadRequest(response);
            }
        }

        // GET api/<controller>/5
        [HttpGet("GetUserByID/{id}", Name = "UserById")]
        public async Task<IActionResult> GetUserByID(int id)
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.User.GetById(id);

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
                _logger.LogError($"Something went wrong inside GetUserByID action: {ex.Message}");
                return BadRequest(response);
            }
        }
        [HttpGet("GetByUsernameString/{SearchUserString}", Name = "GetByUsernameString")]
        public async Task<IActionResult> GetByUsernameString(string SearchUserString)
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.User.GetByUsernameString(SearchUserString);

                if (result.ID < 1)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = result,
                        ResponseCode = 404,
                        RecordCount = 0,
                        RespMessage = "User Not Found"
                    };
                    _logger.LogError($"user with Name: {SearchUserString}, hasn't been found in db.");
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
                    _logger.LogInfo($"Returned user with Name: {SearchUserString}");
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
                _logger.LogError($"Something went wrong inside GetByUsernameString action: {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpGet("GetFullProfile/{UserID}", Name = "GetFullProfile")]
        public async Task<IActionResult> FullProfile(int UserID)
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.User.GetFullProfile(UserID);
                response = new ResponseDataDTO
                {
                    ResponseObject = result,
                    ResponseCode = 200,
                    RecordCount = 1,
                    RespMessage = "Success"
                };
                _logger.LogInfo($"Returned Full User Profile For SearchString: {UserID} from database.");
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
                _logger.LogError($"Some error in the Users:GetFullProfile method: {ex}");
                return BadRequest(response);
            }
        }

        // POST api/<controller>
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody]UserViewModel user)
        {
            ResponseDataDTO response;
            try
            {
                //var userToCreate = await _repoWrapper.User.Create(user);

                if (user == null)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = null,
                        ResponseCode = 500,
                        RecordCount = 0,
                        RespMessage = "Internal Server Error. User Object Is Mull"
                    };
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                if (!ModelState.IsValid)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = null,
                        ResponseCode = 500,
                        RecordCount = 0,
                        RespMessage = "Internal Server Error. Invalid Model Object"
                    };
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var checkUser = await _repoWrapper.User.CheckUserExists(user.Email);
                if (checkUser)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseCode = 400,
                        ResponseObject = null,
                        RecordCount = 0,
                        RespMessage = "Sorry, This Email Address Is Already Associated With Another Account"
                    };
                    _logger.LogInfo($"Username Already Exists: {user.Email}");
                    return BadRequest(response);
                }
                else
                {
                    var newuser = await _repoWrapper.User.Create(user);

                    response = new ResponseDataDTO
                    {
                        ResponseObject = newuser,
                        ResponseCode = 200,
                        RecordCount = 1,
                        RespMessage = "Success"
                    };
                    _logger.LogInfo($"Successfully Created user with Username: {newuser.Email}");
                    return Ok(response);                  
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside The Post action: {ex.Message}");
                response = new ResponseDataDTO
                {
                    ResponseCode = 400,
                    ResponseObject = null,
                    RecordCount = 0,
                    RespMessage = ex.Message
                };
                return BadRequest(response);
            }
        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]dynamic user)
        {
            try
            {
                
                if (string.IsNullOrEmpty(user))
                {
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid model object");
                }

                dynamic dbOwner = await _repoWrapper.User.GetById(id);
                if (string.IsNullOrEmpty(dbOwner))
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repoWrapper.User.Update(dbOwner);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Update action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
