using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderCrateAPI.Entities
{
    public partial class User
    {
        public User()
        {
            Business = new HashSet<Business>();
        }

        public int ID { get; set; }
        [Required]
        [StringLength(30)]
        public string Lastname { get; set; }
        [Required]
        [StringLength(30)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(8)]
        public string Gender { get; set; }
        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date_Joined { get; set; }

        [InverseProperty("User")]
        public virtual Login Login { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Business> Business { get; set; }
    }
}