using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCrateAPI.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AdLoginViewModel
    {
        public string Username { get; set; }
        public int UserID { get; set; }
        public bool Status { get; set; }
    }
}
