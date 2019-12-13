using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCrateAPI.Models.ViewModels
{
   public class OrderViewModel
    {
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public string OrderPlatform { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public double? Discount { get; set; }
        public int? CustomerID { get; set; }
        public int BusinessID { get; set; }
    }
}
