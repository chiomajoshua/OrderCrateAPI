using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCrateAPI.Models.DTOs
{
   public class ResponseDataDTO
    {
        public int ResponseCode { get; set; }
        public int RecordCount { get; set; }
        public dynamic ResponseObject { get; set; }
        public string RespMessage { get; set; }
    }
}
