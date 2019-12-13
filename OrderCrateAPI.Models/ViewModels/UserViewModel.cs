using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCrateAPI.Models.ViewModels
{
   public class UserViewModel
   {
        public int ID { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public DateTime? Date_Joined { get; set; }
   }
}
