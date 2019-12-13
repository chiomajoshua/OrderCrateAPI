using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCrateAPI.Models.ViewModels
{
    public class CustomerViewModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Platform { get; set; }
    }
}
