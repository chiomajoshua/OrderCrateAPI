using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderCrateAPI.Contracts;
using OrderCrateAPI.Models.DTOs;
using OrderCrateAPI.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderCrateAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private ILoggerManager _logger;

        public OrderController(ILoggerManager logger, IRepositoryWrapper repoWrapper)
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
                var result = await _repoWrapper.Order.GetAll();
                response = new ResponseDataDTO
                {
                    ResponseObject = result,
                    ResponseCode = 200,
                    RecordCount = result.Count(),
                    RespMessage = "Success"
                };
                _logger.LogInfo($"Returned all Orders from database.");
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

                _logger.LogError($"Some error in the Order:GetAll method: {ex}");
                return BadRequest(response);
            }
        }

        [HttpGet("GetOrderByID/{OrderID}", Name = "OrderById")]
        public async Task<IActionResult> GetOrderByID(int OrderID)
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.Order.GetByID(OrderID);

                if (result == null)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = result,
                        ResponseCode = 404,
                        RecordCount = 0,
                        RespMessage = "User Not Found"
                    };
                    _logger.LogError($"Order with id: {OrderID}, hasn't been found in db.");
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
                    _logger.LogInfo($"Returned Order with id: {OrderID}");
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
                _logger.LogError($"Something went wrong inside GetOrderByID action: {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpGet("GetOrderByBusinessID/{BusinessID}", Name = "OrderByBusinessId")]
        public async Task<IActionResult> GetOrdersByBusinessID(int BusinessID)
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.Order.GetBusinessOrdersByID(BusinessID);

                if (result == null)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = result,
                        ResponseCode = 404,
                        RecordCount = 0,
                        RespMessage = "Order Not Found"
                    };
                    _logger.LogError($"Order with id: {BusinessID}, hasn't been found in db.");
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
                    _logger.LogInfo($"Returned Order with id: {BusinessID}");
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
                _logger.LogError($"Something went wrong inside GetOrderByBusinessID action: {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpGet("GetOrderByInvoiceNumber/{InvoiceNumber}/{BusinessID}", Name = "OrderByInvoiceNumber")]
        public async Task<IActionResult> GetOrderByInvoiceNumber(string InvoiceNumber, int BusinessID)
        {
            ResponseDataDTO response;
            try
            {
                var result = await _repoWrapper.Order.GetOrderByInvoiceNumber(InvoiceNumber, BusinessID);

                if (result == null)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = result,
                        ResponseCode = 404,
                        RecordCount = 0,
                        RespMessage = "Order Not Found"
                    };
                    _logger.LogError($"Order with Invoice Number: {result.InvoiceNumber}, hasn't been found in db.");
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
                    _logger.LogInfo($"Returned Order with Invoice Number: {result.InvoiceNumber}");
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
                _logger.LogError($"Something went wrong inside GetOrderByInvoiceNumber action: {ex.Message}");
                return BadRequest(response);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]OrderViewModel order, int BusinessID)
        {
            ResponseDataDTO response;
            try
            {
                if (order == null)
                {
                    response = new ResponseDataDTO
                    {
                        ResponseObject = null,
                        ResponseCode = 500,
                        RecordCount = 0,
                        RespMessage = "Internal Server Error. Order Object Is Mull"
                    };

                    _logger.LogError("Order object sent from client is null.");
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

                    _logger.LogError("Invalid order object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var newOrder = await _repoWrapper.Order.Create(order, BusinessID);

                response = new ResponseDataDTO
                {
                    ResponseObject = newOrder,
                    ResponseCode = 200,
                    RecordCount = 1,
                    RespMessage = "Success"
                };

                _logger.LogInfo($"Successfully Created Order with Invoice Number: {newOrder.InvoiceNumber}");
                return Ok(response);
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

       
    }
}
