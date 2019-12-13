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
    [Route("api/business")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private ILoggerManager _logger;

        public BusinessController(ILoggerManager logger, IRepositoryWrapper repoWrapper)
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
                _logger.LogInfo($"Returned all Businesses from database.");
                var result = await _repoWrapper.Business.GetAll();
                response = new ResponseDataDTO
                {
                    ResponseObject = result,
                    ResponseCode = 200,
                    RecordCount = result.Count(),
                    RespMessage = "Success"
                };
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
                _logger.LogError($"Some error in the Business:GetAll method: {ex}");
                return BadRequest(response);
            }
        }

        [HttpGet("GetBusinessByID/{BusinessID}", Name = "BusinessById")]
        public async Task<IActionResult> GetBusinessByID(int BusinessID)
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.Business.GetByID(BusinessID);

                if (result == null)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = result,
                        ResponseCode = 404,
                        RecordCount = 0,
                        RespMessage = "Business Not Found"
                    };
                    _logger.LogError($"Business with id: {BusinessID}, hasn't been found in db.");
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
                    _logger.LogInfo($"Returned Business with id: {BusinessID}");
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
                _logger.LogError($"Something went wrong inside GetBusinessByID action: {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpGet("GetUserBusinessByID/{UserID}", Name = "UserBusinessById")]
        public async Task<IActionResult> GetUserBusinessByID(int UserID)
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.Business.GetUserBusinessByID(UserID);

                if (result == null)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = result,
                        ResponseCode = 404,
                        RecordCount = 0,
                        RespMessage = "Business Not Found"
                    };
                    _logger.LogError($"Business with User ID: {UserID}, hasn't been found in db.");
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
                    _logger.LogInfo($"Returned Business with User ID: {UserID}");
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
                _logger.LogError($"Something went wrong inside GetBusinessesByID action: {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpPost("CreateBusiness")]
        public async Task <IActionResult> CreateBusiness([FromBody]BusinessViewModel business, int UserID)
        {
            ResponseDataDTO response;
            try
            {
                if (business == null)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = null,
                        ResponseCode = 500,
                        RecordCount = 0,
                        RespMessage = "Internal Server Error. Business Object Is Mull"
                    };
                    _logger.LogError("Business object sent from client is null.");
                    return BadRequest(response);
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
                    _logger.LogError("Invalid business object sent from client.");
                    return BadRequest(response);
                }

                var checkBusinessName = await _repoWrapper.Business.CheckBusinessExistsName(business.Name);
                if(checkBusinessName)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseCode = 400,
                        ResponseObject = null,
                        RecordCount = 0,
                        RespMessage = "Sorry, This Business Name Is Already Registered On Order Crate. Please Go To Forgot Password To Recover Your Account"
                    };
                    _logger.LogInfo($"Business Name Already Exists: {business.Name}");
                    return BadRequest(response);
                }
                else
                {
                    var checkBusinessPhone = await _repoWrapper.Business.CheckBusinessExistsPhone(business.Phone);
                    if(checkBusinessPhone)
                    {
                        response = new ResponseDataDTO
                        {
                            ResponseCode = 400,
                            ResponseObject = null,
                            RecordCount = 0,
                            RespMessage = "Sorry, This Phone Number Is Already Linked With An Existing Business"
                        };
                        _logger.LogInfo($"Phone Number Already Exists: {business.Phone}");
                        return BadRequest(response);
                    }
                    else
                    {
                        var result = await _repoWrapper.Business.Create(business, UserID);
                        response = new ResponseDataDTO
                        {
                            ResponseObject = result,
                            ResponseCode = 200,
                            RecordCount = 1,
                            RespMessage = "Success"
                        };
                        _logger.LogInfo($"Created New Business from database.");
                        return Ok(response);
                    }
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
                _logger.LogError($"Some error in the Business:CreateNewBusiness method: {ex}");
                return BadRequest(response);
            }
        }
    }
}