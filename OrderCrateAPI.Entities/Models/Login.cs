using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderCrateAPI.Entities
{
    public partial class Login
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        public byte[] PinHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int UserID { get; set; }
        public bool Status { get; set; }

        [ForeignKey("UserID")]
        [InverseProperty("Login")]
        public virtual User User { get; set; }
    }
}